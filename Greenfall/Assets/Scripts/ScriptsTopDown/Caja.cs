using UnityEngine;
using UnityEngine.SceneManagement;

public class Caja : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject player = collision.gameObject;  

        // Solo reaccionamos si el que entra tiene tag "Player"
        if (collision.CompareTag("Player"))
        {
            // Carga la escena "Tres"
            SceneManager.LoadScene("Level_1");
        }
    }

}
