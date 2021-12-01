Shader "CustomPostProcess/HexFade"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Fade("Fade", Range(-1.5, 1.5)) = 0
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
			float _Fade;



			float HexDist(float2 p) {
				p = abs(p);

				float c = dot(p, normalize(float2(1, 1.73)));
				c = max(c, p.x);

				return c;
			}

			float4 HexCoords(float2 uv) {
				float2 r = float2(1, 1.73);
				float2 h = r * .5;

				float2 a = fmod(uv, r) - h;
				float2 b = fmod(uv - h, r) - h;

				float2 gv = dot(a, a) < dot(b, b) ? a : b;

				float x = atan2(gv.x, gv.y);
				float y = .5 - HexDist(gv);
				float2 id = uv - gv;
				return float4(x, y, id.x, id.y);
			}


            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 colOri = tex2D(_MainTex, i.uv);


				float2 uv = (i.uv - .5) * float2(_ScreenParams.x/_ScreenParams.y, 1);

				uv *= 10.;

				float4 hc = HexCoords(uv + 100.);

				float c = smoothstep(.01, .03, hc.y*min(1, hc.z*hc.w) - _Fade + sin(hc.z*hc.w));


				return colOri * c;
            }
            ENDCG
        }
    }
}