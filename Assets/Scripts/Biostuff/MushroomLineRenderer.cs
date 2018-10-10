using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class MushroomLineRenderer : MonoBehaviour {

    [SerializeField] LinearAnimationMinMax heigthAnimation;
    [SerializeField] SinAnimationMinMax imperfectionAnimation;
    [SerializeField] AnimationCurve shroomCurve;
    [SerializeField] float radius;
    [SerializeField] int resolution = 100;
    [SerializeField] float phiSpeed;
    [SerializeField] float radiusCurveMultiplier;
    [SerializeField] bool recording = true;
    [SerializeField] float precalculationRatio = 0.02f;
    float theta;

    LineRenderer lineRenderer;
    LinkedList<Vector3> trailPosition = new LinkedList<Vector3>();
    Vector3[] positions;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!recording)
            return;

        GenerateTrail();
    }

    [ContextMenu("Precalculate")]
    public void Precalculate()
    {
        recording = false;
        positions = new Vector3[resolution];
        float timer = 0;

        for (int i = 0; i < resolution; i++)
        {
            positions[i] = CalculateAnimationPosition(timer);
            timer += precalculationRatio;
        }
        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
    }

    void GenerateTrail()
    {
        while(trailPosition.Count > resolution)
        {
            trailPosition.RemoveFirst();
        }

        trailPosition.AddLast(CalculatePosition());

        positions = new Vector3[trailPosition.Count];
        int i = 0;
        foreach(Vector3 vector3 in trailPosition)
        {
            positions[i++] = vector3;
        }
        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
    }

    Vector3 CalculateAnimationPosition(float timer)
    {
        float y = heigthAnimation.CalculateMinMax(timer);

        float radiusCurve = radiusCurveMultiplier * shroomCurve.Evaluate(heigthAnimation.currentT);
        float imperfection = imperfectionAnimation.CalculateMinMax(timer);

        return Polar3DPosition(radius + radiusCurve + imperfection, timer * phiSpeed, y);
    }

    Vector3 CalculatePosition()
    {
        float y = heigthAnimation.CalculateMinMax();
        theta += Time.deltaTime * phiSpeed;

        float radiusCurve = radiusCurveMultiplier * shroomCurve.Evaluate(heigthAnimation.currentT);
        float imperfection = imperfectionAnimation.CalculateMinMax();
        return Polar3DPosition(radius + radiusCurve + imperfection, theta, y);
    }

    Vector3 Polar3DPosition(float radius, float theta, float y)
    {
        Vector2 coordinate = GameMath.PolarToCartesian(radius, theta);

        return new Vector3(coordinate.x, y, coordinate.y);
    }
}
