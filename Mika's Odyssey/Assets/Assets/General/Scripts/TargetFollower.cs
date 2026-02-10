using UnityEngine;

public class FollowTarget : MonoBehaviour
{

    // adapt to follow player
    [SerializeField] private GameObject target;

    private void LateUpdate() // LateUpdate to make sure it follows after target has moved
    {
        transform.position = new Vector3(transform.position.x + target.transform.position.x,
                                         transform.position.y, // stay at the same elevation
                                         transform.position.z + target.transform.position.z);
    }
}
