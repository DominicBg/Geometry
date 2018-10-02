using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour {

    public enum FlockMode { Flat, InsideSphere, OnSphere}
    public FlockMode flockMode;
    public float aquariumSize;

    [SerializeField] int numberBoids;
    [SerializeField] Boids boidsPrefab;
    [SerializeField] BoidsDome boidsDomePrefab;

    [SerializeField] float spawnRadius;

    Dictionary<int, Boids> boidsDictionary = new Dictionary<int, Boids>();

    [SerializeField] Transform boidsContainer;
    [SerializeField] Transform boidsDomeContainer;

    private void Start()
    {
        for (int i = 0; i < numberBoids; i++)
        {
            Vector3 startPos;
            if (flockMode == FlockMode.InsideSphere)
                startPos = Random.insideUnitSphere * spawnRadius;
            else
                startPos = Random.insideUnitCircle * spawnRadius;

            Boids boids = Instantiate(boidsPrefab, startPos, Quaternion.identity);
            boids.transform.SetParent(boidsContainer);
            boidsDictionary.Add(boids.gameObject.GetInstanceID(), boids);

            if(flockMode == FlockMode.OnSphere)
            {
                InstantiateBoidsDome(boids);
            }

            boids.isSpherical = (flockMode == FlockMode.InsideSphere); //On unit sphere map la position 2D
            boids.flock = this;
        }
    }

    void InstantiateBoidsDome(Boids boids)
    {
        BoidsDome boidsDome = Instantiate(boidsDomePrefab, boids.transform.position, Quaternion.identity);
        boids.GetComponent<TrailRenderer>().enabled = false;

        boidsDome.boids = boids;
        boidsDome.transform.SetParent(boidsDomeContainer);
    }

    public Boids GetBoidsByID(int id)
    {
        if(boidsDictionary.ContainsKey(id))
        {
            return boidsDictionary[id];
        }
        else
        {
            //Debug.LogError("ERROR KEY IN DICTIONARY");
            return null;
        }
    }


}
