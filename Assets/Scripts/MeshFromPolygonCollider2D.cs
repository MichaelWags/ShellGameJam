using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(LineRenderer))]

public class MeshFromPolygonCollider2D : MonoBehaviour
{
    // Based onh ttp://answers.unity3d.com/questions/835675/how-to-fill-polygon-collider-with-a-solid-color.html

    enum Interior { None, Filled }
    enum Outline { None, Present }

    [SerializeField] Material interiorMat;
    [SerializeField] Material outlineMat;
    [SerializeField] float outlineThickness = 0.01f;
    [SerializeField] Interior interior = Interior.Filled;
    [SerializeField] Outline outline = Outline.Present;

    PolygonCollider2D polygonCollider2D;
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    LineRenderer lineRenderer;

    void Start()
    {
        
    }

    public void CreateMesh()
    {
        if (polygonCollider2D == null) polygonCollider2D = gameObject.GetComponent<PolygonCollider2D>();
        if (meshFilter == null) meshFilter = GetComponent<MeshFilter>();
        if (meshRenderer == null) meshRenderer = GetComponent<MeshRenderer>();
        if ((meshFilter == null || meshRenderer == null) && interior == Interior.None) return;
        if (meshFilter == null)
        {
            Debug.LogError(this + " has null meshFilter");
            return;
        }

        if (meshRenderer == null)
        {
            Debug.LogError(this + " has null meshRenderer");
            return;
        }

        if (interior == Interior.None)
        {
            meshRenderer.enabled = false;
            return;
        }

        meshRenderer.enabled = true;
        meshRenderer.material = interiorMat;
        // FIXME: only works at 0,0,0 coordinates for some reason. Otherwise doubles each coordinate for mesh
        meshFilter.mesh = polygonCollider2D.CreateMesh(false, false);
        meshFilter.mesh.Optimize();
    }

    public void CreateLine()
    {
        if (polygonCollider2D == null) polygonCollider2D = gameObject.GetComponent<PolygonCollider2D>();
        if (lineRenderer == null) lineRenderer = GetComponent<LineRenderer>();

        if (lineRenderer == null && outline == Outline.None) return;
        if (lineRenderer == null)
        {
            Debug.LogError(this + " has null lineRenderer");
            return;
        }
        if (outline == Outline.None)
        {
            lineRenderer.enabled = false;
            return;
        }

        lineRenderer.enabled = true;
        lineRenderer.useWorldSpace = false;
        lineRenderer.startWidth = lineRenderer.endWidth = outlineThickness;
        lineRenderer.material = outlineMat;

        lineRenderer.positionCount = polygonCollider2D.points.Length;
        for (int i = 0; i < polygonCollider2D.points.Length; i++)
        {
            lineRenderer.SetPosition(i, polygonCollider2D.points[i]);
        }
    }
}
