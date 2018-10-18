using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularSeaController : CircularSea {

    [SerializeField] SinFloat animRadius;
    [SerializeField] SinFloat animSinAmplitude;
    [SerializeField] SinFloat animSinFrequency;
    [SerializeField] SinFloat animSubSinAmplitude;
    [SerializeField] SinFloat animSubsinFrequency;
    [SerializeField] SinFloat animDiffVectorSin;

    [SerializeField] LerpFloat animThetaOffset;

    private void Update()
    {
        radius = animRadius;
        sinAmplitude = animSinAmplitude;
        sinFrequency = animSinFrequency;
        subSinAmplitude = animSubSinAmplitude;
        subsinFrequency = animSubsinFrequency;
        diffVectorSin = animDiffVectorSin;
        thetaOffset = animThetaOffset;

        base.Update();
    }
}
