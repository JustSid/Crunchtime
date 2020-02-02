using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupInsert : MonoBehaviour
{
    [SerializeField]
    private BoxCollider collider;

    [SerializeField]
    private List<WirePowerAction> powerActions = new List<WirePowerAction>();

    private void OnDrawGizmos()
    {
        if (collider == null)
        {
            return;
        }
        if (collider != null)
        {
            Gizmos.matrix = collider.transform.localToWorldMatrix;
            Gizmos.DrawWireCube(collider.center, collider.size);
        }
    }

    public void OnPluggedIn()
    {
        foreach(WirePowerAction action in powerActions)
            action.OnPowerEnabledInternal();
    }

    public void OnUnplugged()
    {
        foreach(WirePowerAction action in powerActions)
            action.OnPowerDisabledInternal();
    }
}
