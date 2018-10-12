using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhyllotaxisParticles : MonoBehaviour {

    [SerializeField] int numberOfParticles;

    [SerializeField] float sizeCurveScalar = 1;
    [SerializeField] AnimationCurve sizeCurve;
    [SerializeField] Gradient colorGradient;
    [SerializeField] AnimationFloat particleScaleSin;
    [SerializeField] AnimationFloat particleSizeSin;
    [SerializeField] AnimationFloat particleAngleSin;

    float angle = 137.5f;
    float scaling = 1;
    float particleSize = 1;

    ParticleSystem particleSystem;

	// Use this for initialization
	void Start ()
    {
        particleSystem = GetComponent<ParticleSystem>();
        CalculateFlower();
    }


    private void OnValidate()
    {
        particleSystem = GetComponent<ParticleSystem>();
        CalculateFlower();
    }
    [ContextMenu("Calculate flower")]
    public void CalculateFlower()
    {
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[numberOfParticles];
        for (int i = 0; i < numberOfParticles; i++)
        {
            float t = (float)i / numberOfParticles;
            particles[i] = new ParticleSystem.Particle();
            particles[i].position = GetPosition(i);
            particles[i].size = particleSize + sizeCurve.Evaluate(t) * sizeCurveScalar;
            particles[i].color = colorGradient.Evaluate(t);

        }
        particleSystem.SetParticles(particles, numberOfParticles);
    }

	// Update is called once per frame
	void Update () {

        scaling = particleScaleSin.CalculateMinMax();
        particleSize = particleSizeSin.CalculateMinMax();
        angle = particleAngleSin.CalculateMinMax();
        CalculateFlower();
    }

    Vector2 GetPosition(int n)
    {
        float r = scaling * Mathf.Sqrt(n);
        float theta = n * angle * Mathf.Deg2Rad;
        return GameMath.PolarToCartesian(r, theta);
    }
}
