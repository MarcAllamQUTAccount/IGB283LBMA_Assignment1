using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class AssignmentShape : MonoBehaviour
{
    [SerializeField] private Material material;

    private Mesh mesh;
    private IGB283Vector[] baseVertices;   // original shape
    private IGB283Vector[] transformedVertices;
    private int[] triangles;

    // Movement parameters
    public IGB283Vector leftPoint = new IGB283Vector(-3f, 0f, 0f);
    public IGB283Vector rightPoint = new IGB283Vector(3f, 0f, 0f);
    public float moveSpeed = 1f;
    public float rotationSpeed = 1f; // radians per second

    private float time;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().material = material;

        CreateShape();
        UpdateMesh();
    }

    void CreateShape()
    {
        int triangleCount = 40;
        float radius = 1.5f;

        List<IGB283Vector> verts = new List<IGB283Vector>();
        List<int> tris = new List<int>();

        // Center vertex
        verts.Add(new IGB283Vector(0f, 0f, 0f));

        // Outer ring vertices
        for (int i = 0; i < triangleCount; i++)
        {
            float angle = (Mathf.PI * 2f) * (i / (float)triangleCount);
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            verts.Add(new IGB283Vector(x, y, 0f));
        }

        // Build triangles (triangle fan)
        for (int i = 1; i <= triangleCount; i++)
        {
            int next = (i == triangleCount) ? 1 : i + 1;

            tris.Add(0);      // center
            tris.Add(i);      // current outer vertex
            tris.Add(next);   // next outer vertex
        }

        baseVertices = verts.ToArray();
        transformedVertices = new IGB283Vector[baseVertices.Length];
        triangles = tris.ToArray();
    }

    void Update()
    {
        time += Time.deltaTime;

        // Side-to-side between leftPoint and rightPoint
        float t = Mathf.PingPong(time * moveSpeed, 1f);
        IGB283Vector pos = new IGB283Vector(
            Mathf.Lerp(leftPoint.x, rightPoint.x, t),
            Mathf.Lerp(leftPoint.y, rightPoint.y, t),
            0f
        );

        // Continuous rotation
        float angle = time * rotationSpeed;

        // No scaling (1,1)
        IGB283Vector scale = new IGB283Vector(1f, 1f, 1f);

        // Build TRS matrix using your own transform functions
        Matrix3x3 trs = IGB283Transform.TRS(pos, angle, scale);

        // Apply to all vertices
        for (int i = 0; i < baseVertices.Length; i++)
        {
            transformedVertices[i] = IGB283Transform.Apply(trs, baseVertices[i]);
        }

        UpdateMesh();
    }

    void UpdateMesh()
    {
        Vector3[] unityVerts = new Vector3[transformedVertices.Length];
        for (int i = 0; i < transformedVertices.Length; i++)
        {
            unityVerts[i] = transformedVertices[i].ToUnityVector3();
        }

        mesh.Clear();
        mesh.vertices = unityVerts;
        mesh.triangles = triangles;

        // --- DEBUG: Make triangles visible ---
        Color[] colors = new Color[unityVerts.Length];

        // Assign a unique color per triangle
        for (int i = 0; i < triangles.Length; i += 3)
        {
            Color triColor = new Color(
                Random.value,
                Random.value,
                Random.value
            );

            colors[triangles[i]] = triColor;
            colors[triangles[i + 1]] = triColor;
            colors[triangles[i + 2]] = triColor;
        }

        mesh.colors = colors;
        // -------------------------------------

        mesh.RecalculateNormals();
    }
}
