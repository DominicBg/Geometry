using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularSeaAnimation : CircularSea {

    [SerializeField] SinFloat animTheta;

	void Update ()
    {
        thetaOffset = animTheta;
        base.Update();
	}
}
