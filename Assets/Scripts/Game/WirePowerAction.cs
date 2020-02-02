using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WirePowerAction : MonoBehaviour
{
    [SerializeField]
    protected bool powered = false;

    [SerializeField]
    public Transform attachmentPoint = null;

    public void OnPowerEnabledInternal()
    {
        if (!powered)
        {
            powered = true;
            OnPowerEnabled();
        }
    }

    public void OnPowerDisabledInternal()
    {
        if (powered)
        {
            powered = false;
            OnPowerDisabled();
        }
    }

    protected virtual void OnPowerEnabled()
    {

    }

    protected virtual void OnPowerDisabled()
    {
    
    }
}
