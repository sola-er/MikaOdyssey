using UnityEngine;

public class Weather : MonoBehaviour
{
    [SerializeField] float forceApplied = 10f;
    // rajouter la particule qu'on veut pour l'effet.


    Rigidbody currentRigidbody;

    //private void FixedUpdate()
    //{
    //    if (currentRigidbody != null)
    //    {
    //        currentRigidbody.AddForce(forceApplied * Vector3.forward, ForceMode.Force);
    //    }
    //}

    //private void OnTriggerEnter(Collider other) // i have no idea if this works on everything, but i think i need to make a list if i want it to work on EVERY object
    //{
    //    Rigidbody peutVoler = other.GetComponent<Rigidbody>();
    //    if (peutVoler != null)
    //        currentRigidbody = peutVoler;
    //    Debug.Log("zone Entre");

    //}

 
    //private void OnTriggerExit(Collider other)
    //{
    //    Rigidbody peutVoler = other.GetComponent<Rigidbody>();
    //    if (peutVoler != null && peutVoler == currentRigidbody)
    //    currentRigidbody = null;
    //    Debug.Log("zone sortie");
    //}

    private void OnTriggerStay(Collider other)
    {
        Rigidbody peutVoler = other.GetComponent<Rigidbody>();
        if (peutVoler != null)
        {
            currentRigidbody = peutVoler;
            currentRigidbody.AddForce(forceApplied * Vector3.forward, ForceMode.Force);
        }
    }
}
