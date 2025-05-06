using UnityEngine;

public class PlayerScript : MonoBehaviour
{
   private Animator animator; // Animator component for the player
    private Rigidbody2D rb2d;
    public float speed = 2f; // Speed of the player
    public float jumpForce = 150f; // Jump force of the player
    private bool isGrounded; // Check if the player is on the ground
    public Transform groundCheck;
    public float checkRadius = 0.1f;
    public LayerMask groundLayer;
    private float horizontalInput; // Variable to store the horizontal input
    public int health = 4; // Health of the player
    public bool isDead = false; // Check if the player is dead
    public GameObject bulletPrefab; // Prefab of the bullet to be instantiated
    public float lastShotTime;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
       
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation; // Prevent rotation  

        animator = GetComponent<Animator>(); // Get the Animator component attached to the player
    }


    // Update is called once per frame
    void Update()
    {
        float horizontal = 0f;


        horizontalInput = Input.GetAxisRaw("Horizontal");
        animator.SetBool("isMoving", horizontalInput != 0.0f); // Set the "Running" parameter in the Animator based on horizontal input
        


        if(horizontalInput > 0.0f) {
            transform.localScale = new Vector3(1, 1, 1); // Flip the player to the right
        } else if(horizontalInput < 0.0f) {
            transform.localScale = new Vector3(-1, 1, 1); // Flip the player to the left
        }


        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.A)){
            horizontal = -1f*speed; // Move left
        } else if(Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.D)){
            horizontal = 1f*speed; // Move right
        } else if (Input.GetKey(KeyCode.A)) {
            horizontal = -1f; // Move left
        } else if (Input.GetKey(KeyCode.D)) {
            horizontal = 1f; // Move right
        }


        transform.Translate(new Vector3(horizontal * Time.deltaTime, 0, 0));


       isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);


    if (Input.GetKeyDown(KeyCode.W) && isGrounded)
    {
        Jump();
    }


    Debug.Log("¿Está en el suelo? " + isGrounded);


    if (Input.GetKeyDown(KeyCode.Space) && Time.time > lastShotTime * 0.25f) // Check if the space key is pressed
    {
        Shoot(); // Call the Shoot method to shoot a bullet
        Cooldown(); // Call the Cooldown method to start the cooldown
    }


    }
    public void Jump() {
        rb2d.AddForce(Vector2.up * jumpForce); // Add force to the player to jump
    }


    public void Shoot() {
        Vector3 direction;
        if (transform.localScale.x > 0) // Check the direction the player is facing
        {
            direction = Vector3.right; // Right direction
        }
        else
        {
            direction = Vector3.left; // Left direction
        }


        GameObject bullet = Instantiate(bulletPrefab, transform.position + direction * 0.1f, Quaternion.identity); // Instantiate the bullet prefab at the player's position
        bullet.GetComponent<BulletScript>().SetDirection(direction); // Get the Rigidbody2D component of the bullet
    }
    public void Cooldown() {
        lastShotTime = Time.time; // Set the last shot time to the current time
    }

    public void Hit(int damage)
    {
        // This method is called when the player is hit by an attack
        health -= damage; // Decrease the player's health by 1
        Debug.Log("Salud: " + health); // Log the player's health
        if(health <= 0) // Check if the player's health is less than or equal to 0
        {
            isDead = true; // Set the isDead property to true
            Destroy(gameObject); // Destroy the player game object
        }
    }
   


    void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyScript enemy = collision.GetComponent<EnemyScript>(); // Get the enemy component from the collided object  
        if (enemy !=null) // Check if the collided object has the enemy component
        {
            health -= 1; // Decrease the player's health by 1
            Debug.Log("Salud: " + health); // Log the player's health
            if(health <= 0) // Check if the player's health is less than or equal to 0
            {
                isDead = true; // Set the isDead property to true
                Destroy(gameObject); // Destroy the player game object
            }
           
        }
    }
}


