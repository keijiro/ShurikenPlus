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
            AssetDatabase.CreateAsset(CreateNullTriangle(), "Assets/NullTriangle.asset");
            AssetDatabase.CreateAsset(CreateNullHexagon(), "Assets/NullHexagon.asset");
            AssetDatabase.SaveAssets();
        }

        static Mesh CreateNullTriangle()
        {
            var mesh = new Mesh();
            mesh.vertices = new Vector3[3];
            mesh.SetTriangles(new int[] {0, 1, 2}, 0, true);
            mesh.UploadMeshData(true);
            return mesh;
        }

        static Mesh CreateNullHexagon()
        {
            var mesh = new Mesh();
            mesh.vertices = new Vector3[7];
            mesh.SetTriangles(
                new int[] {
                    0, 1, 2,  0, 2, 3,
                    0, 3, 4,  0, 4, 5,
                    0, 5, 6,  0, 6, 1 }, 0, true);
            mesh.UploadMeshData(true);
            return mesh;
        }
    }
}

#endif
