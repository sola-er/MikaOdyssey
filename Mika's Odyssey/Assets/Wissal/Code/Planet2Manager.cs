using UnityEngine;
public class Planet2Manager : MonoBehaviour
{
    public static Planet2Manager Singleton { get; private set; }
    public bool questHasStarted = false;
    public bool trashComplete = false;
    public bool rocksComplete = false;
    public bool questIsComplete => rocksComplete && trashComplete;


    private void Awake()
    {
        if (Singleton == null)
            Singleton = this;
        else
            Destroy(gameObject);
        TrashCollecting.collectedTrashCounter = 0;
        RocksTriggerZone.collectedRocksCounter = 0;
    }
    public void CheckCompletion()
    {
        if(questIsComplete)
            SpaceGameManager.Singleton.planet2Complete = true;
    }
}
