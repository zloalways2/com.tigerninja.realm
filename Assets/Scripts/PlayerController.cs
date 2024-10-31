using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private float screenWidth;
    private float xMin;
    private float xMax;

    private void Start()
    {
        screenWidth = Screen.width / 2; // Calculate half of the screen width to determine the center
        UpdateCameraBounds(); // Initialize camera bounds
    }

    void Update()
    {
        MoveToTouchOrMouse();
    }

    private void MoveToTouchOrMouse()
    {
        
        float moveX = 0;

        // Check for touch input
        if (Input.touchCount > 0 )
        {
            Touch touch = Input.GetTouch(0);
            moveX = touch.position.x;
        }
        // If no touch input, use mouse input
        else if (Input.GetMouseButton(0) && PlayerManager.Instance.IsGameActive) // Check if the left mouse button is pressed
        {
            moveX = Input.mousePosition.x;
        }

        // Determine the direction of movement based on the x position of the input
        if (moveX > 0)
        {
            if (moveX > screenWidth && PlayerManager.Instance.IsGameActive)
            {
                // If input is on the right side, move right
                if (transform.position.x < xMax)
                {
                    transform.Translate(speed * Time.deltaTime, 0, 0);
                    FlipPlayerDirection(false);
                }
            }
            else
            {
                // If input is on the left side, move left
                if (transform.position.x > xMin)
                {
                    transform.Translate(-speed * Time.deltaTime, 0, 0);
                    FlipPlayerDirection(true);
                }
            }
        }
    }

    private void UpdateCameraBounds()
    {
        // Calculate the camera bounds based on the camera's viewport
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            float screenWidthInWorldUnits = mainCamera.orthographicSize * 2 * Screen.width / Screen.height;
            xMin = mainCamera.transform.position.x - (screenWidthInWorldUnits / 2);
            xMax = mainCamera.transform.position.x + (screenWidthInWorldUnits / 2);
        }
    }

    private void FlipPlayerDirection(bool facingRight)
    {
       
            // Flip the player's scale based on direction
            Vector3 theScale = transform.localScale;
            if (facingRight && theScale.x < 0) // Facing right
            {
                theScale.x = Mathf.Abs(theScale.x); // Set to positive
            }
            else if (!facingRight && theScale.x > 0) // Facing left
            {
                theScale.x = -Mathf.Abs(theScale.x); // Set to negative
            }
            transform.localScale = theScale; // Apply the new scale
        }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player collides with a bomb
        if (other.CompareTag("Bomb")) // Make sure your bomb prefab has the tag "Bomb"
        {
            PlayerManager.Instance.EndGame(); // Call the EndGame method in PlayerManager
        }
    }
}