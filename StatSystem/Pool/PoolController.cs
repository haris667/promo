using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// ��� ��������� ������� ����.
/// �������� ��� ����� �����, �� ������� ����� �������� ������, �������� � ������ ��� ����������.
/// ������ ���� ����� ���� ����� ���� �������, �������� ��� ���� ����, ��� ����� ������� ����������� ������ ����� ����
/// ����������.
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