using UnityEngine;
public class RaisinOrbit : MonoBehaviour
{
    [SerializeField] private Transform planet;
    [SerializeField] private float orbitRadius = 20f;
    [SerializeField] private float orbitSpeed = 30f;
    [SerializeField] private float orbitHeight = 5f;
    private float angle = 0f;
    private Vector3 up;
    private Vector3 right;
    private Vector3 forward;

    private void Start()
    {
        up = CustomGravity.GetUpAxis(planet.position);
        right = Vector3.Cross(up, Vector3.forward).normalized;
        if (right == Vector3.zero)
            right = Vector3.Cross(up, Vector3.right).normalized;
        forward = Vector3.Cross(right, up).normalized;
    }

    private void Update()
    {
        angle += orbitSpeed * Time.deltaTime;
        transform.position = planet.position
            + up * orbitHeight
            + (Mathf.Cos(angle * Mathf.Deg2Rad) * right
            + Mathf.Sin(angle * Mathf.Deg2Rad) * forward) * orbitRadius;
    }
}