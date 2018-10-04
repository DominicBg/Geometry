using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardRotateAround : MonoBehaviour {

    [SerializeField] Transform rotateAroundTransform;
    [SerializeField] float radius;
    [SerializeField] Vector3 offset;
    [SerializeField] Vector3 axis;
    private void Update()
    {
        Vector3 diff = (transform.position - rotateAroundTransform.position).SetY(0);
        float angle = Vector3.SignedAngle(rotateAroundTransform.forward, diff.normalized, Vector3.up);
        Debug.DrawRay(rotateAroundTransform.transform.position, diff);
        Debug.DrawRay(rotateAroundTransform.transform.position, rotateAroundTransform.forward*50);

        //Debug.Log(angle);
        transform.eulerAngles = axis * angle + offset;

    }
    // Update is called once per frame
    //void Update () {

    //       Vector3 nextPoint = RemapOnRadius(transform.position + transform.forward);
    //       transform.position = RemapOnRadius(transform.position);

    //       //Diff en x z seuelement;
    //       transform.LookAt(nextPoint);
    //   }

    //   Vector3 RemapOnRadius(Vector3 position)
    //   {
    //       Vector3 diff = (position - rotateAroundTransform.position);
    //       if (diff.magnitude != radius)
    //       {
    //           return rotateAroundTransform.position + diff;
    //       }
    //       return position;
    //   }
}
