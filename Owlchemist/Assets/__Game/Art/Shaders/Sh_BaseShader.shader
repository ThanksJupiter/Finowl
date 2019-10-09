Shader "OWL/BaseRampShader"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Texture", 2D) = "white" {}
		[Normal] _BumpMap("Normal", 2D) = "bump" {}
		_Ramp("Ramp Texture", 2D) = "white" {}
		_Cutoff("Alpha Cutoff", Range(0, 1)) = 0.5
	}
		SubShader
	{
		Tags { "RenderType" = "Transparent" }
		LOD 100
		Cull Off

		CGPROGRAM
		#pragma surface surf Ramp

		#pragma target 3.0

		sampler2D _Ramp;
		float _Cutoff;

		half4 LightingRamp(SurfaceOutput s, half3 lightDir, half atten)
		{
			half NdotL = dot(s.Normal, lightDir);
			half diff = NdotL * 0.5 + 0.5;
			half3 ramp = tex2D(_Ramp, float2(diff, 0)), rgb;
			half4 c;
			c.rgb = s.Albedo * _LightColor0.rgb * ramp * atten;
			c.a = s.Alpha;
			return c;
		}

		struct Input
		{
			float2 uv_MainTex;
			float2 uv_BumpMap;
		};

		float4 _Color;
		sampler2D _MainTex;
		sampler2D _BumpMap;

		void surf(Input IN, inout SurfaceOutput o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			clip(c.a - _Cutoff);
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
		}
		ENDCG
	}
		Fallback "Diffuse"
}