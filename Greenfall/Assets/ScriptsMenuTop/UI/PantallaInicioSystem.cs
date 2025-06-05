using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewMonoBehaviourScript : MonoBehaviour
{

    [SerializeField] private string url = "https://drive.google.com/file/d/13XolvmGvgPFY745IIUmiFqRAt5l32CYL/view?usp=sharing";
    public void Jugar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Salir()
    {
        Debug.Log("Salien2");
        Application.Quit();
    }

    public void Manual()
    {
        if (!string.IsNullOrEmpty(url))
        {
            Application.OpenURL(url);
        }
        else
        {
            Debug.LogWarning("AbrirEnlace: la URL est� vac�a.");
        }
    }
}
