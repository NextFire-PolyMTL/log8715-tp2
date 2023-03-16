using Unity.Collections;
using UnityEngine;
public struct LTParams {
    float decTime;
    bool repr;
}

public class Lifetime : MonoBehaviour
{
    private const float MinLifeTimeStart = 5;
    private const float MaxLifeTimeStart = 15;

    public float decreasingFactor = 1;
    public bool alwaysReproduce, reproduced;

    private float _startingLifetime;
    private float _lifetime;

    public float GetProgression() {
        return _lifetime / _startingLifetime;
    }

    void Start() {
        alwaysReproduce = (GetComponent<Entity>().type == EntityType.ETT_PLNT);
        reproduced = false;
        _startingLifetime = Random.Range(MinLifeTimeStart, MaxLifeTimeStart);
        _lifetime = _startingLifetime;
    }


    void Update()
    {
        _lifetime -= Time.deltaTime * decreasingFactor;
        if (_lifetime > 0) return;

        if (reproduced || alwaysReproduce) {
            Start();
            SimulationMain.Instance.Respawn(transform);
        } else {
            gameObject.SetActive(false);
        }
    }
}
