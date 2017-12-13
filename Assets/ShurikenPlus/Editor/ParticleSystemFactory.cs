// ShurikenPlus - Custom shader library for Unity particle system
// https://github.com/keijiro/ShurikenPlus

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace ShurikenPlus
{
    static class ParticleSystemFactory
    {
        static Material LoadDefaultMaterial(string name)
        {
            var path = "Assets/ShurikenPlus/Defaults/ShurikenPlus " + name + ".mat";
            return AssetDatabase.LoadAssetAtPath<Material>(path);
        }

        static Mesh LoadDefaultMesh(string name)
        {
            var path = "Assets/ShurikenPlus/Defaults/ShurikenPlus " + name + ".asset";
            return AssetDatabase.LoadAssetAtPath<Mesh>(path);
        }

        [MenuItem("GameObject/Effects/ShurikenPlus (Random Triangle)")]
        static void CreateTriangle()
        {
            var go = new GameObject("Particle System");
            go.AddComponent<ParticleSystem>();

            var psr = go.GetComponent<ParticleSystemRenderer>();
            psr.sharedMaterial = LoadDefaultMaterial("RandomTriangle");
            psr.mesh = LoadDefaultMesh("NullTriangle");
            psr.alignment = ParticleSystemRenderSpace.World;
            psr.renderMode = ParticleSystemRenderMode.Mesh;

            psr.SetActiveVertexStreams(
                new List<ParticleSystemVertexStream>(
                    new ParticleSystemVertexStream[] {
                        ParticleSystemVertexStream.Position,
                        ParticleSystemVertexStream.Color,
                        ParticleSystemVertexStream.SizeX,
                        ParticleSystemVertexStream.Rotation,
                        ParticleSystemVertexStream.VertexID,
                        ParticleSystemVertexStream.StableRandomX
                    }
                )
            );

            Selection.objects = new [] { go };
            Undo.RegisterCreatedObjectUndo(go, "Create Particle System");
        }

        [MenuItem("GameObject/Effects/ShurikenPlus (Random Hexagon)")]
        static void CreateHexagon()
        {
            var go = new GameObject("Particle System");
            go.AddComponent<ParticleSystem>();

            var psr = go.GetComponent<ParticleSystemRenderer>();
            psr.sharedMaterial = LoadDefaultMaterial("RandomHexagon");
            psr.mesh = LoadDefaultMesh("NullHexagon");
            psr.alignment = ParticleSystemRenderSpace.World;
            psr.renderMode = ParticleSystemRenderMode.Mesh;

            psr.SetActiveVertexStreams(
                new List<ParticleSystemVertexStream>(
                    new ParticleSystemVertexStream[] {
                        ParticleSystemVertexStream.Position,
                        ParticleSystemVertexStream.Color,
                        ParticleSystemVertexStream.SizeX,
                        ParticleSystemVertexStream.Rotation,
                        ParticleSystemVertexStream.VertexID,
                        ParticleSystemVertexStream.StableRandomX
                    }
                )
            );

            Selection.objects = new [] { go };
            Undo.RegisterCreatedObjectUndo(go, "Create Particle System");
        }
    }
}
