using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Projectille : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    private bool _isGhost;
    //add audio and Effects if wanted

    public void Init(Vector3 velocity, bool isGhost) // need to add a bool when we are going to add effects, right now it is not nessesary
    {
        _isGhost = isGhost;
        rb.AddForce(velocity,ForceMode.Impulse);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (_isGhost) return;
    }
}
