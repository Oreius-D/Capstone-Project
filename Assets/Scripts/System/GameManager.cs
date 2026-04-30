using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Istance property to implement the singleton pattern, allowing other scripts to easily access the GameManager instance without needing to reference it directly.
    public static GameManager Instance { get; private set; }

    // Property to check for pause state, allowing other scripts to check if the game is currently paused and adjust their behavior accordingly.
    public bool IsPaused { get; private set; }

    // Parameters for scene names and transition settings, allowing the GameManager to manage scene transitions and other related functionality. These can be set in the inspector.
    [Header("Scene Management")]
    [SerializeField] private string mainMenuSceneName = "MainMenu"; // Name of the main menu scene, used for transitioning back to the main menu.

    [Header("Transition Settings")]
    [SerializeField] private float transitionDuration = 1f; // Duration of scene transitions, allowing for smooth transitions between scenes.

    private bool locked;

    // Awake method to implement the singleton pattern, ensuring that only one instance of the GameManager exists and is accessible throughout the game.
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return; // Destroy any duplicate instances of the GameManager to maintain the singleton pattern.
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Prevent the GameManager from being destroyed when loading new scenes, allowing it to persist throughout the game.
    }

    // OnDestroy method to clean up the singleton instance when the GameManager is destroyed, ensuring that the Instance property is set to null to prevent references to a destroyed object.
    private void OnDestroy()
    {
        if (Instance == this) Time.timeScale = 1f; // Set the Instance property to null if the current instance is being destroyed, allowing for proper cleanup of the singleton instance.
    }

    // Lock method to set the locked state of the GameManager, which can be used to control whether certain actions or transitions are allowed based on the game's state.
    public void Lock()
    {
        locked = true;
        Invoke(nameof(Unlock), transitionDuration); // Automatically unlock after the specified transition duration, allowing for temporary locking during scene transitions or other events.
    }

    // Unlock method to set the locked state of the GameManager to false, allowing actions or transitions to proceed after being locked.
    public void Unlock() => locked = false;

    // Method to restart the current level
    public void RestartLevel()
    {
        SetPause(false); // Ensure the game is unpaused when restarting the level, allowing for a fresh start without any pause state affecting gameplay.
        if (locked) return;
        Lock();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Method to load next level
    public void LoadNextLevel()
    {
        SetPause(false);

        if (locked) return; // Prevent loading the next level if the GameManager is currently locked, ensuring that transitions or actions are not interrupted.
        Lock(); // Lock the GameManager to prevent other actions or transitions while loading the next level, ensuring a smooth transition between scenes.

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex; // Get the index of the current active scene to determine which level is currently being played.
        int nextSceneIndex = currentSceneIndex + 1; // Calculate the index of the next scene by incrementing the current scene index.

        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(mainMenuSceneName); // Load the main menu scene if there are no more levels to load, allowing the player to return to the main menu.
        }
        else
        {
            SceneManager.LoadScene(nextSceneIndex); // Load the next scene if it exists in the build settings, allowing for progression through levels.
        }
    }

    // Method to load main menu
    public void LoadMainMenu()
    {
        SetPause(false);

        if (locked) return; // Prevent loading the main menu if the GameManager is currently locked, ensuring that transitions or actions are not interrupted.
        Lock(); // Lock the GameManager to prevent other actions or transitions while loading the main menu, ensuring a smooth transition between scenes.
        SceneManager.LoadScene(mainMenuSceneName); // Load the main menu scene, allowing the player to return to the main menu.
    }

    // Method to toggle pause state
    public void SetPause(bool paused)
    {
        IsPaused = paused;
        Time.timeScale = paused ? 0f : 1f; // Set the time scale to 0 when paused to freeze the game, and back to 1 when unpaused to resume normal gameplay.
    }
}
