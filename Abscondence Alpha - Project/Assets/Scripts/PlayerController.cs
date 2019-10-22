﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public int currentHealth = 100;
    public int maxHealth = 100;
    public float walkSpeed = 5;
    //public float runSpeed = 10; // For Debug purposes [REMOVE IN ALPHA]
    public int playerLightDamage = 1;
    public int playerHeavyDamage = 2;
    public float turnSmoothTime = 0.1f;
    public float speedSmoothTime = 0.1f;
    public float invulnerabilityTime = 0.5f;
    public float knockBackForce = 150f;
    public float knockBackTime = 0.45f;
    private float knockBackCounter;
    public float slowDownAmount = 0.2f;
    public float gravityModifier = 10.0f;
    public int storedPowerCell = 0;
    public int maxPowerCell = 5;

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
    private Vector3 gravity;
    private BottomlessPit ifFallen;
    private Vector2 input;
    private Vector3 relativePosition;

    [HideInInspector]
    public bool gamePaused;
    [HideInInspector]
    public float startingHeight;
    [HideInInspector]
    public bool lightAttackUsed = false;
    [HideInInspector]
    public bool heavyAttackUsed = false;
    [HideInInspector]
    public Vector3 draggableBlockVelocity;

    public bool DeathToMenu = false;



    GameObject box;


    enum DraggingState
    {
        NONE,
        VERTICAL,
        HORIZONTAL,
    };

    DraggingState currentState;


    void Start()
    {
        gravity = Physics.gravity * gravityModifier;
        meleeSwipe = meleeWeapon.GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider>();
        controller = GetComponent<CharacterController>();
        startingHeight = transform.position.y;
        currentState = DraggingState.NONE;
        //ifFallen = GameObject.Find("BottomlessPit_Half").GetComponent<BottomlessPit>();
    }

    void Update()
    {
        // Ignore the collisions between the sword and the environment (mostly the enemy cause it would damage him)
        //Physics.IgnoreLayerCollision(0, 9, true);

        // Get the direction of input from the user
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (currentState == DraggingState.VERTICAL)
            input.x = 0;
        else if (currentState == DraggingState.HORIZONTAL)
            input.y = 0;

        // Normalize the input
        Vector2 inputDir = input.normalized;

        bool movementDisabled = false;
        // Debug addition to get around faster
        //bool running = Input.GetKey(KeyCode.LeftShift);
        // Set to walkSpeed in alpha test
        float targetSpeed = (/*(running) ? runSpeed : */walkSpeed) * inputDir.magnitude;
        // Speed up the player overtime when they move
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        if (inputDir != Vector2.zero)
        {
            /* ***THIS CODE LOCKS THE PLAYER ROTATION WHEN ATTACKING ANIMATION IS PLAYING***
            if (!(meleeSwipe.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1) && !meleeSwipe.IsInTransition(0))
            {*/
            if (currentState == DraggingState.NONE)
            {
                // Set the target rotation to be equal to the direction that the player is facing
                float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;
                // Change the rotation to the player to be equal to that direction with smoothing
                transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);

                // Move the character relevant to the set current speed
                //transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);
                if (!movementDisabled/* && !ifFallen*/)
                    controller.Move(((transform.forward * currentSpeed) + velocity) * Time.deltaTime);
            }
            else
            {
                Vector3 dir = new Vector3(inputDir.x, 0, inputDir.y);

                if (!movementDisabled/* && !ifFallen*/)
                    controller.Move(((dir * currentSpeed) + velocity) * Time.deltaTime);

            }
            //}
        }



        // Add gravity to the player
        velocity += gravity * Time.deltaTime;

        // Subtract the velocity by the slowDownAmount to slow down the knockback
        velocity -= velocity * slowDownAmount;

        // If the velocity gets below a certain threshold, set it to zero
        if (velocity.magnitude < 0.35f)
            velocity = Vector3.zero;

        // Logic checks to make sure HealthBar array doesn't go out of bounds
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        if (currentHealth <= 0)
            currentHealth = 0;

        // 14 is the magic number so shut up
        if (maxHealth > 14)
            maxHealth = 14;

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

        if (currentHealth == 0 && DeathToMenu == true)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
        }

        //Box dragging
        if (Input.GetButtonUp("Interact"))
        {
            currentState = DraggingState.NONE;
            //Set velocity to zero
            
            //set box to null
            box = null;
        }
        Debug.DrawLine(transform.position + new Vector3(-0.5f, 5.0f, 0.0f), transform.position + new Vector3(0.5f, 5.0f, 0.0f), Color.red);

    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case DraggingState.VERTICAL:
                Vector3 targetPosition = controller.transform.position + relativePosition;
                targetPosition.y = box.transform.position.y;

                Vector3 velocity = (targetPosition - box.transform.position) / Time.fixedDeltaTime;

                box.GetComponent<Rigidbody>().AddForce(velocity * 40);
                //box.GetComponent<Rigidbody>().velocity = velocity;

                break;

            case DraggingState.HORIZONTAL:
                targetPosition = controller.transform.position + relativePosition;
                targetPosition.y = box.transform.position.y;

                velocity = (targetPosition - box.transform.position) / Time.fixedDeltaTime;

                box.GetComponent<Rigidbody>().AddForce(velocity * 40);
                break;
        }
    }

    public void PlayLightAnimation()
    {
        if (Input.GetMouseButtonDown(0) && !gamePaused)
        {
            lightAttackUsed = true;
            heavyAttackUsed = false;
            meleeSwipe.SetTrigger("ActiveLClick");
        }
    }

    public void PlayHeavyAnimation()
    {
        if (Input.GetMouseButtonDown(1) && !gamePaused)
        {
            heavyAttackUsed = true;
            lightAttackUsed = false;
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

    // When the enemy hits the player, the player takes damage
    void OnCollisionEnter(Collision other)
    {
        Debug.Log("Collision");
        Vector3 playerHitDirection = other.transform.forward /*other.transform.position - transform.position*/;
        playerHitDirection = playerHitDirection.normalized;



        if (other.gameObject.tag == "EnemySword")
        {
            //TrooperBehaviour enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<TrooperBehaviour>();
            TrooperBehaviour enemy = other.gameObject.GetComponentInParent<TrooperBehaviour>();

            playerWasDamaged = true;
            KnockBack(playerHitDirection);
            currentHealth -= enemy.enemyAttackStrength;
        }
    }

    // Function to call when the player takes damage
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

    void OnTriggerEnter(Collider other)
    {
        //if (other.name == "TopCollider" || other.name == "BottomCollider")
        //{
        //    //Vector3 relativePosition = other.transform.position - transform.position;
        //    //Vector3 targetPosition = transform.position + relativePosition;
        //    //targetPosition.y = other.transform.position.y;
        //    ////other.transform.Translate(relativePosition - transform.position);
        //    ////other.transform.position = transform.position + relativePosition.normalized;
        //    //other.transform.position += (targetPosition - other.transform.position) * Time.deltaTime;

        //    currentState = DraggingState.VERTICAL;
        //}
        //else if (other.name == "LeftCollider" || other.name == "RightCollider")
        //{
        //    currentState = DraggingState.HORIZONTAL;
        //}
        //else
        //{
        //    currentState = DraggingState.NONE;
        //}
    }

    //updated with on trigger stay
    void OnTriggerStay(Collider other)
    {
        if (currentState == DraggingState.NONE)
        {
            if (other.name == "TopCollider" && Input.GetButton("Interact") || other.name == "BottomCollider" && Input.GetButton("Interact"))
            {
                box = other.transform.parent.gameObject;
                relativePosition = box.transform.position - transform.position;
                Debug.Log("booya");
                if (relativePosition.magnitude < 2.0f)
                {
                    relativePosition.Normalize();
                    relativePosition *= 2.0f;
                }

                currentState = DraggingState.VERTICAL;
            }
            else if (other.name == "LeftCollider" && Input.GetButton("Interact") || other.name == "RightCollider" && Input.GetButton("Interact"))
            {
                box = other.transform.parent.gameObject;
                relativePosition = box.transform.position - transform.position;
                Debug.Log("booya");
                if (relativePosition.magnitude < 2.0f)
                {
                    relativePosition.Normalize();
                    relativePosition *= 2.0f;
                }

                currentState = DraggingState.HORIZONTAL;
            }
        }

        //execution of enemy
        if (other.gameObject.tag == "Enemy")
        {
            bool enemyDeadCheck = other.gameObject.GetComponent<TrooperBehaviour>().xIsDownedX;
            if (other.gameObject.tag == "Enemy" && Input.GetKeyDown(KeyCode.E) && enemyDeadCheck == true)
            {
                other.gameObject.GetComponent<TrooperBehaviour>().xIsDeadX = true;
            }
        }

        // Powercell pickup
        if (other.gameObject.tag == "PowerCell" && Input.GetKeyDown(KeyCode.E))
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
                Destroy(other.gameObject);
            }
        }

        // Door open -- this requires the panel object to have the tag 'Panel'
        if (other.gameObject.tag == "Panel" && Input.GetKeyDown(KeyCode.E))
        {

            //if the player has 1 or more power cells and the panel has not been activated before
            if (storedPowerCell >= 1 && !other.gameObject.GetComponent<Panel>().xActivatedX) // panel activatd = false
            {
                //open the door
                storedPowerCell--;

                //play sound effect of door opening

                //destroy the door
                other.gameObject.GetComponent<Panel>().xActivatedX = true;
            }
            else
            {
                //play sound effect of --NO--, DO NOT REMOVE FROM SCORE
            }
        }

        //health interact
        if (other.gameObject.tag == "Health" && Input.GetKeyDown(KeyCode.E))
        {
            //if the player has less than max health
            if (currentHealth < maxHealth)
            {

                //sets up the amount to heal
                int healthGained = other.gameObject.GetComponent<HealthPickup>().healthRestoreAmount;

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
                Destroy(other.gameObject);
            }
            else
            {
                //play sound effect of --NO--, DO NOT REMOVE FROM SCORE
            }
        }
    }
}