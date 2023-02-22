using System.Collections.Generic;
using UnityEngine;

public class BounceBackSystem : ISystem
{
    public string Name
    {
        get
        {
            return GetType().Name;
        }
    }

    private List<Vector2> CircleToCircleRestitution(Vector2 pos1, float radius1, float speed1, Vector2 pos2, float radius2, float speed2)
    {
        if (Vector3.Distance(pos1, pos2) <= radius1 + radius2)
        {
            float speedMagnitude = speed1;
            float speedOtherEntityMagnitude = speed2;
            Vector2 newSpeed = pos2 - pos1;
            newSpeed *= speedMagnitude;
            Vector2 newSpeed2 = -newSpeed.normalized * speedOtherEntityMagnitude;
            return new List<Vector2>() { newSpeed, newSpeed2 };
        }
        return null;
    }

    public void UpdateSystem()
    {
        ComponentsManager.Instance.ForEach<CollisionEventComponent, PositionComponent, SpeedComponent, SizeComponent>((entity, collisionEventComponent, positionComponent, speedComponent, sizeComponent) =>
        {
            Vector2 speed = speedComponent.speed;
            Vector2 screenBorderPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

            speedComponent.speed = -speedComponent.speed;
            ComponentsManager.Instance.SetComponent<SpeedComponent>(entity, speedComponent);
        });
    }
}