using UnityEngine;

public class SpaceGameManager : MonoBehaviour
{
    public static SpaceGameManager Singleton { get; private set; }

    public bool planet1Complete = false;
    public bool planet2Complete = false;
    public bool planet3Complete = false;
    public bool planet4Complete = false;

    public bool AllPlanetsComplete =>
        planet1Complete && planet2Complete && planet3Complete && planet4Complete;

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
}
