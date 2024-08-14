using Game;
using Newtonsoft.Json;
using System;

using UnityEngine;
using static Zenject.SignalSubscription;


/// <summary>
/// Кидаем 5 кинжалов перед собой, нанося урон и замедление в процентах
/// </summary>
[JsonObject(MemberSerialization.OptIn)]
public class ThrowingKnives : Ability
{
    [JsonProperty]
    private DmgPack _damagePack;
    [JsonProperty]
    private StatChangePack _changePack;

    [JsonProperty]
    [SerializeField]
    private float changeMovementPercent = 20;

    [JsonProperty]
    [SerializeField]
    private float _durationStatChange = 3f;

    [JsonProperty]
    private float statChange;

    [JsonIgnore]
    private Vector3 _projectileStartPosition
    { 
        get => source.position + Vector3.up + source.rotation * Vector3.forward;
    }
    
    public ThrowingKnives(int ID, string name, string description, float castTime ,Transform source, Pool pool) : base(ID, name, description, castTime)
    {
        Type = AbilityType.Point;
        this.source = source;

        _damagePack = new DmgPack(DmgType.Physic, 25f, null);

        this.projectilePool = pool;
    }
    /// <summary>
    /// Для каста абилки. Переопределяем для новой логики способности, получаем из пула и стреляем кинжалами.
    /// </summary>
    protected override void CastPoint(IEntity source, Vector3? point)
    {
        float startAngle = -20f;
        for(int i = 0; i < 5; i++)
        {
            Transform proj = projectilePool.GetItem();
            proj.position = _projectileStartPosition;
            proj.GetComponent<U_Projectile>().onReleaseHitted += AddDamage; 
            StartMoveProjectile(proj, startAngle + (i * 10f));
        }
    }

    /// <summary>
    /// Для подсчета направления будущих зарядов.
    /// </summary>
    private void StartMoveProjectile(Transform proj, float shiftForwardAngle)
    {
        Vector3 direction = source.rotation.eulerAngles;
        direction.y += shiftForwardAngle;
        direction.x += Camera.main.transform.rotation.eulerAngles.x;
        Vector3 localForward = Quaternion.Euler(direction) * Vector3.forward * 10;

        proj.GetComponent<U_ProjectileMovement>().StartMove(localForward, 1f);
    }

    /// <summary>
    /// Для нанесения урона цели.
    /// Вызывается при касании проджектайла с Ability, далее урон считается тут
    /// </summary>
    private void AddDamage(U_Projectile proj, Transform target)
    {
        U_Unit unit = target.GetComponent<U_Unit>();
        unit.AddDamage(_damagePack);

        statChange = unit.Stats[(int)StatType.MoveSpeedMpl].Current / 100 * changeMovementPercent;
        _changePack = new StatChangePack(StatType.MoveSpeedMpl, -statChange, _durationStatChange);

        unit.AddChange(_changePack);

        proj.onReleaseHitted -= AddDamage;
    }  
}