using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignTransformChild : MonoBehaviour {

	[ContextMenu("Align")]
    public void Align()
    {
        Vector3 first = transform.GetChild(0).position;
        Vector3 last = transform.GetChild(transform.childCount - 1).position;

        for (int i = 0; i < transform.childCount; i++)
        {
            float t = (float)i / (transform.childCount - 1);
            Transform child = transform.GetChild(i);
            child.position = Vector3.Lerp(first, last, t);
        }
    }


}
