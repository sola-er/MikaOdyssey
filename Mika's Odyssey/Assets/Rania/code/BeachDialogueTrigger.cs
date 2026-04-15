using UnityEngine;

public class BeachDialogueTrigger : MonoBehaviour, IInteractable
{
    [SerializeField] private Dialogue notStarted;
    [SerializeField] private Dialogue inProgress;
    [SerializeField] private Dialogue complete;

    public void Interact()
    {
        Dialogue selectedDialogue;

        if (BeachManager.Singleton.questIsComplete)
        {
            selectedDialogue = complete;
        }
        else if (BeachManager.Singleton.questHasStarted)
        {
            selectedDialogue = inProgress;
        }
        else
        {
            selectedDialogue = notStarted;
            BeachManager.Singleton.questHasStarted = true;
        }

        FindFirstObjectByType<DialogueManager>().StartDialogue(selectedDialogue);
    }
}