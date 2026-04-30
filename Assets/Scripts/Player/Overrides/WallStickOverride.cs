using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallStickOverride : MonoBehaviour, IMovementOverride
{
    // Variables for climbing speed and to detect detachment from the wall. Can be adjusted in the inspector.
    [SerializeField] private float climbSpeed = 3f;
    [SerializeField] private float detachTreshold = 0.5f;
    [SerializeField] private DashOverride dash;

    // Variables to check if the player is stuck and int value to determine stuck direction.
    private bool stuck;
    private int stuckDirection; // -1 for left, 1 for right

    // Method to try to override the movement. It checks if the player is stuck and applies climbing movement if so.
    public bool TryOverrideMovement(in PlayerFrame currentFrame)
    {
        bool stickyL = currentFrame.sensors.IsWallStickingLeft;
        bool stickyR = currentFrame.sensors.IsWallStickingRight;

        if (!stickyL && !stickyR)
        {
            stuck = false;
            return false;
        }

        //Debug.Log($"WALLSTICK called. stickyL={currentFrame.sensors.IsWallStickingLeft} stickyR={currentFrame.sensors.IsWallStickingRight}");

        // If the player is in Lift state, don't allow wall stick
        if (currentFrame.flags.InLiftZone) return false;

        // If dash override is active and sticky blocked, don't allow wall stick
        if (dash != null && dash.StickyBlocked)
        {
            stuck = false;
            return false;
        }

        // Don't attach if grounded
        if (currentFrame.sensors.IsGrounded)
        {
            stuck = false;
            return false; // No override applied
        }

        // If not stuck, become stuck if touching a wall and pressing towards it
        if (!stuck)
        {
            bool touchingLeftWall = currentFrame.inputX < -detachTreshold && currentFrame.sensors.IsWallStickingLeft;
            bool touchingRightWall = currentFrame.inputX > detachTreshold && currentFrame.sensors.IsWallStickingRight;

            if (touchingLeftWall) { stuck = true; stuckDirection = -1; }
            else if (touchingRightWall) { stuck = true; stuckDirection = 1; }
            else return false; // No override applied
        }

        // If stuck, check sensors to see if still touching the wall. If not, become unstuck.
        if (stuckDirection == -1 && !currentFrame.sensors.IsWallStickingLeft) { stuck = false; return false; }
        if (stuckDirection == 1 && !currentFrame.sensors.IsWallStickingRight) { stuck = false; return false; }

        // If input is away from the wall, detach
        if (stuckDirection == -1 && currentFrame.inputX > detachTreshold) { stuck = false; return false; }
        if (stuckDirection == 1 && currentFrame.inputX < -detachTreshold) { stuck = false; return false; }

        // When stuck, remove gravity and force only vertical movement
        currentFrame.motor.SetGravityMultiplier(0f);
        currentFrame.motor.SetVelocityX(0f);

        // Move up or down based on vertical input
        float verticalVelocity = Mathf.Abs(currentFrame.inputY) > 0.01f ? currentFrame.inputY * climbSpeed : 0f;
        currentFrame.motor.SetVelocityY(verticalVelocity);

        return true; // Override applied
    }

}
