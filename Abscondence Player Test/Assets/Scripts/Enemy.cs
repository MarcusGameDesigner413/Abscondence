using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {
	public NavMeshAgent enemyAI;
	public Transform player;
	public ParticleSystem deathParticles;

    public int health = 100;
    public TextMesh healthCounter;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
        healthCounter.text = "HP: " + health.ToString();

		// If not dead, Follow player
        if (health > 0)
        {
            enemyAI.SetDestination(player.position);
        }

        if (health <= 0)
        {
            health = 0;
            transform.rotation = Quaternion.AngleAxis(90, Vector3.right);
        }
	}

    // Code to destroy the enemy when it comes into contact with player's melee weapon
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Sword")
        {
            health -= 25;
        }
    }
}