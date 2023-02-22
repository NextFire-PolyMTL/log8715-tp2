public class EndEventSystem : ISystem
{
    public string Name => "EndEventSystem";

    public void UpdateSystem()
    {
        ComponentsManager.Instance.ClearComponents<CollisionEventComponent>();
        // other events?
    }
}