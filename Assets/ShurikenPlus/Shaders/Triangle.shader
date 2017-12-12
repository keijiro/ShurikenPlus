Shader "Shuriken Plus/Triangle"
{
    Properties
    {
        [HDR] _Color("Color", Color) = (1, 1, 1, 1)
    }

    CGINCLUDE

    #include "Common.hlsl"

    half4 _Color;

    struct Varyings
    {
        float4 position : SV_POSITION;
        half4 color : COLOR;
        UNITY_FOG_COORDS(1)
    };

    Varyings Vertex(
        float4 position : POSITION,
        half4 color : COLOR,
        float4 params : TEXCOORD0
    )
    {
        float size = params.x;
        float angle = params.y;
        uint vid = params.z;
        uint seed = params.w * 0x7fffffff;

        float3 v1 = RandomVector(seed + 0);
        float3 v2 = RandomVector(seed + 3);
        float3 v3 = -(v1 + v2);
        half3 axis = RandomUnitVector(seed + 6);

        float3 v = vid == 0 ? v1 : (vid == 1 ? v2 : v3);
        v *= size;

        Varyings output;
        position.xyz += mul(Euler3x3(axis * angle), v);
        output.position = UnityObjectToClipPos(position);
        output.color = color * _Color;
        UNITY_TRANSFER_FOG(output, output.position);
        return output;
    }

    half4 Fragment(Varyings input) : SV_Target
    {
        return input.color;
    }

    ENDCG

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Cull off
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
