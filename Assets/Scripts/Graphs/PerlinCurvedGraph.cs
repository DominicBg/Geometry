using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinCurvedGraph : PerlinGraph
{
    [SerializeField] float height;
    [SerializeField] float heightCubed;

    [SerializeField] Vector2 offset;

    //[SerializeField] float diffFromMiddleToVoid;
    [SerializeField] float distanceToVoid;
    [SerializeField] AnimationFloat diffFromMiddleToVoid;

    public override float CalculatePoint(float x, float y)
    {
        x += offset.x;
        y += offset.y;

        float point = base.CalculatePoint(x, y);

        float diffX = Mathf.Abs(middlePoint.x - x);
        float diffY = Mathf.Abs(middlePoint.y - y);

       // diffFromMiddleToVoid = voidAnimation.CalculateMinMax(); 

        Vector2 diff = new Vector2(diffX ,diffY);
        Vector2 diffCubed = new Vector2(diffX * diffX, diffY * diffY);

        float value = point + heightCubed * diffCubed.magnitude + height * diff.magnitude;

        if (diff.magnitude < diffFromMiddleToVoid)
            return value + distanceToVoid;
        //if (diffX < diffFromMiddleToVoid && diffY < diffFromMiddleToVoid)
        //    return value + distanceToVoid;
        else
            return value;
    }
}
