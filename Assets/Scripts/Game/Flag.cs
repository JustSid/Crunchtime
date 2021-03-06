﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Flag : WirePowerAction
{
    [SerializeField]
    private Material flagMaterial;

    [SerializeField]
    private MeshRenderer flagRenderer;

    [SerializeField]
    private AudioClip winClip;

    [SerializeField]
    private string sceneName;

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
            AudioSource.PlayClipAtPoint(winClip, transform.position);
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
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
