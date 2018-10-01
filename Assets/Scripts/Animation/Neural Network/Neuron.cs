using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Neuron : MonoBehaviour {

    public Connection[] incomingConnections;

    SpriteRenderer spriteRenderer;
     float growthFactor = 0.15f;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.LookAt(Camera.main.transform);
    }

    public void PlayAnimationWithConnection(int connectionIndex)
    {
        incomingConnections[connectionIndex].PlayAnimation();
    }

    public void Growth()
    {
        PlayAnimSize(growthFactor);
    }
    public void Shrink()
    {
        PlayAnimSize(-growthFactor);
    }

    void PlayAnimSize(float deltaSize)
    {
        transform.localScale += Vector3.one * deltaSize;
        //transform.DOScale(transform.localScale + Vector3.one * deltaSize, 0.1f);
    }
}
