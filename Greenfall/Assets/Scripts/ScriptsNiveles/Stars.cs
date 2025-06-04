using UnityEngine;
using UnityEngine.UI;

public class Stars : MonoBehaviour
{
    [Header("Configuración de estrellas")]
    [Tooltip("Imágenes UI para cada estrella")]
    public Image[] starImages;
    [Tooltip("Sprite de estrella llena")]
    public Sprite fullStar;

    [Header("Referencias de juego")]
    [Tooltip("Objeto con el script de lógica (puntuación…)")]
    public LogicScript logicScript;
    [Tooltip("Objeto con el script de meta (finishLine.finish)")]
    public FinishLine finishLine;
    [Tooltip("Script del jugador para saber si fue golpeado")]
    public PlayerScript playerScript;

    // Número de estrellas ganadas (0..starImages.Length)
    public int stars = 0;
    // Para asegurarnos de mostrar solo una vez al terminar
    private bool shown = false;

    void Start()
    {
        // Si no lo arrastraste por inspector, los buscamos por Tag
        if (logicScript   == null) logicScript   = GameObject.FindWithTag("Logic")      .GetComponent<LogicScript>();
        if (finishLine    == null) finishLine    = GameObject.FindWithTag("FinishLine") .GetComponent<FinishLine>();
        if (playerScript  == null) playerScript  = GameObject.FindWithTag("Player")      .GetComponent<PlayerScript>();

        // Al inicio, ocultamos todas las estrellas
        for (int i = 0; i < starImages.Length; i++)
            starImages[i].gameObject.SetActive(false);
    }

    void Update()
    {
        // Cuando el jugador llegue a meta (finishLine.finish == true)
        if (!shown && finishLine.finish)
        {
            CalculateStars();
            ShowStars();
            shown = true; 
        }
    }

    // Calcula cuántas estrellas corresponden
    public void CalculateStars()
    {
        stars = 0;

        // 1 estrella si la puntuación >= 80
        if (logicScript.score >= 80)
            stars++;

        // 1 estrella por cruzar la meta
        if (finishLine.finish)
            stars++;

        // 1 estrella si el jugador no fue golpeado
        if (!playerScript.isHit)
            stars++;

        // Nunca exceder el número de imágenes disponibles
        stars = Mathf.Clamp(stars, 0, starImages.Length);
    }

    // Muestra (activa) solo tantas estrellas como valga 'stars'
    public void ShowStars()
    {
        for (int i = 0; i < starImages.Length; i++)
        {
            bool shouldShow = i < stars;
            starImages[i].gameObject.SetActive(shouldShow);
            if (shouldShow)
                starImages[i].sprite = fullStar;
        }
    }
}

