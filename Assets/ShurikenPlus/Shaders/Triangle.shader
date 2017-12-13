// ShurikenPlus - Custom shader library for Unity particle system
// https://github.com/keijiro/ShurikenPlus

// Procedurally randomized triangle mesh particle shader
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
        // Extract the custom vertex data.
        float size = params.x;
        float angle = params.y;
        uint vid = params.z;
        uint seed = params.w * 0x7fffffff;

        // Random triangle with centered at POSITION
        float3 v1 = RandomVector(seed + 0);
        float3 v2 = RandomVector(seed + 3);
        float3 v3 = -(v1 + v2);

        // Select this vertex
        float3 v = vid == 0 ? v1 : (vid == 1 ? v2 : v3);

        // Apply the rotation.
        v = mul(AngleAxis3x3(angle, RandomUnitVector(seed + 6)), v);

        // Apply the size parameter.
        v *= size;

        // Vertex output
        Varyings output;
        output.position = UnityObjectToClipPos(float4(position.xyz + v, 1));
        output.color = color;
        output.bccoord = half3(vid == 0, vid == 1, vid == 2);
        UNITY_TRANSFER_FOG(output, output.position);
        return output;
    }

    half4 Fragment(Varyings input) : SV_Target
    {
        // Detect edges from the barycentric coordinates.
        float3 bcc = input.bccoord;
        float3 fw = fwidth(bcc);
        float3 edge3 = min(smoothstep(fw / 2, fw,     bcc),
                           smoothstep(fw / 2, fw, 1 - bcc));
        float edge = 1 - min(min(edge3.x, edge3.y), edge3.z);

        // Color blending
        half4 c = input.color * (_Color + _EdgeColor * edge);
        c.a = saturate(c.a);

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
