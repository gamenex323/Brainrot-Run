Shader "TSF/Base1" {
	Properties {
		[MaterialToggle(_TEX_ON)] _DetailTex ("Enable Detail texture", Float) = 0
		_MainTex ("Detail", 2D) = "white" {}
		_ToonShade ("Shade", 2D) = "white" {}
		[MaterialToggle(_COLOR_ON)] _TintColor ("Enable Color Tint", Float) = 0
		_Color ("Base Color", Vector) = (1,1,1,0.1)
		[MaterialToggle(_VCOLOR_ON)] _VertexColor ("Enable Vertex Color", Float) = 0
		_Brightness ("Brightness 1 = neutral", Float) = 1
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Legacy Shaders/Diffuse"
}