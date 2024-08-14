using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;



/// <summary>
/// Для запуска обычных и оверрайд анимаций
/// </summary>
[RequireComponent(typeof(U_BehaviourSystem))]
public class U_PlayerAnimationSystem : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private U_BehaviourSystem _behaviour;

    [Header("Перезаписывает анимации, если не пуст")]
    [SerializeField] private AnimatorOverrideController _overridedAnimator;

    [SerializeField] private float _transitionSpeed = 5;
    private bool _courched = false;
    private PlayerControl _control;

    private void Awake()
    {
        _behaviour = GetComponent<U_BehaviourSystem>();
        InitControl();
        InitButtons();

        if (_overridedAnimator != null)
            ChangeAnimationController(_overridedAnimator);
    }

    /// <summary>
    /// Для смены !оверрайднутого! контроллера анимаций
    /// В таком аниматоре граф остается таким же, однако анимации можно сменить на другие в этом графе
    /// </summary>
    public void ChangeAnimationController(AnimatorOverrideController controller) =>
        _animator.runtimeAnimatorController = controller;

    /// <summary>
    /// Для смены конкретной анимации !оверрайднутого! контроллера анимаций
    /// В таком аниматоре граф остается таким же, однако анимации можно сменить на другие в этом графе
    /// ВАЖНО - в 1 параметре пишется не название узла графа, а название АНИМАЦИИ ВНУТРИ ЭТОГО ГРАФА
    /// </summary>
    public void ChangeAnimation(string changedAnimName, AnimationClip clip)
    {
        AnimatorOverrideController newAnimator = new AnimatorOverrideController(_animator.runtimeAnimatorController); 
        if(_overridedAnimator != null) 
            newAnimator.clips = _overridedAnimator.clips; 

        newAnimator[changedAnimName] = clip; 
        ChangeAnimationController(newAnimator); 
    }

    private void InitControl()
    {
        _control = new PlayerControl();
        _control.Enable();
    }

    /// <summary>
    /// Для подписки анимаций на кнопки
    /// </summary>
    private void InitButtons()
    {
        _behaviour.OnBaseAttacked += context => _animator.SetTrigger("Attack");

        _behaviour.OnJumped += context => OnJumpAnimation();
        _behaviour.OnCrouched += context => 
        {
            _courched = !_courched;
            _animator.SetBool("SquatPosition", _courched);
        };
        _behaviour.OnMoved += context => UpdateMovementAnimation();
    }

    /// <summary>
    /// Анимируем движение игрока.
    /// Так же для плавности вводим интерполяцию, чтобы анимации перетекали друг в друга плавнее
    /// </summary>
    private void UpdateMovementAnimation()
    {
        float x = _control.Movement.Direction.ReadValue<Vector2>().x;
        x = Mathf.Lerp(_animator.GetFloat("DirectionX"), x, Time.deltaTime * _transitionSpeed);
        _animator.SetFloat("DirectionX", x);

        float y = _control.Movement.Direction.ReadValue<Vector2>().y;
        y = Mathf.Lerp(_animator.GetFloat("DirectionY"), y, Time.deltaTime * _transitionSpeed);
        _animator.SetFloat("DirectionY", y);
    }

    private void OnJumpAnimation() => _animator.SetTrigger("Jump");
}
