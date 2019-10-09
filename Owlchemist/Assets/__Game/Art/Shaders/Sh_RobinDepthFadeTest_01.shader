// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "OWL/RobinDFTest"
{
	Properties
	{
		_TopTexture1("Top Texture 1", 2D) = "white" {}
		_TopTexture0("Top Texture 0", 2D) = "white" {}
		_Metallic("Metallic", Range( 0 , 1)) = 0
		_Roughness("Roughness", Range( 0 , 1)) = 0.2
		_EmissiveColor("EmissiveColor", Color) = (0,0,0,0)
		_Emissive2("Emissive2", Color) = (0,0,0,0)
		_NormalStrenght("Normal Strenght", Range( 0 , 5)) = 1
		[Toggle]_InvertNormal("Invert Normal", Float) = 0
		_DFDistance("DF Distance", Range( 0.1 , 30)) = 5
		_TriScale("TriScale", Range( 0 , 1)) = 0
		_AtriTiling("AtriTiling", Range( 0 , 0.5)) = 0
		_TriTiling("TriTiling", Range( 0 , 0.5)) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Custom"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Blend One One
		
		AlphaToMask On
		CGPROGRAM
		#include "UnityCG.cginc"
		#pragma target 5.0
		#define ASE_TEXTURE_PARAMS(textureName) textureName

		#pragma surface surf Standard keepalpha noshadow 
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
			float4 screenPos;
		};

		uniform float _NormalStrenght;
		uniform sampler2D _TopTexture0;
		uniform float _TriTiling;
		uniform float _TriScale;
		uniform float _InvertNormal;
		uniform sampler2D _TopTexture1;
		uniform float _AtriTiling;
		uniform float4 _EmissiveColor;
		uniform float4 _Emissive2;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _DFDistance;
		uniform float _Metallic;
		uniform float _Roughness;


		inline float3 TriplanarSamplingSNF( sampler2D topTexMap, float3 worldPos, float3 worldNormal, float falloff, float tilling, float3 normalScale, float3 index )
		{
			float3 projNormal = ( pow( abs( worldNormal ), falloff ) );
			projNormal /= ( projNormal.x + projNormal.y + projNormal.z ) + 0.00001;
			float3 nsign = sign( worldNormal );
			half4 xNorm; half4 yNorm; half4 zNorm;
			xNorm = ( tex2D( ASE_TEXTURE_PARAMS( topTexMap ), tilling * worldPos.zy * float2( nsign.x, 1.0 ) ) );
			yNorm = ( tex2D( ASE_TEXTURE_PARAMS( topTexMap ), tilling * worldPos.xz * float2( nsign.y, 1.0 ) ) );
			zNorm = ( tex2D( ASE_TEXTURE_PARAMS( topTexMap ), tilling * worldPos.xy * float2( -nsign.z, 1.0 ) ) );
			xNorm.xyz = half3( UnpackScaleNormal( xNorm, normalScale.y ).xy * float2( nsign.x, 1.0 ) + worldNormal.zy, worldNormal.x ).zyx;
			yNorm.xyz = half3( UnpackScaleNormal( yNorm, normalScale.x ).xy * float2( nsign.y, 1.0 ) + worldNormal.xz, worldNormal.y ).xzy;
			zNorm.xyz = half3( UnpackScaleNormal( zNorm, normalScale.y ).xy * float2( -nsign.z, 1.0 ) + worldNormal.xy, worldNormal.z ).xyz;
			return normalize( xNorm.xyz * projNormal.x + yNorm.xyz * projNormal.y + zNorm.xyz * projNormal.z );
		}


		inline float4 TriplanarSamplingSF( sampler2D topTexMap, float3 worldPos, float3 worldNormal, float falloff, float tilling, float3 normalScale, float3 index )
		{
			float3 projNormal = ( pow( abs( worldNormal ), falloff ) );
			projNormal /= ( projNormal.x + projNormal.y + projNormal.z ) + 0.00001;
			float3 nsign = sign( worldNormal );
			half4 xNorm; half4 yNorm; half4 zNorm;
			xNorm = ( tex2D( ASE_TEXTURE_PARAMS( topTexMap ), tilling * worldPos.zy * float2( nsign.x, 1.0 ) ) );
			yNorm = ( tex2D( ASE_TEXTURE_PARAMS( topTexMap ), tilling * worldPos.xz * float2( nsign.y, 1.0 ) ) );
			zNorm = ( tex2D( ASE_TEXTURE_PARAMS( topTexMap ), tilling * worldPos.xy * float2( -nsign.z, 1.0 ) ) );
			return xNorm * projNormal.x + yNorm * projNormal.y + zNorm * projNormal.z;
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			float3 triplanar37 = TriplanarSamplingSNF( _TopTexture0, ase_worldPos, ase_worldNormal, 1.0, _TriTiling, _TriScale, 0 );
			float3 tanTriplanarNormal37 = mul( ase_worldToTangent, triplanar37 );
			float4 appendResult18 = (float4(tanTriplanarNormal37.x , lerp(tanTriplanarNormal37.y,( 1.0 - tanTriplanarNormal37.y ),_InvertNormal) , 0.0 , 0.0));
			float4 appendResult15 = (float4(( _NormalStrenght * appendResult18 ).xy , tanTriplanarNormal37.z , 0.0));
			o.Normal = appendResult15.xyz;
			float4 triplanar43 = TriplanarSamplingSF( _TopTexture1, ase_worldPos, ase_worldNormal, 1.0, _AtriTiling, 1.0, 0 );
			o.Albedo = triplanar43.xyz;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth21 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD( ase_screenPos ))));
			float distanceDepth21 = ( screenDepth21 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _DFDistance );
			float4 lerpResult31 = lerp( _EmissiveColor , _Emissive2 , distanceDepth21);
			o.Emission = lerpResult31.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Roughness;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16400
1921;1;1918;1017;1817.391;764.0387;1.367296;True;False
Node;AmplifyShaderEditor.RangedFloatNode;39;-2370.464,-406.3264;Float;False;Property;_TriScale;TriScale;16;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;40;-2364.314,-280.2514;Float;False;Property;_TriTiling;TriTiling;19;0;Create;True;0;0;False;0;0;0;0;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.TriplanarNode;37;-2035.288,-318.1766;Float;True;Spherical;World;True;Top Texture 0;_TopTexture0;white;2;None;Mid Texture 0;_MidTexture0;white;-1;None;Bot Texture 0;_BotTexture0;white;-1;None;Triplanar Sampler;False;9;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;8;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;20;-1614.071,-22.07248;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;17;-1414.071,-48.07246;Float;False;Property;_InvertNormal;Invert Normal;11;0;Create;True;0;0;False;0;0;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;18;-1156.071,-64.07246;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-1209.543,-181.5028;Float;False;Property;_NormalStrenght;Normal Strenght;10;0;Create;True;0;0;False;0;1;1;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-1169.856,-300.9731;Float;False;Property;_DFDistance;DF Distance;13;0;Create;True;0;0;False;0;5;0;0.1;30;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;21;-800.3448,-257.7033;Float;False;True;False;False;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;5;-737.0505,-660.4003;Float;False;Property;_EmissiveColor;EmissiveColor;6;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;32;-729.4385,-532.4014;Float;False;Property;_Emissive2;Emissive2;7;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;42;-1903.026,-613.2452;Float;False;Property;_AtriTiling;AtriTiling;18;0;Create;True;0;0;False;0;0;0;0;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-911.9483,-86.71803;Float;False;2;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;10;-1961.327,-1.309646;Float;True;Property;_T_Placeholder_N;T_Placeholder_N;9;0;Create;True;0;0;False;0;62ba14c7513541d4ebe3b7cf9955bd97;62ba14c7513541d4ebe3b7cf9955bd97;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;23;-993.976,-544.152;Float;False;Property;_DepthVertexPos;DepthVertex Pos;14;0;Create;True;0;0;False;0;0,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ColorNode;47;-795.0132,309.4618;Float;False;Property;_Color0;Color 0;8;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;1;-268,-431;Float;False;Property;_BaseColor;BaseColor;3;0;Create;True;0;0;False;0;0.6037736,0.6037736,0.6037736,0;0.4528302,0.08757564,0.1214057,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;15;-719.5432,-34.50281;Float;False;FLOAT4;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-2485.263,-58.8516;Float;False;Property;_PannerSpeed;PannerSpeed;15;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-438,274;Float;False;Property;_Roughness;Roughness;5;0;Create;True;0;0;False;0;0.2;0.2;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;31;-462.2377,-183.4768;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PannerNode;33;-2258.739,-95.75162;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TriplanarNode;43;-1574,-651.1703;Float;True;Spherical;World;False;Top Texture 1;_TopTexture1;white;1;None;Mid Texture 1;_MidTexture1;white;-1;None;Bot Texture 1;_BotTexture1;white;-1;None;Triplanar Sampler;False;9;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;8;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;41;-1909.176,-739.3203;Float;False;Property;_ATriScale;ATriScale;17;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;46;-333.1047,26.36938;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;48;-740.0132,119.4618;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;44;-1035.212,175.1429;Float;False;True;False;False;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-1404.723,131.8731;Float;False;Property;_Float0;Float 0;12;0;Create;True;0;0;False;0;5;0;0.1;30;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-439,188;Float;False;Property;_Metallic;Metallic;4;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;52;-562.2128,-53.04468;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;58.62645,-77.24826;Float;False;True;7;Float;ASEMaterialInspector;0;0;Standard;OWL/RobinDFTest;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;False;Custom;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;4;1;False;-1;1;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;True;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;37;8;39;0
WireConnection;37;3;40;0
WireConnection;20;0;37;2
WireConnection;17;0;37;2
WireConnection;17;1;20;0
WireConnection;18;0;37;1
WireConnection;18;1;17;0
WireConnection;21;0;22;0
WireConnection;11;0;12;0
WireConnection;11;1;18;0
WireConnection;10;1;33;0
WireConnection;15;0;11;0
WireConnection;15;2;37;3
WireConnection;31;0;5;0
WireConnection;31;1;32;0
WireConnection;31;2;21;0
WireConnection;33;2;34;0
WireConnection;43;3;42;0
WireConnection;46;0;31;0
WireConnection;46;2;48;0
WireConnection;48;0;44;0
WireConnection;44;0;45;0
WireConnection;0;0;43;0
WireConnection;0;1;15;0
WireConnection;0;2;31;0
WireConnection;0;3;3;0
WireConnection;0;4;4;0
ASEEND*/
//CHKSM=78C63FABEF1BBCF7004EF0C01A27198A1B1D298C