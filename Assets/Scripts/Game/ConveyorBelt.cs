using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : WirePowerAction
{
    private Player activePlayer = null;
    private PlayerController activeController = null;

    [SerializeField]
    private YouSpinMeRoundRightRound[] spinners = new YouSpinMeRoundRightRound[4];

    [SerializeField]
    private Vector3 direction = Vector3.right;

    private bool fudgedPlayer = false;

    void Awake()
    {
        foreach (YouSpinMeRoundRightRound spinner in spinners)
        {
            spinner.localAxis = new Vector3(0, 0, direction.x);
        }
    }

    void Update()
    {
        foreach(YouSpinMeRoundRightRound spinner in spinners)
        {
            spinner.rate = powered ? 100 : 0;
        }
    }

    protected override void OnPowerDisabled()
    {
        base.OnPowerDisabled();

        if (fudgedPlayer)
        {
            activeController.EnableGroundVelocityDecay();
            fudgedPlayer = false;
        }
    }

    protected override void OnPowerEnabled()
    {
        base.OnPowerEnabled();

        if (!fudgedPlayer && activeController)
        {
            activeController.DisableGroundVelocityDecay();
            fudgedPlayer = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if(player != null)
        {
            activePlayer = player;
            activeController = activePlayer.GetComponent<PlayerController>();

            if(!fudgedPlayer && powered)
            {
                activeController.DisableGroundVelocityDecay();
                fudgedPlayer = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if(player != null && activePlayer == player)
        {
            if (fudgedPlayer)
            {
                activeController.EnableGroundVelocityDecay();
                fudgedPlayer = false;
            }

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
