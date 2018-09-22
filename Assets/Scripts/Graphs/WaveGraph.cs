using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveGraph : Graph {

    [SerializeField] float amplitude;
    [SerializeField] float freqency;

    public override float CalculatePoint(float x, float y)
    {
        return amplitude*Mathf.Sin(x + Time.time* freqency) + amplitude*Mathf.Sin( y + Time.time* freqency);
    }

}
