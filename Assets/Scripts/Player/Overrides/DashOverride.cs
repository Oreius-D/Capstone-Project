using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashOverride : MonoBehaviour, IMovementOverride
{
    // variables for dash impulse, vertical movement change and treshold to avoid immediate re-stucking on walls
    [SerializeField] private float dashImpulse = 20f;
    [SerializeField] private float restuckAvoid = 0.06f;

    // Variable to track how long to avoid restucking after a dash
    private float blockStuckUntil;

    // Method to try to override the movement. It checks if the player is trying to dash and applies the dash impulse if so.
    public bool TryOverrideMovement(in PlayerFrame currentFrame)
    {
        // Check if time is less than the blockStuckUntil, if so, return false to avoid restucking
        if (Time.time < blockStuckUntil) return false;

        // Check if player dasked. If so return false since he has expended his dash
        if (!currentFrame.dashPressed) return false;
        if (!currentFrame.flags.ConsumeDashCharge()) return false;

        // Set direction of the dash based on player input, if no input, dash in the facing direction. Default to right if no facing direction
        float inputX = currentFrame.inputX;
        if (Mathf.Abs(inputX) < 0.01f)
            inputX = (Mathf.Abs(currentFrame.motor.RB.velocity.x) > 0.01f) ? Mathf.Sign(currentFrame.motor.RB.velocity.x) : 1f;

        int direction = (int)Mathf.Sign(inputX);

        // Apply dash impulse
        currentFrame.motor.AddImpulse(new Vector2(direction * dashImpulse, 0f));

        // Play dash animation if the player has an animation controller
        var anim = GetComponent<PlayerAnimationController>();
        if (anim) anim.PlayDash();

        // Set blockStuckUntil to current time plus restuckAvoid to avoid immediate restucking on walls
        blockStuckUntil = Time.time + restuckAvoid;

        return false;
    }

    public bool StickyBlocked => Time.time < blockStuckUntil;
}
