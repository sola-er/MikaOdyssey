using UnityEngine;

public class RocksTriggerZone : MonoBehaviour
{
    [SerializeField] int rocksTarget = 5;
    public static int collectedRocksCounter = 0;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Rock"))
        {
            ++collectedRocksCounter;
            if(collectedRocksCounter >= rocksTarget)
            {
                Planet2Manager.Singleton.rocksComplete = true;
                Planet2Manager.Singleton.CheckCompletion();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Rock"))
            --collectedRocksCounter;
    }
}
