using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ReactsToPlayerTriggerBase : MonoBehaviour
{
    public PlayerTrigger[] validTriggers;
    protected abstract void DoTheThing(PlayerTrigger trigger);

    private void Evaluate(PlayerTrigger trigger)
    {
        for(int i = 0; i < validTriggers.Length; i++)
        {
            if (validTriggers[i] != trigger) continue;
            DoTheThing(trigger);
            return;
        }
    }

    void Awake()
    {
        PlayerTrigger.OnPlayerTriggered += Evaluate;
    }

    void OnDestroy()
    {
        PlayerTrigger.OnPlayerTriggered -= Evaluate;
    }
}
