using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldInteractable : MonoBehaviour
{

    public virtual bool CanPickup()
    {
        return true;
    }

    public virtual void OnPickup(Player player)
    {
        gameObject.layer = 31;
    }

    public virtual void OnDrop()
    {
        gameObject.layer = 11;
    }

    public virtual void Interact()
    {

    }
}
