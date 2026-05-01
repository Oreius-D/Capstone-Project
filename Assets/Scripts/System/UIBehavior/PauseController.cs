using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    // Parameter for the pause menu UI, allowing the script to reference the pause menu GameObject and control its visibility when pausing and unpausing the game.
    [Header("UI Elements")]
    [SerializeField] private GameObject pauseUI; // Reference to the pause menu GameObject, which can be set in the inspector.

    // Start method to initialize the pause menu state, ensuring that the pause menu is hidden when the game starts.
    private void Start()
    {
        if (pauseUI) pauseUI.SetActive(false); // Hide the pause menu at the start of the game, ensuring that it is not visible until the player chooses to pause the game.
    }

    // Update method to check for pause input, allowing the player to toggle the pause state by pressing the Escape key.
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) TogglePause(); // Check for the Escape key press to toggle the pause state, allowing the player to easily pause and unpause the game.
    }

    // Method to toggle the pause state
    private void TogglePause()
    {
        if (GameManager.Instance == null) return; // Ensure that the GameManager instance exists before attempting to toggle the pause state, preventing potential null reference errors.

        bool newPauseState = !GameManager.Instance.IsPaused; // Determine the new pause state by toggling the current state, allowing the player to switch between paused and unpaused states.
        GameManager.Instance.SetPause(newPauseState); // Call the TogglePause method on the GameManager instance to update the game's pause state, ensuring that the game responds appropriately to the player's input.

        if (pauseUI) pauseUI.SetActive(newPauseState); // Update the visibility of the pause menu based on the new pause state, showing the pause menu when the game is paused and hiding it when unpaused.
    }

    // Resume method to unpause the game, allowing the player to resume gameplay from the pause menu.
    public void Resume()
    {
        if (GameManager.Instance == null) return; // Ensure that the GameManager instance exists before attempting to resume the game, preventing potential null reference errors.

        GameManager.Instance.SetPause(false); // Set the pause state to false to unpause the game, allowing the player to resume gameplay from the pause menu.
        if (pauseUI) pauseUI.SetActive(false); // Hide the pause menu when resuming the game, ensuring that it is not visible while gameplay is active.
    }

    // Restart method to restart the current level, allowing the player to restart the level from the pause menu.
    public void RestartLevel()
    {
        if (GameManager.Instance == null) return; // Ensure that the GameManager instance exists before attempting to restart the level, preventing potential null reference errors.
        GameManager.Instance.RestartLevel(); // Call the RestartLevel method on the GameManager instance to restart the current level, allowing the player to quickly restart from the pause menu.
    }

    // MainMenu method to return to the main menu, allowing the player to return to the main menu from the pause menu.
    public void MainMenu()
    {
        if (GameManager.Instance == null) return; // Ensure that the GameManager instance exists before attempting to return to the main menu, preventing potential null reference errors.
        GameManager.Instance.LoadMainMenu(); // Call the LoadMainMenu method on the GameManager instance to transition back to the main menu, allowing the player to exit the current game and return to the main menu from the pause menu.
    }
}