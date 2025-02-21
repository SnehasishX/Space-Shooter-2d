using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance;  // Singleton instance
    public GameObject gameOverPanel;  // The Game Over panel
    public Text scoreText;            // Text element to show the score

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of GameOverManager exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicate instances if any
        }
    }

    private void Start()
    {
        gameOverPanel.SetActive(false);  // Hide the Game Over screen initially
    }

    public void ShowGameOverScreen(int score)
    {
        scoreText.text = "Score: " + score;  // Display the score in the UI
        gameOverPanel.SetActive(true);  // Show the Game Over screen
    }

    // Retry Button: Restart the game
    public void RetryGame()
    {
        // Reload the current scene to restart the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Exit Button: Quit the game
    public void ExitGame()
    {
        // Exit the game
        Application.Quit();

        // If you are testing in the Unity editor, this will stop play mode
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
