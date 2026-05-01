using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalZone : MonoBehaviour
{
    // OnTriggerEnter is called when another collider enters the trigger collider attached to the GameObject this script is on.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.GetComponent<PlayerController>()) return; // Check if the colliding object has a PlayerController component, ensuring that only the player can trigger the goal's effect.
        GameManager.Instance.LoadNextLevel(); // Call the LoadNextLevel method on the GameManager instance to proceed to the next level, allowing the player to advance after reaching the goal.
    }
}
