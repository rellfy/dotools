using System;
using Unity.Entities;
using Unity.Physics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;
using Material = UnityEngine.Material;

namespace DOTools {

    public static partial class Initiator {

        private static readonly RenderMesh DefaultRenderMesh = new RenderMesh() {
            castShadows = ShadowCastingMode.On,
            receiveShadows = true,
            material = default,
            mesh = default
        };

        private static Entity GetEntityWithRenderMeshRequirements(World world) {
            Entity entity = Transform(world);
            return entity;
        }

        private static Entity RenderMesh(
            Entity entity,
            World world,
            RenderMesh renderMesh
        ) {
            try {
                world
                    .EntityManager
                    .GetComponentData<LocalToWorld>(entity);
            } catch (ArgumentException) {
                entity.InitiateTransform(world);
            }

            world.EntityManager.AddComponents(entity,
                new ComponentTypes(
                    ComponentType.ReadWrite<RenderBounds>(),
                    ComponentType.ReadWrite<WorldRenderBounds>(),
                    ComponentType.ReadWrite<ChunkWorldRenderBounds>()
                )
            );

            world.EntityManager.AddSharedComponentData(entity, renderMesh);

            return entity;
        }

        public static Entity InitiateRenderMesh(
            this Entity entity,
            World world,
            RenderMesh renderMesh
        ) {
            return RenderMesh(
                entity,
                world,
                renderMesh
            );
        }

        public static Entity InitiateRenderMesh(
            this Entity entity,
            World world,
            Mesh mesh,
            Material material
        ) {
            RenderMesh renderMesh = DefaultRenderMesh;
            renderMesh.mesh = mesh;
            renderMesh.material = material;

            return RenderMesh(
                entity,
                world,
                renderMesh
            );
        }

        public static Entity RenderMesh(
            World world,
            RenderMesh renderMesh
        ) {
            Entity entity = GetEntityWithRenderMeshRequirements(world);

            return RenderMesh(
                entity,
                world,
                renderMesh
            );
        }

        public static Entity RenderMesh(
            World world,
            Mesh mesh,
            Material material
        ) {
            RenderMesh renderMesh = DefaultRenderMesh;
            renderMesh.mesh = mesh;
            renderMesh.material = material;

            return RenderMesh(
                world,
                renderMesh
            );
        }

    }

}