using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace DOTools {

    public static partial class Initiator {

        private static readonly float3 DefaultPosition = float3.zero;
        private static readonly float3 DefaultScale = new float3(1, 1, 1);
        private static readonly quaternion DefaultRotation = quaternion.identity;

        private static Entity Transform(
            Entity entity,
            World world,
            float3 position,
            quaternion rotation,
            float3 scale
        ) {
            world.EntityManager.SetComponentData(entity,
                new Translation() {
                    Value = position
                }
            );

            world.EntityManager.SetComponentData(entity,
                new Rotation() {
                    Value = rotation
                }
            );

            world.EntityManager.SetComponentData(entity,
                new NonUniformScale() {
                    Value = scale
                }
            );

            return entity;
        }

        public static Entity InitiateTransform(this Entity entity, World world) {
            return Transform(
                entity,
                world,
                DefaultPosition,
                DefaultRotation,
                DefaultScale
            );
        }

        public static Entity Transform(
            World world,
            float3 position,
            quaternion rotation,
            float3 scale
        ) {
            Entity entity = world.EntityManager.CreateEntity(
                ComponentType.ReadWrite<LocalToWorld>(),
                ComponentType.ReadWrite<Translation>(),
                ComponentType.ReadWrite<Rotation>(),
                ComponentType.ReadWrite<NonUniformScale>()
            );

            return Transform(
                entity,
                world,
                position,
                rotation,
                scale
            );
        }

        public static Entity Transform(
            World world,
            float3 position,
            quaternion rotation
        ) {
            return Transform(
                world,
                position,
                rotation,
                DefaultScale
            );
        }

        public static Entity Transform(
            World world,
            float3 position
        ) {
            return Transform(
                world,
                position,
                DefaultRotation,
                DefaultScale
            );
        }

        public static Entity Transform(
            World world
        ) {
            return Transform(
                world,
                DefaultPosition,
                DefaultRotation,
                DefaultScale
            );
        }

    }

}