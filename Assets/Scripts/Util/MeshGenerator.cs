using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator {

    MeshFilter meshFilter;
    Mesh mesh;

    public MeshGenerator(MeshFilter meshFilter)
    {
        this.meshFilter = meshFilter;
        meshFilter.mesh = new Mesh();
        mesh = meshFilter.mesh;
    }

    public void ShowMesh(bool show)
    {
        meshFilter.mesh = (show) ? mesh : null;
    }

    public void GenerateMesh(int rows, int cols, Vector3[,] points)
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        for (int y = 0; y < rows - 1; y++)
        {
            for (int x = 0; x < cols - 1; x++)
            {
                //(x,y),  (x+1,y), (x, y+1) 
                triangles.Add(GetIndexFromXY(x, y, cols));
                triangles.Add(GetIndexFromXY(x, y + 1, cols));
                triangles.Add(GetIndexFromXY(x + 1, y, cols));

                //(x,y+1), (x+1, y), (x+1, y+1)
                triangles.Add(GetIndexFromXY(x, y + 1, cols));
                triangles.Add(GetIndexFromXY(x + 1, y + 1, cols));
                triangles.Add(GetIndexFromXY(x + 1, y, cols));
            }
        }

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                //float x = twirlGrid[row, col].x * scale;
                //float y = twirlGrid[row, col].y * scale;

                //Vector3 vertex = new Vector3(x + offset.x, zPositionMatrix[row, col], y + offset.y);
                vertices.Add(points[row,col]);
            }
        }
        mesh.SetTriangles(new int[0], 0);
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);

        mesh.RecalculateNormals();
    }

    int GetIndexFromXY(int x, int y, int width)
    {
        return y * width + x;
    }
}
