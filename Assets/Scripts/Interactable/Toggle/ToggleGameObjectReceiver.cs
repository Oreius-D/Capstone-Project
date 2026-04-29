using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleGameObjectReceiver : MonoBehaviour, IReciever
{
    // Reference to the GameObject to toggle
    [SerializeField] private GameObject target;
    [SerializeField] private bool invert = false;

    // SetState is called by the ToggleButton to update the state of the target GameObject
    public void SetState(bool on)
    {
        if (target)
            target.SetActive(invert ? !on : on);
        else
            Debug.LogError("Target GameObject is not assigned.");
    }
}
