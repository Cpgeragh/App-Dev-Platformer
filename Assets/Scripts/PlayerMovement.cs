using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    // Existing variables
    Rigidbody2D rb;
    Animator animator;
    [SerializeField] Transform groundCheckCollider;
    [SerializeField] LayerMask groundLayer;
    const float groundCheckRadius = 0.2f;
    [SerializeField] float speed = 1;
    [SerializeField] float jumpPower = 500;
    float horizontalValue;
    float runSpeedModifier = 2f;
    [SerializeField] bool isGrounded;
    bool isRunning = false;
    bool facingRight = true;
    bool jump;
    bool hasDoubleJump = false;
    int jumpCount = 0;
    [SerializeField] float doubleJumpTimer = 30f; // Changed to 30 seconds
    float timer; // Current timer value
    bool isDead = false;
    [SerializeField] TMP_Text doubleJumpTimerText; // Reference to the UI Text element
    AudioSource audioSource; // Reference to the AudioSource component
    [SerializeField] private AudioClip damageSound; // Holds the damage sound effect
    [SerializeField] private AudioClip doubleJumpSound; // The sound effect for double jump

    // JetpackController variable
    [SerializeField] JetpackController jetpackController;
    [SerializeField] float jetpackForce = 10f; // You can adjust this force as necessary
    [SerializeField] private SpriteRenderer spriteRenderer; // Add the SpriteRenderer reference
    [SerializeField] private Sprite normalSprite; // Sprite for normal state
    [SerializeField] private Sprite damagedSprite; // Sprite for damaged state
    [SerializeField] private float damageDuration = 1.0f; // Time the damaged sprite is visible

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!CanMove())
        {
            return;
        }

        horizontalValue = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRunning = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded || (hasDoubleJump && doubleJumpTimer > 0f && jumpCount < 1))
            {
                animator.SetBool("Jump", true);
                jump = true;
                PlayJumpSound(); // Play the jump sound
            }
        }
        else if (Input.GetButtonUp("Jump"))
        {
            jump = false;
        }

        animator.SetFloat("yVelocity", rb.velocity.y);


        UpdateDoubleJumpTimer();
    }

    void FixedUpdate()
    {
        GroundCheck();
        Move(horizontalValue, ref jump);


        // Integrate the jetpack force application here
        if (jetpackController != null && jetpackController.IsUsingJetpack)
        {
            rb.AddForce(Vector2.up * jetpackController.JetpackForce, ForceMode2D.Force);
        }
    }

    void UpdateDoubleJumpTimer()
    {
        if (hasDoubleJump && doubleJumpTimer > 0f && doubleJumpTimerText != null)
        {
            doubleJumpTimer -= Time.deltaTime;
            doubleJumpTimerText.fontSize = 16; // Set the font size to 16
            doubleJumpTimerText.text = "Double Jump: " + doubleJumpTimer.ToString("F1"); // Update the timer text with the countdown

            if (doubleJumpTimer <= 0f)
            {
                hasDoubleJump = false;
                doubleJumpTimerText.enabled = false; // Disable the text when the timer expires
                Debug.Log("Double jump timer expired. Player lost double jump ability.");
            }
            else
            {
                // Change text color to red when the remaining time is 5 seconds or less
                if (doubleJumpTimer <= 5f)
                {
                    doubleJumpTimerText.color = Color.red;
                }
                else
                {
                    // Change text color to its original color (if it's not red)
                    doubleJumpTimerText.color = Color.white; // Or whatever your original color is
                }
            }
        }
    }

    void Move(float dir, ref bool jumpFlag)
    {
        if (isGrounded && jumpFlag)
        {
            rb.AddForce(new Vector2(0f, jumpPower));
            isGrounded = false;
            jumpFlag = false;
            jumpCount = 0;
            Debug.Log("Jumped from ground. Jump count reset to 0.");
        }
        else if (!isGrounded && jumpFlag)
        {
            if (hasDoubleJump && doubleJumpTimer > 0f && jumpCount < 1)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0f);
                rb.AddForce(new Vector2(0f, jumpPower));
                jumpFlag = false;
                jumpCount++;
                Debug.Log("Performed double jump. Jump count: " + jumpCount);
            }
        }

        float xValue = dir * speed * 100 * Time.fixedDeltaTime;

        if (isRunning)
        {
            xValue *= runSpeedModifier;
        }

        Vector2 targetVelocity = new Vector2(xValue, rb.velocity.y);
        rb.velocity = targetVelocity;

        animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));

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

    bool CanMove()
    {
        return !isDead;
    }

    void GroundCheck()
    {
        isGrounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius, groundLayer);
        if (colliders.Length > 0)
        {
            isGrounded = true;
        }

        animator.SetBool("Jump", !isGrounded);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DoubleJump"))
        {
            other.gameObject.SetActive(false);
            StartCoroutine(ReactivateFeatherAfterDelay(other.gameObject, 20f));
            hasDoubleJump = true;
            doubleJumpTimer = 30f; // Reset timer to initial value (30 seconds)
            PlayDoubleJumpSound(); // Play the double jump sound effect
            if (doubleJumpTimerText != null)
            {
                doubleJumpTimerText.enabled = true; // Enable the text when the feather is collected
            }


            Debug.Log("Player gained double jump ability. Timer started.");
        }
    }

    IEnumerator ReactivateFeatherAfterDelay(GameObject featherObject, float delay)
    {
        yield return new WaitForSeconds(delay);
        featherObject.SetActive(true);
        Debug.Log("Feather reactivated after delay.");
    }

    public void Die()
    {
        isDead = true;
        FindObjectOfType<LevelManager>().Respawn();
    }

    void PlayJumpSound()
    {
        if (audioSource != null)
        {
            audioSource.Play(); // Play the jump sound
        }
    }

    public void TakeDamage()
{
    Debug.Log("ShowDamageEffect called.");
    StartCoroutine(ShowDamageEffect());

    // Play the damage sound
    if (audioSource != null && damageSound != null)
    {
        audioSource.PlayOneShot(damageSound);
    }
}

    private IEnumerator ShowDamageEffect()
    {
        // Disable the animator to prevent it from changing the sprite.
    animator.enabled = false;
    spriteRenderer.sprite = damagedSprite;

    yield return new WaitForSeconds(damageDuration);

    // Re-enable the animator and revert to the previous sprite.
    spriteRenderer.sprite = normalSprite;
    animator.enabled = true;
    }

    private void PlayDoubleJumpSound()
    {
        if (audioSource != null && doubleJumpSound != null)
        {
            audioSource.PlayOneShot(doubleJumpSound);
        }
    }

}