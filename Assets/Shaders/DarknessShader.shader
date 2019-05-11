// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Sprites/Darkness"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        _Scale ("Scale", Float) = 1
        _Offset ("Texture Offset", Vector) = (0, 0, 0, 0)
        _Dark ("Dark", Color) = (0, 0, 0, 1)
        _Light ("Light", Color) = (1, 1, 1, 0)
        _LowerBorder ("Lava Border", Float) = -10
    }

    SubShader
    {
        Tags
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Fog { Mode Off }
        Blend One OneMinusSrcAlpha

        Pass
        {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile DUMMY PIXELSNAP_ON
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                half2 texcoord  : TEXCOORD0;
                fixed4 worldPos : TEXCOORD1;
            };

            fixed4 _Color;
            fixed4 _Light;
            fixed4 _Dark;

            float3 _Lights[100];
            int _LightCount;

            float _LowerBorder;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.worldPos = mul(unity_ObjectToWorld, IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color;
                #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap (OUT.vertex);
                #endif
                return OUT;
            }

            sampler2D _MainTex;

            float borderHeight(float x)
            {
                return _LowerBorder + 0.1 * sin(x) + 0.1 * cos(_Time.w) * cos(x * 0.77);
            }

            float light (float distance, float size)
            {
                float b = 2;
                float PI = 3.14159;

                if(distance < size - b)
                {
                    return 0;
                }
                float l = (distance - size + b) / b;
                l = saturate(l);
                // l *= l;
                l = (1 - cos(PI * l)) / 2;
                return l;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 c = _Dark;
                float fac = 1;
                for (int i = 0; i < _LightCount; i++)
                {
                    float2 dif = IN.worldPos.xy - _Lights[i].xy;
                    fac *= light(length(dif), _Lights[i].z);
                }
                fac *= saturate(abs(IN.worldPos.y - borderHeight(IN.worldPos.x)) / 4);
                fac = saturate(fac);
                c.rgba = fac * _Dark + (1-fac) * _Light;
                // c.rgb *= c.a;
                return c;
            }
        ENDCG
        }
    }
}
