using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractalStormController : FractalStorm {

    [Header("Animation")]
    [SerializeField] SinFloat totalAngle_anim;
    [SerializeField] SinFloat offsetAngle_anim;
    [SerializeField] SinFloat lengthFactorOverDepth_anim;
    [SerializeField] SinFloat angleFactorOverDepth_anim;

	// Update is called once per frame
	void Update ()
    {
        totalAngle = totalAngle_anim;
        offsetAngle = offsetAngle_anim;
        lengthFactorOverDepth = lengthFactorOverDepth_anim;
        angleFactorOverDepth = angleFactorOverDepth_anim;
        UpdateStorm();
    }
}
