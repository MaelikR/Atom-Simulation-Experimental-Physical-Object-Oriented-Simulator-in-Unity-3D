using Fusion;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class GravityHandler : NetworkBehaviour
{
    [Header("Gravity Settings")]
    public float gravity = -9.81f;
    public float groundedOffset = -1f;
    public float fallMultiplier = 2f;

    [Networked] private float verticalVelocity { get; set; }

    private CharacterController controller;
    private bool grounded;

    public override void Spawned()
    {
        controller = GetComponent<CharacterController>();
    }

    public void ApplyGravity()
    {
        grounded = controller.isGrounded;

        if (grounded && verticalVelocity < 0f)
        {
            verticalVelocity = -1f; // légère pression vers le sol
        }
        else
        {
            verticalVelocity += gravity * fallMultiplier * Runner.DeltaTime;
        }

        Vector3 gravityMove = new Vector3(0f, verticalVelocity, 0f);
        controller.Move(gravityMove * Runner.DeltaTime);
    }

    public bool IsGrounded() => grounded;
    public float GetVerticalVelocity() => verticalVelocity;
}
