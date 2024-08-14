using UnityEngine;
using System;
using UnityEngine.Pool;
using System.Collections;
/// <summary>
/// Какой либо заряд, работает в связке с передвижением заряда.
/// </summary>
public class U_Projectile : MonoBehaviour, IPooled
{
    /// <summary>
    /// Отрабатывает при попадании
    /// </summary>
    public Action<U_Projectile, Transform> onReleaseHitted = delegate { }; //отрабатывает при попадании
    /// <summary>
    /// Отрабатывает при непопадании
    /// </summary>
    public Action<U_Projectile> onRelease = delegate { }; 
                                                          //соответственно эффекты в конце у них могут быть разные
                                                          //нужно ток подписаться
    public bool pooled = true;

    public IObjectPool<Transform> abilityPool { get; set; } //пул тут нужен, чтобы при касании мы могли залезть обратно пул
    public float timeToRelease
    {
        get { return _timeToRelease; }
        set
        {
            _timeToRelease = value;
            _currentTimeToDestroy = value;
        }
    }

    private float _timeToRelease;
    private float _currentTimeToDestroy;

    /// <summary>
    /// ВАЖНО - на всех Entity (tag) объектах нужен U_Unit, иначе может словиться NRE
    /// </summary>
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("Entity"))
        {
            Release();
            onReleaseHitted?.Invoke(this, collision.transform); // отправляем ивент крича кто мы и в кого попали
        }
        else
            Release();
    }


    private void OnEnable() => pooled = false;

    private void OnDisable() => pooled = true;

    private void Update() => TimeToRelease();

    /// <summary>
    /// Метод вызывается при нужде положить себя в пул
    /// </summary>
    private void Release()
    {
        _currentTimeToDestroy = timeToRelease; //обновляем нужные нам параметры

        if(!pooled) //при быстром клике мог получить ошибку, это проверка на всякий случай
            abilityPool.Release(this.transform); //а вот и сам релиз
    }

    /// <summary>
    /// Насильный релиз через некоторое время, чтобы объекты всегда вернулись в пул
    /// </summary>
    private void TimeToRelease()
    {
        _currentTimeToDestroy -= Time.deltaTime;

        if (_currentTimeToDestroy <= 0)
        {
            onRelease?.Invoke(this);
            Release();
        }
            
    }
}
