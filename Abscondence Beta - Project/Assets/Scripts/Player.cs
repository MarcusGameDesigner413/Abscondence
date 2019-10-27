using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    // We have asked the computer to remember these
    public CharacterController charControl;
    public float playerSpeed = 0.5f;
    Vector3 playerDirection;
    public Transform spawn;
    public ParticleSystem deathParticles;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Apply gravity
        charControl.Move(Vector3.down * Time.deltaTime * 5.0f);

        // Spawns bullet and shoots it
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //GameObject spawnedBullet = (GameObject)Instantiate(bullet, shootPosition.position, Quaternion.identity);
            //spawnedBullet.GetComponent<Rigidbody>().AddForce(playerDirection * 10.0f, ForceMode.Impulse);
            //Destroy(spawnedBullet, 3.0f);
        }

        // Rotate the player depending on the direction the player is heading
        transform.localEulerAngles = playerDirection;

        //Move up when W key is pressed
        if (Input.GetKey(KeyCode.W))
        {
            charControl.Move(Vector3.forward * Time.deltaTime * playerSpeed);
            playerDirection = Vector3.forward;
        }
        // Move left when A key is pressed
        if (Input.GetKey(KeyCode.A))
        {
            charControl.Move(Vector3.left * Time.deltaTime * playerSpeed);
            playerDirection = Vector3.left * 90;
        }
        //Move down when S key is pressed
        if (Input.GetKey(KeyCode.S))
        {
            charControl.Move(Vector3.back * Time.deltaTime * playerSpeed);
            playerDirection = Vector3.back * 180;
        }
        // Move right when D key is pressed
        if (Input.GetKey(KeyCode.D))
        {
            charControl.Move(Vector3.right * Time.deltaTime * playerSpeed);
            playerDirection = Vector3.right * 90;
        }

        //if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
        //{
        //    playerDirection = Vector3.forward + Vector3.left;
        //}

        //if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
        //{
        //    playerDirection = Vector3.forward + Vector3.right;
        //}

        //if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
        //{
        //    playerDirection = Vector3.back + Vector3.left;
        //}

        //if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        //{
        //    playerDirection = Vector3.back + Vector3.right;
        //}
    }
}

//void OnTriggerEnter(Collider other)
//{
//    // Compare the object's tag to "Destroy"
//    if (other.tag == "Destroy")
//    {
//        // Set particle position to player's position
//        deathParticles.transform.position = transform.position + Vector3.up * 2.0f;
//        // Set's the players position to spawn position
//        transform.position = spawn.position;
//        deathParticles.Play();
//        lives -= 1;
//        if (lives == 0)
//        {
//            Debug.Log("Player Died!");
//            SceneManager.LoadScene(0);
//        }
//    }
//}
