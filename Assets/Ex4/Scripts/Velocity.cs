using Unity.Collections;
using UnityEngine;

public class Velocity : MonoBehaviour
{
    public Vector3 velocity;

    /* As for `Lifetime` parameters, we need an array to actually modify values
     * inside a job, so that function wraps velocoty in a `NativeArray` */
    public NativeArray<Vector3> ConvertToArray() {
        var array = new NativeArray<Vector3>(1, Allocator.Persistent);
        array[0] = velocity;
        return array;
    }

    /* Again as for `Lifetime` the attribute is updated via the array passed in
     * the job object */
    public void UpdateValues(NativeArray<Vector3> paramArray) {
        velocity = paramArray[0];
    }

    void Update()
    {
        transform.localPosition += velocity * Time.deltaTime;
    }
}
