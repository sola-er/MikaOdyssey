using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    private GameOverManager _gameManager;

    void Start()
    {
        // Find the manager in the scene
        _gameManager = Object.FindFirstObjectByType<GameOverManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object we hit has the "Player" tag
        if (collision.gameObject.CompareTag("Player"))
        {
            _gameManager.TriggerGameOver();
        }
    }
}