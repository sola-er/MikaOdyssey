using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControllerWater : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveAcceleration = 12f;
    [SerializeField] private float maxSpeed = 6f;

    [Header("Vertical")]
    [SerializeField] private float verticalAcceleration = 10f;

    [Header("Water")]
    [SerializeField] private Transform waterSurface;
    [SerializeField] private float surfaceOffset = 0.5f; // how close you can get to surface

    [Header("Physics")]
    [SerializeField] private float waterDrag = 2f;
    [SerializeField] private float verticalDrag = 4f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.linearDamping = 0f; // we control drag manually
    }

    private void FixedUpdate()
    {
        HandleMovement();
        ApplyDrag();
        ApplySurfaceLimit();
    }

    private void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 inputDir = (transform.right * moveX + transform.forward * moveZ);
        if (inputDir.magnitude > 1f) inputDir.Normalize();

        // Vertical input
        float moveY = 0f;
        if (Input.GetKey(KeyCode.Space)) moveY += 1f;
        if (Input.GetKey(KeyCode.LeftControl)) moveY -= 1f;

        Vector3 force =
            inputDir * moveAcceleration +
            Vector3.up * moveY * verticalAcceleration;

        rb.AddForce(force, ForceMode.Acceleration);

        // Clamp horizontal speed only (important)
        Vector3 horizontalVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (horizontalVel.magnitude > maxSpeed)
        {
            horizontalVel = horizontalVel.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(horizontalVel.x, rb.linearVelocity.y, horizontalVel.z);
        }
    }

    private void ApplyDrag()
    {
        Vector3 vel = rb.linearVelocity;

        // Horizontal drag
        Vector3 horizontal = new Vector3(vel.x, 0f, vel.z);
        horizontal *= (1f - waterDrag * Time.fixedDeltaTime);

        // Vertical drag (separate control)
        float vertical = vel.y * (1f - verticalDrag * Time.fixedDeltaTime);

        rb.linearVelocity = new Vector3(horizontal.x, vertical, horizontal.z);
    }

    private void ApplySurfaceLimit()
    {
        float surfaceY = waterSurface.position.y - surfaceOffset;

        // If above allowed height → gently push down
        if (rb.position.y > surfaceY)
        {
            float excess = rb.position.y - surfaceY;

            // Soft downward force (NOT a clamp)
            float pushDown = excess * 20f;

            rb.AddForce(Vector3.down * pushDown, ForceMode.Acceleration);

            // Prevent upward velocity from accumulating
            if (rb.linearVelocity.y > 0f)
            {
                rb.linearVelocity = new Vector3(
                    rb.linearVelocity.x,
                    0f,
                    rb.linearVelocity.z
                );
            }
        }
    }
}