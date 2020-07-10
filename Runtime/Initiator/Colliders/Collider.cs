using Unity.Entities;
using Unity.Physics;
using Material = Unity.Physics.Material;

namespace DOTools {

    public static partial class Initiator {

        private static readonly CollisionFilter DefaultFilter = default;
        private static readonly Material DefaultMaterial = default;

        private static Entity GetEntityWithColliderRequirements(World world) {
            Entity entity = Transform(world);

            world.EntityManager.AddComponent(entity,
                ComponentType.ReadWrite<PhysicsCollider>()
            );

            return entity;
        }

    }

}