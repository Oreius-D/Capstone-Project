using UnityEngine;

public class SfxTest : MonoBehaviour
{
    [SerializeField] private AudioClip sfx;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            AudioManager.Instance?.PlaySFX(sfx);
    }
}
