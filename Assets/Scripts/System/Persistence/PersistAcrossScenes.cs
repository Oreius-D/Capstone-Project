using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistAcrossScenes : MonoBehaviour
{
    // Awake is called when the script instance is being loaded. This method is used to ensure that the GameObject this script is attached to persists across scene loads.
    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // This line tells Unity not to destroy the GameObject when loading a new scene, allowing it to persist across different scenes in the game.
    }
}
