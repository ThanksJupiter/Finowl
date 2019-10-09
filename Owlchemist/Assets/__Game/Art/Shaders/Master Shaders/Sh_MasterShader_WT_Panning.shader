// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "OWL/WorldTilingPanning"
{
	Properties
	{
		_Falloff("Falloff", Range( 0 , 10)) = 1
		_Tiling("Tiling", Range( 0 , 2)) = 0.2
		_NormalScale("Normal Scale", Float) = 0
		_RoughnessTex("Roughness Tex", 2D) = "white" {}
		_MetallicTex("Metallic Tex", 2D) = "white" {}
		_EmissiveTex("Emissive Tex", 2D) = "white" {}
		_NormalTex("Normal Tex", 2D) = "white" {}
		_AlbedoTex("Albedo Tex", 2D) = "white" {}
		_WaveScale("Wave Scale", Range( 0 , 1)) = 0.15
		_Cutoff( "Mask Clip Value", Float ) = 0.03
		_WaveSpeed("Wave Speed", Range( 0 , 5)) = 3
		[HDR]_AlbedoColor("Albedo Color", Color) = (1,1,1,0)
		[Toggle(_USECUSTOMAMBIENTCOLOR_ON)] _UseCustomAmbientColor("Use Custom Ambient Color", Float) = 0
		[HDR]_AmbientColor("Ambient Color", Color) = (0,0,0,0)
		_AmbientIntensity("Ambient Intensity", Range( 0 , 1)) = 1
		[Toggle(_USEALBEDOMAP_ON)] _UseAlbedoMap("Use Albedo Map", Float) = 0
		[HDR]_AlbedoColorOverlay("Albedo Color Overlay", Color) = (1,1,1,0)
		_OverlayStrength("Overlay Strength", Range( 0 , 1)) = 0
		_Roughness("Roughness", Range( 0 , 1)) = 1
		_RoughnessTexIntensity("Roughness Tex Intensity", Range( 0 , 1)) = 1
		[Toggle(_USEROUGHNESSTEX_ON)] _UseRoughnessTex("Use Roughness Tex", Float) = 0
		_Metallic("Metallic", Range( 0 , 1)) = 0
		[Toggle(_USEMETALLICTEX_ON)] _UseMetallicTex("Use Metallic Tex", Float) = 0
		[Toggle]_InvertNormal("Invert Normal", Float) = 0
		_EmissiveTexStrength("Emissive Tex Strength", Range( 0 , 5)) = 1
		[HDR]_EmissiveColor("Emissive Color", Color) = (0,0,0,0)
		_T_GrassEdgeMask_01("T_GrassEdgeMask_01", 2D) = "white" {}
		[HDR]_EdgeHighlight("EdgeHighlight", Color) = (0.6111389,0.7735849,0.1569064,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma shader_feature _USEALBEDOMAP_ON
		#pragma shader_feature _USECUSTOMAMBIENTCOLOR_ON
		#pragma shader_feature _USEMETALLICTEX_ON
		#pragma shader_feature _USEROUGHNESSTEX_ON
		#define ASE_TEXTURE_PARAMS(textureName) textureName

		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
			float2 uv_texcoord;
		};

		uniform float _WaveSpeed;
		uniform float _WaveScale;
		uniform sampler2D _NormalTex;
		uniform float _Tiling;
		uniform float _Falloff;
		uniform float _NormalScale;
		uniform float _InvertNormal;
		uniform float4 _AlbedoColor;
		uniform float4 _AlbedoColorOverlay;
		uniform sampler2D _AlbedoTex;
		uniform float _OverlayStrength;
		uniform float4 _EdgeHighlight;
		uniform sampler2D _T_GrassEdgeMask_01;
		uniform float4 _T_GrassEdgeMask_01_ST;
		uniform float4 AmbientColorGlobal;
		uniform float4 _AmbientColor;
		uniform float _AmbientIntensity;
		uniform sampler2D _EmissiveTex;
		uniform float _EmissiveTexStrength;
		uniform float4 _EmissiveColor;
		uniform float _Metallic;
		uniform sampler2D _MetallicTex;
		uniform float _Roughness;
		uniform sampler2D _RoughnessTex;
		uniform float _RoughnessTexIntensity;
		uniform float _Cutoff = 0.03;


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


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float simplePerlin2D201 = snoise( ( ase_worldPos + ( _WaveSpeed * _Time.x ) ).xy );
			float3 temp_cast_1 = ((( _WaveScale * 0.0 ) + (simplePerlin2D201 - 0.0) * (_WaveScale - ( _WaveScale * 0.0 )) / (1.0 - 0.0))).xxx;
			v.vertex.xyz += temp_cast_1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			float3 triplanar177 = TriplanarSamplingSNF( _NormalTex, ase_worldPos, ase_worldNormal, _Falloff, _Tiling, _NormalScale, 0 );
			float3 tanTriplanarNormal177 = mul( ase_worldToTangent, triplanar177 );
			float4 appendResult115 = (float4(tanTriplanarNormal177.x , lerp(tanTriplanarNormal177.y,( 1.0 - tanTriplanarNormal177.y ),_InvertNormal) , tanTriplanarNormal177.z , 0.0));
			o.Normal = appendResult115.xyz;
			float4 triplanar171 = TriplanarSamplingSF( _AlbedoTex, ase_worldPos, ase_worldNormal, _Falloff, _Tiling, 1.0, 0 );
			float4 blendOpSrc184 = _AlbedoColorOverlay;
			float4 blendOpDest184 = triplanar171;
			float4 lerpResult185 = lerp( ( saturate( (( blendOpDest184 > 0.5 ) ? ( 1.0 - ( 1.0 - 2.0 * ( blendOpDest184 - 0.5 ) ) * ( 1.0 - blendOpSrc184 ) ) : ( 2.0 * blendOpDest184 * blendOpSrc184 ) ) )) , triplanar171 , ( 1.0 - _OverlayStrength ));
			#ifdef _USEALBEDOMAP_ON
				float4 staticSwitch119 = lerpResult185;
			#else
				float4 staticSwitch119 = _AlbedoColor;
			#endif
			float2 uv_T_GrassEdgeMask_01 = i.uv_texcoord * _T_GrassEdgeMask_01_ST.xy + _T_GrassEdgeMask_01_ST.zw;
			float4 tex2DNode191 = tex2D( _T_GrassEdgeMask_01, uv_T_GrassEdgeMask_01 );
			float4 lerpResult190 = lerp( staticSwitch119 , _EdgeHighlight , tex2DNode191.r);
			o.Albedo = lerpResult190.rgb;
			#ifdef _USECUSTOMAMBIENTCOLOR_ON
				float4 staticSwitch162 = _AmbientColor;
			#else
				float4 staticSwitch162 = ( unity_AmbientSky * AmbientColorGlobal );
			#endif
			float4 triplanar179 = TriplanarSamplingSF( _EmissiveTex, ase_worldPos, ase_worldNormal, _Falloff, _Tiling, 1.0, 0 );
			float4 temp_cast_4 = (_EmissiveTexStrength).xxxx;
			float4 lerpResult135 = lerp( triplanar179 , temp_cast_4 , triplanar179.z);
			o.Emission = ( ( staticSwitch162 * _AmbientIntensity ) + ( lerpResult135 * _EmissiveColor ) ).rgb;
			float4 temp_cast_8 = (_Metallic).xxxx;
			float4 triplanar180 = TriplanarSamplingSF( _MetallicTex, ase_worldPos, ase_worldNormal, _Falloff, _Tiling, 1.0, 0 );
			#ifdef _USEMETALLICTEX_ON
				float4 staticSwitch58 = triplanar180;
			#else
				float4 staticSwitch58 = temp_cast_8;
			#endif
			o.Metallic = staticSwitch58.x;
			float4 temp_cast_10 = (( 1.0 - _Roughness )).xxxx;
			float4 triplanar183 = TriplanarSamplingSF( _RoughnessTex, ase_worldPos, ase_worldNormal, _Falloff, _Tiling, 1.0, 0 );
			#ifdef _USEROUGHNESSTEX_ON
				float4 staticSwitch60 = ( ( 1.0 - triplanar183 ) * _RoughnessTexIntensity );
			#else
				float4 staticSwitch60 = temp_cast_10;
			#endif
			o.Smoothness = staticSwitch60.x;
			o.Alpha = 1;
			clip( tex2DNode191.a - _Cutoff );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows exclude_path:deferred vertex:vertexDataFunc 

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
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
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
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
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
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
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
1920;1;1906;1011;3525.207;1152.162;2.772014;True;False
Node;AmplifyShaderEditor.CommentaryNode;181;-2157.128,-190.1441;Float;False;308.0509;290.2449;TriPlanar Properties;3;176;175;178;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;166;-1650.636,-271.83;Float;False;1202.036;656.7611;Emissive;13;179;159;52;137;136;53;135;162;122;131;161;121;25;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;176;-2118.639,17.10721;Float;False;Property;_Falloff;Falloff;0;0;Create;True;0;0;False;0;1;2.8;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;164;-1687.918,-1121.384;Float;False;1646.43;430.4599;Albedo;8;186;187;184;120;185;119;171;1;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;175;-2117.188,-66.77731;Float;False;Property;_Tiling;Tiling;1;0;Create;True;0;0;False;0;0.2;0.2;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;194;-1691.172,1238.866;Float;False;1136.111;349.8941;Vertex Displacement;9;203;202;201;200;199;198;197;196;195;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TimeNode;195;-1651.854,1291.258;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;196;-1660.29,1440.288;Float;False;Property;_WaveSpeed;Wave Speed;12;0;Create;True;0;0;False;0;3;5;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;168;-1694.257,783.4546;Float;False;986.1802;421.9637;Roughness;7;183;60;123;142;124;143;61;;1,1,1,1;0;0
Node;AmplifyShaderEditor.FogAndAmbientColorsNode;25;-1605.202,-219.3799;Float;False;unity_AmbientSky;0;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;1;-1231.773,-1066.981;Float;False;Property;_AlbedoColorOverlay;Albedo Color Overlay;19;1;[HDR];Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;121;-1607.455,-142.7744;Float;False;Global;AmbientColorGlobal;Ambient Color Global;1;1;[HDR];Create;True;0;0;False;0;0,0.1188681,0.509434,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;178;-2026.254,-146.4892;Float;False;Property;_NormalScale;Normal Scale;2;0;Create;True;0;0;False;0;0;-0.01;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;165;-1621.091,-606.8537;Float;False;1154.299;268.9151;Normals;4;177;115;114;113;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TriplanarNode;171;-1346.081,-890.6331;Float;True;Spherical;World;False;Albedo Tex;_AlbedoTex;white;8;None;Mid Texture 0;_MidTexture0;white;-1;None;Bot Texture 0;_BotTexture0;white;-1;None;Albedo Tex;False;9;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;8;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;186;-1661.567,-1070.986;Float;False;Property;_OverlayStrength;Overlay Strength;20;0;Create;True;0;0;False;0;0;0.13;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;198;-1429.701,1298.86;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;197;-1379.052,1442.993;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;187;-1396.087,-1066.386;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TriplanarNode;177;-1596.793,-551.3152;Float;True;Spherical;World;True;Normal Tex;_NormalTex;white;6;None;Mid Texture 1;_MidTexture1;white;-1;None;Bot Texture 1;_BotTexture1;white;-1;None;Normal Tex;False;9;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;8;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendOpsNode;184;-942.2861,-1066.625;Float;True;Overlay;True;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;122;-1311.55,-202.2362;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;161;-1609.199,27.94277;Float;False;Property;_AmbientColor;Ambient Color;15;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TriplanarNode;183;-1666.898,922.7202;Float;True;Spherical;World;False;Roughness Tex;_RoughnessTex;white;3;None;Mid Texture 5;_MidTexture5;white;-1;None;Bot Texture 5;_BotTexture5;white;-1;None;Roughness Tex;False;9;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;8;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;131;-1276.093,188.4036;Float;False;Property;_EmissiveTexStrength;Emissive Tex Strength;27;0;Create;True;0;0;False;0;1;0;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.TriplanarNode;179;-1351.234,-2.206539;Float;True;Spherical;World;False;Emissive Tex;_EmissiveTex;white;5;None;Mid Texture 2;_MidTexture2;white;-1;None;Bot Texture 2;_BotTexture2;white;-1;None;Emissive Tex;False;9;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;8;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;199;-1131.47,1359.217;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;200;-1213.54,1459.967;Float;False;Property;_WaveScale;Wave Scale;10;0;Create;True;0;0;False;0;0.15;0.026;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;113;-1231.12,-459.857;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;120;-591.2114,-1055.266;Float;False;Property;_AlbedoColor;Albedo Color;13;1;[HDR];Create;True;0;0;False;0;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;135;-959.9653,-5.109568;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ColorNode;136;-1607.225,199.2879;Float;False;Property;_EmissiveColor;Emissive Color;28;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;143;-1600.665,1115.908;Float;False;Property;_RoughnessTexIntensity;Roughness Tex Intensity;22;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;185;-556.3467,-844.1064;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;61;-1602.246,832.7278;Float;False;Property;_Roughness;Roughness;21;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;53;-1276.821,-87.80283;Float;False;Property;_AmbientIntensity;Ambient Intensity;16;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;124;-1306.685,922.4757;Float;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;167;-1603.989,439.53;Float;False;675.0671;312.6807;Metallic;3;180;58;59;;1,1,1,1;0;0
Node;AmplifyShaderEditor.StaticSwitch;162;-1134.978,-206.8835;Float;False;Property;_UseCustomAmbientColor;Use Custom Ambient Color;14;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;205;-2605.294,-669.2471;Float;False;812.5968;332.8633;Normal Map;3;208;207;206;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;202;-943.3727,1443.585;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;201;-946.9605,1365.567;Float;False;Simplex2D;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;137;-811.7037,-5.179026;Float;True;2;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;191;-451.5356,-1412.992;Float;True;Property;_T_GrassEdgeMask_01;T_GrassEdgeMask_01;30;0;Create;True;0;0;False;0;b58978d8f515a7b4bbad8c4b1ce27511;76993a50e04ca85488c3d40c5adb3b4f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;169;-427.3672,-594.148;Float;False;609.2;251.2341;Opacity Mask;2;140;182;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;-813.4876,-201.2055;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;114;-1054.356,-479.9146;Float;False;Property;_InvertNormal;Invert Normal;26;0;Create;True;0;0;False;0;0;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;142;-1153.357,922.5938;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.StaticSwitch;119;-350.1434,-802.778;Float;False;Property;_UseAlbedoMap;Use Albedo Map;18;0;Create;True;0;0;False;0;0;0;1;True;;Toggle;2;Key0;Key1;Create;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;193;-131.9021,-1402.702;Float;False;Property;_EdgeHighlight;EdgeHighlight;31;1;[HDR];Create;True;0;0;False;0;0.6111389,0.7735849,0.1569064,0;0.5660378,0.1949092,0.1949092,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;123;-1308.726,838.0841;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TriplanarNode;180;-1580.556,560.4805;Float;True;Spherical;World;False;Metallic Tex;_MetallicTex;white;4;None;Mid Texture 3;_MidTexture3;white;-1;None;Bot Texture 3;_BotTexture3;white;-1;None;Metallic Tex;False;9;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;8;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;59;-1502.72,488.2339;Float;False;Property;_Metallic;Metallic;24;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;207;-2589.239,-492.6831;Float;False;Property;_TexPanningSpeed;Tex Panning Speed;7;0;Create;True;0;0;False;0;1;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;206;-2580.721,-620.1578;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;204;-2929.569,-615.5742;Float;False;Property;_TexTiling;Tex Tiling;17;0;Create;True;0;0;False;0;0;1;1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;203;-744.1682,1364.677;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;208;-2315.352,-529.3405;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TriplanarNode;182;-414.2583,-543.4895;Float;True;Spherical;World;False;Mask Tex;_MaskTex;white;9;None;Mid Texture 4;_MidTexture4;white;-1;None;Bot Texture 4;_BotTexture4;white;-1;None;Mask Tex;False;9;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;8;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;159;-572.9119,-201.3394;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;115;-804.9419,-555.2575;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.StaticSwitch;140;-57.76389,-544.3906;Float;False;Property;_UseMaskTex;Use Mask Tex;29;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;9;1;FLOAT4;0,0,0,0;False;0;FLOAT4;0,0,0,0;False;2;FLOAT4;0,0,0,0;False;3;FLOAT4;0,0,0,0;False;4;FLOAT4;0,0,0,0;False;5;FLOAT4;0,0,0,0;False;6;FLOAT4;0,0,0,0;False;7;FLOAT4;0,0,0,0;False;8;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.LerpOp;190;368.41,-778.2903;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;60;-1010.923,833.7592;Float;False;Property;_UseRoughnessTex;Use Roughness Tex;23;0;Create;True;0;0;False;0;0;0;1;True;;Toggle;2;Key0;Key1;Create;9;1;FLOAT4;0,0,0,0;False;0;FLOAT4;0,0,0,0;False;2;FLOAT4;0,0,0,0;False;3;FLOAT4;0,0,0,0;False;4;FLOAT4;0,0,0,0;False;5;FLOAT4;0,0,0,0;False;6;FLOAT4;0,0,0,0;False;7;FLOAT4;0,0,0,0;False;8;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.StaticSwitch;58;-1208.547,489.5292;Float;False;Property;_UseMetallicTex;Use Metallic Tex;25;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;9;1;FLOAT4;0,0,0,0;False;0;FLOAT4;0,0,0,0;False;2;FLOAT4;0,0,0,0;False;3;FLOAT4;0,0,0,0;False;4;FLOAT4;0,0,0,0;False;5;FLOAT4;0,0,0,0;False;6;FLOAT4;0,0,0,0;False;7;FLOAT4;0,0,0,0;False;8;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;10;519.377,-584.6972;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;OWL/WorldTilingPanning;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.03;True;True;0;True;Transparent;;Geometry;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0.06;0,0,0,0;VertexOffset;False;False;Cylindrical;False;Relative;0;;11;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;28;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;171;3;175;0
WireConnection;171;4;176;0
WireConnection;197;0;196;0
WireConnection;197;1;195;1
WireConnection;187;0;186;0
WireConnection;177;8;178;0
WireConnection;177;3;175;0
WireConnection;177;4;176;0
WireConnection;184;0;1;0
WireConnection;184;1;171;0
WireConnection;122;0;25;0
WireConnection;122;1;121;0
WireConnection;183;3;175;0
WireConnection;183;4;176;0
WireConnection;179;3;175;0
WireConnection;179;4;176;0
WireConnection;199;0;198;0
WireConnection;199;1;197;0
WireConnection;113;0;177;2
WireConnection;135;0;179;0
WireConnection;135;1;131;0
WireConnection;135;2;179;3
WireConnection;185;0;184;0
WireConnection;185;1;171;0
WireConnection;185;2;187;0
WireConnection;124;0;183;0
WireConnection;162;1;122;0
WireConnection;162;0;161;0
WireConnection;202;0;200;0
WireConnection;201;0;199;0
WireConnection;137;0;135;0
WireConnection;137;1;136;0
WireConnection;52;0;162;0
WireConnection;52;1;53;0
WireConnection;114;0;177;2
WireConnection;114;1;113;0
WireConnection;142;0;124;0
WireConnection;142;1;143;0
WireConnection;119;1;120;0
WireConnection;119;0;185;0
WireConnection;123;0;61;0
WireConnection;180;3;175;0
WireConnection;180;4;176;0
WireConnection;206;0;204;0
WireConnection;203;0;201;0
WireConnection;203;3;202;0
WireConnection;203;4;200;0
WireConnection;208;0;206;0
WireConnection;208;2;207;0
WireConnection;182;3;175;0
WireConnection;182;4;176;0
WireConnection;159;0;52;0
WireConnection;159;1;137;0
WireConnection;115;0;177;1
WireConnection;115;1;114;0
WireConnection;115;2;177;3
WireConnection;140;1;171;4
WireConnection;140;0;182;0
WireConnection;190;0;119;0
WireConnection;190;1;193;0
WireConnection;190;2;191;1
WireConnection;60;1;123;0
WireConnection;60;0;142;0
WireConnection;58;1;59;0
WireConnection;58;0;180;0
WireConnection;10;0;190;0
WireConnection;10;1;115;0
WireConnection;10;2;159;0
WireConnection;10;3;58;0
WireConnection;10;4;60;0
WireConnection;10;10;191;4
WireConnection;10;11;203;0
ASEEND*/
//CHKSM=3E4BA43D9D6AA9B498317FA9D3FEE3F81F189CA3