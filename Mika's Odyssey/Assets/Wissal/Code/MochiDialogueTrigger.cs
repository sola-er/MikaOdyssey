using UnityEngine;

public class MochiDialogueTrigger : MonoBehaviour, IInteractable
{
    [SerializeField] private Dialogue notStarted;
    [SerializeField] private Dialogue platformIsGone;
    [SerializeField] private Dialogue switchActivated;
    [SerializeField] private Dialogue complete;
    public void Interact()
    {
        Dialogue dialogue = Planet1Manager.Singleton.mochiQuest switch
        {
            MochiQuestState.NotStarted => notStarted,
            MochiQuestState.PlatformIsGone => platformIsGone,
            MochiQuestState.SwitchActivated => switchActivated,
            MochiQuestState.Complete => complete,
            _ => notStarted
        };
        FindFirstObjectByType<DialogueManager>().StartDialogue(dialogue);
    }
}
