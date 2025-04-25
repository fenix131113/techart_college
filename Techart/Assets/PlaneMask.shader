Shader "Unlit/PlaneMask"
{
 SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry+11" }

        Pass
        {
            ZWrite On
            ZTest LEqual

            Stencil
            {
                Ref 1
                Comp Equal
                Pass Keep
            }
            
            Color (1, 0, 0, 1)
        }
    }
    FallBack "Diffuse"
}