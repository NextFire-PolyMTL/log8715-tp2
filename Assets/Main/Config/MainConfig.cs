using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MainConfig", menuName = "Config/Main", order = 1)]
public class MainConfig : ScriptableObject
{
    [SerializeField]
    public List<string> scenesToLoad;
}
