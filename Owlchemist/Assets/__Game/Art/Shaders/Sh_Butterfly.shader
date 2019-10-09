Shader  "OWL/Sh_Butterfly"
{
	Properties
	{
		_Color("Color", Color) = (1 , 1 , 1 , 1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Speed("Speed", Range(0 , 20)) = 1.0
		_XAmplitude("AmplitudeX", Range(0 , 5)) = 1
		_YAmplitude("AmplitudeY", Range(0 , 5)) = 1
		_YFrequency("YFrequency", Range(0 , 10)) = 1
		_Cutoff("Alpha Cutoff", Range(0, 1)) = 0.5

	}
		SubShader
		{
			Tags { "RenderType" = "Transparent" "PreviewType" = "Plane" }
			LOD 100
			Cull Off

			CGPROGRAM
			#pragma surface surf Lambert addshadow vertex:winganim
			#pragma target 3.0

			sampler2D  _MainTex;
			float  _Speed;
			half  _XAmplitude;
			half  _YAmplitude;
			half  _YFrequency;
			float _Cutoff;

			struct  Input
			{
			float2 uv_MainTex;
			};

			fixed4 _Color;

			struct appdata
			{
				float4 vertex:POSITION;
				float3 normal:NORMAL;
				float4 color:COLOR;
				float4 texcoord:TEXCOORD0;
				float4 texcoord1:TEXCOORD1;
				float4 texcoord2:TEXCOORD2;
			};

			void winganim(inout appdata v)
			{
				float2 value = v.texcoord.xy;
				float dist = distance(value.x, 0.5); // What is the distance between the body of the butterfly and the current texture coordinates?

				v.vertex.xyz +=
					v.normal *
					sin(dist + _Time.y * _Speed)
					* dist * _XAmplitude;
				// Slide each vertex in the normal direction. y = sin (abs (x-0.5) * time) * abs (x-0.5);

				v.vertex.xyz +=
					v.normal *
					sin(value.y * _YFrequency + _Time.y * _Speed)
					* dist * _YAmplitude;
				// To make the wings of the butterfly wing more smooth, let's do the operations on the y coordinate of the texture coordinates. 
			}

			void surf(Input IN, inout SurfaceOutput o)
			{
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				o.Albedo = c.rgb;
				o.Alpha = c.a;
				clip(c.a - _Cutoff);
			}
			ENDCG
		}
			FallBack "Diffuse"
}