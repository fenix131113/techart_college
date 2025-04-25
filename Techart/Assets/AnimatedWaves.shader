Shader "Custom/AnimatedWaves"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _WaveMagnitude ("Amplitude", Range(0, 0.2)) = 0.05
        _WaveFrequency ("Frequency", Range(0, 10)) = 3.0
        _Speed ("Speed", Range(0, 5)) = 1.0
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent" "RenderType"="Transparent"
        }
        LOD 200
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;

            float _WaveMagnitude;
            float _WaveFrequency;
            float _Speed;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float waveSin(float2 pos, float time)
            {
                return sin(pos.x * _WaveFrequency + time) * cos(pos.y * _WaveFrequency - time);
            }

            v2f vert(appdata_t v)
            {
                v2f o;
                float time = _Time.y * _Speed;

                float wave = waveSin(v.vertex.xy, time);
                v.vertex.y += wave * _WaveMagnitude;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;

                fixed4 texColor = tex2D(_MainTex, uv);
                return texColor * _Color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}