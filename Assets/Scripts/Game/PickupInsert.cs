using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupInsert : MonoBehaviour
{
    [SerializeField]
    private BoxCollider collider;

    [SerializeField]
    private List<WirePowerAction> powerActions = new List<WirePowerAction>();

    private List<LineRenderer> lineRenderers = new List<LineRenderer>();

    [SerializeField]
    private bool unpluggable = false;

    [SerializeField]
    private bool providesPower = false;

    private bool isPowered = false;

    [SerializeField]
    private Plug.ProngType prongType;


    [SerializeField]
    private BoxCollider standCollider;

    public InsertZone insertZone;

    public Plug.ProngType ProngType
    {
        get { return prongType; }
    }

    void Awake()
    {
        Material material = new Material(Shader.Find("Unlit/VertexColor"));

        foreach (WirePowerAction action in powerActions)
        {
            GameObject go = new GameObject();

            LineRenderer renderer = go.AddComponent(typeof(LineRenderer)) as LineRenderer;

            go.transform.SetParent((transform));
            go.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

            Vector3 startPoint = action.attachmentPoint ? action.attachmentPoint.position : action.transform.position;
            Vector3[] segments = BezierThing.CalculateAutoAttachmentPoints(startPoint, transform.position);

            renderer.positionCount = segments.Length;
            renderer.SetPositions(segments);

            renderer.startColor = Color.red;
            renderer.endColor = Color.red;
            renderer.startWidth = 0.1f;
            renderer.endWidth = 0.1f;
            renderer.material = material;

            lineRenderers.Add(renderer);
        }
    }


    public bool CanUnplug()
    {
        return unpluggable;
    }
    public bool ProvidesPower()
    {
        return providesPower;
    }

    public Plug currentPlug = null;

    public Transform plugPoint;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        foreach (WirePowerAction action in powerActions)
        {
            if(action)
                Gizmos.DrawLine(action.transform.position, transform.position);
        }

        if (collider == null)
        {
            return;
        }
        if (collider != null)
        {
            Gizmos.color = Color.white;
            Gizmos.matrix = collider.transform.localToWorldMatrix;
            Gizmos.DrawWireCube(collider.center, collider.size);
        }
    }

    private void Update()
    {
        if(currentPlug && insertZone)
        {
            Plug temp = currentPlug;
            currentPlug = null;
            temp.InteractWithInsert(insertZone);
            insertZone = null;
        }

        if (currentPlug != null)
        {
            currentPlug.transform.position = plugPoint.transform.position;
            currentPlug.transform.rotation = Quaternion.Euler(-90, 0, 0);
        }

        bool hasPower = providesPower;

        if (!hasPower && currentPlug)
            hasPower = currentPlug.HasPower();

        if (hasPower != isPowered)
        {
            isPowered = hasPower;

            if (isPowered)
            {
                foreach (WirePowerAction action in powerActions)
                    action.OnPowerEnabledInternal();
            }
            else
            {
                foreach (WirePowerAction action in powerActions)
                    action.OnPowerDisabledInternal();
            }

            Color color = isPowered ? Color.green : Color.white;

            foreach (LineRenderer renderer in lineRenderers)
            {
                renderer.startColor = color;
                renderer.endColor = color;
            }
        }
    }

    public void OnPluggedIn(Plug plug)
    {
        currentPlug = plug;
        standCollider.enabled = true;
    }

    public void OnUnplugged()
    {
        currentPlug = null;
        standCollider.enabled = false;
    }
}
