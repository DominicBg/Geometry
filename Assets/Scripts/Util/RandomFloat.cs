using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct RandomFloat {
    [SerializeField] float min;
    [SerializeField] float max;
    public float value { get; private set; }

    public float Randomise()
    {
        value = Random.Range(min, max);
        return value;
    }

    public static implicit operator float(RandomFloat randomFloat)
    {
        return randomFloat.value;
    }
}
