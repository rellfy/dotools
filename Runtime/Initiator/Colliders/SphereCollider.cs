using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using Material = Unity.Physics.Material;

namespace DOTools {

    public static partial class Initiator {

        private static readonly SphereGeometry DefaultSphereGeometry = new SphereGeometry() {
            Center = float3.zero,
            Radius = 0.5f,
        };

        private static Entity SphereCollider(
            Entity entity,
            World world,
            SphereGeometry geometry,
            CollisionFilter filter,
            Material material
        ) {
            if (geometry.Equals(default))
                geometry = DefaultSphereGeometry;

            try {
                world
                    .EntityManager
                    .GetComponentData<LocalToWorld>(entity);
            } catch (ArgumentException) {
                entity.InitiateTransform(world);
            }

            world.EntityManager.AddComponentData(entity,
                new PhysicsCollider() {
                    Value = Unity.Physics.SphereCollider.Create(
                        geometry,
                        filter,
                        material
                    )
                }
            );

            return entity;
        }

        public static Entity InitiateSphereCollider(
            this Entity entity,
            World world,
            SphereGeometry geometry,
            CollisionFilter filter,
            Material material
        ) {
            return SphereCollider(
                entity,
                world,
                geometry,
                filter,
                material
            );
        }

        public static Entity InitiateSphereCollider(
            this Entity entity,
            World world,
            SphereGeometry geometry,
            CollisionFilter filter
        ) {
            return SphereCollider(
                entity,
                world,
                geometry,
                filter,
                DefaultMaterial
            );
        }

        public static Entity InitiateSphereCollider(
            this Entity entity,
            World world,
            SphereGeometry geometry
        ) {
            return SphereCollider(
                entity,
                world,
                geometry,
                DefaultFilter,
                DefaultMaterial
            );
        }

        public static Entity InitiateSphereCollider(
            this Entity entity,
            World world
        ) {
            return SphereCollider(
                entity,
                world,
                DefaultSphereGeometry,
                DefaultFilter,
                DefaultMaterial
            );
        }

        public static Entity SphereCollider(
            World world,
            SphereGeometry geometry,
            CollisionFilter filter,
            Material material
        ) {
            Entity entity = GetEntityWithColliderRequirements(world);

            return SphereCollider(
                entity,
                world,
                geometry,
                filter,
                material
            );
        }

        public static Entity SphereCollider(
            World world,
            SphereGeometry geometry,
            CollisionFilter filter
        ) {
            Entity entity = GetEntityWithColliderRequirements(world);

            return SphereCollider(
                entity,
                world,
                geometry,
                filter,
                DefaultMaterial
            );
        }

        public static Entity SphereCollider(
            World world,
            SphereGeometry geometry
        ) {
            Entity entity = GetEntityWithColliderRequirements(world);

            return SphereCollider(
                entity,
                world,
                geometry,
                DefaultFilter,
                DefaultMaterial
            );
        }

        public static Entity SphereCollider(
            World world
        ) {
            Entity entity = GetEntityWithColliderRequirements(world);

            return SphereCollider(
                entity,
                world,
                DefaultSphereGeometry,
                DefaultFilter,
                DefaultMaterial
            );
        }

    }

}