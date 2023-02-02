using UnityEngine;

abstract public class Unit : MonoBehaviour, IDamagable
{
    [SerializeField] protected int health;
    protected IMovable movableBehaviour;
    protected Vector3 target;
    protected float speed;

    public Unit(IMovable movableBehaviour) => this.movableBehaviour = movableBehaviour;

    abstract public void Move();

    public void GetDamage(int amountDamage) => health -= amountDamage;
}
