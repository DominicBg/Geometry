using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractalTreeBranch : MonoBehaviour {

    public Vector3 position { get { return vertices[vertices.Count - 1]; } }
    public float length;
    public float angle;
    public float totalAngle;
    public Vector3 direction;
    public List<Vector3> vertices;

    public FractalTreeBranch[] childs;
    LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void SetColor(Color color)
    {
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
    }

    public void SetLineRendererPositions()
    {
        lineRenderer.positionCount = vertices.Count;
        lineRenderer.SetPositions(vertices.ToArray());
    }
}
