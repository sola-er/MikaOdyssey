using TMPro;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour, IInteractable
{
    public Dialogue dialogue;

    public void Interact() //le findAnyObjectByType might be wrong, we should maybe put FindObjectByType
    {
       FindFirstObjectByType<DialogueManager>().StartDialogue(dialogue);
    }
}
