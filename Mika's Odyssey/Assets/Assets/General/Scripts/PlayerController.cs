using System.IO;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Editor;


// add rotation based on movement direction (D=right)
public class PlayerAction : MonoBehaviour // i need to put headers to seperate things
{
    [Header("Deplacement")]
    public float speed = 10f;
    public float groundDrag = 5f;
    public float jump = 5f;
    public int jumpCount = 1;

    [SerializeField]
    KeyCode jumpCode = KeyCode.Space;
    [SerializeField]
    KeyCode Sprinter = KeyCode.LeftShift;

    [Header("Others")]
    public float pHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation; // might be able to remove it, check where orientation is used

    private float horizontalInput;
    private float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Inputs() // a changer pour choisir les keybinds
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(jumpCode) && grounded && jumpCount <= 0)
        {
            Jump();
        }


    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, pHeight * 0.5f + 0.2f, whatIsGround);
        Inputs();
        if (grounded)
        {
            rb.linearDamping = groundDrag;
        }
        else
        {
            rb.linearDamping = 0;
        }
        SpeedLimit();
        Sprint();
    }
    private void FixedUpdate()
    {
        PlayerMove();

    }

    private void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(transform.up * jump, ForceMode.Impulse);
        jumpCount--;
        if (grounded)
            jumpCount = 1;
    }

    private void PlayerMove() // im pretty sure if we put the player's transform into the orientation, we should be fine regardless,
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if (grounded)
            rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);
        else
            rb.AddForce(moveDirection.normalized * speed * 10f * 0.5f, ForceMode.Force); // to slow if hes in the air



    }

    private void SpeedLimit()
    {
        Vector3 maxVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (maxVelocity.magnitude > speed)
        {
            Vector3 limitedVelo = maxVelocity.normalized * speed;
            rb.linearVelocity = new Vector3(limitedVelo.x, rb.linearVelocity.y, limitedVelo.z);
        }
    }

    private void Sprint() // doesnt really work, probably beacause of the speed limiter
    {
        if (Input.GetKey(Sprinter))
        {
            rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);
        }
    }

}