using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTransform : MonoBehaviour {

    [SerializeField] bool animatePosition, animateEulerAngles, animateScale;
    [SerializeField] LerpVector3 position;
    [SerializeField] LerpVector3 eulerAngles;
    [SerializeField] LerpVector3 scale;


    [SerializeField] AnimationTransform[] animationToStartAtEnd;

    [ContextMenu("Set Start Transform")]
    public void SetStartTransform()
    {
        Update();
    }
    [ContextMenu("Set End Transform")]

    public void SetEndTransform()
    {
        if(animatePosition)
            transform.localPosition = position.max;
        if(animateEulerAngles)
            transform.localEulerAngles = eulerAngles.max;
        if(animateScale)
            transform.localScale = scale.max;
    }

    [ContextMenu("Save as Start Transform")]
    public void SaveAsStartTransform()
    {
        position.min = transform.localPosition;
        eulerAngles.min = transform.localEulerAngles;
        scale.min = transform.localScale;

    }

    [ContextMenu("Save as End Transform")]
    public void SaveAsEndTransform()
    {
        position.max = transform.localPosition;
        eulerAngles.max = transform.localEulerAngles;
        scale.max = transform.localScale;
    }

    public void StartAnimation()
    {
        this.enabled = true;
        position.StartAnimation();
        eulerAngles.StartAnimation();
        scale.StartAnimation();
    }

    public void EndAnimation()
    {
        this.enabled = false;
        position.EndAnimation();
        eulerAngles.EndAnimation();
        scale.EndAnimation();
    }

    private void Start()
    {
        DisableAtEnd();
        SubsribeNextAnimation();
    }

    /// <summary>
    /// Quand L'anim finit, elle se disable
    /// </summary>
    private void DisableAtEnd()
    {
        if (animatePosition)
        {
            position.OnEndEvent.AddListener(() => enabled = false);
        }
        else if (animateEulerAngles)
        {
            eulerAngles.OnEndEvent.AddListener(() => enabled = false);
        }
        else if (animateScale)
        {
            scale.OnEndEvent.AddListener(() => enabled = false);
        }
    }

    private void SubsribeNextAnimation()
    {
        for (int i = 0; i < animationToStartAtEnd.Length; i++)
        {
            //Un des 3, pour pas subscribe 3 fois
            if (animatePosition)
            {
                SubscribeToEvent(position, animationToStartAtEnd[i]);
            }
            else if (animateEulerAngles)
            {
                SubscribeToEvent(eulerAngles, animationToStartAtEnd[i]);
            }
            else if (animateScale)
            {
                SubscribeToEvent(scale, animationToStartAtEnd[i]);
            }
        }
    }

    void SubscribeToEvent(LerpVector3 lerpVector3, AnimationTransform animationTransform)
    {
        lerpVector3.OnEndEvent.AddListener(animationTransform.StartAnimation);
    }

    // Update is called once per frame
    void Update ()
    {
        if(animatePosition)
            transform.localPosition = position;
        if(animateEulerAngles)
            transform.localEulerAngles = eulerAngles;
        if(animateScale)
            transform.localScale = scale;
	}
}
