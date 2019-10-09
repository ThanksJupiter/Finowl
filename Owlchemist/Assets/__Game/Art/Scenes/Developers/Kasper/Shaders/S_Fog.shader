Shader "Unlit/S_Fog"
{
	Properties
	{
		_FogTex("Fog Texture", 2D) = "black" {}
		_FogOffset("Fog Offset", Vector) = (0, 0, 0)
		_FogSize("Fog Size", float) = 1
	}
	SubShader
	{
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "IgnoreProjector" = "True" }
		LOD 100
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog

			#include "UnityCG.cginc"


			struct appdata 
			{
				float4 vertex : POSITION;
			};

			struct v2f 
			{
				float4 vertex : SV_POSITION;
				float3 worldPos : TEXCOORD0;
			};

			sampler2D _FogTex;
			float3 _FogOffset;
			float _FogSize;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(UNITY_MATRIX_M, v.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed2 uv = i.worldPos.xz;
				uv -= _FogOffset.xz;
				uv /= _FogSize;

				uv = uv * 0.5 + 0.5;

				fixed fog = 1.0 - tex2D(_FogTex, uv).x;
				//clip(fog - 0.2);

				return fixed4(0, 0.0, 0.0, fog);
			}
			ENDCG
		}
	}
}