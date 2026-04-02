using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] private KeyCode boutonInteractif = KeyCode.E;
    [SerializeField] GameObject promptUI;

    private IInteractable currentInteractable;
    private void Awake()
    {
        Debug.Assert(promptUI != null);
    }
    private void Update()
    {
        if (currentInteractable != null && (currentInteractable as MonoBehaviour) == null)
            currentInteractable = null;
        if (Input.GetKeyDown(boutonInteractif))
        {
            if (DialogueManager.dialogueIsActive)
                FindFirstObjectByType<DialogueManager>().DisplayNextSentence();
            else if (currentInteractable != null)
                currentInteractable.Interact();
        }
        promptUI.SetActive(currentInteractable != null && !DialogueManager.dialogueIsActive);
    }
    private void OnTriggerEnter(Collider other) // peut-etre a changer pour mettre quand nous somme dans la zone et pas seulement quand on entre.
    {
        Debug.Log("Entered trigger with: " + other.name);

        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            currentInteractable = interactable;
            promptUI.SetActive(true);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null && interactable == currentInteractable)
        {
            currentInteractable = null;
            Debug.Log("Zone quittee");

        } 
    }
}
