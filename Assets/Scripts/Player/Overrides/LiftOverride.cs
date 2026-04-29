using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftOverride : MonoBehaviour, IMovementOverride
{
    // Parameters for lift movement
    [SerializeField] private float liftSpeed = 5f; // Speed at which the player is lifted
    [SerializeField] private float epsilon = 0.1f; // Small value to check if the player is close enough to the lift point

    // Method to try to override the movement. It checks if the player is in the lift area and applies lift movement if so.
    public bool TryOverrideMovement(in PlayerFrame currentFrame)
    {
        // If lift is not enables among flags, return false
        if (!currentFrame.flags.InLiftZone) return false;

        // Switch off the player's gravity and apply lift movement
        currentFrame.motor.SetGravityMultiplier(0f);

        // Calculate position and target y position for the lift
        Vector2 position = currentFrame.motor.RB.position;
        float targetY = currentFrame.flags.LiftTargetY;

        // Calculate new x position based on input to allow horizontal movement while in the lift zone
        float newX = position.x + currentFrame.motor.RB.velocity.x * currentFrame.deltaTime;

        // Calculate the new y position based on the lift speed and delta time
        float newY = Mathf.MoveTowards(position.y, targetY, liftSpeed * currentFrame.deltaTime);

        // Move Y avoiding bounces
        currentFrame.motor.RB.MovePosition(new Vector2(newX, newY));

        // Vertical kill for stability
        currentFrame.motor.SetVelocityY(0f);

        // Final Snap
        if (Mathf.Abs(targetY - newY) <= epsilon)
            currentFrame.motor.RB.MovePosition(new Vector2(newX, targetY));

        Debug.Log("Lift override applied.");

        // Block other movement overrides while in the lift zone
        return true;
    }
}
