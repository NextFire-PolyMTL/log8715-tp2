using System;
using Unity.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
// using paramArray = UnityEngine.Vector3;



public class SimulationMain : MonoBehaviour
{
    public static Transform[] PlantTransforms, PreyTransforms, PredatorTransforms;
    public static NativeArray<Vector3> PlantPos, PreyPos, PredPos;
    public static Lifetime[] PlantLifetimes, PreyLifetimes, PredatorLifetimes;
    public static NativeArray<LTParams> PlantLTParams, PreyLTParams, PredLTParams;
    public static NativeArray<Vector3> PreyVel, PredVel;

    public Ex4Config config;
    public GameObject predatorPrefab, preyPrefab, plantPrefab;

    private int _height;
    private int _width;

    public static SimulationMain Instance { get; private set; }

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
        /* Size of the window */
        var size = (float) config.gridSize;
        var ratio = Camera.main!.aspect;
        _height = (int)Math.Round(Math.Sqrt(size / ratio));
        _width = (int)Math.Round(size / _height);

        /* Plants init */
        PlantTransforms = new Transform[config.plantCount];
        PlantPos = new NativeArray<Vector3>(config.plantCount, Allocator.Persistent);
        PlantLifetimes = new Lifetime[config.plantCount];
        for (var i = 0; i < config.plantCount; i++)
        {
            var go = Create(plantPrefab);
            PlantTransforms[i] = go.transform;
            PlantLifetimes[i] = go.GetComponent<Lifetime>();
        }

        PreyTransforms = new Transform[config.preyCount];
        PreyLifetimes = new Lifetime[config.plantCount];
        PreyVel = new NativeArray<Vector3>(config.preyCount, Allocator.Persistent);
        for (var i = 0; i < config.preyCount; i++)
        {
            var go = Create(preyPrefab);
            PreyTransforms[i] = go.transform;
            PreyLifetimes[i] = go.GetComponent<Lifetime>();
        }

        PredatorTransforms = new Transform[config.predatorCount];
        PredatorLifetimes = new Lifetime[config.plantCount];
        PredVel = new NativeArray<Vector3>(config.predatorCount, Allocator.Persistent);
        for (var i = 0; i < config.predatorCount; i++)
        {
            var go = Create(predatorPrefab);
            PredatorTransforms[i] = go.transform;
            PredatorLifetimes[i] = go.GetComponent<Lifetime>();
        }
    }

    void Update() {

    }

    private GameObject Create(GameObject prefab)
    {
        var go = Instantiate(prefab);
        Respawn(go.transform);
        return go;
    }
}
