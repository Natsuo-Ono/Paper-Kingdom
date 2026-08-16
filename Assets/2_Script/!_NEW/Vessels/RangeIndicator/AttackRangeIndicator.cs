using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class AttackRangeIndicator : MonoBehaviour
{
    [Header("References")]
    public VesselTargeting vesselTargeting;

    [Header("Circle")]
    public int segments = 64;

    [Header("Height")]
    public float height = 0.1f;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        CreateCircle();

        Hide();
    }

    private void CreateCircle()
    {
        Mesh mesh = new Mesh();
        mesh.name = "Attack Range Circle";

        Vector3[] vertices = new Vector3[segments + 1];
        int[] triangles = new int[segments * 3];

        vertices[0] = new Vector3(0f, height, 0f);

        for (int i = 0; i < segments; i++)
        {
            float angle = (float)i / segments * Mathf.PI * 2f;

            float x = Mathf.Cos(angle);
            float z = Mathf.Sin(angle);

            vertices[i + 1] = new Vector3(
                x,
                height,
                z
            );
        }

        for (int i = 0; i < segments; i++)
        {
            int current = i + 1;
            int next = ((i + 1) % segments) + 1;

            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = next;
            triangles[i * 3 + 2] = current;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();

        meshFilter.sharedMesh = mesh;
    }

    private void Update()
    {
        if (vesselTargeting == null)
            return;

        float radius = vesselTargeting.attackRange;

        transform.localScale = new Vector3(
            radius,
            1f,
            radius
        );
    }

    public void Show()
    {
        meshRenderer.enabled = true;
    }

    public void Hide()
    {
        meshRenderer.enabled = false;
    }
}