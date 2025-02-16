using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of the player
    private Vector2 moveInput;

    void Update()
    {
        // Movement Input
        moveInput.x = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow
        moveInput.y = Input.GetAxis("Vertical");   // W/S or Up/Down Arrow

        // Move the spaceship
        transform.position += (Vector3)moveInput * moveSpeed * Time.deltaTime;

        // Rotate spaceship to face the mouse cursor
        RotateTowardsMouse();
    }

    void RotateTowardsMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
