using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    public bool allowLeftMovement = false;

    [SerializeField]
    public bool allowRightMovement = false;

    [SerializeField]
    public bool allowPickup = false;

    public bool allowJump = false;

    public LayerMask walkLayer;

    public LayerMask pickupLayer;

    private List<Collider> colliders = new List<Collider>();

    private Collider[] myCols;

    public Transform pickupPoint;

    public HeldInteractable pickup;

    private List<PickupInsert> inserts = new List<PickupInsert>();

    private MeshFilter activeGlowFilter;

    private void Awake()
    {
        myCols = GetComponentsInChildren<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        AbilityPickup pickup = other.gameObject.GetComponentInChildren<AbilityPickup>();
        if (pickup != null)
        {
            switch (pickup.ability)
            {
                case AbilityPickup.Ability.Jump:
                    allowJump = true;
                    break;
                case AbilityPickup.Ability.Pickup:
                    Debug.Log("Enable pickup");
                    allowPickup = true;
                    break;
                case AbilityPickup.Ability.WalkLeft:
                    allowLeftMovement = true;
                    break;
                case AbilityPickup.Ability.WalkRight:
                    allowRightMovement = true;
                    break;
            }
            Destroy(pickup.gameObject);
        }
        InsertZone insert = other.gameObject.GetComponent<InsertZone>();
        if (insert != null)
        {
            Debug.Log("Add insert " + insert.name);
            inserts.Add(insert.insert);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PickupInsert insert = other.gameObject.GetComponentInChildren<PickupInsert>();
        if (insert != null)
        {
            Debug.Log("Remove insert " + insert.name);
            inserts.Remove(insert);
        }
    }

    private void SetGlowFilter(MeshFilter filter)
    {
        if (activeGlowFilter != null)
        {
            GlowCamera.renderers.Remove(activeGlowFilter);
        }
        if (filter != null)
        {
            activeGlowFilter = filter;
            GlowCamera.renderers.Add(filter);
        }
    }

    public void DropHeld()
    {
        if (pickup != null)
        {
            Collider[] hits = pickup.GetComponentsInChildren<Collider>();
            foreach (Collider col in myCols)
            {
                Physics.IgnoreCollision(hits[0], col, false);
            }
            pickup.GetComponent<Rigidbody>().useGravity = true;
            pickup = null;
        }
    }

    private void Update()
    {
        if (allowPickup)
        {
            if (pickup != null)
            {
                pickup.transform.position = Vector3.Lerp(pickup.transform.position, pickupPoint.transform.position, Time.deltaTime * 5f);
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    DropHeld();
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    pickup.Interact();
                }
            }
            else if (pickup == null)
            {
                Collider[] hits = Physics.OverlapBox(transform.position + Vector3.up + Vector3.right, Vector3.one * .5f, Quaternion.identity, pickupLayer);
                if (hits.Length > 0)
                {
                    SetGlowFilter(hits[0].gameObject.GetComponent<MeshFilter>());
                    if (Input.GetKeyDown(KeyCode.E) && pickup == null)
                    {

                        if (hits.Length > 0)
                        {
                            pickup = hits[0].GetComponent<Pickup>();
                            pickup.OnPickup(this);
                            SetGlowFilter(null);
                            foreach (Collider col in myCols)
                            {
                                Physics.IgnoreCollision(hits[0], col, false);
                            }
                            pickup.GetComponent<Rigidbody>().useGravity = false;
                        }
                    }
                }
            }
        }

        Vector3 dif = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position + Vector3.up);

        dif.z = 0;
        dif = dif.normalized;
        if (Vector3.Dot(dif, Vector3.up) > -0.5f)
        {
            pickupPoint.position = transform.position + Vector3.up + dif.normalized * 1.4f;
        }

        for (int i = 0; i < colliders.Count; i++)
        {
            foreach (Collider col in myCols)
            {
                Physics.IgnoreCollision(col, colliders[i], false);
            }
        }
        RaycastHit hit;
        if (Physics.Raycast(new Ray(transform.position, Vector3.up), out hit, 2.2f, walkLayer))
        {
            foreach (Collider col in myCols)
            {
                Physics.IgnoreCollision(col, hit.collider, true);
            }
            colliders.Add(hit.collider);
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (Physics.Raycast(new Ray(transform.position + Vector3.up * 2f, Vector3.down), out hit, 2.2f, walkLayer))
            {
                foreach (Collider col in myCols)
                {
                    Physics.IgnoreCollision(col, hit.collider, true);
                }
                colliders.Add(hit.collider);
            }
        }
        Collider[] tcols = Physics.OverlapCapsule(transform.position + Vector3.up * 0.5f, transform.position + Vector3.up * 1.5f, 0.45f, walkLayer);
        for (int i = 0; i < tcols.Length; i++)
        {
            foreach (Collider col in myCols)
            {
                Physics.IgnoreCollision(col, tcols[i], true);
            }
            colliders.Add(tcols[i]);
        }

    }
}
