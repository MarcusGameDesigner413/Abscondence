using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public int health = 100;
    public int maxHealth = 100;
    public float walkSpeed = 5;
    public float runSpeed = 10; // For Debug purposes [REMOVE IN ALPHA]
    public float turnSmoothTime = 0.1f;
    public float speedSmoothTime = 0.1f;
    public float invulnerabilityTime = 0.5f;
    public float knockBackForce;

    float turnSmoothVelocity;
    float speedSmoothVelocity;
    float currentSpeed;

    public GameObject player;
    public GameObject meleeWeapon;
    Animator meleeSwipe;

    private CapsuleCollider playerCollider;
    private Vector3 playerMoveDirection;
    private bool playerWasDamaged;
    private float timer = 0;
    public CharacterController controller;

    void Start()
    {
        meleeSwipe = meleeWeapon.GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Physics.IgnoreCollision(playerCollider, meleeWeapon.GetComponent<Collider>(), true);

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

        // Debug addition to get around faster
        bool running = Input.GetKey(KeyCode.LeftShift);
        // Set to walkSpeed in alpha test
        float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
        // Speed up the player overtime when they move
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        // Move the character relevant to the set current speed
        transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);

        // Make sure the health doesn't overcap
        if (health > maxHealth)
            health = maxHealth;

        PlayerTookDamage();

        // Check for animation plays
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


    void OnTriggerEnter(Collider other)
    {
        Vector3 playerHitDirection = other.transform.forward /*other.transform.position - transform.position*/;
        playerHitDirection = playerHitDirection.normalized;

        if (other.tag == "Enemy")
        {
            playerWasDamaged = true;
            KnockBack(playerHitDirection);
            health -= 10;
        }

        // ***W.I.P***
        // MAKE IT SO THE SWORD COLLIDER BOX IGNORES THE PLAYER COLLIDER BOX 
        // (No rigidbody currently applied to player cause of this issue)
        //if (other.gameObject.tag == "Sword")
        //{
        //Debug.Log("Collision ignored");
        //}
    }

    void PlayerTookDamage()
    {
        // If the enemy took damage turn off the box collider
        if (playerWasDamaged)
        {
            PlayerInvulnerabilityOn();
            timer += Time.deltaTime;
        }
        //else
        //{
        //    playerRigidBody.Sleep();
        //}

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
        playerMoveDirection = direction * knockBackForce;

        controller.Move(playerMoveDirection * Time.deltaTime);
        playerMoveDirection = Vector3.Lerp(playerMoveDirection, Vector3.zero, 5 * Time.deltaTime);
    }
}