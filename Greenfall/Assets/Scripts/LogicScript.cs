using UnityEngine;
using UnityEngine.SceneManagement;

public class LogicScript : MonoBehaviour
{
    [Header("UI Panels")]    
    public GameObject gameOverPanel;  // Panel de Game Over
    public GameObject pausePanel;     // Panel de Pausa

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausePanel.activeSelf)
                ContinueGame();
            else
                PauseGame();
        }
    }

  
    public void PauseGame()
    {
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
    }

    
    public void ContinueGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

   
    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // Pausa el juego
    }

    
    public void RestartGame()
    {
        Time.timeScale = 1f; // Asegurar que el tiempo esté normal
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    
    public void ExitGame()
    {
        Application.Quit();
    }
}
