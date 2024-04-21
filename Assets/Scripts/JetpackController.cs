using UnityEngine;
using TMPro;
using System.Collections;

public class JetpackController : MonoBehaviour
{
    [SerializeField] private float jetpackForce = 10f;
    [SerializeField] private float jetpackDuration = 30f;
    [SerializeField] private TMP_Text flyCountdownText;
    [SerializeField] private AudioClip jetpackSound;  // The sound effect for the jetpack

    private AudioSource audioSource;  // AudioSource component reference
    private float jetpackTimeRemaining;
    private bool canUseJetpack = true;
    private bool isUsingJetpack;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();  // Get the AudioSource component
        jetpackTimeRemaining = jetpackDuration;
        UpdateFlyCountdownUI();
    }

    void Update()
    {
        if (canUseJetpack && Input.GetKey(KeyCode.W))
        {
            if (!isUsingJetpack)  // If the jetpack wasn't being used, start the sound
            {
                audioSource.clip = jetpackSound;
                audioSource.loop = true;
                audioSource.Play();
            }
            isUsingJetpack = true;
            jetpackTimeRemaining -= Time.deltaTime;
            UpdateFlyCountdownUI();

            if (jetpackTimeRemaining <= 0)
            {
                canUseJetpack = false;
                isUsingJetpack = false;
                audioSource.Stop();  // Stop the sound when the jetpack can no longer be used
                StartCoroutine(HideFuelEmptyText());
            }
        }
        else if (isUsingJetpack)
        {
            isUsingJetpack = false;
            audioSource.Stop();  // Stop the sound if W is released
        }
    }

    public bool IsUsingJetpack => canUseJetpack && isUsingJetpack;
    public float JetpackForce => jetpackForce;

    private void UpdateFlyCountdownUI()
    {
        if (flyCountdownText != null)
        {
            if (jetpackTimeRemaining > 0)
            {
                flyCountdownText.fontSize = 16;
                flyCountdownText.color = jetpackTimeRemaining <= 5f ? Color.red : Color.white;
                flyCountdownText.text = "Fuel Remaining: " + jetpackTimeRemaining.ToString("F1");
            }
            else if (canUseJetpack) // Ensure this block only runs once when time first runs out
            {
                canUseJetpack = false;
                flyCountdownText.text = "Fuel Empty!";
                flyCountdownText.color = Color.white;
            }
        }
    }

    IEnumerator HideFuelEmptyText()
    {
        yield return new WaitForSeconds(5);
        if (flyCountdownText != null)
        {
            flyCountdownText.text = ""; // Clear the text
        }
    }
}