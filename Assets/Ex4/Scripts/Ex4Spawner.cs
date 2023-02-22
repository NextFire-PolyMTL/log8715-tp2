using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ex4Spawner : MonoBehaviour
{
    public static Transform[] PlantTransforms;
    public static Transform[] PreyTransforms;
    public static Transform[] PredatorTransforms;
    
    public static Lifetime[] PlantLifetimes;
    public static Lifetime[] PreyLifetimes;
    public static Lifetime[] PredatorLifetimes;
    
    public static Velocity[] PlantVelocities;
    public static Velocity[] PreyVelocities;
    public static Velocity[] PredatorVelocities;
    
    public Ex4Config config;
    public GameObject predatorPrefab;
    public GameObject preyPrefab;
    public GameObject plantPrefab;

    private int _height;
    private int _width;
    
    public static Ex4Spawner Instance { get; private set; }

    public void Respawn(Transform t)
    {
        var halfWidth = _width / 2;
        var halfHeight = _height / 2;
        t.position = new Vector3Int(Random.Range(-halfWidth, halfWidth), Random.Range(-halfHeight, halfHeight));
    }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        var size = (float) config.gridSize;
        var ratio = Camera.main!.aspect;
        _height = (int)Math.Round(Math.Sqrt(size / ratio));
        _width = (int)Math.Round(size / _height);
        
        PlantTransforms = new Transform[config.plantCount];
        PlantLifetimes = new Lifetime[config.plantCount];
        PlantVelocities = new Velocity[config.plantCount];
        for (var i = 0; i < config.plantCount; i++)
        {
            var go = Create(plantPrefab);
            PlantTransforms[i] = go.transform;
            PlantLifetimes[i] = go.GetComponent<Lifetime>();
            PlantVelocities[i] = go.GetComponent<Velocity>();
        }

        PreyTransforms = new Transform[config.preyCount];
        PreyLifetimes = new Lifetime[config.plantCount];
        PreyVelocities = new Velocity[config.plantCount];
        for (var i = 0; i < config.preyCount; i++)
        {
            var go = Create(preyPrefab);
            PreyTransforms[i] = go.transform;
            PreyLifetimes[i] = go.GetComponent<Lifetime>();
            PreyVelocities[i] = go.GetComponent<Velocity>();
        }
        
        PredatorTransforms = new Transform[config.predatorCount];
        PredatorLifetimes = new Lifetime[config.plantCount];
        PredatorVelocities = new Velocity[config.plantCount];
        for (var i = 0; i < config.predatorCount; i++)
        {
            var go = Create(predatorPrefab);
            PredatorTransforms[i] = go.transform;
            PredatorLifetimes[i] = go.GetComponent<Lifetime>();
            PredatorVelocities[i] = go.GetComponent<Velocity>();
        }
    }

    private GameObject Create(GameObject prefab)
    {
        var go = Instantiate(prefab);
        Respawn(go.transform);
        return go;
    }
}
