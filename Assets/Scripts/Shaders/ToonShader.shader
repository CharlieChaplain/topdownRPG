Shader "Custom/ToonShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert finalcolor:toonerize

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		
		struct Input {
			float2 uv_MainTex;
		};

		void toonerize (Input IN, SurfaceOutput o, inout fixed4 color)
		{
			float3 diffuse = color.rgb;
			float intensity = dot(diffuse, float3(0.2326, 0.7152, 0.0722)); //light intensity constant. Dotting gets intensity of pixel color
			color.rgb = intensity;
		}

		void surf(Input IN, inout SurfaceOutput o) {
			half4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}

		ENDCG
	}
	FallBack "Diffuse"
}
