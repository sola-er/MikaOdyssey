using UnityEngine;

public class TrashCollecting : MonoBehaviour, IInteractable
{
    [SerializeField] int trashTarget = 5;
    public static int collectedTrashCounter = 0;
    public void Interact()
    {
        ++collectedTrashCounter;
        Destroy(transform.parent.gameObject);
        if (collectedTrashCounter >= trashTarget)
        {
            Planet2Manager.Singleton.trashComplete = true;
            Planet2Manager.Singleton.CheckCompletion();
        }
    }
}
