using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinCurvedGraph : PerlinGraph
{
    [SerializeField] LerpFloat height;
    [SerializeField] LerpFloat heightCubed;

    [SerializeField] Vector2 offset;
    [SerializeField] float distanceToVoid;
    [SerializeField] SinFloat diffFromMiddleToVoid;
    
    public override float CalculatePoint(float x, float y)
    {
        x += offset.x;
        y += offset.y;

        float point = base.CalculatePoint(x, y);

        float diffX = Mathf.Abs(middlePoint.x - x);
        float diffY = Mathf.Abs(middlePoint.y - y);

        Vector2 diff = new Vector2(diffX ,diffY);
        Vector2 diffCubed = new Vector2(diffX * diffX, diffY * diffY);

        float value = point + heightCubed * diffCubed.magnitude + height * diff.magnitude;

        if (diff.magnitude < diffFromMiddleToVoid)
            return value + distanceToVoid;
        else
            return value;
    }
}
