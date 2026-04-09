using UnityEngine;

public class Planet2DialogueTrigger : MonoBehaviour, IInteractable
{
    [SerializeField] private Dialogue notStarted;
    [SerializeField] private Dialogue inProgress;
    [SerializeField] private Dialogue complete;
    public void Interact()
    {
        Dialogue dialogue;
        if (Planet2Manager.Singleton.questIsComplete)
            dialogue = complete;
        else if (Planet2Manager.Singleton.questHasStarted)
            dialogue = inProgress;
        else
        {
            dialogue = notStarted;
            Planet2Manager.Singleton.questHasStarted = true;
        }
        FindFirstObjectByType<DialogueManager>().StartDialogue(dialogue);
    }
}
