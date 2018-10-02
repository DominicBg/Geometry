using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidsDome : MonoBehaviour {

    [HideInInspector] public Boids boids;
    float TWOPI = Mathf.PI * 2;
    [SerializeField] float radius = 50;
    public void Update()
    {
        Transform2DToDomePosition();
    }

    void Transform2DToDomePosition()
    {
        Vector2 originalPosition = boids.transform.position;

        Vector2 positionRatio = originalPosition / boids.flock.aquariumSize;

        //float inverseAquariumSizeRatio = 1 / boids.flock.aquariumSize;
        float theta = Mathf.Lerp(0, TWOPI, positionRatio.y);
        float phi = Mathf.Lerp(0, TWOPI, positionRatio.x);

        //float theta = Mathf.Lerp(0, Mathf.PI, positionRatio.y);
        ///float phi = Mathf.Lerp(0,  Mathf.PI, positionRatio.x);


        Vector3 newPosition = GameMath.SphericalRotation(radius, phi, theta);
        transform.localPosition = newPosition;
    }
}
