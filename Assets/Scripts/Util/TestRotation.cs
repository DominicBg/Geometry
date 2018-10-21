using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRotation : MonoBehaviour {
    [SerializeField] float length;
    [SerializeField] float speed;
    [SerializeField] Vector3 baseVector;
    float angle;

    public bool X, Y, Z;

	// Update is called once per frame
	void Update ()
    {
        angle += Time.deltaTime * speed;
        Vector3 direction = baseVector.normalized * length;

        if(X)
        {
            direction = GameMath.RotateVectorX(angle, direction);
        }
        else if (Y)
        {
            direction = GameMath.RotateVectorY(angle, direction);
        }
        else if (Z)
        {
            direction = GameMath.RotateVectorZ(angle, direction);
        }
        Debug.DrawRay(transform.position, direction);
    }
}
