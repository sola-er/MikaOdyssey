using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Projectille : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    //add audio and Effects if wanted

    public void Init(Vector3 velocity) // need to add a bool when we are going to add effects, right now it is not nessesary
    {
        rb.AddForce(velocity,ForceMode.Impulse);
    }
    
}
