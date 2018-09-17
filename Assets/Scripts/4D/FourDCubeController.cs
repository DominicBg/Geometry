using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourDCubeController : MonoBehaviour {

    FourDimensionCube[] fourDCubes;
    //[SerializeField] Gradient gradient;
    [SerializeField] float angleOffset;
    [SerializeField] float angleOffsetMin = 1;
    [SerializeField] float angleOffsetMax = 2;
    [SerializeField] float speed = 1;
    [SerializeField] float sinSpeed = 1;

    private void Start()
    {
        fourDCubes = GetComponentsInChildren<FourDimensionCube>();
    }

    [ContextMenu("UpdateSettings")]
    private void UpdateSettings()
    {
        if (fourDCubes == null || fourDCubes.Length != transform.childCount)
            fourDCubes = GetComponentsInChildren<FourDimensionCube>();

    }

    private void Update()
    {
        float sinT = (Mathf.Sin(Time.time * sinSpeed) + 1) / 2;
        angleOffset = Mathf.Lerp(angleOffsetMin, angleOffsetMax, sinT);
        UpdateOffset();
    }

    void UpdateOffset()
    {
        for (int i = 0; i < fourDCubes.Length; i++)
        {
            fourDCubes[i].angleOffset = angleOffset * i;
            fourDCubes[i].timer += Time.deltaTime * speed;
        }
    }
}
