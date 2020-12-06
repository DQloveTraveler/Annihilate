Shader "Custom/StandardDissolve" {
	Properties{
		[NoScaleOffset]_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		[PowerSlider(1)]_Cutoff("Cutoff", Range(0, 1)) = 0.5
		[NoScaleOffset]_BumpMap("Normal Map", 2D) = "white" {}

		[PowerSlider(1)]_Glossiness("Smoothness", Range(0,1)) = 0.5
		[PowerSlider(1)]_Metallic("Metallic", Range(0,1)) = 0.0
		[NoScaleOffset]_OcclusionMap("Occlusion", 2D) = "white" {}

		[NoScaleOffset]_SliceGuide("Dissolve Mask", 2D) = "white" {}
		[PowerSlider(1)]_DissolveAmount("Dissolve Amount", Range(0, 1)) = 0.0
		[NoScaleOffset]_BurnRamp("Dissolve Ramp", 2D) = "white" {}
		[PowerSlider(1)]_BurnSize("Dissolve Size", Range(0, 1)) = 0.15
		[PowerSlider(1)]_EmissionAmount("Dissolve Emission Power", Range(0, 2)) = 2.0
	}

	SubShader{
		Tags{ "RenderType" = "CutOut" }
		LOD 200

		CGPROGRAM
#pragma surface surf Standard addshadow alphatest:_Cutoff
#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _BumpMap;
		sampler2D _OcclusionMap;
		sampler2D _SliceGuide;
		sampler2D _BurnRamp;

		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float2 uv_OcclusionMap;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		half _DissolveAmount;
		half _BurnSize;
		half _EmissionAmount;

		void surf(Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);

			half test = tex2D(_SliceGuide, IN.uv_MainTex).rgb - _DissolveAmount;
			clip(test);

			// I skipped the _BurnColor here 'cause I was getting enough 
			// colour from the BurnRamp texture already.

			o.Albedo = c.rgb * _Color.rgb;
			o.Occlusion = tex2D(_OcclusionMap, IN.uv_OcclusionMap);
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
			o.Metallic = _Metallic;


			// My Albedo map has smoothness in its Alpha channel.
			o.Smoothness = _Glossiness * c.a;
			o.Alpha = c.a;

			if (test < _BurnSize && _DissolveAmount > 0) {
				o.Emission = tex2D(_BurnRamp, float2(test * (1 / _BurnSize), 0)) * _EmissionAmount;
			}
		}
		ENDCG
	}
	FallBack "Diffuse"
}
