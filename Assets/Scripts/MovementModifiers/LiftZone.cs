using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftZone : MonoBehaviour
{
    // Transform of high point to reach
    [SerializeField] private Transform topPoint;

    // On trigger enter, call player flags to enter lift zone with the top point y position
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Player entered lift zone");
        // Fetch player flags component from the other collider
        var playerFlags = other.GetComponent<PlayerFlags>();
        if (!playerFlags) return;

        // Call enter lift zone method with the top point y position
        playerFlags.EnterLiftZone(topPoint.position.y);
    }

    // On trigger exit, call player flags to exit lift zone
    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Player exited lift zone");
        // Fetch player flags component from the other collider
        var playerFlags = other.GetComponent<PlayerFlags>();
        if (!playerFlags) return;

        // Call exit lift zone method
        playerFlags.ExitLiftZone();
    }
}