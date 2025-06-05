using UnityEngine;

public class HeartDrop : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject player = collision.gameObject;
        if (player.CompareTag("Player"))
        {
            PlayerScript playerScript = player.GetComponent<PlayerScript>();
            if (playerScript != null)
            {
                playerScript.AddLife(1); // Aumenta la vida del jugador en 1
                Destroy(gameObject); // Destruye el objeto de la gota de corazón
            }
        }
    }
}
