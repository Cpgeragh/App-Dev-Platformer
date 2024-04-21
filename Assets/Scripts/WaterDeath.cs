using UnityEngine;

public class WaterDeath : MonoBehaviour
{
    // Reference to the HeartCount script
    public HeartCount heartCount;

    // Called when a collider enters this trigger zone
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider is the player character
        if (other.CompareTag("PlayerPig"))
        {
            Debug.Log("Player collided with water death object."); // Debug statement
            // Call the LoseFourHearts method of the HeartCount script
            heartCount.LoseFourHearts();
        }
    }
}