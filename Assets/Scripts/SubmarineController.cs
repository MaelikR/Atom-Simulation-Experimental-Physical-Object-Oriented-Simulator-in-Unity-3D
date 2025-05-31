// Attach this script to the submarine prefab
using UnityEngine;

public class SubmarineController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float turnSpeed = 30f;
    public float verticalSpeed = 2f;

    [Header("Physics")]
    public Rigidbody rb;

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.drag = 1.5f;
    }

    void FixedUpdate()
    {
        float moveForward = Input.GetAxis("Vertical") * moveSpeed;
        float turn = Input.GetAxis("Horizontal") * turnSpeed;
        float vertical = 0f;

        if (Input.GetKey(KeyCode.Space)) vertical = verticalSpeed;
        if (Input.GetKey(KeyCode.LeftControl)) vertical = -verticalSpeed;

        Vector3 movement = transform.forward * moveForward + transform.up * vertical;
        rb.AddForce(movement);
        rb.MoveRotation(rb.rotation * Quaternion.Euler(Vector3.up * turn * Time.fixedDeltaTime));
    }
}
