using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is intended to be used as a component on the player GameObject to manage various sensors related to the player's interactions with the environment, such as
// ground detection, wall detection, and other relevant sensor data. It can be expanded in the future to include specific sensor implementations and logic for handling player interactions based on those sensors.
// Every go that uses this will require a collider2D component to function properly, as it will likely rely on collision detection to determine the player's interactions with the environment.

[RequireComponent(typeof(Collider2D))]
public class PlayerSensors2D : MonoBehaviour
{
    // Collider2D component reference, required for the sensor functionality. Can be set in the inspector or obtained in the Awake method.
    [SerializeField] private Collider2D playerCollider;

    // Masks for layer detection, allowing the player to specify which layers are relevant for different types of sensors (e.g., ground, walls, etc.). These can be set in the inspector.
    [Header("Layer Masks")]
    [SerializeField] private LayerMask groundLayerMask; // Layer mask for detecting ground surfaces.
    [SerializeField] private LayerMask wallStickLayerMask; // Layer mask for detecting walls or other vertical surfaces.

    // Float values for ray to be casted from the player to detect the ground. These can be set in the inspector to adjust the distance of the raycast for ground detection.
    [Header("Ground Detection")]
    [SerializeField] private float groundRayLength = 0.1f; // Length of the raycast used for ground detection.
    [SerializeField] private float groundInset = 0.1f; // Inset from the player's collider bounds to start the ground raycast, allowing for more accurate detection of the ground surface.

    // Public bool properties to check if the player is grounded or wall sticking, either left or right.
    public bool IsGrounded { get; private set; } // Indicates whether the player is currently grounded.
    public bool IsWallStickingLeft { get; private set; } // Indicates whether the player is currently sticking to a wall on the left side.
    public bool IsWallStickingRight { get; private set; } // Indicates whether the player is currently sticking to a wall on the right side.

    // Reset method to get the Collider2D component.
    private void Reset() => playerCollider = GetComponent<Collider2D>();

    // Awake method to ensure that the Collider2D component is assigned, either through the inspector or by getting it from the GameObject.
    private void Awake()
    {
        if (playerCollider == null)
            playerCollider = GetComponent<Collider2D>();
    }

    // FixedUpdate method to perform sensor checks at a consistent rate, ensuring that the player's interactions with the environment are updated regularly.
    // This is where the logic for checking if the player is grounded or wall sticking would be implemented.
    private void FixedUpdate()
    {
        Bounds bounds = playerCollider.bounds; // Get the bounds of the player's collider to use for raycasting and sensor checks.

        // Use 2 raycasts for ground detection, one on the left side and one on the right side of the player's collider, to ensure accurate detection of the ground surface.
        Vector2 leftRayOrigin = new Vector2(bounds.min.x + groundInset, bounds.min.y); // Origin of the left raycast, inset from the left edge of the collider.
        Vector2 rightRayOrigin = new Vector2(bounds.max.x - groundInset, bounds.min.y); // Origin of the right raycast, inset from the right edge of the collider.

        // Is Grounded check using raycasts to detect if either the left or right raycast hits a collider on the ground layer mask, indicating that the player is grounded.
        IsGrounded = Physics2D.Raycast(leftRayOrigin, Vector2.down, groundRayLength, groundLayerMask) ||
                     Physics2D.Raycast(rightRayOrigin, Vector2.down, groundRayLength, groundLayerMask);

        // Use 2 raycasts for each side for wall sticking detection, one at the top and one at the bottom of the player's collider, to ensure accurate detection of walls or vertical surfaces.

        // Left
        Vector2 leftWallTopRayOrigin = new Vector2(bounds.min.x, bounds.max.y - groundInset); // Origin of the left wall top raycast, at the top edge of the collider.
        Vector2 leftWallBottomRayOrigin = new Vector2(bounds.min.x, bounds.min.y + groundInset); // Origin of the left wall bottom raycast, at the bottom edge of the collider.

        IsWallStickingLeft = Physics2D.Raycast(leftWallTopRayOrigin, Vector2.left, groundRayLength, wallStickLayerMask) ||
                            Physics2D.Raycast(leftWallBottomRayOrigin, Vector2.left, groundRayLength, wallStickLayerMask);

        // Right
        Vector2 rightWallTopRayOrigin = new Vector2(bounds.max.x, bounds.max.y - groundInset); // Origin of the right wall top raycast, at the top edge of the collider.
        Vector2 rightWallBottomRayOrigin = new Vector2(bounds.max.x, bounds.min.y + groundInset); // Origin of the right wall bottom raycast, at the bottom edge of the collider.

        IsWallStickingRight = Physics2D.Raycast(rightWallTopRayOrigin, Vector2.right, groundRayLength, wallStickLayerMask) ||
                             Physics2D.Raycast(rightWallBottomRayOrigin, Vector2.right, groundRayLength, wallStickLayerMask);
    }
}
