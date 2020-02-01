using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerDestroy : WirePowerAction
{
    [SerializeField]
    private List<GameObject> disableOnPower = new List<GameObject>();

    protected override void OnPowerEnabled()
    {
        base.OnPowerEnabled();
        for (int i = 0; i < disableOnPower.Count; i++)
        {
            GameObject.Destroy(disableOnPower[i]);
        }
        disableOnPower.Clear();
    }
}
