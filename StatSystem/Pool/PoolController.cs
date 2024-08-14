using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// Для получение нужного пула.
/// Хранится вся кучка пулов, из которой можно выцепить нужный, например с ножами или фаерболами.
/// Причем типы пулов тоже могут быть разными, например для пула лута, где нужно цеплять ОПРЕДЛЕННЫЕ шмотки нужна своя
/// реализация.
/// </summary>
public class PoolController : MonoBehaviour
{
    public static PoolController instance;
    [SerializeField] private List<Pool> pools;

    public Pool GetPool(PoolType type) => pools.First(pool => pool.type == type);

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
}