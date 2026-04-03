using UnityEngine;

public class DomeSwitchTrigger : MonoBehaviour, IInteractable
{
    [SerializeField] private PlatformTrigger platformTrigger;
    public void Interact()
    {
        Planet1Manager.Singleton.AdvanceQuest(MochiQuestState.SwitchActivated);
        platformTrigger.MoveBack();
        Destroy(gameObject);
    }
}
