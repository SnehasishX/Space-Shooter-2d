using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviour, IPunObservable
{
    public float moveSpeed = 5f;
    public Transform followTarget; // Optional target to follow (can be null)
    public GameObject crosshairPrefab; // 🎯 Crosshair prefab

    private PhotonView photonView;
    private Rigidbody2D rb;
    private GameObject crosshair;

    private Vector2 networkPosition;
    private Vector2 networkCursorPosition;
    private float lerpSpeed = 10f;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();

        if (photonView.IsMine)
        {
            Cursor.visible = false; // 🔥 Hide system cursor
            crosshair = Instantiate(crosshairPrefab);
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            HandleMovement();
            HandleRotation();
            UpdateCrosshair();
        }
        else
        {
            // 🔥 Smooth movement & rotation interpolation for remote players
            transform.position = Vector2.Lerp(transform.position, networkPosition, Time.deltaTime * lerpSpeed);

            Vector2 direction = (networkCursorPosition - (Vector2)transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle), Time.deltaTime * lerpSpeed);
        }
    }

    void HandleMovement()
    {
        if (followTarget != null)
        {
            // 🔥 Move towards the follow target smoothly
            transform.position = Vector2.MoveTowards(transform.position, followTarget.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            // 🔥 Normal movement with WASD
            float moveX = Input.GetAxis("Horizontal");
            float moveY = Input.GetAxis("Vertical");
            Vector2 movement = new Vector2(moveX, moveY) * moveSpeed;
            rb.velocity = movement;
        }
    }

    void HandleRotation()
    {
        if (!photonView.IsMine) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // Ensure it's on the same 2D plane
        networkCursorPosition = mousePos;

        // 🔥 Rotate player towards the mouse
        Vector2 direction = (mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void UpdateCrosshair()
    {
        if (crosshair)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0; // Ensure it's on the same 2D plane

            // 🎯 Move crosshair smoothly to the cursor position
            crosshair.transform.position = Vector3.Lerp(crosshair.transform.position, mousePos, Time.deltaTime * 20f);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext((Vector2)transform.position);
            stream.SendNext((Vector2)networkCursorPosition);
        }
        else
        {
            networkPosition = (Vector2)stream.ReceiveNext();
            networkCursorPosition = (Vector2)stream.ReceiveNext();
        }
    }
}
