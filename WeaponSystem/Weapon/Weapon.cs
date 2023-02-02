using UnityEngine;

public abstract class Weapon 
{
    protected SpriteRenderer spriteRend;
    public WeaponInfo WeaponInfo;

    protected Animator animator;

    protected float timer = 0;

    protected bool canHit 
    {
        get 
        {
            timer -= Time.deltaTime;
            if (WeaponInfo.durable == 100)
            {
                WeaponInfo.durable--;
                timer = WeaponInfo.delay / 2;
                return false;
            }
            if(timer <= 0) 
            {
                timer = WeaponInfo.delay;
                return true;
            }
            else return false;
        }
    }

    public abstract void Attack(GameObject target);
    protected void DealDamage(Creature Creature) 
    {
        if (Creature)
        {
            Creature.DealDamage( WeaponInfo.damage );
        }
             
    }
    public abstract void InitAgain(Transform tr, LayerMask contactFilter, Animator animator);
    public int CheckDurable() => WeaponInfo.durable;
    protected void AddTriggerAnimationAttack(string nameAnimation) 
    {
        if (animator)
            animator.SetTrigger(nameAnimation);
    }

}

[System.Serializable]
public struct WeaponInfo 
{
    public int damage;
    public int durable;
    public float delay;
    public WeaponInfo(int damage, int durable, float delay)
    {
        this.damage     = damage;
        this.durable    = durable;
        this.delay      = delay;
    }
}
