using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DashHud : MonoBehaviour
{
    // Parameters for dash icon and dash text, which are assigned in the Unity editor.
    [SerializeField] private Image dashIcon; // Reference to the GameObject representing the dash icon in the HUD.
    [SerializeField] private TMP_Text dashText; // Reference to the GameObject representing the dash text in the HUD.

    // Parameters for the icon alpha
    [SerializeField] private float enableAlpha = 1f; // The alpha value to set for the dash icon when it is active (between 0 and 1).
    [SerializeField] private float disableAlpha = 0.25f; // The alpha value to set for the dash icon when it is inactive (between 0 and 1).

    // Parameter for the player flags, which is assigned in the Unity editor.
    [SerializeField] private PlayerFlags playerFlags; // Reference to the PlayerFlags component that tracks the player's dash state.

    // Parameter for dash root
    [SerializeField] private GameObject dashRoot; // Reference to the root GameObject of the dash HUD elements, which can be enabled or disabled as a group.

    private void BindToPlayerFlags()
    {
        // stacca da quello vecchio, se cÅfera
        if (playerFlags != null)
            playerFlags.onDashChargeAdded -= HandleDashChanged;

        // trova il nuovo nella scena
        playerFlags = FindObjectOfType<PlayerFlags>();

        if (playerFlags != null)
        {
            playerFlags.onDashChargeAdded += HandleDashChanged;
            HandleDashChanged(playerFlags.CanDash);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        BindToPlayerFlags();
    }

    // OnEnable method that subscribes to the onDashChargeAdded event of the PlayerFlags component. This ensures that the DashHud will update its display whenever the player's dash charge state changes.
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        GameManager.OnDashUnlock += OnDashUnlockChanged;
        BindToPlayerFlags();

        if (GameManager.Instance != null)
            OnDashUnlockChanged(GameManager.Instance.IsDashUnlocked);
        else
            OnDashUnlockChanged(false);
    }

    // OnDisable method that unsubscribes from the onDashChargeAdded event of the PlayerFlags component. This prevents the DashHud from trying to update its display when it is disabled, which could lead to errors if the PlayerFlags component is still active.
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        GameManager.OnDashUnlock -= OnDashUnlockChanged;

        if (playerFlags != null)
            playerFlags.onDashChargeAdded -= HandleDashChanged;
    }

    private void OnDashUnlockChanged(bool isUnlocked)
    {
        if (dashRoot != null)
            dashRoot.SetActive(isUnlocked); // Enable or disable the entire dash HUD based on whether the dash ability is unlocked, ensuring that the HUD only appears when the player has access to the dash ability.
    }

    // HandleDashChanged method that takes a boolean parameter indicating whether the player has a dash charge available (true) or not (false). It updates the alpha of the dash icon and the active state of the dash text based on whether the player can dash.
    private void HandleDashChanged(bool canDash)
    {
        if (dashText) dashText.text = canDash ? "1/1" : "0/1"; // Update the dash text to indicate whether the dash is ready or not based on the canDash parameter.

        if (dashIcon)
        {
            var color = dashIcon.color; // Get the current color of the dash icon.
            color.a = canDash ? enableAlpha : disableAlpha; // Set the alpha of the color based on whether the player can dash (enableAlpha if canDash is true, disableAlpha if canDash is false).
            dashIcon.color = color; // Apply the updated color back to the dash icon to visually indicate whether the dash is available or not.
        }
    }
}
