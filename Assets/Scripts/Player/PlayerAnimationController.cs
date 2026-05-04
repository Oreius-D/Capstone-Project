using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerMotor2D motor;
    [SerializeField] private PlayerSensors2D sensors;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float flipDeadzone = 0.05f;

    [Header("Tuning")]
    [SerializeField] private float maxRunSpeedForAnim = 6f; // per normalizzare Speed 0..1
    //[SerializeField] private float yVelThreshold = 0.1f;

    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int GroundedHash = Animator.StringToHash("Grounded");
    private static readonly int YVelHash = Animator.StringToHash("YVel");
    private static readonly int DashTrigHash = Animator.StringToHash("Dash");
    private static readonly int DeathTrigHash = Animator.StringToHash("Death");

    private bool facingRight = true;

    private void Reset()
    {
        animator = GetComponentInChildren<Animator>();
        motor = GetComponent<PlayerMotor2D>();
        sensors = GetComponent<PlayerSensors2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Awake()
    {
        if (!animator) animator = GetComponentInChildren<Animator>();
        if (!motor) motor = GetComponent<PlayerMotor2D>();
        if (!sensors) sensors = GetComponent<PlayerSensors2D>();
        if (!spriteRenderer) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        Vector2 vector = motor.RB.velocity;

        float vectorX = motor.RB.velocity.x;

        if (sensors.IsWallStickingLeft) facingRight = false;
        if (sensors.IsWallStickingRight) facingRight = true;
        if (vectorX > flipDeadzone) facingRight = true;
        else if (vectorX < -flipDeadzone) facingRight = false;

        // flip solo della grafica
        if (spriteRenderer)
            spriteRenderer.flipX = !facingRight;

        // Speed: meglio normalizzato 0..1 per transizioni pulite
        float speed01 = Mathf.Clamp01(Mathf.Abs(vector.x) / Mathf.Max(0.01f, maxRunSpeedForAnim));
        animator.SetFloat(SpeedHash, speed01);

        animator.SetBool(GroundedHash, sensors.IsGrounded);
        animator.SetFloat(YVelHash, vector.y);
    }


    public void PlayDash()
    {
        animator.SetTrigger(DashTrigHash);
    }

    // Chiamalo quando muori (prima del restart)
    public void PlayDeath()
    {
        animator.SetTrigger(DeathTrigHash);
        motor.RB.velocity = Vector2.zero;
    }
}
