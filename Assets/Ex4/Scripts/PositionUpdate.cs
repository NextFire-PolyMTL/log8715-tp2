using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using static JobHandler;

public class PositionUpdate : MonoBehaviour, JobUpdater
{
    public void ApplyJob()
    {
        PositionJob preyJob = new PositionJob() {
            velocity = SimulationMain.PreyVel,
            position = SimulationMain.PreyPos,
            deltaTime = Time.deltaTime
        };
        PositionJob predJob = new PositionJob() {
            velocity = SimulationMain.PredVel,
            position = SimulationMain.PredPos,
            deltaTime = Time.deltaTime
        };
        JobHandle jh1 = preyJob.Schedule(SimulationMain.PreyPos.Length, 64);
        JobHandle jh2 = predJob.Schedule(SimulationMain.PredPos.Length, 64);
        jh1.Complete();
        jh2.Complete();
    }
}
