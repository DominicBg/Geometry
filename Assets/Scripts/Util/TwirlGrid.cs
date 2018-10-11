using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TwirlGrid
{

    [Header("Twist")]
    [SerializeField] float twistFactor = 1;
    [SerializeField] AnimationCurve twistOverDistanceCurve;
    [SerializeField] float maxTwist;

    [Header("Twist anim")]
    [SerializeField] Vector2 twistOffSet;
    [SerializeField] Vector2 twistSinAmp;
    [SerializeField] Vector2 twistSinFreq;
    [SerializeField] Vector2 twistSinFreqOffset;

    Vector2 internalTwistOffSet;
    Vector2[,] twirlMatrix;

    public Vector2 this[int iRow, int iCol]
    {
        get { return twirlMatrix[iRow, iCol]; }
    }

    public void UpdateTwirl(int rows, int cols)
    {
        RotatePointMatrix(rows, cols);
        UpdateTwistOffset();
    }

    private void RotatePointMatrix(int rows, int cols)
    {
        twirlMatrix = new Vector2[rows, cols];
        Vector2 middlePoint = new Vector2(rows * 0.5f, cols * 0.5f);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                Vector2 currentPoint = new Vector2(row + internalTwistOffSet.x, col + internalTwistOffSet.y);
                //Prend la distance du point par rapport au milieu
                //calcule t comme ratio de distance 
                //Twist par rapport a la distance
                float distanceFromCenter = (currentPoint - middlePoint).magnitude;
                float max = middlePoint.magnitude;
                float t = (Mathf.Min(distanceFromCenter, max) / max);
                float angle = Mathf.Lerp(0, maxTwist, 1 - twistOverDistanceCurve.Evaluate(t));
                twirlMatrix[row, col] = GameMath.RotateVector(angle * twistFactor, currentPoint - middlePoint);
            }
        }
    }

    private void UpdateTwistOffset()
    {
        internalTwistOffSet = twistOffSet + new Vector2(
            twistSinAmp.x * Mathf.Sin(Time.time * twistSinFreq.x + twistSinFreqOffset.x * Mathf.Deg2Rad),
            twistSinAmp.y * Mathf.Sin(Time.time * twistSinFreq.y + twistSinFreqOffset.y * Mathf.Deg2Rad));
    }
}
