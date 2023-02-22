public struct CollisionEventComponent : IComponent
{
    public int GetRandomNumber() => randomNumber;
    private static readonly int randomNumber = 789153456;
    readonly public uint entityID;
    readonly public uint otherEntityID;

    public CollisionEventComponent(uint id1, uint id2, bool inverse)
    {
        entityID = id1;
        otherEntityID = id2;
    }
    public override string ToString()
    {
        return $"{entityID} {otherEntityID}";
    }
}