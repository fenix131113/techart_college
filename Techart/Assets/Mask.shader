Shader "Custom/Mask"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue" = "Geometry+10" "IgnoreProjector" = "True" }
        
        Pass
        {
            ColorMask 0
            ZWrite On
            ZTest Greater
            
            Stencil
            {
                Ref 1
                Comp Always
                Pass Replace
            }
        }
    }
    FallBack "Diffuse"
}
