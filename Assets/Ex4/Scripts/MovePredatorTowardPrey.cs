﻿using Unity.Collections;
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
        /* Local arrays used for Job parameters */
        var preysPos = JobHandler.GetPositons(Ex4Spawner.PreyTransforms);
        var paramArray = _velocity.ConvertToArray();

        var job = new JobHandler.MoveJob() {
            paramArray = paramArray,
            referenceSpeed = Ex4Config.PreySpeed,
            ownPos = transform.position,
            chasedPos = preysPos
        };

        JobHandle jh = job.Schedule<JobHandler.MoveJob>();
        jh.Complete();
        _velocity.UpdateValues(paramArray);

        /* Free native arrays used to avoid memory leak */
        preysPos.Dispose();
        paramArray.Dispose();
    }
}
