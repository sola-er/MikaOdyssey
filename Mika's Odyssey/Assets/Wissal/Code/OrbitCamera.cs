using UnityEngine;


// to add later: mouse based input, new input manager, automatic alignment, angle limits, handle collisions
[RequireComponent(typeof(Camera))]
public class OrbitCamera : MonoBehaviour
{
    [SerializeField] Transform focus = default; // tells the camera what to focus on

    [SerializeField, Range(1f, 20f)] float distance = 5f; // at what distance from the focus should cam be

    [SerializeField, Min(0f)] float focusRadius = 1f; // cam will only move to stay within the radius, not follow every sphere movement.

    [SerializeField, Range(0f, 1f)] float focusCentering = 0.5f; // value by which the focus speed decreases every time step as we get closer to center of focus

    [SerializeField, Range(1f, 360f)] float rotationSpeed = 90f;

    [SerializeField, Min(0f)] float upAlignmentSpeed = 360f;

    [SerializeField] float mouseSensitivity = 2f;

    Vector3 focusPoint;
    Vector2 orbitAngles = new Vector2(45f, 0f);
    Quaternion gravityAlignment = Quaternion.identity;
    Quaternion orbitRotation;

    private void Awake()
    {
        focusPoint = focus.position;
        transform.localRotation = orbitRotation = Quaternion.Euler(orbitAngles);
        Debug.Assert(focusPoint != null, "The camera needs a focus point!");
    }

    private void LateUpdate()
    {
        UpdateGravityAlignment();
        UpdateFocusPoint();
        if (Input.GetMouseButton(1))
            ManualRotation();
        orbitRotation = Quaternion.Euler(orbitAngles);
        Quaternion lookRotation = gravityAlignment * orbitRotation;
        Vector3 lookDirection = lookRotation * Vector3.forward;
        Vector3 lookPosition = focusPoint - lookDirection * distance; // a ameliorer avec un tire de raycast
        transform.SetPositionAndRotation(lookPosition, lookRotation);
    }

    private void UpdateGravityAlignment()
    {
        Vector3 fromUp = gravityAlignment * Vector3.up;
        Vector3 toUp = CustomGravity.GetUpAxis(focusPoint);
        float dot = Mathf.Clamp(Vector3.Dot(fromUp, toUp), -1f, 1f);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
        float maxAngle = upAlignmentSpeed * Time.deltaTime;

        Quaternion newAlignment = 
            Quaternion.FromToRotation(fromUp, toUp) * gravityAlignment;
        if (angle <= maxAngle)
            gravityAlignment = newAlignment;
        else
            gravityAlignment = Quaternion.SlerpUnclamped(
                gravityAlignment, newAlignment, maxAngle / angle
                );
    }

    void UpdateFocusPoint()
    {
        Vector3 targetPoint = focus.position;
        if (focusRadius > 0f)
        {
            // centering the focus slower and slower. speed decrease determined by focusCentering field.
            float distance = Vector3.Distance(targetPoint, focusPoint);
            float t = 1f;
            if (distance > 0.01f && focusCentering > 0f)
                t = Mathf.Pow(1f - focusCentering, Time.unscaledDeltaTime); // unscaled so that slow motion effects or freezes dont affect the camera

            if (distance > focusRadius)
                t = Mathf.Min(t, focusRadius / distance);

            focusPoint = Vector3.Lerp(targetPoint, focusPoint, t); // lerp to interpolate progressively using custom t
        }
        else
            focusPoint = targetPoint;
    }
    void ManualRotation()
    {
        Vector2 input = new Vector2(
            Input.GetAxis("Mouse Y") * mouseSensitivity, Input.GetAxis("Mouse X") * mouseSensitivity);
        const float e = 0.001f;
        if (input.x < -e || input.x > e || input.y < -e || input.y > e)
        {
            orbitAngles += rotationSpeed * Time.unscaledDeltaTime * input;
            orbitAngles = new Vector2(Mathf.Clamp(orbitAngles.x, -89, 89), orbitAngles.y);
           

        }
            
    }
}
