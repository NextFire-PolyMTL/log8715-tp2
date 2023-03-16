using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;

public class JobHandler : MonoBehaviour
{
    public static float touchDist = Ex4Config.TouchingDistance;

    /* Transforms an array of `Tranform` into a `NativeArray` of `Vector3`
     * to be compatible with `IJob` requierments */
    public static NativeArray<Vector3> GetPositons(Transform[] tList) {
        int size = tList.Length;
        var positions = new NativeArray<Vector3>(size, Allocator.Persistent);
        for (int i = 0; i < size; ++i) {
            positions[i] = tList[i].position;
        }
        return positions;
    }

    /* Job handling the Lifetime of all entities */
    [BurstCompile(CompileSynchronously = true)]
    public struct LifeChangeJob : IJob {
        /* An array of parameters, see Lifetime methods for more details*/
        public NativeArray<float> paramArray;
        /* The entity position */
        [ReadOnly] public Vector3 ownPos;
        /* Positions of the entities that can accelerate the dying process */
        [ReadOnly] public NativeArray<Vector3> acceleratorsPos;
        /* Positions of the entities that can slow the dying process down*/
        [ReadOnly] public NativeArray<Vector3> slowersPos;
        /* Positions of the of the same type for reproduction purposes */
        [ReadOnly] public NativeArray<Vector3> ownTypePos;
        // The code actually running on the job

        public void Execute()
        {
            paramArray[0] = 1.0f;
            foreach (var pos in acceleratorsPos) {
                if (Vector3.Distance(pos, ownPos) < paramArray[3]) {
                    paramArray[0] *= 2f;
                    break;
                }
            }
            foreach (var pos in slowersPos) {
                if (Vector3.Distance(pos, ownPos) < paramArray[3]) {
                    paramArray[0] /= 2f;
                    break;
                }
            }
            /* Float equality safety
             * Also cuts execution time by checking if the flag is already
             * activated in for the entity */
            if (Mathf.Approximately(paramArray[1], 1.0f)) return;

            foreach (var pos in ownTypePos) {
                float dist = Vector3.Distance(pos, ownPos);
                if (!Mathf.Approximately(dist, 0) && dist < paramArray[2]) {
                    paramArray[1] = 1.0f;
                    break;
                }
            }
        }
    }

    /* Job handling the entities movement */
    [BurstCompile(CompileSynchronously = true)]
    public struct MoveJob : IJob {
        public NativeArray<Vector3> paramArray;
        /* Either predator or prey speed defined in config*/
        [ReadOnly] public float referenceSpeed;
        [ReadOnly] public Vector3 ownPos;
        [ReadOnly] public NativeArray<Vector3> chasedPos;
        public void Execute() {
            var closestDist = float.MaxValue;
            var closestPos = ownPos;
            foreach (var pos in chasedPos) {
                var dist = Vector3.Distance(pos, ownPos);
                if (dist < closestDist) {
                    closestDist = dist;
                    closestPos = pos;
                }
            }
            paramArray[0] = (closestPos - ownPos) * referenceSpeed;
        }
    }


}
