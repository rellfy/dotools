using System;
using NUnit.Framework;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace DOTools.Tests.Initiator {

    public class SphereCollider {

        private const string WorldName = "TestWorld";

        private (World, Entity) CreateSphereCollider(
            SphereGeometry geometry = default,
            CollisionFilter filter = default,
            Material material = default
        ) {
            World world = new World(WorldName);
            Entity entity = DOTools.Initiator
                .Transform(world)
                .InitiateSphereCollider(
                    world,
                    geometry,
                    filter,
                    material
                );

            return (world, entity);
        }

        [Test]
        public void CreateSphereCollider_DoesNotThrow() {
            (World world, Entity entity) = CreateSphereCollider();

            Assert.DoesNotThrow(() => {
                world.EntityManager.GetComponentData<PhysicsCollider>(entity);
            });
        }

    }

}