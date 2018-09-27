using System.Collections.Generic;
using UnityEngine;

public class NDCube : MonoBehaviour {

    public List<VectorN> vertices { get; protected set; }
    public List<LineGroup> lineGroups { get; set; }

    public int dimension;
    public float size = 10;
    NDCube min1DimensionCubeLeft;
    NDCube min1DimensionCubeRight;
    NDCube root;

    Matrix verticesMatrix;
    Vector3[] rotatedVertices;

    [SerializeField] LineGroup lineGroupPrefab;

    float angle = 0;

    [ContextMenu("Generate N Cube")]
    private void StartGeneration()
    {
        root = this;
        GenerateNCube(root);


        CalculateVerticesMatrix();
    }

    void CalculateVerticesMatrix()
    {
        verticesMatrix = new Matrix(vertices.Count, dimension);
        for (int i = 0; i < vertices.Count; i++)
        {
            verticesMatrix.SetRow(i, vertices[i].GetValues());
        }
    }

    private void Update()
    {
        if (verticesMatrix == null)
            return;

        angle += Time.deltaTime;
        //x = 0, y = 1, z = 2, w = 3.. etc
        Matrix rotation_3_4 = MatrixRotationND.Rotation(angle, dimension, 3, 4);
        Matrix rotation_0_3 = MatrixRotationND.Rotation(angle, dimension, 0, 3);
        Matrix rotation_4_5 = MatrixRotationND.Rotation(angle, dimension, 4, 5);


        //16x5 X 5x5
        Matrix rotatedMatrix = Matrix.StupidMultiply(verticesMatrix, rotation_3_4);
        rotatedMatrix = Matrix.StupidMultiply(rotatedMatrix, rotation_0_3);
        rotatedMatrix = Matrix.StupidMultiply(rotatedMatrix, rotation_4_5);

        rotatedVertices = new Vector3[rotatedMatrix.rows];
        for (int i = 0; i < rotatedMatrix.rows; i++)
        {
            //Transform matrix row into 3d projection etc etc

            rotatedVertices[i] = VectorN.NDtoVector3(rotatedMatrix.GetRow(i));
        }

        UpdateLineGroups();
    }


    public void GenerateNCube(NDCube root)
    {
        this.root = root;
        size = root.size;
        vertices = new List<VectorN>();
        lineGroups = new List<LineGroup>();
        min1DimensionCubeLeft = GenerateLowerDimension();
        min1DimensionCubeRight = GenerateLowerDimension();

        //block at 2dim
        if (min1DimensionCubeLeft == null || min1DimensionCubeRight == null)
        {
            GenerateSquare();
            return;
        }

        AddLowerDimensionVertices(min1DimensionCubeLeft, -0.5f * size);
        AddLowerDimensionVertices(min1DimensionCubeRight, 0.5f * size);
        Link2LowerDimensions();
        CollectLineGroups();
    }

    NDCube GenerateLowerDimension()
    {
        if(dimension <= 2)
        {
            return null;
        }

        int lowerDimension = dimension - 1;
        GameObject lowerDimGo = new GameObject("Dimension" + lowerDimension);
        lowerDimGo.transform.SetParent(transform);
        NDCube ncube = lowerDimGo.AddComponent<NDCube>();
        ncube.dimension = lowerDimension;

        ncube.GenerateNCube(root);

        return ncube;
    }

    void GenerateSquare()
    {
        float[] v1 = { -0.5f * size, -0.5f * size };
        float[] v2 = { -0.5f * size, 0.5f * size };
        float[] v3 = { 0.5f * size, 0.5f * size };
        float[] v4 = { 0.5f * size, -0.5f * size };

        vertices.Add(new VectorN(v1));
        vertices.Add(new VectorN(v2));
        vertices.Add(new VectorN(v3));
        vertices.Add(new VectorN(v4));

        LineGroup lineGroup = Instantiate(root.lineGroupPrefab, transform);
        lineGroup.indexTransform = new int[5];
        for (int i = 0; i < 4; i++)
        {
            lineGroup.indexTransform[i] = i;
        }
        lineGroup.indexTransform[4] = 0;

        lineGroups.Add(lineGroup);
    }

    void AddLowerDimensionVertices(NDCube min1DimensionCube, float newValue)
    {
        //Copy vertices from lower dimension, and add the newValue at the end of the vertex
        //if lower dimension = 2,  copy x y, and add z as newValue;

        for (int i = 0; i < min1DimensionCube.vertices.Count; i++)
        {
            VectorN newDimensionVertex = new VectorN(min1DimensionCube.vertices[i]);
            newDimensionVertex.AddDimension(newValue);
            Debug.Log(newValue);
            vertices.Add(newDimensionVertex);
        }
    }

    void Link2LowerDimensions()
    {
        GameObject connectionNull = new GameObject("Connection Null");
        connectionNull.transform.SetParent(transform);

        int min1Length = min1DimensionCubeLeft.vertices.Count;
        for (int i = 0; i < min1Length; i++)
        {
            LineGroup lineGroup = Instantiate(root.lineGroupPrefab, connectionNull.transform);
            lineGroup.indexTransform = new int[2];
            lineGroup.indexTransform[0] = i;
            lineGroup.indexTransform[1] = i + min1Length;
            lineGroups.Add(lineGroup);
        }
    }

    void CollectLineGroups()
    {
        //Add ups both linegroups
        lineGroups.AddRange(min1DimensionCubeLeft.lineGroups);
        lineGroups.AddRange(min1DimensionCubeRight.lineGroups);
        
        //off set the second one
        int count = min1DimensionCubeRight.lineGroups.Count;
        int offset = min1DimensionCubeRight.vertices.Count;

        for (int i = 0; i < count; i++)
        {
            min1DimensionCubeRight.lineGroups[i].ApplyOffsetToIndices(offset);
        }
    }

    void UpdateLineGroups()
    {
        //Vector3[] v3 = new Vector3[vertices.Count];
        //for (int i = 0; i < v3.Length; i++)
        //{
        //    v3[i] = vertices[i].ToVector3();
        //}

        for (int i = 0; i < lineGroups.Count; i++)
        {
            lineGroups[i].ApplyLineToGroup(rotatedVertices);
        }
    }
}

public class MatrixRotation5D
{
    public static Matrix RotationXW(float angle)
    {
        double cos = Mathf.Cos(angle);
        double sin = Mathf.Sin(angle);
        return new Matrix
        (
            new double[,]
            {
                    { cos, 0, 0, -sin ,0},
                    { 0  , 1, 0, 0    ,0},
                    { 0  , 0, 1, 0    ,0},
                    { sin, 0, 0, cos  ,0},
                    { 0  , 0, 0, 0    ,1},
            }
        );
    }

    public static Matrix RotationWV(float angle)
    {
        double cos = Mathf.Cos(angle);
        double sin = Mathf.Sin(angle);
        return new Matrix
        (
            new double[,]
            {
                    { 1, 0, 0, 0  ,0},
                    { 0, 1, 0, 0  ,0},
                    { 0, 0, 1, 0  ,0},
                    { 0, 0, 0, cos,-sin},
                    { 0, 0, 0, sin,cos},
            }
        );
    }
}

public class MatrixRotationND
{
    public static Matrix Rotation(float angle, int dimension, int rotateDim1, int rotateDim2)
    {
        double cos = Mathf.Cos(angle);
        double sin = Mathf.Sin(angle);

        double[,] matrix = new double[dimension, dimension];
        //Diagonal de 1
        for (int i = 0; i < dimension; i++)
        {
            matrix[i, i] = 1;
        }
        matrix[rotateDim1, rotateDim1] = cos;
        matrix[rotateDim1, rotateDim2] = -sin;
        matrix[rotateDim2, rotateDim2] = cos;
        matrix[rotateDim2, rotateDim1] = sin;

        return new Matrix (matrix);
    }

}