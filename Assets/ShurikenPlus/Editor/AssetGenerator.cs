using UnityEngine;
using UnityEditor;

namespace ShurikenPlus
{
    static class AssetGenerator
    {
        [MenuItem("Assets/Create/ShurikenPlus Assets")]
        static void CreateAssets()
        {
            AssetDatabase.CreateAsset(CreateTriangle(), "Assets/Triangle.asset");
            AssetDatabase.SaveAssets();
        }

        static Mesh CreateTriangle()
        {
            var mesh = new Mesh();
            mesh.vertices = new Vector3[3];
            mesh.SetTriangles(new int[] {0, 1, 2}, 0, true);
            mesh.UploadMeshData(true);
            return mesh;
        }
    }
}
