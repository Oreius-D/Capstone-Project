using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlags : MonoBehaviour
{
    // Public property to track if gliding is currently active.
    public bool IsGliding { get; private set; } = false;

    // Public property to track if player as a consumable dash available.
    public bool CanDash { get; private set; } = false;

    // Public properties to track if the player is currently in a lift zone and the target Y position of the lift zone.
    public bool InLiftZone { get; private set; }
    public float LiftTargetY { get; private set; }

    // Public property to track if the dash ability is unlocked, allowing other scripts to check if the player has access to the dash ability.
    public bool DashUnlocked { get; private set; }

    // Parameters to control glide behavior, such as glide speed multiplier and maximum fall speed while gliding.
    [SerializeField] private float glideGroundGraceTime = 0.35f;
    private float glideLockUntil = -999f;

    // Field to define if gliding gets deactivated when the player touches the ground.
    [SerializeField] private bool stopGlidingOnGround = true;

    // Event for when the player gets a dash charge, allowing other scripts to subscribe and react to this event when a dash charge is added.
    public event Action<bool> onDashChargeAdded; // Event that takes a boolean parameter indicating whether a dash charge was added (true) or consumed (false).

    // Methods to activate or deactivate gliding
    public void ActivateGliding() => IsGliding = true; // Method to activate gliding, setting the IsGliding property to true.
    public void DeactivateGliding() => IsGliding = false; // Method to deactivate gliding, setting the IsGliding property to false.

    // Method to add dash charge, singleuse of dash is allowed, so it sets CanDash to true if player doesn't have a dash charge, otherwise it does nothing.
    public void AddDashCharge()
    {
        if(!DashUnlocked) return; // If the dash ability is not unlocked, do not add a dash charge and return early to prevent the player from gaining access to the dash ability before it is supposed to be available.

        if (!CanDash)
            CanDash = true;
        onDashChargeAdded?.Invoke(CanDash);
    }

    // Method to consume dash charge, setting CanDash to false after use. Usable only if player has a dash charge available (CanDash is true), otherwise it does nothing.
    // Returns boolean value indicating whether the dash charge was successfully consumed (true if it was consumed, false if there was no charge to consume).
    public bool ConsumeDashCharge()
    {
        if(!DashUnlocked) return false; // If the dash ability is not unlocked, do not consume a dash charge and return false to indicate that the dash could not be consumed because the ability is not available.
        if (!CanDash) return false; // If there is no dash charge available, return false to indicate that the dash could not be consumed.
        CanDash = false; // Set CanDash to false to indicate that the dash charge has been consumed.
        onDashChargeAdded?.Invoke(CanDash); // Invoke the onDashChargeAdded event with false to indicate that a dash charge was consumed.
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

    // OnEnable method to subscribe to the GameManager's OnDashUnlock event, allowing the PlayerFlags to react to changes in the dash unlock state.
    private void OnEnable()
    {
        GameManager.OnDashUnlock += HandleDashUnlock;

        if (GameManager.Instance != null)
            HandleDashUnlock(GameManager.Instance.IsDashUnlocked);
    }

    // OnDisable method to unsubscribe from the GameManager's OnDashUnlock event, ensuring that the PlayerFlags does not react to events when it is disabled.
    private void OnDisable()
    {
        GameManager.OnDashUnlock -= HandleDashUnlock;
    }

    // HandleDashUnlock method to update the DashUnlocked property based on the value received from the GameManager's OnDashUnlock event, allowing the PlayerFlags to keep track of whether the dash ability is unlocked.
    private void HandleDashUnlock(bool isUnlocked)
    {
        DashUnlocked = isUnlocked;

        // If the dash ability is unlocked, we can add a dash charge to the player to allow them to use the dash immediately. This is optional and can be adjusted based on game design preferences.
        if(!DashUnlocked && CanDash)
        {
            CanDash = false; // If the dash ability is locked again (which is unlikely but possible in some game designs), we can remove any existing dash charge from the player to prevent them from using the dash when it is not supposed to be available.
            onDashChargeAdded?.Invoke(CanDash); // Invoke the onDashChargeAdded event with false to indicate that a dash charge was removed due to the dash ability being locked.
        }
    }
}
