using UnityEngine;

public class SpawnSystem : ISystem
{
    public string Name { get { return GetType().Name; } }

    public void UpdateSystem()
    {
        bool spawnFound = ComponentsManager.Instance.TryGetComponent(new EntityComponent(0), out SpawnInfo spawn);

        if (!spawnFound || !spawn.spawnDone)
        {
            ComponentsManager.Instance.ClearComponents<CollisionEventComponent>();

            Vector2 screenBorderPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

            uint currentID = 0;
            int staticCounter = 0;
            for (int i=0; i<ECSManager.Instance.Config.numberOfShapesToSpawn; i++)
            {
                var randomPos = new Vector2(UnityEngine.Random.Range(-screenBorderPos.x, screenBorderPos.x), UnityEngine.Random.Range(-screenBorderPos.y, screenBorderPos.y));
                var entity = new Ex3Config.ShapeConfig() {
                    initialPos = randomPos,
                    size = UnityEngine.Random.Range(ECSManager.Instance.Config.MinSize, ECSManager.Instance.Config.MaxSize),
                    color = Color.red,
                };
                bool isStatic = false;
                if (staticCounter == 4)
                {
                    staticCounter = 0;
                    isStatic = true;

                    entity.color = NiceColors.NiceRed;
                    ComponentsManager.Instance.SetComponent<ColorComponent>(currentID, new ColorComponent(entity.color));
                }
                else
                {
                    staticCounter += 1;
                }
                SpawnEntity(currentID, new Vector2(UnityEngine.Random.Range(-5.0f, 5.0f), UnityEngine.Random.Range(-5.0f, 5.0f)), entity, isStatic);
                currentID += 1;
            }
            ECSManager.Instance.InitDisplay();
            var spawnInfo = new SpawnInfo
            {
                spawnDone = true
            };
            ComponentsManager.Instance.SetComponent<SpawnInfo>(new EntityComponent(), spawnInfo);
        }
    }

    private static void SpawnEntity(uint entityId, Vector2 speed, Ex3Config.ShapeConfig entityConfig, bool isStatic)
    {
        ECSManager.Instance.CreateShape(entityId, entityConfig);
        ComponentsManager.Instance.SetComponent<ColliderComponent>(entityId, new ColliderComponent());
        ComponentsManager.Instance.SetComponent<PositionComponent>(entityId, new PositionComponent(entityConfig.initialPos));
        ComponentsManager.Instance.SetComponent<SizeComponent>(entityId, new SizeComponent(entityConfig.size));
        ComponentsManager.Instance.SetComponent<EntityComponent>(entityId, new EntityComponent(entityId));

        if (!isStatic)
        {
            SpeedComponent speedData = new SpeedComponent
            {
                speed = speed
            };
            ComponentsManager.Instance.SetComponent<SpeedComponent>(entityId, speedData);
        }
    }
}
