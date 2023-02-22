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
