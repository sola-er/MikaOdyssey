using UnityEngine;

public class WindZone : MonoBehaviour
{
    [Header("Force du vent")]
    [SerializeField] private float windForce = 20f;

    [Header("Masse de reference (Science)")]
    [SerializeField] private float referenceMaxMass = 4.5f;

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb == null) return;

        // Plus Mika est lourd, moins le vent l'affecte
        // Small   : presque 100% de la force
        // Normal  : force reduite
        // Big     : presque immunise
        float massFactor = 1f - Mathf.Clamp01(rb.mass / referenceMaxMass);
        float effectiveForce = windForce * massFactor;

        rb.AddForce(transform.forward * effectiveForce, ForceMode.Force);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.5f, 0.8f, 1f, 0.3f);
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(Vector3.zero, Vector3.one);

        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(Vector3.zero, Vector3.forward * 2f);
    }
}