using UnityEngine;
using UnityEngine.UI;

public class HeartCount : MonoBehaviour
{
    public Image[] hearts;
    public int heartsRemaining;

    private bool isInvincible = false;
    private float invincibilityTimer = 0f;

    public void StartInvincibility(float duration)
    {
        isInvincible = true;
        invincibilityTimer = duration;
    }

    private void Update()
    {
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0)
            {
                isInvincible = false;
            }
        }
    }

    // Lose one heart
    public void LoseHeart()
{
    if (!isInvincible && heartsRemaining > 0)
    {
        heartsRemaining--;
        hearts[heartsRemaining].enabled = false;

        // Notify the PlayerMovement script to show damage effect
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        if (player != null)
        {
            player.TakeDamage();
        }

        if (heartsRemaining <= 0)
        {
            Debug.Log("YOU LOSE");
            // Since Die method already respawns the player, no need to call Respawn again
            player.Die();
        }
    }
}

    // Lose four hearts (specifically for water death)
    public void LoseFourHearts()
    {
        if (!isInvincible && heartsRemaining >= 4)
        {
            heartsRemaining -= 4;
        }
        else if (!isInvincible)
        {
            heartsRemaining = 0;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < heartsRemaining)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }

        if (!isInvincible && heartsRemaining <= 0)
        {
            FindObjectOfType<PlayerMovement>().Die();
            FindObjectOfType<LevelManager>().Respawn();
            Debug.Log("YOU LOSE");
        }
    }

    // Lose hearts (variable amount)
    public void LoseHearts(int heartsToLose)
    {
        if (!isInvincible)
        {
            for (int i = 0; i < heartsToLose; i++)
            {
                LoseHeart();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Invincibility"))
        {
            // Start invincibility
            StartInvincibility(20f);
            // Remove the invincibility game object
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            if (!isInvincible)
            {
                LoseHeart();
            }
        }
    }
    public void RegainHeart()
{
    if (heartsRemaining < hearts.Length)
    {
        // Make the next disabled heart icon visible again
        hearts[heartsRemaining].enabled = true;
        heartsRemaining++;  // Increment the number of hearts remaining
    }
}
}