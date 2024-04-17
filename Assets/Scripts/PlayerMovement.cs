
using System.Collections;

using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    
    // Private Variables
    Rigidbody2D rb;
    Animator animator;

    [SerializeField] Transform groundCheckCollider;
    [SerializeField] LayerMask groundLayer;
    const float groundCheckRadius = 0.2f;
    [SerializeField] float speed = 1;
    [SerializeField] float jumpPower = 500;
    float horizontalValue;
    float runSpreedModifier = 2f;
    [SerializeField] bool isGrounded;
    bool isRunning = false;
    bool facingRight = true; 
    bool jump;
    bool hasDoubleJump = false;
    int jumpCount = 0;
    [SerializeField] float doubleJumpTimer = 20f; // Initial timer value in seconds
    float timer; // Current timer value
    bool isDead = false;

    
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if(CanMove() == false)
        {

            return;

        }

        horizontalValue = Input.GetAxisRaw("Horizontal");
        // Debug.Log(HorizontalValue);

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {

            isRunning = true;

        }

        if(Input.GetKeyUp(KeyCode.LeftShift))
        {

            isRunning = false;

        }

        if(Input.GetButtonDown("Jump"))
        {

            animator.SetBool("Jump", true);
            jump = true;

        }

        else if(Input.GetButtonUp("Jump"))
        {

            jump = false;

        }

        animator.SetFloat("yVelocity", rb.velocity.y);

        UpdateDoubleJumpTimer();
        
    }

    void UpdateDoubleJumpTimer()
{
    // Check if the player has double jump ability and is currently counting down
    if (hasDoubleJump && doubleJumpTimer > 0f)
    {
        // Decrement the timer
        doubleJumpTimer -= Time.deltaTime;

        // Log timer value (for debugging)
        Debug.Log("Double jump timer: " + doubleJumpTimer.ToString("F1"));

        // Check if timer has expired
        if (doubleJumpTimer <= 0f)
        {
            // Reset the double jump ability state
            hasDoubleJump = false;
            Debug.Log("Double jump timer expired. Player lost double jump ability.");
        }
    }

}

    void FixedUpdate()
    {

        GroundCheck();
        Move(horizontalValue, ref jump);
        
    }

    bool CanMove()
    {

        bool can = true;

        if(isDead)
        {

            can = false;

        }

        return can;

    }

    public void Die()
    {

        isDead = true;
        FindObjectOfType<LevelManager>().Respawn();

    }
    

    void GroundCheck()
    {

        isGrounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius, groundLayer);
        if(colliders.Length > 0)
        {

            isGrounded = true;

        }

        animator.SetBool("Jump", !isGrounded);

    }

   void Move(float dir, ref bool jumpFlag)
{
    if (isGrounded && jumpFlag)
    {
        // Perform the initial jump
        rb.AddForce(new Vector2(0f, jumpPower));
        isGrounded = false; // Player is no longer grounded after jumping
        jumpFlag = false;   // Reset jump flag
        jumpCount = 0;
        Debug.Log("Jumped from ground. Jump count reset to 0.");
    }
    else if (!isGrounded && jumpFlag)
{
    if (hasDoubleJump && doubleJumpTimer > 0f && jumpCount < 1)
    {
        // Perform double jump
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(new Vector2(0f, jumpPower));
        jumpFlag = false;
        jumpCount++;
        Debug.Log("Performed double jump. Jump count: " + jumpCount);
    }
}

    // Calculate horizontal movement based on input direction and speed
    float xValue = dir * speed * 100 * Time.fixedDeltaTime;

    // Apply speed modifier if running
    if (isRunning)
    {
        xValue *= runSpreedModifier;
    }

    // Set the new target velocity for horizontal movement
    Vector2 targetVelocity = new Vector2(xValue, rb.velocity.y);
    rb.velocity = targetVelocity;

    // Update the animator parameter based on horizontal velocity
    animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));

    // Flip character sprite based on movement direction
    if (facingRight && dir < 0)
    {
        transform.localScale = new Vector3(1, 1, 1);
        facingRight = false;
    }
    else if (!facingRight && dir > 0)
    {
        transform.localScale = new Vector3(-1, 1, 1);
        facingRight = true;
    }
}

    void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("DoubleJump"))
    {
        // Perform actions when player collides with the DoubleJump object
        Debug.Log("Player collided with DoubleJump object!");

        // Deactivate the DoubleJump object after collision
        other.gameObject.SetActive(false);

        // Start a coroutine to reactivate the DoubleJump object after 20 seconds
        StartCoroutine(ReactivateFeatherAfterDelay(other.gameObject, 20f));

        // Perform additional actions (e.g., grant double jump ability to player)
        // Example: playerController.EnableDoubleJump();

        hasDoubleJump = true;

        doubleJumpTimer = 20f; // Reset timer to initial value (20 seconds)
        Debug.Log("Player gained double jump ability. Timer started.");
    }
}

// Coroutine to reactivate the feather object after a delay
private IEnumerator ReactivateFeatherAfterDelay(GameObject featherObject, float delay)
{
    yield return new WaitForSeconds(delay);

    // Reactivate the feather object after the delay
    featherObject.SetActive(true);
    Debug.Log("Feather reactivated after delay.");
}
    
    
}
