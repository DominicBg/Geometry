using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Connection : MonoBehaviour {

    public Neuron from;
    public Neuron to;

    LineRenderer lineRenderer;

    public Color disableColor;
    public Color enableColor;

    //public float sizeMin;
    //public float sizeMax;

    Coroutine animCoroutine;

    public void ConnectionFromTo()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, from.transform.position);
        lineRenderer.SetPosition(1, to.transform.position);

        lineRenderer.startColor = disableColor;
        lineRenderer.endColor = disableColor;
    }

    public void PlayAnimation()
    {
        from.Shrink();
        to.Growth();

        if (animCoroutine != null)
            StopCoroutine(animCoroutine);

        animCoroutine = StartCoroutine(LineRendererAnimation(.5f, 2.5f));
    }    

    IEnumerator LineRendererAnimation(float fadeInTime, float fadeOutTime)
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        float speed = 1 / fadeInTime;
        float t = 0;
        while(t < 1)
        {
            AnimateLinerenderer(t, true);
            t += Time.deltaTime * speed;
            yield return wait;
        }

        t = 0;
        speed = 1 / fadeOutTime;
        while (t < 1)
        {
            AnimateLinerenderer(t, false);
            t += Time.deltaTime * speed;
            yield return wait;
        }
    }

    void AnimateLinerenderer(float t, bool fadeIn)
    {
        if (!fadeIn)
            t = 1 - t;

        t = GameMath.Sigmoid01(t);

        Color color = Color.Lerp(disableColor, enableColor, t);
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        if (fadeIn)
        { 
            Vector3 endPosition = Vector3.Lerp(to.transform.position, from.transform.position, t);
            lineRenderer.SetPosition(0, endPosition);
        }
        //float width = Mathf.Lerp(sizeMin, sizeMax, t);
        //lineRenderer.startWidth = width;
        //lineRenderer.endWidth = width;
    }
}
