public class ColorDecisionSystem : ISystem
{
    public string Name => "ColorDecisionSystem";

    public void UpdateSystem()
    {
        ComponentsManager.Instance.ForEach<SpeedComponent>((entity, _ )=> {
            // everything with a speed is green
            var colorToAssign = NiceColors.NiceGreen;
            // then everything with a collider AND speed in addition to that is blue
            if (ComponentsManager.Instance.EntityContains<ColliderComponent>(entity))
            {
                colorToAssign = NiceColors.NiceBlue;
            }
            ComponentsManager.Instance.SetComponent<ColorComponent>(entity, new ColorComponent(colorToAssign));
        });
    }
}
