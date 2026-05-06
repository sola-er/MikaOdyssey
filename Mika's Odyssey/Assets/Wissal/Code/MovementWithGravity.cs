using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovementWithGravity : MonoBehaviour
{
    [SerializeField] private float normalJumpHeight = 2.5f;
    [SerializeField] private float spaceJumpHeight = 10f;
    [SerializeField] private int normalAirJumps = 3;
    [SerializeField] private int spaceAirJumps = 5;

    public bool spaceTravelMode = false;

    [SerializeField, Range(0f, 100f)] private float maxSpeed = 10f;

    [SerializeField, Range(0f, 100f)] 
    float maxAcceleration = 10f, maxAirAcceleration = 1f;

    [SerializeField, Range(0f, 10f)] float jumpHeight = 2.5f;

    [SerializeField, Range(0, 5)] int maxAirJumps = 3;

    [SerializeField, Range(0f, 90f)]
    float maxGroundAngle = 25f, maxStairsAngle = 50f;

    [SerializeField, Range(0f, 100f)] float maxSnapSpeed = 100f;

    [SerializeField, Min(0f)] float probeDistance = 1f;

    [SerializeField] 
    LayerMask probeMask = -1, stairsMask = -1;

    [SerializeField]
	Transform playerInputSpace = default;

    [SerializeField] private Transform modelTransform;
    [SerializeField] private float rotationSpeed = 10f;

    private Vector3 velocity, desiredVelocity;
    Rigidbody rb;
    bool desiredJump;
    int groundContactCount, steepContactCount; // could be boolean and still work, but this gives more info
    bool OnGround => groundContactCount > 0;
    bool OnSteep => steepContactCount > 0;
    int jumpPhase;
    float minGroundDotProduct, minStairsDotProduct;
    Vector3 contactNormal, steepNormal;
    int stepsSinceLastGrounded, stepsSinceLastJump;

    Vector3 upAxis, rightAxis, forwardAxis;

    public void SetSpaceTravellingMode(bool enabled)
    {
        spaceTravelMode = enabled;
        jumpHeight = enabled ? spaceJumpHeight : normalJumpHeight;
        maxAirJumps = enabled ? spaceAirJumps : normalAirJumps;
    }
    void OnValidate()
    {
        minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
        minStairsDotProduct = Mathf.Cos(maxStairsAngle * Mathf.Deg2Rad);
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        OnValidate();
    }
    void Update()
    {
        Vector2 playerInput;
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);

        if (playerInputSpace)
        {
            rightAxis = ProjectDirectionOnPlane(playerInputSpace.right, upAxis);
            forwardAxis =
                ProjectDirectionOnPlane(playerInputSpace.forward, upAxis);
        }
        else
        {
            rightAxis = ProjectDirectionOnPlane(Vector3.right, upAxis);
            forwardAxis = ProjectDirectionOnPlane(Vector3.forward, upAxis);
        }
        desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;
        desiredJump |= Input.GetButtonDown("Jump");

        if (desiredVelocity.sqrMagnitude > 0.01f)
        {
            Vector3 moveDir = (rightAxis * playerInput.x + forwardAxis * playerInput.y).normalized;
            if (moveDir != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDir, upAxis) * Quaternion.Euler(0, 180, 0);
                modelTransform.rotation = Quaternion.Slerp(
                    modelTransform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
            }
        }
    }
    private void FixedUpdate()
    {
        Vector3 gravity = CustomGravity.GetGravity(rb.position, out upAxis);

        UpdateState();

        AdjustVelocity();

        if (desiredJump)
        {
            desiredJump = false;
            Jump(gravity);
        }
        velocity += gravity * Time.deltaTime;

        rb.linearVelocity = velocity;
        ClearState();
    }
    void ClearState()
    {
        groundContactCount = steepContactCount = 0;
        contactNormal = steepNormal = Vector3.zero; // 2 = ok?
    }

    private void UpdateState()
    {
        stepsSinceLastGrounded += 1;
        stepsSinceLastJump += 1;
        velocity = rb.linearVelocity;
        if (OnGround || SnapToGround() || CheckSteepContacts()) // snap only gets called when OnGround is false
        {
            stepsSinceLastGrounded = 0;
            if (stepsSinceLastJump > 1)
                jumpPhase = 0;
            if (groundContactCount > 1)
            {
                contactNormal.Normalize();
            }
        }
        else
            contactNormal = upAxis;
    }

    private void Jump(Vector3 gravity)
    {
        // enables regular, crevice and wall jumping. each if is a check for the situation
        Vector3 jumpDirection;
        if (OnGround)
        {
            jumpDirection = contactNormal;
        }
        else if (OnSteep)
        {
            jumpDirection = steepNormal;
            jumpPhase = 0;
        }
        else if (maxAirJumps > 0 && jumpPhase <= maxAirJumps)
        {
            if (jumpPhase == 0)
                jumpPhase = 1;
            jumpDirection = contactNormal;
        }
        else
        {
            return;
        }
        stepsSinceLastJump = 0;
        jumpPhase += 1;
        float jumpSpeed = Mathf.Sqrt(2f * gravity.magnitude * jumpHeight);
        jumpDirection = (jumpDirection + upAxis).normalized;

        float alignedSpeed = Vector3.Dot(velocity, jumpDirection);
        if (alignedSpeed > 0f)
        {
            jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0f);
        }
        velocity += jumpDirection * jumpSpeed;
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
        float minDot = GetMinDot(collision.gameObject.layer);
        for (int i = 0; i != collision.contactCount; ++i)
        {
            Vector3 normal = collision.GetContact(i).normal;
            float upDot = Vector3.Dot(upAxis, normal);
            if (upDot >= minDot)
            {
                groundContactCount += 1;
                contactNormal += normal;
            }
            // accumulating normals then averaging them to deal w multiple contact points 
            else if (upDot > -0.01f)
            {
                steepContactCount += 1;
                steepNormal += normal;
            }
        }
    }
    Vector3 ProjectDirectionOnPlane(Vector3 direction, Vector3 normal)
    {
        return (direction - normal * Vector3.Dot(direction, normal)).normalized;
    }
        void AdjustVelocity()
    {
        Vector3 xAxis = ProjectDirectionOnPlane(rightAxis, contactNormal);
        Vector3 zAxis = ProjectDirectionOnPlane(forwardAxis, contactNormal);

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
    bool SnapToGround()
    {
        if (stepsSinceLastGrounded > 1 || stepsSinceLastJump <= 2)
        {
            return false; //only snap once after losing contact with ground 
        }
        float speed = velocity.magnitude;
        if (speed > maxSnapSpeed)
            return false;

        if(!Physics.Raycast(
            rb.position, -upAxis, out RaycastHit hit, 
            probeDistance, probeMask)) 
            // is there ground below? + close enough? + is part of background?
            return false;

        float upDot = Vector3.Dot(upAxis, hit.normal);
        if (upDot < GetMinDot(hit.collider.gameObject.layer))
            return false;

        groundContactCount = 1;
        contactNormal = hit.normal;
        float dot = Vector3.Dot(velocity, hit.normal);
        if (dot > 0f)
            velocity = (velocity - hit.normal * dot).normalized * speed;
        return true;
    }

    float GetMinDot (int layer)
    {
        return (stairsMask & (1 << layer)) == 0 ? minGroundDotProduct : minStairsDotProduct;
    }
    bool CheckSteepContacts()
    {
        if (steepContactCount > 1)
        {
            steepNormal.Normalize();
            float upDot = Vector3.Dot(upAxis, steepNormal);
            if (upDot >= minGroundDotProduct)
            {
                groundContactCount = 1;
                contactNormal = steepNormal;
                return true;
            }
        }
        return false;
    }
}
