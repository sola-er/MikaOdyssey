using UnityEngine;

public class SpaceRaisinCollecting : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        if (Planet0Manager.Singleton.bliQuest != BliQuestState.FinalQuest)
            return;
        Planet0Manager.Singleton.AdvanceQuest(BliQuestState.Complete);
        Destroy(transform.parent.gameObject);
    }
}
