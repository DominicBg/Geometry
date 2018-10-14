using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moon : MonoBehaviour {

    [SerializeField] int resolution;
    [SerializeField] int depthResolution;
    [SerializeField] CurvedFloat depthOffset;

    [SerializeField] float radius;
    [SerializeField, Range(0,1)] float ratioInnerCurve;
    [SerializeField] AnimationCurve moonCurve;
    [SerializeField] bool useCurveSymmetry;
    [SerializeField] CurvedFloat fullMoonAnimation;

    [Header("rotation")]
    [SerializeField] float degree;
    [SerializeField] CurvedFloat rotationCurve;
    [SerializeField] float offsetGradient;

    LineRenderer lineRenderer;
    Vector3[] positions;

	// Use this for initialization
	void Start ()
    {
        lineRenderer = GetComponent<LineRenderer>();
        Generate();
    }

    private void OnValidate()
    {
        lineRenderer = GetComponent<LineRenderer>();
        Generate();
    }

    // Update is called once per frame
    void Update ()
    {
        //lineRenderer = GetComponent<LineRenderer>();

        Generate();
    }

    void Generate()
    {
        positions = new Vector3[resolution * 2 * depthResolution];

        for (int i = 0; i < depthResolution; i++)
        {
            CalculateOutterCurve(i * resolution * 2, i);
            CalculateInnerCurve(i * resolution * 2, i);
        }

        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
    }

    void CalculateOutterCurve(int startIndex, int depthIndex)
    {
        //Calcule d'en bas jusqu'en haut
        for (int i = 0; i < resolution; i++)
        {
            int realIndex = i + startIndex;

            float t = (float)i / (resolution-1);
            float angle = t * degree;

            positions[realIndex] = GameMath.RotateVectorZ(angle, Vector3.up * radius);

            //offset animation
            positions[realIndex] = GameMath.RotateVectorY(depthIndex * depthOffset, positions[realIndex]);

            //rotation formula
            positions[realIndex] = RotateY(positions[realIndex], i);
        }
    }

    void CalculateInnerCurve(int startIndex, int depthIndex)
    {
        for (int i = 0; i < resolution; i++)
        {
            int realIndex = i + resolution + startIndex;
            //lit le i inverse par rapport a resolution
            float t = (float)(resolution-1-i) / (resolution-1);
            float angle = t * degree;

            float curveEvaluation = 1 - ((useCurveSymmetry) ? moonCurve.SymmetricEvaluate(t) : moonCurve.Evaluate(t));
            float currentRadius = Mathf.Lerp(radius * ratioInnerCurve, radius, curveEvaluation);

            float fullMoonAnimationRadius = Mathf.Lerp(currentRadius, radius, fullMoonAnimation);

            Vector3 position = GameMath.RotateVectorZ(angle, Vector3.up * currentRadius);
            Vector3 fullMoonPosition = GameMath.RotateVectorZ(-angle, Vector3.up * radius);

            Vector3 realPosition = Vector3.Lerp(position, fullMoonPosition, fullMoonAnimation);
            positions[realIndex] = realPosition;

            //offset animation
            positions[realIndex] = GameMath.RotateVectorY(depthIndex * depthOffset, positions[realIndex]);

            //rotation formula
            positions[realIndex] = RotateY(positions[realIndex], resolution - i);
        }
    }

    Vector3 RotateY(Vector3 vector, int i)
    {
       return GameMath.RotateVectorY(rotationCurve.CalculateMinMax(Time.time + rotationCurve.offset + i * offsetGradient), vector);
       // return GameMath.RotateVectorY(rotationCurve.max * Mathf.Sin(Time.time * rotationSpeed + i * rotationOffset), vector);
    }
}
