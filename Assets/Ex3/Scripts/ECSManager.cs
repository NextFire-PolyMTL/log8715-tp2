using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ECSManager : MonoBehaviour
{
    [SerializeField]
#pragma warning disable 0649
    private ParticlePool _particlePool;
#pragma warning restore 0649

    #region Public API

    public Ex3Config Config
    {
        get
        {
            return _config;
        }
    }

    [SerializeField]
#pragma warning disable 0649
    private GameObject text;
#pragma warning restore 0649

    private Dictionary<uint, Text> allTexts = new Dictionary<uint, Text>();

    public List<ISystem> AllSystems { get { return _allSystems; } }

    [SerializeField]
    public bool ShouldDisplayEntityIDs = true;

    public void CreateShape(uint id, Ex3Config.ShapeConfig entityConfig)
    {
        _particlePool.CreateParticle(id, entityConfig.initialPos, entityConfig.size, entityConfig.color);
        if (ShouldDisplayEntityIDs)
        {
            var newText = Instantiate(text);
            newText.transform.SetParent(text.transform.parent, false);
            newText.transform.localScale = Vector3.one;
            newText.transform.position = entityConfig.initialPos;
            newText.transform.SetAsLastSibling();
            Text t = newText.GetComponent<Text>();
            t.text = id.ToString();
            t.color = Color.white;
            allTexts[id] = t;
        }
    }

    public void InitDisplay()
    {
        _particlePool.DisplayParticlesFirst();
    }

    public void UpdateShapePosition(uint id, Vector2 position)
    {
        _particlePool.SetParticlePosition(id, position);
        if (Instance.ShouldDisplayEntityIDs)
        {
            allTexts[id].transform.position = position;
        }
    }

    public void UpdateShapeSize(uint id, float size)
    {
        _particlePool.SetParticleSize(id, size);
    }

    public void UpdateShapeColor(uint id, Color color)
    {
        _particlePool.SetParticleColor(id, color);
    }
    #endregion

    #region System Management
    private List<ISystem> _allSystems = new List<ISystem>();

    private void Awake()
    {
        _allSystems = RegisterSystems.GetListOfSystems();
    }

    [SerializeField]
    private bool _debugPrint = false;

    // Update is called once per frame
    private void Update()
    {
        if (_debugPrint)
        {
            ComponentsManager.Instance.DebugPrint();
        }
        foreach (var system in _allSystems)
        {
            system.UpdateSystem();
        }
        _particlePool.DisplayParticles();
    }

    #endregion

    #region Singleton
    private static ECSManager _instance;
    private static bool _instanceInitialized = false;
    public static ECSManager Instance
    {
        get
        {
            if (!_instanceInitialized)
            {
                _instance = FindObjectOfType<ECSManager>();
                _instanceInitialized = true;
            }
            return _instance;
        }
    }
    private ECSManager() { }
    #endregion

    #region Private attributes
    [SerializeField]
#pragma warning disable 0649
    private Ex3Config _config;
#pragma warning restore 0649
    #endregion
}

public interface ISystem
{
    void UpdateSystem();
    string Name { get; }
}

public interface IComponent
{
    int GetRandomNumber();
}