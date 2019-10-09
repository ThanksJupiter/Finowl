// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "OWL/Water Simple"
{
	Properties
	{
		[HDR]_WaterColor("Water Color", Color) = (0.2783019,0.6363112,1,0)
		_DepthFadeAmount("Depth Fade Amount", Range( 0 , 1)) = 0
		[HDR]_FoamColor("Foam Color", Color) = (1,1,1,0)
		[Toggle(_USECUSTOMAMBIENTCOLOR_ON)] _UseCustomAmbientColor("Use Custom Ambient Color", Float) = 0
		_EdgeFoamSize("Edge Foam Size", Range( 0 , 2)) = 0.1
		[HDR]_AmbientColor("Ambient Color", Color) = (0,0,0,0)
		_TexPanningSpeed("Tex Panning Speed", Range( 0 , 1)) = 1
		_AmbientIntensity("Ambient Intensity", Range( 0 , 1)) = 1
		_Roughness("Roughness", Range( 0 , 1)) = 0.2
		_WaveScale("Wave Scale", Range( 0 , 1)) = 0.15
		_WaveSpeed("Wave Speed", Range( 0 , 5)) = 3
		[Toggle]_InvertNormal("Invert Normal", Float) = 1
		_NormalTex("Normal Tex", 2D) = "bump" {}
		_TexTiling("Tex Tiling", Range( 1 , 10)) = 0
		_NormalStrength("Normal Strength", Range( 0 , 5)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma shader_feature _USECUSTOMAMBIENTCOLOR_ON
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
			float4 screenPos;
		};

		uniform float _WaveSpeed;
		uniform float _WaveScale;
		uniform float _NormalStrength;
		uniform sampler2D _NormalTex;
		uniform float _TexPanningSpeed;
		uniform float _TexTiling;
		uniform float _InvertNormal;
		uniform float4 _WaterColor;
		uniform float4 AmbientColorGlobal;
		uniform float4 _AmbientColor;
		uniform float _AmbientIntensity;
		uniform float4 _FoamColor;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _EdgeFoamSize;
		uniform float _Roughness;
		uniform float _DepthFadeAmount;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float simplePerlin2D14 = snoise( ( ase_worldPos + ( _WaveSpeed * _Time.x ) ).xy );
			float3 temp_cast_1 = ((( _WaveScale * 0.0 ) + (simplePerlin2D14 - 0.0) * (_WaveScale - ( _WaveScale * 0.0 )) / (1.0 - 0.0))).xxx;
			v.vertex.xyz += temp_cast_1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 temp_cast_0 = (_TexPanningSpeed).xx;
			float2 temp_cast_1 = (_TexTiling).xx;
			float2 uv_TexCoord58 = i.uv_texcoord * temp_cast_1;
			float2 panner37 = ( _Time.x * temp_cast_0 + uv_TexCoord58);
			float3 tex2DNode70 = UnpackNormal( tex2D( _NormalTex, panner37 ) );
			float4 appendResult73 = (float4(tex2DNode70.r , lerp(tex2DNode70.g,( 1.0 - tex2DNode70.g ),_InvertNormal) , tex2DNode70.b , 0.0));
			float4 appendResult76 = (float4(( _NormalStrength * appendResult73 ).xy , tex2DNode70.b , 0.0));
			o.Normal = appendResult76.xyz;
			float4 temp_output_1_0 = _WaterColor;
			o.Albedo = temp_output_1_0.rgb;
			#ifdef _USECUSTOMAMBIENTCOLOR_ON
				float4 staticSwitch81 = _AmbientColor;
			#else
				float4 staticSwitch81 = ( unity_AmbientSky * AmbientColorGlobal );
			#endif
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth34 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD( ase_screenPos ))));
			float distanceDepth34 = saturate( abs( ( screenDepth34 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _EdgeFoamSize ) ) );
			o.Emission = ( ( ( staticSwitch81 * _AmbientIntensity ) + float4( 0,0,0,0 ) ) + ( _FoamColor * ( 1.0 - distanceDepth34 ) ) ).rgb;
			o.Smoothness = ( 1.0 - _Roughness );
			float screenDepth2 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD( ase_screenPos ))));
			float distanceDepth2 = saturate( abs( ( screenDepth2 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _DepthFadeAmount ) ) );
			o.Alpha = distanceDepth2;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows exclude_path:deferred vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float4 screenPos : TEXCOORD3;
				float4 tSpace0 : TEXCOORD4;
				float4 tSpace1 : TEXCOORD5;
				float4 tSpace2 : TEXCOORD6;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.screenPos = ComputeScreenPos( o.pos );
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.screenPos = IN.screenPos;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16400
1920;51;1635;839;2510.196;600.9036;1.58068;True;True
Node;AmplifyShaderEditor.CommentaryNode;66;-1453.954,772.7739;Float;False;1136.111;349.8941;Vertex Displacement;9;60;9;16;64;12;10;14;13;15;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;77;-1787.54,424.7344;Float;False;Property;_TexTiling;Tex Tiling;13;0;Create;True;0;0;False;0;0;1;1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;67;-1463.265,371.0614;Float;False;812.5968;332.8633;Normal Map;5;48;37;58;70;71;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TimeNode;60;-1414.636,825.1665;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;58;-1438.692,420.1507;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;48;-1447.21,547.6255;Float;False;Property;_TexPanningSpeed;Tex Panning Speed;6;0;Create;True;0;0;False;0;1;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;37;-1173.323,510.968;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FogAndAmbientColorsNode;79;-1584.924,-513.1307;Float;False;unity_AmbientSky;0;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;78;-1587.177,-436.5251;Float;False;Global;AmbientColorGlobal;Ambient Color Global;1;1;[HDR];Create;True;0;0;False;0;0,0.1188681,0.509434,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;70;-983.3084,488.4836;Float;True;Property;_NormalTex;Normal Tex;12;0;Create;True;0;0;False;0;None;816e2efa06af43c459cac182094725ea;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;68;-1456.549,-123.0431;Float;False;853.8621;456.0956;Color and Foam;9;55;56;1;31;24;5;2;33;34;;1,1,1,1;0;0
Node;AmplifyShaderEditor.ColorNode;85;-1588.921,-265.808;Float;False;Property;_AmbientColor;Ambient Color;5;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;80;-1320.545,-507.1393;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-1420.549,110.0067;Float;False;Property;_EdgeFoamSize;Edge Foam Size;4;0;Create;True;0;0;False;0;0.1;0.6;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;71;-683.8576,606.9567;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-1423.072,974.1963;Float;False;Property;_WaveSpeed;Wave Speed;10;0;Create;True;0;0;False;0;3;5;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;72;-534.0933,577.899;Float;False;Property;_InvertNormal;Invert Normal;11;0;Create;True;0;0;False;0;1;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;64;-1141.834,976.9015;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;81;-1179.825,-509.0287;Float;False;Property;_UseCustomAmbientColor;Use Custom Ambient Color;3;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DepthFade;34;-1151.484,102.1632;Float;False;True;True;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;16;-1192.483,832.7682;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;82;-1360.277,-395.4637;Float;False;Property;_AmbientIntensity;Ambient Intensity;7;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;83;-847.1761,-504.9397;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;12;-894.2525,893.1249;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;73;-319.6793,535.5562;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;74;-683.4536,491.8141;Float;False;Property;_NormalStrength;Normal Strength;14;0;Create;True;0;0;False;0;1;1.3;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-976.3224,993.8754;Float;False;Property;_WaveScale;Wave Scale;9;0;Create;True;0;0;False;0;0.15;0.026;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;31;-907.0435,107.1095;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;56;-1173.582,-69.47177;Float;False;Property;_FoamColor;Foam Color;2;1;[HDR];Create;True;0;0;False;0;1,1,1,0;0.1811143,0.2075472,0.1981032,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NoiseGeneratorNode;14;-709.7425,899.4755;Float;False;Simplex2D;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-1420.161,218.8967;Float;False;Property;_DepthFadeAmount;Depth Fade Amount;1;0;Create;True;0;0;False;0;0;0.3;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;57;-569.0797,128.797;Float;False;Property;_Roughness;Roughness;8;0;Create;True;0;0;False;0;0.2;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;75;-191.082,536.5942;Float;False;2;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;84;-673.3347,-503.4847;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-706.1547,977.4936;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;55;-749.4991,-58.23494;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;69;-210.4652,131.7409;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;2;-1149.16,209.1169;Float;False;True;True;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;1;-1417.574,-66.19757;Float;False;Property;_WaterColor;Water Color;0;1;[HDR];Create;True;0;0;False;0;0.2783019,0.6363112,1,0;0.1934407,0.4097999,0.5943396,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;24;-896.5375,207.1453;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;76;-59.26376,540.439;Float;False;FLOAT4;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;86;-395.6714,-258.9668;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;15;-506.9502,898.5856;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;OWL/Water Simple;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;5;True;True;0;False;Transparent;;Transparent;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;5;True;2;5;False;-1;10;False;-1;1;5;False;-1;5;False;-1;0;False;-1;0;False;-1;1;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.3;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;58;0;77;0
WireConnection;37;0;58;0
WireConnection;37;2;48;0
WireConnection;37;1;60;1
WireConnection;70;1;37;0
WireConnection;80;0;79;0
WireConnection;80;1;78;0
WireConnection;71;0;70;2
WireConnection;72;0;70;2
WireConnection;72;1;71;0
WireConnection;64;0;9;0
WireConnection;64;1;60;1
WireConnection;81;1;80;0
WireConnection;81;0;85;0
WireConnection;34;0;33;0
WireConnection;83;0;81;0
WireConnection;83;1;82;0
WireConnection;12;0;16;0
WireConnection;12;1;64;0
WireConnection;73;0;70;1
WireConnection;73;1;72;0
WireConnection;73;2;70;3
WireConnection;31;0;34;0
WireConnection;14;0;12;0
WireConnection;75;0;74;0
WireConnection;75;1;73;0
WireConnection;84;0;83;0
WireConnection;13;0;10;0
WireConnection;55;0;56;0
WireConnection;55;1;31;0
WireConnection;69;0;57;0
WireConnection;2;0;5;0
WireConnection;24;0;1;0
WireConnection;24;1;2;0
WireConnection;76;0;75;0
WireConnection;76;2;70;3
WireConnection;86;0;84;0
WireConnection;86;1;55;0
WireConnection;15;0;14;0
WireConnection;15;3;13;0
WireConnection;15;4;10;0
WireConnection;0;0;1;0
WireConnection;0;1;76;0
WireConnection;0;2;86;0
WireConnection;0;4;69;0
WireConnection;0;9;2;0
WireConnection;0;11;15;0
ASEEND*/
//CHKSM=5D3C91C659E1594AE882FC4BB2A2D3A5EDAE12F2