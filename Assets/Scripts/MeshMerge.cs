using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(MeshMerge))]
public class MeshMergeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Merge mesh"))
        {
            (target as MeshMerge).MergeChildren();
        }
    }
}
#endif
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MeshMerge : MonoBehaviour
{

    public List<MeshFilter> filters = new List<MeshFilter>();
    public Mesh mesh;
    public void MergeChildren()
    {
        Debug.Log("Filters count " + filters.Count);
        List<Vector3> verts = new List<Vector3>();
        List<Vector3> norms = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> tris = new List<int>();
        List<int> tris2 = new List<int>();
        foreach (MeshFilter filter in filters)
        {
            if (filter.gameObject != gameObject)
            {
                MeshRenderer renderer = filter.GetComponent<MeshRenderer>();
                Mesh tm = filter.sharedMesh;
                Vector3[] vertices = tm.vertices;
                Vector3[] normals = tm.normals;
                int start = verts.Count;
                int[] triangles = tm.GetTriangles(0);
                for (int i = 0; i < vertices.Length; i++)
                {
                    verts.Add(transform.InverseTransformPoint(filter.transform.TransformPoint(vertices[i])));
                    norms.Add(Vector3.Normalize(transform.InverseTransformDirection(filter.transform.TransformDirection(normals[i]))));
                }
                for (int i = 0; i < triangles.Length; i++)
                {
                    tris.Add(start + triangles[i]);
                }
                if (tm.subMeshCount > 1)
                {
                    triangles = tm.GetTriangles(1);
                    for (int i = 0; i < triangles.Length; i++)
                    {
                        tris2.Add(start + triangles[i]);
                    }
                }
                uvs.AddRange(tm.uv);
            }
        }

        Mesh mesh = new Mesh();
        mesh.SetVertices(verts);
        mesh.SetNormals(norms);
        mesh.SetUVs(0, uvs);
        mesh.subMeshCount = tris2.Count > 0 ? 2 : 1;
        mesh.SetTriangles(tris, 0);
        if (tris2.Count > 0)
        {
            mesh.SetTriangles(tris2, 1);
        }
#if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(this.mesh);
        if (this.mesh == null)
        {
            AssetDatabase.CreateAsset(mesh, "Assets/Outmesh.asset");
            AssetDatabase.Refresh();
            mesh = AssetDatabase.LoadAssetAtPath<Mesh>("Assets/Outmesh.asset");
            GetComponent<MeshFilter>().sharedMesh = AssetDatabase.LoadAssetAtPath<Mesh>("Assets/Outmesh.asset");
        }
        else
        {
            AssetDatabase.DeleteAsset(path);
            AssetDatabase.CreateAsset(mesh,path);
            AssetDatabase.SaveAssets();
            AssetDatabase.ImportAsset(path);
            AssetDatabase.Refresh();
            this.mesh = AssetDatabase.LoadAssetAtPath<Mesh>(path);
            GetComponent<MeshFilter>().sharedMesh = this.mesh;
        }
#else
        GetComponent<MeshFilter>().sharedMesh=mesh;
#endif
    }
}
