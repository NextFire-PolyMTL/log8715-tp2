using UnityEngine;

public struct SpeedComponent : IComponent
{
    public int GetRandomNumber() => randomNumber;
    private static readonly int randomNumber = 46845378;

    public Vector2 speed;
    public override string ToString()
    {
        return $"{speed.ToString()}";
    }
}