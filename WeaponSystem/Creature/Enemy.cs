using UnityEngine;
using UnityEngine.Pool;

public class Enemy : Creature
{
    private Material    mat;
    private Collider2D  coll;
    private float       enemyFade = 1f;
    private IObjectPool<Creature> pool;
    public Vector3 direction;
    private Vision Vision;

    private void Awake() 
    {
        contactFilter = LayerMask.GetMask("Hero");
        SetMovement( new HumanoidMovement(transform.GetComponent<Rigidbody2D>() , 4f) );
        SetWeapon( new Axe(transform, contactFilter, new WeaponInfo(-10, 20, 0.5f)) );

        Vision = new CentralView(transform);

        Health = GetComponent<Health>();
        mat = GetComponent<SpriteRenderer>().material;
        coll = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        ChangeAnimation(0);
        
        if (allive)
        {
            var result = Vision.CheckVision();
            switch (result.Item1)
            {
                case 0: if ( (transform.position - direction).magnitude > 0.2f) 
                    ChangeAnimation(1);

                    Move(direction);    
                    break;
                case 1: return;
                case 2: 
                    UseWeapon(result.Item2);                                                    
                    break;
                default: goto case 1;
            } 
        }
        else if (enemyFade == 1f && !allive)
        {
            Death();
            enemyFade = 0.99f;
        }
        else if (!allive)
        {
            enemyFade -= 0.02f;
            mat.SetFloat("_Fade", enemyFade);

            if (enemyFade <= 0)
            {
                if (pool != null)
                    pool.Release(this);
                else
                    Destroy(gameObject);
            }
        }
    }

    public void Reborn(SpawnOrigin spawnOrigin, EnemyStats stats)
    {
        weapon.WeaponInfo = stats.weapon;

        transform.position  = spawnOrigin.spawnPos;
        direction           = spawnOrigin.destinationPos;
        Vision              = NewVision(spawnOrigin.visionID);

        gameObject.name = stats.name;
        GetComponent<SpriteRenderer>().sprite = stats.sprite;
        animator.runtimeAnimatorController = stats.animController;
        
        Health.Init(new Vector2Int(stats.hp, stats.hp));
    }
    private Vision NewVision(int i)
    {
        switch (i) 
        {
            case 0: return new UpperView(transform);
            case 1: return new CentralView(transform);
            case 2: return new LowerView(transform);
            default: goto case 1;
        }
    }

    public void SetPool(IObjectPool<Creature> pool) => this.pool = pool;
    protected override void Death()
    {
        DeathEvent.PlayerDead(1);
        coll.enabled = false;
    }

    private void OnDisable()
    {
        enemyFade = 1f;
        mat.SetFloat("_Fade", 1f);
    }   
    private void OnEnable() 
    {
        coll.enabled = true;
        allive = true;
    } 
    private void ChangeAnimation(float speed) 
    {
        animator.SetFloat("Speed", speed);
        
        if(speed > 0)
        {
            animator.SetFloat("Vertical", -direction.y);
            animator.SetFloat("Horizontal", 1);
        }
    }                    
}
