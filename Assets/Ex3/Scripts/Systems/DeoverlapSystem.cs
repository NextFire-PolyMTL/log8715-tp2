public class DeoverlapSystem : ISystem
{
    public string Name => "DeoverlapSystem";

    public void UpdateSystem()
    {
        ComponentsManager.Instance.ForEach<PositionComponent, SizeComponent, CollisionEventComponent, SpeedComponent>((entity, position, size, collision, _) => {
            var radius = size.size / 2;
            var otherPosition = ComponentsManager.Instance.GetComponent<PositionComponent>(collision.otherEntityID);
            var distanceVector = (position.pos - otherPosition.pos).normalized;
            position.pos += distanceVector * radius;
            ComponentsManager.Instance.SetComponent<PositionComponent>(entity, position);
        });
    }
}