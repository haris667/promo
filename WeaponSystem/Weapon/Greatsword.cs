using UnityEngine;

///<summary>
/// Оружие предназначенное для массовой анигиляции врагов перед Героем
/// Прочность: "обычное"; Дистанция: 1; Скорость: "медленное"; Урон: "Средний";
///</summary>
public class Greatsword : Weapon
{
    private Transform tr;
    private LayerMask contactFilter;
    private Creature target;


    public override void InitAgain(Transform tr, LayerMask contactFilter, Animator animator)
    {
        this.tr =               tr;
        this.contactFilter =    contactFilter;
        this.animator =         animator;
    } 
    public Greatsword(Transform tr, LayerMask contactFilter, WeaponInfo WeaponInfo)
    {
        this.tr =               tr;
        this.contactFilter =    contactFilter;
        this.WeaponInfo =       WeaponInfo;
    }
    // public override void InitAgain(Transform tr, LayerMask contactFilter)
    // {
    //     this.tr =               tr;
    //     this.contactFilter =    contactFilter;
    // }
    public override void Attack(GameObject target)
    {
        if(canHit) 
        {
            Vector2 v2 = tr.right;
            for ( float f = -0.75f; f < 0.8f; f += 0.75f)
            {
                v2.y = tr.right.y + f;
                RaycastHit2D hit = Physics2D.Raycast(tr.position, v2, 4f, contactFilter);
                if (hit)
                {   
                    AddTriggerAnimationAttack("GreatswordAttackTrigger");
                    hit.collider.gameObject.TryGetComponent(out Creature creature);
                    Debug.Log(creature.transform.gameObject.name);
                    DealDamage(creature);
                }
            } 
            WeaponInfo.durable--;
        }
    }
}
