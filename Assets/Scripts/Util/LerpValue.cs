using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class LerpValue {
    //public float min;
    //public float max;
    public AnimationCurve curve;
    public float duration;
    public KeyCode startKeyCode = KeyCode.Space;

    public UnityEvent OnStartEvent = new UnityEvent();
    public UnityEvent OnEndEvent = new UnityEvent();

    private float timeStart;
    enum State { Waiting, Recording, Finished};

    State state;

    public void StartAnimation()
    {
        OnStartEvent.Invoke();
        state = State.Recording;
        timeStart = Time.time;
    }

    protected float GetFloatValue(float min, float max)
    {
        if(Input.GetKeyDown(startKeyCode))
        {
            StartAnimation();
        }

        if(state == State.Recording)
        {
            return Calculate(min, max);
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

    protected float Calculate(float min, float max)
    {
        float timeRecording = Time.time - timeStart;
        float t = timeRecording / duration;

        if(t > 1)
        {
            Debug.Log("End animation");
            OnEndEvent.Invoke();
            state = State.Finished;
            t = 1;
        }
        return Mathf.Lerp(min, max, curve.Evaluate(t));
    }
}

[System.Serializable]
public class LerpFloat : LerpValue
{
    public float min;
    public float max;
    public static implicit operator float(LerpFloat lerpFloat)
    {
        return lerpFloat.GetFloatValue(lerpFloat.min, lerpFloat.max);
    }
}

[System.Serializable]
public class LerpVector3 : LerpValue
{
    public Vector3 min;
    public Vector3 max;
    public static implicit operator Vector3(LerpVector3 lerpVector3)
    {
        float x = lerpVector3.GetFloatValue(lerpVector3.min.x, lerpVector3.max.x);
        float y = lerpVector3.GetFloatValue(lerpVector3.min.y, lerpVector3.max.y);
        float z = lerpVector3.GetFloatValue(lerpVector3.min.z, lerpVector3.max.z);
        return new Vector3(x, y, z);
    }
}
