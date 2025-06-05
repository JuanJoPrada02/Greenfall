using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    public int sceneIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            if (currentIndex == 1)
            {
                GameManager.nextPlayerSpawn = new Vector3(13.86874f, 10.79739f);
                SceneManager.LoadScene(2);

            }

            if (currentIndex == 4)
            {
                GameManager.nextPlayerSpawn = new Vector3(-8.026448f, 19.15937f);
                SceneManager.LoadScene(5);
            }
            

        }
    }
}
