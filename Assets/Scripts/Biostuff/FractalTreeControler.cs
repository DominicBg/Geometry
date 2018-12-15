using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractalTreeControler : FractalTree {

    [SerializeField] LerpFloat lengthAnimation;
    [SerializeField] LerpFloat angleAnimation;

    public void Update()
    {
        lengthFactorOverDepth = lengthAnimation;
        angleFactorOverDepth = angleAnimation;
        UpdateStorm();
    }
}
