using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1; // Damage inflicted by the bullet

    // Start is called before the first frame update
    void Start()
    {
        // Start the coroutine to destroy the bullet after 2 seconds
        StartCoroutine(DestroyAfterDelay(2f));
    }

    // Coroutine to destroy the bullet after a delay
    private System.Collections.IEnumerator DestroyAfterDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Destroy the bullet GameObject
        Destroy(gameObject);
    }

    // This method is called when the Collider2D other enters the trigger
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the bullet collides with any object (except triggers)
        // You can add additional conditions here if needed
        if (!collision.collider.isTrigger)
        {
            // Check if the bullet collides with the player
            if (collision.gameObject.CompareTag("PlayerPig"))
            {
                // Reduce the player's hearts
                HeartCount heartCount = collision.gameObject.GetComponent<HeartCount>();
                if (heartCount != null)
                {
                    heartCount.LoseHearts(damage); // Apply damage to the player
                }
            }

            // Destroy the bullet GameObject when it collides with something
            Destroy(gameObject);
        }
    }

}