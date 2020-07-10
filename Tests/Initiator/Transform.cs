using System;
using NUnit.Framework;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace DOTools.Tests.Initiator {

    public class Transform {

        private const string WorldName = "TestWorld";

        private (World, Entity) CreateTransform() {
            World world = new World(WorldName);
            Entity entity = DOTools.Initiator.Transform(world);

            return (world, entity);
        }

        private (World, Entity) CreateTransform(float3 position) {
            World world = new World(WorldName);
            Entity entity = DOTools.Initiator.Transform(world, position);

            return (world, entity);
        }

        private (World, Entity) CreateTransform(float3 position, quaternion rotation) {
            World world = new World(WorldName);
            Entity entity = DOTools.Initiator.Transform(world, position, rotation);

            return (world, entity);
        }

        private (World, Entity) CreateTransform(float3 position, quaternion rotation, float3 scale) {
            World world = new World(WorldName);
            Entity entity = DOTools.Initiator.Transform(world, position, rotation, scale);

            return (world, entity);
        }

        [Test]
        public void HasLocalToWorld_DoesNotThrow() {
            (World world, Entity entity) = CreateTransform();

            Assert.DoesNotThrow(() => {
                world.EntityManager.GetComponentData<LocalToWorld>(entity);
            });
        }

        [Test]
        public void HasNonUniformScale_DoesNotThrow() {
            (World world, Entity entity) = CreateTransform();

            Assert.DoesNotThrow(() => {
                world.EntityManager.GetComponentData<NonUniformScale>(entity);
            });
        }

        [Test]
        public void HasTranslation_DoesNotThrow() {
            (World world, Entity entity) = CreateTransform();

            Assert.DoesNotThrow(() => {
                world.EntityManager.GetComponentData<Translation>(entity);
            });
        }

        [Test]
        public void HasRotation_DoesNotThrow() {
            (World world, Entity entity) = CreateTransform();

            Assert.DoesNotThrow(() => {
                world.EntityManager.GetComponentData<Rotation>(entity);
            });
        }

        [Test]
        public void HasScale_Throws() {
            (World world, Entity entity) = CreateTransform();

            Assert.Throws<ArgumentException>(() => {
                world.EntityManager.GetComponentData<Scale>(entity);
            });
        }

        [Test]
        public void IsDefaultPositionZero_True() {
            (World world, Entity entity) = CreateTransform();

            Translation translation = world
                .EntityManager
                .GetComponentData<Translation>(entity);

            Assert.True(translation.Value.Equals(float3.zero));
        }

        [Test]
        public void IsDefaultRotationQuaternionIdentity_True() {
            (World world, Entity entity) = CreateTransform(
                float3.zero
            );

            Rotation rotation = world
                .EntityManager
                .GetComponentData<Rotation>(entity);

            Assert.True(rotation.Value.Equals(quaternion.identity));
        }

        [Test]
        public void IsDefaultScaleOne_True() {
            (World world, Entity entity) = CreateTransform(
                float3.zero,
                quaternion.identity
            );

            NonUniformScale scale = world
                .EntityManager
                .GetComponentData<NonUniformScale>(entity);

            Assert.True(scale.Value.Equals(
                    new float3(1, 1, 1)
                )
            );
        }

        [Test]
        public void IsScaleTwo_True() {
            float3 value = new float3(2, 2, 2);

            (World world, Entity entity) = CreateTransform(
                float3.zero,
                quaternion.identity,
                value
            );

            NonUniformScale scale = world
                .EntityManager
                .GetComponentData<NonUniformScale>(entity);

            Assert.True(scale.Value.Equals(value));
        }

    }

}