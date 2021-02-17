using UnityEngine;
using UnityEngine.AI;

public class AITutorial : MonoBehaviour, HeroAbstract
{
    public AudioSource audioSource;
    public AudioClip enemyMusic;
    public static bool isPlayingMusic = false;

    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public Rigidbody rb;
    public Animator animator;

    // patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    // states
    public float sightRange, attackRange;
    public bool playerInSigtRange, playerInAttackRange;

    public int health;
    bool isDead = false;

    public float nextTimeToFire = 0f;
    public Gun gun;

    public AudioSource hero;
    public AudioClip hit;
    public AudioClip deathHit;
    public AudioClip step;
    public AudioClip inwater;
    // public AudioClip in_out_water;

    public bool inWater = false;
    
    public float timerToStep = 0f;
    public float stepTime = 0.5f;
    public float waterStepTime = 3f;
    public float moveSpeed;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        if (!AITutorial.isPlayingMusic)
        {
            audioSource.loop = true;
            audioSource.Play();
            isPlayingMusic = true;
        }
    }

    private void FixedUpdate()
    {
        if (moveSpeed > 0)
        {
            StepSound();
        }
        
        if (PlayerControllerTransform.healthPoints > 0)
        {
            if (health <= 0 && isDead == false)
            {
                PlayerControllerTransform.killedOpponents++;
                isDead = true;
                death();
            }

            if (!isDead)
            {
                playerInSigtRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
                playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

                if (!playerInSigtRange && !playerInAttackRange) Patrolling();
                if (playerInSigtRange && !playerInAttackRange) ChasePlayer();
                if (playerInSigtRange && playerInAttackRange) AttackPlayer();
            }
        }
        else
        {
            animator.SetFloat("moveSpeed", 0f);
            moveSpeed = 0;
            
        }
    }

    private void Patrolling()
    {
        if (!walkPointSet)
        {
            animator.SetFloat("moveSpeed", 0.0f);
            SearchWalkPoint();
            moveSpeed = 2;
        }

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
            animator.SetFloat("moveSpeed", 1.1f);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f) walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        moveSpeed = 0;
        animator.SetFloat("moveSpeed", 0.0f);

        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) walkPointSet = true;
    }

    private void ChasePlayer()
    {
        if (PlayerControllerTransform.healthPoints > 0)
        {
            agent.SetDestination(player.position);
            moveSpeed = 1.1f;
        }
        // else
        // {
        //     return;
        // }
    }

    private void AttackPlayer()
    {
        moveSpeed = 0f;
        transform.LookAt(player);
        agent.SetDestination(transform.position);
        
        if (!alreadyAttacked)
        {
            animator.SetFloat("moveSpeed", 0.0f);

            if (Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / gun.fireRate;
                shoot();
            }

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }


    public void shoot()
    {
        gun.spawnBullet();
        animator.SetTrigger("weaponFire");
    }

    public void getHit(int damageAmount)
    {
        if (!isDead)
        {
            this.health -= damageAmount;
            animator.SetTrigger("hit");
        }
    }

    public void death()
    {
        agent.enabled = false;
        this.enabled = false;
        animator.enabled = false;
        rb.freezeRotation = false;
        isDead = true;
        PlayerControllerTransform p = (PlayerControllerTransform) player.GetComponent<PlayerControllerTransform>();

        p.weaponHolder
            .allGuns
            .Find(gun1 => gun1.name.Equals(this.gun.name))
            .isAllowed = true;
        audioSource.Stop();
        isPlayingMusic = false;
        
        
        
    }
    
    
    void StepSound()
    {
        if (timerToStep > 0)
        {
            timerToStep -= Time.deltaTime * 1.5f;
        }

        if (timerToStep <= 0 && moveSpeed > 0 && !inWater)
        {
            hero.PlayOneShot(step);
            timerToStep = stepTime;
        }
    }
    
    void WaterStepSound()
    {
        if (timerToStep > 0)
        {
            timerToStep -= Time.deltaTime * .01f;
        }

        if (timerToStep <= 0 && moveSpeed > 0)
        {
            hero.PlayOneShot(inwater);
            timerToStep = waterStepTime;
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        // if (collision.gameObject.CompareTag("water"))
        // {
        //     animator.SetBool("inWater", true);
        //     audioSource.PlayOneShot(in_out_water);
        //     inWater = true;
        //     moveSpeed = 0.5f;
        // }
    }
    
    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("water") && moveSpeed > 0)
        {
            WaterStepSound();
        }

    }
    
    
}