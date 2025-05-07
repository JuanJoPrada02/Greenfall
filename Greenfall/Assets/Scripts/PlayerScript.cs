using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 4f;                   // Velocidad base
    public float runMultiplier = 1.5f;         // Multiplicador al correr
    public float jumpForce = 500f;             // Fuerza del salto

    [Header("Disparo")]
    public GameObject bulletPrefab;            // Prefab de la bala
    public float shotCooldown = 0.3f;          // Tiempo entre disparos

    [Header("Suelo")]
    public Transform groundCheck;              // Origen del overlap circle
    public float checkRadius = 0.1f;
    public LayerMask groundLayer;

    [Header("Vida")]
    public int health = 3;                     // Vida del jugador

    // Componentes internos
    private Rigidbody2D rb2d;
    private Animator animator;
    private Vector3 originalScale;
    private LogicScript logicScript; // Referencia al script de lógica del juego

    // Estados de input y control
    private float horizontalInput;
    private bool runModifier;
    private bool jumpRequest;
    private bool isGrounded;
    private float lastShotTime;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;

        animator = GetComponent<Animator>();
        originalScale = transform.localScale;  // Guardamos el scale del Inspector

        logicScript = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
    }

    void Update()
    {
        // 1) Lectura de inputs
        horizontalInput = Input.GetAxisRaw("Horizontal");
        runModifier = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        // 2) Animaciones
        animator.SetBool("isMoving", horizontalInput != 0);
        

        // 3) Flip sin perder escala
        if (horizontalInput != 0)
        {
            float sign = Mathf.Sign(horizontalInput);
            transform.localScale = new Vector3(
                originalScale.x * sign,
                originalScale.y,
                originalScale.z
            );
        }

        // 4) Detección de suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        // 5) Solicitud de salto
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded)
            jumpRequest = true;

        // 6) Disparo con cooldown
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= lastShotTime + shotCooldown)
        {
            Shoot();
            lastShotTime = Time.time;
        }
    }

    void FixedUpdate()
    {
        // 7) Aplicar movimiento horizontal a través de la física
        float currentSpeed = speed * (runModifier ? runMultiplier : 1f);
        rb2d.linearVelocity = new Vector2(horizontalInput * currentSpeed, rb2d.linearVelocityY);

        // 8) Ajustar gravedad si corres (opcional)
        rb2d.gravityScale = runModifier ? 1.5f : 2f;

        // 9) Ejecutar salto
        if (jumpRequest)
        {
            rb2d.AddForce(Vector2.up * jumpForce);
            jumpRequest = false;
        }
    }

    void Shoot()
    {
        Vector3 dir = transform.localScale.x > 0 ? Vector3.right : Vector3.left;
        Instantiate(bulletPrefab, transform.position + dir * 0.5f, Quaternion.identity)
            .GetComponent<BulletScript>()
            .SetDirection(dir);
    }

    public void Hit(int damage)
    {
        health -= damage;
        if (health <= 0)
            Destroy(gameObject);
            logicScript.GameOver(); // Llama a la función GameOver del LogicScript
    }
}
