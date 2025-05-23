Shader "Custom/DissolveShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _DissolveThreshold ("Dissolve Threshold", Range(0, 0.6)) = 0.5
        _EdgeColor ("Edge Color", Color) = (1, 1, 1, 1)
        _EdgeWidth ("Edge Width", Range(0, 0.5)) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _NoiseTex;
            float _DissolveThreshold;
            float4 _EdgeColor;
            float _EdgeWidth;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Получаем значение из текстуры шума
                float noiseValue = tex2D(_NoiseTex, i.uv).r;

                // Если значение шума меньше порога, отбрасываем пиксель
                if (noiseValue < _DissolveThreshold)
                {
                    discard;
                }

                // Получаем цвет из основной текстуры
                fixed4 col = tex2D(_MainTex, i.uv);

                // Добавляем эффект края
                if (noiseValue < _DissolveThreshold + _EdgeWidth)
                {
                    col = _EdgeColor;
                }

                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}