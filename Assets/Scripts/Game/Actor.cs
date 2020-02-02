using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Actor : MonoBehaviour
{
    public UnityEvent deathEvent;

    [SerializeField]
    private AudioClip deathEffect;

    private void OnDestroy()
    {
    }

    public void OnDeath()
    {
        AudioSource.PlayClipAtPoint(deathEffect, transform.position);
        if (deathEvent != null)
        {
            deathEvent.Invoke();
        }
    }
}
