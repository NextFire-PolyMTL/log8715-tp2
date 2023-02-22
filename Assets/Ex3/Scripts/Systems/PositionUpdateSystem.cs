using UnityEngine;

public class PositionUpdateSystem : ISystem {
    public string Name
    {
        get
        {
            return GetType().Name;
        }
    }

    public void UpdateSystem()
    {
        UpdateSystem(Time.deltaTime);
    }

    public void UpdateSystem(float deltaTime)
    {
        ComponentsManager.Instance.ForEach<PositionComponent, SpeedComponent>((entityID, positionComponent, speedComponent) => {
            Vector2 speed = speedComponent.speed;
            var newPosition = positionComponent.pos + speed * deltaTime;
            ComponentsManager.Instance.SetComponent<PositionComponent>(entityID, new PositionComponent(newPosition));
        });
    }
}