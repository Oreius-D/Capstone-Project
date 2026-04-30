using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    // Property to hold the singleton instance of the AudioManager, allowing other scripts to easily access the audio manager throughout the game.
    public static AudioManager Instance { get; private set; }

    // Parameters for mixing audio, allowing the script to reference an AudioMixer and control the volume levels of different audio groups (e.g., music, sound effects) through the inspector.
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer mixer; // Reference to the AudioMixer, which can be set in the inspector to control audio levels.
    [SerializeField] private string MasterVolumeParam = "MasterVolume"; // Name of the parameter in the AudioMixer for controlling master volume, which can be set in the inspector.
    [SerializeField] private string MusicVolumeParam = "MusicVolume"; // Name of the parameter in the AudioMixer for controlling music volume, which can be set in the inspector.
    [SerializeField] private string SFXVolumeParam = "SFXVolume"; // Name of the parameter in the AudioMixer for controlling sound effects volume, which can be set in the inspector.

    // Parameters for audio sources, allowing the script to reference AudioSource components for music and sound effects, enabling the script to play and manage audio clips through these sources.
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource; // Reference to the AudioSource for music, which can be set in the inspector to play background music.
    [SerializeField] private AudioSource sfxSource; // Reference to the AudioSource for sound effects, which can be set in the inspector to play sound effects.

    // Parameters for default audio values, allowing the script to define default volume levels for master, music, and sound effects, which can be set in the inspector to ensure consistent audio levels when the game starts.
    [Header("Default Audio Values")]
    [Range(0f, 1f)] [SerializeField] private float defaultMasterVolume = 1f; // Default master volume level, which can be set in the inspector to ensure consistent audio levels when the game starts.
    [Range(0f, 1f)] [SerializeField] private float defaultMusicVolume = 1f; // Default music volume level, which can be set in the inspector to ensure consistent audio levels when the game starts.
    [Range(0f, 1f)] [SerializeField] private float defaultSFXVolume = 1f; // Default sound effects volume level, which can be set in the inspector to ensure consistent audio levels when the game starts.

    // Constants for PlayerPrefs keys, allowing the script to define constant string values for saving and loading audio settings in PlayerPrefs, ensuring that the same keys are used consistently throughout the code when accessing audio settings.
    private const string PREF_MASTER_VOLUME = "Master_Volume"; // Key for saving and loading master volume in PlayerPrefs.
    private const string PREF_MUSIC_VOLUME = "Music_Volume"; // Key for saving and loading music volume in PlayerPrefs.
    private const string PREF_SFX_VOLUME = "SFX_Volume"; // Key for saving and loading sound effects volume in PlayerPrefs.

    // Awake method to implement the singleton pattern, ensuring that only one instance of the AudioManager exists and persists across scenes, allowing for consistent audio management throughout the game.
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy this object if another instance already exists, ensuring that only one instance of the AudioManager exists at any time.
            return;
        }

        Instance = this; // Set the singleton instance to this object, allowing other scripts to access the AudioManager through the Instance property.
        DontDestroyOnLoad(gameObject); // Prevent this object from being destroyed when loading new scenes, ensuring that the AudioManager persists across scene transitions.

        ApplySavedVolumes(); // Apply saved volume settings from PlayerPrefs when the game starts, ensuring that the player's audio preferences are maintained across sessions.
    }

    // ==== Music Methods ====

    // Method to play music, allowing the script to play a specified audio clip through the music audio source, enabling background music in the game.
    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if(!clip || !musicSource) return; // Ensure that the audio clip and music source are valid before attempting to play music, preventing potential null reference errors.

        if (musicSource.clip == clip && musicSource.isPlaying) return; // If the music source is already playing the specified clip, do nothing to avoid restarting the music unnecessarily.

        musicSource.clip = clip; // Set the music source's clip to the specified audio clip, preparing it for playback.
        musicSource.loop = loop; // Set the looping behavior of the music source based on the provided parameter, allowing for continuous background music if desired.
        musicSource.Play(); // Play the music through the music audio source, enabling background music in the game.
    }

    // Method to stop music, allowing the script to stop the currently playing music through the music audio source, providing control over background music playback.
    public void StopMusic()
    {
        if (!musicSource) return; // Ensure that the music source is valid before attempting to stop music, preventing potential null reference errors.
        musicSource.Stop(); // Stop the currently playing music through the music audio source, providing control over background music playback.
        musicSource.clip = null;
    }

    // ==== SFX Methods ====

    // Method to play a sound effect, allowing the script to play a specified audio clip through the sound effects audio source, enabling sound effects in the game.
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (!clip || !sfxSource) return; // Ensure that the audio clip and sound effects source are valid before attempting to play a sound effect, preventing potential null reference errors.
        sfxSource.PlayOneShot(clip, Mathf.Clamp01(volume)); // Play the specified audio clip as a one-shot sound effect through the sound effects audio source, enabling sound effects in the game without interrupting any currently playing music.
    }

    // ==== Volume (0..1) Methods ====

    public void SetMaster(float volume) => SetVolume(PREF_MASTER_VOLUME, MasterVolumeParam, volume);
    public void SetMusic(float volume) => SetVolume(PREF_MUSIC_VOLUME, MusicVolumeParam, volume);
    public void SetSFX(float volume) => SetVolume(PREF_SFX_VOLUME, SFXVolumeParam, volume);

    public float GetMaster() => PlayerPrefs.GetFloat(PREF_MASTER_VOLUME, defaultMasterVolume);
    public float GetMusic() => PlayerPrefs.GetFloat(PREF_MUSIC_VOLUME, defaultMusicVolume);
    public float GetSFX() => PlayerPrefs.GetFloat(PREF_SFX_VOLUME, defaultSFXVolume);

    // Method to apply saved volume settings from PlayerPrefs, allowing the script to load and apply the player's previously saved audio preferences when the game starts, ensuring a consistent audio experience across sessions.
    private void ApplySavedVolumes()
    {
        SetMixerVolume(MasterVolumeParam, GetMaster()); // Apply the saved master volume setting to the AudioMixer, ensuring that the player's master volume preference is applied when the game starts.
        SetMixerVolume(MusicVolumeParam, GetMusic()); // Apply the saved music volume setting to the AudioMixer, ensuring that the player's music volume preference is applied when the game starts.
        SetMixerVolume(SFXVolumeParam, GetSFX()); // Apply the saved sound effects volume setting to the AudioMixer, ensuring that the player's sound effects volume preference is applied when the game starts.
    }

    // Method to set volume
    private void SetVolume(string prefKey, string mixerParam, float volume)
    {
        volume = Mathf.Clamp01(volume); // Clamp the volume value between 0 and 1 to ensure valid volume levels.
        PlayerPrefs.SetFloat(prefKey, volume); // Save the volume setting in PlayerPrefs using the specified key, allowing the player's audio preferences to be saved across sessions.
        PlayerPrefs.Save(); // Save the PlayerPrefs to ensure that the volume settings are stored on disk, allowing them to be loaded the next time the game starts.
        SetMixerVolume(mixerParam, volume); // Apply the volume setting to the AudioMixer using the specified parameter name, ensuring that the audio levels are updated in real-time based on the player's preferences.
    }

    // Convert slider value (0..1) to mixer volume (-80..0 dB) and set it on the AudioMixer
    private void SetMixerVolume(string mixerParam, float volume)
    {
        if (!mixer) return; // Ensure that the AudioMixer reference is valid before attempting to set the mixer volume, preventing potential null reference errors.

        float dB = (volume <= 0.0001f) ? -80f : Mathf.Log10(volume) * 20f; // Convert the slider value (0 to 1) to a decibel value for the AudioMixer, using a logarithmic scale to provide a more natural volume control experience. If the volume is very close to zero, set it to -80 dB to effectively mute the audio without causing issues with logarithmic calculations.
        mixer.SetFloat(mixerParam, dB); // Set the calculated decibel value on the AudioMixer using the specified parameter name, updating the audio levels in real-time based on the player's preferences.
    }


}
