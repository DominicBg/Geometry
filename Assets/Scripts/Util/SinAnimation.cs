using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SinFloatNormalized  {
    public float frequency = 1;
    public float offset = 0;

    public float Calculate(float timer)
    {
        return CalculateSin(timer);
    }

    public float Calculate()
    {
        return Calculate(Time.time);
    }

    protected float CalculateSin(float timer)
    {
        return Mathf.Sin(timer * frequency + offset * Mathf.Deg2Rad);
    }

    public static implicit operator float(SinFloatNormalized sin)
    {
        return sin.Calculate();
    }
}

[System.Serializable]
public class SinFloat : SinFloatNormalized
{
    public float min = 0;
    public float max = 1;

    /// <summary>
    /// Current t used to lerp with Sin, between 0 and 1
    /// </summary>
    public float currentT { get; private set; }

    public float CalculateMinMax()
    {
        return CalculateMinMax(Time.time);
    }

    public float CalculateMinMax(float timer)
    {
        currentT = (1 + CalculateSin(timer)) * .5f;
        return Mathf.Lerp(min, max, currentT);
    }

    public static implicit operator float(SinFloat sin)
    {
        return sin.CalculateMinMax();
    }
}
[System.Serializable]
public class LinearFloat
{
    public float min = 0;
    public float max = 1;
    public float speed;
    public float offset = 0;
    public bool loop;

    public float currentT { get { return t; } }

    float t = 0;
    public float CalculateMinMax()
    {
        return CalculateMinMax(Time.time + offset);
    }

    public float CalculateMinMax(float timer)
    {
        if (loop)
        {
            t = (timer * speed) % 2;
            t = (t > 1) ? 2 - t : t;
        }
        else
        {
            t = (timer * speed) % 1;
        }
        return CalculateLerp(t);
    }

    protected virtual float CalculateLerp(float t)
    {
        return Mathf.Lerp(min, max, t);
    }

    public static implicit operator float(LinearFloat linearAnimation)
    {
        return linearAnimation.CalculateMinMax();
    }
}

[System.Serializable]
public class CurvedFloat : LinearFloat
{
    public AnimationCurve curve;

    protected override float CalculateLerp(float t)
    {
        return Mathf.Lerp(min, max, curve.Evaluate(t));
    }
}

[System.Serializable]
public class PerlinOctave
{
    public float amplitude;
    public Vector2 seeds;
    public Vector2 frequency;
    public Vector2 speed;

    public float CalculatePerlin(float timer, float x, float y)
    {
        return amplitude * Mathf.PerlinNoise(seeds.x + timer * speed.x + x * frequency.x, seeds.y + timer * speed.y + y * frequency.y);
    }
    public float CalculatePerlin(float x, float y)
    {
        return CalculatePerlin(Time.time, x, y);
    }

    public static float CalculateOctaves(PerlinOctave[] octaves, float timer, float x, float y)
    {
        float result = 0;
        for (int i = 0; i < octaves.Length; i++)
        {
            result += octaves[i].CalculatePerlin(timer, x, y);
        }
        return result;
    }

    public static float CalculateOctaves(PerlinOctave[] octaves, float x, float y)
    {
        return CalculateOctaves(octaves, Time.time, x, y);
    }
}