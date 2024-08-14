using UnityEngine;
using UnityEngine.Pool;
using System;

/// <summary>
/// Интерфейс для сущностей которые будут храниться пуле.
/// Например - прожектайлы, монеты, выпадающий лут
/// </summary>
public interface IPooled
{
    /// <summary>
    /// Пул в котором объект находится. Для релиза из пула при тригере каком-либо
    /// </summary>
    IObjectPool<Transform> abilityPool { get; set; }

    /// <summary>
    /// Самоупуливание через n времени
    /// </summary>
    float timeToRelease { get; set; } 
}