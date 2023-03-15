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
        var preysPos = JobHandler.GetPositons(SinulationMain.PreyTransforms);
        var paramArray = _lifetime.ConvertToArray();

        var job = new JobHandler.LifeChangeJob() {
            paramArray = paramArray,
            ownPos = transform.position,
            acceleratorsPos = preysPos,
            slowersPos = emptyArray,
            ownTypePos = emptyArray
        };

        JobHandle jobHandler = job.Schedule<JobHandler.LifeChangeJob>();
        jobHandler.Complete();
        _lifetime.UpdateValues(paramArray);

        /* Free native arrays used to avoid memory leak */
        emptyArray.Dispose();
        preysPos.Dispose();
        paramArray.Dispose();
    }
}
