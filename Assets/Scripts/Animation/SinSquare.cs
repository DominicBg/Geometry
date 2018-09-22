using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinSquare : MonoBehaviour {

    LineRenderer[] lineRenderers;

    Vector3[] vertices;
    [SerializeField] int resolution = 10;
    [SerializeField] float sinAmplitude = 1;
    [SerializeField] float sinFreq = 10;

    [SerializeField] float animationSinFreq = 10;

    [SerializeField] float sinOffsetMin = 0;
    [SerializeField] float sinOffsetMax = 10;


    private void Start()
    {
        lineRenderers = GetComponentsInChildren<LineRenderer>();

        vertices = new Vector3[]
        {
            new Vector3(-1,1,0),
            new Vector3(1, 1, 0),
            new Vector3(1, -1, 0),
            new Vector3(-1, -1, 0),
        };
    }


    // Update is called once per frame
    void Update ()
    {
        UpdateLineRenderer();
    }
    void UpdateLineRenderer()
    {
        float sinAnimationT = (1 + Mathf.Sin(Time.time * animationSinFreq)) * 0.5f;
        float realSinOffset = Mathf.Lerp(sinOffsetMin, sinOffsetMax, sinAnimationT);

        for (int i = 0; i < lineRenderers.Length; i++)
        {
            lineRenderers[i].positionCount = resolution;
            Vector3 startPos = vertices[i];
            Vector3 endPos = vertices[(i + 1) % lineRenderers.Length];

            //For orthogonal vector (local up)
            Vector3 nextPos = vertices[(i + 2) % lineRenderers.Length];

            for (int j = 0; j < resolution; j++)
            {
                float t = (float)j / resolution;
                Vector3 pos = Vector3.Lerp(startPos, endPos, t);

                Vector3 localUp = endPos - nextPos;
                float sinT = Mathf.Sin(Time.time * sinFreq + j * realSinOffset);
                Vector3 sinPos = Vector3.Lerp(pos, pos + localUp  * sinAmplitude, sinT);
                lineRenderers[i].SetPosition(j, sinPos);
            }
        }
    }

}
