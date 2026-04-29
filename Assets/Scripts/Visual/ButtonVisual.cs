using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonVisual : MonoBehaviour, IReciever
{
    [SerializeField] private GameObject onVisual;
    [SerializeField] private GameObject offVisual;

    public void SetState(bool on)
    {
        if (onVisual) onVisual.SetActive(on);
        if (offVisual) offVisual.SetActive(!on);
    }
}
