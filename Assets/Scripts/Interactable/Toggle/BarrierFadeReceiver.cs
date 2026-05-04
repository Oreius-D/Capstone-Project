using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierFadeReceiver : MonoBehaviour, IReciever
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Collider2D col;
    [SerializeField] private float fadeOutTime = 0.35f;
    [SerializeField] private float fadeInTime = 0.35f;
    [SerializeField] private bool startOn = true;

    [SerializeField] private bool disableColliderWhenOff = true;
    [SerializeField] private bool disableRendererWhenOff = false;

    private Coroutine routine;

    private void Reset()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    private void Awake()
    {
        if (!sr) sr = GetComponentInChildren<SpriteRenderer>(true);
        if (!col) col = GetComponent<Collider2D>();
    }

    private void Start()
    {
        ApplyInstant(startOn);
    }

    private void ApplyInstant(bool on)
    {
        if (!sr) return;

        Color c = sr.color;
        c.a = on ? 1f : 0f;
        sr.color = c;

        if (col) col.enabled = on || !disableColliderWhenOff ? true : false;

        if (disableRendererWhenOff)
            sr.enabled = on;
        else
            sr.enabled = true;
    }

    public void SetState(bool on)
    {
        // on=true => barriera si spegne (svanisce)
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(FadeTo(on));
    }

    private IEnumerator FadeTo(bool on)
    {
        if (!sr) yield break;

        // Assicura che il renderer sia attivo prima di un fade-in
        if (on)
        {
            sr.enabled = true;
            if (col) col.enabled = true; // torna solida subito (se vuoi, puoi spostarlo a fine fade)
        }

        float duration = on ? fadeInTime : fadeOutTime;

        Color c = sr.color;
        float startA = c.a;
        float endA = on ? 1f : 0f;

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(startA, endA, Mathf.Clamp01(t / duration));
            c.a = a;
            sr.color = c;
            yield return null;
        }

        c.a = endA;
        sr.color = c;

        if (!on)
        {
            if (disableColliderWhenOff && col) col.enabled = false;
            if (disableRendererWhenOff) sr.enabled = false;
        }
    }
}
