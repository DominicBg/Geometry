using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gridrenderer : MonoBehaviour {

    [Header("General")]
    [SerializeField] int cols;
    [SerializeField] int rows;
    [SerializeField] float scale;
    [SerializeField] float lineRendererYoffset = 1;
    [SerializeField] Vector2 offset;
    [SerializeField] float distanceFromCenter;
    [SerializeField] SinFloat realRadius;

    [Header("Modes")]
    [SerializeField] bool showRows;
    [SerializeField] bool showCols;
    [SerializeField] bool mirrorFlip;
    [SerializeField] bool generateMesh;
    [SerializeField] bool realTimeCalculate;

    [Header("Prefabs")]
    [SerializeField] LineRenderer lineRendererPrefab;

    LineRenderer[] lineRendererCol;
    LineRenderer[] lineRendererRow;

    float[,] zPositionMatrix;
    Vector3[,] positionMatrix;

    [SerializeField] Graph graph;
    [SerializeField] TwirlGrid twirlGrid;
    [SerializeField] MeshGenerator meshGenerator;

    private void Start()
    {
        meshGenerator = new MeshGenerator(GetComponent<MeshFilter>());
        Initialize();
    }

    [ContextMenu("Update")]
    private void Initialize()
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

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            showCols = showRows = false;
            generateMesh = true;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            showCols = showRows = true;
            generateMesh = false;
        }

            if (realTimeCalculate)
            CalculateGrid();
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
            meshGenerator.GenerateMesh(rows, cols, positionMatrix);

    }

    void CalculateFinalGridPosition()
    {
        positionMatrix = new Vector3[cols, rows];
        for (int col = 0; col < cols; col++)
        {
            for (int row = 0; row < rows; row++)
            {
                Vector2 twirledPosition = twirlGrid[row, col];
                Vector3 newPosition = new Vector3(twirledPosition.x * scale, zPositionMatrix[row, col] + lineRendererYoffset, twirledPosition.y * scale);
                positionMatrix[row, col] = newPosition + newPosition.normalized * distanceFromCenter;

                if(positionMatrix[row, col].magnitude > realRadius)
                {
                    positionMatrix[row, col] = positionMatrix[row, col].normalized * realRadius;
                }
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

                lineRenderers[row].SetPosition(col, positionMatrix[row2, col2]);
            }
        }
    }
}
