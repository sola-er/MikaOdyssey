using UnityEngine;
using UnityEngine.SceneManagement; 

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverUI;

    public void TriggerGameOver()
    {
        // Show the UI
        gameOverUI.SetActive(true);
        
        // Freeze the game time
        Time.timeScale = 0f; 
        
        // Unlock the mouse cursor so you can click "Restart"
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        // Reset time and reload the current scene
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}