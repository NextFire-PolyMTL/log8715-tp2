using UnityEngine;
using Unity.Collections;
using Unity.Jobs;

public class ChangePredatorLifetime : MonoBehaviour
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
        var preysPos = JobHandler.GetPositons(Ex4Spawner.PreyTransforms);
        var predsPos = JobHandler.GetPositons(Ex4Spawner.PredatorTransforms);
        var paramArray = _lifetime.ConvertToArray();

        var job = new JobHandler.LifeChangeJob() {
            paramArray = paramArray,
            ownPos = transform.position,
            acceleratorsPos = emptyArray,
            slowersPos = preysPos,
            ownTypePos = predsPos
        };

        JobHandle jh = job.Schedule<JobHandler.LifeChangeJob>();
        jh.Complete();
        _lifetime.UpdateValues(paramArray);

        /* Free native arrays used to avoid memory leak */
        emptyArray.Dispose();
        preysPos.Dispose();
        predsPos.Dispose();
        paramArray.Dispose();
    }
}
