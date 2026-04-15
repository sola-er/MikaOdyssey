using UnityEngine;

public class UnderwaterManager : MonoBehaviour
{
    public static UnderwaterManager Singleton { get; private set; }

    public int starfishesCollected = 0;
    public int starfishesTarget = 5;

    public bool questIsComplete => starfishesCollected >= starfishesTarget;

    private void Awake()
    {
        if (Singleton == null)
            Singleton = this;
        else
            Destroy(gameObject);

        starfishesCollected = 0;
    }

    public void AddStarfish()
    {
        starfishesCollected++;
        CheckCompletion();
    }

    private void CheckCompletion()
    {
        if (questIsComplete)
        {
            Debug.Log("Underwater quest complete");
        }
    }
}