using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourDCubeController : MonoBehaviour
{
    enum Degree { _0, _45, _90, _180 };

    FourDimensionCube[] fourDCubes;
    //[SerializeField] Gradient gradient;
    [SerializeField] float angleOffset;
    [SerializeField] float angleOffsetMin = 1;
    [SerializeField] float angleOffsetMax = 2;
    [SerializeField] float speed = 1;
    [SerializeField] float sinSpeed = 1;
    [SerializeField] float sinOffSet = 0;

    [Header("Pulse")]
    [SerializeField] bool enablePulseMode;
    [SerializeField] Degree pulseDegre;
    [SerializeField] AnimationCurve pulseCurve;

    [Header("Animation")]
    [SerializeField] float squishHeight;
    [SerializeField] AnimationCurve animationCurve;
    [SerializeField] Vector3 squishScale;
    [SerializeField] Vector3 squashScale;


    float animationValue;
    float pulseTimer = 0;
    int pulseDegreeTurnNumber = 0;

    private void Start()
    {
        fourDCubes = GetComponentsInChildren<FourDimensionCube>();
    }

    [ContextMenu("UpdateSettings")]
    private void UpdateSettings()
    {
        if (fourDCubes == null || fourDCubes.Length != transform.childCount)
            fourDCubes = GetComponentsInChildren<FourDimensionCube>();
    }

    private void Update()
    {

        if (enablePulseMode)
        {
            UpdatePulseTimer();
        }
        else
        {
        
            UpdateTimer();
        }

        UpdateOffSet();
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
            fourDCubes[i].timer = (((curvedPulse * degree)+ additionalDegree) * Mathf.Deg2Rad);
        }
    }

    void Animate()
    {
        transform.localScale = Vector3.Lerp(squashScale, squishScale, animationValue);

        for (int i = 0; i < fourDCubes.Length; i++)
        {
            fourDCubes[i].transform.localPosition = Vector3.Lerp(Vector3.zero, Vector3.up * squishHeight, animationValue);
        }
    }

    void UpdateOffSet()
    {
        for (int i = 0; i < fourDCubes.Length; i++)
        {
            fourDCubes[i].angleOffset = angleOffset * i;
        }
    }

    void UpdateTimer()
    {
        float sinT = (Mathf.Sin(Time.time * sinSpeed + sinOffSet) + 1) / 2;
        angleOffset = Mathf.Lerp(angleOffsetMin, angleOffsetMax, sinT);

        for (int i = 0; i < fourDCubes.Length; i++)
        {
            fourDCubes[i].timer += Time.deltaTime * speed;
        }
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
