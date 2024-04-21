using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform target;          // Target to follow
    public Vector3 offset;            // Offset from the target
    [Range(1, 10)]
    public float smoothFactor;        // Smooth motion factor

    private void FixedUpdate()
    {
        Follow();
    }

    void Follow()
    {
        // Compute the target position based on the target's position and the offset
        Vector3 targetPosition = target.position + offset;

        // Only use the target position's x and y, preserving the current z position of the camera
        Vector3 smoothPosition = Vector3.Lerp(new Vector3(transform.position.x, transform.position.y, transform.position.z), 
                                              new Vector3(targetPosition.x, targetPosition.y, transform.position.z), 
                                              smoothFactor * Time.fixedDeltaTime);

        // Update the transform's position
        transform.position = smoothPosition;
    }
}