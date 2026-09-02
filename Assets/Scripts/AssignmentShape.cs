using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class AssignmentShape : MonoBehaviour
{
    [SerializeField] private Material material;

    private Mesh mesh;
    private IGB283Vector[] baseVertices;
    private int[] triangles;

    [System.Serializable]
    public class ShapeInstance
    {
        public IGB283Vector leftPoint;
        public IGB283Vector rightPoint;
        public float movespeed;
        public float rotationSpeed;

        public float time;
        public float t;

        public IGB283Vector[] transformedVertices;
    }

    public ShapeInstance[] shapes;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().material = material;

        CreateShape();

        shapes = new ShapeInstance[2];
        // make it variables instead to track the x or y values of the boundary boxes
        shapes[0] = new ShapeInstance()
        {
            leftPoint = new IGB283Vector(-5, 0, 0),
            rightPoint = new IGB283Vector(5, 0, 0),
            movespeed = 0.5f,
            rotationSpeed = 0.5f,
            time = 0f,
            transformedVertices = new IGB283Vector[baseVertices.Length]
        };

        shapes[1] = new ShapeInstance()
        {
            leftPoint = new IGB283Vector(0, -5, 0),
            rightPoint = new IGB283Vector(0, 5, 0),
            movespeed = 0.5f,
            rotationSpeed = 0.5f,
            time = 0f,
            transformedVertices = new IGB283Vector[baseVertices.Length]
        };
        UpdateMesh();
    }

    void Update()
    {
        foreach (var s in shapes)
        {
            s.time += Time.deltaTime;

            s.t = Mathf.PingPong(s.time * s.movespeed, 1f);

            IGB283Vector pos = new IGB283Vector(
                Mathf.Lerp(s.leftPoint.x, s.rightPoint.x, s.t),
                Mathf.Lerp(s.leftPoint.y, s.rightPoint.y, s.t),
                0f
            );
            float angle = s.time * s.rotationSpeed;

            float scaleValue = Mathf.Lerp(0.5f, 2.0f, s.t);
            IGB283Vector scale = new IGB283Vector(scaleValue, scaleValue, 1f);

            Matrix3x3 trs = IGB283Transform.TRS(pos, angle, scale);

            for (int i = 0; i < baseVertices.Length; i++)
            {
                s.transformedVertices[i] = IGB283Transform.Apply(trs, baseVertices[i]);
            }
        }

        UpdateMesh();
    }

    void UpdateMesh()
    {
        List<Vector3> unityVerts = new List<Vector3>();
        List<int> allTriangles = new List<int>();
        List<Color> allColors = new List<Color>();

        int vertexOffset = 0;
        // de-complexify (goofy marc (๑ᵔ⤙ᵔ๑))
        foreach (var s in shapes)
        {
            for (int i = 0; i < s.transformedVertices.Length; i++)
            {
                unityVerts.Add(s.transformedVertices[i].ToUnityVector3());
            }

            for (int i = 0; i < triangles.Length; i++)
            {
                allTriangles.Add(triangles[i] + vertexOffset);
            }

            Color c = Color.Lerp(Color.black, Color.white, s.t);

            for (int i = 0; i < s.transformedVertices.Length; i++)
            {
                allColors.Add(c);  
            }

            vertexOffset += s.transformedVertices.Length;
        }

        mesh.Clear();
        mesh.vertices = unityVerts.ToArray();
        mesh.triangles = allTriangles.ToArray();
        mesh.colors = allColors.ToArray();
        mesh.RecalculateNormals();
    }
    void CreateShape()
    {
        int segments = 40;

        float startRadius = 3.0f;
        float endRadius = 0.3f;
        float turns = 2.5f;

        float bandWidth = 0.4f;

        List<IGB283Vector> verts = new List<IGB283Vector>();
        List<int> tris = new List<int>();

        for (int i = 0; i < segments; i++)
        {
            float t0 = i / (float)segments;
            float t1 = (i + 1) / (float)segments;

            float angle0 = (Mathf.PI * 2f) * t0 * turns;
            float angle1 = (Mathf.PI * 2f) * t1 * turns;

            float radius0 = Mathf.Lerp(startRadius, endRadius, Mathf.Pow(t0, 0.7f));
            float radius1 = Mathf.Lerp(startRadius, endRadius, Mathf.Pow(t1, 0.7f));

            float innerRadius0 = Mathf.Max(radius0 - bandWidth, 0.1f);
            float innerRadius1 = Mathf.Max(radius1 - bandWidth, 0.1f);

            IGB283Vector outer0 = new IGB283Vector(
                Mathf.Cos(angle0) * radius0,
                Mathf.Sin(angle0) * radius0,
                0f
            );

            IGB283Vector outer1 = new IGB283Vector(
                Mathf.Cos(angle1) * radius1,
                Mathf.Sin(angle1) * radius1,
                0f
            );

            IGB283Vector inner0 = new IGB283Vector(
            Mathf.Cos(angle0) * innerRadius0,
            Mathf.Sin(angle0) * innerRadius0,
            0f
            );

            IGB283Vector inner1 = new IGB283Vector(
                Mathf.Cos(angle1) * innerRadius1,
                Mathf.Sin(angle1) * innerRadius1,
                0f
            );

            int v0 = verts.Count;
            int v1 = verts.Count + 1;
            int v2 = verts.Count + 2;
            int v3 = verts.Count + 3;

            verts.Add(outer0);
            verts.Add(inner0);
            verts.Add(outer1);
            verts.Add(inner1);


            tris.Add(v0); tris.Add(v2); tris.Add(v1);
            tris.Add(v1); tris.Add(v2); tris.Add(v3);

        }

        baseVertices = verts.ToArray();
        triangles = tris.ToArray();
    }


}
