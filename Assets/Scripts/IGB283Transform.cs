using UnityEngine;

public static class IGB283Transform
{
    // Translation matrix
    public static Matrix3x3 Translation(IGB283Vector pos)
    {
        return new Matrix3x3(
            new IGB283Vector(1, 0, pos.x),
            new IGB283Vector(0, 1, pos.y),
            new IGB283Vector(0, 0, 1)
            );
    }

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

    public static Matrix3x3 Scale(IGB283Vector s)
    {
        return new Matrix3x3(
            new IGB283Vector(s.x, 0, 0),
            new IGB283Vector(0, s.y, 0),
            new IGB283Vector(0, 0, 1)
        );
    }
    public static Matrix3x3 TRS(IGB283Vector pos, float angleRadians, IGB283Vector scale)
    {
        Matrix3x3 T = Translation(pos);
        Matrix3x3 R = Rotation(angleRadians);
        Matrix3x3 S = Scale(scale);

        return T * R * S;
    }

    public static IGB283Vector Apply(Matrix3x3 m, IGB283Vector v)
    {
        return m.MultiplyPoint(v);
    }
}