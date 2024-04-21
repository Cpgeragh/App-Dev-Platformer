using UnityEngine;

public class WaterColumnInteraction : MonoBehaviour
{
    public HeartCount heartCount;  // Reference to the HeartCount script on the player

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collider belongs to the player
        if (collision.CompareTag("PlayerPig"))
        {
            Debug.Log("Player has collided with a water column.");
            // Trigger the heart loss; you can set this to lose any number of hearts
            heartCount.LoseHearts(1);  // This calls to lose 1 heart, adjust as necessary
        }
    }
}