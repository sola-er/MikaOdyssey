using Unity.VisualScripting;
using UnityEngine;

public class CameraControllerBeach : MonoBehaviour
{

    public Transform target;

    [Header("Rotation")]
    public float sensitivity = 3f;
    public float minPitch = -20f;
    public float maxPitch = 40f;

    [Header("Zoom")]
    public float distance = 5f;
    public float minZoom = 2f;
    public float maxZoom = 8f;
    public float zoomSpeed = 2f;

    [Header("Offset")]
    public float heightOffset = 0.5f;

    private float yaw;
    private float pitch;

    void LateUpdate()
    {
        // Mouse input
        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;

        // Clamp vertical rotation
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // Zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distance -= scroll * zoomSpeed;
        distance = Mathf.Clamp(distance, minZoom, maxZoom);

        // Rotation
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        // Target position (center of player)
        Vector3 targetPosition = target.position + Vector3.up * heightOffset;

        // Camera position
        Vector3 cameraPosition = targetPosition - rotation * Vector3.forward * distance;

        transform.position = cameraPosition;
        transform.LookAt(targetPosition);
    }
}
