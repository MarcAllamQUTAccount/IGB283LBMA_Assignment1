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

    public float this[int index]
    {
        get
        {
            if (index == 0) return x;
            if (index == 1) return y;
            if (index == 2) return z;
            throw new System.IndexOutOfRangeException();
        }
        set
        {
            if (index == 0) x = value;
            else if (index == 1) y = value;
            else if (index == 2) z = value;
            else throw new System.IndexOutOfRangeException();
        }
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
}
