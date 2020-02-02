using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupInsert : MonoBehaviour
{
    [SerializeField]
    private BoxCollider collider;

    [SerializeField]
    private WirePowerAction powerActions;


    public Plug currentPlug = null;

    public Transform plugPoint;

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

    private void Update()
    {
        if (currentPlug != null)
        {
            currentPlug.transform.position = plugPoint.transform.position;
            currentPlug.transform.rotation = Quaternion.Euler(-90, 0, 0);
        }
    }

    private void OnDrawGizmosSelected()
    {

    }


    public void OnPluggedIn(Plug plug)
    {
        this.currentPlug = plug;
        plug.OnSocketConnected();
        powerActions.OnPowerEnabledInternal();
    }

    public void OnUnplugged()
    {
        if (currentPlug != null)
        {
            currentPlug.OnSocketDisconnected();
        }
        currentPlug = null;
        powerActions.OnPowerDisabledInternal();
    }
}
