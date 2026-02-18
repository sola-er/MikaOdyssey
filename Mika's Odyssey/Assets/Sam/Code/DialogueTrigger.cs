using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    public void TriggerDialogue() //le findAnyObjectByType might be wrong, we should maybe put FindObjectByType
    {
       FindFirstObjectByType<DialogueManager>().StartDialogue(dialogue);
    }
}
