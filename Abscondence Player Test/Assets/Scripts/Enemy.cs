using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    public float knockBackForce;
    public float knockBackTime;
    public float invulnerabilityTime = 0.5f;
    public TextMesh healthCounter;
    public ParticleSystem deathParticles;

    private float knockBackCounter;
    private bool wasDamaged = false;
    private float timer = 0;

    private NavMeshAgent enemyAI;
    private PlayerController player;
    private Rigidbody enemyRigidbody;
    private Vector3 enemyMoveDirection;
    private BoxCollider enemyCollider;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        enemyAI = GetComponent<NavMeshAgent>();
        enemyRigidbody = GetComponent<Rigidbody>();
        enemyCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        var enemyPosition = transform.position;

        // Print the health to the text box above enemy
        if (health > 0 || health == 0)
        {
            healthCounter.text = "HP: " + health.ToString();
        }

        if (health <= 0) // Make the enemy fall over when HP reaches 0
        {
            health = 0;
            transform.rotation = Quaternion.AngleAxis(90, Vector3.back);
            transform.position = new Vector3(enemyPosition.x, 0.5f, enemyPosition.z);
            enemyCollider.enabled = false;
            deathParticles.Play();

            // Disable NavMesh to stop enemy from following the player
            GetComponent<NavMeshAgent>().enabled = false;
        }

        // Only use the timer if the counter has been activated
        if (knockBackCounter > 0)
            knockBackCounter -= Time.deltaTime;

        // Once the Counter reaches 0, removes the force applied to the enemy
        if (knockBackCounter <= 0)
        {
            enemyRigidbody.Sleep();
        }

        // Function is called when the player takes damage
        EnemyTookDamage();
    }

    // Code to damage the enemy when it comes into contact with player's melee weapon
    void OnCollisionEnter(Collision other)
    {
        // The direction they will be sent after being hit is opposite to the direction they are facing
        // (Can be changed to where they were hit with the sword with commented code, but it's really awkward)
        Vector3 enemyHitDirection = -transform.forward /*other.transform.position - transform.posiion*/;
        enemyHitDirection = enemyHitDirection.normalized;

        if (other.gameObject.tag == "Sword")
        {
            wasDamaged = true;
            KnockBack(enemyHitDirection);
            health -= 25;
        }
    }

    // ***BUG: KnockbackTime is based on the Invulnerability time***
    void EnemyTookDamage()
    {
        // If the enemy took damage turn off the box collider
        if (wasDamaged)
        {
            timer += Time.deltaTime;
            EnemyInvulnerabilityOn();
            // Testing the enemy to fall off the map (Requires physics I think)
            //GetComponent<NavMeshAgent>().enabled = false;
        }
        else
        {
            // While the enemy has health and the knockback counter hasn't reached 0, let the enemy move
            if (health > 0 && knockBackCounter <= 0)
            {
                enemyAI.SetDestination(player.transform.position);
                enemyMoveDirection = transform.forward;
            }

            //GetComponent<NavMeshAgent>().enabled = true;
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