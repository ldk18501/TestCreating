// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Triniti/Model/ModelEdge_Additive"
{
    Properties
    {
        _MainTex("Texture (RGB)", 2D) = "black" {}
        _Color("Color", Color) = (1, 1, 1, 1)
        _AtmoColor("Atmosphere Color", Color) = (0.5, 0.5, 1.0, 1)
		_TH ("Range TH",Range (0, 1))  = 1
		_ZP ("Range Tp",Range (0, 2))  = 1
    }
   
 SubShader
    {
    
        Pass
        {
            Cull back
			Name "BASE"
			BindChannels 
			{
				Bind "Vertex", vertex
				Bind "texcoord", texcoord0
				Bind "normal", normal
			}
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
               
                #pragma fragmentoption ARB_fog_exp2
                #pragma fragmentoption ARB_precision_hint_fastest
               
                #include "UnityCG.cginc"
               
                uniform sampler2D _MainTex;
                uniform float4 _MainTex_ST;
                uniform float4 _Color;
                uniform float4 _AtmoColor;
				uniform half _TH;
				uniform half _ZP;
               
                struct v2f
                {
                    float4 pos : SV_POSITION;
                    float3 normal : TEXCOORD0;
                    float3 worldvertpos : TEXCOORD1;
                    float2 texcoord : TEXCOORD2;
					float amount : TEXCOORD3;
                };

                v2f vert(appdata_base v)
                {
                    v2f o;
                   
                    o.pos = UnityObjectToClipPos (v.vertex);
                    o.normal = mul (UNITY_MATRIX_MV, float4(v.normal,0));//mul (UNITY_MATRIX_MVP,v.normal.xyzz).xyz;
					o.normal = normalize(o.normal);
                    o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                    o.amount =  saturate((o.normal.x*o.normal.x + o.normal.y*o.normal.y - o.normal.z*o.normal.z*_ZP));
					o.amount *= o.amount;
                    return o;
                }
              
                fixed4 frag(v2f i) : COLOR
                {

					half amount = i.amount;
					if(amount < _TH)
					{
						amount = 0;
					}
                    fixed4 color = tex2D(_MainTex, i.texcoord) * _Color;
                    color.rgba = color.rgba * lerp(1,(_AtmoColor + 1), amount);
               
                    return color;
                }
            ENDCG
        }
   
    }
}

