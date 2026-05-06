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

        rb.freezeRotation = true; // CRITICAL: prevents physics spin

       // Cursor.lockState = CursorLockMode.Locked;
    }

    private Quaternion targetRotation;

    private void Update()
    {
        ReadInput();
        UpdateTargetRotation();
        ApplyRotation();
    }

    private void UpdateTargetRotation()
    {
        if (inputDirection.sqrMagnitude < 0.01f)
            return;

        Vector3 camForward = Vector3.ProjectOnPlane(playerCamera.transform.forward, Vector3.up).normalized;
        Vector3 camRight   = Vector3.ProjectOnPlane(playerCamera.transform.right, Vector3.up).normalized;

        Vector3 moveDir = camForward * inputDirection.z + camRight * inputDirection.x;

        if (moveDir.sqrMagnitude < 0.01f)
            return;

        targetRotation = Quaternion.LookRotation(moveDir);
    }

    private void ApplyRotation()
    {
        transform.rotation = targetRotation;
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
        );
    }

    // ---------- CAMERA ROTATION (ONLY CAMERA NOW) ----------
    private void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // rotate player horizontally ONLY for camera yaw
        transform.Rotate(Vector3.up * mouseX);

        camRotation -= mouseY;
        camRotation = Mathf.Clamp(camRotation, -80f, 80f);

        playerCamera.transform.localRotation = Quaternion.Euler(camRotation, 0f, 0f);
    }

    // ---------- MOVEMENT ----------
    private void Move()
    {
        Vector3 camForward = Vector3.ProjectOnPlane(playerCamera.transform.forward, Vector3.up).normalized;
        Vector3 camRight   = Vector3.ProjectOnPlane(playerCamera.transform.right, Vector3.up).normalized;

        Vector3 moveDir = camForward * inputDirection.z +
                          camRight * inputDirection.x;

        if (moveDir.sqrMagnitude < 0.0001f)
        {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            return;
        }

        moveDir.Normalize();

        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && stamina > 0f;

        float currentSpeed = isSprinting ? sprintSpeed : speed;

        Vector3 velocity = moveDir * currentSpeed;

        rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);
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
        bool isMoving = inputDirection.sqrMagnitude > 0.01f;
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