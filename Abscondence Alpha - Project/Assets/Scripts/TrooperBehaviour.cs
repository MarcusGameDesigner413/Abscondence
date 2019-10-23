using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class TrooperBehaviour : MonoBehaviour
{

    public enum trooperState
    {
        idle,
        suspicious,
        alert,
        downed,
        dead,
        riverSpawn,
        doorSpawn
    }

    //the state the trooper is currently in
    private trooperState currentState;

    //the hitbox used when the enemy takes damage
    private BoxCollider hitCollision;

    //the hitbox used when enemy is downed
    private CapsuleCollider downedCollision;

    public bool canSpawnOnDeath = true;

    //the item that spawns if the enemy dies
    public GameObject deathItem;

    //instance of navmesh agent
    public NavMeshAgent enemyAI;

    //was i spawned in via spawn object?
    public bool wasSpawnedInRiver = false;
    public bool wasSpawnedInDoor = false;

    //Speed variables:
    public int idleWalkSpeed = 5; //speeds it moves in idle
    public int suspiciousWalkSpeed = 7; //speeds it moves in suspicious
    public int alertWalkSpeed = 10; //speed it moves in alert


    //current health trooper has
    public float currentHealth = 100;

    //health the troop gains when it goes from downed to not being downed
    public int reviveHealthGain = 100;

    //health the trooper has to be downed
    public int downedHealth = 1;

    //check to see if the enemy dies by a bottomless pit
    private bool deathByPit = false;

    //time until downed is over
    public int downedTimeUntilAlive = 5; //time (in seconds) trooper has until it gets back up
    private float downedCounter = 0; //set as a float to use with deltaTime, iterates until downeduntilalive is reached

    //radius of suspcious circle
    public int maxSuspiciousRadius = 20;

    //radius of alert cirlce
    public int maxAlertRadius = 10;

    //how close the trooper needs to be to hit the enemy
    public int attackRadius = 2;

    //public float accesed by player script when they get damaged by this enemy
    public int enemyAttackStrength = 1;

    //how close the enemy can walk to the player
    public float MeleeRotation = 2;

    //addition of suspicious and alert radius
    private int combinedAlertRadius;

    //distance before the agent returns to its spawn position (idle only)
    public float maxIdleTravelDistanceRadius = 10;

    //allows idle to walk left, right, up back accordingly
    public bool idleCanWalkUpDown = true;
    public bool idleCanWalkLeftRight = true;

    //idleTimer variables:
    public int minIdleTimerRange = 2; //min seconds of random time
    public int maxIdleTimerRange = 10; //max seconds of random time
    private int idleTimer = 5; //random time set set back to private
    private float idleCounter = 0; //set as a float to use with deltaTime, iterates through
    //private bool setOppositeTravelDirection = false; //boolean to flip if outside bounds

    //booleans for setting direction, used in idle
    private bool justSetDirection = false;
    private bool idleReturning = false; //set back to private

    //random int used to determine travel direction in idle
    private int idleTravelDirection = 0;


    //stored position of where the agent's middle radius is (set to private)
    private Vector3 idleCentrePosition;

    //stored position of last time the agent knew where the player was (set to private)
    private Vector3 lastKnownPlayerPosition;

    //knockback taken when enemy is hit
    public float knockBackForce;

    //time the player is knocked back for
    public float knockBackTime;

    //invulerbility time after being hit
    public float invulnerabilityTime = 0.5f;

    //???
    private float knockBackCounter;

    //boolean to see if it just took damage
    [HideInInspector]
    public bool wasDamaged = false;

    //timer used when ememny was hit by player
    private float wasHitTimer = 0;

    //the object that the enemy sword is
    public GameObject meleeWeapon;

    //animator tool?
    Animator trooperAnimation;

    //move backwards when hit?
    private Vector3 enemyMoveDirection;

    //refer to its own rigidbody
    private Rigidbody enemyRigidbody;

    //bool to trigger invincible code when it just revived
    private bool justRevived = false;

    //max time it has invincible when it respawns
    public float respawnInvincibleTime = 2;

    //private iterartion to make it no longer invincible when it respawns
    private float respawnIteration = 0;


    //is the enemy downed? --- DO NOT MODIFY DESIGNERSSS
    [HideInInspector]
    public bool xIsDownedX = false;

    //is the enemy alive --- DO NOT MODIFY DESIGNERSSS
    [HideInInspector]
    public bool xIsDeadX = false;

    
    
    
    // Start is called before the first frame update
    void Start()
    {
        hitCollision = GetComponent<BoxCollider>();
        downedCollision = GetComponent<CapsuleCollider>();

        //ensure the downed collison is false
        //downedCollision.gameObject.SetActive(false);
        downedCollision.enabled = false;

        trooperAnimation = meleeWeapon.GetComponent<Animator>();
        //trooperAnimation = GetComponent<Animator>();

        //ensure own rigidbody is correct
        enemyRigidbody = GetComponent<Rigidbody>();

        //false makes it slide like ice but cant go through stuff
        //true removes knockback
        //enemyRigidbody.isKinematic = false;

        //on startup enemy set to idle by default
        if (wasSpawnedInDoor)
        {
            currentState = (trooperState)6;
        }
        else if (wasSpawnedInRiver)
        {
            currentState = (trooperState)5;
        }
        else
        {
            currentState = (trooperState)0;
        }
        

        //add the alert and suspicious radius
        combinedAlertRadius = maxSuspiciousRadius + maxAlertRadius;

        //store start position
        idleCentrePosition = this.transform.position;

        lastKnownPlayerPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Physics.IgnoreLayerCollision(0, 12, true);

        //switch statement, triggers functions based on which state ai is in
        switch ((int)currentState)
        {
            case 0: //idle
                UpdateIdle();
                break;

            case 1: //suspicious
                UpdateSuspicious();
                break;

            case 2: //alert
                UpdateAlert();
                break;
                
            case 3: //downed
                UpdateDowned();
                break;

            case 4: //dead
                UpdateDead();
                break;

            case 5: //unique spawn (river)
                UpdateRiverSpawn();
                break;

            case 6: //unique spawn (door)
                UpdateDoorSpawn();
                break;
        }

        EnemyTookDamage();

        //if the remaining health = downedhealth, state is now downed
        if (currentHealth <= downedHealth)
        {
            currentState = (trooperState)3;

            //the trooper cannot take damage from normal attacks
            //hitCollision.gameObject.SetActive(false);
            hitCollision.enabled = false;

            //the trooper can now be executed
            //downedCollision.gameObject.SetActive(true);
            downedCollision.enabled = true;
        }

        //peform the revive timer if it just revived
        if (justRevived)
        {
            //update the iteration
            respawnIteration += 1 * Time.deltaTime;

            //once the iteration is greater than or equal to the respawn time
            if (respawnIteration >= respawnInvincibleTime)
            {
                //you are no lo9nger just revived
                justRevived = false;

                //you can now be hit
                hitCollision.enabled = true;

                //reset the iteration
                respawnIteration = 0;
            }
        }
        
        if(xIsDeadX)
        {
            currentState = (trooperState)4;
        }

    }

    //in this state the ai walks around randomly in a set area
    void UpdateIdle()
    {

        //if the ai state is the idle state
        if (currentState == (trooperState)0)
        {
           //setup the navmesh agent speed
            enemyAI.speed = idleWalkSpeed;
        }
       

        //choose random direction, 1 = left, 2 = right, 3 = forward, 4 = backward
        if (!justSetDirection)
        {
            //store the previous direction
            int previousDirection = idleTravelDirection;

            //if it can walk left/right and NOT up/down 
            if(idleCanWalkLeftRight && !idleCanWalkUpDown)
            {
                //the range can only be between 1 and 2
                idleTravelDirection = Random.Range(1, 2);

            }

            //if it can NOT walk left/right and up/down 
            if (!idleCanWalkLeftRight && idleCanWalkUpDown)
            {
                //the range can only be between 3 and 4
                idleTravelDirection = Random.Range(3, 4);

            }

            //if it can move in all 4, it can be between 1 - 4
            if (idleCanWalkLeftRight && idleCanWalkUpDown)
            {
                //the range can only be between 1 and 4
                idleTravelDirection = Random.Range(1, 4);

            }

            //if it cannot travel in any direction for some reason...
            if (!idleCanWalkLeftRight && !idleCanWalkUpDown)
            {
                //the range can only be between 1 and 2
                idleTravelDirection = 0;

            }

             //timer gets set to random number
             idleTimer = Random.Range(minIdleTimerRange, maxIdleTimerRange);

            //idle counter gets reset
            idleCounter = 0;

            //we just set the direction so you are now true
            justSetDirection = true;
        }



        //if the direction has been set and the trooper is not returning
        if (justSetDirection)
        {
            //move enemy based on random number assigned to idleTravelDirection
            if (idleTravelDirection == 1) //left -- opposite is 2
            {
                Vector3 newPosition = new Vector3(transform.position.x + maxIdleTravelDistanceRadius, 0, transform.position.z);
                
                enemyAI.SetDestination(newPosition);
            }
            if (idleTravelDirection == 2) //right -- opposite is 1
            {
                Vector3 newPosition = new Vector3(transform.position.x - maxIdleTravelDistanceRadius, 0, transform.position.z);

                enemyAI.SetDestination(newPosition);
            }
            if (idleTravelDirection == 3) //forward -- opsosite is 4
            {
                Vector3 newPosition = new Vector3(transform.position.x, 0, transform.position.z + maxIdleTravelDistanceRadius);

                enemyAI.SetDestination(newPosition);
            }
            if (idleTravelDirection == 4) //backward -- oposite is 3
            {
                Vector3 newPosition = new Vector3(transform.position.x, 0, transform.position.z - maxIdleTravelDistanceRadius);

                enemyAI.SetDestination(newPosition);
            }
           

            if (idleTravelDirection == 0) //go to idle position.... used if idlereturning or no random direction set...
            {
                //go to idle position
                enemyAI.SetDestination(idleCentrePosition);
            }

            //add to the counter
            idleCounter += 1 * Time.deltaTime;

            
        }

        //if idle counter is greater or equal to idletimer and we are not idle returning
        if (idleCounter >= idleTimer)
        {
            //setup next loop
            justSetDirection = false;

            //idleReturning = false;
        }

        //setup to check if 
        //Vector3 positionPlusIdleTravelDistance = idleCentrePosition;
        //positionPlusIdleTravelDistance.x = positionPlusIdleTravelDistance.x + maxIdleTravelDistanceRadius;
        //positionPlusIdleTravelDistance.z = positionPlusIdleTravelDistance.z + maxIdleTravelDistanceRadius;

        


        float inRadiusIdlePositive = Vector3.Distance(transform.position, idleCentrePosition);
        //float inRadiusIdleNegative = Vector3.Distance(transform.position, positionPlusIdleTravelDistance);

        //if the agent is outside the maxIdleDistance, it should return --------- kinda janky
        if (inRadiusIdlePositive > maxIdleTravelDistanceRadius /*|| inRadiusIdlePositive < -maxIdleTravelDistanceRadius*/)
        {
            idleReturning = true;
            //set the random number to the one based on opposite direction
            //if (idleTravelDirection == 1 && !setOppositeTravelDirection)
            //{
            //    idleTravelDirection = 2;
            //    setOppositeTravelDirection = true;
            //}
            //if (idleTravelDirection == 2 && !setOppositeTravelDirection)
            //{
            //    idleTravelDirection = 1;
            //    setOppositeTravelDirection = true;
            //}
            //if (idleTravelDirection == 3 && !setOppositeTravelDirection)
            //{
            //    idleTravelDirection = 4;
            //    setOppositeTravelDirection = true;
            //}
            //if (idleTravelDirection == 4 && !setOppositeTravelDirection)
            //{
            //    idleTravelDirection = 3;
            //    setOppositeTravelDirection = true;
            //}

            //enemyAI.SetDestination(idleCentrePosition);
        }
        else
        {
            idleReturning = false;
            //set to false for future usage in checking travel distance
            //setOppositeTravelDirection = false;
        }

        //if the ai is returning (outside radius) --------- kinda janky?
        if (idleReturning)
        {
            //go to centre of radius -- idle travel direction is set to 0
            idleTravelDirection = 0;

        }

        //get the player position to check if it is in range
        Vector3 playerPos = GameObject.Find("Player").GetComponent<Transform>().position;

        float distance = Vector3.Distance(playerPos, transform.position);
        //if inside the radius
        if (distance <= combinedAlertRadius)
        {
            //change the state to suspicious
            currentState = (trooperState)1;
        }
        
    }

    void UpdateSuspicious()
    {
        //if the ai state is the suspicious state
        if (currentState == (trooperState)1)
        {
            //change ai speed
            enemyAI.speed = suspiciousWalkSpeed;
        }
        
        //get the player position
        Vector3 playerPos = GameObject.Find("Player").GetComponent<Transform>().position;

        //setup distance for checking
        float distance = Vector3.Distance(playerPos, transform.position);

        //get cross product of 
        Vector3 direction = (playerPos - transform.position).normalized;

        //do a raycast to see if the player is within sight 
        Ray ray = new Ray(transform.position, direction); 
        RaycastHit hit = new RaycastHit();
        int mask = 1 << LayerMask.NameToLayer("Player");
        
        
        
        //if in sight, store playerPos as lastKnownPlayerPosition and go there
        if (Physics.Raycast(ray, out hit, 9999, mask))
        {
            Debug.DrawRay(transform.position, direction * hit.distance, Color.yellow);

            if(Physics.Raycast(ray, out hit, distance, ~mask))
            {
                Debug.DrawRay(transform.position, direction * hit.distance, Color.blue);
                //reset idle returning and justsetdirection
                idleReturning = false;
                //justSetDirection = false;
                //do idle movement
                UpdateIdle();
            }
            else 
            {
                
                //update last known player position
                lastKnownPlayerPosition = playerPos;

                //update idle centre position
                idleCentrePosition = lastKnownPlayerPosition;

                //travel to the player
                enemyAI.SetDestination(lastKnownPlayerPosition);
                
                




            }

        }
       

        
        
        //if outside the combined radius
        if (distance >= combinedAlertRadius)
        {
            //reset idle returning and justsetdirection
            idleReturning = false;
            //justSetDirection = false;
            //change the state to idle
            currentState = (trooperState)0;
        }

        //if inside the alert radius
        if (distance <= maxAlertRadius)
        {
            
            //change the state to alert
            currentState = (trooperState)2;
        }
    }

    void UpdateAlert()
    {
        //change speed if now alert
        //if (currentState == (trooperState)2)
        //{
            //change ai speed
            enemyAI.speed = alertWalkSpeed;
        //}

        UpdateSuspicious();

        //get the player position
        Vector3 playerPos = GameObject.Find("Player").GetComponent<Transform>().position;

        //setup distance for check
        float distance = Vector3.Distance(playerPos, transform.position);

        //attack if close
        if(distance < attackRadius)
        {
            RotateTowards(playerPos);
            gameObject.GetComponent<NavMeshAgent>().isStopped = true;
            PlayAttackAnimation();
            
        }
        else
        {
            gameObject.GetComponent<NavMeshAgent>().isStopped = false;
        }

        //if outside alert radius become suspicious
        if (distance > maxAlertRadius) //took out = from >=
        {
            //change the state to suspicious
            currentState = (trooperState)1;
        }
    }

    void UpdateDowned()
    {
        //downed animation
        //PlayDownedAnimation();

        xIsDownedX = true;

        //set destination to itself so it doesnt move
        //enemyAI.SetDestination(transform.position);
        gameObject.GetComponent<NavMeshAgent>().isStopped = true;

        transform.rotation = Quaternion.AngleAxis(90, Vector3.back);

        //settup timer
        if (downedCounter >= downedTimeUntilAlive)
        {
            //restore the health set in revive health
            currentHealth = reviveHealthGain;

            //play getup animation
            PlayGetUpFromDownedAnimation();

            //the trooper can now take damage from normal attacks
            //hitCollision.gameObject.SetActive(true);
            //hitCollision.enabled = true;

            //the trooper cannot be executed 
            //downedCollision.gameObject.SetActive(false);
            downedCollision.enabled = false;

            //reset dead counter
            downedCounter = 0;

            transform.rotation = Quaternion.AngleAxis(90, Vector3.back);

            gameObject.GetComponent<NavMeshAgent>().isStopped = false;

            xIsDownedX = false;

            //respawn invulerability timer starts
            justRevived = true;

            //set to idle
            currentState = (trooperState)0;
        }

        //add to the counter
        downedCounter += 1 * Time.deltaTime;
    }

    void UpdateDead()
    {
        

        //if cause of death is not a pit
        if(!deathByPit)
        {
            //play death animation

            //if the troop can spawn an item on death
            if(canSpawnOnDeath)
            {
                //spawn death object on self
                Instantiate(deathItem, transform.position, transform.rotation);
            }
         
        }
        else
        {
            //play falling scream audio???
        }
        

        //trooper is gone from game world
        Destroy(gameObject);

    }

    void UpdateRiverSpawn()
    {
        //play swim animation

        //play jump animation


        currentState = (trooperState)0;
    }

    void UpdateDoorSpawn()
    {
        //play jump through door animation

        currentState = (trooperState)0;
    }

    //used to make the enemy rotate towards the player (used when it stand still and attacks)
    void RotateTowards(Vector3 target)
    {
        //set y as 90 for both to prevent it doing a Michael Jackson and slanting
        Vector3 targetOnlyY = new Vector3 (target.x, 90, target.z);
        Vector3 positionOnlyY = new Vector3(transform.position.x, 90, transform.position.z);

        Vector3 direction = (targetOnlyY - positionOnlyY).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * MeleeRotation);
    }

    // Code to damage the enemy when it comes into contact with player's melee weapon
    void OnCollisionEnter(Collision other)
    {
        // The direction they will be sent after being hit is opposite to the direction they are facing
        // (Can be changed to where they were hit with the sword with commented code, but it's really awkward)
        Vector3 enemyHitDirection = -transform.forward /*other.transform.position - transform.posiion*/;
        enemyHitDirection = enemyHitDirection.normalized;

        if (other.gameObject.tag == "Sword" && !xIsDownedX)
        {
            float healthLostOnHit = 0;
            PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            //if attack1 bolean == true
            if (player.lightAttackUsed)
            {
                //healthLostOnHit = Player.gameObject.GetComponent<PlayerController>().whateverattack1is;
                healthLostOnHit = player.playerLightDamage;
            }

            //if attack2 boolean == true
            if (player.heavyAttackUsed)
            {
                //healthLostOnHit = Player.gameObject.GetComponent<PlayerController>().whateverattack1is;
                healthLostOnHit = player.playerHeavyDamage;
            }
            wasDamaged = true;
            KnockBack(enemyHitDirection);
            currentHealth -= healthLostOnHit;
            
        }

        //if (other.gameObject.tag != "Sword" /*&& enemyRigidbody.isKinematic == false*/)
        //{
        //    enemyRigidbody.velocity = Vector3.zero;
        //}
        
    }

   

    // a timer that gets called each frame, code only runs if it was damaged tho
    void EnemyTookDamage()
    {
        // If the enemy took damage turn off the box collider
        if (wasDamaged)
        {
            wasHitTimer += Time.deltaTime;
            // Testing the enemy to fall off the map (Requires physics I think)
            GetComponent<NavMeshAgent>().enabled = false;

            EnemyInvulnerabilityOn();
            
        }
        //else
        //{
        //    enemyRigidbody.velocity = Vector3.zero;
        //}
       

        // And turn it back on after half a second (or change to be after the spin attack is finished)
        if (wasHitTimer >= invulnerabilityTime)
        {
            EnemyInvulnerabilityOff();
            wasDamaged = false;
            GetComponent<NavMeshAgent>().enabled = true;
        }
    }

    // Enemy Invulnerability was added because the enemy could take damage multiple times from a spin attack
    void EnemyInvulnerabilityOn()
    {
        //hitCollision.gameObject.SetActive(false);
        hitCollision.enabled = false;
        //Debug.Log("Collider.enabled = " + enemyCollider.enabled);
        //enemyRigidbody.isKinematic = false;

    }

    void EnemyInvulnerabilityOff()
    {
        //hitCollision.gameObject.SetActive(true);
        hitCollision.enabled = true;
        wasHitTimer = 0;
        //Debug.Log("Collider.enabled = " + enemyCollider.enabled);
        enemyRigidbody.isKinematic = true;

        //refreeze position and rotation
        //enemyRigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void KnockBack(Vector3 direction)
    {
        enemyRigidbody.isKinematic = false;

        //unfreeze position and rotation
       // enemyRigidbody.constraints = RigidbodyConstraints.None;

        knockBackCounter = knockBackTime;

        enemyMoveDirection = direction * knockBackForce;

        //transform.Translate(enemyMoveDirection);
        enemyRigidbody.AddForce(enemyMoveDirection, ForceMode.Impulse);
    }

    private void OnDrawGizmosSelected() //makes a sphare to match the size of the enemys "lookRadius" in the scene view
    {
        Gizmos.DrawWireSphere(transform.position, maxSuspiciousRadius);
        Gizmos.color = Color.cyan;

        Gizmos.DrawWireSphere(transform.position, maxAlertRadius);
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(transform.position, attackRadius);
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, maxIdleTravelDistanceRadius);
        Gizmos.color = Color.magenta;

    }


    public void PlayAttackAnimation()
    {
        
       trooperAnimation.SetTrigger("AttackAnimation");
        
    }

    public void PlayDownedAnimation()
    {

        trooperAnimation.SetTrigger("DownedAnimation");

    }

    public void PlayisDownedAnimation()
    {

        trooperAnimation.SetTrigger("isDownedAnimation");

    }

    public void PlayGetUpFromDownedAnimation()
    {

        trooperAnimation.SetTrigger("GetUpFromDownedAnimation");

    }
}





//extra code

//ensures the next random direction is not the previous random number -------
/*if(idleTravelDirection == previousDirection)
{
    //if it is 4
    if(idleTravelDirection == 4)
    {
        //it now equals 1
        idleTravelDirection = 1;
    }
    else
    {
        //add 1
        idleTravelDirection++;
    }

}*/

//cut idle directions
/*//if (idleTravelDirection == 5) //diagonal 1 - opposite is 6
       //{
       //    Vector3 newPosition = new Vector3(transform.position.x + maxIdleTravelDistanceRadius, 0, transform.position.z + maxIdleTravelDistanceRadius);

       //    enemyAI.SetDestination(newPosition);
       //}
       //if (idleTravelDirection == 6) //diagonal 2 -- oposite is 5
       //{
       //    Vector3 newPosition = new Vector3(transform.position.x - maxIdleTravelDistanceRadius, 0, transform.position.z - maxIdleTravelDistanceRadius);

       //    enemyAI.SetDestination(newPosition);
       //}
       //if (idleTravelDirection == 7) //diagonal 3 -- oposite is 8
       //{
       //    Vector3 newPosition = new Vector3(transform.position.x + maxIdleTravelDistanceRadius, 0, transform.position.z - maxIdleTravelDistanceRadius);

       //    enemyAI.SetDestination(newPosition);
       //}
       //if (idleTravelDirection == 8) //diagonal 4 -- oposite is 7
       //{
       //    Vector3 newPosition = new Vector3(transform.position.x - maxIdleTravelDistanceRadius, 0, transform.position.z + maxIdleTravelDistanceRadius);

       //    enemyAI.SetDestination(newPosition);
       //}*/

//if (idleTravelDirection == 5 && !setOppositeTravelDirection)
//{
//    idleTravelDirection = 6;
//    setOppositeTravelDirection = true;
//}
//if (idleTravelDirection == 6 && !setOppositeTravelDirection)
//{
//    idleTravelDirection = 5;
//    setOppositeTravelDirection = true;
//}
//if (idleTravelDirection == 7 && !setOppositeTravelDirection)
//{
//    idleTravelDirection = 8;
//    setOppositeTravelDirection = true;
//}
//if (idleTravelDirection == 8 && !setOppositeTravelDirection)
//{
//    idleTravelDirection = 7;
//    setOppositeTravelDirection = true;
//}
