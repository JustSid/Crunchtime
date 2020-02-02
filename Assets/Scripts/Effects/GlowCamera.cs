using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Camera))]
public class GlowCamera : MonoBehaviour
{
    private CommandBuffer buffer;

    [SerializeField]
    private Material glowShader;

    [SerializeField]
    private Camera cam;

    public static List<MeshFilter> renderers = new List<MeshFilter>();

    private void Awake()
    {
        cam = GetComponent<Camera>();
        buffer = new CommandBuffer();
        cam.AddCommandBuffer(CameraEvent.AfterEverything, buffer);
    }

    private void Update()
    {
        buffer.Clear();
        buffer.ClearRenderTarget(true, false, Color.black);
        foreach (MeshFilter renderer in renderers)
        {
            Mesh mesh = renderer.sharedMesh;
            for (int i = 0; i < mesh.subMeshCount; i++)
            {
                buffer.DrawMesh(renderer.sharedMesh, renderer.transform.localToWorldMatrix, glowShader, i);
            }
        }
    }


    private void OnDestroy()
    {
        if (cam != null)
        {
            cam.RemoveCommandBuffer(CameraEvent.AfterEverything, buffer);
        }
    }
}
