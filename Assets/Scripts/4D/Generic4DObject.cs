using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic4DObject : Rotation4D {

    [SerializeField] MeshFilter meshFilter;

    [SerializeField] Gradient startColorGradient;
    [SerializeField] Gradient endColorGradient;

    [SerializeField] protected float w1;
    [SerializeField] protected float w2;

    Vector3[] vertices;
    [SerializeField] LineRenderer lineRendererPrefab;

    int[] triangles;
    LineRenderer[] lineRenderers;
   

    void Awake()
    {
        Calculate();
    }

    void Update()
    {
        if (verticesMatrixPosition != null)
        {
            base.Rotate();
            CalculateLineRenderer(triangles);
            ColorLineRenderer();
        }
    }

    protected override void Calculate()
    {
        vertices = meshFilter.mesh.vertices;
        triangles = meshFilter.mesh.triangles;

        verticesMatrixPosition = new Matrix(vertices.Length * 2, 4);

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = vertices[i];
            verticesMatrixPosition[i, 0] = vertex.x;
            verticesMatrixPosition[i, 1] = vertex.y;
            verticesMatrixPosition[i, 2] = vertex.z;
            verticesMatrixPosition[i, 3] = w1; //(vertex.x+ vertex.y+ vertex.z) * 0.333f;
        }

        for (int i = vertices.Length; i < vertices.Length * 2; i++)
        {
            Vector3 vertex = vertices[i - vertices.Length];
            verticesMatrixPosition[i, 0] = vertex.x;
            verticesMatrixPosition[i, 1] = vertex.y;
            verticesMatrixPosition[i, 2] = vertex.z;
            verticesMatrixPosition[i, 3] = w2; // -(vertex.x + vertex.y + vertex.z) * 0.333f;
        }

        InstanciateLineRenderers(triangles);
    }

    private void InstanciateLineRenderers(int[] triangles)
    {
        int numberLineToInstanciate = (triangles.Length * 2) / 3;
        lineRenderers = new LineRenderer[numberLineToInstanciate];
        for (int i = 0; i < numberLineToInstanciate; i++)
        {
            lineRenderers[i] = Instantiate(lineRendererPrefab);
            lineRenderers[i].transform.SetParent(transform);
            lineRenderers[i].transform.localPosition = Vector3.zero;
        }

    }

    void CalculateLineRenderer(int[] triangles)
    {
        int indexLineRenderer = 0;
        for (int indexTri = 0; indexTri < triangles.Length; indexTri += 3)
        {
            Ajust3DRenderer(triangles, indexLineRenderer, indexTri);
            Ajust4DLineRenderer(triangles, indexLineRenderer, indexTri);

            //try
            //{

            //}
            //catch
            //{
            //    Debug.Log("Erreur indexLine " + indexLineRenderer + " index tri " + indexTri);

            //}

            indexLineRenderer++;
        }
    }

    private void Ajust3DRenderer(int[] triangles, int indexLineRenderer, int indexTri)
    {
        LineRenderer lineRenderer = lineRenderers[indexLineRenderer];
        lineRenderer.positionCount = 4;
        lineRenderer.SetPosition(0, verticesPositionsAfterRotation[triangles[indexTri]]);
        lineRenderer.SetPosition(1, verticesPositionsAfterRotation[triangles[indexTri + 1]]);
        lineRenderer.SetPosition(2, verticesPositionsAfterRotation[triangles[indexTri + 2]]);
        lineRenderer.SetPosition(3, verticesPositionsAfterRotation[triangles[indexTri]]);
    }

    private void Ajust4DLineRenderer(int[] triangles, int indexLineRenderer, int indexTri)
    {
        LineRenderer lineRenderer4D = lineRenderers[indexLineRenderer + (triangles.Length / 3)];

        lineRenderer4D.positionCount = 10;

        lineRenderer4D.SetPosition(0, verticesPositionsAfterRotation[triangles[indexTri]]);
        lineRenderer4D.SetPosition(1, verticesPositionsAfterRotation[triangles[indexTri] + vertices.Length]);

        lineRenderer4D.SetPosition(2, verticesPositionsAfterRotation[triangles[indexTri + 1] + vertices.Length]);
        lineRenderer4D.SetPosition(3, verticesPositionsAfterRotation[triangles[indexTri + 1]]);
        lineRenderer4D.SetPosition(4, verticesPositionsAfterRotation[triangles[indexTri + 1] + vertices.Length]);

        lineRenderer4D.SetPosition(5, verticesPositionsAfterRotation[triangles[indexTri + 2] + vertices.Length]);
        lineRenderer4D.SetPosition(6, verticesPositionsAfterRotation[triangles[indexTri + 2]]);
        lineRenderer4D.SetPosition(7, verticesPositionsAfterRotation[triangles[indexTri + 2] + vertices.Length]);

        lineRenderer4D.SetPosition(8, verticesPositionsAfterRotation[triangles[indexTri] + vertices.Length]);
        lineRenderer4D.SetPosition(9, verticesPositionsAfterRotation[triangles[indexTri]]);
    }

    void ColorLineRenderer()
    {
        for (int i = 0; i < lineRenderers.Length; i++)
        {
            float t = i / (float)lineRenderers.Length;
            lineRenderers[i].startColor = startColorGradient.Evaluate(t);
            lineRenderers[i].endColor = endColorGradient.Evaluate(t);
        }
    }
}
