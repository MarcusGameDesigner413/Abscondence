using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    public float walkSpeed = 7;
    public float runSpeed = 10;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public float speedSmoothTime = 0.1f;
    float speedSmoothVelocity;
    float currentSpeed;

    public int currentHealth = 75;
    public int maxHealth = 100;

    public int storedPowerCell = 0;
    public int maxPowerCell = 5;

    public GameObject meleeWeapon;

    Animator meleeSwipe;
    bool moving = false;

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

        playAnimation();
    }

    public void playAnimation()
    {
        if (Input.GetMouseButtonDown(0))
        {
            meleeSwipe.SetTrigger("Active");
        }
    }

    //updated with on trigger stay

    void OnTriggerStay(Collider collision)
    {


        //powercell pickup
        if (collision.gameObject.tag == "PowerCell" && Input.GetKeyDown(KeyCode.E))
        {
            if (storedPowerCell >= maxPowerCell)
            {
                //play sound of --NO--, DO NOT ADD to SCORE
            }
            else
            {
                //update the score
                storedPowerCell++;

                //delete the power cell
                Destroy(collision.gameObject);
            }

        }

        //door open -- this requires the panel object to have the tag 'Panel'
        if (collision.gameObject.tag == "Panel" && Input.GetKeyDown(KeyCode.E))
        {

            //if the player has 1 or more power cells and the panel has not been activated before
            if (storedPowerCell >= 1 && !collision.gameObject.GetComponent<Panel>().activated) // panel activatd = false
            {
                //open the door
                storedPowerCell--;

                //play sound effect of door opening

                //destroy the door
                collision.gameObject.GetComponent<Panel>().activated = true;
            }
            else
            {
                //play sound effect of --NO--, DO NOT REMOVE FROM SCORE

            }


        }

        //health interact
        if (collision.gameObject.tag == "Health" && Input.GetKeyDown(KeyCode.E))
        {
            //if the player has less than max health
            if (currentHealth < maxHealth)
            {

                //sets up the amount to heal
                int healthGained = collision.gameObject.GetComponent<HealthPickup>().healthRestoreAmount;

                //heal the player
                currentHealth = currentHealth + healthGained;

                //if player gains more than max health
                if (currentHealth > maxHealth)
                {
                    //current health gets set to max health
                    currentHealth = maxHealth;
                }

                //play sound effect of healing

                //destroy the door
                Destroy(collision.gameObject);
            }
            else
            {
                //play sound effect of --NO--, DO NOT REMOVE FROM SCORE

            }

        }

    }
}