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
            AssetDatabase.CreateAsset(CreateNullCube(), "Assets/NullCube.asset");
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

        static Mesh CreateNullCube()
        {

            var mesh = new Mesh();

            mesh.vertices = new Vector3[24];

            {
                var x = new Vector3(1, 0, 0);
                var y = new Vector3(0, 1, 0);
                var z = new Vector3(0, 0, 1);

                mesh.normals = new Vector3[] {
                    -x, -x, -x, -x,
                     x,  x,  x,  x,
                    -y, -y, -y, -y,
                     y,  y,  y,  y,
                    -z, -z, -z, -z,
                     z,  z,  z,  z
                };
            }

            {
                var x = new Vector4(1, 0, 0, 1);
                var y = new Vector4(0, 1, 0, 1);
                var z = new Vector4(0, 0, 1, 1);

                mesh.tangents = new Vector4[] {
                    -y, -y, -y, -y,
                     y,  y,  y,  y,
                    -z, -z, -z, -z,
                     z,  z,  z,  z,
                    -x, -x, -x, -x,
                     x,  x,  x,  x
                };
            }

            {
                var uv00 = new Vector2(0, 0);
                var uv10 = new Vector2(1, 0);
                var uv01 = new Vector2(0, 1);
                var uv11 = new Vector2(1, 1);

                mesh.uv = new Vector2[] {
                    uv00, uv10, uv01, uv11,
                    uv00, uv10, uv01, uv11,
                    uv00, uv10, uv01, uv11,
                    uv00, uv10, uv01, uv11,
                    uv00, uv10, uv01, uv11,
                    uv00, uv10, uv01, uv11
                };
            }

            mesh.SetTriangles(
                new int[] {
                     0,  1,  2,   1,  3,  2,
                     4,  5,  6,   5,  7,  6,
                     8,  9, 10,   9, 11, 10,
                    12, 13, 14,  13, 15, 14,
                    16, 17, 18,  17, 19, 18,
                    20, 21, 22,  21, 23, 22
                }, 0, true);

            mesh.UploadMeshData(true);
            return mesh;
        }
    }
}

#endif
