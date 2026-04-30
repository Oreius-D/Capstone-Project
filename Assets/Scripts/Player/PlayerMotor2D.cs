using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Class responsible for moving the player in a 2D game. Every go that uses this class should have a Rigidbody2D component attached to it.
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMotor2D : MonoBehaviour
{
    // Public reference to the player's Rigidbody2D component.
    public Rigidbody2D RB { get; private set; }

    // Private reference to gravity multiplier, which can be used to adjust the strength of gravity applied to the player.
    private float gravityMultiplier = 1f;

    // Awake is called when the script instance is being loaded. It initializes the RB property by getting the Rigidbody2D component attached to the same GameObject.
    private void Awake() => RB = GetComponent<Rigidbody2D>();

    // External velocity parameter that can be set by other scripts to apply additional forces or movement to the player. This can be used for things like knockback, dashing, or other special movements.
    private Vector2 externalVelocity;

    public void AddExternalVelocity(Vector2 velocity) => externalVelocity += velocity; // Method to add external velocity, allowing other scripts to contribute to the player's movement.

    public void ApplyAndClearExternalVelocity()
    {
        if (externalVelocity != Vector2.zero)
        {
            RB.velocity += externalVelocity; // Apply the accumulated external velocity as an impulse force to the Rigidbody2D.
        }

        externalVelocity = Vector2.zero; // Clear the external velocity after applying it to prevent it from being applied multiple times.
    }

    // FixedUpdate used to calculate and apply gravity to the player. It multiplies the gravity by the gravityMultiplier and applies it as a force to the Rigidbody2D component. 
    private void FixedUpdate()
    {
        if(gravityMultiplier == 1f) return; // If gravity multiplier is 1, we can skip applying additional gravity since the default gravity will already be applied by the physics engine.

        Vector2 gravity = Physics2D.gravity * RB.gravityScale;
        Vector2 additionalGravity = gravity * (gravityMultiplier - 1f); // Calculate the additional gravity to apply based on the multiplier.
        RB.AddForce(additionalGravity, ForceMode2D.Force);
    }

    public void SetGravityMultiplier(float multiplier) => gravityMultiplier = Mathf.Max(0f, multiplier); // Method to set the gravity multiplier, allowing other scripts to adjust the strength of gravity applied to the player.

    public void SetVelocity(Vector2 velocity) => RB.velocity = velocity; // Method to set the player's velocity directly, allowing for movement control from other scripts.
    public void SetVelocityX(float x) => RB.velocity = new Vector2(x, RB.velocity.y); // Method to set the player's horizontal velocity while keeping the vertical velocity unchanged.
    public void SetVelocityY(float y) => RB.velocity = new Vector2(RB.velocity.x, y); // Method to set the player's vertical velocity while keeping the horizontal velocity unchanged.

    public void AddImpulse(Vector2 impulse) => RB.AddForce(impulse, ForceMode2D.Impulse); // Method to add an impulse force to the player, allowing for sudden changes in velocity (e.g., jumping or dashing).
}
