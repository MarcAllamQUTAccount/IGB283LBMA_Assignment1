using UnityEngine;

public static class IGB283Transform
{
    // Translation matrix
    public static Matrix3x3 Translation(IGB283Vector t)
    {
        Matrix3x3 m = Matrix3x3.identity;

        // 2D homogeneous:
        // [1 0 tx]
        // [0 1 ty]
        // [0 0 1 ]
        m[0, 2] = t.x;
        m[1, 2] = t.y;

        return m;
    }

    // Scaling matrix (uniform or non-uniform)
    public static Matrix3x3 Scaling(IGB283Vector s)
    {
        Matrix3x3 m = Matrix3x3.identity;

        // [sx 0  0]
        // [0  sy 0]
        // [0  0  1]
        m[0, 0] = s.x;
        m[1, 1] = s.y;

        return m;
    }

    // Rotation around Z axis (2D rotation)
    public static Matrix3x3 Rotation(float angleRadians)
    {
        float c = Mathf.Cos(angleRadians);
        float s = Mathf.Sin(angleRadians);

        // [ c -s 0]
        // [ s  c 0]
        // [ 0  0 1]
        Matrix3x3 m = Matrix3x3.identity;
        m[0, 0] = c;
        m[0, 1] = -s;
        m[1, 0] = s;
        m[1, 1] = c;

        return m;
    }

    // Build TRS matrix: T * R * S
    public static Matrix3x3 TRS(IGB283Vector pos, float angleRadians, IGB283Vector scale)
    {
        Matrix3x3 T = Translation(pos);
        Matrix3x3 R = Rotation(angleRadians);
        Matrix3x3 S = Scaling(scale);

        // Order: T * R * S
        return T * R * S;
    }

    // Apply matrix to our custom vector
    public static IGB283Vector Apply(Matrix3x3 m, IGB283Vector v)
    {
        // Convert to Unity Vector3 with z=1 for homogeneous coordinates
        Vector3 p = new Vector3(v.x, v.y, 1f);
        Vector3 result = m.MultiplyPoint(p); // uses your Matrix3x3

        return new IGB283Vector(result.x, result.y, v.z);
    }
}