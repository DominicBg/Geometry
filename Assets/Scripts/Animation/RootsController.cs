using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootsController : MonoBehaviour {

    [SerializeField] Gradient color1;
    [SerializeField] Gradient color2;

    Roots[] roots;

    void Start () {
        CalculateOffset();
    }

    [ContextMenu("Calculate offset")]
    public void CalculateOffset()
    {
        if (roots == null)
            roots = GetComponentsInChildren<Roots>();

        for (int i = 0; i < roots.Length; i++)
        {
            float ratio = ((float)i / roots.Length);
            roots[i].SetOffset(ratio * 360);
            TrailRenderer tr = roots[i].GetComponent<TrailRenderer>();

            tr.startColor = color1.Evaluate(ratio);
            tr.endColor = color2.Evaluate(ratio);
        }
    }
}
