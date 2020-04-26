/*****************************************************************************
// File Name : FinalBossBehaviour
// Author : Kyle Grenier
// Creation Date : March 31, 2020
//
// Brief Description : Code that controls the mechanics of the final boss, including spawning minion waves and transitioning to the next stage.
*****************************************************************************/

using UnityEngine;

public class FinalBossBehaviour : MonoBehaviour
{
    private Transform player;
    private Rigidbody2D rb;
    //The distance the player must be to the final boss to initiate the waves spawning.
    public float minDistanceToPlayer = 10f;

    public int wavesToSpawn = 3;
    public int maxRounds = 3;

    public GameObject smokeParticle;
    private int currentWave = 0;
    private int currentEnemyCount = 0;

    private bool playerInRange = false;
    private bool waveStarted = false;
    private bool waveFinished = false;
    private bool roundOver = false;
    private bool runBoss = false;

    public float timeBetweenWaves = 3f;
    private float currentTime = 0f;

    private int currentRound = 0;

    private float distToPlayer;
    private BossHealthBehaviour healthBehaviour;
    //private Animator anim;
    private BossShootingBehaviour shootingBehaviour;

    private Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        healthBehaviour = GetComponent<BossHealthBehaviour>();
        shootingBehaviour = GetComponent<BossShootingBehaviour>();
        anim = GetComponent<Animator>();
        audioController = GetComponentInChildren<AudioController>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            FinalizeWave();

        if (runBoss)
        {
            HandleBossBehaviour();
            return;
        }
        
        //Make sure the player is in range before doing anything.
        //Only for the inital wave so the player finds the boss before the first wave starts.
        if (!playerInRange)
        {
            distToPlayer = Vector2.Distance(transform.position, player.position);
            playerInRange = distToPlayer < minDistanceToPlayer;
            return;
        }

        //Start the wave
        if (!waveStarted)
        {
            //If roundOver is true, make it false because there are still waves to spawn.
            if (roundOver)
                roundOver = false;

            StartWave();
        }
        //If the wave has started and the player has killed all of the spawned enemies, 
        //set finished var to true.
        else if (waveStarted && !waveFinished && currentEnemyCount <= 0)
        {
            print("WAVE " + (currentWave + 1) + " CLEARED!");

            waveFinished = true;
        }

        //Now that the wave is finished, we begin waiting for timeBetweenWaves until the next wave comes.
        if (waveFinished && currentTime < timeBetweenWaves)
        {
            currentTime += Time.deltaTime;
        }
        else if (currentTime >= timeBetweenWaves && !roundOver)
        {
            //Finalize the current wave and start the new one.
            FinalizeWave();
        }
    }

    void StartWave()
    {
        audioController.PlayClip(AudioController.BossSFX.whistleBlow);
        print("WHISTLE BLOW!: Round #" + (currentRound + 1) + "     Wave #" + (currentWave + 1));
        waveStarted = true;

        //Get an array of all of the spawners in the scene.
        GameObject[] spawnerObjects = GameObject.FindGameObjectsWithTag("Minion Spawner");
        MinionSpawner[] spawners = new MinionSpawner[spawnerObjects.Length];
        for (int i = 0; i < spawnerObjects.Length; ++i)
        {
            spawners[i] = spawnerObjects[i].GetComponent<MinionSpawner>();
            //If the spawner is not supposed to be run this round, skip to the next available spawner.
            if (spawners[i].roundActive != (currentRound + 1))
                continue;

            spawners[i].SpawnMinions();
            currentEnemyCount += spawners[i].GetEnemyCount();
        }
    }

    public void DecreaseMinionCount()
    {
        //If the round is over or the wave hasn't started yet, don't decrease the enemy count.
        if (roundOver || !waveStarted)
            return;
        currentEnemyCount--;
        print("Current Enemy Count: " + currentEnemyCount);
    }

    void FinalizeWave()
    {
        currentTime = 0f;
        if (currentEnemyCount != 0)
            currentEnemyCount = 0;

        //If we reached the max number of waves for the level, perform transition operations.
        currentWave++;
        if (currentWave >= wavesToSpawn)
        {
            //Increase currentRound to the round that just ended.
            currentRound++;
            if (currentRound >= maxRounds)
            {
                print("ALL ROUNDS HAVE BEEN COMPLETED!");
                roundOver = true;
                ActivateBoss();
                return;
            }
            currentWave = 0;
            print("THIS ROUND IS OVER. THE BOSS HAS MOVED!");
            MoveBossPosition();
            TurnOnInteractables();
            playerInRange = false;
            roundOver = true;
        }

        waveFinished = false;
        waveStarted = false;
    }

    //Interactables with the tag "'Round ' + currentRound'" will be turned on after the final boss moves.
    void TurnOnInteractables()
    {
        string tag = "Round " + currentRound;
        GameObject[] interactableObjects = GameObject.FindGameObjectsWithTag(tag);
        InteractableBehaviour[] interactables = new InteractableBehaviour[interactableObjects.Length];

        for (int i = 0; i < interactables.Length; ++i)
        {
            interactables[i] = interactableObjects[i].GetComponent<InteractableBehaviour>();
            interactables[i].PowerOn();
        }
    }

    void MoveBossPosition()
    {
        //Instantiate the smoke effect and move the boss.
        Instantiate(smokeParticle, transform.position, Quaternion.identity);

        string name = "Boss Pos " + (currentRound + 1);
        Transform newPos = GameObject.Find(name).transform;

        transform.position = newPos.position;
    }





    [Header("Final boss attributes")]
    public float shootingDistance = 10f;
    public float jumpAttackDistance = 3f;
    public float jumpForce = 4f;
    public float smashForce = 10f;
    private bool jumped = false;
    public float movementSpeed = 12f;
    private Vector2 newPos;
    private bool facingRight = false;
    private bool isGrounded = false;
    public Transform groundCheck;
    public LayerMask whatIsGround;

    private bool knockedDown = false;
    public float knockDownRecoveryTime = 2f;
    private float currentRecoveryTime = 0f;

    public Transform[] tpPositions;

    private AudioController audioController;

    //Sets up the battle with the boss.
    void ActivateBoss()
    {
        MoveBossPosition();
        rb.isKinematic = false;
        runBoss = true;
    }

    private void FixedUpdate()
    {
        //Handles boss movement.

        //Don't move if he's been hit, killed, or knocked down.
        if (!runBoss || jumped || knockedDown || healthBehaviour.beenHit || healthBehaviour.beenKilled || knockedDown)
        {
            anim.SetBool("IsRunning", false);
            return;
        }

        //Moving the rigidbody along x-axis.
        Vector2 pos = rb.position;
        pos.x += newPos.x * Time.fixedDeltaTime;
        rb.position = pos;

    }

    void HandleBossBehaviour()
    {
        //If the enemy has been hit or killed, don't make him move or shoot -- wait until he has recovered.
        if (healthBehaviour.beenHit || healthBehaviour.beenKilled)
        {
            anim.SetBool("IsRunning", false);
            anim.SetBool("IsShooting", false);
            return;
        }
            

        //Don't do anything while he recovers -- hence why we return.
        if (knockedDown && currentRecoveryTime < knockDownRecoveryTime)
        {
            currentRecoveryTime += Time.deltaTime;
            return;
        }
        else if (currentRecoveryTime >= knockDownRecoveryTime)
        {
            knockedDown = false;
            currentRecoveryTime = 0f;

            int r = Random.Range(0, tpPositions.Length);
            transform.position = tpPositions[r].position;
        }

        //Calculating distance and direction towards the player.
        distToPlayer = Vector2.Distance(transform.position, player.position);
        Vector2 direction = (player.position - transform.position).normalized;

        isGrounded = Physics2D.Linecast(transform.position, groundCheck.position, whatIsGround);
        //If the enemy has jumped and they are now grounded, they have recovered from the jump so set jumped to true.
        if (isGrounded && jumped)
        {
            jumped = false;
            anim.SetBool("IsJumping", false);
        }
            

        //Smashing down.
        if (jumped && transform.position.y > -21f)
        {
            audioController.PlayClip(AudioController.BossSFX.smash);
            rb.velocity = Vector2.zero;
            rb.AddForce(-Vector2.up * smashForce, ForceMode2D.Impulse);
        }

        //Handle flipping here.
        HandleFlipping(direction);

        if (distToPlayer > shootingDistance)
        {
            anim.SetBool("IsRunning", true);
            //Calculating the velocity for the minion.
            newPos.x = direction.x * movementSpeed;
        }
        else if (distToPlayer < shootingDistance && distToPlayer > jumpAttackDistance) //shoot the player.
        {
            //Don't shoot in air.
            //if (!isGrounded)
               // return;

            anim.SetBool("IsShooting", true);
            anim.SetBool("IsRunning", false);
            if (newPos.x != 0)
            {
                newPos.x = 0;
            }

            //Handle shooting. Time between shots, bullet prefab, etc., are all in the MinionShootingBehaviour script.
            shootingBehaviour.HandleShooting();
        }
        else //Perform jump attack
        {
            anim.SetBool("IsRunning", false);
            if (newPos.x != 0)
            {
                newPos.x = 0;
            }

            //Handle jump attack
            if (isGrounded)
            {
                rb.velocity = Vector2.zero;
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                anim.SetBool("IsJumping", true);
                jumped = true;
                audioController.PlayClip(AudioController.BossSFX.jump);
            }

        }
    }

    private void HandleFlipping(Vector2 direction)
    {
        if (direction.x < 0 && facingRight)
        {
            Flip();
        }
        else if (direction.x > 0 && !facingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        Vector3 rot = transform.rotation.eulerAngles;
        rot.y += 180;

        //Invert the boolean.
        facingRight = !facingRight;

        transform.rotation = Quaternion.Euler(rot);
    }


    private void OnCollisionEnter2D(Collision2D col)
    {
        //If the enemy collides with the player while he is in his jumping attack, damage the player.
        if (col.gameObject.CompareTag("Player"))
        {
            if (col.GetType() == typeof(CircleCollider2D))
                return;
            
            if (col.gameObject.GetComponentInParent<PlayerController>().umbrellaUp && col.gameObject.GetComponentInParent<PlayerController>().umbrella && !knockedDown && jumped)
            {
                print("Boss knocked down!");
                healthBehaviour.TakeDamage(1);
                knockedDown = true;
            }
            else if (!knockedDown) //if the boss collides with the player any other time he's not knocked down.
            {
                GameControllerScript.instance.RemoveLivesFromPlayer(1);
            }
        }
    }
}