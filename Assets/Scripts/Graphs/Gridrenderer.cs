using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridRenderer : MonoBehaviour {

    [Header("General")]
    [SerializeField] protected int cols;
    [SerializeField] protected int rows;
    [SerializeField] protected float scale;
    [SerializeField] protected float lineRendererYoffset = 1;
    [SerializeField] protected Vector2 offset;
    [SerializeField] protected float distanceFromCenter;
    [SerializeField] protected float radius;

    [Header("Modes")]
    [SerializeField] protected bool showRows;
    [SerializeField] protected bool showCols;
    [SerializeField] protected bool mirrorFlip;
    [SerializeField] protected bool generateMesh;
    [SerializeField] protected bool realTimeCalculate;

    [Header("Prefabs")]
    [SerializeField] LineRenderer lineRendererPrefab;

    LineRenderer[] lineRendererCol;
    LineRenderer[] lineRendererRow;

    float[,] zPositionMatrix;
    Vector3[,] positionMatrix;
    Vector3[] vertices;

    [SerializeField] Graph graph;
    [SerializeField] TwirlGrid twirlGrid;
    [SerializeField] MeshGenerator meshGenerator;
    [SerializeField] Transform followAtPoint;
    [SerializeField] Vector2Int positionToFollow;
    [SerializeField] Vector3 followOffset;

    private void Start()
    {
        meshGenerator = new MeshGenerator(GetComponent<MeshFilter>());
        GenerateLineRenderers();

        CalculateGrid();
        if (generateMesh)
            meshGenerator.GenerateMesh(rows, cols);
    }

    [ContextMenu("Update")]
    private void GenerateLineRenderers()
    {
        for (int i = 0; i < cols + rows; i++)
        {
            LineRenderer lr = Instantiate(lineRendererPrefab, transform.position, Quaternion.identity);
            lr.transform.SetParent(transform);
        }

        LineRenderer[] lineRendererTotal = GetComponentsInChildren<LineRenderer>();

        lineRendererCol = new LineRenderer[cols];
        for (int i = 0; i < cols; i++)
        {
            lineRendererCol[i] = lineRendererTotal[i];
        }

        lineRendererRow = new LineRenderer[rows];
        for (int i = 0; i < rows; i++)
        {
            lineRendererRow[i] = lineRendererTotal[i + cols];
        }
    }

    protected void Update()
    {

        if (realTimeCalculate)
            CalculateGrid();

        if(followAtPoint != null)
            followAtPoint.position = positionMatrix[positionToFollow.x, positionToFollow.y] + followOffset;
    }   

    [ContextMenu("Calculate Grid")]
    void CalculateGrid()
    {
        CalculateFunctionMatrix();
        twirlGrid.UpdateTwirl(rows, cols);

        CalculateFinalGridPosition();
        UpdateLineRenderers();

        meshGenerator.ShowMesh(generateMesh);

        if (generateMesh)
        {
            meshGenerator.SetVertices(vertices);
        }
        //    meshGenerator.GenerateMesh(rows, cols, positionMatrix, scale);

    }

    private void OnValidate()
    {
        if (meshGenerator != null && generateMesh)
            meshGenerator.GenerateMesh(rows, cols);
    }

    void CalculateFinalGridPosition()
    {
        positionMatrix = new Vector3[rows, cols];
        vertices = new Vector3[cols * rows];

        for (int col = 0; col < cols; col++)
        {
            for (int row = 0; row < rows; row++)
            {
                Vector2 twirledPosition = twirlGrid[row, col];
                Vector3 newPosition = new Vector3(twirledPosition.x * scale, zPositionMatrix[row, col], twirledPosition.y * scale);
                positionMatrix[row, col] = newPosition + newPosition.normalized * distanceFromCenter;

                if(positionMatrix[row, col].magnitude > radius)
                {
                    positionMatrix[row, col] = positionMatrix[row, col].normalized * radius;
                }

                vertices[GameMath.GetIndexFromXY(row, col, cols)] = positionMatrix[row, col];
            }
        }
    }

    void UpdateLineRenderers()
    {
        SetLineRendererPosition(lineRendererCol, false, showRows);
        SetLineRendererPosition(lineRendererRow, true, showCols);
    }

    private void CalculateFunctionMatrix()
    {
        zPositionMatrix = new float[rows, cols];

        graph.SetMiddlePoint(new Vector2(rows / 2.0f, cols / 2.0f));
        for (int col = 0; col < cols; col++)
        {
            for (int row = 0; row < rows; row++)
            {
                zPositionMatrix[row, col] = graph.CalculatePoint((row + offset.x), (col + offset.y));
            }
        }
    }

    void SetLineRendererPosition(LineRenderer[] lineRenderers, bool invertedXY, bool show)
    {
        int internalRows = rows;//(invertedXY) ? cols : rows;
        int internalCols = cols;//(invertedXY) ? rows : cols;
   
        for (int row = 0; row < internalRows; row++)
        {
            if (!show)
            {
                if(row < lineRenderers.Length)
                    lineRenderers[row].positionCount = 0;
                continue;
            }

            lineRenderers[row].positionCount = internalRows;

            for (int col = 0; col < internalCols; col++)
            {
                //inverted variables
                int row2 = (invertedXY) ? col : row;
                int col2 = (invertedXY) ? row : col;

                lineRenderers[row].SetPosition(col, positionMatrix[row2, col2] + Vector3.up * lineRendererYoffset);
            }
        }
    }
}
