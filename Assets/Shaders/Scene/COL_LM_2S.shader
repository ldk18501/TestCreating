// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


Shader "Triniti/Scene/COL_LM_2S"
{
	Properties
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("MainTex(RGB)", 2D) = "" {}
		_LightMap ("Lightmap (RGB)", 2D) = "white" {}
	}

	//SubShader
	//{
	//	Tags { "Queue" = "Geometry" }
	
	//	Pass
	//	{
	//		Cull Off
	//		Lighting Off
	//		Fog{ Mode Off }
			
	//		BindChannels
	//		{
	//			Bind "vertex", vertex
	//			Bind "texcoord", texcoord0
	//			Bind "texcoord1", texcoord1
	//		}
			
	//		SetTexture [_MainTex]{
	//		 ConstantColor [_Color]
	//		 combine texture * constant
	//		 }
	//		SetTexture [_LightMap] { combine texture * previous }
	//	}
	//}

	SubShader {
		Lighting off
		Tags { "RenderType"="Opaque" "Reflection" = "RenderReflectionOpaque" "Queue"="Geometry" }	
				
		Pass {
			Lighting off
			Cull Off
			Fog{ Mode Off }
			
		
			CGPROGRAM
			#include "UnityCG.cginc"
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest 
			
			uniform half4 _MainTex_ST;
			uniform sampler2D _MainTex;
			uniform half4 _LightMap_ST;
			uniform sampler2D _LightMap;
			
			uniform float4 _Color;
			
			struct V2In
			{
				half4 vertex : POSITION;
				half2 texcoord : TEXCOORD0;
				half2 texcoord1 : TEXCOORD1;
			};
									
			struct v2f
			{
				half4 pos : POSITION;
				half2 uv : TEXCOORD1;
				half2 uv1 : TEXCOORD2;
			};
			
			v2f vert (V2In v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);

				o.uv = TRANSFORM_TEX(v.texcoord,_MainTex);
				
				o.uv1 = TRANSFORM_TEX(v.texcoord1,_LightMap);
				
				return o; 
			}
			
			fixed4 frag (v2f i) : COLOR 
			{				
				fixed4 tex = tex2D(_MainTex, i.uv.xy);
				
				tex *= tex2D(_LightMap, i.uv1.xy);

				return _Color * tex;
			}
			
			ENDCG
		}
		

	}
	

}
