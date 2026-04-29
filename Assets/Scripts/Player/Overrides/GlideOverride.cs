using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlideOverride : MonoBehaviour, IMovementOverride
{
    [SerializeField] private float glideGravityMultiplier = 0.5f; // Adjust this value to control how much gravity is reduced while gliding
    [SerializeField] private float maxFallSpeed = 2.5f; // Maximum downward speed while gliding

    // Method to try to override the movement. It checks if the player is falling and applies gliding movement if so.
    public bool TryOverrideMovement(in PlayerFrame currentFrame)
    {
        // If glide is not enables among flags, return false
        if (!currentFrame.flags.IsGliding) return false;
        // IF the player is groudned, return false
        if (currentFrame.sensors.IsGrounded) return false;
        // If the player does not have glide input, return false
        if (!currentFrame.glide) return false;
        // If the player is not falling, return false
        if (currentFrame.motor.RB.velocity.y >= 0f) return false;


        // Apply gliding movement by reducing gravity and capping fall speed
        currentFrame.motor.SetGravityMultiplier(glideGravityMultiplier);
        // Cap the fall speed to the maximum allowed while gliding
        if (currentFrame.motor.RB.velocity.y < -maxFallSpeed)
        {
            currentFrame.motor.SetVelocityY(-maxFallSpeed);
        }

        return true;
    }
}
