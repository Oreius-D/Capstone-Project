using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatform2D : MonoBehaviour, IReciever
{
    // Parameters for path movement
    [Header("Path Movement")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private bool loop = true;

    // Parameters for movement
    [Header("Movement")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float waitTime = 1f;

    // Starting state
    [Header("Starting State")]
    [SerializeField] private bool startActive = true;

    // Property for delta movement, used for moving passengers
    public Vector2 DeltaMovement { get; private set; }

    // private variables for movement logic
    private Rigidbody2D rb;
    private int currentWaypointIndex = 0;
    private int direction = 1; // 1 for forward, -1 for backward
    private float waitTimer = 0f;
    private bool active;
    private Vector2 lastPosition;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        active = startActive;
        lastPosition = rb.position;
    }

    // FixedUpdate is called at a fixed interval and is used for physics updates
    private void FixedUpdate()
    {
        Vector2 currentPosition = rb.position;

        if(!active || waypoints == null || waypoints.Length < 2)
        {
            DeltaMovement = Vector2.zero;
            lastPosition = currentPosition;
            return;
        }

        // Check waiting at a waypoint
        if(waitTimer > 0f)
        {
            waitTimer -= Time.fixedDeltaTime;
            DeltaMovement = Vector2.zero;
            lastPosition = currentPosition;
            return;
        }

        // Set target waypoint and move towards it
        Vector2 targetWaypoint = waypoints[currentWaypointIndex].position;
        Vector2 newPosition = Vector2.MoveTowards(currentPosition, targetWaypoint, speed * Time.fixedDeltaTime);

        // Update the platform's position
        rb.MovePosition(newPosition);

        // Calculate delta movement for passengers
        DeltaMovement = newPosition - lastPosition;
        lastPosition = newPosition;

        // Check if the platform has reached the target waypoint
        if(Vector2.Distance(newPosition, targetWaypoint) < 0.001f)
        {
            waitTimer = waitTime;
            AdvanceIndex();
        }
    }

    // Method to advance the waypoint index based on the current direction and looping settings
    private void AdvanceIndex()
    {
        if(!loop)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            return;
        }

        // Looping logic for back and forth movement
        if(currentWaypointIndex == waypoints.Length - 1) direction = -1; // Reverse direction at the end
        else if(currentWaypointIndex == 0) direction = 1; // Forward direction at the start

        currentWaypointIndex += direction;
    }

    // Recieve method to toggle the active state of the platform
    public void SetState(bool on)
    {
        active = on;
        DeltaMovement = Vector2.zero; // Reset delta movement when toggling state
    }
}
