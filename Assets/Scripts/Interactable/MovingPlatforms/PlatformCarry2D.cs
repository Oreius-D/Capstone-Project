using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCarry2D : MonoBehaviour
{
    // Parameter for platform
    [Header("Platform")]
    [SerializeField] private MovingPlatform2D platform;

    // Readonly list of passengers currently on the platform
    private readonly List<PlayerMotor2D> passengers = new List<PlayerMotor2D>(4);

    // Reset method
    private void Reset()
    {
        if(!platform)platform = GetComponentInParent<MovingPlatform2D>();
    }

    // FixedUpdate is called at a fixed interval and is used for physics updates
    private void FixedUpdate()
    {
        if (!platform) return;

        Vector2 deltaMovement = platform.DeltaMovement;
        if (deltaMovement == Vector2.zero) return;

        // Calculate platform velocity for moving passengers
        Vector2 platformVelocity = deltaMovement / Time.fixedDeltaTime;

        // Move each passenger by the platform's delta movement
        for (int i = passengers.Count - 1; i >= 0; i--)
        {
            var motor = passengers[i];
            if (!motor)
            {
                passengers.RemoveAt(i);
                continue;
            }

            motor.AddExternalVelocity(platformVelocity);
        }
    }

    // OnTriggerEnter2D is called when another collider enters the trigger collider attached to this object
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Make sure it's only the player that can be carried by the platform (May be removed later)
        if(!collision.GetComponent<PlayerController>()) return;

        var motor = collision.GetComponent<PlayerMotor2D>();
        if (!passengers.Contains(motor)) passengers.Add(motor);
    }

    // OnTriggerExit2D is called when another collider exits the trigger collider attached to this object
    private void OnTriggerExit2D(Collider2D collision)
    {
        var motor = collision.GetComponent<PlayerMotor2D>();
        if(!motor) return;

        passengers.Remove(motor);
    }
}
