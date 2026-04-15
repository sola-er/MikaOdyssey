using UnityEngine;

public class RaisinCollecting : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        if (Planet0Manager.Singleton.bliQuest != BliQuestState.FinalQuest)
            return;
        Planet0Manager.Singleton.AdvanceQuest(BliQuestState.Complete);
        gameObject.SetActive(false);
    }
}