// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "OWL/ParticlesASE2"
{
	Properties
	{
		_DepthFadeAmount("Depth Fade Amount", Range( 0.5 , 1.5)) = 1
		_Emission("Emission", Range( 1 , 5)) = 1
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit alpha:fade keepalpha noshadow 
		struct Input
		{
			float4 vertexColor : COLOR;
			float4 screenPos;
		};

		uniform float _Emission;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _DepthFadeAmount;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float4 color15 = IsGammaSpace() ? float4(1,0.7092538,0.3443396,0) : float4(1,0.4612797,0.09714398,0);
			float4 color16 = IsGammaSpace() ? float4(0.2028302,1,0.85506,0) : float4(0.03399345,1,0.7013942,0);
			float clampResult21 = clamp( _SinTime.w , 0.0 , 1.0 );
			float4 lerpResult17 = lerp( color15 , color16 , clampResult21);
			o.Emission = ( lerpResult17 * ( i.vertexColor * _Emission ) ).rgb;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth3 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD( ase_screenPos ))));
			float distanceDepth3 = saturate( abs( ( screenDepth3 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _DepthFadeAmount ) ) );
			o.Alpha = ( i.vertexColor.a * distanceDepth3 );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16400
156;78;1635;839;2330.015;1087.32;1.915743;True;True
Node;AmplifyShaderEditor.RangedFloatNode;22;-1149.883,-252.4729;Float;False;Constant;_Min;Min;2;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-1153.093,-127.3031;Float;False;Constant;_Max;Max;2;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinTimeNode;28;-1279.867,-382.4566;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;6;-903.2,191.2;Float;False;Property;_DepthFadeAmount;Depth Fade Amount;0;0;Create;True;0;0;False;0;1;1;0.5;1.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;2;-506.2,0;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;9;-766.7471,8.081916;Float;False;Property;_Emission;Emission;1;0;Create;True;0;0;False;0;1;1;1;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;21;-894.7295,-252.4728;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;16;-1021.503,-509.2312;Float;False;Constant;_EndCol;End Col;4;0;Create;True;0;0;False;0;0.2028302,1,0.85506,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;15;-1019.277,-765.9902;Float;False;Constant;_StartCol;Start Col;4;0;Create;True;0;0;False;0;1,0.7092538,0.3443396,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DepthFade;3;-625.2,188.2;Float;False;True;True;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-307.8468,2.881886;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;17;-636.7892,-254.736;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-283.0423,145.542;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-154.5266,-14.25008;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;OWL/ParticlesASE2;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;6;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;21;0;28;4
WireConnection;21;1;22;0
WireConnection;21;2;23;0
WireConnection;3;0;6;0
WireConnection;8;0;2;0
WireConnection;8;1;9;0
WireConnection;17;0;15;0
WireConnection;17;1;16;0
WireConnection;17;2;21;0
WireConnection;7;0;2;4
WireConnection;7;1;3;0
WireConnection;29;0;17;0
WireConnection;29;1;8;0
WireConnection;0;2;29;0
WireConnection;0;9;7;0
ASEEND*/
//CHKSM=43D25D08114647C592A3A506B1249059AC5529D0