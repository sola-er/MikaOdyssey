using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpaceGameOverManager : MonoBehaviour
{
    public static SpaceGameOverManager Singleton { get; private set; }
    [SerializeField] private GameObject gameOverUI;

    private void Awake()
    {
        if (Singleton == null)
            Singleton = this;
        else
            Destroy(gameObject);
        gameOverUI.SetActive(false);
    }

    public void TriggerGameOver()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}