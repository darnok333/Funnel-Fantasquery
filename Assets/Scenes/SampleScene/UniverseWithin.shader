Shader "CustomPostProcess/UniverseWithin"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Fade("Fade", FLOAT) = 0
    }
    SubShader
    {
        // No culling or depth
		Blend SrcAlpha OneMinusSrcAlpha
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

			// The Universe Within - by Martijn Steinrucken aka BigWings 2018
			// Email:countfrolic@gmail.com Twitter:@The_ArtOfCode
			// License Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.

			// After listening to an interview with Michael Pollan on the Joe Rogan
			// podcast I got interested in mystic experiences that people seem to
			// have when using certain psycoactive substances. 
			//
			// For best results, watch fullscreen, with music, in a dark room.
			// 
			// I had an unused 'blockchain effect' lying around and used it as
			// a base for this effect. Uncomment the SIMPLE define to see where
			// this came from.
			// YouTube video of this effect:
			// https://youtu.be/GAhu4ngQa48
			//
			// YouTube Tutorial for this effect:
			// https://youtu.be/3CycKKJiwis


			#define S(a, b, t) smoothstep(a, b, t)
			#define NUM_LAYERS 4.

			float N21(float2 p) {
				float3 a = frac(float3(p.xyx) * float3(213.897, 653.453, 253.098));
				a += dot(a, a.yzx + 79.76);
				return frac((a.x + a.y) * a.z);
			}

			float2 GetPos(float2 id, float2 offs, float t) {
				float n = N21(id + offs);
				float n1 = frac(n*10.);
				float n2 = frac(n*100.);
				float a = t + n;
				return offs + float2(sin(a*n1), cos(a*n2))*.4;
			}

			float GetT(float2 ro, float2 rd, float2 p) {
				return dot(p - ro, rd);
			}

			float LineDist(float3 a, float3 b, float3 p) {
				return length(cross(b - a, p - a)) / length(p - a);
			}

			float df_line(in float2 a, in float2 b, in float2 p)
			{
				float2 pa = p - a, ba = b - a;
				float h = clamp(dot(pa, ba) / dot(ba, ba), 0., 1.);
				return length(pa - ba * h);
			}

			float lineFunc(float2 a, float2 b, float2 uv) {
				float r1 = .04;
				float r2 = .01;

				float d = df_line(a, b, uv);
				float d2 = length(a - b);
				float fade = S(1.5, .5, d2);

				fade += S(.05, .02, abs(d2 - .75));
				return S(r1, r2, d)*fade;
			}

			float NetLayer(float2 st, float n, float t) {
				float2 id = floor(st) + n;

				st = frac(st) - .5;

				float2 p[9];
				int i = 0;
				for (float y = -1.; y <= 1.; y++) {
					for (float x = -1.; x <= 1.; x++) {
						p[i++] = GetPos(id, float2(x, y), t);
					}
				}

				float m = 0.;
				float sparkle = 0.;

				for (i = 0; i < 9; i++) {
					m += lineFunc(p[4], p[i], st);

					float d = length(st - p[i]);

					float s = (.005 / (d*d));
					s *= S(1., .7, d);
					float pulse = sin((frac(p[i].x) + frac(p[i].y) + t)*5.)*.4 + .6;
					pulse = pow(pulse, 20.);

					s *= pulse;
					sparkle += s;
				}

				m += lineFunc(p[1], p[3], st);
				m += lineFunc(p[1], p[5], st);
				m += lineFunc(p[7], p[5], st);
				m += lineFunc(p[7], p[3], st);

				float sPhase = (sin(t + n) + sin(t*.1))*.25 + .5;
				sPhase += pow(sin(t*.1)*.5 + .5, 50.)*5.;
				m += sparkle * sPhase;//(*.5+.5);

				return m;
			}



            fixed4 frag (v2f i) : SV_Target
            {
				float2 uv = (i.uv - .5) * (_ScreenParams.x / _ScreenParams.y) * 0.5;
				float2 M = 0;// iMouse.xy / iResolution.xy - .5;

				float t = _Time.y * .1;

				float s = sin(t);
				float c = cos(t);
				float2x2 rot = float2x2(c, -s, s, c);
				float2 st = mul(uv, rot);
				M = mul(M, rot * 2.);

				float m = 0.;
				for (float j = 0.; j < 1.; j += 1. / NUM_LAYERS) {
					float z = frac(t + j);
					float size = lerp(15., 1., z);
					float fade = S(0., .6, z)*S(1., .8, z);

					m += fade * NetLayer(st*size - M * z, j, _Time.y);
				}

				float fft = 1;// texelFetch(iChannel0, ivec2(.7, 0), 0).x;
				float glow = -uv.y*fft*2.;

				float3 baseCol = float3(s, cos(t*.4), -sin(t*.24))*.4 + .6;
				float3 col = baseCol * m;
				col += baseCol * glow;

				uv *= 10.;
				col = float3(1,1,1)*NetLayer(uv, 0., _Time.y);
				uv = frac(uv);

				float4 colEffect = float4(col, col.r) * col.r * _Fade;


				fixed4 colOri = tex2D(_MainTex, i.uv);


				return lerp(colOri, 1, colEffect.r);
            }
            ENDCG
        }
    }
}