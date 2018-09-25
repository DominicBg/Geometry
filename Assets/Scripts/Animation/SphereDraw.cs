using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereDraw : MonoBehaviour {

    [SerializeField] float minRadius;
    [SerializeField] float maxRadius;
    [SerializeField] float sinRadiusFrequency;

    [SerializeField] Transform drawer;
    [SerializeField] float thetaSpeed;
    [SerializeField] float phiSpeed;

    float radius;
    float phi, theta;
    float TWOPI = Mathf.PI * 2;
    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        CalculatePosition();
        float t = (Mathf.Sin(Time.time * sinRadiusFrequency) + 1) * 0.5f;
        radius = Mathf.Lerp(minRadius, maxRadius, t);
    }

    void CalculatePosition()
    {
        theta += Time.deltaTime * thetaSpeed;
        phi += Time.deltaTime * phiSpeed;

        if (theta > TWOPI)
            theta -= TWOPI;
        if (theta > TWOPI)
            theta -= TWOPI;

        drawer.localPosition = GameMath.SphericalRotation(radius, theta, phi);
    }
}
