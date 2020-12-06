
Shader "Custom/RuneShader" {
	Properties{
		_MainTex1("Albedo1", 2D) = "white" {}
		_Color1("Color1", Color) = (1, 1, 1, 1)
		_MainTex2("Albedo2", 2D) = "white" {}
		_Color2("Color2", Color) = (1, 1, 1, 1)
		_RotateSpeed("Rotate Speed", float) = 1.0
	}

		SubShader{
			Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
			LOD 100

			Lighting Off
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			Pass{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 2.0
				#pragma multi_compile_fog

				#include "UnityCG.cginc"

				struct appdata_t {
					float4 vertex : POSITION;
					float4 color : COLOR;
					float2 uv : TEXCOORD0;
					UNITY_VERTEX_INPUT_INSTANCE_ID
				};

				struct v2f {
					float4 vertex : SV_POSITION;
					float4 color : COLOR;
					float2 uv : TEXCOORD0;
					UNITY_FOG_COORDS(1)
					UNITY_VERTEX_OUTPUT_STEREO
				};

				sampler2D _MainTex1;
				fixed4 _Color1;
				float4 _MainTex1_ST;

				v2f vert(appdata_t v)
				{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex1);
					o.color = v.color;
					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					fixed4 col = tex2D(_MainTex1, i.uv);
					col *= _Color1;

					float time = abs(sin(_Time.x * 20)) / 2;
					col += fixed4(1, 1, 1, 0) * time;

					UNITY_APPLY_FOG(i.fogCoord, col);
					return col;
				}

				ENDCG
			}


			Pass{
					CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag
					#pragma target 2.0
					#pragma multi_compile_fog

					#define PI 3.141592

					#include "UnityCG.cginc"

					struct appdata_t {
						float4 vertex : POSITION;
						float4 color : COLOR;
						float2 uv : TEXCOORD0;
						UNITY_VERTEX_INPUT_INSTANCE_ID
					};

					struct v2f {
						float4 vertex : SV_POSITION;
						float4 color : COLOR;
						float2 uv : TEXCOORD0;
						UNITY_FOG_COORDS(1)
						UNITY_VERTEX_OUTPUT_STEREO
					};

					sampler2D _MainTex2;
					fixed4 _Color2;
					float4 _MainTex2_ST;
					float  _RotateSpeed;

					v2f vert(appdata_t v)
					{
						v2f o;
						UNITY_SETUP_INSTANCE_ID(v);
						UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
						o.vertex = UnityObjectToClipPos(v.vertex);
						o.uv = TRANSFORM_TEX(v.uv, _MainTex2);
						o.color = v.color;
						UNITY_TRANSFER_FOG(o,o.vertex);
						return o;
					}

					fixed4 frag(v2f i) : SV_Target
					{
						half angle = frac(_Time.x) * PI * 2;
						// 回転行列を作る
						half angleCos = cos(angle * _RotateSpeed);
						half angleSin = sin(angle * _RotateSpeed);
						half2x2 rotateMatrix = half2x2(angleCos, -angleSin, angleSin, angleCos);

						// 中心を起点にUVを回転させる
						i.uv = mul(i.uv - 0.5, rotateMatrix) + 0.5;

						fixed4 col = tex2D(_MainTex2, i.uv);
						col *= _Color2;
						UNITY_APPLY_FOG(i.fogCoord, col);
						return col;
					}

					ENDCG
				}
		}
}
