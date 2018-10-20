using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticuleFlock : Flock {

    ParticleSystem particleSystem;
    ParticleSystem.Particle[] particles;
    [SerializeField] Gradient gradientOverParticles;
    [SerializeField] float size;

    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        base.Start();
        SpawnParticles();
    }

    private void OnValidate()
    {
        if(particleSystem != null)
            SpawnParticles();
    }

    [ContextMenu("Reload particles")]
    void SpawnParticles()
    {
        particles = new ParticleSystem.Particle[numberBoids];
        for (int i = 0; i < numberBoids; i++)
        {
            float t = (float)i / numberBoids;

            particles[i] = new ParticleSystem.Particle();
            particles[i].startSize = size;
            particles[i].startColor = gradientOverParticles.Evaluate(t);
        }
        particleSystem.SetParticles(particles, numberBoids);
    }

    private void Update()
    {
        //SpawnParticles();

        for (int i = 0; i < boidsArray.Length; i++)
        {
            particles[i].position = boidsArray[i].transform.position;
        }
        particleSystem.SetParticles(particles, numberBoids);

    }
}
