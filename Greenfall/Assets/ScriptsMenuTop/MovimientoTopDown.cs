using System;
using Unity.VisualScripting;
using UnityEngine;

public class MovimientoTopDown : MonoBehaviour
{
    private float speed = 5f;
    private Vector2 direccion;
    private Rigidbody2D rb2D;
    
    private Animator animator;
    

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        direccion.x = Input.GetAxisRaw("Horizontal");
        direccion.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", direccion.x);
        animator.SetFloat("Vertical", direccion.y);
        animator.SetFloat("Speed", direccion.magnitude);


        if(direccion.x !=0|| direccion.y != 0)
        {
            animator.SetFloat("UltimoX",direccion.x);
            animator.SetFloat("UltimoY", direccion.y);
        }
        direccion = direccion.normalized;
    }

    private void FixedUpdate()
    {
        rb2D.linearVelocity = direccion * speed;
    }

}
