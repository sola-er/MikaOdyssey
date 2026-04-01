using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControllerWater : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float sprintSpeed = 10f;

    [Header("Vertical Movement")]
    [SerializeField] private float verticalSpeed = 4f;

    private Rigidbody rb;

    private float currentSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // no gravity underwater
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        // Input
        float moveX = Input.GetAxis("Horizontal"); // A / D
        float moveZ = Input.GetAxis("Vertical");   // W / S

        // Sprint
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        currentSpeed = isSprinting ? sprintSpeed : speed;

        // Direction relative to player
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Optional vertical movement (space = up, ctrl = down)
        float moveY = 0f;
        if (Input.GetKey(KeyCode.Space))
            moveY = 1f;
        else if (Input.GetKey(KeyCode.LeftControl))
            moveY = -1f;

        Vector3 verticalMove = Vector3.up * moveY * verticalSpeed;

        // Apply velocity
        Vector3 finalVelocity = (move * currentSpeed) + verticalMove;

        rb.linearVelocity = finalVelocity;
    }
}
