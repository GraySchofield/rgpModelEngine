Shader "Custom/Distor2D"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color	 ("Tint", Color) = (1,1,1,1)
		_BlendTex ("Blend Texture", 2D) = "white" {}
		_NoiseLevel ("Noise Level", Range(50,2000)) = 300
		_NoiseSpeed ("Noise Speed", Range(10,100)) = 60
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
				fixed4 color : COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 uv_blend : TEXCOORD1;
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv ;
				o.uv_blend = v.uv_blend;
				o.color = v.color;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _BlendTex;
			fixed4	  _Color;
			fixed 	  _NoiseLevel;
			fixed 	  _NoiseSpeed;

			fixed4 frag (v2f i) : SV_Target
			{
				float2 uv_displacement = i.uv + float2(
   					 	tex2D(_BlendTex, i.vertex.xy/_NoiseLevel + float2(-(_Time.w%_NoiseSpeed)/_NoiseSpeed, 0)).r - .5,                            
    					tex2D(_BlendTex, i.vertex.xy/_NoiseLevel + float2(0, (_Time.w%_NoiseSpeed)/_NoiseSpeed)).r - .5
				)/20;
				fixed4 col = tex2D(_MainTex, uv_displacement) * i.color;
				return col * _Color;
			}
			ENDCG
		}
	}
}
