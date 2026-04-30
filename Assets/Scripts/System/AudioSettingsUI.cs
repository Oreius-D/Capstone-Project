using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    // Slider parameters for controlling master, music, and sound effects volume, allowing the script to reference UI sliders that can be set in the inspector to adjust audio levels in real-time.
    [Header("Audio Sliders")]
    [SerializeField] private Slider masterVolumeSlider; // Reference to the UI slider for controlling master volume, which can be set in the inspector to allow players to adjust the master volume level.
    [SerializeField] private Slider musicVolumeSlider; // Reference to the UI slider for controlling music volume, which can be set in the inspector to allow players to adjust the music volume level.
    [SerializeField] private Slider sfxVolumeSlider; // Reference to the UI slider for controlling sound effects volume, which can be set in the inspector to allow players to adjust the sound effects volume level.

    // Start method to initialize the sliders with the current audio settings from the AudioManager, ensuring that the sliders reflect the current audio levels when the settings UI is opened.
    private void Start()
    {
        if (AudioManager.Instance == null) return; // Ensure that the AudioManager instance exists before attempting to access it, preventing potential null reference errors.
        // Initialize the sliders with the current audio settings from the AudioManager, allowing players to see the current audio levels when they open the settings UI.
        if(masterVolumeSlider) masterVolumeSlider.value = AudioManager.Instance.GetMaster(); // Set the master volume slider's value to the current master volume level from the AudioManager.
        if(musicVolumeSlider) musicVolumeSlider.value = AudioManager.Instance.GetMusic(); // Set the music volume slider's value to the current music volume level from the AudioManager.
        if(sfxVolumeSlider) sfxVolumeSlider.value = AudioManager.Instance.GetSFX(); // Set the sound effects volume slider's value to the current sound effects volume level from the AudioManager.

        // Add listeners to the sliders to update the AudioManager's volume settings in real-time as the sliders are adjusted, allowing players to hear the changes immediately as they adjust the sliders.
        if (masterVolumeSlider) masterVolumeSlider.onValueChanged.AddListener(volume => AudioManager.Instance?.SetMaster(volume)); // Add a listener to the master volume slider to update the master volume in the AudioManager when the slider value changes.
        if (musicVolumeSlider) musicVolumeSlider.onValueChanged.AddListener(volume => AudioManager.Instance?.SetMusic(volume)); // Add a listener to the music volume slider to update the music volume in the AudioManager when the slider value changes.
        if (sfxVolumeSlider) sfxVolumeSlider.onValueChanged.AddListener(volume => AudioManager.Instance?.SetSFX(volume)); // Add a listener to the sound effects volume slider to update the sound effects volume in the AudioManager when the slider value changes.
    }
}
