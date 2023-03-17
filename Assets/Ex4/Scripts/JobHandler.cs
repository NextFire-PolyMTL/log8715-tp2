using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;
/* Interface for other classes that will handle the Jobs calls */
public interface JobUpdater { void ApplyJob(); }


public static class JobHandler
{
    /* Job Handling the update of all lifetime entities */
    [BurstCompile(CompileSynchronously = true)]
    public struct LifeTimeJob : IJobParallelFor {
        /* The entity position */
        [ReadOnly] public NativeArray<Vector3> ownPos;
        /* Positions of the entities that can accelerate the dying process */
        [ReadOnly] public NativeArray<Vector3> accPos;
        /* Positions of the entities that can slow the dying process down*/
        [ReadOnly] public NativeArray<Vector3> slowPos;
        /* Positions of the of the same type for reproduction purposes */
        [ReadOnly] public NativeArray<Vector3> ownTypePos;
        /* An array of parameters, see Lifetime methods for more details*/
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

    /* Job handling the chasing behaviour of preys and predators */
    [BurstCompile(CompileSynchronously = true)]
    public struct ChaseJob : IJobParallelFor {
        public NativeArray<Vector3> ownVel;
        [ReadOnly] public NativeArray<Vector3> ownPos;
        [ReadOnly] public NativeArray<Vector3> chasedPos;
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

    //TODO : Say smthg about it's heavily inspired form unity doc ?
    [BurstCompile(CompileSynchronously = true)]
    public struct PositionJob : IJobParallelFor {
        [ReadOnly] public NativeArray<Vector3> velocity;

        public NativeArray<Vector3> position;
        [ReadOnly] public float deltaTime;


        public void Execute(int i) {
            position[i] += velocity[i] * deltaTime;
        }
    }

}
