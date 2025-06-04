using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pasoNivel : MonoBehaviour
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void ActivarNivel()
    {
        animator.SetTrigger("Activar");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SceneManager.LoadScene("Nivel_1");
    }
    void Update()
    {
        
    }
}
