//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.AI;

//public class SpawnDoor : MonoBehaviour
//{
//    public GameObject objectToSpawn;
//    public int numberOfEnemies = 1;
//    public float spawnTimer = 3f;
//    public float timeBetweenEachSpawn = 1f;
//    public GameObject spawnDoorTrigger;
//    public bool alwaysActive = false;

//    private GameObject[] enemyArray;
//    private BoxCollider boxCheckCollider;
//    private Collider triggerBoxCollider;
//    private int enemyDead = 0;
//    private float tempSpawnTimer = 0;
//    private bool spawnDeath = false;
//    private bool doorBlocked = false;
//    private bool firstWaveSpawned = false;

//    [HideInInspector]
//    public bool playerTriggeredDoor = false;

//    // Use this for initialization
//    void Start()
//    {
//        enemyArray = new GameObject[numberOfEnemies];
//        boxCheckCollider = gameObject.GetComponent<BoxCollider>();
//        triggerBoxCollider = spawnDoorTrigger.GetComponent<Collider>();
//        tempSpawnTimer = spawnTimer;
//        boxCheckCollider.isTrigger = true;
//        if (alwaysActive)
//        {
//            StartCoroutine(WaitABit(1));
//        }
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (playerTriggeredDoor && !alwaysActive)
//        {
//            StartCoroutine(WaitABit(1));
//            playerTriggeredDoor = false;
//        }

//        // Only do all this if the door isn't blocked
//        if (!doorBlocked && firstWaveSpawned)
//        {
//            // Keep first cause spawnDeath is true by default to spawn in the first wave
//            if (spawnDeath && tempSpawnTimer < 0) // Spawn in a wave of troopers if they are all dead and the timer reaches 0
//            {
//                StartCoroutine(WaitABit(2));
//            }

//            enemyDead = 0;

//            // Keep track of how many troopers are dead
//            for (int i = 0; i < enemyArray.Length; i++)
//            {
//                if (enemyArray[i].GetComponent<TrooperBehaviour>().xIsDeadX)
//                {
//                    enemyDead++;
//                }
//                else if (!enemyArray[i].GetComponent<TrooperBehaviour>().xIsDeadX && enemyDead != enemyArray.Length)
//                    spawnDeath = false;
//            }

//            // Once all the troopers are dead, start the timer to spawn more in
//            if (enemyDead == numberOfEnemies)
//            {
//                tempSpawnTimer -= Time.deltaTime;
//                spawnDeath = true;
//                enemyDead = 0;
//            }
//        }
//    }

//    IEnumerator WaitABit(int actionNumber)
//    {
//        switch (actionNumber)
//        {
//            case 1: // Fill array with enemies and Spawn enemies at spawn position
//                for (int i = 0; i < enemyArray.Length; i++)
//                {
//                    enemyArray[i] = Instantiate(objectToSpawn, transform.position, Quaternion.identity);
//                    enemyArray[i].GetComponent<TrooperBehaviour>().wasSpawnedInDoor = true;
//                    yield return new WaitForSeconds(timeBetweenEachSpawn);
//                }
//                firstWaveSpawned = true;
//                break;
//            case 2: // Respawn the enemies when they are dead
//                for (int i = 0; i < enemyArray.Length; i++)
//                {
//                    ResetEnemy(enemyArray[i]);
//                    yield return new WaitForSeconds(timeBetweenEachSpawn);
//                }
//                tempSpawnTimer = spawnTimer;
//                break;
//            default:
//                break;
//        }
//    }

//    void OnTriggerEnter(Collider other)
//    {
//        if (other.tag == "Block")
//        {
//            doorBlocked = true;
//            Debug.Log("Door blocked.");
//        }
//        else if (other.tag != "Block")
//        {
//            doorBlocked = false;
//            Debug.Log("Door unblocked.");
//        }
//    }

//    void ResetEnemy(GameObject enemy)
//    {
//        TrooperBehaviour trooper = enemy.GetComponent<TrooperBehaviour>();
//        BoxCollider enemyCollider = enemy.GetComponent<BoxCollider>();
//        NavMeshAgent enemyNavMesh = enemy.GetComponent<NavMeshAgent>();


//        enemy.SetActive(true); // Set trooper as active so the scripts can be adjusted
//        enemyNavMesh.enabled = false; // Disable their NavMesh so they can be teleported through walls
//        enemy.transform.SetPositionAndRotation(transform.position, enemy.transform.rotation); // Set their position to the object the script is attached to
//        trooper.currentHealth = trooper.reviveHealthGain; // Reset their health
//        trooper.xIsDeadX = false;
//        trooper.xIsDownedX = false;
//        trooper.currentState = TrooperBehaviour.trooperState.doorSpawn; // Change their AI state
//        enemyCollider.enabled = true;
//        enemyNavMesh.enabled = true;
//    }
//}
