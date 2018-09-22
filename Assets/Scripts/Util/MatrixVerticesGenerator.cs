using System.Collections.Generic;

public static class MatrixVerticesGenerator
{
    public static Matrix GenerateCubeVertices(int dimension)
    {
        List<List<float>> verticesList = new List<List<float>>();
        RecursiveCalculateVertices(verticesList, new List<float>(), dimension);
        return VerticesListToMatrix(verticesList);
    }

    static void RecursiveCalculateVertices(List<List<float>> verticesList, List<float> vertices, int dimensionLeft)
    {
        if (dimensionLeft == 0)
        {
            verticesList.Add(vertices);
            return;
        }

        dimensionLeft--;

        List<float> vertices1 = new List<float>(vertices);
        List<float> vertices2 = new List<float>(vertices);

        vertices1.Insert(0, .5f);
        vertices2.Insert(0, -.5f);

        RecursiveCalculateVertices(verticesList, vertices1, dimensionLeft);
        RecursiveCalculateVertices(verticesList, vertices2, dimensionLeft);
    }


    static Matrix VerticesListToMatrix(List<List<float>> verticesList)
    {
        Matrix verticesMatrix = new Matrix(verticesList.Count, verticesList[0].Count);

        for (int i = 0; i < verticesList.Count; i++)
        {
            for (int j = 0; j < verticesList[i].Count; j++)
            {
                verticesMatrix[i, j] = verticesList[i][j];
            }
        }
        return verticesMatrix;
    }
}