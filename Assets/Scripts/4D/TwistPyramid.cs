using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwistPyramid : MonoBehaviour {

    LineRenderer[] lineRenderers;

    Matrix pointMatrix;
    Matrix rotatedMatrix;

    [Header("Animation")]
    [SerializeField] bool animateAngle;
    [SerializeField] float angle = 0;
    [SerializeField] bool useSin;

    [Header("coucou ma amie")]
    [SerializeField] float speed = 5;
    [SerializeField] float sinAmplitude; 
    [SerializeField] int resolution = 5;
    [SerializeField] float angleOffset = 5;
    [SerializeField] AnimationCurve twistCurve;

    private void Start()
    {
        lineRenderers = GetComponentsInChildren<LineRenderer>();
        InitMatrix();
    }

    void InitMatrix()
    {
        double[,] points = {
            { .5f, 1, -.5f},
            { .5f, 1, .5f},
            { -.5f, 1, .5f},
            { -.5f, 1, -.5f},
            { 0, 0, 0},
        };

        pointMatrix = new Matrix(points);
    }

    private void Update()
    {
        if(animateAngle)
            angle = Time.time * speed;

        float sinAngle = sinAmplitude * Mathf.Sin(angle + angleOffset);
        RotateTopVertices((useSin) ? sinAngle : angle + angleOffset);
        UpdateLineRenderer();
    }

    void RotateTopVertices(float angle)
    {
        //5x3 X 3x3 = 5x3 
        rotatedMatrix = Matrix.StupidMultiply(pointMatrix, RotationXZ(angle));
    }

    void UpdateLineRenderer()
    {
        for (int i = 0; i < 4; i++)
        {
            lineRenderers[i].positionCount = resolution + 1;
            for (int j = 0; j <= resolution; j++)
            {
                float t = (1 / (float)resolution) * j;
                Vector3 startPoint = Vector3.Lerp(Vector3.zero, pointMatrix.GetRowVector3(i), t);
                Vector3 endPoint   = Vector3.Lerp(Vector3.zero, rotatedMatrix.GetRowVector3(i), t);

                Vector3 lerpPosition = Vector3.Lerp(startPoint, endPoint, twistCurve.Evaluate(t));

                lineRenderers[i].SetPosition(j, lerpPosition);
            }
        }
        
        lineRenderers[4].positionCount = 5;
        for (int i = 0; i < 4; i++)
        {
            lineRenderers[4].SetPosition(i, rotatedMatrix.GetRowVector3(i));
        }
        lineRenderers[4].SetPosition(4, rotatedMatrix.GetRowVector3(0));
    }

    public static Matrix RotationXZ(float angle)
    {
        float cos = Mathf.Cos(angle);
        float sin = Mathf.Sin(angle);
        return new Matrix
        (
            new double[,]
            {
                    { cos, 0   , -sin},
                    { 0  , 1   , 0  },
                    { sin, 0   , cos},
            }
        );
    }

    public void UpdateAngle(float newAngle)
    {
        angle = newAngle * speed;
    }

    public void UpdateOffset(float newOffset)
    {
        angleOffset = newOffset;
    }
    public void UpdateColor(Color color)
    {
        for (int i = 0; i < lineRenderers.Length; i++)
        {
            lineRenderers[i].startColor = color;
            lineRenderers[i].endColor = color;
        }
    }
}
