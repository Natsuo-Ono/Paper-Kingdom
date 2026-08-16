using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class VesselMovement : MonoBehaviour
{
    private VesselCombat combat;

    [Header("Movement")]
    public float walkSpeed = 4f;
    public float runSpeed = 7f;

    [Header("Rotation")]
    public float rotationSpeed = 720f;

    [Header("Gravity")]
    public float gravity = -20f;

    [Header("Ground Check")]
    public float groundCheckDistance = 0.2f;

    [Header("Action State")]
    public bool CanMove { get; private set; } = true;

    private CharacterController controller;
    private Vector3 velocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        combat = GetComponent<VesselCombat>();
    }

    private void Update()
    {
        Move();
        ApplyGravity();
    }

    private void Move()
    {
        if (!CanMove)
            return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 input =
            new Vector3(
                horizontal,
                0f,
                vertical
            );

        if (input.sqrMagnitude > 0.001f)
        {
            combat.ResetBasicAttackCombo();
        }

        if (input.magnitude > 1f)
            input.Normalize();

        bool isRunning =
            Input.GetKey(KeyCode.LeftShift);

        float speed =
            isRunning
                ? runSpeed
                : walkSpeed;

        Vector3 movement =
            input * speed;

        // Rotate toward movement direction
        if (input.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(input);

            transform.rotation =
                Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
        }

        controller.Move(
            movement * Time.deltaTime
        );
    }

    private void ApplyGravity()
    {
        if (controller.isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        velocity.y +=
            gravity * Time.deltaTime;

        controller.Move(
            velocity * Time.deltaTime
        );
    }

    public void LockMovement()
    {
        CanMove = false;
    }

    public void UnlockMovement()
    {
        CanMove = true;
    }
}