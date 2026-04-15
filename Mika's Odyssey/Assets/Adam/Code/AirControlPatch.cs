using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerAction))]
public class AirControlPatch : MonoBehaviour
{
    [Header("Air Control")]
    [SerializeField][Range(0f, 1f)] private float airControlFactor = 0.8f;

    private Rigidbody rb;
    private PlayerAction playerAction;
    private bool grounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerAction = GetComponent<PlayerAction>();
    }

    private void FixedUpdate()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down,
                                   playerAction.playerHeight * 0.5f + 0.2f,
                                   playerAction.whatIsGround);

        if (!grounded)
        {
            // Recupere la direction de mouvement depuis l'orientation
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            Vector3 moveDir = playerAction.orientation.forward * v +
                              playerAction.orientation.right * h;

            rb.AddForce(moveDir.normalized * playerAction.speed * 10f * airControlFactor,
                        ForceMode.Force);
        }
    }
}