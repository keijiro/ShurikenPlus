// ShurikenPlus - Custom shader library for Unity particle system
// https://github.com/keijiro/ShurikenPlus

// Procedurally randomized triangle mesh particle shader
Shader "Shuriken Plus/Wire Cube"
{
    Properties
    {
        [HDR] _Color("Color", Color) = (0.5, 0.5, 0.5, 1)
        [HDR] _EdgeColor("Edge Color", Color) = (1, 1, 1, 1)
    }

    CGINCLUDE

    #include "Common.hlsl"

    half4 _Color;
    half4 _EdgeColor;

    struct Varyings
    {
        float4 position : SV_POSITION;
        half4 color : COLOR;
        half2 edge : TEXCOORD0;
        UNITY_FOG_COORDS(1)
    };

    Varyings Vertex(
        float4 position : POSITION,
        half3 normal : NORMAL,
        half4 tangent : TANGENT,
        half4 color : COLOR,
        half4 texcoord0 : TEXCOORD0, // U, V, Rx, Ry
        half4 texcoord1 : TEXCOORD1  // Rz, Sx, Sy, Sz
    )
    {
        half3 bitangent = cross(normal, tangent.xyz);
        half2 uv = (texcoord0.xy - 0.5) * 2;
        half3 rot = half3(texcoord0.zw, texcoord1.x);
        half3 size = texcoord1.yzw;

        // Relative position of the vertex
        float3 v = normal + tangent.xyz * uv.x + bitangent.xyz * uv.y;

        // Particle rotation
        v = mul(Euler3x3(rot / 360), v);

        // Particle size
        v *= size;

        // Vertex output
        Varyings output;
        output.position = UnityObjectToClipPos(float4(position.xyz + v, 1));
        output.color = color;
        output.edge = uv;
        UNITY_TRANSFER_FOG(output, output.position);
        return output;
    }

    half4 Fragment(Varyings input) : SV_Target
    {
        // Edge detection
        half2 bcc = 1 - abs(input.edge);
        half2 edge2 = saturate(bcc / fwidth(bcc));
        half edge = 1 - min(edge2.x, edge2.y);

        // Color blending
        half4 c = input.color * (_Color + _EdgeColor * edge);
        c.a = saturate(c.a);

        UNITY_APPLY_FOG(input.fogCoord, c);

        return c;
    }

    ENDCG

    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        Cull off Blend SrcAlpha One ZWrite Off
        Pass
        {
            CGPROGRAM
            #pragma vertex Vertex
            #pragma fragment Fragment
            #pragma multi_compile_fog
            ENDCG
        }
    }
}
