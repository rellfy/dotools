using System;
using NUnit.Framework;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace DOTools.Tests.Initiator {

    public class CapsuleCollider {

        private const string WorldName = "TestWorld";

        private (World, Entity) CreateCapsuleCollider(
            CapsuleGeometry geometry = default,
            CollisionFilter filter = default,
            Material material = default
        ) {
            World world = new World(WorldName);
            Entity entity = DOTools.Initiator
                .Transform(world)
                .InitiateCapsuleCollider(
                    world,
                    geometry,
                    filter,
                    material
                );

            return (world, entity);
        }

        [Test]
        public void CreateCapsuleCollider_DoesNotThrow() {
            (World world, Entity entity) = CreateCapsuleCollider();

            Assert.DoesNotThrow(() => {
                world.EntityManager.GetComponentData<PhysicsCollider>(entity);
            });
        }

    }

}