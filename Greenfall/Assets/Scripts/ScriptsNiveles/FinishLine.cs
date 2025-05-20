using UnityEngine;

public class FinishLine : MonoBehaviour{

    LogicScript logicScript; // Referencia al script de lógica del juego

void Start()
{

 logicScript = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();

}


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            {
                logicScript.GameOver();
            }
    }

}