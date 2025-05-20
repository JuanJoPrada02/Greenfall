using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Necesario para usar UI

public class LogicScript : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject gameOverPanel;  // Panel de Game Over
    public GameObject pausePanel;     // Panel de Pausa
    public int score = 0;       // Texto para mostrar la puntuación
    public Text limpiezaTotalText; // Texto para mostrar la limpieza total
    public Text limpiezaActualText; // Texto para mostrar la limpieza actual
    public int limpiezaTotal = 100; // Puntuación total de limpieza

    void Start()
    {
        Time.timeScale = 1f;

        limpiezaTotalText.text = " / " + limpiezaTotal.ToString(); // Inicializa el texto de limpieza total

    }

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


    public void AddLimpieza(int value)
    {
        score += value; // Sumar el valor de limpieza al score
        if(score > 100){
            score = 100; // Limitar el score a 100
        }
        limpiezaActualText.text = score.ToString(); // Actualiza el texto de limpieza actual

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

    public void FinishGame()
    {
        Time.timeScale = 0f; // Asegurar que el tiempo esté normal
        //finishPanel.SetActive(true);
    }

    public void LoadTopDown(string levelName)
    {
        Time.timeScale = 1f; // Asegurar que el tiempo esté normal
        //SceneManager.LoadScene(TopDown);
    }
}
