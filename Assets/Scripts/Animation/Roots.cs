using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(TrailRenderer))]
[RequireComponent(typeof(LineRenderer))]
public class Roots : MonoBehaviour {

    [SerializeField] float radius = 10;

    [Header("Rotation")]
    [SerializeField] float thetaOffset = 90;
    [SerializeField] float phiOffset = 0;
    [SerializeField] float thetaRatio = 5;
    [SerializeField] float phiRatio = 1;

    [Header("Noise")]
    [SerializeField] float noiseAmmount = 1;
    [SerializeField] float noiseSpeed = 1;
    [SerializeField] float shakyshaky = 1;

    [Header("Time")]
    [SerializeField] float speed;
    [SerializeField] float intervalTimeBetweenPoints;
    [SerializeField] float maxTimer;

    float intervalTime;
    float timer;

    List<Vector3> positions = new List<Vector3>();
    LineRenderer lineRenderer;
    TrailRenderer trail;
    Vector3 lastPos;

    [ContextMenu("Reset Timer")]
    public void ResetTimer()
    {
        timer = 0;
        //trail.SetPositions(new Vector3[0]);
        positions = new List<Vector3>();
        //trail.enabled = false;
    }


    private void Start()
    {
        trail = GetComponent<TrailRenderer>();
        lineRenderer = GetComponent<LineRenderer>();
        trail.enabled = false;
    }

    private void Update()
    {
        if (timer > maxTimer)
            return;

        timer += Time.deltaTime * speed;
        intervalTime += Time.deltaTime * speed;

        if (intervalTime >= intervalTimeBetweenPoints)
        {
            intervalTime -= intervalTimeBetweenPoints;
            AddPosition();
            UpdateLineRenderer();
        }
    }

    private void AddPosition()
    {
        lastPos = transform.localPosition;

        Vector3 newPosition = CalculatePositionSpherical();
        //transform.localPosition = newPosition;
        positions.Add(newPosition);
        //trail.enabled = true;
    }

    private Vector3 CalculatePositionSpherical()
    {
        float realPhiOffset = phiOffset * Mathf.Deg2Rad;
        float realThetaOffset = thetaOffset * Mathf.Deg2Rad;

        Vector3 newPosition = GameMath.SphericalRotation(radius, timer * thetaRatio + realThetaOffset, timer * phiRatio + realPhiOffset);
        newPosition = CalculateNoisePosition(newPosition);
        return newPosition;
    }

    Vector3 CalculateNoisePosition(Vector3 position)
    {
        //normal of plan, a tester lol
        //Vector3 normal = (position - lastPos).normalized;
        //Vector3 v1 = new Vector3(-normal.y, normal.x, normal.z);
        //Vector3 v2 = new Vector3(normal.z, normal.z, -normal.y);

        //Vector3 noiseVector = noiseSpeed * position;
        //float t = Perlin.Noise(noiseVector.x, noiseVector.y, noiseVector.z);

        //noise (0,1) * 2 = (0,2) - 1 = (-1,1)
        // return position + (2 * Mathf.PerlinNoise(x,y) - 1) * Vector3.one * noiseAmmount;
        //return position + Vector3.Lerp(v1, v2, t) * noiseAmmount;
        return position + Random.insideUnitSphere * noiseAmmount;
    }

    public void SetOffset(float offset)
    {
        phiOffset = offset;
    }

    void UpdateLineRenderer()
    {
        lineRenderer.positionCount = positions.Count;
        for (int i = 0; i < positions.Count; i++)
        {
            lineRenderer.SetPosition(i, positions[i] + shakyshaky * Random.insideUnitSphere);
        }
    }
}
