using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using static JobHandler;

public class PositionUpdate
{
    public static void ApplyJob()
    {
        int preyCount = SimulationMain.PreyPos.Length;
        int predCount = SimulationMain.PredPos.Length;

        var preyJob = new PositionJob() {
            velocity = SimulationMain.PreyVel,
            position = SimulationMain.PreyPos,
            deltaTime = Time.deltaTime
        };

        var predJob = new PositionJob() {
            velocity = SimulationMain.PredVel,
            position = SimulationMain.PredPos,
            deltaTime = Time.deltaTime
        };

        JobHandle preyJH = preyJob.Schedule(preyCount, 64);
        JobHandle predJH = predJob.Schedule(predCount, 64);

        preyJH.Complete();
        predJH.Complete();
    }
}
