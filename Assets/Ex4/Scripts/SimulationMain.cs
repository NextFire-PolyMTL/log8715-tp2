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
    private int plantCount, preyCount, predCount;
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

        plantCount = config.plantCount;
        preyCount = config.preyCount;
        predCount = config.predatorCount;


        /* Plants init */
        PlantTransforms = new Transform[plantCount];
        PlantPos = new NativeArray<Vector3>(plantCount, Allocator.Persistent);
        PlantLifetimes = new Lifetime[plantCount];
        PlantLTParams = new NativeArray<LTParams>(plantCount, Allocator.Persistent);
        for (var i = 0; i < plantCount; i++)
        {
            var go = Create(plantPrefab);
            PlantTransforms[i] = go.transform;
            PlantLifetimes[i] = go.GetComponent<Lifetime>();
            PlantPos[i] = PlantTransforms[i].position;
        }

        PreyTransforms = new Transform[preyCount];
        PreyPos = new NativeArray<Vector3>(preyCount, Allocator.Persistent);
        PreyLifetimes = new Lifetime[preyCount];
        PreyVel = new NativeArray<Vector3>(preyCount, Allocator.Persistent);
        PreyLTParams = new NativeArray<LTParams>(preyCount, Allocator.Persistent);
        for (var i = 0; i < preyCount; i++)
        {
            var go = Create(preyPrefab);
            PreyTransforms[i] = go.transform;
            PreyLifetimes[i] = go.GetComponent<Lifetime>();
            PreyPos[i] = PreyTransforms[i].position;
        }

        PredatorTransforms = new Transform[predCount];
        PredPos = new NativeArray<Vector3>(predCount, Allocator.Persistent);
        PredatorLifetimes = new Lifetime[predCount];
        PredVel = new NativeArray<Vector3>(predCount, Allocator.Persistent);
        PredLTParams = new NativeArray<LTParams>(predCount, Allocator.Persistent);
        for (var i = 0; i < predCount; i++)
        {
            var go = Create(predatorPrefab);
            PredatorTransforms[i] = go.transform;
            PredatorLifetimes[i] = go.GetComponent<Lifetime>();
            PredPos[i] = PredatorTransforms[i].position;
        }
    }

    void GetPos() {
        for (int i = 0; i < plantCount; ++i)
            PlantPos[i] = PlantTransforms[i].position;
        for (int i = 0; i < preyCount; ++i)
            PreyPos[i] = PreyTransforms[i].position;
        for (int i = 0; i < predCount; ++i)
            PredPos[i] = PredatorTransforms[i].position;
    }

    void GetLTParams() {
        for (int i = 0; i < plantCount; ++i)
            PlantLTParams[i] = PlantLifetimes[i].exportLTParams();
        for (int i = 0; i < preyCount; ++i)
            PreyLTParams[i] = PreyLifetimes[i].exportLTParams();
        for (int i = 0; i < predCount; ++i)
            PredLTParams[i] = PredatorLifetimes[i].exportLTParams();
    }
    void UpdateTransforms() {
        for (int i = 0; i < plantCount; ++i)
            PlantTransforms[i].position = PlantPos[i];
        for (int i = 0; i < preyCount; ++i)
            PreyTransforms[i].position = PreyPos[i];
        for (int i = 0; i < predCount; ++i)
            PredatorTransforms[i].position = PredPos[i];
    }

    void UpdateLifetimes() {
        for (int i = 0; i < plantCount; ++i)
            PlantLifetimes[i].UpdateFromLTParams(PlantLTParams[i]);
        for (int i = 0; i < preyCount; ++i)
            PreyLifetimes[i].UpdateFromLTParams(PreyLTParams[i]);
        for (int i = 0; i < predCount; ++i)
            PredatorLifetimes[i].UpdateFromLTParams(PredLTParams[i]);
    }

    void Update() {
        GetPos();
        GetLTParams();
        LifeTimeUpdate.ApplyJob();
        ChasingUpdate.ApplyJob();
        PositionUpdate.ApplyJob();
        UpdateTransforms();
        UpdateLifetimes();
    }

    private GameObject Create(GameObject prefab)
    {
        var go = Instantiate(prefab);
        Respawn(go.transform);
        return go;
    }
}
