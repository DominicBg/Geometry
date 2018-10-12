using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushtrailroom : MonoBehaviour
{
    [SerializeField] AnimationFloat heigthAnimation;
    [SerializeField] AnimationCurve shroomCurve;
    [SerializeField] float radius;
    [SerializeField] Transform drawer;
    [SerializeField] float phiSpeed;
    [SerializeField] float radiusCurveMultiplier;
    float theta;

    // Update is called once per frame
    void Update()
    {
        CalculatePosition();
    }

    void CalculatePosition()
    {
        float y = heigthAnimation.CalculateMinMax();
        theta += Time.deltaTime * phiSpeed;

        float radiusCurve = radiusCurveMultiplier * shroomCurve.Evaluate(heigthAnimation.currentT);
        Vector2 coordinate = GameMath.PolarToCartesian(radius + radiusCurve, theta);

        drawer.localPosition = new Vector3(coordinate.x, y, coordinate.y);
        //drawer.localPosition = GameMath.SphericalRotation(realRadius, phi, theta);
    }
}