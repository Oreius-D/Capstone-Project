using UnityEngine;

public class DashPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var flags = other.GetComponent<PlayerFlags>();
        if (!flags) return;

        if (flags.CanDash) return;

        flags.AddDashCharge();
        Destroy(gameObject);
    }
}
