using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    public delegate void PlayerTriggerHandler(PlayerTrigger invoker);
    public static event PlayerTriggerHandler OnPlayerTriggered;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player")) OnPlayerTriggered?.Invoke(this);
    }


    [ContextMenu("Force Fire Event")]
    public void ForceEvent()
    {
        if(!Application.isPlaying) return;
        OnPlayerTriggered?.Invoke(this);
    }
}
