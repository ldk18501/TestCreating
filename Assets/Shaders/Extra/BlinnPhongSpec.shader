Shader "Triniti/Extra/BlinnPhongSpec " {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 0)
	_Shininess ("Shininess", Range (0.01, 10)) = 0.078125
	_SpecPower ("SpecPower", Range (0.01, 5)) = 0.078125
	_MainTex ("Base (RGB))", 2D) = "white" {}
	_EffectTex ("Effect Texture (RGB)", 2D) = "white" {}
}
// Blinn
SubShader {
//	Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
	//AlphaTest Greater 0.1
	Blend SrcAlpha One
	cull off
	LOD 300

CGPROGRAM
#pragma surface surf BlinnPhongSpec

//特殊的 BlinnPhone
inline fixed4 LightingBlinnPhongSpec (SurfaceOutput s, fixed3 lightDir, half3 viewDir, fixed atten)
{
	half3 h = normalize (lightDir + viewDir);
	
	fixed diff = max (0, dot (s.Normal, lightDir));
	
	float nh = max (0, dot (s.Normal, h));
	float spec = pow (nh, s.Specular*128.0) * s.Gloss;
	
	fixed4 c;
	c.rgb = (s.Albedo * _LightColor0.rgb * diff + _LightColor0.rgb * _SpecColor.rgb * spec) * (atten * 2);
	c.a = _LightColor0.a * _SpecColor.a * spec * atten;
	return c;
}

sampler2D _MainTex, _EffectTex;
float4 _Color;
float _Shininess, _SpecPower;

struct Input {
	float2 uv_MainTex;	
};

void surf (Input IN, inout SurfaceOutput o) {
	half4 color = tex2D(_MainTex, IN.uv_MainTex);
	half4 effectColor = tex2D(_EffectTex, IN.uv_MainTex);
	float spec = effectColor.r;
	spec=spec*spec;
	o.Albedo = 0;//color.rgb *  (half3(1,1,1)*(1-effectColor.g) + _Color.rgb*effectColor.g);
	o.Gloss = _SpecPower * spec;
	o.Specular = _Shininess * spec;
	o.Alpha = color.a * effectColor.b * length(o.Specular);
	
}
ENDCG
}
// Lambert
SubShader {
	Tags {"Queue"="Opaque"}
	AlphaTest Greater 0.1
	Blend SrcAlpha OneMinusSrcAlpha
	cull off
	LOD 300

CGPROGRAM
#pragma surface surf Lambert alphatest:_Cutoff

sampler2D _MainTex, _EffectTex;
float4 _Color;
float _Shininess, _SpecPower;

struct Input {
	float2 uv_MainTex;
};

void surf (Input IN, inout SurfaceOutput o) {
	half4 color = tex2D(_MainTex, IN.uv_MainTex);
	half4 effectColor = tex2D(_EffectTex, IN.uv_MainTex);
	o.Albedo = color.rgb *  (half3(1,1,1)*(1-effectColor.g) + _Color.rgb*effectColor.g);
	o.Gloss = _SpecPower * effectColor.r;
	o.Alpha = color.a * _Color.a * effectColor.b;
	o.Specular = _Shininess * effectColor.r;
}
ENDCG
}
}
