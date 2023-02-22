using System.Collections.Generic;

public class RegisterSystems
{
    public static List<ISystem> GetListOfSystems()
    {
        // determine order of systems to add
        List<ISystem> toRegister = new List<ISystem>
        {
            // Add your systems
            new EndEventSystem(),
            new SpawnSystem(),
            new CircleCollisionDetectionSystem(),
            new WallCollisionDetectionSystem(),
            new SizeReduceSystem(),
            new BounceBackSystem(),
            new DeoverlapSystem(),
            new PositionUpdateSystem(),
            new ColorDecisionSystem(),
            new DisplayUpdateSystem()
        };

        return toRegister;
    }
}
