using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class MovePreyTowardPlant : MonoBehaviour
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
        foreach(var plant in Ex4Spawner.PlantTransforms)
        {
            var distance = Vector3.Distance(plant.position, transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPosition = plant.position;
            }
        }

        _velocity.velocity = (closestPosition - transform.position) * Ex4Config.PreySpeed;
    }
}
