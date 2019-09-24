using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent enemyAI;
    private PlayerController player;
    //public Transform playerTransform;
    public Rigidbody enemyRigidbody;
    public TextMesh healthCounter;
    public ParticleSystem deathParticles;
    public float knockBackForce;
    public float knockBackTime;
    private float knockBackCounter;

    public int health = 100;
    private bool wasDamaged = false;
    private float timer = 0;
    public float invulnerabilityTime = 0.5f;

    private Vector3 enemyMoveDirection;
    private BoxCollider enemyCollider;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        //playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        enemyCollider = GetComponent<BoxCollider>();
        enemyRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Print the health to the text box above enemy
        if (health > 0 || health == 0)
        {
            healthCounter.text = "HP: " + health.ToString();
        }

        if (health <= 0) // Make the enemy fall over when HP reaches 0
        {
            health = 0;
            transform.rotation = Quaternion.AngleAxis(90, Vector3.back);

            // Disable NavMesh to stop enemy from following the player
            GetComponent<NavMeshAgent>().enabled = false;
        }

        // Function is called when the player takes damage
        EnemyTookDamage();

        //Debug.Log(enemyRigidbody.velocity);
    }

    // Code to damage the enemy when it comes into contact with player's melee weapon
    void OnTriggerEnter(Collider other)
    {
        // The direction they will be sent after being hit is opposite to the direction they are facing
        // (Can be changed to where they were hit with the sword with commented code, but it's really awkward)
        Vector3 enemyHitDirection = -transform.forward /*other.transform.position - transform.posiion*/;
        enemyHitDirection = enemyHitDirection.normalized;

        if (other.tag == "Sword")
        {
            wasDamaged = true;
            KnockBack(enemyHitDirection);
            health -= 25;
        }
    }

    void EnemyTookDamage()
    {
        // If the enemy took damage turn off the box collider
        if (wasDamaged)
        {
            timer += Time.deltaTime;
            EnemyInvulnerabilityOn();
            knockBackCounter -= Time.deltaTime;
            // Testing the enemy to fall off the map (Requires physics I think)
            //GetComponent<NavMeshAgent>().enabled = false;
        }
        else
        {
            //GetComponent<NavMeshAgent>().enabled = true;
            enemyAI.SetDestination(player.transform.position);
            enemyMoveDirection = transform.forward;
            enemyRigidbody.Sleep();
        }

        // And turn it back on after half a second (or change to be after the spin attack is finished)
        if (timer >= invulnerabilityTime)
        {
            EnemyInvulnerabilityOff();
            wasDamaged = false;
        }
    }

    // Enemy Invulnerability was added because the enemy could take damage multiple times from a spin attack
    void EnemyInvulnerabilityOn()
    {
        enemyCollider.enabled = false;
        Debug.Log("Collider.enabled = " + enemyCollider.enabled);
    }

    void EnemyInvulnerabilityOff()
    {
        enemyCollider.enabled = true;
        timer = 0;
        Debug.Log("Collider.enabled = " + enemyCollider.enabled);
    }

    public void KnockBack(Vector3 direction)
    {
        knockBackCounter = knockBackTime;

        enemyMoveDirection = direction * knockBackForce;

        enemyRigidbody.AddForce(enemyMoveDirection, ForceMode.Impulse);
    }
}