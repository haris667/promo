using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

/// <summary>
/// ��� ��������. ��� ����� ������ ������� ���������� ����������� �� �����
/// </summary>
public class Pool : MonoBehaviour 
{
    public GameObject pooledItem;
    public int amountPooledItems = 20;
    public float timeToRelease = 3;

    public PoolType type;
    private IObjectPool<Transform> pool;
    private List<Transform> supPool; //��� ������ �� �����. � ����������� ���� ����� ������� ������,
                                     //������-��� ��� ������� �� cpp � ��� �������������
    private Transform lastPooledItem;//����� �� ��������� ����� ������ ������ ���

    private void Awake() => Init();


    /// <summary>
    /// ��� ������������ ������ ������� �� �������.
    /// ������ - ��� ������� ����� ������� �������, ��������� ��� �������
    /// </summary>
    private void Init()
    {
        pool = new ObjectPool<Transform>(CreatePooledItem, //����� ����� �������, ������� ��������� ��� �������
                                         OnTakeFromPool,
                                         OnReturnedToPool,
                                         OnDestroyPoolObject,
                                         true, 10, amountPooledItems);
    }

    public Transform GetItem() => pool.Get();

    public Transform GetItem(string name)
    {
        lastPooledItem = supPool.First(go => go.name == name);
        lastPooledItem.gameObject.SetActive(true);
        return lastPooledItem;
    }

        /// <summary>
        /// ��� ������� ��� ����������� ������ � ��������� IPooled
        /// </summary>
    Transform CreatePooledItem()
    {
        GameObject go = Object.Instantiate(pooledItem);
        go.transform.parent = transform;

        IPooled item = go.GetComponent(typeof(IPooled)) as IPooled;

        item.timeToRelease = timeToRelease;
        item.abilityPool = pool;

        supPool.Add(go.transform);
        return go.transform;
    }

    void OnReturnedToPool(Transform system) => system.gameObject.SetActive(false);

    void OnTakeFromPool(Transform system) => system.gameObject.SetActive(true);

    void OnDestroyPoolObject(Transform system) => GameObject.Destroy(system.gameObject);
}

public enum PoolType
{
    Money,
    Knives,
    Fireball,
    Bullet,
    Abilities
}