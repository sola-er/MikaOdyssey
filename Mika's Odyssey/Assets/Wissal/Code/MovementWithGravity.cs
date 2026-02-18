using System;
using UnityEngine;

public class MovementWithGravity : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)] private float maxSpeed = 10f;

    [SerializeField, Range(0f, 100f)] 
    float maxAcceleration = 10f, maxAirAcceleration = 1f;

    [SerializeField, Range(0f, 10f)] float jumpHeight = 2f;

    [SerializeField, Range(0, 5)] int maxAirJumps = 0;

    [SerializeField, Range(0f, 90f)]
    float maxGroundAngle = 25f;

    private Vector3 velocity, desiredVelocity;
    Rigidbody rb;
    bool desiredJump;

    int groundContactCount; // could be boolean and still work, but this gives more info
    bool OnGround => groundContactCount > 0;

    int jumpPhase;
    float minGroundDotProduct;
    Vector3 contactNormal;

    void OnValidate()
    {
        minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        OnValidate();
    }
    void Update()
    {
        Vector2 playerInput;
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);

        desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;

        desiredJump |= Input.GetButtonDown("Jump");
    }
    private void FixedUpdate()
    {
        UpdateState();

        AdjustVelocity();

        if (desiredJump)
        {
            desiredJump = false;
            Jump();
        }

        rb.linearVelocity = velocity;

        ClearState();
    }
    void ClearState()
    {
        groundContactCount = 0;
        contactNormal = Vector3.zero;
    }

    private void UpdateState()
    {
        velocity = rb.linearVelocity;
        if (OnGround)
        {
            jumpPhase = 0;
            if (groundContactCount > 1)
            {
                contactNormal.Normalize();
            }
        }
        else
            contactNormal = Vector3.up;
    }

    private void Jump()
    {
        if (OnGround || jumpPhase < maxAirJumps)
        {
            jumpPhase += 1;
            float jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
            float alignedSpeed = Vector3.Dot(velocity, contactNormal);
            if (alignedSpeed > 0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0f);
            }
            velocity += contactNormal * jumpSpeed;
        }
            // derive manually to better understand
    }
    void OnCollisionEnter(Collision collision)
    {
        EvaluateCollision(collision);
    }

    void OnCollisionStay(Collision collision)
    {
        EvaluateCollision(collision);
    }
    private void EvaluateCollision(Collision collision)
    {
        for (int i = 0; i != collision.contactCount; ++i)
        {
            Vector3 normal = collision.GetContact(i).normal;
            if (normal.y >= minGroundDotProduct)
            {
                groundContactCount += 1;
                contactNormal += normal;
            }
             // accumulating normals then averaging them to deal w multiple contact points 
        }
    }
    Vector3 ProjectOnContactPlane(Vector3 vector) // project movemnt on the plane instead of straight line that causes issues when going downhill
    {
        return vector - contactNormal * Vector3.Dot(vector, contactNormal);
        // could use Vector3.ProjectOnPlane instead but it'd try to normalize again when its not needed
    }
    void AdjustVelocity()
    {
        Vector3 xAxis = ProjectOnContactPlane(Vector3.right).normalized;
        Vector3 zAxis = ProjectOnContactPlane(Vector3.forward).normalized;

        float currentX = Vector3.Dot(velocity, xAxis);
        float currentZ = Vector3.Dot(velocity, zAxis);

        float acceleration = OnGround ? maxAcceleration : maxAirAcceleration;
        float maxSpeedChange = acceleration * Time.deltaTime;

        float newX =
            Mathf.MoveTowards(currentX, desiredVelocity.x, maxSpeedChange);
        float newZ =
            Mathf.MoveTowards(currentZ, desiredVelocity.z, maxSpeedChange);

        velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);
    }
}
