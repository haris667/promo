using Game;
using Newtonsoft.Json;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

[JsonObject(MemberSerialization.OptIn)]
public class Ability : MonoBehaviour, IAbility
{
    [JsonProperty]
    public int ID { get; }
    [JsonProperty]
    public string Name { get; }
    [JsonProperty]
    public string Description { get; }
    [JsonProperty]
    public float CastTime { get; }
    [JsonProperty]
    public bool LockedCast { get; set; }

    //по идее у всех способок есть носитель. 
    //Добавил, чтобы удобно заполнять из конфига и все производные так же заполнялись
    [JsonIgnore]
    public Transform source;

    [JsonIgnore]
    public Pool projectilePool;
    public AbilityType Type { get; protected set; }

    public Ability(int ID, string name, string description, float castTime)
    {
        this.ID = ID;
        Name = name;
        Description = description;
        CastTime = castTime; 
    }

    public void Cast(IEntity source, IEntity target = null, Vector3? point = null) 
        => StartCoroutine(CastWithType(source, target, point));

    /// <summary>
    /// Для отмены активной корутины. Не пугаться с StopAllCoroutines() - стоп происходит только в нынешнем монобехе
    /// </summary>
    public void CancelCast() => StopAllCoroutines();

    protected virtual IEnumerator CastWithType(IEntity source, IEntity target = null, Vector3? point = null)
    {
        yield return new WaitForSeconds(CastTime);
        if (Type == AbilityType.Target)
            CastTarget(source, target);
        else
            CastPoint(source, point);
    }

    protected virtual void CastTarget(IEntity source, IEntity target) { }
    protected virtual void CastPoint(IEntity source, Vector3? point) { }
}

public enum AbilityType
{
    Target,
    Point
}
