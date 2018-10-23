using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PolarRose : MonoBehaviour {

    [SerializeField] float radius;
    [SerializeField] SinFloat n;
    [SerializeField] float d;
    [SerializeField] int resolution;
    [SerializeField] SinFloat loops;
    [SerializeField] bool mapSphere;
    [SerializeField] AnimationCurve curveSphere;
    LineRenderer lineRenderer;
    float HALFPI = Mathf.PI * 0.25f;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update ()
    {
        lineRenderer.positionCount = resolution;
        Vector3[] positions = GameMath.GetPolarRoseCoordinateVector3(n / d, radius, resolution, loops);

        if(mapSphere)
        {
            ApplyMapSphere(ref positions);
        }

        lineRenderer.SetPositions(positions);
    }

    void ApplyMapSphere(ref Vector3[] positions)
    {
        for (int i = 0; i < positions.Length; i++)
        {
            float magnitude = positions[i].magnitude;
            float t = magnitude / radius;
            float sinT = Mathf.Lerp(0, HALFPI, t);
            // float z = -curveSphere.Evaluate(1-t) * radius;
            float z = -Mathf.Cos(-sinT) * radius;
            positions[i] = positions[i].SetZ(Mathf.Sin(z));
        }
    }
}
