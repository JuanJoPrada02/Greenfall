using System;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public Boolean isDead = false; // Check if the grunt
    
    private float distanceAttack; // Distance to attack the player
    public int health = 2; // Health of the grunt
    private Animator animator; // Animator component for the grunt
    private float lastAttack; // Last attack time
    public int damage = 1; // Damage dealt by the grunt

    public float patrolRange   = 3f;   // Distancia máxima desde la posición inicial
    public float patrolSpeed   = 2f;   // Velocidad de patrulla
    private Vector3 startPos;          // Posición donde empezó el enemy


    public GameObject player; // Reference to the player object
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
       startPos = transform.position;
    }


    // Update is called once per frame
    void Update()
    {
       
       if(player == null) // Check if the player object is not assigned
        {
            return; // Exit the Update method if the player object is not assigned
        }


        Vector3 directionEnemy = player.transform.position - transform.position; // Calculate the direction from the grunt to the player
        if(directionEnemy.x >= 0.0f) // Check if the player is to the right of the grunt
        {
            transform.localScale = new Vector3(1, 1, 1); // Flip the grunt to the right
        }
        else if(directionEnemy.x < 0) // Check if the player is to the left of the grunt
        {
            transform.localScale = new Vector3(-1, 1, 1); // Flip the grunt to the left
        }


        float distanceShoot = Math.Abs(player.transform.position.x - transform.position.x); // Calculate the distance to the player
        if(distanceAttack < 0.3f) // Check if the distance to the player is greater than 0.5f
        {
        if(!isDead && Time.time > lastAttack + 1f){ // Check if the grunt is not dead and the cooldown time has passed{
            Attack(); // Call the Shoot method to shoot a bullet
            Cooldown(); // Call the Cooldown method to start the cooldown
        }
        }


        // Check if the grunt is dead and destroy the game object
        if(isDead == true)
        {
            Destroy(gameObject); // Destroy the grunt game object
        }

        Patrol(); // Call the Patrol method to move the grunt back and forth
        
    }


    public void Attack() {
        // This method is called when the grunt attacks the player
        animator.SetTrigger("attack"); // Set the attack trigger in the animator
        Debug.Log("Ataque"); // Log the attack action
    }


    public void Cooldown() {
        lastAttack = Time.time; // Set the last shot time to the current time
    }


    public void Hit()
    {
        // This method is called when the player is hit by a bullet
        health -= 1; // Decrease the enemy health by 1
        Debug.Log("Salud: " + health); // Log the enemy health
        if(health <= 0) // Check if the player's health is less than or equal to 0
        {
            isDead = true; // Set the isDead property to true
            Destroy(gameObject); // Destroy the enemy game object
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerScript player = collision.gameObject.GetComponent<PlayerScript>(); // Get the PlayerScript component from the collided object
        if (player != null) // Check if the collided object has the PlayerScript component
        {
            player.Hit(damage); // Call the Hit method
    }

    }

    private void Patrol()
    {
        // PingPong va de 0→rango→0→rango… multiplicado por 2 en el dominio
        float offsetX = Mathf.PingPong(Time.time * patrolSpeed, patrolRange * 2) - patrolRange;
        transform.position = new Vector3(startPos.x + offsetX, transform.position.y, transform.position.z);
        PatrolAnimation(); // Call the PatrolAnimation method to play the patrol animation
    }

    private void PatrolAnimation()
    {
        // Play the patrol animation
        animator.SetBool("isMoving", true); // Set the isPatrol parameter in the animator to true
    }

}
