using UnityEngine;

public class OgrWeapon : Weapon
{
    private Transform tr;
    private LayerMask contactFilter;
    private Creature  target;

    public OgrWeapon(Transform tr, LayerMask contactFilter, WeaponInfo WeaponInfo, Animator animator)
    {
        this.tr =               tr;
        this.contactFilter =    contactFilter;
        this.WeaponInfo =       WeaponInfo;
        this.animator =         animator;
    }
    public override void InitAgain(Transform tr, LayerMask contactFilter, Animator animator)
    {
        this.tr =               tr;
        this.contactFilter =    contactFilter;
        this.animator =         animator;
    }
    public override void Attack(GameObject target)
    {
        if(canHit) 
        {
            Vector3 targetV2 = target.transform.position - tr.position;
            RaycastHit2D hit = Physics2D.Raycast(tr.position, targetV2, 6f, contactFilter);

            if (hit)
            {
                AddTriggerAnimationAttack("Attack");
                hit.collider.gameObject.TryGetComponent(out Hero creature);
                creature.DropWeapon();
            } 
        }
    }
}
