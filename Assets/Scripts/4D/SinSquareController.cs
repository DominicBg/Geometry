using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinSquareController : MonoBehaviour {

    [SerializeField] float sinAmp;
    [SerializeField] float sinFreq;
    [SerializeField] Vector3 rotation;
    //rotate avec sin 

    void Update()
    {
        transform.eulerAngles += sinAmp * rotation * (Mathf.Max(0, Mathf.Cos(Time.time * sinFreq))) * Time.deltaTime;
    }
}
