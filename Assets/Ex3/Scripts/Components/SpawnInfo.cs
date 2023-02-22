public struct SpawnInfo : IComponent
{
    public int GetRandomNumber() => randomNumber;
    private static readonly int randomNumber = 765412356;
    public bool spawnDone;
    public override string ToString()
    {
        return $"{spawnDone}";
    }
}