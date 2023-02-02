using UnityEngine;

public abstract class Creature : MonoBehaviour
{
    protected Health    Health;
    protected Animator  animator;
    protected LayerMask contactFilter; 

    protected string    appellation;
    protected bool      allive = true;

    private IMovement   movement;
    protected Weapon      weapon;

    public void SetMovement(IMovement movement)     => this.movement = movement;
    public void ChangeSpeed(float newspead)         => movement.SetMoveSpeed(newspead);
    public void Move(Vector3 direction)             => movement.Move(direction);

    public void SetWeapon(Weapon weapon)            => this.weapon = weapon;
    public void UseWeapon(GameObject target)        => weapon.Attack(target);

    public void DealDamage(int damage)              => Health.DealDamage(damage, out allive);
    
    protected virtual void Death()                  => transform.gameObject.SetActive(false);
}