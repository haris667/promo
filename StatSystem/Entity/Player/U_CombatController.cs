using Game;
using Newtonsoft.Json;
using System.IO;
using System.Threading;
using UnityEngine;
using Zenject;

[JsonObject(MemberSerialization.OptIn)]
public class U_CombatController : MonoBehaviour
{
    //заполн€ютс€ из конфига json
    [JsonProperty]
    public Ability _additionalAttack;
    [JsonProperty]
    public Ability _firstAbility;
    [JsonProperty]
    public Ability _secondAbility;
    [JsonProperty]
    public Ability _ultimateAbility;

    [JsonIgnore]
    private IEntity target;
    [JsonIgnore]
    private U_BehaviourSystem _behaviourSystem;
    [JsonIgnore]
    private U_BattleCalculator _battleCalculator;
    [JsonIgnore]
    private U_Unit _sourceUnit;

    //CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
    //private CancellationToken _allLockToken;

    [JsonIgnore]
    [Inject] private PoolController poolController;
    [JsonIgnore]
    [Inject] private U_AimTargetSystem aimTarget;

    private void Awake()
    {
        

        _behaviourSystem = GetComponent<U_BehaviourSystem>();
        _battleCalculator = GetComponent<U_BattleCalculator>();

        //_behaviourSystem.OnTokenUpdated += context => SubscribeAdditionalAttack();
        //_behaviourSystem.OnTokenPreUpdated += context => UnSubscribeAdditionalAttack();

        _sourceUnit = GetComponent<U_Unit>();
        aimTarget.onHitTarget += context => target = context.GetComponent<U_Unit>();
        SubscribeAdditionalAttack();
        //InitAbilities();

        //string str = JsonConvert.SerializeObject(this);
        //File.WriteAllText(Application.persistentDataPath + "/U_CombatPlayer.json", str);
    }

    void SubscribeAdditionalAttack()
    {
        _behaviourSystem.OnAdditionalAttack += context => 
            _additionalAttack.Cast(_sourceUnit, target);
    }
    void UnSubscribeAdditionalAttack()
    {
        //_behaviourSystem.OnAdditionalAttack -= context =>
          //  _additionalAttack.Cast( _sourceUnit, target);
    }

/*    private void InitAbilities()
    {
        ThrowingKnives kn = new ThrowingKnives(0, " инуть ножи", " идаем ножи куда смотрим", 1, transform, poolController.GetPool(PoolType.Knives));
        _additionalAttack = kn;

    }*/
}
