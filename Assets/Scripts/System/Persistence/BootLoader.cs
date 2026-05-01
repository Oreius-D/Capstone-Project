using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootLoader : MonoBehaviour
{
    [SerializeField] private int firstLevelBuildIndex = 1; // 0=Boot, 1=Level01

    private void Start()
    {
        SceneManager.LoadScene(firstLevelBuildIndex);
    }      
}
