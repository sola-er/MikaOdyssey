using UnityEngine;

public enum BliQuestState
{
    Start,
    OngoingSpaceQuest,
    FinalQuest,
    Complete
}

public class Planet0Manager : MonoBehaviour
{
    public static Planet0Manager Singleton { get; private set; }
    public BliQuestState bliQuest = BliQuestState.Start;

    private void Awake()
    {
        if (Singleton == null)
            Singleton = this;
        else
            Destroy(gameObject);
    }

    public void AdvanceQuest(BliQuestState newState)
    {
        if (newState > bliQuest)
            bliQuest = newState;
    }
}