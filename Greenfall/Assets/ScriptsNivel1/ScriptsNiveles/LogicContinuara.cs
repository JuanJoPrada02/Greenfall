using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Necesario para usar UI

public class LogicContinuara : MonoBehaviour
{
   public void CargarMenu()
    {
        Time.timeScale = 1f; // Asegurar que el tiempo esté normal
        SceneManager.LoadScene("PantallaInicio");
    }
}
