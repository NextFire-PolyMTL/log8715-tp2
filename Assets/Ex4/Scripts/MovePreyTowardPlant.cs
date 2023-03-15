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
        /* Local arrays used for Job parameters */
        var plantsPos = JobHandler.GetPositons(SimulationMain.PlantTransforms);
        var paramArray = _velocity.ConvertToArray();

        var job = new JobHandler.MoveJob() {
            paramArray = paramArray,
            referenceSpeed = Ex4Config.PreySpeed,
            ownPos = transform.position,
            chasedPos = plantsPos
        };

        JobHandle jobHandler = job.Schedule<JobHandler.MoveJob>();
        jobHandler.Complete();
        _velocity.UpdateValues(paramArray);

        /* Free native arrays used to avoid memory leak */
        plantsPos.Dispose();
        paramArray.Dispose();
    }
}
