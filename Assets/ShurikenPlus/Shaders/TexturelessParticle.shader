// ShurikenPlus - Custom shader library for Unity particle system
// https://github.com/keijiro/ShurikenPlus

// Textureless particle shader
Shader "Shuriken Plus/Textureless Particle"
{
    Properties
    {
        [HDR] _Color("Color", Color) = (1, 1, 1, 1)

        [KeywordEnum(Exponent, SmoothStep)]
        _CurveType("Curve Type", Float) = 1

        _Shape("Shape Parameter", Range(0, 1)) = 0.5
    }

    CGINCLUDE

    #include "Common.hlsl"

    half4 _Color;
    half _Shape;

    struct Varyings
    {
        float4 position : SV_POSITION;
        half4 color : COLOR;
        half2 texcoord : TEXCOORD0;
        UNITY_FOG_COORDS(1)
    };

    Varyings Vertex(
        float4 position : POSITION,
        half4 color : COLOR,
        half2 texcoord : TEXCOORD0
    )
    {
        Varyings output;
        output.position = UnityObjectToClipPos(position);
        output.color = color * _Color;
        output.texcoord = texcoord;
        UNITY_TRANSFER_FOG(output, output.position);
        return output;
    }

    half4 Fragment(Varyings input) : SV_Target
    {
        half x = length(input.texcoord - 0.5) * 2;
    #if _CURVETYPE_EXPONENT
        x = pow(saturate(1 - x), 0.01 + 9.99 * _Shape);
    #else
        x = 1 - smoothstep(_Shape, 1, x);
    #endif
        half4 c = input.color * x;
        UNITY_APPLY_FOG(input.fogCoord, c);
        return c;
    }

    ENDCG

    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
        Cull off Blend SrcAlpha One ZWrite Off
        Pass
        {
            CGPROGRAM
            #pragma vertex Vertex
            #pragma fragment Fragment
            #pragma multi_compile_fog
            #pragma multi_compile _CURVETYPE_EXPONENT _CURVETYPE_SMOOTHSTEP
            ENDCG
        }
    }
}
