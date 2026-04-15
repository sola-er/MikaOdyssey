using UnityEngine;

public class SnapToStart : MonoBehaviour
{
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private KeyCode snapKey = KeyCode.R;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(snapKey))
            Snap();
    }

    public void Snap()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = startPosition;
        HeartsManager.Singleton.LoseHeart();
    }
}