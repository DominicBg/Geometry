using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinGraph : Graph
{
    [SerializeField] PerlinOctave[] perlinOctaves;

    public override float CalculatePoint(float x, float y)
    {
        return PerlinOctave.CalculateOctaves(perlinOctaves,x,y);
     }
}
