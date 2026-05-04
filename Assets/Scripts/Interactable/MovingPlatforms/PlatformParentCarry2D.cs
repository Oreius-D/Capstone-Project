using UnityEngine;

public class PlatformParentCarry2D : MonoBehaviour
{
    [SerializeField] private Transform platformRoot;

    private Transform currentPlayer;
    private Transform previousParent;

    private void Reset()
    {
        if (!platformRoot) platformRoot = transform.parent; // root piattaforma
        // Assicurati che questo collider sia trigger
        var c = GetComponent<Collider2D>();
        if (c) c.isTrigger = true;
    }

    private void Awake()
    {
        if (!platformRoot) platformRoot = transform.parent;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var pc = other.GetComponentInParent<PlayerController>();
        if (!pc) return;

        currentPlayer = pc.transform;

        // evita doppio enter
        if (currentPlayer.parent == platformRoot) return;

        previousParent = currentPlayer.parent;
        currentPlayer.SetParent(platformRoot, true);

        Debug.Log($"[Carry] Parent ON -> {currentPlayer.name} parent={platformRoot.name}");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var pc = other.GetComponentInParent<PlayerController>();
        if (!pc) return;

        if (currentPlayer != pc.transform) return;

        currentPlayer.SetParent(previousParent, true);

        Debug.Log($"[Carry] Parent OFF -> {currentPlayer.name} parent={(previousParent ? previousParent.name : "null")}");
        currentPlayer = null;
        previousParent = null;
    }
}
