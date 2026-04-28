using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceGameOverManager : MonoBehaviour
{
    public static SpaceGameOverManager Singleton { get; private set; }
    [SerializeField] private GameObject gameOverUI;

    private void Awake()
    {
        Singleton = this;
        gameOverUI.SetActive(false);
    }

    public void TriggerGameOver()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
    }
    public static void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}