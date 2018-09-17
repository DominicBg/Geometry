using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourDimensionCube : Rotation4D
{
    [SerializeField] Color startColor;
    [SerializeField] Color endColor;
    [SerializeField] Color highColor;
    [SerializeField] Color lowColor;


    LineGroup[] lineGroups;
    int dimension = 4;

    void Start()
    {
        Calculate();
        lineGroups = GetComponentsInChildren<LineGroup>();
    }

    void Update()
    {
        if (verticesMatrixPosition != null)
        {
            base.Rotate();
            UpdateLineRenderer();
        }
    }

    void UpdateLineRenderer()
    {
        for (int i = 0; i < lineGroups.Length; i++)
        {
            lineGroups[i].ApplyLineToGroup(verticesPositionsAfterRotation);

            float x = angle + Time.time;
            float y = (float)i * lineGroups.Length + Time.time ;
            float t = Mathf.PerlinNoise(x, y);

            Color realStartColor = Color.Lerp(startColor, lowColor, t);
            Color realEndColor = Color.Lerp(endColor, highColor, t);
            lineGroups[i].ApplyColor(realStartColor, realEndColor);
        }
    }

    protected override void Calculate()
    {
        verticesMatrixPosition = MatrixVerticeGenerator.GenerateCubeVertices(dimension);
    }    
}