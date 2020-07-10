using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using Material = Unity.Physics.Material;

namespace DOTools {

    public static partial class Initiator {

        private static readonly CapsuleGeometry DefaultCapsuleGeometry = new CapsuleGeometry() {
            Radius = 0.5f,
            Vertex0 = 0.5f,
            Vertex1 = -0.5f,
        };

        private static Entity CapsuleCollider(
            Entity entity,
            World world,
            CapsuleGeometry geometry,
            CollisionFilter filter,
            Material material
        ) {
            if (geometry.Equals(default))
                geometry = DefaultCapsuleGeometry;

            try {
                world
                    .EntityManager
                    .GetComponentData<LocalToWorld>(entity);
            } catch (ArgumentException) {
                entity.InitiateTransform(world);
            }

            world.EntityManager.AddComponentData(entity,
                new PhysicsCollider() {
                    Value = Unity.Physics.CapsuleCollider.Create(
                        geometry,
                        filter,
                        material
                    )
                }
            );

            return entity;
        }

        public static Entity InitiateCapsuleCollider(
            this Entity entity,
            World world,
            CapsuleGeometry geometry,
            CollisionFilter filter,
            Material material
        ) {
            return CapsuleCollider(
                entity,
                world,
                geometry,
                filter,
                material
            );
        }

        public static Entity InitiateCapsuleCollider(
            this Entity entity,
            World world,
            CapsuleGeometry geometry,
            CollisionFilter filter
        ) {
            return CapsuleCollider(
                entity,
                world,
                geometry,
                filter,
                DefaultMaterial
            );
        }

        public static Entity InitiateCapsuleCollider(
            this Entity entity,
            World world,
            CapsuleGeometry geometry
        ) {
            return CapsuleCollider(
                entity,
                world,
                geometry,
                DefaultFilter,
                DefaultMaterial
            );
        }

        public static Entity InitiateCapsuleCollider(
            this Entity entity,
            World world
        ) {
            return CapsuleCollider(
                entity,
                world,
                DefaultCapsuleGeometry,
                DefaultFilter,
                DefaultMaterial
            );
        }

        public static Entity CapsuleCollider(
            World world,
            CapsuleGeometry geometry,
            CollisionFilter filter,
            Material material
        ) {
            Entity entity = GetEntityWithColliderRequirements(world);

            return CapsuleCollider(
                entity,
                world,
                geometry,
                filter,
                material
            );
        }

        public static Entity CapsuleCollider(
            World world,
            CapsuleGeometry geometry,
            CollisionFilter filter
        ) {
            Entity entity = GetEntityWithColliderRequirements(world);

            return CapsuleCollider(
                entity,
                world,
                geometry,
                filter,
                DefaultMaterial
            );
        }

        public static Entity CapsuleCollider(
            World world,
            CapsuleGeometry geometry
        ) {
            Entity entity = GetEntityWithColliderRequirements(world);

            return CapsuleCollider(
                entity,
                world,
                geometry,
                DefaultFilter,
                DefaultMaterial
            );
        }

        public static Entity CapsuleCollider(
            World world
        ) {
            Entity entity = GetEntityWithColliderRequirements(world);

            return CapsuleCollider(
                entity,
                world,
                DefaultCapsuleGeometry,
                DefaultFilter,
                DefaultMaterial
            );
        }

    }

}