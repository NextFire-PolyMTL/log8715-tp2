using UnityEngine;

public struct PositionComponent : IComponent
{
    public int GetRandomNumber() => 14868754;

    public PositionComponent(Vector2 pos)
    {
        this.pos = pos;
    }
    public Vector2 pos;
    public override string ToString()
    {
        return $"{pos.ToString()}";
    }
}

public struct SizeComponent : IComponent
{
    public int GetRandomNumber() => randomNumber;
    private static readonly int randomNumber = 156456789;
    public float size;
    readonly public float originalSize;
    public SizeComponent(float size, float originalSize)
    {
        this.size = size;
        this.originalSize = originalSize;
    }
    public SizeComponent(float size)
    {
        this.size = size;
        this.originalSize = size;
    }
    public override string ToString()
    {
        return $"{size}, {originalSize}";
    }
}

public struct ColliderComponent : IComponent
{
    public int GetRandomNumber() => randomNumber;
    private static readonly int randomNumber = 47891856;
}