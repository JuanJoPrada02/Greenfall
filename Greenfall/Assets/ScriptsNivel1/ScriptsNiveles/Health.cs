using UnityEngine;
using UnityEngine.UI; // Necesario para usar UI

public class Health : MonoBehaviour
{
    public int health = 3; // Vida inicial del jugador
    public int numberOfHearts = 3; // Número de corazones que se mostrarán en la UI
    public Image[] hearts; // Array de imágenes para los corazones
    public Sprite fullHeart; // Sprite del corazón lleno
    public Sprite emptyHeart; // Sprite del corazón vacío

    public LogicScript logicScript; // Referencia al script de lógica del juego

    void Start()
    {
        // Inicializa los corazones en la UI
        for (int i = 0; i < numberOfHearts; i++)
        {
            hearts[i].sprite = fullHeart;
            hearts[i].enabled = true;
        }

        logicScript = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
    }
    public void TakeDamage(int damage)
    {
        health -= damage; // Resta vida al jugador

        // Actualiza la UI de los corazones
        for (int i = 0; i < numberOfHearts; i++)
        {
            if (i < health)
                hearts[i].sprite = fullHeart; // Corazón lleno
            else
                hearts[i].sprite = emptyHeart; // Corazón vacío

            if (i >= numberOfHearts)
                hearts[i].enabled = false; // Desactiva corazones adicionales
        }

        // Si la vida llega a 0, llama a GameOver
        if (health <= 0)
        {
            Destroy(gameObject); // Destruye el objeto jugador
            logicScript.GameOver();
        }
    }

    public void AddHealth(int life)
    {
        health += life; // Aumenta la vida del jugador

        // Asegúrate de no exceder el número máximo de corazones
        if (health > numberOfHearts)
        {
            health = numberOfHearts; // Limita la vida al número máximo de corazones
        }

        // Actualiza la UI de los corazones
        for (int i = 0; i < numberOfHearts; i++)
        {
            if (i < health)
                hearts[i].sprite = fullHeart; // Corazón lleno
            else
                hearts[i].sprite = emptyHeart; // Corazón vacío

            if (i >= numberOfHearts)
                hearts[i].enabled = false; // Desactiva corazones adicionales
        }
    }
}
