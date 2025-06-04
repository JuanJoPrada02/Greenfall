using System;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 4f;                   // Velocidad base
    public float runMultiplier = 2f;         // Multiplicador al correr
    public float jumpForce = 500f;             // Fuerza del salto
   

    [Header("Disparo")]
    public BulletScript bulletPrefab;            // Prefab de la bala
    public float shotCooldown = 0.3f;          // Tiempo entre disparos
    public float bulletSpeed = 20f;            // Velocidad de la bala

    [Header("Suelo")]
    public Transform groundCheck;              // Origen del overlap circle
    public float checkRadius = 0.1f;
    public LayerMask groundLayer;

    [Header("Vida")]
    public Health healthScript;                     // Vida del jugador
    public float invulnerabilityTime = 1f; // Tiempo de invulnerabilidad tras recibir daño
    public float lastTimeHit = -Mathf.Infinity; // Último tiempo que recibió daño
    public Boolean isHit = false; // Indica si el jugador ha sido golpeado

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
        rb2d.linearDamping = 0.5f; // Fricción del jugador

        animator = GetComponent<Animator>();
        originalScale = transform.localScale;  // Guardamos el scale del Inspector

        logicScript = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
        healthScript = GetComponent<Health>(); // Inicializa el script de salud
        
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
            transform.localScale = new Vector3(originalScale.x * sign, originalScale.y, originalScale.z);
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
            animator.SetTrigger("shoot"); // Disparar animación
        }

        animator.SetBool("isGrounded", isGrounded);
        if (!isGrounded && rb2d.linearVelocityY > 0f)
    {
        animator.SetBool("isJumping", true);
        animator.SetBool("isFalling", false);
    }
    // 3) En el ápice / descendiendo (fall)
    else if (!isGrounded && rb2d.linearVelocityY <= 0f)
    {
        animator.SetBool("isJumping", false);
        animator.SetBool("isFalling", true);
    }
    // 4) En suelo de nuevo
    else
    {
        animator.SetBool("isJumping", false);
        animator.SetBool("isFalling", false);
        animator.SetBool("isGrounded", true);
    }

    }

    void FixedUpdate()
    {
        // 7) Aplicar movimiento horizontal a través de la física
        float currentSpeed = speed * (runModifier ? runMultiplier : 1f);
        rb2d.linearVelocity = new Vector2(horizontalInput * currentSpeed, rb2d.linearVelocityY);

        // 8) Ajustar gravedad si corres (opcional)
        rb2d.gravityScale = runModifier ? 1.3f : 2f; // Ajusta la gravedad al correr

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
        Vector3 spawnPos = transform.position + dir * 0.5f;

    // Esta sobrecarga de Instantiate devuelve directamente un BulletScript
        BulletScript bs = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
        bs.speed = bulletSpeed;
        bs.SetDirection(dir);

        Collider2D bulletCol = bs.GetComponent<Collider2D>();
        Collider2D playerCol = GetComponent<Collider2D>();
        if (bulletCol != null && playerCol != null)
            Physics2D.IgnoreCollision(bulletCol, playerCol);
    }

    public void Hit(int damage)
    {
        isHit = true; // Marcar como golpeado
        if (Time.time >= lastTimeHit + invulnerabilityTime)
        {
            lastTimeHit = Time.time;
            healthScript.TakeDamage(damage);
        }
        
    }

    public void AddLife(int value)
    {
        healthScript.AddHealth(value); // Añadir vida al jugador
    }
}
