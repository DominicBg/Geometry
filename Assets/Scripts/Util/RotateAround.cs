using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour {

    [SerializeField] Transform rotateAround;
    [SerializeField] float radius;
    [SerializeField] float speed;

    void Update()
    {
        transform.position = (transform.position - rotateAround.position).normalized * radius;

        transform.RotateAround(rotateAround.position, Vector3.up, speed * Time.deltaTime);
        //transform.LookAt(rotateAround);
    }
}
