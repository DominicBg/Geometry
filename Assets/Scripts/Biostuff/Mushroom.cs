using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour {

    [SerializeField] int resolution;
    [SerializeField] AnimationCurve mushroomCurve;
    [SerializeField] SinFloat heigthAnimation;
    [SerializeField] float width;
    [SerializeField] bool inverseCurve;
    LineRenderer lineRender;
	// Use this for initialization
	void Start ()
    {
        lineRender = GetComponent<LineRenderer>();
    }

    Vector3 CalculateCirclePosition(int n, float y)
    {
        float t = ((float)n / resolution);
        float theta = t * Mathf.PI * 2;

        float curveT = mushroomCurve.Evaluate(y / heigthAnimation.max);
        float realCurveT = (inverseCurve) ? 1 - curveT : curveT;

        float realWidth = width * realCurveT;
        Vector2 coordinate = GameMath.PolarToCartesian(realWidth, theta);
        return new Vector3(coordinate.x, y, coordinate.y);
    }
	
	// Update is called once per frame
	void Update ()
    {
        float y = heigthAnimation.CalculateMinMax();
        lineRender.positionCount = resolution;
        for (int i = 0; i < resolution; i++)
        {
            lineRender.SetPosition(i, CalculateCirclePosition(i, y));
        }
    }
}
