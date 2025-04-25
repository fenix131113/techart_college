Shader "Custom/ColorHoverShader"
{
    Properties
    {
        _Color ("Object Color", Color) = (1,1,1,1)
        _Albedo ("Albedo", 2D) = "White" {}
        _Speed ("Speed", Range(0.1, 5)) = 2
        _Hover ("Hover State", Range(0, 1)) = 0
        _HoverPos ("Hover Position", Vector) = (-1, -1, 0, 0)
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 200
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _Color;
            float _Hover;
            float4 _HoverPos;
            sampler2D _Albedo;
            float _Speed;
            float4 _Albedo_ST;

            float noise(float2 p)
            {
                return frac(sin(dot(p, float2(12.9898, 78.233))) * 43758.5453);
            }

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv * _Albedo_ST.xy + _Albedo_ST.zw;
                return o;
            }

            float4 frag(v2f i) : SV_TARGET
            {
                float4 texColor = tex2D(_Albedo, i.uv);

                float grayscale = dot(texColor.rgb, float3(0.3, 0.59, 0.11));
                float4 grayTexColor = float4(grayscale, grayscale, grayscale, texColor.a);

                if (all(_HoverPos.xy == float2(-1, -1)))
                {
                    return grayTexColor;
                }

                float dist = distance(i.uv, _HoverPos.xy);

                float noiseFactor = noise(i.uv * 10) * 0.5;

                float blendFactor = saturate((_Hover - dist * 0.3 + noiseFactor) * _Speed);

                return lerp(grayTexColor, texColor, blendFactor);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}