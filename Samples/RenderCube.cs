#if DOTOOLS_SAMPLES
using DOTools;
using Unity.Entities;
using UnityEngine;

public class RenderCubeSample : SystemBase {

    protected override void OnCreate() {
        (Mesh mesh, Material material) = GetDefaultMeshAndMaterial();

        DOTools.Initiator
            .Transform(World)
            .InitiateBoxCollider(World)
            .InitiateRenderMesh(World, mesh, material);
    }

    protected override void OnUpdate() { }

    private (Mesh, Material) GetDefaultMeshAndMaterial() {
        GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Mesh mesh = temp.GetComponent<MeshFilter>().sharedMesh;
        Material material = temp.GetComponent<MeshRenderer>().sharedMaterial;
        Object.Destroy(temp);

        return (mesh, material);
    }

}
#endif