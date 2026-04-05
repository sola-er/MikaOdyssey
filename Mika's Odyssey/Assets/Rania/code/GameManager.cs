using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}