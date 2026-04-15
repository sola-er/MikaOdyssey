using UnityEngine;

public class BeachManager : MonoBehaviour
{
    public static BeachManager Singleton { get; private set; }

    [Header("Quest")]
    public bool questHasStarted = false;

    [Header("Shells")]
    public int shellsCollected = 0;
    public int shellsTarget = 5;

    public bool questIsComplete => shellsCollected >= shellsTarget;

    private void Awake()
    {
        if (Singleton == null)
            Singleton = this;
        else
            Destroy(gameObject);

        shellsCollected = 0;
    }

    public void AddShell()
    {
        shellsCollected++;
        CheckCompletion();
    }

    private void CheckCompletion()
    {
        if (questIsComplete)
        {
            Debug.Log("Beach quest complete");
        }
    }
}