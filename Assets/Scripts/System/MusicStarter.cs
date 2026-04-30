using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicStarter : MonoBehaviour
{
    [SerializeField] private AudioClip music;

    private void Start()
    {
        AudioManager.Instance?.PlayMusic(music, true);
    }
}
