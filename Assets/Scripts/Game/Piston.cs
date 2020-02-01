using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piston : WirePowerAction
{

    public float upForce = 5f;
    public float upForcePlayer = 1f;
    public float downForce = 2f;
    public bool travellingUp = true;
    public bool activated = false;
    public float headDistance = 1f;
    private Player activePlayer = null;

    public Transform head;
    private Vector3 headStart;


    private void Awake()
    {
        headStart = head.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (player != null)
        {
            Debug.Log("Enable player lift");
            activePlayer = player;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (player != null && activePlayer == player)
        {
            Debug.Log("Disable player lift");
            activePlayer = null;
        }
    }

    protected override void OnPowerEnabled()
    {
        base.OnPowerEnabled();
        activated = true;
        travellingUp = true;
    }

    private void FixedUpdate()
    {
        if (activated)
        {
            if (travellingUp)
            {
                if (activePlayer != null)
                {
                    activePlayer.GetComponent<PlayerController>().AddForce(Vector3.up * upForcePlayer);
                }
                float mag = upForce * Time.deltaTime;
                float remaining = headDistance - (head.position - headStart).magnitude;
                if (remaining <= mag)
                {
                    travellingUp = false;
                    head.position = headStart + transform.up * headDistance;
                }
                else
                {
                    head.position += transform.up * mag;
                }
            }
            else
            {
                float mag = downForce * Time.deltaTime;
                Vector3 dist = head.position - headStart;
                if (dist.magnitude <= mag)
                {
                    travellingUp = true;
                    head.position = headStart;
                    if (powered == false)
                    {
                        activated = false;
                    }
                }
                else
                {
                    head.position -= dist.normalized * mag;
                }


            }
        }
    }


    private void OnDrawGizmos()
    {
        if (head != null)
        {
            if (Application.isPlaying)
            {
                Gizmos.DrawLine(headStart, headStart + transform.up * headDistance);
            }
            else
            {
                Gizmos.DrawLine(head.position, head.position + transform.up * headDistance);
            }
        }
    }
}
