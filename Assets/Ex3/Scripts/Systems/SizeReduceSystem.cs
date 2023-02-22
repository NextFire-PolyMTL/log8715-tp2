public class SizeReduceSystem : ISystem
{
    public string Name => "SizeReduceSystem";

    public void UpdateSystem()
    {
        ComponentsManager.Instance.ForEach<CollisionEventComponent, SpeedComponent, SizeComponent>((entity, listOfEvents, speedComponent, sizeComponent) => 
        {
            if (sizeComponent.size > ECSManager.Instance.Config.MinSize)
            {
                ComponentsManager.Instance.SetComponent<SizeComponent>(entity, new SizeComponent(sizeComponent.size / 2, sizeComponent.originalSize));
            }
            else
            {
                ComponentsManager.Instance.RemoveComponent<ColliderComponent>(entity);
            }
        });
    }
}
