using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : WirePowerAction
{
    [SerializeField]
    private Material flagMaterial;

    [SerializeField]
    private MeshRenderer flagRenderer;

    private bool alwaysPowered = false;

    private void Awake()
    {
        flagMaterial = GameObject.Instantiate(flagMaterial);
        flagRenderer.sharedMaterial = flagMaterial;
        OnPowerChanged();
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (powered || alwaysPowered)
        {
            Debug.Log("You wiiiinn!");
        }
    }

    protected override void OnPowerDisabled()
    {
        base.OnPowerDisabled();
        OnPowerChanged();
    }

    private void OnPowerChanged()
    {
        if (alwaysPowered || powered)
        {
            flagMaterial.SetColor("_Color", Color.green);
            flagMaterial.SetColor("_EmissionColor", Color.green);
        }
        else
        {
            flagMaterial.SetColor("_Color", Color.red);
            flagMaterial.SetColor("_EmissionColor", Color.red);
        }
    }

    protected override void OnPowerEnabled()
    {
        base.OnPowerEnabled();
        OnPowerChanged();
    }
}
