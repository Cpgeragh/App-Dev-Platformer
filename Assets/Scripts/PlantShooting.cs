using UnityEngine;

public class PlantShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float shootCooldown = 1f;
    public float bulletSpeed = 10f;
    public int bulletSpawnFrame = 47; // Frame to spawn the bullet
    public float animationFrameRate = 83f; // Frame rate of the animation
    private float shootTimer = 0f;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        shootTimer += Time.deltaTime;

        if (shootTimer >= shootCooldown)
        {
            StartCoroutine(ShootAtAnimationFrame());
            shootTimer = 0f;
        }
    }

    private System.Collections.IEnumerator ShootAtAnimationFrame()
{
    // Calculate the time duration for the desired animation frame
    float frameDuration = bulletSpawnFrame / animationFrameRate;

    // Wait for the calculated time duration
    yield return new WaitForSeconds(frameDuration);

    // Instantiate the bullet prefab at the shoot point
    GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
    Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

    if (bulletRb != null)
    {
        // Determine the direction the plant is facing
        Vector2 plantDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        bulletRb.velocity = plantDirection * bulletSpeed;
    }

    // Play shooting animation
    if (animator != null)
    {
        animator.SetTrigger("Shoot");
    }

    // Deactivate the bullet after a certain duration
    yield return new WaitForSeconds(2f); // Adjust the duration as needed

    // Check if the bullet still exists before trying to deactivate it
    if (bullet != null && bullet.activeSelf)
    {
        bullet.SetActive(false);
    }
}
    private void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("PlayerPig"))
    {
        Debug.Log("Plant collided with player."); // Add a debug log statement

        HeartCount heartCount = other.GetComponent<HeartCount>();
        if (heartCount != null)
        {
            heartCount.LoseHeart();
        }
    }
}
}