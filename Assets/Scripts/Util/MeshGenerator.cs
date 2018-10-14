﻿using System.Collections;
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

    public void GenerateMesh(int rows, int cols) //, Vector3[,] points, float scale)
    {
       // List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        for (int y = 0; y < rows - 1; y++)
        {
            for (int x = 0; x < cols - 1; x++)
            {
                //(x,y),  (x+1,y), (x, y+1) 
                triangles.Add(GameMath.GetIndexFromXY(x, y, cols));
                triangles.Add(GameMath.GetIndexFromXY(x, y + 1, cols));
                triangles.Add(GameMath.GetIndexFromXY(x + 1, y, cols));

                //(x,y+1), (x+1, y), (x+1, y+1)
                triangles.Add(GameMath.GetIndexFromXY(x, y + 1, cols));
                triangles.Add(GameMath.GetIndexFromXY(x + 1, y + 1, cols));
                triangles.Add(GameMath.GetIndexFromXY(x + 1, y, cols));
            }
        }

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                //float x = twirlGrid[row, col].x * scale;
                //float y = twirlGrid[row, col].y * scale;

                //Vector3 vertex = new Vector3(x + offset.x, zPositionMatrix[row, col], y + offset.y);
                //vertices.Add(points[row, col]);
                uvs.Add(new Vector2((float)row / rows, (float)col / cols));
            }
        }
        mesh.SetTriangles(new int[0], 0);
        //mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.SetUVs(0,uvs);
        //mesh.RecalculateNormals();
    }

    public void SetVertices(Vector3[] points)
    {

        mesh.vertices = points;
        //mesh.SetVertices(points);

        mesh.RecalculateNormals();

    }
}
