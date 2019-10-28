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
    public int maxPowerCell = 3;
    public int storedDetPack = 0;
    public int maxDetPack = 4;
    public int storedMedvial = 0;
    public int maxMedvial = 6;
    public GameObject meleeWeapon;
    public string MainMenuName;
    public bool DeathToMenu = false;

    float turnSmoothVelocity;
    float speedSmoothVelocity;
    float currentSpeed;

    Animator meleeSwipe;
    GameObject box;

    private CharacterController controller;
    private CapsuleCollider playerCollider;
    private AITeleport aiFollower;
    private Vector3 playerMoveDirection;
    private bool playerWasDamaged;
    private float timer = 0;
    private float fallAmount;
    private Vector3 velocity;
    private Vector3 gravity;
    private BottomlessPit ifFallen;
    private Vector2 input;
    private Vector3 relativePosition;

    [HideInInspector]
    public bool hasKey = false;
    [HideInInspector]
    public bool hasUniqueKey = false;
    [HideInInspector]
    public int keyType = 0;
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

    private float healthVialTimer;
    private bool healthPickedUp;

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
        aiFollower = GameObject.Find("AIFollow").GetComponent<AITeleport>();
        startingHeight = transform.position.y;
        fallAmount = startingHeight + 5.0f;
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
                    controller.Move((transform.forward * currentSpeed) * Time.deltaTime);
            }
            else
            {
                Vector3 dir = new Vector3(inputDir.x, 0, inputDir.y);

                if (!movementDisabled/* && !ifFallen*/)
                    controller.Move((dir * currentSpeed) * Time.deltaTime);

            }
            //}
        }

        controller.Move(velocity * Time.deltaTime);

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
        else if (currentHealth <= 0)
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

        // If the player falls below a certain Y level, it will teleport to the AIFollower and take damage
        if (transform.position.y < -fallAmount)
        {
            Debug.Log("Player fallen off map");
            TeleportToAI();
            currentHealth -= 2; // Player takes damage upon falling into hole
            //playerFallen = false;
        }

        // Check if player took damage
        PlayerTookDamage();

        // Check for animation plays
        PlayLightAnimation();
        PlayHeavyAnimation();

        if (currentHealth == 0 && DeathToMenu == true)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(MainMenuName);
            //UnityEngine.SceneManagement.SceneManager.LoadScene("Main_Menu");
        }

        //Box dragging
        if (Input.GetButtonUp("Interact"))
        {
            currentState = DraggingState.NONE;
            //Set velocity to zero
            
            //set box to null
            box = null;
        }

        if(Input.GetButtonDown("Medvial") && storedMedvial > 0 && currentHealth != maxHealth)
        {
            currentHealth++;
            storedMedvial--;
        }
        //Debug.DrawLine(transform.position + new Vector3(-0.5f, 5.0f, 0.0f), transform.position + new Vector3(0.5f, 5.0f, 0.0f), Color.red);

        healthVialTimer -= Time.deltaTime;
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
        if (Input.GetButtonDown("LightAttack") && !gamePaused)
        {
            lightAttackUsed = true;
            heavyAttackUsed = false;
            meleeSwipe.SetTrigger("ActiveLClick");
        }
    }

    public void PlayHeavyAnimation()
    {
        if (Input.GetButtonDown("SpinAttack") && !gamePaused)
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
            if(enemy.xIsDownedX == false)
            {
                playerWasDamaged = true;
                KnockBack(playerHitDirection);
                currentHealth -= enemy.enemyAttackStrength;
            }
            
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

    //updated with on trigger stay
    void OnTriggerStay(Collider other)
    {
        // Block Dragging
        if (currentState == DraggingState.NONE)
        {
            if (other.name == "TopCollider" && Input.GetButton("Interact") || other.name == "BottomCollider" && Input.GetButton("Interact"))
            {
                box = other.transform.parent.gameObject;
                relativePosition = box.transform.position - transform.position;
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
            if (other.gameObject.tag == "Enemy" && Input.GetButtonDown("Interact") && enemyDeadCheck == true)
            {
                other.gameObject.GetComponent<TrooperBehaviour>().xIsDeadX = true;
            }
        }

        // Powercell pickup
        if (other.gameObject.tag == "PowerCell" && Input.GetButtonDown("Interact"))
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
        if (other.gameObject.tag == "Panel" && Input.GetButtonDown("Interact"))
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
        if (other.gameObject.tag == "Health" && Input.GetButtonDown("Interact"))
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

        if (other.gameObject.tag == "Card" && Input.GetButtonDown("Interact"))
        {
            if(other.gameObject.GetComponent<KeyCard>().CurrentLevel == 6)
            {
                hasUniqueKey = true;
            }
            else
            {
                keyType = other.gameObject.GetComponent<KeyCard>().CurrentLevel;
                //update the score
                hasKey = true;
            }

            //delete the card
            Destroy(other.gameObject);
            
        }

        if (other.gameObject.tag == "CardPanel" && Input.GetButtonDown("Interact"))
        {
            //if the panel requires the master and the player has the master AND has not been activated 
            if (other.gameObject.GetComponent<CardPanel>().requiresMaster && hasUniqueKey == true
                && !other.gameObject.GetComponent<CardPanel>().xActivatedX)
            {
                other.gameObject.GetComponent<CardPanel>().xActivatedX = true;
            }


            //if the player has the key, the panel has not been activated before AND does not need the master
            if (hasKey == true && !other.gameObject.GetComponent<CardPanel>().xActivatedX
                && !other.gameObject.GetComponent<CardPanel>().requiresMaster)
            {
                //play sound effect of door opening

                //destroy the door
                other.gameObject.GetComponent<CardPanel>().xActivatedX = true;
            }

            //the "else" that goes after as it is denied
            if (!other.gameObject.GetComponent<CardPanel>().xActivatedX)
            {
                //play sound effect of --NO--, DO NOT REMOVE FROM SCORE
            }
        }

            // Detpack pickup
        if (other.gameObject.tag == "DetPack" && Input.GetButtonDown("Interact"))
        {
            if (storedDetPack >= maxDetPack)
            {
                //play sound of --NO--, DO NOT ADD to SCORE
            }
            else
            {
                //update the score
                storedDetPack++;

                //delete the power cell
                Destroy(other.gameObject);
            }
        }

        // Health Station
        if (other.gameObject.tag == "HealthStation" && Input.GetButtonDown("Interact") || healthPickedUp)
        {
            if (storedMedvial >= maxMedvial || (!other.GetComponent<HealthStation>().activeState && !healthPickedUp))
            {
                //play sound of --NO--, DO NOT ADD to SCORE
            }
            else
            {
                //update the score
                if (!healthPickedUp)
                {
                    healthPickedUp = true;
                    healthVialTimer = 1.5f;
                }

                if (healthVialTimer <= 0)
                {
                    storedMedvial += Random.Range(other.GetComponent<HealthStation>().minMedvialOutput, other.GetComponent<HealthStation>().maxMedvialOutput);
                    healthPickedUp = false;
                    Debug.Log("Help");
                    if (storedMedvial > maxMedvial)
                        storedMedvial = maxMedvial;
                }
                other.GetComponent<Animator>().SetTrigger("Activated");
                other.GetComponent<HealthStation>().activeState = false;
                //play animation of healthstation deactivating
            }
        }

        //rad jammer gonna block yo screen unless you destroy it with a detpack
        if (other.gameObject.tag == "Jammer" && Input.GetButtonDown("Interact"))
        {
            //got more than 1 detpack, good, now make it go boom
            if (storedDetPack >= 1 && other.gameObject.GetComponent<Jammer>().isJamming)
            {
                other.gameObject.GetComponent<Jammer>().isJamming = false;

                storedDetPack--;
            }
        }

        if (other.gameObject.tag == "WallDestroy" && Input.GetButtonDown("Interact"))
        {
            //got more than 1 detpack, good, now make it go boom
            if (storedDetPack >= 1 && !other.gameObject.GetComponent<WeakWallDestroy>().isGonnaBlow)
            {
                other.gameObject.GetComponent<WeakWallDestroy>().isGonnaBlow = true;

                storedDetPack--;
            }
        }
    }

    public void TeleportToAI()
    {
        controller.enabled = false;
        controller.transform.SetPositionAndRotation(aiFollower.transform.position, aiFollower.transform.rotation);
        controller.enabled = true;
    }
}