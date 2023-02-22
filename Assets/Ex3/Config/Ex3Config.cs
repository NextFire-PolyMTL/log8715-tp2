using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Ex3Config", menuName = "Config/Ex3", order = 1)]
public class Ex3Config : ScriptableObject {

    [Serializable]
    public struct ShapeConfig
    {
        public Vector2 initialPos;
        public float size;
        public Color color;
    }

    [SerializeField]
    public int numberOfShapesToSpawn;

    public float MinSize
    {
        get
        {
            return 1f / (Mathf.Sqrt(numberOfShapesToSpawn)) * 5f;
        }
    }

    public float MaxSize
    {
        get
        {
            return MinSize * 1.5f;
        }
    }
}

