using System;
using UnityEngine;

public class MovementWithGravity : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)] private float maxSpeed = 10f;

    [SerializeField, Range(0f, 100f)] 
    float maxAcceleration = 10f, maxAirAcceleration = 1f;

    [SerializeField, Range(0f, 10f)] float jumpHeight = 2f;

    [SerializeField, Range(0, 5)] int maxAirJumps = 0;

    private Vector3 velocity, desiredVelocity;
    Rigidbody rb;
    bool desiredJump;
    bool onGround;
    int jumpPhase;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
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

        float acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        float maxSpeedChange = acceleration * Time.deltaTime;
        velocity.x =
            Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z =
            Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

        if (desiredJump)
        {
            desiredJump = false;
            Jump();
        }

        rb.linearVelocity = velocity;

        onGround = false;
    }

    private void UpdateState()
    {
        velocity = rb.linearVelocity;
        if(onGround)
            jumpPhase = 0;
    }

    private void Jump()
    {
        if (onGround || jumpPhase < maxAirJumps)
        {
            jumpPhase += 1;
            float jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
            if (jumpSpeed > 0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
            }
            velocity.y += jumpSpeed;
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
        for(int i=0; i!= collision.contactCount; ++i)
        {
            Vector3 normal = collision.GetContact(i).normal;
            onGround |= normal.y > 0.9f;
        }
    }
}
