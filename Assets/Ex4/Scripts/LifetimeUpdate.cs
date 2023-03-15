using Unity.Collections;
using UnityEngine;

public class LifeTimeUpdate : MonoBehaviour, JobUpdater
{
    private Lifetime _lifetime;
    public static NativeArray<Vector3> emptyArray;

    void Start() {
        emptyArray = new NativeArray<Vector3>(0, Allocator.Persistent);
        _lifetime = GetComponent<Lifetime>();
    }

    public void ApplyJob()
    {

    }
}
