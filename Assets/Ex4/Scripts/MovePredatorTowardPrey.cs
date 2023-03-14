using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class MovePredatorTowardPrey : MonoBehaviour
{
    private Velocity _velocity;

    public void Start()
    {
        _velocity = GetComponent<Velocity>();
    }

    public void Update()
    {
        var closestDistance = float.MaxValue;
        var closestPosition = transform.position;
        foreach(var prey in Ex4Spawner.PreyTransforms)
        {
            var distance = Vector3.Distance(prey.position, transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPosition = prey.position;
            }
        }

        _velocity.velocity = (closestPosition - transform.position) * Ex4Config.PreySpeed;
    }
}
