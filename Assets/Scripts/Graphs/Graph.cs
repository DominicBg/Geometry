using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Graph : MonoBehaviour{
    [SerializeField] protected Vector2 middlePoint; 

    public void SetMiddlePoint(Vector2 middlePoint)
    {
        this.middlePoint = middlePoint;
    }

    public abstract float CalculatePoint(float x, float y);
}
