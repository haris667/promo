using UnityEngine;
using UnityEngine.Pool;
using System;

/// <summary>
/// ��������� ��� ��������� ������� ����� ��������� ����.
/// �������� - �����������, ������, ���������� ���
/// </summary>
public interface IPooled
{
    /// <summary>
    /// ��� � ������� ������ ���������. ��� ������ �� ���� ��� ������� �����-����
    /// </summary>
    IObjectPool<Transform> abilityPool { get; set; }

    /// <summary>
    /// �������������� ����� n �������
    /// </summary>
    float timeToRelease { get; set; } 
}