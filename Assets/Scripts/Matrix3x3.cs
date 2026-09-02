using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matrix3x3
{
    private const int matrixOrder = 3;

    private List<IGB283Vector> m = new List<IGB283Vector>();
    public Matrix3x3()
    {
        m.Add(new IGB283Vector(0, 0, 0));
        m.Add(new IGB283Vector(0, 0, 0));
        m.Add(new IGB283Vector(0, 0, 0));
    }

    public Matrix3x3(IGB283Vector r1, IGB283Vector r2, IGB283Vector r3)
    {
        m.Add(r1);
        m.Add(r2);
        m.Add(r3);
    }

    public static Matrix3x3 identity
    {
        get
        {
            return new Matrix3x3(
                new IGB283Vector(1, 0, 0),
                new IGB283Vector(0, 1, 0),
                new IGB283Vector(0, 0, 1)
            );
        }
    }

    public IGB283Vector GetRow(int row)
    {
        return m[row];
    }

    public IGB283Vector GetColumn(int col)
    {
        return new IGB283Vector(
            m[0][col],
            m[1][col],
            m[2][col]
        );
    }

    public void SetRow(int row, IGB283Vector v)
    {
        m[row] = v;
    }
    public void SetColumn(int col, IGB283Vector v)
    {
        IGB283Vector r0 = m[0];
        IGB283Vector r1 = m[1];
        IGB283Vector r2 = m[2];

        r0[col] = v.x;
        r1[col] = v.y;
        r2[col] = v.z;

        m[0] = r0;
        m[1] = r1;
        m[2] = r2;
    }

    public float this[int row, int col]
    {
        get
        {
            return m[row][col];
        }
        set
        {
            IGB283Vector r = m[row];
            r[col] = value;
            m[row] = r;
        }
    }

    public Matrix3x3 transpose
    {
        get
        {
            return new Matrix3x3(
                GetColumn(0),
                GetColumn(1),
                GetColumn(2)
            );
        }
    }

    public IGB283Vector MultiplyPoint(IGB283Vector p)
    {
        float x = m[0].x * p.x + m[0].y * p.y + m[0].z * 1f;
        float y = m[1].x * p.x + m[1].y * p.y + m[1].z * 1f;
        float z = m[2].x * p.x + m[2].y * p.y + m[2].z * 1f;

        return new IGB283Vector(x, y, z);
    }

    public static Matrix3x3 operator *(Matrix3x3 a, Matrix3x3 b)
    {
        Matrix3x3 result = new Matrix3x3();

        for (int i = 0; i < 3; i++)
        {
            IGB283Vector row = a.GetRow(i);

            float x = row.x * b[0, 0] + row.y * b[1, 0] + row.z * b[2, 0];
            float y = row.x * b[0, 1] + row.y * b[1, 1] + row.z * b[2, 1];
            float z = row.x * b[0, 2] + row.y * b[1, 2] + row.z * b[2, 2];

            result.SetRow(i, new IGB283Vector(x, y, z));
        }

        return result;
    }

    public static Matrix3x3 operator *(float s, Matrix3x3 m)
    {
        Matrix3x3 result = new Matrix3x3();

        for (int i = 0; i < 3; i++)
        {
            IGB283Vector row = m.GetRow(i);
            result.SetRow(i, new IGB283Vector(row.x * s, row.y * s, row.z * s));
        }
        return result;
    }
}