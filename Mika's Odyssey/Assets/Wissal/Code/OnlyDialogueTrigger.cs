using UnityEngine;

public class OnlyDialogueTrigger : MonoBehaviour, IInteractable
{
    [SerializeField] private Dialogue[] dialogues;
    [SerializeField] private int planetNumber = 3;
    private int dialogueIndex = 0;


    public void Interact()
    {
        int index = Mathf.Clamp(dialogueIndex, 0, dialogues.Length - 1);
        FindFirstObjectByType<DialogueManager>().StartDialogue(dialogues[index]);
        dialogueIndex++;
        if (dialogueIndex >= dialogues.Length)
        {
            if(planetNumber == 3)
                SpaceGameManager.Singleton.planet3Complete = true;
            else if(planetNumber == 4)
                SpaceGameManager.Singleton.planet4Complete = true;
        }
    }
}