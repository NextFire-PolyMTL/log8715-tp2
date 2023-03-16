using Unity.Collections;
using UnityEngine;

public class Lifetime : MonoBehaviour
{
    private const float StartingLifetimeLowerBound = 5;
    private const float StartingLifetimeUpperBound = 15;

    public float decreasingFactor = 1;
    public bool alwaysReproduce;
    public bool reproduced;

    private float _startingLifetime;
    private float _lifetime;

    public float GetProgression()
    {
        return _lifetime / _startingLifetime;
    }

    void Start()
    {
        reproduced = false;
        _startingLifetime = Random.Range(StartingLifetimeLowerBound, StartingLifetimeUpperBound);
        _lifetime = _startingLifetime;
    }

    /* Stores some attributes in a `NativeArray` of `float` since `IJob`
     * unfortunately can't take reference to attributes */
    public NativeArray<float> ConvertToArray() {
        var array = new NativeArray<float>(3, Allocator.Persistent);
        array[0] = decreasingFactor;
        array[1] = (reproduced | alwaysReproduce) ? 1.0f : 0.0f;
        array[2] = JobHandler.touchDist;
        return array;
    }

    /* Performs the symetrical operation of `ConvertToArray` by updating
     * the class attributes with new values after the job got handled */
    public void UpdateValues(NativeArray<float> paramArray) {
        decreasingFactor = paramArray[0];
        reproduced = Mathf.Approximately(paramArray[1], 1.0f);
    }

    void Update()
    {
        _lifetime -= Time.deltaTime * decreasingFactor;
        if (_lifetime > 0) return;

        if (reproduced || alwaysReproduce)
        {
            Start();
            Ex4Spawner.Instance.Respawn(transform);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
