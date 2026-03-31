using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float sprintSpeed = 12f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 5f;

    [Header("Stamina")]
    [SerializeField] private float maxStamina = 50f;
    [SerializeField] private float staminaDrainRate = 5f;
    [SerializeField] private float staminaRegenRate = 3f;

    [Header("Camera")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float mouseSensitivity = 2f;

    private Rigidbody rb;
    private float stamina;
    private float camRotation;
    private bool isGrounded;

    private Vector3 inputDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        stamina = maxStamina;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        ReadInput();
        RotatePlayer();
        HandleJump();
        HandleStamina();
    }

    private void FixedUpdate()
    {
        Move();
    }

    // ---------- INPUT ----------
    private void ReadInput()
    {
        inputDirection = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            0f,
            Input.GetAxisRaw("Vertical")
        ).normalized;
    }

    // ---------- MOVEMENT ----------
    private void Move()
    {
        // Movement relative to player (stable)
        Vector3 moveDir = transform.forward * inputDirection.z +
                          transform.right * inputDirection.x;

        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && stamina > 0f && moveDir != Vector3.zero;
        float currentSpeed = isSprinting ? sprintSpeed : speed;

        Vector3 velocity = moveDir * currentSpeed;

        rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);
    }

    // ---------- ROTATION ----------
    private void RotatePlayer()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate player horizontally
        transform.Rotate(Vector3.up * mouseX);

        // Rotate camera vertically
        camRotation -= mouseY;
        camRotation = Mathf.Clamp(camRotation, -80f, 80f);

        playerCamera.transform.localRotation = Quaternion.Euler(camRotation, 0f, 0f);
    }

    // ---------- JUMP ----------
    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    // ---------- STAMINA ----------
    private void HandleStamina()
    {
        bool isMoving = inputDirection != Vector3.zero;
        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && stamina > 0f && isMoving;

        float delta = isSprinting ? -staminaDrainRate : staminaRegenRate;
        stamina = Mathf.Clamp(stamina + delta * Time.deltaTime, 0f, maxStamina);
    }

    // ---------- GROUND CHECK ----------
    private void OnCollisionStay(Collision collision)
    {
        if (collision.contacts[0].normal.y > 0.5f)
            isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}