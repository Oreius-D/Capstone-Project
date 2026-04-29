using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalChannelReceiver : MonoBehaviour, IReciever
{
    // Parameters for receiving the channel information and portal
    [SerializeField] private Portal portal;
    [SerializeField] private int channelWhenOff = 0;
    [SerializeField] private int channelWhenOn = 1;

    // Reset method to initialize references in the editor
    private void Reset()
    {
        portal = GetComponent<Portal>();
    }

    // Set state method to update the portal's channel based on the received state
    public void SetState(bool state)
    {
        if(!portal) return;
        portal.SetChannel(state ? channelWhenOn : channelWhenOff);
    }
}
