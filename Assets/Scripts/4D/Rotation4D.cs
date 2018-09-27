using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Rotation4D : MonoBehaviour
{
    [SerializeField] protected float scaleByW;
    [SerializeField] protected float size;
    [SerializeField] protected float rotationSpeed = 1;

    [SerializeField] AnimationCurve angleCurve;
    [SerializeField] Matrix4DRotation rotationMatrix;

    protected Matrix verticesMatrixPosition;
    public Vector3[] verticesPositionsAfterRotation { get; protected set; }

    [SerializeField] bool animateOverTime;

    public float angleOffset = 0;
    public float timer = 0;
    protected float angle = 0;
    float TWO_PI = 6.28318530718f;

    [ContextMenu("Recalculate")]
    protected abstract void Calculate();

    protected void Rotate()
    {
        float realAngle = angle + (angleOffset * Mathf.Deg2Rad);
        Matrix matrixNewPosition = ApplyRotations(verticesMatrixPosition, realAngle);
        UpdatePosition(matrixNewPosition);
            
        AnimateAngle();
    }

    private void AnimateAngle()
    {
        if(animateOverTime)
            timer += Time.deltaTime * rotationSpeed;

        angle = (angleCurve.Evaluate(timer / TWO_PI) * TWO_PI);
        if (timer > TWO_PI)
            timer -= TWO_PI;
    }

    Matrix ApplyRotations(Matrix positionMatrix, float angle)
    {
        Matrix matrixNewPosition = verticesMatrixPosition;
        if (rotationMatrix.rotateXY)
            matrixNewPosition = Matrix.StupidMultiply(matrixNewPosition, Matrix4DRotation.RotationXY(angle * rotationMatrix.speedXY));
        if (rotationMatrix.rotateXZ)
            matrixNewPosition = Matrix.StupidMultiply(matrixNewPosition, Matrix4DRotation.RotationXZ(angle * rotationMatrix.speedXZ));
        if (rotationMatrix.rotateXW)
            matrixNewPosition = Matrix.StupidMultiply(matrixNewPosition, Matrix4DRotation.RotationXW(angle * rotationMatrix.speedXW));

        if (rotationMatrix.rotateYZ)
            matrixNewPosition = Matrix.StupidMultiply(matrixNewPosition, Matrix4DRotation.RotationYZ(angle * rotationMatrix.speedYZ));
        if (rotationMatrix.rotateYW)
            matrixNewPosition = Matrix.StupidMultiply(matrixNewPosition, Matrix4DRotation.RotationYW(angle * rotationMatrix.speedYW));

        if (rotationMatrix.rotateZW)
            matrixNewPosition = Matrix.StupidMultiply(matrixNewPosition, Matrix4DRotation.RotationZW(angle * rotationMatrix.speedZW));

       
        return matrixNewPosition;
    }

    void UpdatePosition(Matrix matrixPosition)
    {
        if (verticesPositionsAfterRotation == null)
            verticesPositionsAfterRotation = new Vector3[matrixPosition.rows];

        for (int i = 0; i < matrixPosition.rows; i++)
        {
            float w = 1 / (scaleByW - (float)matrixPosition[i, 3]);

            Vector3 position = new Vector3(
                (float)matrixPosition[i, 0],
                (float)matrixPosition[i, 1],
                (float)matrixPosition[i, 2]);

            verticesPositionsAfterRotation[i] = position * w * size;
        }
    }
}



[System.Serializable]
public class Matrix4DRotation
{
    public bool rotateXY;
    public bool rotateXZ;
    public bool rotateXW; 
    public bool rotateYZ;
    public bool rotateYW;
    public bool rotateZW;

    public float speedXY = 1;
    public float speedXZ = 1;
    public float speedXW = 1;
    public float speedYZ = 1;
    public float speedYW = 1;
    public float speedZW = 1;

    public static Matrix RandomXd(float angle)
    {
        double cos = Mathf.Cos(angle);
        double sin = Mathf.Sin(angle);
        return new Matrix
        (
            new double[,]
            {
                    { sin, -cos, 0, 0 },
                    { cos,  cos,  0, 0 },
                    { 0  , sin  ,  1, 0 },
                    { 0  , 0   ,  0, sin }
            }
        );
    }

    public static Matrix RotationXY(float angle)
    {
        double cos = Mathf.Cos(angle);
        double sin = Mathf.Sin(angle);
        return new Matrix
        (
            new double[,]
            {
                    { cos, -sin, 0, 0 },
                    { sin,  cos,  0, 0 },
                    { 0  , 0   ,  1, 0 },
                    { 0  , 0   ,  0, 1 }
            }
        );
    }

    public static Matrix RotationXZ(float angle)
    {
        double cos = Mathf.Cos(angle);
        double sin = Mathf.Sin(angle);
        return new Matrix
        (
            new double[,]
            {
                    { cos, 0, -sin, 0 },
                    { 0  , 1,  0  , 0 },
                    { sin, 0,  cos, 0 },
                    { 0  , 0,  0  , 1 }
            }
        );
    }

    public static Matrix RotationXW(float angle)
    {
        double cos = Mathf.Cos(angle);
        double sin = Mathf.Sin(angle);
        return new Matrix
        (
            new double[,]
            {
                    { cos, 0, 0, -sin },
                    { 0  , 1, 0, 0 },
                    { 0  , 0, 1, 0 },
                    { sin, 0, 0, cos}
            }
        );
    }

    public static Matrix RotationYZ(float angle)
    {
        double cos = Mathf.Cos(angle);
        double sin = Mathf.Sin(angle);
        return new Matrix
        (
            new double[,]
            {
                    { 1, 0   ,    0, 0 },
                    { 0, cos , -sin, 0 },
                    { 0, sin ,  cos, 0 },
                    { 0, 0   ,    0, 1 }
            }
        );
    }

    public static Matrix RotationYW(float angle)
    {
        double cos = Mathf.Cos(angle);
        double sin = Mathf.Sin(angle);
        return new Matrix
        (
            new double[,]
            {
                    { 1, 0   ,0, 0 },
                    { 0, cos , 0, -sin },
                    { 0, 0 ,  1, 0 },
                    { 0, sin,0, cos }
            }
        );
    }

    public static Matrix RotationZW(float angle)
    {
        double cos = Mathf.Cos(angle);
        double sin = Mathf.Sin(angle);
        return new Matrix
        (
            new double[,]
            {
                    { 1, 0, 0  , 0 },
                    { 0, 1, 0  , 0 },
                    { 0, 0, cos, -sin },
                    { 0, 0, sin, cos}
            }
        );
    }
}
