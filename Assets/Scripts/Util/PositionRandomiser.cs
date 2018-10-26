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
    private void Awake()
    {
        RandomPosition();
    }

    [ContextMenu("Random Position")]
    public void RandomPosition()
    {
        Vector3 newPosition = transform.position;
        
        if (useRadius)
        { 
            newPosition = Random.insideUnitSphere * maxRadius;
            if (newPosition.magnitude < minRadius)
                newPosition = newPosition.normalized * minRadius;
        }

        if(useMinMax)
        {
            newPosition = ClampedValue(newPosition, min, max);
        }

        transform.position = newPosition;
    }
	
    Vector3 ClampedValue(Vector3 value, Vector3 min, Vector3 max)
    {
        float x = Random.Range(min.x, max.x);
        float y = Random.Range(min.y, max.y);
        float z = Random.Range(min.z, max.z);


        return new Vector3(x, y, z);
    }

}
