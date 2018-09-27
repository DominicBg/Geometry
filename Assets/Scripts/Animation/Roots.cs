using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class Roots : MonoBehaviour {

    [SerializeField] float radius = 10;

    [SerializeField] float thetaOffset = 90;
    [SerializeField] float phiOffset = 0;
    [SerializeField] float thetaRatio = 5;
    [SerializeField] float phiRatio = 1;

    [SerializeField] float noiseAmmount = 1;
    [SerializeField] float noiseSpeed = 1;

    [SerializeField] float speed;
    [SerializeField] float maxTimer;
    float timer;

    TrailRenderer trail;
    Vector3 lastPos;

    [ContextMenu("Reset Timer")]
    public void ResetTimer()
    {
        timer = 0;
        trail.SetPositions(new Vector3[0]);
        //trail.enabled = false;
    }


    private void Start()
    {
        trail = GetComponent<TrailRenderer>();
        trail.enabled = false;
    }

    private void Update()
    {
        if (timer > maxTimer)
            return;

        lastPos = transform.localPosition;

        timer += Time.deltaTime * speed;

        float realPhiOffset = phiOffset * Mathf.Deg2Rad;
        float realThetaOffset = thetaOffset * Mathf.Deg2Rad;

        Vector3 newPosition = GameMath.SphericalRotation(radius, timer * thetaRatio + realThetaOffset, timer * phiRatio + realPhiOffset);
        newPosition = CalculateNoisePosition(newPosition);
        transform.localPosition = newPosition;

        trail.enabled = true;
    }

    Vector3 CalculateNoisePosition(Vector3 position)
    {
        //normal of plan, a tester lol
        Vector3 normal = (position - lastPos).normalized;
        Vector3 v1 = new Vector3(-normal.y, normal.x, normal.z);
        Vector3 v2 = new Vector3(normal.z, normal.z, -normal.y);

        float x = noiseSpeed * (position.x + position.z);
        float y = noiseSpeed * (position.y + position.z);
        float t = Mathf.PerlinNoise(x, y);

        //noise (0,1) * 2 = (0,2) - 1 = (-1,1)
        // return position + (2 * Mathf.PerlinNoise(x,y) - 1) * Vector3.one * noiseAmmount;
        return position + Vector3.Lerp(v1, v2, t) * noiseAmmount;
    }

    public void SetOffset(float offset)
    {
        phiOffset = offset;
    }
}
