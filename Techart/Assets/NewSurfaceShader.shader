Shader "Custom/NewSurfaceShader"
{
    Properties
    {
        _MainColor ("Color", Color) = (1,1,1,1)
        _EdgeColor ("Edge Color", Color) = (1, 0, 0, 1)
        _EdgeThreshold ("Edge Threshold", Range(0, 1)) = 0.5
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

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldNormal : TEXCOORD0;
                float3 viewDir : TEXTCOORD1;
            };

            float4 _MainColor;
            float4 _EdgeColor;
            float _EdgeThreshold;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = normalize(_WorldSpaceCameraPos - mul(unity_ObjectToWorld, v.vertex).xyz);
                return  o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float edge = dot(i.worldNormal, i.viewDir);
                if (edge <= _EdgeThreshold)
                {
                    return _EdgeColor;
                }
                else
                {
                    return _MainColor;
                }
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}