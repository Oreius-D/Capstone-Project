using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HazardKill : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.GetComponent<PlayerController>()) return; // Check if the colliding object has a PlayerController component, ensuring that only the player can trigger the hazard's effect.

        Debug.Log("[HazardKill] Player hit!");
        if (GameManager.Instance != null) GameManager.Instance.RestartLevel(); // Call the RestartLevel method on the GameManager instance to restart the current level, allowing the player to try again after triggering the hazard.
        else SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
