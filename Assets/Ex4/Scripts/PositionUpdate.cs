using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

/* Defines the Job Structure for Position update from velocity */
public class PositionUpdate {
	/* Job handlign the position update */
	[BurstCompile(CompileSynchronously = true)]
	public struct PositionJob : IJobParallelFor {
		/* Iterated on. The entity's velocity */
		[ReadOnly] public NativeArray<Vector3> velocity;
		/* Iterated on and modified. The entity's position */
		public NativeArray<Vector3> position;
		/* `Time.deltaTime` passed as an argument */
		[ReadOnly] public float deltaTime;

		public void Execute(int i) {
			position[i] += velocity[i] * deltaTime;
		}
	}

	public static void ApplyJob() {
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
