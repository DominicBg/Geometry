using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractalStormBranch : MonoBehaviour {

    public float length;
    public List<Vector3> vertices;

    public FractalStormBranch[] childs;
    LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void SetLineRendererPositions()
    {
        lineRenderer.positionCount = vertices.Count;
        lineRenderer.SetPositions(vertices.ToArray());
    }
}
