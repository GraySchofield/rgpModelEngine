Shader "Custom/MyWaterShader2D"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color	 ("Tint", Color) = (1,1,1,1)
		_GlowColor ("Glow Color", Color) = (1,1,1,1)
		_BlendTex ("Blend Texture", 2D) = "white" {}
		_NoiseLevel ("Noise Level", Range(50,2000)) = 300
		_NoiseSpeed ("Noise Speed", Range(10,100)) = 60
		_OutlineSize ( "Outline Size", int) = 10
		_MainTex_TexelSize ("Glow Size", Vector) = (1,1,1,1)

		_MaskTexture ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Opaque"}
		Cull Off ZWrite Off ZTest Always 

		Blend SrcAlpha OneMinusSrcAlpha 

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
				float2 uv_blend : TEXCOORD1;
				float2 uv_mask : TEXCOORD2;
				fixed4 color : COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 uv_blend : TEXCOORD1;
				float2 uv_mask : TEXCOORD2;

				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv ;
				o.uv_blend = v.uv_blend;
				o.uv_mask = v.uv_mask;
				o.color = v.color;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _BlendTex;
			sampler2D _MaskTexture;

			fixed4	  _Color;
			fixed4 	  _GlowColor;
			fixed 	  _NoiseLevel;
			fixed 	  _NoiseSpeed;
			fixed4 	  _MainTex_TexelSize;
			int 	  _OutlineSize;

			fixed4 frag (v2f IN) : SV_Target
			{

			//Change Color as distortion
				float2 uv_displacement = IN.uv + float2(
   					 	tex2D(_BlendTex, IN.vertex.xy/_NoiseLevel + float2(-(_Time.w%_NoiseSpeed)/_NoiseSpeed, 0)).r - .5,                            
    					tex2D(_BlendTex, IN.vertex.xy/_NoiseLevel + float2(0, (_Time.w%_NoiseSpeed)/_NoiseSpeed)).r - .5
				)/20;

				fixed4 col = tex2D(_MainTex, uv_displacement) * IN.color;


				//Changing outline
//				{
//					fixed4 col = tex2D(_MainTex, IN.uv) * IN.color;
//
//					float totalAlpha = 1.0;
//					int cutIndex = 0;
//					for(int i = 1 ; i < _OutlineSize ; i ++){
//						fixed4 upC = tex2D(_MainTex, IN.uv + fixed2(0, i * _MainTex_TexelSize.y));
//						fixed4 downC = tex2D(_MainTex, IN.uv + fixed2(0, -i * _MainTex_TexelSize.y));
//						fixed4 rightC = tex2D(_MainTex, IN.uv + fixed2(i * _MainTex_TexelSize.x, 0));
//						fixed4 leftC = tex2D(_MainTex, IN.uv + fixed2(- i * _MainTex_TexelSize.x, 0));
//
//						totalAlpha = totalAlpha * upC.a * downC.a * rightC.a * leftC.a;
//
//						if(totalAlpha == 0)
//							cutIndex = i;
//					}
//
//					if(totalAlpha == 0){
//						col.rgb =  _GlowColor.rgb;
//					}
//				}

				//Noise Line
				//fixed4 col = tex2D(_MainTex, IN.uv) * IN.color;

				fixed4 crackNoise = tex2D(_MaskTexture, IN.uv_mask);
				if(crackNoise.r  > 0.4){
					col *= _GlowColor;
				}

			
				return col * _Color;
			}
			ENDCG
		}
	}
}
