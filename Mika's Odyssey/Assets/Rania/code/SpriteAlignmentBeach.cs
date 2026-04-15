using UnityEngine;

public class SpriteAlignmentBeach : MonoBehaviour
{
    void LateUpdate()
    {
        Vector3 upAxis = CustomGravity.GetUpAxis(transform.position);

        // Direction from sprite to camera
        Vector3 toCam = Camera.main.transform.position - transform.position;

        // Remove vertical component (no tilt)
        Vector3 flattened = Vector3.ProjectOnPlane(toCam, upAxis);

        if (flattened.sqrMagnitude > 0.0001f)
        {
            transform.rotation = Quaternion.LookRotation(flattened.normalized, upAxis);
        }
    }
}