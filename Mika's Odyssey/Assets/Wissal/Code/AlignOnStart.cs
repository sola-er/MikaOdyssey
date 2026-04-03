using UnityEngine;

public class AlignOnStart : MonoBehaviour
{
    [SerializeField] private float snapDistance = 10f;
    void Start()
    {
        Vector3 up = CustomGravity.GetUpAxis(transform.position);
        transform.up = up;

        if (Physics.Raycast(transform.position, -up, out RaycastHit hit, snapDistance))
            transform.position = hit.point;
    }
}