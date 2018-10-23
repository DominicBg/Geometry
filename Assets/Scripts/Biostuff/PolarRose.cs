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
    [SerializeField] float timer;
    [SerializeField] LerpFloat speed;
    [SerializeField] float zScalar = -1;
    [SerializeField] bool inverseCurve;
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            timer = 0;
        timer += Time.deltaTime * speed;

        lineRenderer.positionCount = resolution;
        float realN = n.CalculateMinMax(timer);
        float realLoops = loops.CalculateMinMax(timer);
        Vector3[] positions = GameMath.GetPolarRoseCoordinateVector3(realN / d, radius, resolution, realLoops);

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
            // sinT = Mathf.Lerp(0, HALFPI, t);
            float z = curveSphere.Evaluate((inverseCurve) ? 1 - t : t) * radius;
            //float z = -Mathf.Cos(-sinT) * radius;
            //positions[i] = positions[i].SetZ(Mathf.Sin(z));
            positions[i] = positions[i].SetZ(z * zScalar);

        }
    }
}
