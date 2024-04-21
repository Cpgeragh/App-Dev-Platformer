using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private AudioClip scoreSound; // The sound effect for scoring
    private AudioSource audioSource; // AudioSource component reference

    public int Score { get; private set; }
    public GameObject LevelExitBlocker; // Drag your exit block here through the inspector

    private void Start()
    {
        Score = 0;
        audioSource = GetComponent<AudioSource>(); // Ensure there's an AudioSource component attached
    }

    public void AddScore(int amount)
    {
        Score += amount;

        // Play the scoring sound effect
        if (audioSource != null && scoreSound != null)
        {
            audioSource.PlayOneShot(scoreSound);
        }

        if (Score >= 5) // Assuming you need 5 points to open the exit
        {
            LevelExitBlocker.SetActive(false); // Deactivate the exit block
        }
    }
}