using UnityEngine;

public class PlatformCarry2D : MonoBehaviour
{
    [SerializeField] private MovingPlatform2D platform;

    private PlayerMotor2D motor;

    private void Reset()
    {
        var c = GetComponent<Collider2D>();
        if (c) c.isTrigger = true;

        if (!platform) platform = GetComponentInParent<MovingPlatform2D>();
    }

    private void Awake()
    {
        if (!platform) platform = GetComponentInParent<MovingPlatform2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var pc = other.GetComponentInParent<PlayerController>();
        if (!pc) return;

        motor = pc.GetComponent<PlayerMotor2D>();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var pc = other.GetComponentInParent<PlayerController>();
        if (!pc) return;

        var m = pc.GetComponent<PlayerMotor2D>();
        if (m == motor) motor = null;
    }

    private void FixedUpdate()
    {
        if (!platform || !motor) return;

        Vector2 d = platform.DeltaMovement;
        if (d == Vector2.zero) return;

        // Convertiamo lo spostamento della piattaforma in velocità per questo tick
        Vector2 carryVel = d / Time.fixedDeltaTime;

        // Somma alla velocity del player senza annullare il controllo
        motor.AddExternalVelocity(carryVel);
    }
}