using UnityEngine;
using System;
using UnityEngine.Pool;
using System.Collections;
/// <summary>
/// ����� ���� �����, �������� � ������ � ������������� ������.
/// </summary>
public class U_Projectile : MonoBehaviour, IPooled
{
    /// <summary>
    /// ������������ ��� ���������
    /// </summary>
    public Action<U_Projectile, Transform> onReleaseHitted = delegate { }; //������������ ��� ���������
    /// <summary>
    /// ������������ ��� �����������
    /// </summary>
    public Action<U_Projectile> onRelease = delegate { }; 
                                                          //�������������� ������� � ����� � ��� ����� ���� ������
                                                          //����� ��� �����������
    public bool pooled = true;

    public IObjectPool<Transform> abilityPool { get; set; } //��� ��� �����, ����� ��� ������� �� ����� ������� ������� ���
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
    /// ����� - �� ���� Entity (tag) �������� ����� U_Unit, ����� ����� ��������� NRE
    /// </summary>
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("Entity"))
        {
            Release();
            onReleaseHitted?.Invoke(this, collision.transform); // ���������� ����� ����� ��� �� � � ���� ������
        }
        else
            Release();
    }


    private void OnEnable() => pooled = false;

    private void OnDisable() => pooled = true;

    private void Update() => TimeToRelease();

    /// <summary>
    /// ����� ���������� ��� ����� �������� ���� � ���
    /// </summary>
    private void Release()
    {
        _currentTimeToDestroy = timeToRelease; //��������� ������ ��� ���������

        if(!pooled) //��� ������� ����� ��� �������� ������, ��� �������� �� ������ ������
            abilityPool.Release(this.transform); //� ��� � ��� �����
    }

    /// <summary>
    /// ��������� ����� ����� ��������� �����, ����� ������� ������ ��������� � ���
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
