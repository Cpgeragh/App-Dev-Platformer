using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public TMP_Text animalScoreText;
    public GameObject levelExitBlocker;
    public AudioClip rescueSound; // The sound effect for rescuing an animal
    private AudioSource audioSource; // AudioSource component reference
    
    private int animalsRescued = 0;
    private int totalAnimals = 5;

     private void Awake()
    {
        // Ensure only one instance of the GameManager exists
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
        UpdateAnimalScoreText();
    }

    public void IncrementAnimalCount()
    {
        animalsRescued++;
        UpdateAnimalScoreText();
        CheckLevelExit();
        
        // Play rescue sound effect
        if (audioSource != null && rescueSound != null)
        {
            audioSource.PlayOneShot(rescueSound);
        }
    }


    // Method to update the animal score counter text
    private void UpdateAnimalScoreText()
    {
        if (animalScoreText != null)
        {
            animalScoreText.fontSize = 16; // Set the font size to 16
            // Update the text to display the current score and the total number of animals
            animalScoreText.text = "Animals Rescued: " + animalsRescued.ToString() + "/" + totalAnimals.ToString();
        }
    }

    // Method to check if the level exit conditions are met
    private void CheckLevelExit()
    {
        if (animalsRescued >= totalAnimals && levelExitBlocker != null)
        {
            levelExitBlocker.SetActive(false); // Deactivate the level exit blocker if all animals are rescued
        }
    }
}