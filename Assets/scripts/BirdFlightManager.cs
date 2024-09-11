using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdFlightManager : MonoBehaviour
{
    public float speed = 1f;
    private float spriteWidth; // To store the width of the sprite in world units

    public float offScreenOffset = 2f; // Distance behind the left side of the screen where the bird will reappear
    public float travelDistance = 10f; // Distance the bird travels past the right edge before disappearing

    void Start()
    {
        // Get the sprite's width in world units
        spriteWidth = GetComponent<SpriteRenderer>().bounds.size.x;

        // Position the sprite just off the left side of the screen
        Vector3 leftEdge = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        transform.position = new Vector3(leftEdge.x - spriteWidth - offScreenOffset, transform.position.y, transform.position.z);
    }

    void Update()
    {
        // Move the sprite to the right
        transform.position += new Vector3(speed * Time.deltaTime, 0, 0);

        // Get the right edge of the screen in world units
        Vector3 rightEdge = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, Camera.main.nearClipPlane));

        // If the sprite has traveled past the right edge plus the travel distance, reset its position
        if (transform.position.x - spriteWidth > rightEdge.x + travelDistance)
        {
            // Reset the position to two units behind the left edge of the screen
            Vector3 leftEdge = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
            transform.position = new Vector3(leftEdge.x - spriteWidth - offScreenOffset, transform.position.y, transform.position.z);
        }
    }
}


