using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Actor))]
public class DeathDropHeld : MonoBehaviour
{

    public Rigidbody body;
    public void Drop()
    {
        body.transform.SetParent(null);
        body.useGravity = true;
        foreach (Collider collider in body.GetComponentsInChildren<Collider>())
        {
            collider.enabled = true;
        }
    }
}
