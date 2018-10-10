﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PlantStem : MonoBehaviour {

    [SerializeField] Transform top;
    [SerializeField] int resolution;
    [SerializeField] SinAnimationMinMax stemLength;
    [SerializeField, Range(0, 1)] float ratioRandom = .9f;
    [SerializeField, Range(0, 1)] float ratioRandomAnimation = .99f;
    [SerializeField, Range(0, 1)] float upVectorInfluence = .1f;
    LineRenderer lineRenderer;

    Vector3 lastDirection;
    Vector3 lastPosition;

    Vector3[] positions;
    Vector3[] directions;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        CalculateStem();
        AlignTop();
    }

    private void OnValidate()
    {
        Start();
    }

    void CalculateStem()
    {
        CalculateDirections();
        CalculatePoisitions();
    }

    void CalculateDirections()
    {
        directions = new Vector3[resolution];
        directions[0] = new Vector3(0, 1, 0);
        for (int i = 1; i < resolution; i++)
        {
            directions[i] = RandomiseDirection(directions[i - 1], ratioRandom); ;
        }
    }

    void UpdateDirections()
    {
        for (int i = 1; i < resolution; i++)
        {
            directions[i] = RandomiseDirection(directions[i], ratioRandomAnimation); ;
        }
    }

    Vector3 RandomiseDirection(Vector3 direction, float randomness)
    {
        Vector3 partial = (randomness * direction);
        Vector3 random = ((1 - randomness) * Random.onUnitSphere);
        Vector3 up = Vector3.up * upVectorInfluence;
        return (partial + random + up).normalized;
    }

    void CalculatePoisitions()
    {
        positions = new Vector3[resolution];
        positions[0] = new Vector3(0, 0, 0);

        float fraction = (1.0f / resolution) * stemLength;

        for (int i = 1; i < resolution; i++)
        {
            positions[i] = positions[i - 1] + directions[i] * fraction;
        }
    }

    void SetPositions()
    {
        lineRenderer.positionCount = resolution;
        lineRenderer.SetPositions(positions);
    }

    void SetLastDirectionPosition()
    {
        lastDirection = directions[resolution - 1];
        lastPosition = positions[resolution - 1];
    }

    void AlignTop()
    {
        top.transform.position = lastPosition + transform.position;
        top.transform.up = lastDirection;
    }

    void Update ()
    {
        UpdateDirections();
        CalculatePoisitions();
        SetLastDirectionPosition();

        SetPositions();
        AlignTop();
    }
}
