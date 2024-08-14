using System;
using UnityEngine;

public class U_InputController : MonoBehaviour
{
    public delegate void Vector2Handler(Vector2 message);
    public event Vector2Handler OnMoved;

    public delegate void ActionHandler(string message);
    public event ActionHandler OnJumped;
    public event ActionHandler OnRolled;
    public event ActionHandler OnCrouched;

    public event ActionHandler OnBaseAttacked;
    public event ActionHandler AdditionalAttack;
    public event ActionHandler OnFirstAbility;
    public event ActionHandler OnSecondAbility;
    public event ActionHandler OnUltimateAbility;

    private PlayerControl _control;
    private Vector2 _directionMoved;


    private void Start()
    {
        InitControl();
        InitMovement();
        InitCombat();
    }
    private void FixedUpdate()
    {
        MovementInvoke();
    }
    private void InitControl()
    {
        _control = new PlayerControl();
        _control.Enable();
    }

    private void InitMovement()
    {
        _control.Movement.Jump.performed += context => OnJumped?.Invoke("Jump");
        _control.Movement.Roll.performed += context => OnRolled?.Invoke("Roll");
        _control.Movement.Crouch.performed += context => OnCrouched?.Invoke("Crouch");
    }

    private void InitCombat()
    {
        _control.Combat.BaseAttack.performed += context => OnBaseAttacked?.Invoke("BaseAttack");
        _control.Combat.AdditionallyAttack.performed += ContextBoundObject => AdditionalAttack?.Invoke("OnRMBAbility");
        _control.Combat.FirstAbility.performed += context => OnFirstAbility?.Invoke("FirstAbility");
        _control.Combat.SecondAbility.performed += context => OnSecondAbility?.Invoke("SecondAbility");
        _control.Combat.UltimateAbility.performed += context => OnUltimateAbility?.Invoke("UltimateAbility");
    }

    private void MovementInvoke()
    {
        _directionMoved = _control.Movement.Direction.ReadValue<Vector2>();
        OnMoved?.Invoke(_directionMoved);
    }
}
