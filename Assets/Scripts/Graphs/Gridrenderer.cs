using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gridrenderer : MonoBehaviour {

    [SerializeField] int cols;
    [SerializeField] int rows;
    [SerializeField] float scale;
    [SerializeField] float lineRendererYoffset = 1;
    [SerializeField] Vector2 offset;
    [SerializeField] LineRenderer lineRendererPrefab;

    LineRenderer[] lineRendererCol;
    LineRenderer[] lineRendererRow;

    float[,] zPositionMatrix;

    [SerializeField] Graph graph;

    MeshFilter meshFilder;
    Mesh mesh;

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
        UpdateGrid();
        GenerateMesh();
    }

    void UpdateGrid()
    {
        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                zPositionMatrix[x, y] = graph.CalculatePoint(x+offset.x, y+offset.y);
            }
        }

        for (int i = 0; i < cols; i++)
        {
            lineRendererCol[i].positionCount = rows;

            for (int j = 0; j < rows; j++)
            {
                lineRendererCol[i].SetPosition(j,
                    new Vector3(i * scale, zPositionMatrix[i, j]+ lineRendererYoffset, j* scale));
            }
        }

        for (int i = 0; i < rows; i++)
        {
            lineRendererRow[i].positionCount = cols;

            for (int j = 0; j < cols; j++)
            {
                lineRendererRow[i].SetPosition(j, 
                    new Vector3(j * scale, zPositionMatrix[i, j]+ lineRendererYoffset, i * scale));
            }
        }
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
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                Vector3 vertex = new Vector3(x*scale, zPositionMatrix[x, y], y * scale);
                vertices.Add(vertex);
            }
        }


        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
    }

    int GetIndexFromXY(int x, int y, int width)
    {
        return y * width + x;
    }

}
