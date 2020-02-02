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

    }

    public virtual void OnDrop()
    {


    }

    public virtual void Interact()
    {

    }
}
