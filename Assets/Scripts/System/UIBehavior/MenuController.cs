using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Parameters for scene names
    [Header("Scene Names")]
    [SerializeField] private string firstLevelSceneName = "Level01";

    // Parameters for panel references (optionals)
    [Header("Panel References")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject levelSelectPanel;

    // Start is called before the first frame update
    void Start()
    {
        // Ensure panels are hidden at the start
        if (settingsPanel) settingsPanel.SetActive(false);
        if (levelSelectPanel) levelSelectPanel.SetActive(false);

        Time.timeScale = 1f; // Ensure time is running
    }

    // Play the first level
    public void PlayGame()
    {
        SceneManager.LoadScene(firstLevelSceneName);
    }

    // Open the settings panel
    public void OpenSettings(bool isOpen)
    {
        if (settingsPanel) settingsPanel.SetActive(isOpen);
    }

    // Open the level select panel
    public void OpenLevelSelect(bool isOpen)
    {
        if (levelSelectPanel) levelSelectPanel.SetActive(isOpen);
    }

    // Close settings and level select panels
    public void ClosePanels()
    {
        if (settingsPanel) settingsPanel.SetActive(false);
        if (levelSelectPanel) levelSelectPanel.SetActive(false);
    }

    // Quit the game
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game"); // For editor testing
    }
}
