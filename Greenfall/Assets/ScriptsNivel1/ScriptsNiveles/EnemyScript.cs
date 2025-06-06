using UnityEditor;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [Header("Vida y Daño")]
    public int health;               // Vida del enemigo
    public int damage;               // Daño al jugador
    public int valorLimpieza; // Valor de limpieza al ser destruido
    public int dropChance = 2; // Probabilidad de soltar un objeto (1 de 5)
    public int seed;

    [Header("Rangos y Tiempos")]
    public float patrolRange;     // Distancia máxima desde la posición inicial
    public float patrolSpeed;         // Velocidad de patrulla
    public float attackRange;    // Rango para iniciar ataque
    public float attackCooldown; // Segundos entre ataques
    public float bulletSpeed; // Velocidad de la bala
    public bool invertimos = false; // Si el enemigo patrulla de forma invertida
    

    [Header("Referencias")]
    public PlayerScript player;      // Referencia al jugador
    public LogicScript logicScript; // Referencia al script de lógica del juego
    // Componentes internos
    private Animator animator;
    private Vector3 originalScale;
    private Vector3 startPos;
    private float lastAttackTime;
    private Rigidbody2D rb2d;
    private float previousX;         // Para detectar cambio de dirección
    public GameObject dropPrefab; // Prefab del objeto que puede soltar
    public GameObject bulletPrefab; // Prefab de la bala
    

    void Start()
    {
        // Inicialización de componentes
        animator = GetComponent<Animator>();
        originalScale = transform.localScale;
        startPos = transform.position;
        logicScript = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();

        // Desactivar gravedad si hay Rigidbody2D y congelar rotación
        rb2d = GetComponent<Rigidbody2D>();
        if (rb2d != null)
        {
            rb2d.gravityScale = 0f;
            rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        // Configuración según tag
        if (CompareTag("Mutalga"))
        {
            health = 4;
            damage = 2;
            patrolSpeed = 2f;
            valorLimpieza = 5;
            attackRange = 1f;
            patrolRange = 3f;
            attackCooldown = 1f;
        }
        else if (CompareTag("Neurix"))
        {
            health = 3;
            damage = 1;
            patrolSpeed = 3f;
            valorLimpieza = 3;
            patrolRange = 3f;
        } 
        else if (CompareTag("Lizard"))
        {
            health = 3;
            damage = 1;
            patrolSpeed = 2f;
            valorLimpieza = 7;
            attackRange = 8f;
            patrolRange = 6f;
            attackCooldown = 1f;
            bulletSpeed = 10f;
        }

        // Guardamos posición inicial en X para Face()
        previousX = transform.position.x;

    }

    void Update()
    {
        if (player == null || health <= 0) return;

        float dist = Vector3.Distance(transform.position, player.transform.position);

        // Ataque cuando el jugador esté en rango y el cooldown haya pasado
        if (dist <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            if (CompareTag("Lizard"))
            {
                animator.SetBool("isMoving", false);
                Shoot();
                lastAttackTime = Time.time;
                animator.SetTrigger("attack");
                
            }
            else if (CompareTag("Mutalga"))
            {
                Attack();
                lastAttackTime = Time.time;
            }
            
        }

        if (health <= 0) Die();
    }

    void FixedUpdate()
    {
        if (player == null || health <= 0) return;

        // Patrulla siempre
        Patrol();

        // Voltear según cambio de dirección de movimiento
        Face();

        // Actualizamos previousX tras mover y voltear
        previousX = transform.position.x;
    }

    private void Patrol()
    {
        if(CompareTag("Mutalga") || CompareTag("Neurix"))
        {
           float offsetX = Mathf.PingPong(Time.time * patrolSpeed, patrolRange * 2f) - patrolRange;
           // Fijar Y en startPos para que no se caiga
            transform.position = new Vector3(startPos.x + offsetX, startPos.y, transform.position.z);
            if(invertimos)
            {
                // Invertir la dirección de patrulla
                transform.position = new Vector3(startPos.x - offsetX, startPos.y, transform.position.z);
            }
        } else if(CompareTag("Lizard"))
        {
            // Lizard patrulla entre dos puntos
            float offsetX = Mathf.PingPong(Time.time * patrolSpeed, patrolRange);
    // Restamos ese offset a startPos.x: mueve de startPos.x a (startPos.x – patrolRange) y de vuelta
            transform.position = new Vector3(startPos.x - offsetX, startPos.y, transform.position.z);
            if(invertimos)
            {
                transform.position = new Vector3(startPos.x - offsetX, startPos.y, transform.position.z);
            }
        } 

        if (CompareTag("Mutalga"))
            animator.SetBool("isMoving", true);
        else if (CompareTag("Lizard"))
            animator.SetBool("isMoving", true);
        
    }

    private void Face()
    {
        // Calcula delta X desde el frame anterior
        float delta = transform.position.x - previousX;
        if (Mathf.Abs(delta) > 0.01f)
        {
            float sign = -Mathf.Sign(delta);
            transform.localScale = new Vector3(originalScale.x * sign, originalScale.y, originalScale.z);
        }
    }

    private void Attack()
    {
        if (CompareTag("Mutalga"))
            animator.SetTrigger("attack");
        // Infligir daño al jugador directamente
        player.Hit(damage);
    }

    private void Shoot()
    {
        // Determinar dirección basada en la escala X del sprite,
        // invertimos el signo para corregir la orientación.
        float signX = -Mathf.Sign(transform.localScale.x);
        Vector2 direction = new Vector2(signX, 0f);

        // Generar posición de spawn justo delante del enemigo
        Vector3 spawnPos = transform.position + (Vector3)direction * 0.5f;

        // Instanciar la bala y establecer su dirección
        GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
        BulletScript bs = bullet.GetComponent<BulletScript>();

        bs.speed = bulletSpeed;       
        bs.SetDirection(direction);

        // Ignorar colisión con el propio enemigo
        Collider2D bulletCol = bullet.GetComponent<Collider2D>();
        Collider2D enemyCol = GetComponent<Collider2D>();
        if (bulletCol != null && enemyCol != null)
            Physics2D.IgnoreCollision(bulletCol, enemyCol);
    }

    public void Hit()
    {
        health--;
        if (health <= 0) Die();
    }

    private void Die()
    {
        seed = Random.Range(1, 3); // Generar un número aleatorio entre 1 y 2
        Debug.Log("Seed: " + seed); // Mostrar el número en la consola
        // Si el número aleatorio es igual a la probabilidad de soltar objeto, lo soltamos
        if(seed == dropChance)
        {
            Instantiate(dropPrefab, transform.position, Quaternion.identity); // Soltar objeto
        }
        logicScript.AddLimpieza(valorLimpieza); // Añadir valor de limpieza al jugador
        Destroy(gameObject, 0.3f);
    }

    // Daño por choque si usas trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerScript p = collision.GetComponent<PlayerScript>();
            if (p != null) 
            {// Infligir daño al jugador
                p.Hit(damage);
            }
        }
    }
}
