using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using static JobHandler;

public class ChasingUpdate {
    public static NativeArray<Vector3> emptyArray = new NativeArray<Vector3>(0, Allocator.Persistent);


    public static void ApplyJob() {
        int preyCount = SimulationMain.PreyPos.Length;
        int predCount = SimulationMain.PredPos.Length;

        var preyJob = new ChaseJob() {
            ownVel = SimulationMain.PreyVel,
            ownPos = SimulationMain.PreyPos,
            chasedPos = SimulationMain.PlantPos,
            refSpeed = Ex4Config.PreySpeed
        };
        var predJob = new ChaseJob() {
            ownVel = SimulationMain.PredVel,
            ownPos = SimulationMain.PredPos,
            chasedPos = SimulationMain.PreyPos,
            refSpeed = Ex4Config.PredatorSpeed
        };

        var preyJH = preyJob.Schedule<ChaseJob>(preyCount, 64);
        var predJH = predJob.Schedule<ChaseJob>(predCount, 64);

        preyJH.Complete();
        predJH.Complete();
    }
}
