using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 20f;  // Speed for panning with keyboard

    [Header("Mouse Look Settings")]
    public float rotationSpeed = 5f;  // Sensitivity for mouse-based rotation
    private float yaw = 0f;
    private float pitch = 0f;

    [Header("Scroll Movement Settings")]
    public float scrollSpeed = 10f;  // Speed for moving forward/backward with the mouse wheel

    void Start()
    {
        // Initialize rotation based on current camera orientation.
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;
    }

    void Update()
    {
        // ============
        // Keyboard Panning
        // ============
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = (transform.right * h + transform.up * v) * moveSpeed * Time.deltaTime;
        transform.position += move;

        // ============
        // Mouse Look (Right Mouse Button Held)
        // ============
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            yaw += mouseX * rotationSpeed;
            pitch -= mouseY * rotationSpeed;
            pitch = Mathf.Clamp(pitch, -80f, 80f);

            transform.eulerAngles = new Vector3(pitch, yaw, 0f);
        }

        // ============
        // Mouse Scroll Wheel Movement
        // ============
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.001f)
        {
            // Move the camera forward or backward along its forward vector.
            Vector3 forwardMove = transform.forward * scroll * scrollSpeed;
            transform.position += forwardMove;
        }
    }
}