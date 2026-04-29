using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTraveler : MonoBehaviour
{
    // Parameter to block the player from teleporting again immediately after teleporting
    [SerializeField] private float teleportCooldown = 1f;

    // property to check if the player can teleport
    public bool IsBlocked => Time.time < teleportCooldown;

    // Method to block the player from teleporting for a certain duration
    public void BlockTeleportation(float duration)
    {
        teleportCooldown = Time.time + duration;
    }
}
