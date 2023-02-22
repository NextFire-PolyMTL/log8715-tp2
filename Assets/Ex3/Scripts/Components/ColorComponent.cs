using UnityEngine;

public struct ColorComponent : IComponent
{
    public int GetRandomNumber() => 537563478;
    public Color color;
    public ColorComponent(Color c)
    {
        color = c;
    }
    public override string ToString()
    {
        return color.ToString();
    }
}
