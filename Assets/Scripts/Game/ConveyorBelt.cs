using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : WirePowerAction
{
    private Player activePlayer = null;
    private PlayerController activeController = null;

    [SerializeField]
    private Vector3 direction = Vector3.right;

    void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if(player != null)
        {
            Debug.Log("Enable player conveying");
            activePlayer = player;
            activeController = activePlayer.GetComponent<PlayerController>();

            activeController.DisableGroundVelocityDecay();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if(player != null && activePlayer == player)
        {
            Debug.Log("Disable player conveying");
            activeController.EnableGroundVelocityDecay();

            activeController = null;
            activePlayer = null;
        }
    }

    private void FixedUpdate()
    {
        if(powered)
        {
            if (activePlayer != null)
                activePlayer.GetComponent<PlayerController>().AddForce(direction * 5f);
        }
    }
}
