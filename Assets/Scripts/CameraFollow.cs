using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Assign the Player's Transform in Inspector
    public float smoothSpeed = 5f; // Adjust for slower or faster follow
    public Vector3 offset = new Vector3(0, 0, -10f); // Keep camera behind

    void LateUpdate()
    {
        if (player == null) return; // Prevent errors if player is missing

        // Smoothly interpolate between current position and player's position
        Vector3 targetPosition = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}
