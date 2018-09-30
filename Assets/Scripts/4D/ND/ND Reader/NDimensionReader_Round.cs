using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NDimensionReader_Round : NDimensionReader_Basic
{
    [SerializeField] float roundness = 1;

    public override Vector3 NDtoVector3(float[] values)
    {
        Vector3 result = base.NDtoVector3(values);
        if (result.magnitude > roundness)
            result = result.normalized * roundness;

        return result;
    }
}
