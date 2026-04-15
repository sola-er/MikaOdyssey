using UnityEngine;

public class HouseManager : MonoBehaviour
{
    public static HouseManager Singleton { get; private set; }

    public bool itemCollected = false;

    public bool questIsComplete => itemCollected;

    private void Awake()
    {
        if (Singleton == null)
            Singleton = this;
        else
            Destroy(gameObject);

        itemCollected = false;
    }

    public void CollectItem()
    {
        itemCollected = true;
        Debug.Log("House quest complete");
    }
}