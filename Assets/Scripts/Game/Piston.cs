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

    [SerializeField]
    private LayerMask squashTestMask;

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

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(new Ray(head.position, transform.up), out hit, 100, squashTestMask, QueryTriggerInteraction.Ignore))
        {
            Debug.DrawLine(head.position, head.position + transform.up * hit.distance, Color.green, 1f);
            float dist = hit.distance;

            Vector3 extents = head.GetComponent<Renderer>().bounds.extents * 0.5f;
            extents += transform.up * dist * 0.5f;
            Collider[] squashed = Physics.OverlapBox(head.position + transform.up * dist * 0.5f, extents, Quaternion.identity);
            foreach (Collider collider in squashed)
            {
                if (collider.bounds.min.y > head.position.y && collider.bounds.max.y - collider.bounds.min.y > dist - .1f)
                {
                    Destroy(collider.gameObject);
                }
            }
        }
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
                float mag = upForce * Time.fixedDeltaTime;
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
                float mag = downForce * Time.fixedDeltaTime;
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
