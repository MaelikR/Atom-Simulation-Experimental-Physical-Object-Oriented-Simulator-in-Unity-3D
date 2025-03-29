// =========================
// FirstPersonCamera.cs — Network-Ready Player Controller with Swimming
// =========================
using UnityEngine;
using Fusion;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(NetworkObject))]
public class FirstPersonCamera : NetworkBehaviour
{
    [Header("References")]
    public Transform cameraHolder;
    public Transform cameraTransform;

    [Header("Movement Settings")]
    public float mouseSensitivity = 2f;
    public float moveSpeed = 5f;
    public float swimSpeed = 3f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    [Header("Swimming Settings")]
    public float waterSurfaceY = 0f;
    public float waterThreshold = 1f;

    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation = 0f;
    private bool isSwimming = false;
    private bool isLocalPlayer = false;

    public override void Spawned()
    {
        controller = GetComponent<CharacterController>();

        if (Object.HasInputAuthority)
        {
            isLocalPlayer = true;

            if (cameraTransform == null)
                cameraTransform = Camera.main.transform;

            if (cameraHolder == null && cameraTransform != null)
                cameraHolder = cameraTransform.parent;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            // Disable camera on remote players
            if (cameraHolder != null)
                cameraHolder.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!isLocalPlayer || Time.timeScale == 0f) return;

        CheckSwimmingState();
        HandleMouseLook();
        HandleMovement();
    }

    void CheckSwimmingState()
    {
        float headHeight = cameraTransform.position.y;
        isSwimming = headHeight < (waterSurfaceY - waterThreshold);
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        if (isSwimming)
        {
            float y = 0f;
            if (Input.GetKey(KeyCode.Space)) y += 1f;
            if (Input.GetKey(KeyCode.LeftControl)) y -= 1f;
            move += Vector3.up * y;
            controller.Move(move * swimSpeed * Time.deltaTime);
            velocity = Vector3.zero;
        }
        else
        {
            controller.Move(move * moveSpeed * Time.deltaTime);

            if (controller.isGrounded && velocity.y < 0)
                velocity.y = -2f;

            if (Input.GetButtonDown("Jump") && controller.isGrounded)
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }
}
