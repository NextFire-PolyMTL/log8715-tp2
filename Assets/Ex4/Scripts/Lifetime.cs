using Unity.Collections;
using UnityEngine;
public struct LTParams {
    public float decrFactor {get; set;}
    public bool reproduceFlag;
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

    public void UpdateFromLTParams(LTParams newParams) {
        decreasingFactor = newParams.decrFactor;
        if (!alwaysReproduce)
            reproduced = newParams.reproduceFlag;
    }

    public LTParams exportLTParams() {
        var newParams = new LTParams() {
            decrFactor = decreasingFactor,
            reproduceFlag =  alwaysReproduce || reproduced
        };
        return newParams;
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
