using UnityEngine;

public class FlecheDuVecteur : MonoBehaviour
{
    //pour test
    public float arrowLength = 2.0f; // Longueur de la flèche
    public Color arrowColor = Color.blue; 

    
    void OnDrawGizmos()
    {
       
        Gizmos.color = arrowColor;

       
        Vector3 start = transform.position;

        
        Vector3 end = start + transform.forward * arrowLength;

        
        Gizmos.DrawLine(start, end);

        
        float arrowHeadLength = 0.3f;
        float arrowHeadAngle = 20.0f;

        Vector3 right = Quaternion.LookRotation(transform.forward) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * Vector3.forward;
        Vector3 left = Quaternion.LookRotation(transform.forward) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * Vector3.forward;

        Gizmos.DrawRay(end, right * arrowHeadLength);
        Gizmos.DrawRay(end, left * arrowHeadLength);
    }
}