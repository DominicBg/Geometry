using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FractalTreeReader : MonoBehaviour {

    public abstract float GetAngle(int i, int length, int depth, float minAngle, float maxAngle);
    public abstract int GetFork(int depth);
    public virtual float GetLenght(float lenght) { return lenght; }

    public virtual List<Vector3> GetVertices(Vector3 position, Vector3 direction)
    {
        List<Vector3> pointList =  new List<Vector3>();
        pointList.Add(position + direction);
        return pointList;
    }
}
