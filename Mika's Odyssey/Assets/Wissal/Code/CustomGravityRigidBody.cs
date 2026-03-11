using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CustomGravityRigidBody : MonoBehaviour
{
    [SerializeField] bool floatToSleep = false;

    Rigidbody rb;
    float floatDelay;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }
    private void FixedUpdate()
    {
        if(floatToSleep)
        {
            if (rb.IsSleeping())
            {
                floatDelay = 0f;
                return;
            }
            if (rb.linearVelocity.sqrMagnitude < 0.0001f && rb.angularVelocity.sqrMagnitude < 0.0001f)
            {
                floatDelay += Time.deltaTime;
                if (floatDelay >= 1f)
                    return;
            }
            else
                floatDelay = 0f;
        }

        rb.AddForce(CustomGravity.GetGravity(rb.position), ForceMode.Acceleration);
    }
}
