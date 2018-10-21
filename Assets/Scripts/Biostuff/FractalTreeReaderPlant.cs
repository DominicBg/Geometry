using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractalTreeReaderPlant : FractalTreeReader
{
    [SerializeField] int[] forkPerDepth;

    public override float GetAngle(int i, int length, int depth, float minAngle, float maxAngle)
    {
        float ratio = (float)i / (length - 1);
        //float halfAngle = child.totalAngle * 0.5f;
        float angle = Mathf.Lerp(minAngle, maxAngle, ratio);

        return angle;
    }

    public override int GetFork(int depth)
    {
        return forkPerDepth[depth];
    }
}

