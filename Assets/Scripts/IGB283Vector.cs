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

    // Convert to Unity Vector3 (for mesh)
    public Vector3 ToUnityVector3()
    {
        return new Vector3(x, y, z);
    }
}
