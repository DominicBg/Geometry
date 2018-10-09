using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardRotateAround : MonoBehaviour {

    [SerializeField] Transform rotateAroundTransform;
    [SerializeField] Vector3 offset;
    [SerializeField] Vector3 axis;

    private void Update()
    {
        Vector3 diff = (transform.position - rotateAroundTransform.position).SetY(0);
        float angle = Vector3.SignedAngle(rotateAroundTransform.forward, diff.normalized, Vector3.up);
        Debug.DrawRay(rotateAroundTransform.transform.position, diff);
        Debug.DrawRay(rotateAroundTransform.transform.position, rotateAroundTransform.forward*50);

        transform.eulerAngles = axis * angle + offset;
    }
}
