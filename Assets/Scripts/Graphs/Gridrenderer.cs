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
    [SerializeField] bool showRows;
    [SerializeField] bool showCols;
    [SerializeField] bool mirrorFlip;

    [Header("Twist")]
    [SerializeField] float twistFactor = 1;
    [SerializeField] AnimationCurve twistOverDistanceCurve;
    [SerializeField] float maxTwist;

    [Header("Twist anim")]
    [SerializeField] Vector2 twistOffSet;
    [SerializeField] Vector2 twistSinAmp;
    [SerializeField] Vector2 twistSinFreq;
    [SerializeField] Vector2 twistSinFreqOffset;

    [Header("General")]
    [SerializeField] LineRenderer lineRendererPrefab;

    LineRenderer[] lineRendererCol;
    LineRenderer[] lineRendererRow;

    float[,] zPositionMatrix;
    Vector2[,] twirlMatrix;
    Vector2 internalTwistOffSet;

    [SerializeField] Graph graph;

    MeshFilter meshFilder;
    Mesh mesh;

    [SerializeField] bool generateMesh;
    [SerializeField] bool realTimeCalculate;
    private void Start()
    {
        meshFilder = GetComponent<MeshFilter>();
        meshFilder.mesh = new Mesh();
        mesh = meshFilder.mesh;

        Initialize();
    }

    [ContextMenu("Update")]
    private void Initialize()
    {
        zPositionMatrix = new float[rows, cols];

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
        UpdateTwistOffset();
        UpdateTwirlMatrix();
        UpdateGrid();
        if (generateMesh)
            GenerateMesh();
    }

    void UpdateGrid()
    {
        CalculatePointInMatrix();
        SetLineRendererPosition(lineRendererCol, false, showRows);
        SetLineRendererPosition(lineRendererRow, true, showCols);
    }

    private void CalculatePointInMatrix()
    {
        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                zPositionMatrix[x, y] = graph.CalculatePoint((x + offset.x), (y + offset.y));
            }
        }
    }

    void SetLineRendererPosition(LineRenderer[] lineRenderer, bool invertedXY, bool show)
    {
        int internalRows = (invertedXY) ? cols : rows;
        int internalCols = (invertedXY) ? rows : cols;
   
        for (int i = 0; i < internalRows; i++)
        {
            if (!show)
            {
                lineRenderer[i].positionCount = 0;
                continue;
            }

            lineRenderer[i].positionCount = internalRows;

            for (int j = 0; j < internalCols; j++)
            {
                //inverted variables
                int ii = (invertedXY) ? j : i;
                int jj = (invertedXY) ? i : j;

                float x = twirlMatrix[ii, jj].x * scale;
                float y = twirlMatrix[ii, jj].y * scale;

                if(mirrorFlip && invertedXY)
                {
                    float tempx = x;
                    x = y;
                    y = tempx;
                }

                lineRenderer[i].SetPosition(j,
                    new Vector3(x, zPositionMatrix[i, j] + lineRendererYoffset, y));
            }
        }
    }

    void UpdateTwirlMatrix()
    {
        twirlMatrix = new Vector2[rows, cols];
        Vector2 middlePoint = new Vector2(rows * 0.5f, cols * 0.5f);

        for (int i = 0; i < rows; i++)
        {
            lineRendererRow[i].positionCount = cols;

            for (int j = 0; j < cols; j++)
            {
                Vector2 currentPoint = new Vector2(i + internalTwistOffSet.x, j + internalTwistOffSet.y);
                //Prend la distance du point par rapport au milieu
                //calcule t comme ratio de distance 
                //Twist par rapport a la distance
                float distanceFromCenter = (currentPoint - middlePoint).magnitude;
                float max = middlePoint.magnitude;
                float t = (Mathf.Min(distanceFromCenter, max) / max);
                float angle = Mathf.Lerp(0, maxTwist, 1 - twistOverDistanceCurve.Evaluate(t));
                twirlMatrix[i, j] = GameMath.RotateVector(angle * twistFactor, currentPoint - middlePoint);
            }
        }
    }

    void UpdateTwistOffset()
    {
        internalTwistOffSet = twistOffSet + new Vector2(
            twistSinAmp.x * Mathf.Sin(Time.time * twistSinFreq.x + twistSinFreqOffset.x * Mathf.Deg2Rad),
            twistSinAmp.y * Mathf.Sin(Time.time * twistSinFreq.y + twistSinFreqOffset.y * Mathf.Deg2Rad));
    }

    void GenerateMesh()
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        for (int y = 0; y < rows-1; y++)
        {
            for (int x = 0; x < cols-1; x++)
            {
                //(x,y),  (x+1,y), (x, y+1) 
                triangles.Add(GetIndexFromXY(x, y, cols));
                triangles.Add(GetIndexFromXY(x, y+1, cols));
                triangles.Add(GetIndexFromXY(x + 1, y, cols));

                //(x,y+1), (x+1, y), (x+1, y+1)
                triangles.Add(GetIndexFromXY(x, y+1, cols));
                triangles.Add(GetIndexFromXY(x + 1, y + 1, cols));
                triangles.Add(GetIndexFromXY(x + 1, y, cols));
            }
        }

        for (int j = 0; j < rows; j++)
        {
            for (int i= 0; i< cols; i++)
            {
                float x = twirlMatrix[i, j].x * scale;
                float y = twirlMatrix[i, j].y * scale;

                Vector3 vertex = new Vector3(x + offset.x, zPositionMatrix[i, j], y + offset.y);
                vertices.Add(vertex);
            }
        }

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();
    }

    int GetIndexFromXY(int x, int y, int width)
    {
        return y * width + x;
    }
}
