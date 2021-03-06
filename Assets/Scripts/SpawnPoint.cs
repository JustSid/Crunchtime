﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnPoint : MonoBehaviour
{
    public bool isActive => gameObject.activeSelf;
    public GameObject playerPrefab;
    public GameObject cameraRigPrefab;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void Start()
    {
        Transform playerTransform = Instantiate(playerPrefab).transform;
        playerTransform.SetPositionAndRotation(transform.position, transform.rotation);
        PeekeyBoi cameraRig = Instantiate(cameraRigPrefab).GetComponent<PeekeyBoi>();
        cameraRig.followTarget = playerTransform;
        cameraRig.SnapToTarget();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + Vector3.up * 0.5f, 0.5f);
        Gizmos.DrawWireSphere(transform.position + Vector3.up * 1.5f, 0.5f);
    }

}
