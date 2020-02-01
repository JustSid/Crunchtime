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

    private void Awake()
    {
        col = GetComponent<Collider>();
        body = GetComponent<Rigidbody>();
    }

    public override void OnPickup(Player player)
    {
        base.OnPickup(player);
        this.pickedup = player;
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
            Debug.Log("Plug it in");
            interactable.GetComponentInParent<PickupInsert>().OnPluggedIn();
            pickedup.DropHeld();
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
