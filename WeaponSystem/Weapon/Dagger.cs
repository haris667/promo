using UnityEngine;


///<summary>
/// Оружие предназначенное для быстрого убийства слабых врагов
/// Прочность: "прочное"; Дистанция: 1; Скорость: "быстрая"; Урон: "маленький";
///</summary>
public class Dagger : Weapon
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
    public Dagger(Transform tr, LayerMask contactFilter, WeaponInfo WeaponInfo)
    {
        this.tr =               tr;
        this.contactFilter =    contactFilter;
        this.WeaponInfo =       WeaponInfo;
    } 
    public override void Attack(GameObject target)
    {
        
        if(canHit) 
        {
            Vector3 targetV2 = target.transform.position - tr.position;
            RaycastHit2D hit = Physics2D.Raycast(tr.position, targetV2, 4f, contactFilter);

            if (hit)
            {
                AddTriggerAnimationAttack("AxeAttackTrigger");
                hit.collider.gameObject.TryGetComponent(out Creature creature);
                Debug.Log(creature.transform.gameObject.name);
                DealDamage(creature);
            } 
            WeaponInfo.durable--;   
            ChangePositionAnimAttack(target);
        }
    }
    private void ChangePositionAnimAttack(GameObject target) 
    {
        if(animator) 
        {
            animator.transform.position = new Vector3(animator.transform.position.x, target.transform.position.y, 0);
        }
    }
}
