using UnityEngine;

public class CircleCollisionDetectionSystem : ISystem
{
    public string Name => "CircleCollisionDetectionSystem";

    public void UpdateSystem()
    {
        ComponentsManager.Instance.ForEach<PositionComponent, SizeComponent, ColliderComponent>((entity1, posComponent1, sizeComponent1, _) =>
        {
            ComponentsManager.Instance.ForEach<PositionComponent, SizeComponent, ColliderComponent>((entity2, posComponent2, sizeComponent2, __) =>
            {
                var pos1 = posComponent1.pos;
                var radius1 = sizeComponent1.size / 2;
                var pos2 = posComponent2.pos;
                var radius2 = sizeComponent2.size / 2;

                if (entity1 == entity2)
                {
                    //early return, no need to check self
                    return;
                }

                if (Vector3.Distance(pos1, pos2) <= radius1 + radius2)
                {
                    ComponentsManager.Instance.SetComponent<CollisionEventComponent>(entity1, new CollisionEventComponent(entity1, entity2, true));
                    ComponentsManager.Instance.SetComponent<CollisionEventComponent>(entity2, new CollisionEventComponent(entity2, entity1, true));
                }
            });
        });
    }
}