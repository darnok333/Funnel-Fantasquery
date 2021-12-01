Shader "CustomPostProcess/TwistedToroid"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Fade("Fade", FLOAT) = 0
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

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 colOri = tex2D(_MainTex, i.uv);

				float2 uv = (i.uv - .5) * (_ScreenParams.x / _ScreenParams.y);
				float t = _Time.y * .1;

				uv = mul(uv, float2x2(cos(t), -sin(t), sin(t), cos(t)));

				float3 ro = float3(0, 0, -1);
				float3 lookat = lerp(float3(0, 0, 0), float3(-1, 0, -1), sin(t*1.56)*.5 + .5);
				float zoom = lerp(.2, .7, sin(t)*.5 + .5);

				float3 f = normalize(lookat - ro);
				float3 r = normalize(cross(float3(0, 1, 0), f));
				float3 u = cross(f, r);
				float3 c = ro + f * zoom;
				float3 ii = c + uv.x * r + uv.y * u;
				float3 rd = normalize(ii - ro);

				float radius = lerp(.3, 1.5, sin(t*.4)*.5 + .5);

				float dS, dO;
				float3 p;

				for (int i = 0; i < 100; i++) {
					p = ro + rd * dO;
					dS = -(length(float2(length(p.xz) - 1., p.y)) - radius);
					if (dS < .001) break;
					dO += dS;
				}

				float col = 0;

				if (dS < .001) {
					float x = atan2(p.x, p.z) + t * .5;			// -pi to pi
					float y = atan2(length(p.xz) - 1., p.y);

					float bands = sin(y*10. + x * 30.);
					float ripples = sin((x*10. - y * 30.)*3.)*.5 + .5;
					float waves = sin(x*2. - y * 6. + t * 20.);

					float b1 = smoothstep(-.2, .2, bands);
					float b2 = smoothstep(-.2, .2, bands - .5);

					float m = b1 * (1. - b2);
					m = max(m, ripples*b2*max(0., waves));
					m += max(0., waves*.3*b2);

					col += lerp(m, 1. - m, smoothstep(-.3, .3, sin(x + t)));
				}

				return lerp(colOri, 1-colOri, col * _Fade);
            }
            ENDCG
        }
    }
}