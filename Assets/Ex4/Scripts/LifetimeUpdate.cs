using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

/* Defines the Job Structure for Lifetime management */
public class LifeTimeUpdate {

	/* Job Handling the update of all lifetime entities */
	[BurstCompile(CompileSynchronously = true)]
	public struct LifeTimeJob : IJobParallelFor {
		/* Iterated on. The entity position */
		[ReadOnly] public NativeArray<Vector3> ownPos;
		/* Positions of the entities that can accelerate the dying process */
		[ReadOnly] public NativeArray<Vector3> accPos;
		/* Positions of the entities that can slow the dying process down*/
		[ReadOnly] public NativeArray<Vector3> slowPos;
		/* Positions of the of the same type for reproduction purposes */
		[ReadOnly] public NativeArray<Vector3> ownTypePos;
		/* Iterated on and modified. The parameters of the `Lifetime` class */
		public NativeArray<LTParams> paramArray;
		/* The touching distance defined on the config*/
		[ReadOnly] public float touchDist;

		public void Execute(int i) {
			var newParams = paramArray[i];
			newParams.decrFactor = 1.0f;
			/* Checking contact with enemies*/
			foreach (var pos in accPos) {
				if (Vector3.Distance(pos, ownPos[i]) < touchDist) {
					newParams.decrFactor *= 2f;
					break;
				}
			}
			/* Checking contact with food */
			foreach (var pos in slowPos) {
				if (Vector3.Distance(pos, ownPos[i]) < touchDist) {
					newParams.decrFactor /= 2f;
					break;
				}
			}
			/* Skips check if the reproduce flag is already activated since
				* it cannot be toggled off */
			if (newParams.reproduceFlag) {
				paramArray[i] = newParams;
				return;
			}
			/* Checking reproduce flag with own species while being sure the
				* entity doesn't reproduce with itself */
			foreach(var pos in ownTypePos) {
				float dist = Vector3.Distance(pos, ownPos[i]);
				if (!Mathf.Approximately(dist, 0f) && dist < touchDist) {
					newParams.reproduceFlag = true;
					break;
				}
			}
			paramArray[i] = newParams;
		}
	}


	public static void ApplyJob() {
		int plantCount = SimulationMain.PlantPos.Length;
		int preyCount = SimulationMain.PreyPos.Length;
		int predCount = SimulationMain.PredPos.Length;
		/* Special variable used for entity types that e.g. don't have another
		 * entity that can slow down their dying process. So this array skips
		 * the loop while being an acceptable parameter for the job */
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

		/* Avoids memory leaks with this array */
		emptyArray.Dispose();
	}
}
