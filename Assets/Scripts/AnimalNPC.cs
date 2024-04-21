using UnityEngine;

public class AnimalNPC : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider that entered the trigger has the correct tag
        if (other.CompareTag("PlayerPig")) // Make sure the player GameObject is tagged with "PlayerPig"
        {
            Debug.Log("PlayerPig has collected the animal!");

            // Increment the number of animals rescued
            GameManager.instance.IncrementAnimalCount();

            // Deactivate the animal object
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Something else collided with the animal, not the PlayerPig.");
        }
    }
}