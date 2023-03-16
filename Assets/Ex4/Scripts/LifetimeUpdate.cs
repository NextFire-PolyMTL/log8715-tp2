using Unity.Collections;
using UnityEngine;

public class LifeTimeUpdate //: JobUpdater
{
    public static NativeArray<Vector3> emptyArray = new NativeArray<Vector3>(0, Allocator.Persistent);

    public static void ApplyJob()
    {

    }
}
