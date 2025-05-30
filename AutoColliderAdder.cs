using UnityEngine;

public class AutoColliderAdder : MonoBehaviour
{
    [SerializeField] private bool addToChildren = true;
    [SerializeField] private bool convexColliders = false;
    [SerializeField] private bool isTrigger = false;

    [ContextMenu("Add Colliders to All Meshes")]
    public void AddCollidersToAllMeshes()
    {
        if (addToChildren)
        {
            MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();

            foreach (MeshRenderer meshRenderer in meshRenderers)
            {
                GameObject obj = meshRenderer.gameObject;

                // Zaten collider varsa geç
                if (obj.GetComponent<Collider>() != null)
                    continue;

                // Mesh filter kontrolü
                MeshFilter meshFilter = obj.GetComponent<MeshFilter>();
                if (meshFilter != null && meshFilter.sharedMesh != null)
                {
                    MeshCollider meshCollider = obj.AddComponent<MeshCollider>();
                    meshCollider.sharedMesh = meshFilter.sharedMesh;
                    meshCollider.convex = convexColliders;
                    meshCollider.isTrigger = isTrigger;

                    Debug.Log($"Collider added to: {obj.name}");
                }
            }
        }
    }
}