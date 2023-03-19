using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

/* Defines the Job structure for chasing behaviour */
public class ChasingUpdate {

	/* Job handling the chasing behaviour of preys and predators */
	[BurstCompile(CompileSynchronously = true)]
	public struct ChaseJob : IJobParallelFor {
		/* Iterated on and modified. The entity's velocity */
		public NativeArray<Vector3> ownVel;
		/* Iterated on. The entity's position */
		[ReadOnly] public NativeArray<Vector3> ownPos;
		/* All the positions of the entity's of the type chased */
		[ReadOnly] public NativeArray<Vector3> chasedPos;
		/* The reference speed for the entity type considered */
		[ReadOnly] public float refSpeed;

		public void Execute(int i) {
			float closestDist = float.MaxValue;
			Vector3 closestPos = ownPos[i];
			foreach (var pos in chasedPos) {
				var dist = Vector3.Distance(pos, ownPos[i]);
				if (dist < closestDist) {
					closestDist = dist;
					closestPos = pos;
				}
			}
			ownVel[i] = (closestPos - ownPos[i]) * refSpeed;
		}
	}

	/* Gets all the velocities updated */
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
