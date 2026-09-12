using UnityEngine;

[ExecuteAlways]
public class MeshDebug : MonoBehaviour
{
    public AssignmentShape shape;

    void OnDrawGizmos()
    {
        if (shape == null || shape.baseVertices == null || shape.triangles == null)
            return;

        // Draw vertices
        for (int i = 0; i < shape.baseVertices.Length; i++)
        {
            Vector3 pos = shape.baseVertices[i].ToUnityVector3();
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(pos, 0.05f);

#if UNITY_EDITOR
            UnityEditor.Handles.Label(pos + Vector3.up * 0.1f,
                $"{i}  ({pos.x:F2}, {pos.y:F2})");
#endif
        }

        // Draw triangles
        Gizmos.color = Color.cyan;
        for (int i = 0; i < shape.triangles.Length; i += 3)
        {
            int a = shape.triangles[i];
            int b = shape.triangles[i + 1];
            int c = shape.triangles[i + 2];

            Vector3 A = shape.baseVertices[a].ToUnityVector3();
            Vector3 B = shape.baseVertices[b].ToUnityVector3();
            Vector3 C = shape.baseVertices[c].ToUnityVector3();

            Gizmos.DrawLine(A, B);
            Gizmos.DrawLine(B, C);
            Gizmos.DrawLine(C, A);

#if UNITY_EDITOR
            Vector3 center = (A + B + C) / 3f;
            UnityEditor.Handles.Label(center, $"Tri {i / 3}");
#endif
        }
    }
}
