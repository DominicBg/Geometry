using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour {

    public float aquariumSize;

    [SerializeField] int numberBoids;
    [SerializeField] Boids boidsPrefab;
    [SerializeField] float spawnRadius;

    Dictionary<int, Boids> boidsDictionary = new Dictionary<int, Boids>();

    private void Start()
    {
        for (int i = 0; i < numberBoids; i++)
        {
            //Vector3 startPos = Random.insideUnitSphere * spawnRadius;
            Vector3 startPos = Random.insideUnitCircle * spawnRadius;

            Boids boids = Instantiate(boidsPrefab, startPos, Quaternion.identity);
            boids.transform.SetParent(transform);
            boidsDictionary.Add(boids.gameObject.GetInstanceID(), boids);

            boids.flock = this;
        }
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
