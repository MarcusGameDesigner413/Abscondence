﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public int currentHealth = 100;
    public int maxHealth = 100;
    public float walkSpeed = 5;
    public float runSpeed = 10; // For Debug purposes [REMOVE IN ALPHA]
    public float turnSmoothTime = 0.1f;
    public float speedSmoothTime = 0.1f;
    public float invulnerabilityTime = 0.5f;
    public float knockBackForce;
    public float knockBackTime;
    private float knockBackCounter;
    public float slowDownAmount = 0.2f;

    float turnSmoothVelocity;
    float speedSmoothVelocity;
    float currentSpeed;

    public GameObject meleeWeapon;
    Animator meleeSwipe;

    private CharacterController controller;
    private CapsuleCollider playerCollider;
    private Vector3 playerMoveDirection;
    private bool playerWasDamaged;
    private float timer = 0;
    private Vector3 velocity;
    private float startingHeight;

    [HideInInspector]
    public bool gamePaused;

    public int storedPowerCell = 0;
    public int maxPowerCell = 5;

    void Start()
    {
        meleeSwipe = meleeWeapon.GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider>();
        controller = GetComponent<CharacterController>();
        startingHeight = transform.position.y;
    }

    void Update()
    {
        // Ignore the collisions between the sword and the environment (mostly the enemy cause it would damage him)
        Physics.IgnoreLayerCollision(0, 9, true);

        // Get the direction of input from the user
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        // Normalize the input
        Vector2 inputDir = input.normalized;

        if (inputDir != Vector2.zero)
        {
            /* ***THIS CODE LOCKS THE PLAYER ROTATION WHEN ATTACKING ANIMATION IS PLAYING***
            if (!(meleeSwipe.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1) && !meleeSwipe.IsInTransition(0))
            {*/

            // Set the target rotation to be equal to the direction that the player is facing
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;
            // Change the rotation to the player to be equal to that direction with smoothing
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);

            //}
        }

        bool movementDisabled = false;
        // Debug addition to get around faster
        bool running = Input.GetKey(KeyCode.LeftShift);
        // Set to walkSpeed in alpha test
        float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
        // Speed up the player overtime when they move
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        // Move the character relevant to the set current speed
        //transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);
        if (!movementDisabled)
            controller.Move(((transform.forward * currentSpeed) + velocity) * Time.deltaTime);

        // Subtract the velocity by the slowDownAmount to slow down the knockback
        velocity -= velocity * slowDownAmount;

        // If the velocity gets below a certain threshold, set it to zero
        if (velocity.magnitude < 0.35f)
            velocity = Vector3.zero;

        // Make sure the health doesn't overcap
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        // Player takes damage upon falling into hole
        if (transform.position.y < 0)
        {
            transform.position = new Vector3(0, 5, 0);
            currentHealth -= 50;
        }
        else if (transform.position.y > startingHeight) // Make sure the player stays on the ground
        {
            var previousX = transform.position.x;
            var previousZ = transform.position.z;

            transform.position = new Vector3(previousX, startingHeight, previousZ);
        }

        // Only use the timer if the counter has been activated
        if (knockBackCounter > 0)
        {
            movementDisabled = true;
            knockBackCounter -= Time.deltaTime;
        }
        // Once the Counter reaches 0, removes the force applied to the enemy
        else if (knockBackCounter <= 0)
        {
            movementDisabled = false;
        }


        // Check if player took damage
        PlayerTookDamage();

        // Check for animation plays
        PlayLightAnimation();
        PlayHeavyAnimation();
    }

    public void PlayLightAnimation()
    {
        if (Input.GetMouseButtonDown(0) && !gamePaused)
        {
            meleeSwipe.SetTrigger("ActiveLClick");
        }
    }

    public void PlayHeavyAnimation()
    {
        if (Input.GetMouseButtonDown(1) && !gamePaused)
        {
            meleeSwipe.SetTrigger("ActiveRClick");
        }


        // ***OLD CODE FOR A CHARGE UP AND RELEASE ANIMATION***
        // No it doesn't work.
        /*float holdTimer = 0;

        if (Input.GetMouseButtonDown(1))
        {
            // Start the charge up animation
            meleeSwipe.SetBool("ActiveRClick", true);

            // Once the user has held for 1 second, play the spin animation
            holdTimer += Time.deltaTime;
            if (Input.GetMouseButtonUp(1) && holdTimer > 1.0f)
            {
                meleeSwipe.SetBool("ActiveRClick", false);
                meleeSwipe.SetTrigger("ActiveRComplete");
                holdTimer = 0;
            }
            else if (Input.GetMouseButtonUp(1) && holdTimer < 1.0f)
            {
                meleeSwipe.SetBool("ActiveRClick", false);
            }
        }*/
    }

    void OnCollisionEnter(Collision other)
    {
        Vector3 playerHitDirection = other.transform.forward /*other.transform.position - transform.position*/;
        playerHitDirection = playerHitDirection.normalized;

        if (other.gameObject.tag == "Enemy")
        {
            playerWasDamaged = true;
            KnockBack(playerHitDirection);
            currentHealth -= 10;
        }
    }

    void PlayerTookDamage()
    {
        // If the enemy took damage turn off the box collider
        if (playerWasDamaged)
        {
            PlayerInvulnerabilityOn();
            timer += Time.deltaTime;
        }

        // And turn it back on after half a second (or change to be after the spin attack is finished)
        if (timer >= invulnerabilityTime)
        {
            PlayerInvulnerabilityOff();
            playerWasDamaged = false;
        }
    }

    void PlayerInvulnerabilityOn()
    {
        playerCollider.enabled = false;
        Debug.Log("Collider.enabled = " + playerCollider.enabled);
    }

    void PlayerInvulnerabilityOff()
    {
        playerCollider.enabled = true;
        timer = 0;
        Debug.Log("Collider.enabled = " + playerCollider.enabled);
    }

    public void KnockBack(Vector3 direction)
    {
        knockBackCounter = knockBackTime;

        playerMoveDirection = direction * knockBackForce;

        // Apply velocity relative to the direction the player has been knocked back
        velocity += playerMoveDirection;
    }

    //updated with on trigger stay
    void OnTriggerStay(Collider collision)
    {
        // Powercell pickup
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

        // Door open -- this requires the panel object to have the tag 'Panel'
        if (collision.gameObject.tag == "Panel" && Input.GetKeyDown(KeyCode.E))
        {

            //if the player has 1 or more power cells and the panel has not been activated before
            if (storedPowerCell >= 1 && !collision.gameObject.GetComponent<Panel>().xActivatedX) // panel activatd = false
            {
                //open the door
                storedPowerCell--;

                //play sound effect of door opening

                //destroy the door
                collision.gameObject.GetComponent<Panel>().xActivatedX = true;
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