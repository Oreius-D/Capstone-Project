using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HazardKill : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Prendi il player anche se il collider è su un child
        var player = other.GetComponentInParent<PlayerController>();
        if (!player) return;

        // Animazione death
        var anim = player.GetComponent<PlayerAnimationController>();
        if (anim) anim.PlayDeath();

        // Restart con delay (se GameManager non c’è, fallback immediato)
        if (GameManager.Instance != null)
            GameManager.Instance.RestartLevel(0.25f);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
