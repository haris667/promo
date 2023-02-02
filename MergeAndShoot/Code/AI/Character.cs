using UnityEngine;
using UnityEngine.AI;

sealed public class Character : Unit
{
    [SerializeField] private Transform movementTarget;
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private float regenerationPerFrame;
    private float timer;

    public void ChangeWeapon(WeaponData weaponData) 
    {
        this.weaponData = weaponData;
        attacking = weaponData.attackBehaviour;
    } 
    public Character(IMovementBehaviour movement, WeaponData weaponData, (int, int) health)
    {
        this.movement = movement;
        this.health = health;
        this.weaponData = weaponData;
        attacking = new RangeAttackBehaviour();
    }
    private void Update() 
    {
        FindTarget();
        CheckPosition(); 

        if(target != null)
        {
            RotateToTarget();
            timer -= Time.deltaTime;
            if(timer < 0)
            {
                Attack(weaponData.damage);
                timer = weaponData.delayAttack;
            }
        }
        if(StageManager.Instance.pause && target == null) Move();
    }
    public void ReturnMaxHealth() 
    {
        health.Item1 = health.Item2;
        healthView.UpdateHealthBar(health);
    } 
    private void FindTarget() =>  target = vision.CheckFieldOfView(transform.position);
    private void RotateToTarget() => transform.LookAt(transform.position + vision.directionDamagableObject * Mathf.Rad2Deg);
    public override void GetDamaged(int amountDamage) => base.GetDamaged(amountDamage);

    private void CheckPosition() //не лучшее решение, но быстрое
    {
        if(transform.position.x > movementTarget.position.x - 0.1f)
        {
            transform.position -= new Vector3(20, 0, 0);
            StageManager.Instance.SetStagePause(false);
        }
    }

}