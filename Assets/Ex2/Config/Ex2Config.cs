using UnityEngine;

[CreateAssetMenu(fileName = "Ex2Config", menuName = "Config/Ex2", order = 1)]
public class Ex2Config : ScriptableObject
{
    [SerializeField]
    public int nbCircles = 400;
}
