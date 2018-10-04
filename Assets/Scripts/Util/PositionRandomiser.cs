using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionRandomiser : MonoBehaviour {
    [Header("Clamp position")]
    [SerializeField] bool useMinMax;
    [SerializeField] Vector3 min;
    [SerializeField] Vector3 max;

    [Header("Radius")]
    [SerializeField] bool useRadius;
    [SerializeField] float minRadius;
    [SerializeField] float maxRadius;

    // Use this for initialization
    void Start() {

        Vector3 newPosition = transform.position;


        if (useRadius) { 
            newPosition = Random.insideUnitSphere * maxRadius;
            if (newPosition.magnitude < minRadius)
                newPosition = newPosition.normalized * minRadius;
        }

        if(useRadius)
        {
            newPosition = ClampedValue(newPosition, min, max);
        }

        transform.position = newPosition;
    }
	
    Vector3 ClampedValue(Vector3 value, Vector3 min, Vector3 max)
    {
        return new Vector3(
            Mathf.Clamp(value.x, min.x, max.x),
            Mathf.Clamp(value.y, min.y, max.y),
            Mathf.Clamp(value.z, min.z, max.z));
    }

    // Update is called once per frame
    void Update () {
		
	}
}
