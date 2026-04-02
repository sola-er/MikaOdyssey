using UnityEngine;

public class LettuceCollecting : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Planet1Manager.Singleton.AdvanceQuest(MochiQuestState.Complete);
        Destroy(transform.parent.gameObject);
    }
}
