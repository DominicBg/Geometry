using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGroup : MonoBehaviour {

    public LineRenderer lineRenderer;
    public int[] indexTransform;

    public void ApplyLineToGroup(Vector3[] group)
    {
        Vector3[] positions = new Vector3[indexTransform.Length + 1];
        for (int i = 0; i < indexTransform.Length; i++)
        {
            positions[i] = group[indexTransform[i]];
        }
        //Loop back
        positions[indexTransform.Length] = group[indexTransform[0]];

        lineRenderer.positionCount = indexTransform.Length + 1;
        lineRenderer.SetPositions(positions);
    }

    public void ApplyColor(Color startColor, Color endColor)
    {
        lineRenderer.startColor = startColor;
        lineRenderer.endColor = endColor;
    }
}
