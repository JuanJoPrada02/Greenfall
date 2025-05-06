using UnityEngine;

public class BulletScript : MonoBehaviour
{
    //public AudioClip bulletSound; // Sound effect for the bullet
    public float speed = 2f; // Speed of the bullet
    public Rigidbody2D rb; // Rigidbody2D component for the bullet
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Vector2 direction;
    void Start()
    {
        //Camera.main.GetComponent<AudioSource>().PlayOneShot(bulletSound); // Play the bullet sound effect
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the bullet
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        rb.linearVelocityX = direction.x * speed * Time.fixedDeltaTime; // Set the linear velocity of the bullet in the x direction
    }


    // Function to set the direction of the bullet
    public void SetDirection(Vector2 dir)
    {
        direction = dir; // Set the direction of the bullet
       
    }


    public void DestroyBullet()
    {
        Destroy(gameObject); // Destroy the bullet game object
    }


    void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Ground"))
    {
        DestroyBullet();
    }

    EnemyScript enemy = collision.GetComponent<EnemyScript>();
    if (enemy != null)
    {
        enemy.Hit();
        if (enemy.health <= 0)
        {
            enemy.isDead = true;
        }
        DestroyBullet();
    }
}

}
