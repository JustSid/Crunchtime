using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plug : Pickup
{
    [System.Serializable]
    public enum ProngType
    {
        TwoNA, ThreeNA
    }

    public int ringMaterialIndex = 1;

    private Material ringMaterial;


    public ProngType prongType;

    public Wire wire;
    public PickupInsert socket = null;

    public bool HasPower()
    {
        if (wire)
            return wire.HasPower();

        return false;
    }

    protected override void Update()
    {
        base.Update();

        if (socket)
        {
            transform.rotation = Quaternion.Euler(new Vector3(-90, 0, 0));
        }

        if (HasPower())
        {
            ringMaterial.SetColor("_Color", Color.green);
            ringMaterial.SetColor("_EmissionColor", Color.green);
        }
        else
        {
            ringMaterial.SetColor("_Color", Color.red);
            ringMaterial.SetColor("_EmissionColor", Color.red);
        }
    }

    public override bool CanPickup()
    {
        if (base.CanPickup())
        {
            if (!socket || socket.CanUnplug())
                return true;
        }

        return false;
    }

    public override void OnPickup(Player player)
    {
        base.OnPickup(player);

        if (socket)
        {
            socket.OnUnplugged();
            socket = null;
        }
    }

    public override void Interact()
    {
        base.Interact();
        if (interactable != null)
        {
            PickupInsert insert = interactable.GetComponentInParent<PickupInsert>();
            if (insert.ProngType == prongType && insert.currentPlug == null)
            {
                socket = insert;

                body.isKinematic = true;
                socket.OnPluggedIn(this as Plug);
                LeaveHeld();
                body.isKinematic = true;
            }
        }
    }

    protected override void Awake()
    {
        List<Material> materials = new List<Material>();
        GetComponentInChildren<MeshRenderer>().GetSharedMaterials(materials);
        ringMaterial = Instantiate(materials[ringMaterialIndex]);
        materials[ringMaterialIndex] = ringMaterial;
        GetComponentInChildren<MeshRenderer>().sharedMaterials = materials.ToArray();
        base.Awake();
    }
}
