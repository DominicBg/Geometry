using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StemOffsetController : MonoBehaviour {


    [SerializeField] float startOffset;

    [SerializeField] float endOffset;

    // Use this for initialization
    void Start ()
    {
        PlantStem[] stems = GetComponentsInChildren < PlantStem > ();
        for (int i = 0; i < stems.Length; i++)
        {
            float t = (float)i / stems.Length;
            stems[i].SetStemLenghtOffset(Mathf.Lerp(startOffset, endOffset, t));
        }

    }
	
	
}
