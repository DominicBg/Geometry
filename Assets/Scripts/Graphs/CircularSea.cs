using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularSea : MonoBehaviour {

    LineRenderer lineRenderer;

    [Header("Settings")]
    [SerializeField] protected float radius;
    [SerializeField] protected int resolution;
    [SerializeField] protected int numberOfLoop;
    [SerializeField] protected bool halfSphere;

    [Header("Theta animation")]
    [SerializeField] protected float thetaOffset;

    [Header("Sin Animation")]
    [SerializeField] protected float sinAmplitude;
    [SerializeField] protected float sinFrequency;
    [SerializeField] protected float subSinAmplitude;
    [SerializeField] protected float subsinFrequency;

    [SerializeField] protected float diffVectorSin;
    [SerializeField] protected Vector3 directionSin;
    [SerializeField] protected float intervalX;
    [SerializeField] float speed;

    Vector3[] circularInitialPoints;
    Vector3[] circularPoints; 
    
    float twoPi = Mathf.PI * 2;
    float halfPi = Mathf.PI * 0.5f;

    void Start ()
    {
        lineRenderer = GetComponent<LineRenderer>();
        GenerateCircularPoints();
    }

    void GenerateCircularPoints()
    {
        circularInitialPoints = new Vector3[resolution * numberOfLoop];
        circularPoints = new Vector3[resolution * numberOfLoop];

        for (int i = 0; i < resolution * numberOfLoop; i++)
        {
            float t1 = (float)(i % resolution) / resolution;
            float t2 = (float)i / (resolution * numberOfLoop);

            float phi = Mathf.Lerp(0, twoPi, t1); //Rotation Around Y
            float theta = Mathf.Lerp(0, (halfSphere) ? halfPi : Mathf.PI, t2);

            //Theta + offset = sexy
            Vector3 position = GameMath.SphericalRotation(radius, theta + thetaOffset, phi);
            circularInitialPoints[i] = position;
        }
    }
	
    void UpdateLineRenderer()
    {
        lineRenderer.positionCount = circularPoints.Length;
        lineRenderer.SetPositions(circularPoints);
    }

    protected void Update ()
    {
        GenerateCircularPoints();
        UpdateLinePointPositions();
        UpdateLineRenderer();
    }

    void UpdateLinePointPositions()
    {
        for (int i = 0; i < circularInitialPoints.Length; i++)
        {
            float t = (float)(i) / circularInitialPoints.Length;

            Vector3 diffvector = (circularInitialPoints[i] - gameObject.transform.position).normalized * diffVectorSin;
            circularPoints[i] = circularInitialPoints[i] + (directionSin + diffvector) * CalculateVertexPosition(t * intervalX);
        }
    }

    float CalculateVertexPosition(float x)
    {
        return sinAmplitude* Mathf.Sin(Time.time * speed + x * sinFrequency) + subSinAmplitude * Mathf.Sin(Time.time * speed + x * subsinFrequency);
    }
}
