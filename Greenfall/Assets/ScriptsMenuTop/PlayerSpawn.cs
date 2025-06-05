using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    void Start()
    {
        // Buscamos al jugador por su tag
        GameObject jugador = GameObject.FindWithTag("Player");
        if (jugador != null)
        {
            Vector3 spawnPos = GameManager.nextPlayerSpawn;

            // Si nextPlayerSpawn es distinto de cero, lo aplicamos
            if (spawnPos != Vector3.zero)
            {
                jugador.transform.position = spawnPos;
            }
            // Si fuera Vector3.zero, podrías decidir no moverlo
        }
        else
        {
            Debug.LogError("No se encontró ningún objeto con tag 'Player' en la Escena 3.");
        }
    }
}
