using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using static JobHandler;

public class LifeTimeUpdate
{
    public static NativeArray<Vector3> emptyArray = new NativeArray<Vector3>(0, Allocator.Persistent);

    public static void ApplyJob()
    {
        int plantCount = SimulationMain.PlantPos.Length;
        int preyCount = SimulationMain.PreyPos.Length;
        int predCount = SimulationMain.PredPos.Length;
        var emptyArray =  new NativeArray<Vector3>(0, Allocator.TempJob);

        var jobPlant = new LifeTimeJob() {
            ownPos = SimulationMain.PlantPos,
            accPos = SimulationMain.PreyPos,
            slowPos = emptyArray,
            ownTypePos = emptyArray,
            paramArray = SimulationMain.PlantLTParams,
            touchDist = Ex4Config.TouchingDistance
        };
        var jobPrey = new LifeTimeJob() {
            ownPos = SimulationMain.PreyPos,
            accPos = SimulationMain.PredPos,
            slowPos = SimulationMain.PlantPos,
            ownTypePos = SimulationMain.PredPos,
            paramArray = SimulationMain.PreyLTParams,
            touchDist = Ex4Config.TouchingDistance
        };
        var jobPred = new LifeTimeJob() {
            ownPos = SimulationMain.PredPos,
            accPos = emptyArray,
            slowPos = SimulationMain.PreyPos,
            ownTypePos = SimulationMain.PredPos,
            paramArray = SimulationMain.PredLTParams,
            touchDist = Ex4Config.TouchingDistance
        };

        var JobHandlerPlant = jobPlant.Schedule<LifeTimeJob>(plantCount, 64);
        var JobHandlerPrey  = jobPrey.Schedule< LifeTimeJob>(preyCount , 64);
        var JobHandlerPred  = jobPred.Schedule< LifeTimeJob>(predCount , 64);

        JobHandlerPlant.Complete();
        JobHandlerPrey.Complete();
        JobHandlerPred.Complete();

        emptyArray.Dispose();
    }
}
