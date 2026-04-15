using UnityEngine;

public class WindZone : MonoBehaviour
{
    [Header("Force du vent")]
    [SerializeField] private float windForce = 15f;

    [Header("Masse de reference (Science)")]
    [SerializeField] private float referenceMaxMass = 4.5f;
    // Mika Big a une masse ~4.1, donc a referenceMaxMass il est quasi immunise

    [Header("Debug")]
    [SerializeField] private bool showDebug = true;

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb == null) return;

        // Plus la masse est elevee, moins le vent a d'effet
        // F_effective = windForce * (1 - masse / referenceMaxMass)
        // A masse = 0         -> 100% de la force
        // A masse = reference -> 0% de la force (immunise)
        float massFactor = 1f - Mathf.Clamp01(rb.mass / referenceMaxMass);
        float effectiveForce = windForce * massFactor;

        rb.AddForce(transform.forward * effectiveForce, ForceMode.Force);

        if (showDebug)
            Debug.Log($"[WindZone] masse: {rb.mass:F2} | " +
                      $"massFactor: {massFactor:F2} | " +
                      $"force effective: {effectiveForce:F2}");
    }

    private void OnDrawGizmos()
    {
        // Visualise la direction du vent en scene view
        Gizmos.color = new Color(0.5f, 0.8f, 1f, 0.4f);
        Gizmos.matrix = transform.localToWorldMatrix;

        // Zone de vent
        Gizmos.DrawCube(Vector3.zero, Vector3.one);

        // Fleche de direction
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(Vector3.zero, Vector3.forward * 2f);
    }
}