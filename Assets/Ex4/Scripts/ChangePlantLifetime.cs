using UnityEngine;
using Unity.Collections;
using Unity.Jobs;

public class ChangePlantLifetime : MonoBehaviour
{
    private Lifetime _lifetime;

    public void Start()
    {
        _lifetime = GetComponent<Lifetime>();
    }

    public void Update()
    {
        var emptyArray = new NativeArray<Vector3>(0, Allocator.Persistent);
        var preysPos = JobHandler.GetPositons(Ex4Spawner.PreyTransforms);
        var paramArray = _lifetime.ConvertToArray();
        var job = new JobHandler.LifeChangeJob() {
            paramArray = paramArray,
            ownPos = transform.position,
            acceleratorsPos = preysPos,
            slowersPos = emptyArray,
            ownTypePos = emptyArray
        };

        JobHandle jh = job.Schedule<JobHandler.LifeChangeJob>();
        jh.Complete();
        _lifetime.UpdateValues(paramArray);

        /* Free native arrays used to avoid memory leak */
        paramArray.Dispose();
        emptyArray.Dispose();
        preysPos.Dispose();
    }
}
