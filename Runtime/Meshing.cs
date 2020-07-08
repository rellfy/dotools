using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

namespace DOTools {

    public static class Meshing {

        public static void ScaleMesh(Mesh mesh, float3 scale) {
            Vector3[] vertices = mesh.vertices;

            for (int i = 0; i < vertices.Length; i++) {
                vertices[i].x *= scale.x;
                vertices[i].y *= scale.y;
                vertices[i].z *= scale.z;
            }

            mesh.vertices = vertices;
            mesh.RecalculateBounds();
        }

    }

}