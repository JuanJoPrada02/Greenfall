using UnityEngine;

public class Spikes : MonoBehaviour
{
   private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerScript p = collision.GetComponent<PlayerScript>();
            if (p != null) 
            {// Infligir daño al jugador
                p.Hit(1);
            }
        }
    }
}
