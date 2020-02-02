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

    [SerializeField]
    private bool powered = false;

    private bool pluggedIn = false;


    public int ringMaterialIndex = 1;

    private Material ringMaterial;


    public ProngType prongType;

    public Wire wire;


    public bool HasPower
    {
        get { return powered; }
    }

    public void SetPowered(bool state)
    {
        powered = state;
    }

    protected override void Update()
    {
        base.Update();

        if (pluggedIn)
        {
            transform.rotation = Quaternion.Euler(new Vector3(-90, 0, 0));
        }

    }

    public override void Interact()
    {
        base.Interact();
        if (interactable != null)
        {

            PickupInsert insert = interactable.GetComponentInParent<PickupInsert>();
            if (insert.ProngType == prongType)
            {
                body.isKinematic = true;
                pluggedIn = true;
                insert.OnPluggedIn(this as Plug);
                pickedup.DropHeld();
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
        OnSocketDisconnected();
    }

    public void OnSocketConnected()
    {
        ringMaterial.SetColor("_Color", Color.green);
        ringMaterial.SetColor("_EmissionColor", Color.green);
    }

    public void OnSocketDisconnected()
    {
        ringMaterial.SetColor("_Color", Color.red);
        ringMaterial.SetColor("_EmissionColor", Color.red);
    }

}
