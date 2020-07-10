using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using Material = Unity.Physics.Material;

namespace DOTools {

    public static partial class Initiator {

        private static readonly BoxGeometry DefaultBoxGeometry = new BoxGeometry() {
            Center =  float3.zero,
            Orientation = quaternion.identity,
            Size = new float3(1, 1, 1),
            BevelRadius = 0
        };

        private static Entity BoxCollider(
            Entity entity,
            World world,
            BoxGeometry geometry,
            CollisionFilter filter,
            Material material
        ) {
            if (geometry.Equals(default))
                geometry = DefaultBoxGeometry;

            try {
                world
                    .EntityManager
                    .GetComponentData<LocalToWorld>(entity);
            } catch (ArgumentException) {
                entity.InitiateTransform(world);
            }

            world.EntityManager.AddComponentData(entity, 
                new PhysicsCollider() {
                    Value = Unity.Physics.BoxCollider.Create(
                        geometry,
                        filter,
                        material
                    )
                }
            );

            return entity;
        }

        public static Entity InitiateBoxCollider(
            this Entity entity,
            World world,
            BoxGeometry geometry,
            CollisionFilter filter,
            Material material
        ) {
            return BoxCollider(
                entity,
                world,
                geometry,
                filter,
                material
            );
        }

        public static Entity InitiateBoxCollider(
            this Entity entity,
            World world,
            BoxGeometry geometry,
            CollisionFilter filter
        ) {
            return BoxCollider(
                entity,
                world,
                geometry,
                filter,
                DefaultMaterial
            );
        }

        public static Entity InitiateBoxCollider(
            this Entity entity,
            World world,
            BoxGeometry geometry
        ) {
            return BoxCollider(
                entity,
                world,
                geometry,
                DefaultFilter,
                DefaultMaterial
            );
        }

        public static Entity InitiateBoxCollider(
            this Entity entity,
            World world
        ) {
            return BoxCollider(
                entity,
                world,
                DefaultBoxGeometry,
                DefaultFilter,
                DefaultMaterial
            );
        }

        public static Entity BoxCollider(
            World world,
            BoxGeometry geometry,
            CollisionFilter filter,
            Material material
        ) {
            Entity entity = GetEntityWithColliderRequirements(world);

            return BoxCollider(
                entity,
                world,
                geometry,
                filter,
                material
            );
        }

        public static Entity BoxCollider(
            World world,
            BoxGeometry geometry,
            CollisionFilter filter
        ) {
            Entity entity = GetEntityWithColliderRequirements(world);

            return BoxCollider(
                entity,
                world,
                geometry,
                filter,
                DefaultMaterial
            );
        }

        public static Entity BoxCollider(
            World world,
            BoxGeometry geometry
        ) {
            Entity entity = GetEntityWithColliderRequirements(world);

            return BoxCollider(
                entity,
                world,
                geometry,
                DefaultFilter,
                DefaultMaterial
            );
        }

        public static Entity BoxCollider(
            World world
        ) {
            Entity entity = GetEntityWithColliderRequirements(world);

            return BoxCollider(
                entity,
                world,
                DefaultBoxGeometry,
                DefaultFilter,
                DefaultMaterial
            );
        }

    }

}