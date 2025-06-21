Shader "Custom/BackgroundGradient"
{
    Properties
    {
        _TopColor ("Top Color", Color) = (0.3,0.3,0.3,1)
        _BottomColor ("Bottom Color", Color) = (0,0,0,1)
        _NoiseStrength ("Noise Strength", Range(0,0.2)) = 0.05
        _Scale ("Noise Scale", Float) = 100
        _CameraPos ("Camera Position", Vector) = (0,0,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Background" }
        Pass
        {
            ZWrite Off
            Cull Off
            ZTest Always

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; };
            struct v2f { float2 uv : TEXCOORD0; float4 vertex : SV_POSITION; };

            fixed4 _TopColor;
            fixed4 _BottomColor;
            float _NoiseStrength;
            float _Scale;
            float4 _CameraPos;

            float rand(float2 co)
            {
                return frac(sin(dot(co, float2(12.9898,78.233))) * 43758.5453);
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 worldUV = i.uv + _CameraPos.xy;
                float gradient = worldUV.y;
                fixed4 color = lerp(_BottomColor, _TopColor, gradient);
                float noise = rand(worldUV * _Scale);
                color.rgb += (noise - 0.5) * _NoiseStrength;
                return color;
            }
            ENDCG
        }
    }
}
