using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class Pickup : HeldInteractable
{
    protected Collider col;
    protected Rigidbody body;

    protected Player pickedup;
    protected InsertZone interactable;



    protected virtual void Awake()
    {
        col = GetComponent<Collider>();
        body = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        if (pickedup != null)
        {
            transform.rotation = Quaternion.Euler(new Vector3(135, 0, 0));
        }
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
