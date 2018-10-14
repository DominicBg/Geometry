using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquishingAnimation : MonoBehaviour {

    [SerializeField] SinFloat animationSquish;

    public void Update()
    {
        float squish = animationSquish;
        transform.localScale = new Vector3(1- squish, 1+ squish, 1- squish);
    }
}
