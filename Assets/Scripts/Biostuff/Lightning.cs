using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : FractalTree
{
    int currentIndex = 0;
    enum LightningState { goingUp, goingDown, waitingBetween, waitingApex }
    LightningState state = LightningState.goingDown;

    [SerializeField] float timeAtApex;
    [SerializeField] float timeBetween;
    [SerializeField] int iterationPerFrame = 5;

    float currentTimer = 0;

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
                state = LightningState.waitingBetween;
                return;
            }
            if (currentIndex >= branches.Count)
            {
                currentTimer = 0;
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
