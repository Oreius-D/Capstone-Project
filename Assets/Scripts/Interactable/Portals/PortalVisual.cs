using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalVisual : MonoBehaviour
{
    // Parameters for visual representation of the portal, such as color or animation
    [Header("Visual Parameters")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color[] channelColors; // Array of colors corresponding to different channels

    private void Reset()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Method to apply the visual representation based on the channel of the portal
    public void Apply(int channel)
    {
        if(!spriteRenderer) return;

        if(channelColors == null || channelColors.Length == 0) return;

        int index = Mathf.Clamp(channel, 0, channelColors.Length - 1);
        spriteRenderer.color = channelColors[index];
    }

    // Start is called before the first frame update
    private void Start()
    {
        // Apply the initial visual representation based on the portal's channel
        var portal = GetComponent<Portal>();
        if(portal) Apply(portal.Channel);
    }
}
