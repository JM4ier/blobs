Shader "FX/HeatDistortion"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Height ("Distortion Offset", Float) = -20
        _Spread ("Distortion Spread", Float) = 5
        _Distortion ("Distortion", Float) = 1
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float _Height;
            float _Spread;
            float _Distortion;

            float2 distortion(float2 pos)
            {
                // return float2(0.1 * sin(_Time.w), cos(pos.y) * cos(_Time.w));
                return float2(sin(100*pos.x - sin(_Time.w)), cos(100*pos.y + _Time.z));
            }

            float distortion_factor (float2 pos)
            {
                float fac = pos.y - (_Height + 0.02 * sin(pos.x * 10 + _Time.y) + 0.007 * sin(pos.x * 57 + _Time.w));
                if (fac < 0) fac = 0;
                fac /= _Spread;
                fac = saturate(fac);
                return 1-fac;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float fac = distortion_factor(i.uv);
                fixed4 col = tex2D(_MainTex, i.uv + _Distortion * fac * distortion(i.uv));
                // col = fac;
                return col;
            }
            ENDCG
        }
    }
}
