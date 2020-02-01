using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupInsert : MonoBehaviour
{
    [SerializeField]
    private BoxCollider collider;

    [SerializeField]
    private WirePowerAction powerActions;

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
        powerActions.OnPowerEnabledInternal();
    }

    public void OnUnplugged()
    {
        powerActions.OnPowerDisabledInternal();
    }
}
