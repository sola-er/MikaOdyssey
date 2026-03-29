using UnityEngine;

public class SpriteAlignment : MonoBehaviour
{
    void Update()
    {
        
        Vector3 upAxis = CustomGravity.GetUpAxis(transform.position);

        // direction in relation to camer but flattened so sprite doesn't tilt up/down
        Vector3 toCam = Camera.main.transform.position - transform.position;
        toCam = Vector3.ProjectOnPlane(toCam, upAxis).normalized;

        // rotation faces cam but aligned w planet gravity
        if (toCam != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(toCam, upAxis);
        transform.Rotate(0, 180, 0);
    }
}
