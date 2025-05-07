using UnityEditor.SearchService;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicial : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Jugar()
    {
        Time.timeScale = 1f; // Ensure the game is running at normal speed
        SceneManager.LoadScene("SampleScene");
    }

    // Update is called once per frame
    public void Salir()
    {
        Debug.Log("salir");
        Application.Quit();
    }
}
