using System;
using NUnit.Framework;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace DOTools.Tests.Initiator {

    public class BoxCollider {

        private const string WorldName = "TestWorld";

        private (World, Entity) CreateBoxCollider(
            BoxGeometry geometry = default,
            CollisionFilter filter = default,
            Material material = default
        ) {
            World world = new World(WorldName);
            Entity entity = DOTools.Initiator
                .Transform(world)
                .InitiateBoxCollider(
                    world,
                    geometry,
                    filter,
                    material
                );

            return (world, entity);
        }

        [Test]
        public void CreateBoxCollider_DoesNotThrow() {
            (World world, Entity entity) = CreateBoxCollider();

            Assert.DoesNotThrow(() => {
                world.EntityManager.GetComponentData<PhysicsCollider>(entity);
            });
        }

    }

}