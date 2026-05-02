using UnityEngine;

public class BliDialogueTrigger : MonoBehaviour, IInteractable
{
    [SerializeField] private Dialogue start;
    [SerializeField] private Dialogue ongoing;
    [SerializeField] private Dialogue finalQuest;
    [SerializeField] private Dialogue complete;

    //public void Interact()
    //{
    //    if (SpaceGameManager.Singleton.AllPlanetsComplete &&
    //        Planet0Manager.Singleton.bliQuest == BliQuestState.OngoingSpaceQuest)
    //        Planet0Manager.Singleton.AdvanceQuest(BliQuestState.FinalQuest);

    //    Dialogue dialogue = Planet0Manager.Singleton.bliQuest switch
    //    {
    //        BliQuestState.Start => start,
    //        BliQuestState.OngoingSpaceQuest => ongoing,
    //        BliQuestState.FinalQuest => finalQuest,
    //        BliQuestState.Complete => complete,
    //        _ => start
    //    };

    //    FindFirstObjectByType<DialogueManager>().StartDialogue(dialogue);
    //}
    public void Interact()
    {
        if (SpaceGameManager.Singleton.AllPlanetsComplete &&
            Planet0Manager.Singleton.bliQuest == BliQuestState.OngoingSpaceQuest)
            Planet0Manager.Singleton.AdvanceQuest(BliQuestState.FinalQuest);

        Dialogue dialogue = Planet0Manager.Singleton.bliQuest switch
        {
            BliQuestState.Start => start,
            BliQuestState.OngoingSpaceQuest => ongoing,
            BliQuestState.FinalQuest => finalQuest,
            BliQuestState.Complete => complete,
            _ => start
        };

        FindFirstObjectByType<DialogueManager>().StartDialogue(dialogue);

        if (Planet0Manager.Singleton.bliQuest == BliQuestState.Start)
            Planet0Manager.Singleton.AdvanceQuest(BliQuestState.OngoingSpaceQuest);
    }
}