using UnityEngine;

public class Camara : MonoBehaviour
{
    internal static object main;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject player; // Reference to the player object

    // Update is called once per frame
    void Update()
    {
        if (player != null) // Check if the player object is not assigned
        {
            Vector3 position = transform.position; // Get the current position of the camera
            position.x = player.transform.position.x + 4; // Set the x position of the camera to the player's x position
            position.y = player.transform.position.y + 1; // Set the y position of the camera to the player's y position
            transform.position = position; // Update the camera's position
        }

    }
}

