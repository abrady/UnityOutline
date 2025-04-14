using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 20f;  // Speed for panning with keyboard

    [Header("Mouse Look Settings")]
    public float rotationSpeed = 5f;  // Sensitivity for mouse-based rotation
    private float yaw = 0f;
    private float pitch = 0f;

    [Header("Zoom Settings")]
    public float zoomSpeed = 2f;      // Sensitivity for scroll wheel zooming
    public float minFOV = 15f;        // Minimum field of view for perspective camera
    public float maxFOV = 90f;        // Maximum field of view for perspective camera
    public float minOrthoSize = 1f;   // Minimum size for orthographic camera
    public float maxOrthoSize = 20f;  // Maximum size for orthographic camera

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
        // Get input from Horizontal (A/D or left/right arrows) and Vertical (W/S or up/down arrows)
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Move the camera along its local right and up vectors.
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
            // Clamp pitch to avoid flipping the camera too far vertically.
            pitch = Mathf.Clamp(pitch, -80f, 80f);

            transform.eulerAngles = new Vector3(pitch, yaw, 0f);
        }

        // ============
        // Mouse Scroll Wheel Zoom
        // ============
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.001f)
        {
            Camera cam = GetComponent<Camera>();
            if (cam != null)
            {
                if (cam.orthographic)
                {
                    // Adjust orthographic size when using an orthographic camera.
                    cam.orthographicSize -= scroll * zoomSpeed;
                    cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minOrthoSize, maxOrthoSize);
                }
                else
                {
                    // Adjust field of view for perspective cameras.
                    cam.fieldOfView -= scroll * zoomSpeed * 10f; // Multiply for a more noticeable effect
                    cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, minFOV, maxFOV);
                }
            }
        }
    }
}
