using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meduse4D : FourDCubeController {

    enum Degree { _0, _45, _90, _180 };

    [Header("Pulse")]
    [SerializeField] Degree pulseDegre;
    [SerializeField] AnimationCurve pulseCurve;
    [SerializeField, Range(0,1)] float randomiseRatio;

    [Header("Animation")]
    [SerializeField] float fallingSpeed;
    [SerializeField] float jumpingHeigth;
    [SerializeField] AnimationCurve animationCurve;
    [SerializeField] Vector3 squishScale;
    [SerializeField] Vector3 squashScale;

    [Header("Tentacles")]
    [SerializeField] FourDimensionCube mainCube;
    [SerializeField] Transform[] tentaclesTransform;

    float animationValue;
    float pulseTimer = 0;
    int pulseDegreeTurnNumber = 0;


    private void Start()
    {
        base.Start();
        RandomiseAllValues();
    }


    private void Update()
    {
        UpdatePulseTimer();
        UpdateOffSet();
        UpdateTentaclePositions();

    }

    void RandomiseAllValues()
    {
        fallingSpeed = RandomiseValue(fallingSpeed);
        jumpingHeigth = RandomiseValue(jumpingHeigth);
        speed = RandomiseValue(speed);
    }

    float RandomiseValue(float value)
    {
        float valueRatio = value * randomiseRatio;
        return value + Random.Range(-valueRatio, valueRatio); 
    }

    void UpdateTentaclePositions()
    {
        Vector3[] positions = mainCube.verticesPositionsAfterRotation;

        if (positions == null)
            return;

        for (int i = 0; i < positions.Length; i++)
        {
            tentaclesTransform[i].localPosition = positions[i];
        }
    }

    void UpdatePulseTimer()
    {
        float degree = DegreeToFloat(pulseDegre);
        pulseTimer += Time.deltaTime * speed;

        if (pulseTimer > degree)
        {
            pulseDegreeTurnNumber++;
            if (pulseDegreeTurnNumber == NumberDegreeForFullTurn(pulseDegre))
            {
                pulseDegreeTurnNumber = 0;
            }
            pulseTimer -= degree;
        }

        //Remet sur la curve en degre
        float curvedPulse = pulseCurve.Evaluate(pulseTimer / degree);
        float additionalDegree = pulseDegreeTurnNumber * degree;

        //animationValue = 1 - Mathf.Abs((2 * curvedPulse) - 1);
        animationValue = animationCurve.Evaluate(pulseTimer / degree);
        angleOffset = Mathf.Lerp(angleOffsetMin, angleOffsetMax, animationValue);

        Animate();

        for (int i = 0; i < fourDCubes.Length; i++)
        {
            fourDCubes[i].timer = (((curvedPulse * degree) + additionalDegree) * Mathf.Deg2Rad);
        }
    }

    void Animate()
    {
        transform.localScale = Vector3.Lerp(squashScale, squishScale, animationValue);

        transform.position += (Vector3.down * Time.deltaTime) * fallingSpeed;
        transform.position += (transform.up * Time.deltaTime) * (animationValue * jumpingHeigth);

        //transform.RotateAround()
    }

    float DegreeToFloat(Degree degree)
    {
        if (degree == Degree._0)
            return 0;
        if (degree == Degree._45)
            return 45;
        if (degree == Degree._90)
            return 90;
        if (degree == Degree._180)
            return 180;
        return 0;
    }
    int NumberDegreeForFullTurn(Degree degree)
    {
        if (degree == Degree._0)
            return 0;
        if (degree == Degree._45)
            return 8;
        if (degree == Degree._90)
            return 4;
        if (degree == Degree._180)
            return 2;
        return 0;
    }
}
