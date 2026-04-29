using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    // Parallel teleportation portals, linked by a common id and channel.
    [Header("Linking")]
    [SerializeField] private int portalId = 0;   // coppia logica (es. 0,1,2...)
    [SerializeField] private int channel = 0;  // canale (0=blu,1=verde,...)

    // Teleportation parameters
    [Header("Teleportation")]
    [SerializeField] private Transform exitPoint;
    [SerializeField] private float teleportDelay = 0.15f;

    // Teleport properties
    public int PortalId => portalId;
    public int Channel => channel;

    // On enable and disable, register and unregister the portal in the registry
    private void OnEnable() => PortalRegistry.RegisterPortal(this);
    private void OnDisable() => PortalRegistry.UnregisterPortal(this);

    // Method to set the channel of the portal, which will trigger an update in the registry
    public void SetChannel(int newChannel)
    {
        Debug.Log($"Changing channel of portal {portalId} from {channel} to {newChannel}");
        if (newChannel == channel) return;

        int oldChannel = channel;
        channel = newChannel;

        PortalRegistry.OnPortalChangedChannel(this, oldChannel, channel);

        // Hook to visual update of the portal (e.g. change color) can be added here
        var visual = GetComponent<PortalVisual>();
        if (visual) visual.Apply(channel);
        Debug.Log($"Changing channel of portal {portalId} from {channel} to {newChannel}");
    }

    // OnTriggerEnter is called when another collider enters the portal's trigger collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        var traveler = other.GetComponent<PortalTraveler>();
        if(!traveler || traveler.IsBlocked) return;

        Portal target = PortalRegistry.GetPairedPortal(this);
        if(!target) return;

        traveler.BlockTeleportation(teleportDelay);

        Vector3 exitPosition = target.exitPoint ? target.exitPoint.position : target.transform.position;

        // Move the traveler to the exit position of the target portal, with a delay for teleportation effect
        other.transform.position = exitPosition;

        // Block the target portal for the traveler to prevent immediate re-teleportation
        traveler.BlockTeleportation(teleportDelay);
    }
}
