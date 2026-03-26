using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    private IInteractable currentInteractable;
    [SerializeField] private KeyCode boutonInteractif = KeyCode.E;

    //private void Update()
    //{
    //    if (currentInteractable != null && 
    //        Input.GetKeyDown(boutonInteractif) &&
    //        !DialogueManager.dialogueIsActive)
    //    {
    //        currentInteractable.Interact();
    //    }
    //}
    private void Update()
    {
        if (Input.GetKeyDown(boutonInteractif))
        {
            if (DialogueManager.dialogueIsActive)
                FindFirstObjectByType<DialogueManager>().DisplayNextSentence();
            else if (currentInteractable != null)
                currentInteractable.Interact();
        }
    }
    private void OnTriggerEnter(Collider other) // peut-etre a changer pour mettre quand nous somme dans la zone et pas seulement quand on entre.
    {
        Debug.Log("Entered trigger with: " + other.name);

        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            currentInteractable = interactable;
            Debug.Log("Toucher sur E pour interagir");
        }

    }

    private void OnTriggerExit(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null && interactable == currentInteractable)
        {
            currentInteractable = null;
            Debug.Log("Zone quitte");

        } 
    }


}
