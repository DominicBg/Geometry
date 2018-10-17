using UnityEngine;

public class Follower : MonoBehaviour
{
    public Transform objectToFollow;
    private void Awake()
    {
        transform.position = objectToFollow.position;
    }

    void Update ()
    {
	    transform.position = objectToFollow.position;
	}
}
