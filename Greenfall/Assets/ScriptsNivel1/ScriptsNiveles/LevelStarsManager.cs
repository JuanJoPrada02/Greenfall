using System.IO;
using UnityEngine;

public class LevelStarsManager : MonoBehaviour
{
    // Nombre del fichero donde guardamos el progreso
    private const string FileName = "levelStars.txt";

    public int[] starsPerLevel;

    // Ruta completa al archivo en disco
    private string filePath => Path.Combine(Application.dataPath, "levelStars.txt");


    void Awake()
    {
        LoadStarsFromFile();
    }

    private void LoadStarsFromFile()
    {
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            for (int i = 0; i < starsPerLevel.Length; i++)
            {
                if (i < lines.Length && int.TryParse(lines[i], out int s))
                    starsPerLevel[i] = s;
                else
                    starsPerLevel[i] = 0;
            }
        }
        else
        {
            // No existe todavía: inicializamos todo a cero
            for (int i = 0; i < starsPerLevel.Length; i++)
                starsPerLevel[i] = 0;
        }
    }

    private void SaveStarsToFile()
    {
        string[] lines = new string[starsPerLevel.Length];
        for (int i = 0; i < starsPerLevel.Length; i++)
            lines[i] = starsPerLevel[i].ToString();

        File.WriteAllLines(filePath, lines);
        Debug.Log($"[LevelStarsManager] Guardado {starsPerLevel.Length} niveles en:\n{filePath}");
    }
    public void ReportLevelCompletion(int levelIndex, int starsAchieved)
    {
        if (levelIndex < 0 || levelIndex >= starsPerLevel.Length)
        {
            Debug.LogError($"LevelStarsManager: índice de nivel {levelIndex} fuera de rango");
            return;
        }
        // Solo sobreescribimos si hemos mejorado la marca
        if (starsAchieved > starsPerLevel[levelIndex])
        {
            starsPerLevel[levelIndex] = starsAchieved;
            SaveStarsToFile();
        }

        Debug.Log($"Guardando progreso en: {filePath}");
    }
}
