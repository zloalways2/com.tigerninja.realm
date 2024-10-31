using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CollectibleBehavior : MonoBehaviour
{
    public int value; // Value of the collectible
    public float fallSpeed; // Speed at which the collectible falls
    public float destroyLimit;
    //public PlayerManager playerManager;


    private void Start()
    {
        Debug.Log($"destroyLimit {destroyLimit} ");
        int currentLevel = PlayerPrefs.GetInt("SelectedLevel", 1);
        fallSpeed = fallSpeed + (currentLevel * 15f);

    }
    private void Update()
    {
        // Make the collectible fall downwards
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        // Optional: Check if the collectible has fallen below a certain level
        if (transform.position.y < destroyLimit || !PlayerManager.Instance.IsGameActive) // Adjust this value based on your game area
        {
            Destroy(gameObject); // Destroy the collectible if it falls too low
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Call a method to handle collection (e.g., add points)
            Collect();
        }
    }

    private void Collect()
    {
        // Implement your collection logic here, e.g., add score
        PlayerManager.Instance.AddScore(value);
        Destroy(gameObject); // Destroy the collectible after collection
    }
}
