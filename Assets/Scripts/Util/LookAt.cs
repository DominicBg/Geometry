using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour {

    [SerializeField] Transform lookAt;
    [SerializeField] Vector3 offSet;
	// Update is called once per frame
	void Update () {
        transform.LookAt(lookAt);
        transform.eulerAngles += offSet;

    }
}
