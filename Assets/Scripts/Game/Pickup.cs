using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class Pickup : HeldInteractable
{
    private Collider col;
    private Rigidbody body;

    private Player pickedup;
    private InsertZone interactable;

    private bool pluggedIn = false;


    protected virtual void Awake()
    {
        col = GetComponent<Collider>();
        body = GetComponent<Rigidbody>();
    }

    public override void OnPickup(Player player)
    {
        base.OnPickup(player);
        this.pickedup = player;
    }

    private void Update()
    {
        if (pickedup != null)
        {
            transform.rotation = Quaternion.Euler(new Vector3(135, 0, 0));
        }
        else if (pluggedIn)
        {
            transform.rotation = Quaternion.Euler(new Vector3(-90, 0, 0));
        }
    }

    public override void OnDrop()
    {
        this.pickedup = null;
    }

    public override void Interact()
    {
        base.Interact();
        if (interactable != null)
        {
            body.isKinematic = true;
            pluggedIn = true;
            Debug.Log("Plug in!");
            interactable.GetComponentInParent<PickupInsert>().OnPluggedIn(this as Plug);
            pickedup.DropHeld();
            body.isKinematic = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        InsertZone insert = other.gameObject.GetComponent<InsertZone>();
        if (insert != null)
        {
            interactable = insert;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InsertZone insert = other.gameObject.GetComponent<InsertZone>();
        if (insert != null && interactable != null && insert == interactable)
        {
            interactable = null;
        }
    }
}
