Shader "Sprites/Lava"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _FogColor ("Fog Color", Color) = (0, 0, 0, 0.5)
        _SurfaceHeight ("Lava Height", Float) = -10
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
            fixed4 _FogColor;
            float _SurfaceHeight;

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

            fixed4 frag(v2f IN) : SV_Target
            {
                float PI = 3.14159;
                fixed4 c = 0;
                // float h = _SurfaceHeight - 0.5*abs(sin(IN.worldPos.x + _Time.y)) + 0.2 * cos(0.68 * IN.worldPos.x) + 0.2 * sin(_Time.w) * cos(0.1 * IN.worldPos.x);
                float h1 = _SurfaceHeight - 0.35 * sin(IN.worldPos.x + _Time.y) + 0.1 * cos(IN.worldPos.x * 109 / 67) + 0.02 * sin(IN.worldPos.x * 11);
                float h2 = _SurfaceHeight - 0.2 * sin(IN.worldPos.x / 2 + 1 - _Time.y) + 0.04 * cos(IN.worldPos.x * 3);
                float h3 = _SurfaceHeight - 0.2 * sin(IN.worldPos.x / 2.2 + 1.8 - _Time.y) + 0.04 * cos(IN.worldPos.x * 2.9 + 3);
                float dist = abs(IN.worldPos.y - h1);
                if (IN.worldPos.y < h1)
                {
                    fixed4 pos = IN.worldPos;
                    pos.x += sin(pos.y + sin(_Time.y)) * 0.2;
                    pos.y += sin(pos.x) * 0.2 + sin(_Time.z) * 0.1;
                    c = tex2D(_MainTex, pos  / 2);
                    if (dist < 0.2)
                    {
                        c *= _Color * 2;
                    }
                } 
                else {
                    if (abs(IN.worldPos.y - h2) < 1)
                    {
                        c.rgb = lerp(0, _FogColor.rgb, _FogColor.a);
                        c.a += _FogColor.a;
                    }
                    if (abs(IN.worldPos.y - h3) < 0.5)
                    {
                        c.rgb += lerp(0, _FogColor.rgb, _FogColor.a);
                        c.a += _FogColor.a;
                    }
                }
                
                return saturate(c);
            }
        ENDCG
        }
    }
}
