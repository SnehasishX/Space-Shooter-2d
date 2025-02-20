using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviour, IPunObservable
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 200f; // 🔥 Adjust for smooth aiming
    public Camera playerCamera;
    // public GameObject crosshairPrefab; // 🔥 Custom crosshair

    private PhotonView photonView;
    private Rigidbody2D rb;
    private GameObject crosshair;

    private Vector2 networkPosition;
    private float networkRotationZ;
    private float lerpSpeed = 10f;

    private Vector3 mouseDelta; // 🔥 Smooth mouse movement tracking

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();

        if (photonView.IsMine)
        {
            Cursor.visible = false; // 🔥 Hide the system cursor
            // crosshair = Instantiate(crosshairPrefab); // Create crosshair
        }
        else
        {
            playerCamera.gameObject.SetActive(false);
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
            transform.position = Vector2.Lerp(transform.position, networkPosition, Time.deltaTime * lerpSpeed);
            Quaternion targetRotation = Quaternion.Euler(0, 0, networkRotationZ);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * lerpSpeed);
        }
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(moveX, moveY) * moveSpeed;
        rb.velocity = movement;
    }

    void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

        // 🔥 Smooth mouse movement using interpolation
        mouseDelta = Vector3.Lerp(mouseDelta, new Vector3(mouseX, mouseY, 0), Time.deltaTime * 10f);
        
        float newRotationZ = transform.rotation.eulerAngles.z - mouseDelta.x;
        transform.rotation = Quaternion.Euler(0, 0, newRotationZ);
    }

    void UpdateCrosshair()
    {
        if (crosshair)
        {
            Vector3 mousePos = playerCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            crosshair.transform.position = mousePos;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext((Vector2)transform.position);
            stream.SendNext(transform.rotation.eulerAngles.z);
        }
        else
        {
            networkPosition = (Vector2)stream.ReceiveNext();
            networkRotationZ = (float)stream.ReceiveNext();
        }
    }
}
