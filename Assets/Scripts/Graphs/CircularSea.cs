using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularSea : MonoBehaviour {

    LineRenderer lineRenderer;

    [Header("Settings")]
    [SerializeField] float radius;
    [SerializeField] int resolution;
    [SerializeField] int numberOfLoop;
    [SerializeField] bool halfSphere;

    [Header("Theta animation")]
    [SerializeField] float thetaSinFreq;
    [SerializeField] float thetaSinMin;
    [SerializeField] float thetaSinMax;

    [Header("Sin Animation")]
    [SerializeField] float sinAmplitude;
    [SerializeField] float sinFrequency;
    [SerializeField] float subSinAmplitude;
    [SerializeField] float subsinFrequency;

    [SerializeField] float diffVectorSin;
    [SerializeField] Vector3 directionSin;
    [SerializeField] float intervalX;

    Vector3[] circularInitialPoints;
    Vector3[] circularPoints; 
    
    float twoPi = Mathf.PI * 2;
    float halfPi = Mathf.PI * 0.5f;
    float thetaOffset;

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

    private void OnValidate()
    {
        //if(lineRenderer != null)
        //    GenerateCircularPoints();
    }

    // Update is called once per frame
    void Update ()
    {
        UpdateThetaOffset();
        GenerateCircularPoints();
        UpdateLinePointPositions();
        UpdateLineRenderer();
    }

    void UpdateThetaOffset()
    {
        float t = (Mathf.Sin(Time.time * thetaSinFreq) + 1) * 0.5f;
        thetaOffset = Mathf.Lerp(thetaSinMin, thetaSinMax, t);
    }

    void UpdateLinePointPositions()
    {
        for (int i = 0; i < circularInitialPoints.Length; i++)
        {
            float t = (float)(i) / circularInitialPoints.Length;

            Vector3 diffvector = (circularInitialPoints[i] - gameObject.transform.position).normalized * diffVectorSin;
            circularPoints[i] = circularInitialPoints[i] + (directionSin + diffvector) * CalculateVertexPosition(t * intervalX);
        }
        //circularPoints[circularInitialPoints.Length] = circularPoints[0];
    }

    float CalculateVertexPosition(float x)
    {
        return sinAmplitude* Mathf.Sin(Time.time + x * sinFrequency) + subSinAmplitude * Mathf.Sin(Time.time + x * subsinFrequency);
        //return 0.257f * sinAmplitude * Mathf.Sin(Time.time + x * sinFrequency * 0.153f) -
        //       0.103f * sinAmplitude * Mathf.Cos(Time.time + x * sinFrequency * 0.269f) +
        //       0.951f * sinAmplitude * Mathf.Sin(Time.time + x * sinFrequency * 0.155f) -
        //       0.887f * sinAmplitude * Mathf.Cos(Time.time + x * sinFrequency * 0.707f);
    }
}
