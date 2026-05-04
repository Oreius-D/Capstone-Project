using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    // Quit the game
    public void QuitGameEnded()
    {
        Application.Quit();
        Debug.Log("Quit Game"); // For editor testing
    }
}
