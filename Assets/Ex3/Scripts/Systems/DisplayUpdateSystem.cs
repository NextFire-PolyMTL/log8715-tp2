public class DisplayUpdateSystem : ISystem
{
    public string Name => "DisplayUpdateSystem";

    public void UpdateSystem()
    {
        ComponentsManager.Instance.ForEach<SizeComponent, PositionComponent, ColorComponent>((entity, sizeComponent, posComponent, colorComponent) =>
        {
            ECSManager.Instance.UpdateShapeSize(entity, sizeComponent.size);
            ECSManager.Instance.UpdateShapeColor(entity, colorComponent.color);
            ECSManager.Instance.UpdateShapePosition(entity, posComponent.pos);
        });
    }
}