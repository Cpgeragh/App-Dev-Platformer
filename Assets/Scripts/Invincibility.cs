using System.Collections;
using UnityEngine;
using TMPro;

public class Invincibility : MonoBehaviour
{
    public float invincibilityDuration = 20f;
    public TextMeshProUGUI invincibilityCountdownText;
    public AudioClip invincibilitySound;  // Audio clip to play when invincibility is activated

    private AudioSource audioSource;  // Reference to the AudioSource component
    private float currentInvincibilityTime;
    private SpriteRenderer spriteRenderer;
    private Collider2D collider2D;
    private bool isInvincible;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();  // Get the AudioSource component
        invincibilityCountdownText.enabled = false;  // Initially disable the text
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerPig"))
        {
            HeartCount heartCount = other.GetComponent<HeartCount>();
            if (heartCount != null)
            {
                currentInvincibilityTime = invincibilityDuration;
                isInvincible = true;
                invincibilityCountdownText.enabled = true;
                Debug.Log($"Invincibility activated. Current Invincibility Time set to: {currentInvincibilityTime}");
                heartCount.StartInvincibility(invincibilityDuration);
                PlayInvincibilitySound();
                DisablePowerUp();
                StartCoroutine(RespawnPowerUpCoroutine(invincibilityDuration));
            }
            else
            {
                Debug.LogError("HeartCount component not found on the PlayerPig object.");
            }
        }
    }

    private void PlayInvincibilitySound()
    {
        if (audioSource != null && invincibilitySound != null)
        {
            audioSource.PlayOneShot(invincibilitySound);
        }
    }

    private void Update()
    {
        if (isInvincible)
        {
            currentInvincibilityTime -= Time.deltaTime;
            UpdateInvincibilityCountdownUI();
            if (currentInvincibilityTime <= 0)
            {
                isInvincible = false;
                invincibilityCountdownText.enabled = false;
            }
        }
    }

    private void UpdateInvincibilityCountdownUI()
    {
        invincibilityCountdownText.fontSize = 16;
        invincibilityCountdownText.text = "Invincibility: " + Mathf.CeilToInt(currentInvincibilityTime).ToString();
    }

    private void DisablePowerUp()
    {
        spriteRenderer.enabled = false;
        collider2D.enabled = false;
        Debug.Log("Invincibility power-up is disabled. It will respawn after " + invincibilityDuration + " seconds.");
    }

    private IEnumerator RespawnPowerUpCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetPowerUp();
    }

    private void ResetPowerUp()
    {
        spriteRenderer.enabled = true;
        collider2D.enabled = true;
        Debug.Log("Invincibility power-up has respawned.");
    }
}