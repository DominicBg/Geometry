using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractalTreeReaderLightning : FractalTreeReader
{ 
    [SerializeField] int minFork;
    [SerializeField] int maxFork;
    [SerializeField] AnimationCurve forkCurve;

    [SerializeField] int noiseMin;
    [SerializeField] int noiseMax;
    [SerializeField] float noiseAmplitude;
    [SerializeField] float noiseLength;

    [SerializeField, Range(0,1)] float lengthRandomiser;

    public override float GetAngle(int i, int length, int depth, float minAngle, float maxAngle)
    {
        return Random.Range(minAngle, maxAngle);
    }

    public override int GetFork(int depth)
    {
        if (depth == 0)
            return 1;

        float random = Random.Range(0f, 1f);
        return (int)Mathf.Lerp(minFork, maxFork+1, forkCurve.Evaluate(random));
    }

    public override float GetLenght(float length)
    {
        float lengthRatio = length * lengthRandomiser;
        return length + Random.Range(-lengthRatio, lengthRatio);
    }

    public override List<Vector3> GetVertices(Vector3 position, Vector3 direction)
    {
        List<Vector3> pointList = new List<Vector3>();
        int random = Random.Range(noiseMin, noiseMax+1);

        Vector3 lastPoint = position + direction;
        for (int i = 0; i < random; i++)
        {
            float angleRandom = Random.Range(-noiseAmplitude, noiseAmplitude);
            Vector3 newPoint = lastPoint + GameMath.RotateVectorZ(angleRandom, direction) * noiseLength;
            pointList.Add(newPoint);
            lastPoint = newPoint;
        }

        return pointList;
    }
}
