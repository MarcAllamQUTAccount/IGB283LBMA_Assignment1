using UnityEngine;

public struct IGB283Vector
{
    public float x;
    public float y;
    public float z;

    public IGB283Vector(float x, float y, float z = 0f)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    // Addition
    public static IGB283Vector operator +(IGB283Vector a, IGB283Vector b)
    {
        return new IGB283Vector(a.x + b.x, a.y + b.y, a.z + b.z);
    }

    // Subtraction
    public static IGB283Vector operator -(IGB283Vector a, IGB283Vector b)
    {
        return new IGB283Vector(a.x - b.x, a.y - b.y, a.z - b.z);
    }

    // Negation
    public static IGB283Vector operator -(IGB283Vector a)
    {
        return new IGB283Vector(-a.x, -a.y, -a.z);
    }

    // Dot product
    public static float Dot(IGB283Vector a, IGB283Vector b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z;
    }

    // Cross product (for 3D, but we’ll mostly use z as 0)
    public static IGB283Vector Cross(IGB283Vector a, IGB283Vector b)
    {
        return new IGB283Vector(
            a.y * b.z - a.z * b.y,
            a.z * b.x - a.x * b.z,
            a.x * b.y - a.y * b.x
        );
    }

    // Convert to Unity Vector3 (for mesh)
    public Vector3 ToUnityVector3()
    {
        return new Vector3(x, y, z);
    }

    // Create from Unity Vector3 (if needed)
    public static IGB283Vector FromUnityVector3(Vector3 v)
    {
        return new IGB283Vector(v.x, v.y, v.z);
    }
}
