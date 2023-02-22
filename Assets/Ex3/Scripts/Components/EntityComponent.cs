public struct EntityComponent : IComponent
{
    public int GetRandomNumber() => randomNumber;
    private static readonly int randomNumber = 456834732; // random number to help GetHashCode have different values for class Types in TypeRegistry

    public uint id;
    public EntityComponent(uint id)
    {
        this.id = id;
    }

    public static implicit operator uint(EntityComponent e) => e.id;
    public static implicit operator EntityComponent(uint id)
    {
        return new EntityComponent(id);
    }
    public override string ToString()
    {
        return $"{id}";
    }
}