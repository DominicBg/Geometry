using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Matrix4x4Extension
{
    public static Matrix4x4 TriangleToMatrixGlobal(Transform tr1, Transform tr2, Transform tr3)
    {
        return TriangleToMatrix(tr1.position, tr2.position, tr3.position);
    }

    public static Matrix4x4 TriangleToMatrixLocal(Transform tr1, Transform tr2, Transform tr3)
    {
        return TriangleToMatrix(tr1.localPosition, tr2.localPosition, tr3.localPosition);
    }

    public static Matrix4x4 TriangleToMatrix(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        return new Matrix4x4(p1, p2, p3, Vector4.zero).transpose;
    }

    public static Matrix4x4 FloatArrayToMatrix(float[,] floats)
    {
        Matrix4x4 matrix = new Matrix4x4();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                matrix[i, j] = floats[i, j];
            }
        }
        return matrix;
    }

    public static Matrix4x4 RotationXY(float angle)
    {
        float cos = Mathf.Cos(angle);
        float sin = Mathf.Sin(angle);
        return FloatArrayToMatrix
        (
            new float[,]
            {
                    { cos, -sin,  0, 0 },
                    { sin,  cos,  0, 0 },
                    { 0  , 0   ,  1, 0 },
                    { 0  , 0   ,  0, 1 }
            }
        );
    }

    public static Matrix4x4 RotationXZ(float angle)
    {
        float cos = Mathf.Cos(angle);
        float sin = Mathf.Sin(angle);
        return FloatArrayToMatrix
        (
            new float[,]
            {
                    { cos, 0   , -sin, 0 },
                    { 0  , 1   , 0   , 0 },
                    { sin, 0   , cos , 0 },
                    { 0  , 0   ,  0  , 1 }
            }
        );
    }

    public static string ToString(this Matrix4x4 matrix)
    {
        string output = "";
        for (int i = 0; i < 4; i++)
        {
            output += matrix.GetRow(i) + "\n";
        }
        return output;
    }
}

