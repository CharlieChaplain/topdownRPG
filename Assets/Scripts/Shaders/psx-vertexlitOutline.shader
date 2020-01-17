// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "psx/vertexlitOutline" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_OutlineSize("Outline Size", Float) = 0
	}
		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 200

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

					//Vertex snapping
					float4 snapToPixel = UnityObjectToClipPos(v.vertex);
					float4 vertex = snapToPixel;
					vertex.xyz = snapToPixel.xyz / snapToPixel.w;
					vertex.x = floor(160 * vertex.x) / 160;
					vertex.y = floor(120 * vertex.y) / 120;
					vertex.xyz *= snapToPixel.w;
					o.vertex = vertex;

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

			//draws model
			Pass {
			Lighting On
				CGPROGRAM

					#pragma vertex vert
					#pragma fragment frag
					#include "UnityCG.cginc"

					struct v2f
					{
						fixed4 pos : SV_POSITION;
						half4 color : COLOR0;
						half4 colorFog : COLOR1;
						float2 uv_MainTex : TEXCOORD0;
						half3 normal : TEXCOORD1;
					};

					float4 _MainTex_ST;
					uniform half4 unity_FogStart;
					uniform half4 unity_FogEnd;

					v2f vert(appdata_full v)
					{
						v2f o;

						//Vertex snapping
						float4 snapToPixel = UnityObjectToClipPos(v.vertex);
						float4 vertex = snapToPixel;
						vertex.xyz = snapToPixel.xyz / snapToPixel.w;
						vertex.x = floor(160 * vertex.x) / 160;
						vertex.y = floor(120 * vertex.y) / 120;
						vertex.xyz *= snapToPixel.w;
						o.pos = vertex;

						//Vertex lighting 
					//	o.color =  float4(ShadeVertexLights(v.vertex, v.normal), 1.0);
						o.color = float4(ShadeVertexLightsFull(v.vertex, v.normal, 4, true), 1.0);
						o.color *= v.color;

						float distance = length(mul(UNITY_MATRIX_MV,v.vertex));

						//Affine Texture Mapping
						float4 affinePos = vertex; //vertex;				
						o.uv_MainTex = TRANSFORM_TEX(v.texcoord, _MainTex);
						o.uv_MainTex *= distance + (vertex.w*(UNITY_LIGHTMODEL_AMBIENT.a * 8)) / distance / 2;
						o.normal = distance + (vertex.w*(UNITY_LIGHTMODEL_AMBIENT.a * 8)) / distance / 2;

						//Fog
						float4 fogColor = unity_FogColor;

						float fogDensity = (unity_FogEnd - distance) / (unity_FogEnd - unity_FogStart);
						o.normal.g = fogDensity;
						o.normal.b = 1;

						o.colorFog = fogColor;
						o.colorFog.a = clamp(fogDensity,0,1);

						//Cut out polygons     <------ this makes polygons go weird from far away. Possibly useful if it stops them rendering if they're far enough away
						/*if (distance > unity_FogStart.z + unity_FogColor.a * 255)
						{
							o.pos.w = 0;
						}
						*/
						return o;
					}

					sampler2D _MainTex;

					float4 frag(v2f IN) : COLOR
					{
						half4 c = tex2D(_MainTex, IN.uv_MainTex / IN.normal.r)*IN.color;
						half4 color = c*(IN.colorFog.a);
						color.rgb += IN.colorFog.rgb*(1 - IN.colorFog.a);
						return color;
					}
				ENDCG
			}
	}
}