using UnityEngine;

/// <summary>
/// ����������� ������� �� ����������� � �������, �� ��� Rigidbody
/// </summary>
public class U_ProjectileMovement : MonoBehaviour
{ 
    public Vector3 _gVec = new Vector3(0, -9.81f, 0); //������ ����������
    private Vector3 _direction;

    private float _speed;
    private float timer = 0;
    private bool moved;

    void FixedUpdate()
    {
        if(moved)
            Move();
    }

    public void StartMove(Vector3 direction, float speed, float forceG = -9.81f)
    {
        _gVec.y = forceG;
        _speed = speed;
        _direction = direction;
        moved = true;
    }

    /// <summary>
    /// ����� �����������, ��������� ����������� �� ���������� (gVec) � �������� �� �����, �����
    /// ��� ������ ������ ��� � �������, ��� ������� ������� ������ ����. ������� �� ������ 7 ������ �������
    /// </summary>
    private void Move()
    {
        timer += Time.deltaTime;
        transform.position += (_direction + _gVec * timer) * Time.deltaTime;
    }

    /// <summary>
    /// � ����� � ��������, ��������� ������. 
    /// </summary>
    private void OnDisable()
    {
        moved = false;
        timer = 0;
    }
}
