using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour {

    [SerializeField] SinFloat intensity1;
    [SerializeField] SinFloat intensity2;
    [SerializeField] SinFloat range1;
    [SerializeField] SinFloat range2;
    Light light;

    private void Start()
    {
        light = GetComponent<Light>();
    }

    private void Update()
    {
        light.intensity = (intensity1 + intensity2) / 2;
        light.range = (range1 + range2) / 2;
    }

}
