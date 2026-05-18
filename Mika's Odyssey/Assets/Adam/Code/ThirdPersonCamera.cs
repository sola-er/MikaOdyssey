using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Distance par taille")]
    [SerializeField] private float distanceSmall = 3f;
    [SerializeField] private float distanceNormal = 5f;
    [SerializeField] private float distanceBig = 7f;
    [SerializeField] private float distanceLerpSpeed = 3f;

    [Header("Hauteur")]
    [SerializeField] private float height = 2f;

    [Header("Rotation souris")]
    [SerializeField] private float sensitivityX = 3f;
    [SerializeField] private float sensitivityY = 2f;
    [SerializeField] private float minVerticalAngle = -20f;
    [SerializeField] private float maxVerticalAngle = 60f;

    [Header("Collision")]
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private float collisionRadius = 0.2f;

    private float yaw = 0f;
    private float pitch = 0f;
    private float currentDistance;

    private PlayerRescale rescale;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;

        if (target != null)
            rescale = target.GetComponent<PlayerRescale>();

        currentDistance = distanceNormal;
    }

    private void LateUpdate()
    {
        // Si le jeu est en pause - libere le curseur et ne bouge pas la camera
        if (Time.timeScale == 0f)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }

        // Jeu actif - curseur lock
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (target == null) return;

        // Lecture souris
        yaw += Input.GetAxis("Mouse X") * sensitivityX;
        pitch -= Input.GetAxis("Mouse Y") * sensitivityY;
        pitch = Mathf.Clamp(pitch, minVerticalAngle, maxVerticalAngle);

        // Distance cible selon taille de Mika
        float targetDistance = distanceNormal;
        if (rescale != null)
        {
            targetDistance = rescale.CurrentState switch
            {
                PlayerRescale.SizeState.Small => distanceSmall,
                PlayerRescale.SizeState.Normal => distanceNormal,
                PlayerRescale.SizeState.Big => distanceBig,
                _ => distanceNormal
            };
        }

        // Transition fluide
        currentDistance = Mathf.Lerp(currentDistance, targetDistance,
                                     distanceLerpSpeed * Time.deltaTime);

        // Calcul position
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 targetPos = target.position + Vector3.up * height;
        Vector3 desiredPos = targetPos - rotation * Vector3.forward * currentDistance;

        // Collision camera
        Vector3 finalPos = desiredPos;
        if (Physics.SphereCast(targetPos, collisionRadius,
            (desiredPos - targetPos).normalized,
            out RaycastHit hit,
            currentDistance, collisionMask))
        {
            finalPos = targetPos + (desiredPos - targetPos).normalized * hit.distance;
        }

        transform.position = finalPos;
        transform.LookAt(targetPos);

        UpdatePlayerOrientation(rotation);
    }

    private void UpdatePlayerOrientation(Quaternion camRotation)
    {
        if (target == null) return;
        Transform orientation = target.Find("Orientation");
        if (orientation == null) return;

        Vector3 forward = camRotation * Vector3.forward;
        forward.y = 0f;
        forward.Normalize();

        if (forward != Vector3.zero)
            orientation.rotation = Quaternion.LookRotation(forward);
    }

    public Vector3 GetCameraForward()
    {
        Vector3 f = transform.forward;
        f.y = 0f;
        return f.normalized;
    }

    public float GetYaw() => yaw;
}