using Cinemachine;
using System;
using UnityEngine;
using Zenject;


/// <summary>
/// Позволяет объекту двигаться, прыгать - управлять им.
/// Кнопки на управление задаются в окошке инпут системы. Можно поменять кнопки на любые.
/// По сути заглушка, будет хорошо если отсюда что-то возьмется
/// </summary>
[RequireComponent(typeof(U_Unit))]
[RequireComponent(typeof(Rigidbody))] //обязательство наличия ригида
public class U_PlayerMovementSystem : MonoBehaviour
{
    public event Action<float> OnMoved;

    public U_BehaviourSystem behaviour;
    private bool _movable = false;
    private float _traveledDistance = 0;

    private MovementState _currentMovementState = MovementState.Run;
    private MovementState _previousMovementState = MovementState.Run;

    [SerializeField] Transform _cameraTransform;
    [SerializeField] private CinemachineVirtualCamera _cinCamera;
    [SerializeField] private const float _drag = 0.35f; //Важно - множитель. 

    private IEntity _entity;
    private Rigidbody _rigidbody;
    private Vector3 _direction;
    private PlayerControl _control;
    private CinemachineFramingTransposer zoom;

    private CinemachineCameraHelper _cameraHelper;
    private Ray _groundRay;

    void Awake()
    {
        behaviour = GetComponent<U_BehaviourSystem>();
        _rigidbody = GetComponent<Rigidbody>();
        var cameraComponent = _cinCamera.GetCinemachineComponent(CinemachineCore.Stage.Body); //хитроумная конструкция для получения компонента
        zoom = (cameraComponent as CinemachineFramingTransposer); //хитроумная конструкция для получения компонента с сменой дистанции

        InitControl();
        InitPlayer();

        _cameraHelper = new CinemachineCameraHelper(_cinCamera);
        _groundRay.direction = Vector3.down;
    }

    /// <summary>
    /// Для инициализации камеры и подписки методов к кнопкам
    /// В новой инпут системе нет нужды проверять нажата ли кнопка для отработки метода
    /// </summary>
    private void InitControl()
    {
        _control = new PlayerControl();
        _control.Enable();

        behaviour.OnCrouched += context => ChangeMovementState(MovementState.Courch);
        behaviour.OnJumped += context => Jump(); //работает это так

        behaviour.OnMoved += context =>
        {
            PlayerInput();
            Move();
        };
    }

    private void InitPlayer()
    {
        _entity = GetComponent<U_Player>();
    }

    void FixedUpdate()
    {

        RotateCharacter();

        CheckTraveledDistance();
        Zoom();

        _groundRay.origin = transform.position;
    }

    /// <summary>
    /// Для удобного ввода игрока
    /// Меняем Y и Z местами потому что двигаемся по этим осям (кроме прыжка соответственно)
    /// Инпут игрока возвращает вектор 2, где W S: 1, -1 в X этого вектора, а A D: 1, -1 в Y этого вектора
    /// Таким образом получаем нормализованный вектор направления игрока
    /// </summary>
    private void PlayerInput()
    {
        _direction = _control.Movement.Direction.ReadValue<Vector2>();
        _direction.z = _direction.y;
        _direction.y = 0;

        _direction = Quaternion.AngleAxis(_cameraTransform.rotation.eulerAngles.y, Vector3.up) * _direction;
    }
    /// <summary>
    /// Само передвижение. Работает через адфорс, дабы избежать чего-то неожиданного от velocity
    /// например задав через велосити, сломается гравитация, ибо Y направления = 0
    /// </summary>
    private void Move()
    {
        if(OnGrounded())
        {
            AddClampedForce(_direction * 15); // заглушка для тестов
             //_rigidbody.AddForce(_direction * _entity.Stats[(int)StatType.MoveSpeedMpl].Current);
        }
        else
        {
            AddClampedForce(_direction * 15 * _drag); // заглушка для тестов
            //_rigidbody.AddForce(_direction * _entity.Stats[(int)StatType.MoveSpeedMpl].Current * _drag);
        }

        //AddClampedForce();
    }

    /// <summary>
    /// Для ограничения скорости объекта.
    /// Работает хорошо и без дерганий. 
    /// Убрав последнее условие - точность повысится (не сильно повысится, а вот дергания хорошо так)
    /// , но добавятся дергания и значения скорости будут немного скакать.
    /// </summary>
    private void AddClampedForce(Vector3 direction)
    {
        float speed = 6f;
        //float speed = _entity.Stats[(int)StatType.MoveSpeedMpl].Current;
        float delta = _rigidbody.velocity.magnitude > speed ? 0 : 1;
        
        if (_rigidbody.velocity.magnitude + direction.magnitude > speed)
            delta = (speed - _rigidbody.velocity.magnitude);

        delta = Mathf.Clamp01(delta);// если что-то пойдет не так, то это страховка
        _rigidbody.AddForce(delta * direction);
    }

    /// <summary>
    /// Для подсчета пройденного пути.
    /// Возвращает пройденный путь и отрабатывает событие OnMoved при движении
    /// </summary>
    private float CheckTraveledDistance()
    {
        _movable = _rigidbody.velocity.sqrMagnitude > 0.1f ? true : false; //смотрим velocity и возвращаем булку
                                                                           //true - объект в движении, false - нет
                                                                           //0.1f ибо при поворотах накладывается сила аля 0.00001e-1000
        if (_movable)
        {
            _traveledDistance += _rigidbody.velocity.magnitude * Time.fixedDeltaTime; // магнитуда - длина вектора 
            OnMoved?.Invoke(_traveledDistance);                              // соответственно от велосити >0 если в движении
            return _traveledDistance;
        }
        else
           return _traveledDistance = 0;
    }

    /// <summary>
    /// Для поворота персонажа в ту сторону, куда смотрит камера.
    /// Можно добавить проверку и делать это только во время движения
    /// </summary>
    private void RotateCharacter()
    {
        transform.rotation = Quaternion.AngleAxis(_cameraTransform.rotation.eulerAngles.y, Vector3.up);
    }

    /// <summary>
    /// Прыжок соответственно. Добавляем сверху тряску камеры при прыжке
    /// </summary>
    private void Jump()
    {
        if(OnGrounded() && _currentMovementState != MovementState.Courch)
        {
            _rigidbody.AddForce(Vector3.up * 7.5f, ForceMode.Impulse);// заглушка для тестов
            //_rigidbody.AddForce(Vector3.up * _entity.Stats[(int)StatType.JumpForceMpl].Current, ForceMode.Impulse);
            _cameraHelper.AddNoiseAsync(3, 0.5f);
        }
    }

    /// <summary>
    /// Для зума, смотрим изменение скм, дальше меняем камеру на расстояние изменения скм
    /// делим на 240 - потому что на винде возвращает 240 и -240 при изменении вверх и вниз
    /// Далее ограничиваем на нужные величины
    /// </summary>
    private void Zoom()
    {
        float value = _control.Zoom.Scroll.ReadValue<float>();
        zoom.m_CameraDistance += value / 240; 
        zoom.m_CameraDistance = Mathf.Clamp(zoom.m_CameraDistance, 1, 10);
    }

    private bool OnGrounded()
    {
        if(Physics.Raycast(_groundRay, out RaycastHit hit, 2f))
        {
            if (hit.collider.CompareTag("Static"))
                return true;
            else
                return false;
        }

        return false;
    }
    private void ChangeMovementState(MovementState state)
    {
        _currentMovementState = state == _currentMovementState ? _previousMovementState : state;
    }
}

public enum MovementState
{
    Combat,
    Courch,
    Walk,
    Run
}
