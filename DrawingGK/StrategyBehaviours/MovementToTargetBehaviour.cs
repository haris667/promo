using UnityEngine;

sealed public class MovementToTarget : IMovable
{
    private float speed;
    public MovementToTarget(float speed) => this.speed = speed;
    public void Move(Vector3 target, Transform positionObject)
    {
        positionObject.position = Vector3.MoveTowards(positionObject.position, target, speed * Time.deltaTime);
    }
}