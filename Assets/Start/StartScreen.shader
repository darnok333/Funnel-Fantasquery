Shader "Hidden/StartScreen"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)

		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255

		_ColorMask("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0
    }
    SubShader
    {

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Stencil
		{
			Ref[_Stencil]
			Comp[_StencilComp]
			Pass[_StencilOp]
			ReadMask[_StencilReadMask]
			WriteMask[_StencilWriteMask]
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest[unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask[_ColorMask]

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
			#include "UnityUI.cginc"

			#pragma multi_compile_local _ UNITY_UI_CLIP_RECT
			#pragma multi_compile_local _ UNITY_UI_ALPHACLIP

			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
				UNITY_VERTEX_OUTPUT_STEREO
			};


			sampler2D _MainTex;
			fixed4 _Color;
			fixed4 _TextureSampleAdd;
			float4 _ClipRect;
			float4 _MainTex_ST;

			v2f vert(appdata_t v)
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				OUT.worldPosition = v.vertex;
				OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

				OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

				OUT.color = v.color * _Color;
				return OUT;
			}

            fixed4 frag (v2f i) : SV_Target
            {
                /*fixed4 col = tex2D(_MainTex, i.uv);
                // just invert the colors
                col.rgb = 1 - col.rgb;
                return col;*/


				float2 uv = (i.texcoord - .5) * float2(_ScreenParams.x / _ScreenParams.y, 1) * 10;

				uv = mul(uv, float2x2(.707, -.707, .707, .707));
				uv *= 15.;

				float2 gv = frac(uv) - .5;
				float2 id = floor(uv);

				float m = 0.;
				float t;
				for (float y = -1.; y <= 1.; y++) {
					for (float x = -1.; x <= 1.; x++) {
						float2 offs = float2(x, y);

						t = -_Time.y/5.0 + length(id - offs)*.2;
						float r = lerp(.4, 1.5, sin(t)*.5 + .5);
						float c = smoothstep(r, r*.9, length(gv + offs));
						m = m * (1. - c) + c * (1. - m);
					}
				}

				half4 color = tex2D(_MainTex, i.texcoord) + _TextureSampleAdd;

				color = fixed4(
					sin(color.r + (1.12*_Time.y/4.0)),
					sin(color.g + (0.97*_Time.w/8.0)),
					1 - sin(color.b + (_Time.z/8.0)),
					color.a
					);




				return m * color * 0.5 * i.color;
            }
            ENDCG
        }
    }
}
