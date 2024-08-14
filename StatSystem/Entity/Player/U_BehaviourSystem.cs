using Game;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// Для блока действий, в зависимости от состояния
/// </summary>
public class U_BehaviourSystem : MonoBehaviour
{
    private U_BattleCalculator _battleCalculator;
    public U_InputController input;

    public event Action<CancellationTokenSource> OnTokenPreUpdated;
    public event Action<CancellationTokenSource> OnTokenUpdated;

    public delegate void ActionHandler(string message);
    public event ActionHandler OnMoved;
    public event ActionHandler OnJumped;
    public event ActionHandler OnRolled;
    public event ActionHandler OnCrouched;

    public CancellationTokenSource baseAttackToken = new CancellationTokenSource();
    public event ActionHandler OnBaseAttacked;

    public CancellationTokenSource additionalAttackToken = new CancellationTokenSource();
    public event ActionHandler OnAdditionalAttack;

    public CancellationTokenSource firstAbilityToken = new CancellationTokenSource();
    public event ActionHandler OnFirstAbility;

    public CancellationTokenSource secondtAbilityToken = new CancellationTokenSource();
    public event ActionHandler OnSecondAbility;

    public CancellationTokenSource ultimateAbilityToken = new CancellationTokenSource();
    public event ActionHandler OnUltimateAbility;


    private List<ActionHandler> _availableIsSilence = new List<ActionHandler>(); // доступные действия для состояния хранятся в листах
    private List<ActionHandler> _availableIsStunned = new List<ActionHandler>();

    private Dictionary<BehaviourType, List<ActionHandler>> _availableIsBehaviour = new Dictionary<BehaviourType, List<ActionHandler>>();

    private bool tokensUpdated = false;

    public void Init(U_InputController input, U_BattleCalculator battleCalculator)
    {
        _battleCalculator = battleCalculator;
        _battleCalculator.OnEffectEnded += context => CancelAbilities(context);
        this.input = input;
        InitAvailableIsSilence();
        //SubscribeActions();
    }

    private void Awake()
    {
        SubscribeActions();
    }

    /// <summary>
    /// Инициализируем события
    /// </summary>
    private void SubscribeActions()
    {
        input.OnJumped += context => InvokeAvailableAction(OnJumped);
        input.OnCrouched += context => InvokeAvailableAction(OnCrouched);
        input.OnMoved += context => InvokeAvailableAction(OnMoved);
        input.OnRolled += context => InvokeAvailableAction(OnRolled);

        input.OnBaseAttacked += context => InvokeAvailableAction(OnBaseAttacked);
        input.OnFirstAbility += context => InvokeAvailableAction(OnFirstAbility);
        input.OnSecondAbility += context => InvokeAvailableAction(OnSecondAbility);
        input.OnUltimateAbility += context => InvokeAvailableAction(OnUltimateAbility);
        input.AdditionalAttack += context => InvokeAvailableAction(OnAdditionalAttack);
    }

    private void CancelAbilities(EffectPack pack)
    {    
        switch (pack.Type)
        {
            case BehaviourType.IsStunned:
                CancelToken(baseAttackToken);
                CancelToken(additionalAttackToken);
                CancelToken(firstAbilityToken);
                CancelToken(secondtAbilityToken);
                CancelToken(ultimateAbilityToken);
                break;

            case BehaviourType.IsSilence:
                CancelToken(additionalAttackToken);
                CancelToken(firstAbilityToken);
                CancelToken(secondtAbilityToken);
                CancelToken(ultimateAbilityToken);
                break;

            case BehaviourType.IsDead:
                CancelToken(baseAttackToken);
                CancelToken(additionalAttackToken);
                CancelToken(firstAbilityToken);
                CancelToken(secondtAbilityToken);
                CancelToken(ultimateAbilityToken);
                break;

            default:
                return;
        }
    }
    private void CancelToken(CancellationTokenSource cancellationCource)
    {
        OnTokenPreUpdated?.Invoke(cancellationCource); //для отписки
        cancellationCource.Cancel();
        cancellationCource = new CancellationTokenSource();
        OnTokenUpdated?.Invoke(cancellationCource); //для подписки
    }

    /// <summary>
    /// Так добавляются доступные действия при состоянии
    /// ВАЖНО - если у события не будет подписчика он будет null
    /// поэтому это инитим в старте, а подписчиков набираем в евейке
    /// </summary>
    private void InitAvailableIsSilence()
    {
        _availableIsSilence.Add(OnMoved);
        _availableIsSilence.Add(OnJumped);
        _availableIsSilence.Add(OnRolled);
        _availableIsSilence.Add(OnCrouched);

       // _availableIsSilence.Add(OnBaseAttacked);
    }

    private void InitAvailableIsStunned() { }

    /// <summary>
/// Отправляет события инпута, если состояние позволяет сделать это 
/// </summary>
    private void InvokeAvailableAction(ActionHandler action)
    {
        if(CheckAction(action))
            action?.Invoke(action.ToString());
    }

    /// <summary>
    /// Для проверки возможности сделать действие 
    /// </summary>
    private bool CheckAction(ActionHandler action)
    {
        bool result = _battleCalculator.activeEffects.Count > 0 ? false : true;
        for (int i = 0; i < _battleCalculator.activeEffects.Count; i++)
        {
            switch (_battleCalculator.activeEffects[i].Type)
            {
                case BehaviourType.IsStunned:
                    result = ActionContains(action, _availableIsStunned);
                    break;
                case BehaviourType.IsSilence:
                    result = ActionContains(action, _availableIsSilence);
                    break;
                default:
                    result = true;
                    break;
            }
        }
        return result;
    }

    private bool ActionContains(ActionHandler action, List<ActionHandler> actions) =>
        actions.Contains(action);
}

