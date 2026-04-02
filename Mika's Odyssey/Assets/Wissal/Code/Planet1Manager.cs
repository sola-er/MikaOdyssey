using UnityEngine;

public enum MochiQuestState
{
    NotStarted,
    PlatformIsGone,
    SwitchActivated,
    Complete
}
public class Planet1Manager : MonoBehaviour
{
    public static Planet1Manager Singleton { get; private set; }
    public MochiQuestState mochiQuest = MochiQuestState.NotStarted;

    private void Awake()
    {
        if (Singleton == null)
            Singleton = this;
        else
            Destroy(gameObject);
    }
    public void AdvanceQuest(MochiQuestState newState)
    {
        if (newState > mochiQuest)
            mochiQuest = newState;
        if (mochiQuest == MochiQuestState.Complete)
            SpaceGameManager.Singleton.planet1Complete = true;
    }
}
