using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boids : MonoBehaviour {

    public Flock flock { get; set; }
    public Vector3 direction { get; private set; }

    [Header("Detection and mouvement")]
    [SerializeField] float speed = 1;
    [SerializeField] float detectRadius = 10;
    [SerializeField] float seperationRadius = 2;
    [SerializeField] float steerSpeed;
    [SerializeField] float delayCheckingCollision = 0.1f;

    [Header("Flock settings")]
    [SerializeField] float cohesionFactor = 1;
    [SerializeField] float alignementFactor = 1;
    [SerializeField] float separationFactor = 3;
    [SerializeField] float outOfAquariumFactor = 3;
    [SerializeField] float obstacleAvoidanceFactor = 3;

    Collider[] surrounding = new Collider[0];

    List<Boids> surroundingBoids = new List<Boids>();
    List<Collider> obstaclesList = new List<Collider>();

    float currentDelayCheckingCollision = 0;

    public bool isSpherical;

    private void Start()
    {
        if(isSpherical)
            direction = Random.onUnitSphere.normalized;
        else
            direction = Random.insideUnitCircle.normalized;

    }

    void Update ()
    {
        DetectSurrounding();
        CalculateDirection();

        MoveWithDirection();
        AlignWithDirection();
    }

    void DetectSurrounding()
    {
        currentDelayCheckingCollision += Time.deltaTime;

        if (currentDelayCheckingCollision >= delayCheckingCollision)
        { 
            surrounding = Physics.OverlapSphere(transform.position, detectRadius);

            surroundingBoids.Clear();
            obstaclesList.Clear();

            for (int i = 0; i < surrounding.Length; i++)
            {
                Boids boids = flock.GetBoidsByID(surrounding[i].gameObject.GetInstanceID());
                if (boids != null)
                {
                    surroundingBoids.Add(boids);
                }
                else
                {
                    obstaclesList.Add(surrounding[i]);
                }
            }

            currentDelayCheckingCollision -= delayCheckingCollision;
        }
    }

    private void CalculateDirection()
    {
        if (surrounding.Length == 0)
            return;

        Vector3 desiredDirection =
            CalculateCohesion()          * cohesionFactor +
            CalculateAlignement()        * alignementFactor +
            CalculateSeparation()        * separationFactor +
            CalculateOutOfAquarium()     * outOfAquariumFactor +
            CalculateObstacleAvoidance() * obstacleAvoidanceFactor;        

        direction = Vector3.Lerp(direction, desiredDirection, steerSpeed * Time.deltaTime).normalized;
    }
   
    Vector3 CalculateOutOfAquarium()
    {
        Vector3 direction = Vector3.zero;
        Vector3 diff = (flock.transform.position - transform.position);
        if (diff.magnitude > flock.aquariumSize)
        {
            direction = diff;
        }
        return direction;
    }


    Vector3 CalculateCohesion()
    {
        Vector3 centerOfMass = Vector3.zero;

        foreach(Boids boids in surroundingBoids)
        {
            centerOfMass += boids.transform.position;
        }
        centerOfMass /= surrounding.Length;

        Vector3 directionToCenterOfMass = (centerOfMass - transform.position).normalized;
        return directionToCenterOfMass;
    }

    Vector3 CalculateAlignement()
    {
        Vector3 velocityMean = Vector3.zero;

        foreach (Boids boids in surroundingBoids)
        {
            velocityMean += boids.direction;
        }
        velocityMean /= surrounding.Length;

        return velocityMean.normalized;
    }

    Vector3 CalculateSeparation()
    {
        Vector3 seperationMean = Vector3.zero;

        foreach (Boids boids in surroundingBoids)
        {
            float diff = (transform.position - boids.transform.position).magnitude;
            if (diff < seperationRadius)
            {
                seperationMean += AvoidDirection(boids.transform, diff);
            }
        }

        seperationMean /= surroundingBoids.Count;
        return seperationMean.normalized;
    }

    Vector3 CalculateObstacleAvoidance()
    {
        Vector3 avoidanceMean = Vector3.zero;
        foreach (Collider obstacle in obstaclesList)
        {
            float diff = (transform.position - obstacle.transform.position).magnitude;
            avoidanceMean += AvoidDirection(obstacle.transform, diff);
        }
        avoidanceMean /= surroundingBoids.Count;
        return avoidanceMean.normalized;
    }

    Vector3 AvoidDirection(Transform avoidObject, float diff)
    {
        float ratioSeperation = 1 - (diff / seperationRadius);
        float separationForce = (ratioSeperation * ratioSeperation);
        return (transform.position - avoidObject.position) * separationForce;
    }

    void AlignWithDirection()
    {
        transform.rotation = Quaternion.LookRotation(direction);
    }

    void MoveWithDirection()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
}
