using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class LerpValue {

    public AnimationCurve curve;
    public float duration;
    public KeyCode startKeyCode = KeyCode.Space;

    public UnityEvent OnStartEvent = new UnityEvent();
    public UnityEvent OnEndEvent = new UnityEvent();

    private float timeStart;
    enum State { Waiting, Recording, Finished};
    State state;

    //Only Main value can invoke event
    protected enum ValueType { Main, Secondary};


    protected float GetFloatValue(float min, float max, ValueType valueType)
    {
        if (valueType == ValueType.Main && Input.GetKeyDown(startKeyCode))
        {
            StartAnimation();
        }

        if (state == State.Recording)
        {
            return Calculate(min, max, valueType);
        }
        else if(state == State.Finished)
        {
            return max;
        }
        else
        {
            return min;
        }
    }

    protected float Calculate(float min, float max, ValueType valueType )
    {
        float timeRecording = Time.time - timeStart;
        float t = timeRecording / duration;

        if(valueType == ValueType.Main && t > 1)
        {
            EndAnimation();
            t = 1;
        }
        return Mathf.Lerp(min, max, curve.Evaluate(t));
    }

    public void StartAnimation()
    {
        //Debug.Log("Start animation");
        OnStartEvent.Invoke();
        state = State.Recording;
        timeStart = Time.time;
    }

    public void EndAnimation()
    {
        //Debug.Log("End animation");
        OnEndEvent.Invoke();
        state = State.Finished;
    }
}

[System.Serializable]
public class LerpFloat : LerpValue
{
    public float min;
    public float max;
    public static implicit operator float(LerpFloat lerpFloat)
    {
        return lerpFloat.GetFloatValue(lerpFloat.min, lerpFloat.max, ValueType.Main);
    }
}

[System.Serializable]
public class LerpVector3 : LerpValue
{
    public Vector3 min;
    public Vector3 max;
    public static implicit operator Vector3(LerpVector3 lerpVector3)
    {
        float x = lerpVector3.GetFloatValue(lerpVector3.min.x, lerpVector3.max.x, ValueType.Main);
        float y = lerpVector3.GetFloatValue(lerpVector3.min.y, lerpVector3.max.y, ValueType.Secondary);
        float z = lerpVector3.GetFloatValue(lerpVector3.min.z, lerpVector3.max.z, ValueType.Secondary);
        return new Vector3(x, y, z);
    }
}
