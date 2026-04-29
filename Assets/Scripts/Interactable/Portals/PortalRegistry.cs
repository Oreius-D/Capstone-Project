using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PortalRegistry
{
    // Create a dictionary to store the portals with their unique identifiers
    private static readonly Dictionary<(int portalId, int channel), List<Portal>> map = new();

    // Method to register a portal
    public static void RegisterPortal(Portal portal)
    {
        var key = (portal.PortalId, portal.Channel);
        if (!map.TryGetValue(key, out var list))
        {
            list = new List<Portal>(2);
            map[key] = list;
        }
        if(!list.Contains(portal)) list.Add(portal);
    }

    // Method to unregister a portal
    public static void UnregisterPortal(Portal portal)
    {
        var key = (portal.PortalId, portal.Channel);
        if (!map.TryGetValue(key, out var list)) return;
        list.Remove(portal);
        if (list.Count == 0) map.Remove(key);
    }

    // Method to get the paired portal
    public static Portal GetPairedPortal(Portal from)
    {
        var key = (from.PortalId, from.Channel);
        if (!map.TryGetValue(key, out var list)) return null;

        for(int i = 0; i < list.Count; i++)
        {
            if (list[i] != null && list[i] != from) return list[i];
        }

        return null;
    }

    // Method to compute the channel change for a given portal
    public static void OnPortalChangedChannel(Portal portal, int oldChannel, int newChannel)
    {
        var oldKey = (portal.PortalId, oldChannel);

        // Remove the portal from the old channel list
        if (map.TryGetValue(oldKey, out var oldList))
        {
            oldList.Remove(portal);
            if (oldList.Count == 0) map.Remove(oldKey);
        }
        // Add the portal to the new channel list
        RegisterPortal(portal);
    }
}
