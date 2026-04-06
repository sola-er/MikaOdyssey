using UnityEngine;

public class PushCube : MonoBehaviour
{
    [Header("Physique")]
    [SerializeField] private float mass = 3f;
    [SerializeField] private float drag = 1.5f;
    [SerializeField] private PhysicsMaterial physicsMaterial;

    [Header("Contraintes")]
    [SerializeField] private bool lockRotation = true;
    [SerializeField] private bool lockVerticalMovement = false;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();

        rb.mass = mass;
        rb.linearDamping = drag;

        if (lockRotation)
            rb.constraints = RigidbodyConstraints.FreezeRotation;

        if (lockVerticalMovement)
            rb.constraints |= RigidbodyConstraints.FreezePositionY;

        // Applique le physicsMaterial sur le collider si assigne
        if (physicsMaterial != null)
        {
            Collider col = GetComponent<Collider>();
            if (col != null) col.material = physicsMaterial;
        }
    }

    // Lecture publique pour PressurePlate
    public float GetMass() => rb.mass;
}