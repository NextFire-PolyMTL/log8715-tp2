// #define BAD_PERF // TODO CHANGEZ MOI. Mettre en commentaire pour utiliser votre propre structure

using System;
using UnityEngine;

#if BAD_PERF
using InnerType = System.Collections.Generic.Dictionary<uint, IComponent>;
using AllComponents = System.Collections.Generic.Dictionary<uint, System.Collections.Generic.Dictionary<uint, IComponent>>;
#else
// using InnerType = ...; // TODO CHANGEZ MOI, UTILISEZ VOTRE PROPRE TYPE ICI
// Devrait être InnerType = IComponent[] mais ce n'est pas compilable
using AllComponents = System.Collections.Generic.Dictionary<uint, IComponent[]>; // TODO CHANGEZ MOI, UTILISEZ VOTRE PROPRE TYPE ICI
#endif

// Appeler GetHashCode sur un Type est couteux. Cette classe sert a precalculer le hashcode
public static class TypeRegistry<T> where T : IComponent
{
    public static uint typeID = (uint)Mathf.Abs(default(T).GetRandomNumber()) % ComponentsManager.maxEntities;
}

public class Singleton<V> where V : new()
{
    private static bool isInitiated = false;
    private static V _instance;
    public static V Instance
    {
        get
        {
            if (!isInitiated)
            {
                isInitiated = true;
                _instance = new V();
            }
            return _instance;
        }
    }
    protected Singleton() { }
}

internal class ComponentsManager : Singleton<ComponentsManager>
{
    private AllComponents _allComponents = new AllComponents();

    public const int maxEntities = 2000;

#if !BAD_PERF
    // Maps entity id to index in the component arrays
    private uint?[] _idToIndex = new uint?[maxEntities];
    // Next index to use in the component arrays (pool growing phase)
    private uint _nextIndex = 0;
#endif

    public void DebugPrint()
    {
#if BAD_PERF
        string toPrint = "";
        var allComponents = Instance.DebugGetAllComponents();
        foreach (var type in allComponents)
        {
            toPrint += $"{type}: \n";
            foreach (var component in type.Value)
            {
                toPrint += $"\t{component.Key}: {component.Value}\n";
            }
            toPrint += "\n";
        }
        Debug.Log(toPrint);
#else
        string toPrint = "";
        var allComponents = Instance.DebugGetAllComponents();
        foreach (var type in allComponents)
        {
            toPrint += $"{type.Key}: \n";
            for (int i = 0; i < type.Value.Length; i++)
            {
                if (type.Value[i] != null)
                {
                    toPrint += $"\t{i}: {type.Value[i]}\n";
                }
            }
            toPrint += "\n";
        }
        Debug.Log(toPrint);
#endif
    }

    // CRUD
    public void SetComponent<T>(EntityComponent entityID, IComponent component) where T : IComponent
    {
#if BAD_PERF
        if (!_allComponents.ContainsKey(TypeRegistry<T>.typeID))
        {
            _allComponents[TypeRegistry<T>.typeID] = new InnerType();
        }
        _allComponents[TypeRegistry<T>.typeID][entityID] = component;
#else
        var typeId = TypeRegistry<T>.typeID;
        if (!_allComponents.ContainsKey(typeId))
        {
            _allComponents[typeId] = new IComponent[maxEntities];
        }
        if (!_idToIndex[entityID.id].HasValue)
        {
            _idToIndex[entityID.id] = _nextIndex++;
        }
        var index = _idToIndex[entityID.id].Value;
        _allComponents[typeId][index] = component;
#endif
    }
    public void RemoveComponent<T>(EntityComponent entityID) where T : IComponent
    {
#if BAD_PERF
        _allComponents[TypeRegistry<T>.typeID].Remove(entityID);
#else
        var typeId = TypeRegistry<T>.typeID;
        if (_allComponents.ContainsKey(typeId))
        {
            var index = _idToIndex[entityID.id].Value;
            _allComponents[typeId][index] = null;
        }
#endif
    }
    public T GetComponent<T>(EntityComponent entityID) where T : IComponent
    {
#if BAD_PERF
        return (T)_allComponents[TypeRegistry<T>.typeID][entityID];
#else
        var typeId = TypeRegistry<T>.typeID;
        var index = _idToIndex[entityID.id].Value;
        return (T)_allComponents[typeId][index];
#endif
    }
    public bool TryGetComponent<T>(EntityComponent entityID, out T component) where T : IComponent
    {
#if BAD_PERF
        if (_allComponents.ContainsKey(TypeRegistry<T>.typeID))
        {
            if (_allComponents[TypeRegistry<T>.typeID].ContainsKey(entityID))
            {
                component = (T)_allComponents[TypeRegistry<T>.typeID][entityID];
                return true;
            }
        }
        component = default;
        return false;
#else
        var typeId = TypeRegistry<T>.typeID;
        if (_allComponents.ContainsKey(typeId))
        {
            var index = _idToIndex[entityID.id].Value;
            component = (T)_allComponents[typeId][index];
            return true;
        }
        component = default;
        return false;
#endif
    }

    public bool EntityContains<T>(EntityComponent entity) where T : IComponent
    {
#if BAD_PERF
        return _allComponents[TypeRegistry<T>.typeID].ContainsKey(entity);
#else
        var typeId = TypeRegistry<T>.typeID;
        if (_allComponents.ContainsKey(typeId))
        {
            var index = _idToIndex[entity.id].Value;
            return _allComponents[typeId][index] != null;
        }
        return false;
#endif
    }

    public void ClearComponents<T>() where T : IComponent
    {
#if BAD_PERF
        if (!_allComponents.ContainsKey(TypeRegistry<T>.typeID))
        {
            _allComponents.Add(TypeRegistry<T>.typeID, new InnerType());
        }
        else
        {
            _allComponents[TypeRegistry<T>.typeID].Clear();
        }
#else
        var typeId = TypeRegistry<T>.typeID;
        if (!_allComponents.ContainsKey(typeId))
        {
            _allComponents.Add(typeId, new IComponent[maxEntities]);
        }
        else
        {
            Array.Clear(_allComponents[typeId], 0, _allComponents[typeId].Length);
        }
#endif
    }

    public void ForEach<T1>(Action<EntityComponent, T1> lambda) where T1 : IComponent
    {
#if BAD_PERF
        var allEntities = _allComponents[TypeRegistry<EntityComponent>.typeID].Values;
        foreach (EntityComponent entity in allEntities)
        {
            if (!_allComponents[TypeRegistry<T1>.typeID].ContainsKey(entity))
            {
                continue;
            }
            lambda(entity, (T1)_allComponents[TypeRegistry<T1>.typeID][entity]);
        }
#else
        var typeId = TypeRegistry<T1>.typeID;
        if (_allComponents.ContainsKey(typeId))
        {
            var entities = _allComponents[TypeRegistry<EntityComponent>.typeID];
            var components = _allComponents[typeId];
            for (int i = 0; i < _nextIndex; i++)
            {
                if (components[i] != null)
                {
                    var entity = entities[i];
                    lambda((EntityComponent)entity, (T1)components[i]);
                }
            }
        }
#endif
    }

    public void ForEach<T1, T2>(Action<EntityComponent, T1, T2> lambda) where T1 : IComponent where T2 : IComponent
    {
#if BAD_PERF
        var allEntities = _allComponents[TypeRegistry<EntityComponent>.typeID].Values;
        foreach (EntityComponent entity in allEntities)
        {
            if (!_allComponents[TypeRegistry<T1>.typeID].ContainsKey(entity) ||
                !_allComponents[TypeRegistry<T2>.typeID].ContainsKey(entity)
                )
            {
                continue;
            }
            lambda(entity, (T1)_allComponents[TypeRegistry<T1>.typeID][entity], (T2)_allComponents[TypeRegistry<T2>.typeID][entity]);
        }
#else
        var typeId1 = TypeRegistry<T1>.typeID;
        var typeId2 = TypeRegistry<T2>.typeID;
        if (_allComponents.ContainsKey(typeId1) && _allComponents.ContainsKey(typeId2))
        {
            var entities = _allComponents[TypeRegistry<EntityComponent>.typeID];
            var components1 = _allComponents[typeId1];
            var components2 = _allComponents[typeId2];
            for (int i = 0; i < _nextIndex; i++)
            {
                if (components1[i] != null && components2[i] != null)
                {
                    var entity = entities[i];
                    lambda((EntityComponent)entity, (T1)components1[i], (T2)components2[i]);
                }
            }
        }
#endif
    }

    public void ForEach<T1, T2, T3>(Action<EntityComponent, T1, T2, T3> lambda) where T1 : IComponent where T2 : IComponent where T3 : IComponent
    {
#if BAD_PERF
        var allEntities = _allComponents[TypeRegistry<EntityComponent>.typeID].Values;
        foreach (EntityComponent entity in allEntities)
        {
            if (!_allComponents[TypeRegistry<T1>.typeID].ContainsKey(entity) ||
                !_allComponents[TypeRegistry<T2>.typeID].ContainsKey(entity) ||
                !_allComponents[TypeRegistry<T3>.typeID].ContainsKey(entity)
                )
            {
                continue;
            }
            lambda(entity, (T1)_allComponents[TypeRegistry<T1>.typeID][entity], (T2)_allComponents[TypeRegistry<T2>.typeID][entity], (T3)_allComponents[TypeRegistry<T3>.typeID][entity]);
        }
#else
        var typeId1 = TypeRegistry<T1>.typeID;
        var typeId2 = TypeRegistry<T2>.typeID;
        var typeId3 = TypeRegistry<T3>.typeID;
        if (_allComponents.ContainsKey(typeId1) && _allComponents.ContainsKey(typeId2) && _allComponents.ContainsKey(typeId3))
        {
            var entities = _allComponents[TypeRegistry<EntityComponent>.typeID];
            var components1 = _allComponents[typeId1];
            var components2 = _allComponents[typeId2];
            var components3 = _allComponents[typeId3];
            for (int i = 0; i < _nextIndex; i++)
            {
                if (components1[i] != null && components2[i] != null && components3[i] != null)
                {
                    var entity = entities[i];
                    lambda((EntityComponent)entity, (T1)components1[i], (T2)components2[i], (T3)components3[i]);
                }
            }
        }
#endif
    }

    public void ForEach<T1, T2, T3, T4>(Action<EntityComponent, T1, T2, T3, T4> lambda) where T1 : IComponent where T2 : IComponent where T3 : IComponent where T4 : IComponent
    {
#if BAD_PERF
        var allEntities = _allComponents[TypeRegistry<EntityComponent>.typeID].Values;
        foreach (EntityComponent entity in allEntities)
        {
            if (!_allComponents[TypeRegistry<T1>.typeID].ContainsKey(entity) ||
                !_allComponents[TypeRegistry<T2>.typeID].ContainsKey(entity) ||
                !_allComponents[TypeRegistry<T3>.typeID].ContainsKey(entity) ||
                !_allComponents[TypeRegistry<T4>.typeID].ContainsKey(entity)
                )
            {
                continue;
            }
            lambda(entity, (T1)_allComponents[TypeRegistry<T1>.typeID][entity], (T2)_allComponents[TypeRegistry<T2>.typeID][entity], (T3)_allComponents[TypeRegistry<T3>.typeID][entity], (T4)_allComponents[TypeRegistry<T4>.typeID][entity]);
        }
#else
        var typeId1 = TypeRegistry<T1>.typeID;
        var typeId2 = TypeRegistry<T2>.typeID;
        var typeId3 = TypeRegistry<T3>.typeID;
        var typeId4 = TypeRegistry<T4>.typeID;
        if (_allComponents.ContainsKey(typeId1) && _allComponents.ContainsKey(typeId2) && _allComponents.ContainsKey(typeId3) && _allComponents.ContainsKey(typeId4))
        {
            var entities = _allComponents[TypeRegistry<EntityComponent>.typeID];
            var components1 = _allComponents[typeId1];
            var components2 = _allComponents[typeId2];
            var components3 = _allComponents[typeId3];
            var components4 = _allComponents[typeId4];
            for (int i = 0; i < _nextIndex; i++)
            {
                if (components1[i] != null && components2[i] != null && components3[i] != null && components4[i] != null)
                {
                    var entity = entities[i];
                    lambda((EntityComponent)entity, (T1)components1[i], (T2)components2[i], (T3)components3[i], (T4)components4[i]);
                }
            }
        }
#endif
    }

    public AllComponents DebugGetAllComponents()
    {
        return _allComponents;
    }
}
