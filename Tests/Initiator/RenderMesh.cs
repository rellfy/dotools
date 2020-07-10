using NUnit.Framework;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
using Material = UnityEngine.Material;

namespace DOTools.Tests.Initiator {

    public class RenderMesh {

        private const string WorldName = "TestWorld";

        private (World, Entity) CreateRenderMesh(
            Mesh mesh = default,
            Material material = default
        ) {
            World world = new World(WorldName);
            Entity entity = DOTools.Initiator
                .Transform(world)
                .InitiateBoxCollider(world)
                .InitiateRenderMesh(world, mesh, material);

            return (world, entity);
        }

        [Test]
        public void CreateRenderMesh_DoesNotThrow() {
            (World world, Entity entity) = CreateRenderMesh();

            Assert.DoesNotThrow(() => {
                world.EntityManager.GetSharedComponentData<Unity.Rendering.RenderMesh>(entity);
            });
        }

    }

}