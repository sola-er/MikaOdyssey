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
    public float jumpHeight = 7f;
    public float rotationSpeed = 5f;
    public float sprintSpeed = 30f;

    public int additionalJumpCount = 1;

    [SerializeField]
    KeyCode jumpCode = KeyCode.Space;
    [SerializeField]
    KeyCode Sprinter = KeyCode.LeftShift;

    [Header("Others")]
    public float playerHeight = 2;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation; // might be able to remove it, check where orientation is used

    private float horizontalInput;
    private float verticalInput;

    Vector3 moveDirection;

    // camera stuff
    Vector3 RotationDirection;

    Rigidbody rb;

    int jumpCount;

    RaycastHit hit;
    RaycastHit sprintHit;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Inputs() // a changer pour choisir les keybinds
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(jumpCode) && jumpCount > 0)
        {
            Jump();
            
        }


    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        Inputs();
        if (grounded)
        {
            rb.linearDamping = groundDrag;
                jumpCount = additionalJumpCount;
        }
        else
        {
            rb.linearDamping = 0;
        }
        
        if (RotationDirection != Vector3.zero) // we need to tweak this // before inputdir it was moveDirection != Vector3.zero
        {
            Quaternion targetRotation = Quaternion.LookRotation(RotationDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * 100 * Time.deltaTime);
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
        rb.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
        jumpCount--;
        
    }

    private void PlayerMove() 
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput; //  step one

        if (Physics.Raycast(transform.position, Vector3.down, out hit, playerHeight))
        {
            Vector3 surfaceNormal = hit.normal;
            Vector3 projectedVector = Vector3.ProjectOnPlane(moveDirection, surfaceNormal);

            if (grounded)
                rb.AddForce(projectedVector.normalized * speed * 10f, ForceMode.Force);
            else
                rb.AddForce(projectedVector.normalized * speed * 10f * 0.5f, ForceMode.Force); // to slow if hes in the air
        }

        Vector3 camForward = orientation.forward;
        Vector3 camRight = orientation.right;

        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        RotationDirection = camForward * verticalInput + camRight * horizontalInput;
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

    private void Sprint() // the sprint speed value needs to be 30+ to be able to work properly
    {
        if (Physics.Raycast(transform.position, Vector3.down, out sprintHit, playerHeight))
        {
            Vector3 sprintSurfaceNormal = hit.normal;
            Vector3 sprintProjectedVector = Vector3.ProjectOnPlane(moveDirection, sprintSurfaceNormal);

            if (Input.GetKey(Sprinter))
            {
                rb.AddForce(sprintProjectedVector.normalized * speed * sprintSpeed, ForceMode.Force);
            }
        }
       

       
    }

}