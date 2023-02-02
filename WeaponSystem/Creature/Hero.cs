using UnityEngine;

public class Hero : Creature
{
    public GameObject       alliveEnemy; 
    private Vision          Vision;
    public GameObject       iconObj;
    public ChangeWeapon     changeWeapon;
    private Animator        animatorWeapons;
    private void Awake() 
    {
        contactFilter = LayerMask.GetMask("Monster");
        animatorWeapons = transform.GetChild(0).GetComponent<Animator>();

        SetMovement ( new HeroMovement() );   
        //SetWeapon( new Greatsword(transform, contactFilter, new WeaponInfo(-10, 10, 0.5f)) );
        Vision = new HeroView(transform, "Monster");

        Health = GetComponent<Health>();

        DeathEvent.eDeath += CheckDeath;
    }

    public void DropWeapon()
    {
        SetWeapon( null );
        changeWeapon.TurnOffOn(false);
        Debug.Log("Оружие сломалось");
    }
    private void Update() 
    {
        var result1 = Vision.CheckVision();
        if ( weapon != null)
        {
            if (alliveEnemy != null)
            {
                if ( !alliveEnemy.activeSelf )
                {
                    alliveEnemy = null;
                    return;
                } 
                UseWeapon(alliveEnemy); 
                if (weapon.CheckDurable() <= 0)
                {
                    DropWeapon();
                    return;
                }
                changeWeapon.ChangeCount( weapon.CheckDurable() );
            }
            else
            {
                var result = Vision.CheckVision();

                if (result.Item1 == 2)
                    alliveEnemy = result.Item2;
            }   
        }
        
        if (!allive)
            Death();
    }

    protected override void Death()
    {
        DeathEvent.PlayerDead(0);
        base.Death();
    } 
    private void CheckDeath(int creatureID)
    {
        if (creatureID != 0) 
            alliveEnemy = null;
    } 
    public void ChangeState(bool newState)
    {
        iconObj.SetActive(newState);
    }
    public void ChangeWeapon(Weapon newWeapon)
    {
        newWeapon.InitAgain(transform, contactFilter, animatorWeapons);
        SetWeapon( newWeapon );
        
        changeWeapon.TurnOffOn(true);
        changeWeapon.ChangeCount( weapon.CheckDurable() );
        changeWeapon.ChangeWeaponSprite(CheckWeapon(newWeapon.WeaponInfo.damage));
    }
    private int CheckWeapon(int damage)
    {
        switch (damage)
        {
            case -5: return 2;
            case -10: return 0;
            case -15: return 1;
            default: goto case -10;
        }
    }
    private void OnDestroy()  => DeathEvent.eDeath -= CheckDeath;
}
