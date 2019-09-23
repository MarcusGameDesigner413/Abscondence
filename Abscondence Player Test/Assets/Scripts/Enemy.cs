using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent enemyAI;
    public Transform player;
    public ParticleSystem deathParticles;
    public Rigidbody rb;
    public float knockBackForce;
    public float knockBackTime;
    private float knockBackCounter;

    public int health = 100;
    public TextMesh healthCounter;
    private bool wasDamaged = false;
    private float timer = 0;
    public float invulnerabilityTime = 0.5f;

    private Vector3 enemyMoveDirection;

    private BoxCollider enemyCollider;


    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyCollider = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // The direction they will be sent after being hit is opposite to the direction they are facing
        // (Should be changed in future to be where they were hit when in contact with the sword)


        // Print the health to the text box above enemy
        if (health > 0)
        {
            healthCounter.text = "HP: " + health.ToString();
        }
        else if (health <= 0) // Make the enemy fall over when HP reaches 0
        {
            health = 0;
            transform.rotation = Quaternion.AngleAxis(90, Vector3.right);
            GetComponent<NavMeshAgent>().enabled = false;
        }

        // If the enemy took damage turn off the box collider
        if (wasDamaged)
        {
            timer += Time.deltaTime;
            EnemyInvulnerabilityOn();
            // Testing the enemy to fall off the map
            //GetComponent<NavMeshAgent>().enabled = false;
        }
        else
        {
            //GetComponent<NavMeshAgent>().enabled = true;
            enemyAI.SetDestination(player.position);
            enemyMoveDirection = transform.forward;
            rb.Sleep();
        }

        // And turn it back on after half a second (or change to be after the spin attack is finished)
        if (timer >= invulnerabilityTime)
        {
            EnemyInvulnerabilityOff();
            wasDamaged = false;
        }

        Debug.Log(rb.velocity);
    }

    // Code to destroy the enemy when it comes into contact with player's melee weapon
    void OnTriggerEnter(Collider other)
    {
        Vector3 hitDirection = -transform.forward/*other.transform.position - transform.position*/;
        hitDirection = hitDirection.normalized;

        if (other.tag == "Sword")
        {
            wasDamaged = true;
            KnockBack(hitDirection);
            health -= 25;
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

        rb.AddForce(enemyMoveDirection, ForceMode.Impulse);
    }
}