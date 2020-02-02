using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plug : Pickup
{
    public enum ProngType
    {
        TwoNA, ThreeNA
    }

    public int ringMaterialIndex = 1;

    private Material ringMaterial;

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

    public ProngType prongType;

}
