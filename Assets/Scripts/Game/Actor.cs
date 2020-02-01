using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Actor : MonoBehaviour
{
    public UnityEvent deathEvent;

    private void OnDestroy()
    {
        OnDeath();
    }

    public void OnDeath()
    {
        if (deathEvent != null)
        {
            deathEvent.Invoke();
        }
    }
}
