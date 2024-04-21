using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    public string nextLevelName = "Level2"; // The name of the next level scene

    private void OnTriggerEnter2D(Collider2D other)
{
    Debug.Log("Trigger Entered"); // This should show in the console when the trigger is entered.

    if (other.CompareTag("PlayerPig")) // Make sure the player has the "PlayerPig" tag
    {
        Debug.Log("PlayerPig tagged object has collided with the exit. Loading next level: " + nextLevelName);
        SceneManager.LoadScene(nextLevelName); // Load Level 2
    }
}
}