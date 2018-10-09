using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinGraph : Graph
{
    [SerializeField] float amplitude;
    [SerializeField] Vector2 speed;
    [SerializeField] Vector2 frequency;
    [SerializeField] Vector2 seeds;

    public override float CalculatePoint(float x, float y)
    {
        return amplitude * Mathf.PerlinNoise(seeds.x + Time.time * speed.x + x * frequency.x, seeds.y + Time.time * speed.y + y * frequency.y);
    }
}
