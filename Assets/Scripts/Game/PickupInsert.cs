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

    void Awake()
    {
        Material material = new Material(Shader.Find("Unlit/VertexColor"));

        foreach (WirePowerAction action in powerActions)
        {
            GameObject go = new GameObject();

            LineRenderer renderer = go.AddComponent(typeof(LineRenderer)) as LineRenderer;

            go.transform.SetParent((transform));
            go.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

            Vector3[] segments = new Vector3[2];
            segments[0] = action.attachmentPoint ? action.attachmentPoint.position : action.transform.position;
            segments[1] = transform.position;

            renderer.SetPositions(segments);

            renderer.startColor = Color.red;
            renderer.endColor = Color.red;
            renderer.startWidth = 0.1f;
            renderer.endWidth = 0.1f;
            renderer.material = material;

            lineRenderers.Add(renderer);
        }
    }


    public Plug currentPlug = null;

    public Transform plugPoint;

    private void OnDrawGizmos()
    {
        if (collider == null)
        {
            return;
        }
        if (collider != null)
        {
            Gizmos.matrix = collider.transform.localToWorldMatrix;
            Gizmos.DrawWireCube(collider.center, collider.size);
        }
    }

    private void Update()
    {
        if (currentPlug != null)
        {
            currentPlug.transform.position = plugPoint.transform.position;
            currentPlug.transform.rotation = Quaternion.Euler(-90, 0, 0);
        }
    }

    public void OnPluggedIn(Plug plug)
    {
        this.currentPlug = plug;
        plug.OnSocketConnected();

        foreach(WirePowerAction action in powerActions)
            action.OnPowerEnabledInternal();

        foreach (LineRenderer renderer in lineRenderers)
        {
            renderer.startColor = Color.green;
            renderer.endColor = Color.green;
        }
    }

    public void OnUnplugged()
    {
    	if (currentPlug != null)
        {
            currentPlug.OnSocketDisconnected();
        }
        currentPlug = null;

        foreach(WirePowerAction action in powerActions)
            action.OnPowerDisabledInternal();

        foreach(LineRenderer renderer in lineRenderers)
        {
            renderer.startColor = Color.white;
            renderer.endColor = Color.white;
        }
    }
}
