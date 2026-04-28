using System;
using UnityEngine;
public class HeartsManager : MonoBehaviour
{
    public static HeartsManager Singleton { get; private set; }
    [SerializeField] private int maxHearts = 3;
    public int currentHearts;
    public static event Action<int> OnHeartsChanged;

    private void Awake()
    {
        Singleton = this;
        OnHeartsChanged = null;
        currentHearts = maxHearts;
    }
    private void Start()
    {
        OnHeartsChanged?.Invoke(currentHearts);
    }
    public void LoseHeart()
    {
        --currentHearts;
        OnHeartsChanged?.Invoke(currentHearts);
        if (currentHearts <= 0)
            SpaceGameOverManager.Singleton.TriggerGameOver();
    }

    public void ResetHearts()
    {
        currentHearts = maxHearts;
        OnHeartsChanged?.Invoke(currentHearts);
    }
}