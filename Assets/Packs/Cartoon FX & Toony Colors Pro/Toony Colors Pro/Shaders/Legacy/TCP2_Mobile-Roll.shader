
// Toony Colors Pro+Mobile 2
// (c) 2014-2019 Jean Moreno


Shader "Toony Colors Pro 2/Legacy/Mobile-Roll"
{
	Properties
	{
		//TOONY COLORS
		_Color("Color", Color) = (1,1,1,1)
		_HColor("Highlight Color", Color) = (0.785,0.785,0.785,1.0)
		_SColor("Shadow Color", Color) = (0.195,0.195,0.195,1.0)

		//DIFFUSE
		_MainTex("Main Texture (RGB) Spec/MatCap Mask (A) ", 2D) = "white" {}

	//TOONY COLORS RAMP
	[TCP2Gradient] _Ramp("#RAMPT# Toon Ramp (RGB)", 2D) = "gray" {}
	_RampThreshold("#RAMPF# Ramp Threshold", Range(0,1)) = 0.5
	_RampSmooth("#RAMPF# Ramp Smoothing", Range(0.01,1)) = 0.1

		//BUMP
		_BumpMap("#NORM# Normal map (RGB)", 2D) = "bump" {}

		_Displacement("Displacement", Range(0, 1.0)) = 0.3
		_Tess("Tessellation", Range(1,32)) = 4

		_startAngle("StartAngle",Float) = 90
		_angleDiff("AngleDiff",Float) = 30
		_right("Right",Vector) = (0,0,0)
		_RollCenterPosZ("RollCenterPosZ", Float) = 0
		_Spin("Spin", Float) = 0
		_ZLength("Z Length", Float) = 0
		_yOffSet("Y OffSet", Float) = 0
		_scale("Scale",Vector) = (0,0,0)
	}

		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 200

			CGPROGRAM

			#include "Include/TCP2_Include.cginc"

			#pragma surface surf ToonyColors noforwardadd interpolateview halfasview vertex:disp tessellate:tessFixed 
			#pragma target 4.6
			//#pragma target 2.0

			#pragma shader_feature TCP2_DISABLE_WRAPPED_LIGHT
			#pragma shader_feature TCP2_RAMPTEXT
			#pragma shader_feature TCP2_BUMP

			//================================================================
			// VARIABLES



				struct appdata {
					float4 vertex : POSITION;
					float4 tangent : TANGENT;
					float3 normal : NORMAL;
					float2 texcoord : TEXCOORD0;
					float2 texcoord1 : TEXCOORD1;
					float2 texcoord2 : TEXCOORD2;
				};
				float _Tess;

				float _startAngle;
				float _angleDiff;
				float3 _right;
				float _RollCenterPosZ;
				float _Spin;
				float _ZLength;
				float _yOffSet;
				float3 _scale;

				float4 tessFixed()
				{
					return _Tess;
				}

				sampler2D _DispTex;
				float _Displacement;

				float InverseLerp(float from, float to, float value)
				{
					return (value - from) / (to - from);
				}

				float Remap(float origFrom, float origTo, float targetFrom, float targetTo, float value)
				{
					float rel = InverseLerp(origFrom, origTo, value);
					return lerp(targetFrom, targetTo, rel);
				}

				float3 DoRoll_float(appdata v)
				{
					float3 vertexPos = v.vertex.xyz;
					// // Rulo efekti
					if (_RollCenterPosZ > vertexPos.z * _scale.z)
					{
						float angleRadian = radians(_startAngle + abs(vertexPos.z * _scale.z - _RollCenterPosZ) * _Spin);
						float3 centerPoint = float3(vertexPos.x * _scale.x, _yOffSet, _RollCenterPosZ) /*- mul(unity_WorldToObject, ((_RollCenterPosZ - vertexPos.z*_scale.z) * sin(radians(_angleDiff))) * _right *_scale.x)*/; // Rulomuzu döndürmek için merkez pozisyon
						float incrementalRadius = Remap(0, _ZLength, 1, 3, vertexPos.z * _scale.z) - vertexPos.y * _scale.y + _yOffSet; // vertex.z ile toplamamızın sebebi x'leri aynı olan vertexlerin radiyuslarının farklı olması
						vertexPos = float3(centerPoint.x, centerPoint.y, centerPoint.z) + float3(0, -sin(angleRadian), cos(angleRadian)) * incrementalRadius;

						// Max radiyusu bulup öteliyoruz
						float maxRadius = Remap(0, _ZLength, 1 , 3, _RollCenterPosZ);
						vertexPos.y += maxRadius;

						return(float3(vertexPos.x / _scale.x, vertexPos.y / _scale.y, vertexPos.z / _scale.z));
					}
					return(vertexPos);
				}

				void disp(inout appdata v)
				{
					float d = tex2Dlod(_DispTex, float4(v.texcoord.xy,0,0)).r * _Displacement;
					v.vertex.xyz += v.normal * d;

					v.vertex.xyz = DoRoll_float(v);

					if (_RollCenterPosZ > v.vertex.z * _scale.z)
					{
						//float angleRadian = radians(_startAngle + abs(v.vertex.z * _scale.z - _RollCenterPosZ) * _Spin);
						//v.normal = -float3(0, -sin(angleRadian), cos(angleRadian));
						v.normal = mul(unity_WorldToObject, float3(0,1,-1));
					}
				}


			fixed4 _Color;
			sampler2D _MainTex;

		#if TCP2_BUMP
			sampler2D _BumpMap;
		#endif

			struct Input
			{
				half2 uv_MainTex : TEXCOORD0;
		#if TCP2_BUMP
				half2 uv_BumpMap : TEXCOORD1;
		#endif
			};

			//================================================================
			// SURFACE FUNCTION

			void surf(Input IN, inout SurfaceOutput o)
			{
				half4 main = tex2D(_MainTex, IN.uv_MainTex);
				o.Albedo = main.rgb * _Color.rgb;
				o.Alpha = main.a * _Color.a;

		#if TCP2_BUMP
				//Normal map
				o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
		#endif
			}

			ENDCG

		}

			Fallback "Diffuse"
				CustomEditor "TCP2_MaterialInspector"
}