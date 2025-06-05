using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 4f;                   // Velocidad base
    public float runMultiplier = 2f;           // Multiplicador al correr
    public float jumpForce = 500f;             // Fuerza del salto

    [Header("Disparo")]
    public BulletScript bulletPrefab;          // Prefab de la bala
    public float shotCooldown = 0.3f;          // Tiempo mínimo entre disparos
    public float bulletSpeed = 20f;            // Velocidad de la bala

    [Header("Suelo")]
    public Transform groundCheck;              // Punto origen para comprobar si está en el suelo
    public float checkRadius = 0.1f;
    public LayerMask groundLayer;

    [Header("Vida")]
    public Health healthScript;                // Referencia al script de salud
    public float invulnerabilityTime = 1f;     // Segundos de invulnerabilidad tras recibir daño
    private float lastTimeHit = -Mathf.Infinity;
    public bool isHit = false;

    // Componentes internos
    private Rigidbody2D rb2d;
    private Animator animator;
    private Vector3 originalScale;
    private LogicScript logicScript;

    // Variables de estado e input
    private float horizontalInput;
    private bool runModifier;
    private bool jumpRequest;
    private bool isGrounded;
    private float lastShotTime;

    // Referencia al gamepad (Nuevo Input System)
    private Gamepad gamepad;

    void Awake()
    {
        // Inicialización de componentes
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb2d.linearDamping = 0.5f; // Fricción

        animator = GetComponent<Animator>();
        originalScale = transform.localScale;

        logicScript = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
        healthScript = GetComponent<Health>();

        // Al iniciar, tomamos Gamepad.current si lo hay
        gamepad = Gamepad.current;
    }

    void Update()
    {
        // Cada frame, verificamos si Gamepad.current ha cambiado o se desconectó
        // Si `Gamepad.current` es distinto (o si pasó de ser null a tener un gamepad),
        // lo asignamos a nuestra variable local `gamepad`.
        if (Gamepad.current != gamepad)
        {
            gamepad = Gamepad.current;
        }

        // 1) Lectura de movimiento horizontal
        if (gamepad != null)
        {
            // Lee el valor del stick izquierdo en el eje X
            horizontalInput = gamepad.leftStick.x.ReadValue();
        }
        else
        {
            // Fallback: teclado (flechas A/D o definidas en Input Manager clásico)
            horizontalInput = Input.GetAxisRaw("Horizontal");
        }

        // 2) Determinar si corre: Shift (teclado) o Gatillo Derecho (RT en mando)
        float triggerValue = 0f;
        if (gamepad != null)
        {
            // rightTrigger devuelve un valor entre 0 y 1
            triggerValue = gamepad.rightTrigger.ReadValue();
        }
        runModifier = Input.GetKey(KeyCode.LeftShift)
                   || Input.GetKey(KeyCode.RightShift)
                   || triggerValue > 0.1f;

        // 3) Animaciones básicas de movimiento
        animator.SetBool("isMoving", Mathf.Abs(horizontalInput) > 0.01f);

        // 4) Flip del sprite según dirección de movimiento
        if (Mathf.Abs(horizontalInput) > 0.01f)
        {
            float sign = Mathf.Sign(horizontalInput);
            transform.localScale = new Vector3(originalScale.x * sign, originalScale.y, originalScale.z);
        }

        // 5) Detección de suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        // 6) Solicitud de salto: teclado (W/UpArrow) o botón A en mando
        bool pressedA = false;
        if (gamepad != null)
        {
            pressedA = gamepad.buttonSouth.wasPressedThisFrame; // “buttonSouth” = botón A en Xbox
        }
        else
        {
            pressedA = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
        }

        if (pressedA && isGrounded)
        {
            jumpRequest = true;
        }

        // 7) Disparo: teclado (Space) o botón B en mando
        bool pressedB = false;
        if (gamepad != null)
        {
            pressedB = gamepad.buttonEast.wasPressedThisFrame; // “buttonEast” = botón B en Xbox
        }
        else
        {
            pressedB = Input.GetKeyDown(KeyCode.Space);
        }

        if (pressedB && Time.time >= lastShotTime + shotCooldown)
        {
            Shoot();
            lastShotTime = Time.time;
            animator.SetTrigger("shoot");
        }

        // 8) Estados de animación de salto y caída
        animator.SetBool("isGrounded", isGrounded);
        float verticalVelocity = rb2d.linearVelocityY;
        if (!isGrounded && verticalVelocity > 0f)
        {
            animator.SetBool("isJumping", true);
            animator.SetBool("isFalling", false);
        }
        else if (!isGrounded && verticalVelocity <= 0f)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", true);
        }
        else
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        }
    }

    void FixedUpdate()
    {
        // 9) Aplicar movimiento horizontal y correr
        float currentSpeed = speed * (runModifier ? runMultiplier : 1f);
        rb2d.linearVelocity = new Vector2(horizontalInput * currentSpeed, rb2d.linearVelocityY);

        // 10) Ajustar gravedad si corre (opcional)
        rb2d.gravityScale = runModifier ? 1.3f : 2f;

        // 11) Ejecutar salto cuando se solicitó
        if (jumpRequest)
        {
            rb2d.AddForce(Vector2.up * jumpForce);
            jumpRequest = false;
        }
    }

    private void Shoot()
    {
        Vector3 dir = transform.localScale.x > 0f ? Vector3.right : Vector3.left;
        Vector3 spawnPos = transform.position + dir * 0.5f;

        BulletScript bs = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
        bs.speed = bulletSpeed;
        bs.SetDirection(dir);

        // Evitar colisión de la bala con el jugador
        Collider2D bulletCol = bs.GetComponent<Collider2D>();
        Collider2D playerCol = GetComponent<Collider2D>();
        if (bulletCol != null && playerCol != null)
            Physics2D.IgnoreCollision(bulletCol, playerCol);
    }

    public void Hit(int damage)
    {
        isHit = true;
        if (Time.time >= lastTimeHit + invulnerabilityTime)
        {
            lastTimeHit = Time.time;
            healthScript.TakeDamage(damage);
        }
    }

    public void AddLife(int value)
    {
        healthScript.AddHealth(value);
    }

    private void OnDrawGizmosSelected()
    {
        // Dibuja un círculo en el editor para visualizar el groundCheck
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}
