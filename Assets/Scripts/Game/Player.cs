using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    [SerializeField]
    public bool allowLeftMovement = false;

    [SerializeField]
    public bool allowRightMovement = false;

    [SerializeField]
    public bool allowPickup = false;

    public bool allowJump = false;

    [SerializeField]
    private float pickupDistance = 3f;

    [SerializeField]
    private float fallTimeDelay = 0.05f;

    public LayerMask walkLayer;

    public LayerMask pickupLayer;

    private List<Collider> colliders = new List<Collider>();

    private Collider[] myCols;

    public Transform pickupPoint;

    public HeldInteractable pickup;

    private List<PickupInsert> inserts = new List<PickupInsert>();

    private MeshFilter activeGlowFilter;

    private float lastFallTime = 0f;

    public float plugHoldingRange = 1.4f;



    private void Awake()
    {
        myCols = GetComponentsInChildren<Collider>(true);
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
            pickup.OnDrop();
            Collider[] hits = pickup.GetComponentsInChildren<Collider>();
            pickup.GetComponent<Rigidbody>().isKinematic = false;
            pickup = null;
        }
    }


    private static RaycastHit[] hits = new RaycastHit[128];
    private void Update()
    {
        if (allowPickup)
        {
            if (pickup != null)
            {
                float angle = transform.eulerAngles.y;
                pickup.transform.position = Vector3.Lerp(pickup.transform.position, pickupPoint.transform.position, Time.deltaTime * 5f);
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    DropHeld();
                }
                if (Input.GetMouseButtonDown(0))
                {
                    pickup.Interact();
                }
            }
            else if (pickup == null)
            {
                int hitCount = Physics.RaycastNonAlloc(Camera.main.ScreenPointToRay(Input.mousePosition), hits, Camera.main.farClipPlane, pickupLayer);
                if (hitCount > 0)
                {
                    Vector3 dist = (transform.position + Vector3.up) - hits[0].collider.transform.position;
                    if (dist.magnitude < pickupDistance)
                    {
                        SetGlowFilter(hits[0].collider.gameObject.GetComponentInChildren<MeshFilter>());

                        if (Input.GetMouseButtonDown(0))
                        {
                            HeldInteractable potentialInteractable = hits[0].collider.gameObject.GetComponent<Pickup>();
                            if (potentialInteractable.CanPickup())
                            {
                                pickup = potentialInteractable;

                                pickup.OnPickup(this);
                                SetGlowFilter(null);
                                pickup.GetComponent<Rigidbody>().isKinematic = true;
                            }
                        }
                    }
                }
                else
                    SetGlowFilter(null);
            }
        }

        Vector3 dif = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position + Vector3.up);

        dif.z = 0;
        dif = dif.normalized;
        if (Vector3.Dot(dif, Vector3.up) > -0.5f)
        {
            pickupPoint.position = transform.position + Vector3.up + dif.normalized * plugHoldingRange;
        }
        
        for (int i = 0; i < colliders.Count; i++)
        {
            foreach (Collider col in myCols)
            {
                Physics.IgnoreCollision(col, colliders[i], false);
            }
        }
        RaycastHit hit;
        if (Physics.Raycast(new Ray(transform.position + Vector3.up, Vector3.up), out hit, 2.2f, walkLayer))
        {
            lastFallTime = Time.time;
            foreach (Collider col in myCols)
            {
                Physics.IgnoreCollision(col, hit.collider, true);
            }
            colliders.Add(hit.collider);
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (Physics.Raycast(new Ray(transform.position + Vector3.up, Vector3.down), out hit, 2.2f, walkLayer))
            {
                lastFallTime = Time.time;
                foreach (Collider col in myCols)
                {
                    Physics.IgnoreCollision(col, hit.collider, true);
                }
                colliders.Add(hit.collider);
            }
        }
        if (Time.time - lastFallTime <= fallTimeDelay)
        {
            //keeps player from getting stuck in layers already falling through
            Collider[] tcols = Physics.OverlapCapsule(transform.position + Vector3.up * (0.5f), transform.position + Vector3.up * 1.5f, 0.4f, walkLayer);
            for (int i = 0; i < tcols.Length; i++)
            {
                foreach (Collider col in myCols)
                {
                    Physics.IgnoreCollision(col, tcols[i], true);
                }
                colliders.Add(tcols[i]);
            }
            if (tcols.Length > 0)
            {
                lastFallTime = Time.time;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * 0.5f, .4f);
        Gizmos.DrawWireSphere(transform.position + Vector3.up * 1.5f, .4f);
    }
}
