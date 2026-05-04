using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalStopReceiver : MonoBehaviour, IReciever
{
    [SerializeField] private Animator animator;

    [Tooltip("Se true: quando on=true il cristallo gira. Se false: gira quando on=false.")]
    [SerializeField] private bool animateWhenOn = true;

    private void Reset()
    {
        animator = GetComponentInChildren<Animator>(true);
    }

    private void Awake()
    {
        if (!animator) animator = GetComponentInChildren<Animator>(true);
    }

    public void SetState(bool on)
    {
        if (!animator) return;
        bool shouldAnimate = animateWhenOn ? on : !on;
        animator.enabled = shouldAnimate;
    }
}
