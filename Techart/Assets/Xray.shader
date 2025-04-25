Shader "Unlit/Xray"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _SilColor("Silhouette Color", Color) = (0, 0, 0, 1)
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" "Queue" = "Transparent"
        }

        Pass
        {
            Tags
            {
                "LightMode" = "UniversalForward"
            }

            Stencil
            {
                Ref 1
                Comp Always
                Pass Replace
            }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment  frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            TEXTURE2D (_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
            CBUFFER_END

            struct Attributes
            {
                float4 position_os : POSITION;
                float3 normal_os : NORMAL;
                float2 texcoord : TEXCOORD0;
            };

            struct Varyings
            {
                float4 position_cs : SV_POSITION;
                float3 normal_ws : NORMAL;
                float2 texcoord : TEXCOORD0;
            };

            Varyings vert(Attributes input)
            {
                Varyings output;

                VertexPositionInputs vertex_input = GetVertexPositionInputs(input.position_os.xyz);

                output.position_cs = vertex_input.positionCS;

                output.normal_ws = TransformObjectToWorldNormal(input.normal_os);

                output.texcoord = TRANSFORM_TEX(input.texcoord, _MainTex);

                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                Light main_light = GetMainLight();
                float3 light_direction = normalize(main_light.direction);

                float diff = saturate(dot(normalize(input.normal_ws), light_direction));
                half4 albedo = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.texcoord);

                half3 color = albedo.rgb * main_light.color * diff;
                
                return half4(color, 1.0);
            }
            ENDHLSL
        }

        Pass
        {
            Cull Front
            ZWrite Off
            ZTest Always
            Blend SrcAlpha OneMinusSrcAlpha

            Stencil
            {
                Ref 4
                Comp NotEqual
                Fail Keep
                Pass Replace
            }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment  frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            float4 _SilColor;

            struct Attributes
            {
                float4 position_os : POSITION;
            };

            struct Varyings
            {
                float4 position_cs : SV_POSITION;
            };

            Varyings vert(Attributes input)
            {
                Varyings output;

                output.position_cs = TransformObjectToHClip(input.position_os);

                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                return _SilColor;
            }
            ENDHLSL
        }

        Pass
        {
            Cull Back
            ZWrite Off
            ZTest Always
            Blend SrcAlpha OneMinusSrcAlpha

            Stencil
            {
                Ref 4
                Comp NotEqual
                Pass Keep
            }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment  frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            float4 _SilColor;

            struct Attributes
            {
                float4 position_os : POSITION;
            };

            struct Varyings
            {
                float4 position_cs : SV_POSITION;
            };

            Varyings vert(Attributes input)
            {
                Varyings output;

                output.position_cs = TransformObjectToHClip(input.position_os);

                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                return _SilColor;
            }
            ENDHLSL
        }
    }
}