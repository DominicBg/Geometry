using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningTriangle : MonoBehaviour {

    [SerializeField] Transform[] pointsTr;
    //Vector3[] pointsVector3;
    LineRenderer lr;

    Matrix4x4 points;
    Matrix4x4 rotatedPoints;
    Vector4 normalVector;
  
    float angle = 0;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
        //pointsVector3 = new Vector3[pointsTr.Length];

        //for (int i = 0; i < pointsTr.Length; i++)
        //{
        //    pointsVector3[i] = pointsTr[i].localPosition;
        //}
        //  points = new Matrix4x4(pointsVector3);
        points = Matrix4x4Extension.TriangleToMatrixLocal(pointsTr[0], pointsTr[1], pointsTr[2]);
        Debug.Log(points.ToString());
        //for (int i = 0; i < pointsTr.Length; i++)
        //{
        //    pointsVector3[i] = pointsTr[i].localPosition;
        //}

        ////3x3
        //points = new Matrix(pointsVector3);

        ////1x3
        //Vector3 normal = GameMath.GetNormal(pointsTr[0].localPosition, pointsTr[1].localPosition, pointsTr[2].localPosition);
        //normalMatrix = new Matrix(normal);
        //Matrix transposedNormal = Matrix.Transpose(normalMatrix);
        //double[,] normalIdentityArray = {
        //    { normalMatrix [0,0] , 0                  , 0},
        //    { 0                  , normalMatrix [0,1] , 0},
        //    { 0                  , 0                  , normalMatrix [0,2]},
        //};
        //Matrix normalIdentity = new Matrix(normalIdentityArray);

        //Matrix result = Matrix.StupidMultiply(points, no);

        //    Debug.Log("input \n" + points.ToString());
        // Debug.Log("normal \n" + normalIdentity.ToString());
        // Debug.Log("result \n" + result.ToString());
    }

    private void Update()
    {
        CalculateNormal();
        Rotate(angle);

        UpdateLineRenderer();
        angle += Time.deltaTime;
    }

    private void Rotate(float angle)
    {
        Matrix4x4 rotationMatrix = Matrix4x4Extension.RotationXZ(angle);
        //Matrix4x4 localRotationMatrix = points * transform.worldToLocalMatrix.inverse;

        rotatedPoints = points * rotationMatrix;
    }

    private void CalculateNormal()
    {
        normalVector = GameMath.GetNormal(
            rotatedPoints.GetRow(0),
            rotatedPoints.GetRow(1),
            rotatedPoints.GetRow(2));
    }

    private void UpdateLineRenderer()
    {
        Vector3[] positions = new Vector3[6];
        Vector4 middle = Vector4.zero;
        for (int i = 0; i < 4; i++)
        {
            positions[i] = rotatedPoints.GetRow(i);
            middle += rotatedPoints.GetRow(i);
        }
        middle = middle / 3;
        positions[3] = rotatedPoints.GetRow(0);
        positions[4] = middle;
        positions[5] = middle + normalVector;

        lr.positionCount = 6;
        lr.SetPositions(positions);
    }
}
