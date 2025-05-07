using JetBrains.Annotations;
using UnityEngine;

public class Edge : MonoBehaviour
{
    public LogicScript logicScript; // Referencia al script de lógica del juego
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        logicScript = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
    }

    // Update is called once per frame
       
    public void OnTriggerEnter2D(Collider2D collision)
        {
            // Si el jugador entra en la zona de Game Over, llama a la función GameOver del LogicScript
            if (collision.CompareTag("Player"))
            {
                logicScript.GameOver();
            }
        }
    
}
