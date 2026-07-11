using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float rotationSpeed = 10f;

    private CharacterController controller;
    private float encounterTimer;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // WASD Input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        // Move
        if (moveDirection.magnitude > 0.1f)
        {
            controller.Move(moveDirection * moveSpeed * Time.deltaTime);

            // Rotate towards movement
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        if (moveDirection != Vector3.zero)
        {
            encounterTimer += Time.deltaTime;

            if (encounterTimer >= 1f)
            {
                encounterTimer = 0f;
                EncounterManager.Instance.TryEncounter();
            }
        }
        else
        {
            encounterTimer = 0f;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        EncounterTrigger trigger = other.GetComponent<EncounterTrigger>();

        if (trigger != null)
            EncounterManager.Instance.EnterTrigger(trigger);
    }

    void OnTriggerExit(Collider other)
    {
        EncounterTrigger trigger = other.GetComponent<EncounterTrigger>();

        if (trigger != null)
            EncounterManager.Instance.ExitTrigger();
    }
}