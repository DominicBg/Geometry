using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snakes : MonoBehaviour {

    [SerializeField] float speed;
    [SerializeField] SinWave[] sinWaves;

    [SerializeField] Vector2 perlinSpeed;

    public TrailRenderer trailRenderer { get; private set; }
    Vector2 perlinValue = new Vector2(0, 0);
    float internalTimer = 0;

    void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        RandomiseValues();
    }

    private void RandomiseValues()
    {
        perlinValue = new Vector2(Random.Range(0, 100), Random.Range(0, 100));
        internalTimer = Random.Range(0, 100);
    }

    // Update is called once per frame
    void Update () {

        internalTimer += Time.deltaTime;
        perlinValue += perlinSpeed * Time.deltaTime;
        float perlin = 2 * Mathf.PerlinNoise(perlinValue.x, perlinValue.y) - 1;

        transform.eulerAngles += Vector3.up * perlin;
        Vector3 directionFoward = transform.forward * speed;
        Vector3 sinDirection = Vector3.zero;
        for (int i = 0; i < sinWaves.Length; i++)
        {
            sinDirection  += transform.up * sinWaves[i].amplitude * Mathf.Sin(sinWaves[i].frequency * internalTimer);
        }
        transform.position += directionFoward + sinDirection;
    }

    [System.Serializable]
    public class SinWave
    {
        public float amplitude;
        public float frequency;
    }
}
