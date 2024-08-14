using UnityEngine;

/// <summary>
/// Перемещение объекта по направление с физикой, но без Rigidbody
/// </summary>
public class U_ProjectileMovement : MonoBehaviour
{ 
    public Vector3 _gVec = new Vector3(0, -9.81f, 0); //конста гравитации
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
    /// Берем направление, добавляем зависимость от гравитации (gVec) и умножаем на время, чтобы
    /// чем дольше объект был в воздухе, тем сильнее начинал падать вниз. Формула из физики 7 класса похожая
    /// </summary>
    private void Move()
    {
        timer += Time.deltaTime;
        transform.position += (_direction + _gVec * timer) * Time.deltaTime;
    }

    /// <summary>
    /// В связи с пулингом, обновляем данные. 
    /// </summary>
    private void OnDisable()
    {
        moved = false;
        timer = 0;
    }
}
