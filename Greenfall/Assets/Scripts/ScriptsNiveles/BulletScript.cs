using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [Header("Configuración de Bala")]
    public float speed = 20f;            // Velocidad en unidades/segundo
    public float lifeTime = 3f;         // Tiempo antes de autodestruir la bala
    public AudioClip shootSound;       // Sonido al disparar

    // Dirección establecida al instanciar
    private Vector2 direction;

    // Componentes internos
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Destruye la bala tras 'lifeTime' segundos
        Destroy(gameObject, lifeTime);

        Camera.main.GetComponent<AudioSource>().PlayOneShot(shootSound, 0.5f); // Reproduce el sonido de disparo
    }

    void FixedUpdate()
    {
        // Asigna la velocidad según la dirección (normalizada)
        rb.linearVelocity = direction.normalized * speed;
    }

    /// <summary>
    /// Establece la dirección en la que debe viajar la bala.
    /// </summary>
    public void SetDirection(Vector2 dir)
    {
        direction = dir;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Si golpea el suelo, destruye la bala
        if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
            return;
        }

        // Si golpea un enemigo, le aplica daño y destruye la bala
        EnemyScript enemy = collision.GetComponent<EnemyScript>();
        if (enemy != null)
        {
            enemy.Hit();
            Destroy(gameObject);
        }
    }
}
