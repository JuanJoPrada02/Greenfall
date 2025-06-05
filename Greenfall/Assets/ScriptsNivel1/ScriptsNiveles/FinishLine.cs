using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{

    public LogicScript logicScript; // Referencia al script de lógica del juego
    public Boolean finish = false; // Variable para indicar si se ha alcanzado la meta
    public Stars star; // Referencia al script de la meta
    public LevelStarsManager starsMgr; // Referencia al gestor de estrellas del nivel
    void Start()
    {

        logicScript = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
        star = GameObject.FindGameObjectWithTag("Star").GetComponent<Stars>();
        starsMgr = GameObject.FindGameObjectWithTag("StarsManager").GetComponent<LevelStarsManager>();
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        finish = true;
        logicScript.FinishGame();

        // 1) recalcular las estrellas antes de mostrarlas
        star.CalculateStars();

        // 2) guardar la cantidad correcta
        int nivel = SceneManager.GetActiveScene().buildIndex;
        int estrellasObt = star.stars;
        Debug.Log($"[FinishLine] guardando {estrellasObt} estrellas en nivel {nivel}");
        starsMgr.ReportLevelCompletion(nivel, estrellasObt);
    }

}