// ShurikenPlus - Custom shader library for Unity particle system
// https://github.com/keijiro/ShurikenPlus

// Procedurally randomized hexagon mesh particle shader
Shader "Shuriken Plus/Random Hexagon"
{
    Properties
    {
        [HDR] _Color("Color", Color) = (1, 1, 1, 1)
        [HDR] _EdgeColor("Edge Color", Color) = (1, 1, 1, 1)
        _Randomness("Randomness", Range(0, 1)) = 0.75
    }

    CGINCLUDE

    #include "Common.hlsl"

    half4 _Color;
    half4 _EdgeColor;
    half _Randomness;

    struct Varyings
    {
        float4 position : SV_POSITION;
        half4 color : COLOR;
        half edge : TEXCOORD0;
        UNITY_FOG_COORDS(1)
    };

    Varyings Vertex(
        float4 position : POSITION,
        half4 color : COLOR,
        float4 params : TEXCOORD0
    )
    {
        // Extract the custom vertex data.
        float size = params.x;
        float angle = params.y;
        uint vid = params.z;
        uint seed = params.w * 0x7fffffff;


        float phi = (vid - 1) * UNITY_PI / 3;
        float l = (vid > 0) * lerp(1 - _Randomness, 1, Random(seed + vid));

        float3 v = float3(cos(phi), sin(phi), 0) * l;

        // Apply the rotation.
        v = mul(AngleAxis3x3(angle, RandomUnitVector(seed + 6)), v);

        // Apply the size parameter.
        v *= size;

        // Vertex output
        Varyings output;
        output.position = UnityObjectToClipPos(float4(position.xyz + v, 1));
        output.color = color;
        output.edge = vid > 0;
        UNITY_TRANSFER_FOG(output, output.position);
        return output;
    }

    half4 Fragment(Varyings input) : SV_Target
    {
        // Detect edges from the edge parameter.
        float fw = fwidth(input.edge);
        float edge = smoothstep(1 - fw, 1 - fw / 2, input.edge);

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
