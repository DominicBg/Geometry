using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SinAnimation  {
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
}

[System.Serializable]
public class SinAnimationMinMax : SinAnimation
{
    public float min = 0;
    public float max = 1;

    public float CalculateMinMax()
    {
        return CalculateMinMax(Time.time);
    }

    public float CalculateMinMax(float timer)
    {
        float t = (1 + CalculateSin(timer)) * .5f;
        return amplitude * Mathf.Lerp(min, max, t);
    }
}
