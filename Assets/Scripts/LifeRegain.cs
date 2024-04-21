using UnityEngine;

public class LifeRegain : MonoBehaviour
{
    public HeartCount heartCount;  // Reference to the HeartCount script on the player

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collider belongs to the player
        if (collision.CompareTag("PlayerPig"))
        {
            Debug.Log("Player has picked up a life.");
            // Call the RegainHeart method of the HeartCount script to regain a heart
            heartCount.RegainHeart();
            // Optionally destroy the life pickup object if it should only be used once
            Destroy(gameObject);
        }
    }
}