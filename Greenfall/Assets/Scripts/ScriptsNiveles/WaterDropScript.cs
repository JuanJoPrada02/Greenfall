using UnityEngine;

public class WaterDropScript : MonoBehaviour
{
    public int value;
    public AudioClip collectSound;
    public LogicScript logicScript; // Referencia al script de lógica del juego

    void Start()
    {
        logicScript = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();

        if(CompareTag("Gota"))
        {
            value = 2; // Valor de limpieza para el agua
        }
        else if(CompareTag("Trash"))
        {
            value = 5; // Valor de limpieza para el agua
        }
       
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerScript player = collision.GetComponent<PlayerScript>();
        if (player == null) return;

        logicScript.AddLimpieza(value); // Sumar el valor de limpieza al score

        Destroy(gameObject);
    }
    
}
