using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


// Script to control the player. Processes anything related to player movement.
public class PlayerController : MonoBehaviour
{
    // References to motor, sensors, and flags components.
    [Header("References")]
    [SerializeField] private PlayerMotor2D motor;
    [SerializeField] private PlayerSensors2D sensors;
    [SerializeField] private PlayerFlags flags;

    // References to behaviors. These are the scripts that will actually implement the player's movement logic, and they will be called in a specific order in the Update loop.
    // Order of behaviors is determined by the order of the scripts in the inspector, which can be changed by dragging and dropping the scripts in the inspector.
    // Order = priority, so the first behavior in the list will be executed first, and so on.
    [Header("Behaviors overrides (ordine = priorità)")]
    [SerializeField] private MonoBehaviour[] behaviors;
    private IMovementOverride[] movementOverrides;

    // Base movement (L/R) is always applied every FixedUpdate.
    // Then override behaviors are evaluated in order (priority) and the first one that triggers can override motor settings.
    // Remember: no jump
    [Header("Default movement behavior (used if no overrides provided)")]
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 10f;
    [SerializeField] private float airMovement = 5f;

    // Private variables to hold input values.
    private float inputX;
    private float inputY;
    private bool glide;
    private bool dashPressed;

    // Private variable to check if the player was grounded, which will be used to determine whether to apply ground or air movement.
    private bool wasGrounded;

    // Reset is called when the script is first added to a GameObject or when the user clicks the Reset button in the inspector. It is used to initialize references and set default values for variables.
    private void Reset()
    {
        // Try to automatically assign references to motor, sensors, and flags components if they are not already assigned.
        if (motor == null) motor = GetComponent<PlayerMotor2D>();
        if (sensors == null) sensors = GetComponent<PlayerSensors2D>();
        if (flags == null) flags = GetComponent<PlayerFlags>();
    }

    // Awake is called when the script instance is being loaded. It is used to initialize variables and set up references before the game starts.
    private void Awake()
    {
        // Initialize the movement overrides array based on the number of behaviors provided.
        var movementOverrides = new List<IMovementOverride>(behaviors.Length);

        for (int i = 0; i < behaviors.Length; i++)
        {
            // Try to get the IMovementOverride component from each behavior script. If a script does not implement the interface, log a warning and ignore it.
            if (behaviors[i] is IMovementOverride movementOverride)
                movementOverrides.Add(movementOverride);
            else
                Debug.LogWarning($"Behavior script {behaviors[i].GetType().Name} does not implement IMovementOverride and will be ignored.");
        }

        this.movementOverrides = movementOverrides.ToArray();
    }

    // Update is called once per frame. It is used to process player input and update the player's movement based on the current state and input.
    private void Update()
    {
        // Get input values from the Input system.
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        glide = Input.GetKey(KeyCode.LeftShift);

        if(Input.GetKeyDown(KeyCode.Space))
            dashPressed = true;
    }

    // FixedUpdate is called at a fixed interval and is used for physics calculations. It is used to apply movement to the player based on the current state and input.
    private void FixedUpdate()
    {
        // Tick based movement
        TickBaseMovement();

        // If we touch the ground, we are grounded. Reset glide flag.
        if (sensors.IsGrounded && !wasGrounded) flags.OnGrounded();
        wasGrounded = sensors.IsGrounded;

        // Apply default gravity
        motor.SetGravityMultiplier(1f);

        // Create the new PlayerFrame struct with all the relevant information for the current frame.
        PlayerFrame frame = new PlayerFrame(motor, sensors, flags, inputX, inputY, glide, dashPressed, Time.fixedDeltaTime);

        // Call the movement overrides in the order they are provided. If an override returns true, it means that it has handled the movement for this frame and no further overrides should be called. If it returns false, it means that it has not handled the movement and the next override should be called.
        foreach (var movementOverride in movementOverrides)
        {
            if (movementOverride.TryOverrideMovement(frame))
                break;
        }

        // Consume the dash input after processing it, so it is only true for one frame.
        dashPressed = false;

        motor.ApplyAndClearExternalVelocity();
    }

    // Method to process tick-based movement. It will call the movement overrides in the order they are provided, and if no override is provided, it will apply the default movement behavior.
    private void TickBaseMovement()
    {
        // These variables are used to calculate the target velocity and the control factor based on whether the player is grounded or in the air. The control factor is used to reduce the effectiveness of acceleration
        // and deceleration when the player is in the air, making it feel more floaty and less responsive.
        float target = inputX * maxSpeed;
        float control = sensors.IsGrounded ? 1f : airMovement;

        // Get the current velocity of the player and calculate the difference between the target velocity and the current velocity. Then, apply acceleration or deceleration based on the control factor and
        // the time delta to smoothly transition towards the target velocity.
        float xVelocity = motor.RB.velocity.x;
        float velocityDifference = target - xVelocity;

        // If the target velocity is close to zero, we want to apply deceleration to quickly stop the player. Otherwise, we want to apply acceleration to smoothly reach the target velocity.
        float rate = Mathf.Abs(target) > 0.01f ? acceleration : deceleration;

        // Calculate the new velocity by applying the acceleration or deceleration based on the control factor and the time delta, and set it to the motor.
        float newXVelocity = xVelocity + velocityDifference * rate * control * Time.fixedDeltaTime;

        // Clamp the new velocity to the maximum speed to prevent the player from exceeding it.
        motor.SetVelocityX(Mathf.Clamp(newXVelocity, -maxSpeed, maxSpeed));
    }

    // Method to add an impulse to the player, which can be called from other scripts (e.g., a launch pad) to apply a sudden force to the player.
    public void AddImpulse(Vector2 impulse) => motor.AddImpulse(impulse);

    public void SetVerticalVelocity(float yVelocity) => motor.SetVelocityY(yVelocity);

}