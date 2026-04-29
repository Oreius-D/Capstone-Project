using UnityEngine;

public class LiftPadHover : MonoBehaviour
{
    [SerializeField] private Transform topPoint; // punto X (quota target)

    private void OnTriggerEnter2D(Collider2D other)
    {
        var flags = other.GetComponent<PlayerFlags>();
        if (!flags) return;

        flags.EnterLiftZone(topPoint.position.y);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var flags = other.GetComponent<PlayerFlags>();
        if (!flags) return;

        flags.ExitLiftZone();
    }
}
