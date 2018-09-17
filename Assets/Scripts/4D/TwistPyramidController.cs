using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwistPyramidController : MonoBehaviour {

    TwistPyramid[] twistPyramids;
    [SerializeField] Gradient gradient;
    [SerializeField] float angleOffset;
    [SerializeField] LerpSettings lerpSettings;

    private void Start()
    {
        twistPyramids = GetComponentsInChildren<TwistPyramid>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            LerpAngle();
    }

    [ContextMenu("UpdateSettings")]
    private void UpdateSettings()
    {
        if (twistPyramids == null || twistPyramids.Length != transform.childCount)
            twistPyramids = GetComponentsInChildren<TwistPyramid>();

        UpdateColors();
        UpdateOffset();
    }

    void UpdateColors()
    {
        for (int i = 0; i < twistPyramids.Length; i++)
        {
            float t = (1.0f / twistPyramids.Length) * i;
            twistPyramids[i].UpdateColor(gradient.Evaluate(t));
        }
    }
    void UpdateOffset()
    {
        for (int i = 0; i < twistPyramids.Length; i++)
        {
            twistPyramids[i].UpdateOffset(angleOffset * i);
        }
    }

    [ContextMenu("lerp angles")]
    public void LerpAngle()
    {
        StartCoroutine(LerpAngleAnimation(lerpSettings));
    }

    IEnumerator LerpAngleAnimation(LerpSettings lerpSettings)
    {
        float t = 0;
        float speed = 1 / lerpSettings.time;
        float angle = 0;

        while(t < 2)
        {
            t += Time.deltaTime * speed * 2;
            angle += Time.deltaTime;

            if(t < 1)
                angleOffset = Mathf.Lerp(lerpSettings.from, lerpSettings.to, t);
            else
                angleOffset = Mathf.Lerp(lerpSettings.to, lerpSettings.from, t-1);

            UpdateOffset();

            for (int i = 0; i < twistPyramids.Length; i++)
            {
                twistPyramids[i].UpdateAngle(angle);
            }

            yield return null;
        }
    }

    [System.Serializable]
    public class LerpSettings
    {
        public float from;
        public float to;
        public float time;
    }
}
