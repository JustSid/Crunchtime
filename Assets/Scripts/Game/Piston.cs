using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField]
    private LayerMask squashKillMask;
    [SerializeField]
    private bool usePistonNeck = false;
    [SerializeField]
    private GameObject pistonNeck;
    public Transform head;
    private Vector3 headStart;

    private void Awake()
    {
        headStart = head.transform.position;
        if (!usePistonNeck)
        {
            Destroy(pistonNeck);
        }
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
        Vector3 killdir = travellingUp ? transform.up : -transform.up;
        if (Physics.Raycast(new Ray(head.position, killdir), out hit, 100, squashTestMask, QueryTriggerInteraction.Ignore))
        {
            Debug.DrawLine(head.position, head.position + killdir * hit.distance, Color.green, 1f);
            float dist = hit.distance;
            if (dist > 0.2f)
            {
                Vector3 extents = head.GetComponent<Renderer>().bounds.extents * 0.95f;
                extents += killdir * dist * 0.5f;
                Collider[] squashed = Physics.OverlapBox(head.position + killdir * dist * 0.5f, extents, Quaternion.identity, squashKillMask);
                if (squashed.Length > 0)
                {
                    int axis = 0;
                    float largeV = Mathf.Abs(killdir[0]);
                    for (int i = 1; i < 3; i++)
                    {
                        if (Mathf.Abs(killdir[i]) > largeV)
                        {
                            axis = i;
                            largeV = Mathf.Abs(killdir[i]);
                        }
                    }
                    foreach (Collider collider in squashed)
                    {

                        if (hit.distance <= collider.bounds.max[axis] - collider.bounds.min[axis])
                        {
                            Player player = collider.gameObject.GetComponent<Player>();
                            if (player != null)
                            {
                                player.OnDeath();
                                if (this.enabled)
                                {
                                    this.enabled = false;
                                }
                                DestroyImmediate(collider.gameObject);
                                DestroyImmediate(gameObject);
                                Debug.Log("Player Loading Scene");
                                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
                                return;

                            }
                            else
                            {
                                Destroy(collider.gameObject);
                            }
                        }
                    }
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
                    activePlayer.GetComponent<PlayerController>().AddForce(transform.up * upForcePlayer);
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
        if (usePistonNeck)
        {
            pistonNeck.transform.position = (headStart + head.position) * 0.5f;
            pistonNeck.transform.localScale = Vector3.Max(Vector3.up * (headStart - head.position).magnitude, new Vector3(1, 1, 1) - Vector3.up);
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
        if (head != null)
        {
            RaycastHit hit;
            Gizmos.matrix = Matrix4x4.identity;
            Vector3 killdir = travellingUp ? transform.up : -transform.up;
            if (Physics.Raycast(new Ray(head.position, killdir), out hit, 100, squashTestMask, QueryTriggerInteraction.Ignore))
            {
                float dist = hit.distance;
                Vector3 extents = head.GetComponent<Renderer>().bounds.extents * 0.95f;
                extents += killdir * dist * 0.5f;
                Collider[] squashed = Physics.OverlapBox(head.position + killdir * dist * 0.5f, extents, Quaternion.identity);
                Gizmos.DrawWireCube(head.position + killdir * dist * 0.5f, extents * 2f);
            }
        }
    }
}
