using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Authoring;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;
using Collider = Unity.Physics.Collider;
using Material = UnityEngine.Material;

namespace DOTools {

    public static class Physics {

        public static World DefaultWorld => World.DefaultGameObjectInjectionWorld;

        private static readonly Type k_DrawComponent = typeof(DisplayBodyColliders)
            .GetNestedType("DrawComponent", BindingFlags.NonPublic);

        private static readonly MethodInfo k_DrawComponent_BuildDebugDisplayMesh = k_DrawComponent
            .GetMethod("BuildDebugDisplayMesh", BindingFlags.Static | BindingFlags.NonPublic, null, new[] { typeof(BlobAssetReference<Collider>) }, null);

        private static readonly Type k_DisplayResult = k_DrawComponent.GetNestedType("DisplayResult");

        private static readonly FieldInfo k_DisplayResultsMesh = k_DisplayResult.GetField("Mesh");
        private static readonly PropertyInfo k_DisplayResultsTransform = k_DisplayResult.GetProperty("Transform");

        internal static void CreateRenderMeshForCollider(EntityManager entityManager, Entity entity, BlobAssetReference<Collider> collider, Material material) {
            Mesh meshing = new Mesh { hideFlags = HideFlags.DontSave };
            List<CombineInstance> instances = new List<CombineInstance>(8);
            int numVertices = 0;

            foreach (object displayResult in (IEnumerable)k_DrawComponent_BuildDebugDisplayMesh.Invoke(null, new object[] { collider })) {
                CombineInstance instance = new CombineInstance {
                    mesh = k_DisplayResultsMesh.GetValue(displayResult) as Mesh,
                    transform = (float4x4)k_DisplayResultsTransform.GetValue(displayResult)
                };

                instances.Add(instance);
                numVertices += meshing.vertexCount;
            }

            meshing.indexFormat = numVertices > ushort.MaxValue ? IndexFormat.UInt32 : IndexFormat.UInt16;
            meshing.CombineMeshes(instances.ToArray());
            meshing.RecalculateBounds();

            entityManager.AddSharedComponentData(entity, new RenderMesh {
                mesh = meshing,
                material = material
            });

            entityManager.AddComponentData(entity, new RenderBounds { Value = meshing.bounds.ToAABB() });
        }

        public static Entity CreateBody(float3 position, quaternion orientation, BlobAssetReference<Collider> collider,
            float3 linearVelocity, float3 angularVelocity, float mass, bool isDynamic, Material material) {

            Tuple<Entity, PhysicsCollider> tuple = CreateCollider(position, orientation, collider);
            Entity entity = tuple.Item1;
            PhysicsCollider colliderComponent = tuple.Item2;

            EntityManager entityManager = DefaultWorld.EntityManager;
            CreateRenderMeshForCollider(entityManager, entity, collider, material);

            if (!isDynamic)
                return entity;

            entityManager.AddComponentData(entity, PhysicsMass.CreateDynamic(colliderComponent.MassProperties, mass));

            float3 angularVelocityLocal = math.mul(math.inverse(colliderComponent.MassProperties.MassDistribution.Transform.rot), angularVelocity);

            entityManager.AddComponentData(entity, new PhysicsVelocity {
                Linear = linearVelocity,
                Angular = angularVelocityLocal
            });

            entityManager.AddComponentData(entity, new PhysicsDamping {
                Linear = 0.01f,
                Angular = 0.05f
            });

            return entity;
        }

        public static Tuple<Entity, PhysicsCollider> CreateCollider(float3 position, quaternion orientation, BlobAssetReference<Collider> collider) {
            EntityManager entityManager = DefaultWorld.EntityManager;

            Entity entity = entityManager.CreateEntity(new ComponentType[] { });

            entityManager.AddComponentData(entity, new LocalToWorld());
            entityManager.AddComponentData(entity, new Translation { Value = position });
            entityManager.AddComponentData(entity, new Rotation { Value = orientation });

            PhysicsCollider colliderComponent = new PhysicsCollider { Value = collider };
            entityManager.AddComponentData(entity, colliderComponent);

            return new Tuple<Entity, PhysicsCollider>(entity, colliderComponent);
        }

        public static Entity CreateStaticBody(float3 position, quaternion orientation, BlobAssetReference<Collider> collider, Material material) {
            return CreateBody(position, orientation, collider, float3.zero, float3.zero, 0.0f, false, material);
        }

        public static Entity CreateDynamicBody(float3 position, quaternion orientation, BlobAssetReference<Collider> collider,
            float3 linearVelocity, float3 angularVelocity, float mass, Material material) {
            return CreateBody(position, orientation, collider, linearVelocity, angularVelocity, mass, true, material);
        }

        public static Entity CreateJoint(PhysicsJoint joint, Entity entityA, Entity entityB, bool enableCollision = false) {
            EntityManager entityManager = DefaultWorld.EntityManager;
            ComponentType[] componentTypes = {
                typeof(PhysicsConstrainedBodyPair),
                typeof(PhysicsJoint)
            };

            Entity jointEntity = entityManager.CreateEntity(componentTypes);

            entityManager.SetComponentData(jointEntity, new PhysicsConstrainedBodyPair(entityA, entityB, enableCollision));
            entityManager.SetComponentData(jointEntity, joint);

            return jointEntity;
        }

    }

}