using UnityEngine;

public class LaunchPad : MonoBehaviour
{
    [SerializeField] private Vector2 impulse = new Vector2(0f, 14f);

    private void OnTriggerEnter2D(Collider2D other)
    {
        var controller = other.GetComponent<PlayerController>();
        if (!controller) return;
        controller.AddImpulse(impulse);
    }
}