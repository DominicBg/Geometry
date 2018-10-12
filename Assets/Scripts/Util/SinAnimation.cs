using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimationFloatNormalized  {
    public float amplitude = 1;
    public float frequency = 1;
    public float offset = 0;

    public float Calculate(float timer)
    {
        return amplitude * CalculateSin(timer);
    }
    public float Calculate()
    {
        return Calculate(Time.time);
    }

    protected float CalculateSin(float timer)
    {
        return Mathf.Sin(timer * frequency + offset * Mathf.Deg2Rad);
    }


    public static implicit operator float(AnimationFloatNormalized sin)
    {
        return sin.Calculate();
    }
}

[System.Serializable]
public class AnimationFloat : AnimationFloatNormalized
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
        return amplitude * Mathf.Lerp(min, max, currentT);
    }

    public static implicit operator float(AnimationFloat sin)
    {
        return sin.CalculateMinMax();
    }
}
[System.Serializable]
public class LinearAnimationFloat
{
    public float min = 0;
    public float max = 1;
    public float speed;
    public float currentT { get { return t; } }

    float t = 0;
    public float CalculateMinMax()
    {
        return CalculateMinMax(Time.time);
    }

    public float CalculateMinMax(float timer)
    {
        t = (timer * speed) % 2;

        t = (t > 1) ? 2 - t : t;
        return Mathf.Lerp(min, max, t);
    }

    public static implicit operator float(LinearAnimationFloat linearAnimation)
    {
        return linearAnimation.CalculateMinMax();
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