Shader "Unlit/ToonUnlitShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_OutlineSize("Outline Size", Float) = 0
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 100

			//creates outline
			Pass
			{
				Cull Front

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				float _OutlineSize;

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata v) {
				v2f o;

				float4 outlinePos = v.vertex;

				float disToCam = distance(_WorldSpaceCameraPos, v.vertex);
				float nominalDistance = 100.0f;

				float3 normal = normalize(v.normal);
				outlinePos += float4(normal, 0) * disToCam / nominalDistance * _OutlineSize;

				o.vertex = UnityObjectToClipPos(outlinePos);

				return o;
			}

			fixed4 frag(v2f i) : SV_Target{
				return fixed4(0,0,0,1);
			}

			ENDCG
		}

		// draws the model
		Pass
		{

			Cull Back

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);

				col.rgb = dot(col.rgb, float3(0.2326, 0.7152, 0.0722)); //light intensity constant. Dotting gets intensity of pixel color

				return col;
			}
			ENDCG
		}
	}
}
