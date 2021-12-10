Shader "Custom/MyWaterShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_CausticTex ("Caustic Texture", 2D) = "white" {}
		_Colour	("Colour", Color) = (1,1,1,1)
		_BumpMap ("Noise Texture", 2D) = "bump" {}
		_Magnitude("Magnitude", Range(0,1)) = 0.05
		_WaterPeriod("Water Period", Range(0,10)) = 5
		_offset ("Offset", Range(0,10)) = 1
		
		
	}
	SubShader
	{
		// No culling or depth
		Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Opaque"}
		ZWrite On Lighting Off Cull Off Fog { Mode Off } Blend One Zero

		GrabPass {"_GrabTexture"}

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _GrabTexture;
			sampler2D _CausticTex;
			fixed4 _Colour;
			sampler2D _BumpMap;
			float _Magnitude;
			float _offset;
			float _WaterPeriod;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				float4 uvgrab : TEXCOORD1;
			};


			float2 sinusoid (float2 x, float2 m, float2 M, float2 p) {
				float2 e   = M - m;
				float2 c = 3.1415 * 2.0 / p;
				return e / 2.0 * (1.0 + sin(x * c)) + m;
			}

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
				o.uv = v.uv;
				o.uvgrab = ComputeGrabScreenPos(o.vertex);
				return o;
			}

			half4 frag (v2f i) : COLOR
			{
				half4 mainColour = tex2D(_MainTex, i.uv);
				half4 bump = tex2D(_BumpMap, i.uv);
				
				float time = _Time[1];
				
				float2 distortion = UnpackNormal(bump).rg;

				float2 waterDisplacement = 
				sinusoid
				(
					float2 (time, time) + (distortion) * _offset,
					float2(-_Magnitude, -_Magnitude),
					float2(+_Magnitude, +_Magnitude),
					float2(_WaterPeriod, _WaterPeriod)
				);
				
				i.uvgrab.xy += waterDisplacement;
				
				half4 col = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(i.uvgrab));
				half4 causticColour = tex2D(_CausticTex, i.uv.xy*0.25 + waterDisplacement*5);
				return col * mainColour * _Colour * causticColour;
			}

			
			ENDCG
		}
	}
}
