
// A struct to hold all the relevant information for a single frame of player input and state.
public readonly struct PlayerFrame
{
    public readonly PlayerMotor2D motor; // Reference to the PlayerMotor2D component
    public readonly PlayerSensors2D sensors; // Reference to the PlayerSensors2D component
    public readonly PlayerFlags flags; // Reference to the PlayerFlags struct

    public readonly float inputX; // Horizontal input value for the current frame
    public readonly float inputY; // Vertical input value for the current frame
    public readonly bool glide; // Indicates whether the player is gliding

    public readonly bool dashPressed; // Indicates whether the dash button was pressed during the current frame, allowing for dash input to be processed in the same frame it was pressed.
    public readonly float deltaTime; // Delta time for the current frame, allowing for time-based calculations to be performed accurately based on the time elapsed since the last frame.

    // Constructor to initialize all fields of the PlayerFrame struct.
    public PlayerFrame(PlayerMotor2D motor, PlayerSensors2D sensors, PlayerFlags flags,
        float inputX, float inputY, bool glide, bool dashPressed, float deltaTime)
    {
        this.motor = motor;
        this.sensors = sensors;
        this.flags = flags;
        this.inputX = inputX;
        this.inputY = inputY;
        this.glide = glide;
        this.dashPressed = dashPressed;
        this.deltaTime = deltaTime;
    }
}
