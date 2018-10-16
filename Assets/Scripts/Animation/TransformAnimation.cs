using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformAnimation : MonoBehaviour {

    [SerializeField] bool animatePosition, animationEulerAngle, animateScale;
    [SerializeField] LerpVector3 position, eulerAngles, scale;

    private void Update()
    {
        if (animatePosition)
            transform.localPosition = position;
        if (animationEulerAngle)
            transform.localEulerAngles = eulerAngles;
        if (animateScale)
            transform.localScale = scale;
    }
}
