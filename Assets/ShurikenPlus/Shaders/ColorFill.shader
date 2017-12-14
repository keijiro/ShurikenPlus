// ShurikenPlus - Custom shader library for Unity particle system
// https://github.com/keijiro/ShurikenPlus

// Simple color fill
Shader "Shuriken Plus/Color Fill"
{
    Properties
    {
        [HDR] _Color("Color", Color) = (1, 1, 1, 1)
    }

    CGINCLUDE

    #include "UnityCG.cginc"

    half4 _Color;

    struct Varyings
    {
        float4 position : SV_POSITION;
        half4 color : COLOR;
        UNITY_FOG_COORDS(0)
    };

    Varyings Vertex(float4 position : POSITION, half4 color : COLOR)
    {
        Varyings output;
        output.position = UnityObjectToClipPos(position);
        output.color = color;
        UNITY_TRANSFER_FOG(output, output.position);
        return output;
    }

    half4 Fragment(Varyings input) : SV_Target
    {
        half4 c = input.color * _Color;
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
