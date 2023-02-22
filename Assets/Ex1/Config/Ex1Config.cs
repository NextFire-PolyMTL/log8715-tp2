using UnityEngine;

[CreateAssetMenu(fileName = "Ex1Config", menuName = "Config/Ex1", order = 1)]
public class Ex1Config : ScriptableObject
{
    [SerializeField]
    public int size = 100000;
}
