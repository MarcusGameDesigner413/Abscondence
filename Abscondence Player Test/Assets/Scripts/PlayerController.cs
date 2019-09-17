using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    public float walkSpeed = 5;
    public float runSpeed = 10;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public float speedSmoothTime = 0.1f;
    float speedSmoothVelocity;
    float currentSpeed;

    public GameObject meleeWeapon;
    public GameObject player;
    Animator meleeSwipe;

    void Start()
    {
        meleeSwipe = meleeWeapon.GetComponent<Animator>();
    }

    void Update()
    {
        // Get the direction of input from the user
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        // Normalize the input
        Vector2 inputDir = input.normalized;

        if (inputDir != Vector2.zero)
        {
            // Set the target rotation to be equal to the direction that the player is facing
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;
            // Change the rotation to the player to be equal to that direction with smoothing
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
        }

        // Debug addition to get around faster
        bool running = Input.GetKey(KeyCode.LeftShift);
        // Set to walkSpeed in alpha test
        float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
        // Speed up the player overtime when they move
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        // Move the character relevant to the set current speed
        transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);

        playLightAnimation();
        playHeavyAnimation();
    }

    public void playLightAnimation()
    {
        if (Input.GetMouseButtonDown(0))
        {
            meleeSwipe.SetTrigger("ActiveLClick");
        }
    }

    public void playHeavyAnimation()
    {
        if (Input.GetMouseButtonDown(1))
        {
            meleeSwipe.SetTrigger("ActiveRClick");
        }


        //float holdTimer = 0;

        //if (Input.GetMouseButtonDown(1))
        //{
        //    // Start the charge up animation
        //    meleeSwipe.SetBool("ActiveRClick", true);

        //    // Once the user has held for 1 second, play the spin animation
        //    holdTimer += Time.deltaTime;
        //    if (Input.GetMouseButtonUp(1) && holdTimer > 1.0f)
        //    {
        //        meleeSwipe.SetBool("ActiveRClick", false);
        //        meleeSwipe.SetTrigger("ActiveRComplete");
        //        holdTimer = 0;
        //    }
        //    else if (Input.GetMouseButtonUp(1) && holdTimer < 1.0f)
        //    {
        //        meleeSwipe.SetBool("ActiveRClick", false);
        //    }
        //}
    }
    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Sword")
        {
            Physics.IgnoreCollision(collision.collider, meleeWeapon.GetComponent<Collider>(), true);
        }
    }
}