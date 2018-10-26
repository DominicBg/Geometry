using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lightning : FractalTree
{
    int currentIndex = 0;
    enum LightningState { goingUp, goingDown, waitingBetween, waitingApex }
    LightningState state = LightningState.goingDown;

    [SerializeField] RandomFloat timeAtApex;
    [SerializeField] RandomFloat timeBetween;
    [SerializeField] int iterationPerFrame = 5;

    [SerializeField] UnityEvent OnNewLightning = new UnityEvent();

    float currentTimer = 0;

    private void Start()
    {
        timeAtApex.Randomise();
        timeBetween.Randomise();
        base.Start();
    }

    private void Update()
    {
        AnimateLightning();
    }

    void AnimateLightning()
    {
        for (int i = 0; i < iterationPerFrame; i++)
        {
            if (state == LightningState.goingUp)
            {
                branches[currentIndex].gameObject.SetActive(true);
                currentIndex++;
            }
            else if (state == LightningState.goingDown)
            {
                branches[currentIndex].gameObject.SetActive(false);
                currentIndex--;
            }

            if (currentIndex < 0)
            {
                currentTimer = 0;
                currentIndex = 0;
                timeBetween.Randomise();
                OnNewLightning.Invoke();
                state = LightningState.waitingBetween;
                return;
            }
            if (currentIndex >= branches.Count)
            {
                currentTimer = 0;
                timeAtApex.Randomise();
                currentIndex = branches.Count - 1;
                state = LightningState.waitingApex;
                return;
            }
        }

        if (state == LightningState.waitingApex)
        {
            currentTimer += Time.deltaTime;
            if (currentTimer > timeAtApex)
            {
                state = LightningState.goingDown;
            }
        }
        else if (state == LightningState.waitingBetween)
        {
            currentTimer += Time.deltaTime;
            if (currentTimer > timeBetween)
            {
                UpdateStorm();
                state = LightningState.goingUp;
            }
        }
    }
}
