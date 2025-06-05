using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Necesario para usar UI

public class LogicScript : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject gameOverPanel;  // Panel de Game Over
    public GameObject pausePanel;     // Panel de Pausa
    public GameObject finishPanel;    // Panel de Finalización del juego
    public int score = 0;       // Texto para mostrar la puntuación
    public Text limpiezaTotalText; // Texto para mostrar la limpieza total
    public Text limpiezaActualText; // Texto para mostrar la limpieza actual
    public int limpiezaTotal = 100; // Puntuación total de limpieza
    public Text MotivacionText; // Texto para mostrar mensajes motivadores

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
        if (score > 100)
        {
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
        MensajeMotivador(); // Muestra un mensaje motivador al finalizar el juego
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // Pausa el juego
    }

    public void MensajeMotivador()
    {
        int num_mensajeMotivador = Random.Range(1, 7); // Genera un número aleatorio entre 1 y 4
        switch (num_mensajeMotivador)
        {
            case 1:
                MotivacionText.text = "¡Sigue así, estás haciendo un gran trabajo!";
                break;
            case 2:
                MotivacionText.text = "¡Cada gota cuenta, sigue limpiando!";
                break;
            case 3:
                MotivacionText.text = "¡Tu esfuerzo está marcando la diferencia!";
                break;
            case 4:
                MotivacionText.text = "¡No te rindas, pronto lo vas a lograr!";
                break;
            case 5:
                MotivacionText.text = "Tip: Saltar mientras corres te hace saltar mas alto";
                break;
            case 6:
                MotivacionText.text = "Tip: Saltar mientras corres te hace saltar mas alto";
                break;
        }

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

        finishPanel.SetActive(true);
        Time.timeScale = 0f; // Pausa el juego
    }

    public void LoadTopDown(string levelName)
    {
        Time.timeScale = 1f; // Asegurar que el tiempo esté normal
        //SceneManager.LoadScene(TopDown);
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Volver()
    {
        Time.timeScale = 1f; // Asegurar que el tiempo esté normal
        SceneManager.LoadScene("Historia1");
    }
    public void CargarMenu()
    {
        Time.timeScale = 1f; // Asegurar que el tiempo esté normal
        SceneManager.LoadScene("PantallaInicio");
    }


}
