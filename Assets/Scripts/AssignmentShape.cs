using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class AssignmentShape : MonoBehaviour
{
    [SerializeField] private Material material;

    private Mesh mesh;
    private IGB283Vector[] baseVertices;
    private int[] triangles;
    private float boxLeftPosX;
    private float boxLeftPosY;
    private float boxRightPosX;
    private float boxUpPosY;
    private float boxDownPosY;
    private float moveSpeedVar = 0.5f;

    [System.Serializable]
    public class ShapeInstance
    {
        public IGB283Vector leftPoint;
        public IGB283Vector rightPoint;
        public float movespeed;
        public float rotationSpeed;

        public float time;
        public float t;
        public float phase;

        public IGB283Vector[] transformedVertices;

        public Color color;
    }

    public ShapeInstance[] shapes;
    
    void Start()
    {

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().material = material;

        CreateShape();
        // getting boundary left and right
        boxLeftPosX = GameObject.Find("BoundaryLeft").transform.position.x;
        boxLeftPosY = GameObject.Find("BoundaryLeft").transform.position.y;
        boxRightPosX = GameObject.Find("BoundaryRight").transform.position.x;

        shapes = new ShapeInstance[2];

        shapes[0] = new ShapeInstance()
        {
            leftPoint = new IGB283Vector(boxLeftPosX, boxLeftPosY, 0),
            rightPoint = new IGB283Vector(boxRightPosX, boxLeftPosY, 0),
            movespeed = 0.5f,
            rotationSpeed = 0.5f,
            time = 0f,
            transformedVertices = new IGB283Vector[baseVertices.Length]
        };
        // getting boundary up and down
        boxUpPosY = GameObject.Find("BoundaryUp").transform.position.y;
        boxDownPosY = GameObject.Find("BoundaryDown").transform.position.y;
        shapes[1] = new ShapeInstance()
        {
            leftPoint = new IGB283Vector(0, boxDownPosY, 0),
            rightPoint = new IGB283Vector(0, boxUpPosY, 0),
            movespeed = 0.75f,
            rotationSpeed = 0.75f,
            time = 0f,
            transformedVertices = new IGB283Vector[baseVertices.Length]
        };
        UpdateMesh();
    }

    void Update()
    {
        boxUpPosY = GameObject.Find("BoundaryUp").transform.position.y;
        boxDownPosY = GameObject.Find("BoundaryDown").transform.position.y;
        boxLeftPosX = GameObject.Find("BoundaryLeft").transform.position.x;
        boxLeftPosY = GameObject.Find("BoundaryLeft").transform.position.y;
        boxRightPosX = GameObject.Find("BoundaryRight").transform.position.x;

        // Update the left and right points of the shapes based on the boundary positions
        shapes[0].leftPoint = new IGB283Vector(boxLeftPosX, boxLeftPosY, 0);
        shapes[0].rightPoint = new IGB283Vector(boxRightPosX, boxLeftPosY, 0);
        shapes[1].leftPoint = new IGB283Vector(0, boxDownPosY, 0);
        shapes[1].rightPoint = new IGB283Vector(0, boxUpPosY, 0);

        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            moveSpeedVar += 0.1f;
        }
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            moveSpeedVar -= 0.1f;
        }
        foreach (var s in shapes)
        {
            s.time += Time.deltaTime;

            float currentSpeed = s.movespeed * moveSpeedVar;
            s.phase += Time.deltaTime * currentSpeed;
            s.t = Mathf.PingPong(s.phase, 1f);

            s.color = Color.Lerp(Color.black, Color.white, s.t);

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
        int totalVerts = 0;
        foreach (var s in shapes)
        {
            totalVerts += s.transformedVertices.Length;
        }
        Vector3[] unityVerts = new Vector3[totalVerts];
        int[] allTris = new int[triangles.Length * shapes.Length];
        Color[] allColors = new Color[totalVerts];

        int vOffset = 0;
        int tOffset = 0;

        for (int s = 0; s < shapes.Length; s++)
        {
            for (int i = 0; i < shapes[s].transformedVertices.Length; i++)
            {
                unityVerts[vOffset + i] = shapes[s].transformedVertices[i].ToUnityVector3();
                allColors[vOffset + i] = shapes[s].color;
            }

            for (int i = 0; i < triangles.Length; i++)
            {
                allTris[tOffset + i] = triangles[i] + vOffset;
            }

            vOffset += shapes[s].transformedVertices.Length;
            tOffset += triangles.Length;
        }

        mesh.Clear();
        mesh.vertices = unityVerts;
        mesh.triangles = allTris;
        mesh.colors = allColors;
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
