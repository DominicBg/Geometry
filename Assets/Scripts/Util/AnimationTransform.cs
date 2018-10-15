using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTransform : MonoBehaviour {

    [SerializeField] bool animatePosition, animateEulerAngles, animateScale;
    [SerializeField] LerpVector3 position;
    [SerializeField] LerpVector3 eulerAngles;
    [SerializeField] LerpVector3 scale;


    // Update is called once per frame
    void Update () {
        if(animatePosition)
            transform.localPosition = position;
        if(animateEulerAngles)
            transform.localEulerAngles = eulerAngles;
        if(animateScale)
            transform.localScale = scale;
	}
}
