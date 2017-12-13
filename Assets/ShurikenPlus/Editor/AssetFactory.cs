// ShurikenPlus - Custom shader library for Unity particle system
// https://github.com/keijiro/ShurikenPlus

using UnityEngine;
using UnityEditor;

// This is only used for the initial creation of the shared assets.
// It's rarely needed to be enabled.

#if false

namespace ShurikenPlus
{
    static class AssetFactory
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

#endif
