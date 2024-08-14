using Cinemachine;
using System;
using UnityEngine;
using Zenject;


/// <summary>
/// ��������� ������� ���������, ������� - ��������� ��.
/// ������ �� ���������� �������� � ������ ����� �������. ����� �������� ������ �� �����.
/// �� ���� ��������, ����� ������ ���� ������ ���-�� ���������
/// </summary>
[RequireComponent(typeof(U_Unit))]
[RequireComponent(typeof(Rigidbody))] //������������� ������� ������
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
    [SerializeField] private const float _drag = 0.35f; //����� - ���������. 

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
        var cameraComponent = _cinCamera.GetCinemachineComponent(CinemachineCore.Stage.Body); //���������� ����������� ��� ��������� ����������
        zoom = (cameraComponent as CinemachineFramingTransposer); //���������� ����������� ��� ��������� ���������� � ������ ���������

        InitControl();
        InitPlayer();

        _cameraHelper = new CinemachineCameraHelper(_cinCamera);
        _groundRay.direction = Vector3.down;
    }

    /// <summary>
    /// ��� ������������� ������ � �������� ������� � �������
    /// � ����� ����� ������� ��� ����� ��������� ������ �� ������ ��� ��������� ������
    /// </summary>
    private void InitControl()
    {
        _control = new PlayerControl();
        _control.Enable();

        behaviour.OnCrouched += context => ChangeMovementState(MovementState.Courch);
        behaviour.OnJumped += context => Jump(); //�������� ��� ���

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
    /// ��� �������� ����� ������
    /// ������ Y � Z ������� ������ ��� ��������� �� ���� ���� (����� ������ ��������������)
    /// ����� ������ ���������� ������ 2, ��� W S: 1, -1 � X ����� �������, � A D: 1, -1 � Y ����� �������
    /// ����� ������� �������� ��������������� ������ ����������� ������
    /// </summary>
    private void PlayerInput()
    {
        _direction = _control.Movement.Direction.ReadValue<Vector2>();
        _direction.z = _direction.y;
        _direction.y = 0;

        _direction = Quaternion.AngleAxis(_cameraTransform.rotation.eulerAngles.y, Vector3.up) * _direction;
    }
    /// <summary>
    /// ���� ������������. �������� ����� ������, ���� �������� ����-�� ������������ �� velocity
    /// �������� ����� ����� ��������, ��������� ����������, ��� Y ����������� = 0
    /// </summary>
    private void Move()
    {
        if(OnGrounded())
        {
            AddClampedForce(_direction * 15); // �������� ��� ������
             //_rigidbody.AddForce(_direction * _entity.Stats[(int)StatType.MoveSpeedMpl].Current);
        }
        else
        {
            AddClampedForce(_direction * 15 * _drag); // �������� ��� ������
            //_rigidbody.AddForce(_direction * _entity.Stats[(int)StatType.MoveSpeedMpl].Current * _drag);
        }

        //AddClampedForce();
    }

    /// <summary>
    /// ��� ����������� �������� �������.
    /// �������� ������ � ��� ��������. 
    /// ����� ��������� ������� - �������� ��������� (�� ������ ���������, � ��� �������� ������ ���)
    /// , �� ��������� �������� � �������� �������� ����� ������� �������.
    /// </summary>
    private void AddClampedForce(Vector3 direction)
    {
        float speed = 6f;
        //float speed = _entity.Stats[(int)StatType.MoveSpeedMpl].Current;
        float delta = _rigidbody.velocity.magnitude > speed ? 0 : 1;
        
        if (_rigidbody.velocity.magnitude + direction.magnitude > speed)
            delta = (speed - _rigidbody.velocity.magnitude);

        delta = Mathf.Clamp01(delta);// ���� ���-�� ������ �� ���, �� ��� ���������
        _rigidbody.AddForce(delta * direction);
    }

    /// <summary>
    /// ��� �������� ����������� ����.
    /// ���������� ���������� ���� � ������������ ������� OnMoved ��� ��������
    /// </summary>
    private float CheckTraveledDistance()
    {
        _movable = _rigidbody.velocity.sqrMagnitude > 0.1f ? true : false; //������� velocity � ���������� �����
                                                                           //true - ������ � ��������, false - ���
                                                                           //0.1f ��� ��� ��������� ������������� ���� ��� 0.00001e-1000
        if (_movable)
        {
            _traveledDistance += _rigidbody.velocity.magnitude * Time.fixedDeltaTime; // ��������� - ����� ������� 
            OnMoved?.Invoke(_traveledDistance);                              // �������������� �� �������� >0 ���� � ��������
            return _traveledDistance;
        }
        else
           return _traveledDistance = 0;
    }

    /// <summary>
    /// ��� �������� ��������� � �� �������, ���� ������� ������.
    /// ����� �������� �������� � ������ ��� ������ �� ����� ��������
    /// </summary>
    private void RotateCharacter()
    {
        transform.rotation = Quaternion.AngleAxis(_cameraTransform.rotation.eulerAngles.y, Vector3.up);
    }

    /// <summary>
    /// ������ ��������������. ��������� ������ ������ ������ ��� ������
    /// </summary>
    private void Jump()
    {
        if(OnGrounded() && _currentMovementState != MovementState.Courch)
        {
            _rigidbody.AddForce(Vector3.up * 7.5f, ForceMode.Impulse);// �������� ��� ������
            //_rigidbody.AddForce(Vector3.up * _entity.Stats[(int)StatType.JumpForceMpl].Current, ForceMode.Impulse);
            _cameraHelper.AddNoiseAsync(3, 0.5f);
        }
    }

    /// <summary>
    /// ��� ����, ������� ��������� ���, ������ ������ ������ �� ���������� ��������� ���
    /// ����� �� 240 - ������ ��� �� ����� ���������� 240 � -240 ��� ��������� ����� � ����
    /// ����� ������������ �� ������ ��������
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
