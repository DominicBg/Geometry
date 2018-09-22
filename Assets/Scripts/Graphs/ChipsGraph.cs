using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipsGraph : Graph {

    public override float CalculatePoint(float x, float y)
    {
        return x * x + y * y + Mathf.Sin(Time.time + x*y);
    }
}
