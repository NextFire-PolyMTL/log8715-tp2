using Unity.Collections;
using UnityEngine;

public class Velocity : MonoBehaviour
{
    public Vector3 velocity;

    public NativeArray<Vector3> ConvertToArray() {
        var array = new NativeArray<Vector3>(1, Allocator.Persistent);
        array[0] = velocity;
        return array;
    }

    public void UpdateValues(NativeArray<Vector3> paramArray) {
        velocity = paramArray[0];
    }

    void Update()
    {
        transform.localPosition += velocity * Time.deltaTime;
    }
}
