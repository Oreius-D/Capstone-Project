using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlags : MonoBehaviour
{
    // Public property to track if gliding is currently active.
    public bool IsGliding { get; private set; } = false;

    // Public property to track if player as a consumable dash available.
    public bool CanDash { get; private set; } = false;

    // Parameters to control glide behavior, such as glide speed multiplier and maximum fall speed while gliding.
    [SerializeField] private float glideGroundGraceTime = 0.35f;
    private float glideLockUntil = -999f;

    // Public properties to track if the player is currently in a lift zone and the target Y position of the lift zone.
    public bool InLiftZone { get; private set; }
    public float LiftTargetY { get; private set; }

    // Field to define if gliding gets deactivated when the player touches the ground.
    [SerializeField] private bool stopGlidingOnGround = true;

    // Properties to acticate or deactivate gliding
    public void ActivateGliding() => IsGliding = true; // Method to activate gliding, setting the IsGliding property to true.
    public void DeactivateGliding() => IsGliding = false; // Method to deactivate gliding, setting the IsGliding property to false.

    // Method to add dash charge, singleuse of dash is allowed, so it sets CanDash to true if player doesn't have a dash charge, otherwise it does nothing.
    public void AddDashCharge()
    {
        if (!CanDash)
            CanDash = true;
    }

    // Method to consume dash charge, setting CanDash to false after use. Usable only if player has a dash charge available (CanDash is true), otherwise it does nothing.
    // Returns boolean value indicating whether the dash charge was successfully consumed (true if it was consumed, false if there was no charge to consume).
    public bool ConsumeDashCharge()
    {
        if(!CanDash) return false; // If there is no dash charge available, return false to indicate that the dash could not be consumed.
        CanDash = false; // Set CanDash to false to indicate that the dash charge has been consumed.
        return true; // Return true to indicate that the dash charge was successfully consumed.
    }

    // OnGrounded method to be called when the player touches the ground. It checks if stopGlidingOnGround is true, and if so, it deactivates gliding by calling DeactivateGliding().
    public void OnGrounded()
    {
        if (!stopGlidingOnGround) return;

        if (InLiftZone) return;
        if (Time.time < glideLockUntil) return;

        DeactivateGliding();
    }

    // Method to be called when the player enters a lift zone, setting InLiftZone to true and storing the target Y position of the lift zone in LiftTargetY.
    // It also activates gliding by calling ActivateGliding() when entering the lift zone.
    public void EnterLiftZone(float targetY)
    {
        InLiftZone = true;
        LiftTargetY = targetY;
        ActivateGliding(); // glide automatically when entering a lift zone, to allow the player to reach the target Y position of the lift zone without falling down.
        glideLockUntil = Time.time + glideGroundGraceTime;
    }

    // Method to be called when the player exits a lift zone, setting InLiftZone to false.
    public void ExitLiftZone()
    {
        InLiftZone = false;
        glideLockUntil = Time.time + glideGroundGraceTime;
    }
}
