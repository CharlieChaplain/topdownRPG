Shader "Post Processes/PixelPostProcess"
{
	Properties
	{
		[HideInInspector]_MainTex ("Texture", 2D) = "white" {}
		_ResolutionX("Resolution X", Int) = 640
		_ResolutionY("Resolution Y", Int) = 480
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
			int _ResolutionX;
			int _ResolutionY;

			fixed4 frag (v2f i) : SV_Target
			{
				float2 uv;
				uv.x = trunc(i.uv.x * _ResolutionX) / _ResolutionX;
				uv.y = trunc(i.uv.y * _ResolutionY) / _ResolutionY;

				fixed4 col = tex2D(_MainTex, uv);
				return col;
			}
			ENDCG
		}
	}
}
