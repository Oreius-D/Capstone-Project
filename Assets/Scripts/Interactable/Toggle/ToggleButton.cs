using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleButton : MonoBehaviour
{
    // Reference to array of recievers
    [Header("Recievers")]
    [SerializeField] private MonoBehaviour[] recievers;
    private IReciever[] _recievers;

    // State of the button
    [Header("State")]
    [SerializeField] private bool startOn = false;
    private bool _isOn;

    // Awake is called before Start
    private void Awake()
    {
        // Set initial state
        _isOn = startOn;

        // Get IReciever components from the recievers array
        _recievers = new IReciever[recievers.Length];
        for (int i = 0; i < recievers.Length; i++)
        {
            _recievers[i] = recievers[i] as IReciever;
            if (_recievers[i] == null)
            {
                Debug.LogError($"Reciever at index {i} does not implement IReciever interface.");
            }
        }

        // Set initial state of recievers
        Broadcast();
    }

    // OnTriggerEnter is called when another collider enters the trigger collider attached to this object
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check it's the player
        if (!other.GetComponent<PlayerController>()) return;

        _isOn = !_isOn; // Toggle the state

        Broadcast(); // Broadcast the new state to all recievers
    }

    // Broadcast the current state to all recievers
    private void Broadcast()
    {
        foreach (var reciever in _recievers)
        {
            if (reciever != null)
            {
                reciever.SetState(_isOn);
            }
        }
    }
}
