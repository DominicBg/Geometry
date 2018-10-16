using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Graph : MonoBehaviour{
    protected Vector2 middlePoint; 
    protected float scale;

    public void SetMiddlePoint(Vector2 middlePoint)
    {
        this.middlePoint = middlePoint;
    }
    public void SetScale(float scale)
    {
        this.scale = scale;
    }
    public abstract float CalculatePoint(float x, float y);
}
