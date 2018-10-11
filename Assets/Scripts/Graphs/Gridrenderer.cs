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

    [Header("Modes")]
    [SerializeField] bool showRows;
    [SerializeField] bool showCols;
    [SerializeField] bool mirrorFlip;
    [SerializeField] bool generateMesh;
    [SerializeField] bool realTimeCalculate;

    //[Header("Twist")]
    //[SerializeField] float twistFactor = 1;
    //[SerializeField] AnimationCurve twistOverDistanceCurve;
    //[SerializeField] float maxTwist;

    //[Header("Twist anim")]
    //[SerializeField] Vector2 twistOffSet;
    //[SerializeField] Vector2 twistSinAmp;
    //[SerializeField] Vector2 twistSinFreq;
    //[SerializeField] Vector2 twistSinFreqOffset;


    [Header("Prefabs")]
    [SerializeField] LineRenderer lineRendererPrefab;

    LineRenderer[] lineRendererCol;
    LineRenderer[] lineRendererRow;

    float[,] zPositionMatrix;
    Vector3[,] positionMatrix;

    //Vector2[,] twirlMatrix;
    //Vector2 internalTwistOffSet;

    [SerializeField] Graph graph;
    [SerializeField] TwirlGrid twirlGrid;
    [SerializeField] MeshGenerator meshGenerator;
    //MeshFilter meshFilder;
    //Mesh mesh;
  
    private void Start()
    {
        //meshFilder = GetComponent<MeshFilter>();
        //meshFilder.mesh = new Mesh();
        //mesh = meshFilder.mesh;
        meshGenerator = new MeshGenerator(GetComponent<MeshFilter>());
        Initialize();
    }

    [ContextMenu("Update")]
    private void Initialize()
    {
       // zPositionMatrix = new float[rows, cols];

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

        if(realTimeCalculate)
            CalculateGrid();
    }

    [ContextMenu("Calculate Grid")]
    void CalculateGrid()
    {
        CalculateFunctionMatrix();
        twirlGrid.UpdateTwirl(rows, cols);

        CalculateFinalGridPosition();
        UpdateLineRenderers();
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
                positionMatrix[row, col] = new Vector3(twirledPosition.x * scale, zPositionMatrix[row, col] + lineRendererYoffset, twirledPosition.y * scale);
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

                //float x = twirlGrid[ii, jj].x * scale;
                //float y = twirlGrid[ii, jj].y * scale;

                //if(mirrorFlip && invertedXY)
                //{
                //    float tempx = row;
                //    row = col;
                //    col = tempx;
                //}

                lineRenderers[row].SetPosition(col, positionMatrix[row2, col2]);

                    //new Vector3(x, zPositionMatrix[ii, jj] + lineRendererYoffset, y));
            }
        }
    }

    //void UpdateTwirlMatrix()
    //{
    //    twirlMatrix = new Vector2[rows, cols];
    //    Vector2 middlePoint = new Vector2(rows * 0.5f, cols * 0.5f);

    //    for (int row = 0; row < rows; row++)
    //    {
    //        for (int col = 0; col < cols; col++)
    //        {
    //            Vector2 currentPoint = new Vector2(row + internalTwistOffSet.x, col + internalTwistOffSet.y);
    //            //Prend la distance du point par rapport au milieu
    //            //calcule t comme ratio de distance 
    //            //Twist par rapport a la distance
    //            float distanceFromCenter = (currentPoint - middlePoint).magnitude;
    //            float max = middlePoint.magnitude;
    //            float t = (Mathf.Min(distanceFromCenter, max) / max);
    //            float angle = Mathf.Lerp(0, maxTwist, 1 - twistOverDistanceCurve.Evaluate(t));
    //            twirlMatrix[row, col] = GameMath.RotateVector(angle * twistFactor, currentPoint - middlePoint);
    //        }
    //    }
    //}

    //void UpdateTwistOffset()
    //{
    //    internalTwistOffSet = twistOffSet + new Vector2(
    //        twistSinAmp.x * Mathf.Sin(Time.time * twistSinFreq.x + twistSinFreqOffset.x * Mathf.Deg2Rad),
    //        twistSinAmp.y * Mathf.Sin(Time.time * twistSinFreq.y + twistSinFreqOffset.y * Mathf.Deg2Rad));
    //}

    //void GenerateMesh()
    //{
    //    List<Vector3> vertices = new List<Vector3>();
    //    List<int> triangles = new List<int>();

    //    for (int y = 0; y < rows-1; y++)
    //    {
    //        for (int x = 0; x < cols-1; x++)
    //        {
    //            //(x,y),  (x+1,y), (x, y+1) 
    //            triangles.Add(GetIndexFromXY(x, y, cols));
    //            triangles.Add(GetIndexFromXY(x, y+1, cols));
    //            triangles.Add(GetIndexFromXY(x + 1, y, cols));

    //            //(x,y+1), (x+1, y), (x+1, y+1)
    //            triangles.Add(GetIndexFromXY(x, y+1, cols));
    //            triangles.Add(GetIndexFromXY(x + 1, y + 1, cols));
    //            triangles.Add(GetIndexFromXY(x + 1, y, cols));
    //        }
    //    }

    //    for (int row = 0; row < rows; row++)
    //    {
    //        for (int col= 0; col < cols; col++)
    //        {
    //            float x = twirlGrid[row, col].x * scale;
    //            float y = twirlGrid[row, col].y * scale;

    //            Vector3 vertex = new Vector3(x + offset.x, zPositionMatrix[row, col], y + offset.y);
    //            vertices.Add(vertex);
    //        }
    //    }
    //    mesh.SetTriangles(new int[0], 0);
    //    mesh.SetVertices(vertices);
    //    mesh.SetTriangles(triangles, 0);

    //    mesh.RecalculateNormals();
    //}

    //int GetIndexFromXY(int x, int y, int width)
    //{
    //    return y * width + x;
    //}
}
