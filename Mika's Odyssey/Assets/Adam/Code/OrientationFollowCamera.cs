using UnityEngine;

public class OrientationFollowCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;

    private void Update()
    {
        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0f;
        camForward.Normalize();

        if (camForward != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(camForward);
    }
}