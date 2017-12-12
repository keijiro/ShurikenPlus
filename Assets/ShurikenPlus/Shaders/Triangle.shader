Shader "Shuriken Plus/Triangle"
{
    Properties
    {
        [HDR] _Color("Color", Color) = (1, 1, 1, 1)
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
        half3 bccoord : TEXCOORD0;
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
        position.xyz += mul(AngleAxis3x3(angle, axis), v);
        output.position = UnityObjectToClipPos(position);
        output.color = color * _Color;
        output.bccoord = half3(vid == 0, vid == 1, vid == 2);
        UNITY_TRANSFER_FOG(output, output.position);
        return output;
    }

    half4 Fragment(Varyings input) : SV_Target
    {
        half4 c = input.color;

        float3 bcc = input.bccoord;
        float3 fw = fwidth(bcc);
        float3 edge2 = min(smoothstep(fw / 2, fw,     bcc),
                           smoothstep(fw / 2, fw, 1 - bcc));
        float edge = 1 - min(min(edge2.x, edge2.y), edge2.z);

        c.rgb *= (1 + _EdgeColor * edge);

        UNITY_APPLY_FOG(input.fogCoord, c);
        return c;
    }

    ENDCG

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
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
