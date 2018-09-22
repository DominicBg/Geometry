using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleGraph : Graph
{
    public float amp;
    public float speed;

    public override float CalculatePoint(float x, float y)
    {
        //Let's define r = sqrt(x2 +y2 )
        //Try(a / (1 + r)) * cos((b / log(r + 2)) * r),
        float a = amp;
        float b = speed * Time.time;
        float r = Mathf.Sqrt(x * x + y * y);

        return (a / (1 + r)) * Mathf.Cos((b / Mathf.Log(r + 2)) * r);
        //return Mathf.Sin(x + Time.time) + Mathf.Cos(y + Time.time);
    }
}
