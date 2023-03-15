using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class ChangePlantLifetime : MonoBehaviour
{
    private Lifetime _lifetime;

    public void Start()
    {
        _lifetime = GetComponent<Lifetime>();
    }

    public void Update()
    {
        /* Local arrays used for Job parameters */
        var emptyArray = new NativeArray<Vector3>(0, Allocator.Persistent);
        var paramArray = _lifetime.ConvertToArray();

        var job = new JobHandler.LifeChangeJob() {
            paramArray = paramArray,
            ownPos = transform.position,
            acceleratorsPos = Ex4Spawner.PreyPos,
            slowersPos = emptyArray,
            ownTypePos = emptyArray
        };

        JobHandle jobHandler = job.Schedule<JobHandler.LifeChangeJob>();
        jobHandler.Complete();
        _lifetime.UpdateValues(paramArray);
        for (int i = 0; i < Ex4Spawner.PlantTransforms.Length   ; ++i) { Ex4Spawner.PlantTransforms[i].position    = Ex4Spawner.PlantPos[i]; }
        for (int i = 0; i < Ex4Spawner.PreyTransforms.Length    ; ++i) { Ex4Spawner.PreyTransforms[i].position     = Ex4Spawner.PreyPos[i] ; }
        for (int i = 0; i < Ex4Spawner.PredatorTransforms.Length; ++i) { Ex4Spawner.PredatorTransforms[i].position = Ex4Spawner.PredPos[i] ; }
        /* Free native arrays used to avoid memory leak */
        emptyArray.Dispose();
        paramArray.Dispose();
    }
}
