using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothness = 1f; // higher for fast, snappy following, lower for opposite.
    private Vector3 offset;

    private void Start()
    {
        Debug.Assert(target != null, "Target is not assigned in FollowTarget script.");
        offset = transform.position - target.position;
    }
    private void LateUpdate() // LateUpdate to make sure it follows after target has moved
    {
        // .lerp moves smoothly towards target position, independetly of frame rate bc of time.deltatime
        transform.position = Vector3.Lerp(transform.position, target.position + offset, smoothness * Time.deltaTime);
    }
}
