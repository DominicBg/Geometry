using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NDLineRendererController : MonoBehaviour {

    NDCube ndCube;

    [SerializeField] KeyCode keyCodeShake;
    [SerializeField] float shakeIntensity;
    [SerializeField] float shakeDuration;

    private void Start()
    {
        ndCube = GetComponent<NDCube>();
    }

    private void Update()
    {
        ChangeLineRendererDimension();

        if(Input.GetKeyDown(keyCodeShake))
        {
            GameEffect.Shake(transform.gameObject, shakeIntensity, shakeDuration);
        }
    }

    private void ChangeLineRendererDimension()
    {
        int dimension = GetDimensionKey();

        if (dimension == -1)
            return;

        ToggleDimension(ndCube, dimension);
    }

    void ToggleDimension(NDCube currentCube, int dimension)
    {
        if(currentCube.dimension == dimension)
        {
            bool isActive = currentCube.ConnectionNull.activeInHierarchy;
            currentCube.ConnectionNull.SetActive(!isActive);
        }
        else
        {
            ToggleDimension(currentCube.min1DimensionCubeLeft, dimension);
            ToggleDimension(currentCube.min1DimensionCubeRight, dimension);
        }
    }

    int GetDimensionKey()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            return 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            return 4;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            return 5;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            return 6;
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            return 7;
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            return 8;
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            return 9;
        }
        return -1;
    }
}