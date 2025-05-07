using UnityEngine;
using UnityEngine.InputSystem; // Importante para el nuevo Input System
using UnityEngine.SceneManagement;

public class MovimientoTopDown : MonoBehaviour
{
    [SerializeField] private float velocidadMovimiento;
    private Vector2 direccion;
    private float movimientoX;
    private float movimientoY;

    private Animator animator;
    private Rigidbody2D rb2D;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Keyboard.current != null)
        {
            // Movimiento con teclado
            movimientoX = (Keyboard.current.aKey.isPressed ? -1 : 0) + (Keyboard.current.dKey.isPressed ? 1 : 0);
            movimientoY = (Keyboard.current.sKey.isPressed ? -1 : 0) + (Keyboard.current.wKey.isPressed ? 1 : 0);
        }
        else if (Gamepad.current != null)
        {
            // Movimiento con joystick
            Vector2 input = Gamepad.current.leftStick.ReadValue();
            movimientoX = input.x;
            movimientoY = input.y;
        }

        animator.SetFloat("MovimientoX", movimientoX);
        animator.SetFloat("MovimientoY", movimientoY);

        if (movimientoX != 0 || movimientoY != 0)
        {
            animator.SetFloat("UltimoX", movimientoX);
            animator.SetFloat("UltimoY", movimientoY);
        }

        direccion = new Vector2(movimientoX, movimientoY).normalized;
    }

    
    private void FixedUpdate()
    {
        rb2D.MovePosition(rb2D.position + direccion * velocidadMovimiento * Time.fixedDeltaTime);
    }
}