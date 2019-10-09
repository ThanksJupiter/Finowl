// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "OWL/MasterShader_OL"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		[HDR]_AlbedoColor("Albedo Color", Color) = (1,1,1,0)
		[Toggle(_USECUSTOMAMBIENTCOLOR_ON)] _UseCustomAmbientColor("Use Custom Ambient Color", Float) = 0
		[HDR]_AmbientColor("Ambient Color", Color) = (0,0,0,0)
		_AmbientIntensity("Ambient Intensity", Range( 0 , 1)) = 1
		[Toggle(_USEALBEDOMAP_ON)] _UseAlbedoMap("Use Albedo Map", Float) = 0
		_AlbedoTex("Albedo Tex", 2D) = "white" {}
		[HDR]_AlbedoColorOverlay("Albedo Color Overlay", Color) = (1,1,1,0)
		_OverlayStrength("Overlay Strength", Range( 0.5 , 5)) = 1
		_Roughness("Roughness", Range( 0 , 1)) = 1
		_RoughnessTexIntensity("Roughness Tex Intensity", Range( 0 , 1)) = 1
		[Toggle(_USEROUGHNESSTEX_ON)] _UseRoughnessTex("Use Roughness Tex", Float) = 0
		_RoughnessTex("Roughness Tex", 2D) = "white" {}
		_Metallic("Metallic", Range( 0 , 1)) = 0
		[Toggle(_USEMETALLICTEX_ON)] _UseMetallicTex("Use Metallic Tex", Float) = 0
		_MetallicTex("MetallicTex", 2D) = "white" {}
		[Toggle]_InvertNormal("Invert Normal", Float) = 0
		_NormalTex("Normal Tex", 2D) = "bump" {}
		_NormalStrength("Normal Strength", Range( 0 , 5)) = 1
		_EmissiveTex("Emissive Tex", 2D) = "white" {}
		_EmissiveTexStrength("Emissive Tex Strength", Range( 0 , 5)) = 1
		[HDR]_EmissiveColor("Emissive Color", Color) = (0,0,0,0)
		[Toggle(_USEMASKTEX_ON)] _UseMaskTex("Use Mask Tex", Float) = 0
		_MaskTex("Mask Tex", 2D) = "white" {}
		_OutlineColor("Outline Color", Color) = (0,0,0,0)
		_OutlineSize("Outline Size", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ }
		ZWrite Off
		ZTest LEqual
		Cull Front
		CGPROGRAM
		#pragma target 3.0
		#pragma surface outlineSurf Outline nofog  keepalpha noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nometa noforwardadd vertex:outlineVertexDataFunc 
		
		
		
		struct Input
		{
			half filler;
		};
		uniform float _OutlineSize;
		uniform float4 _OutlineColor;
		
		void outlineVertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float outlineVar = _OutlineSize;
			v.vertex.xyz *= ( 1 + outlineVar);
		}
		inline half4 LightingOutline( SurfaceOutput s, half3 lightDir, half atten ) { return half4 ( 0,0,0, s.Alpha); }
		void outlineSurf( Input i, inout SurfaceOutput o )
		{
			o.Emission = _OutlineColor.rgb;
		}
		ENDCG
		

		Tags{ "RenderType" = "Transparent"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		ZWrite On
		ZTest LEqual
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma shader_feature _USEALBEDOMAP_ON
		#pragma shader_feature _USECUSTOMAMBIENTCOLOR_ON
		#pragma shader_feature _USEMETALLICTEX_ON
		#pragma shader_feature _USEROUGHNESSTEX_ON
		#pragma shader_feature _USEMASKTEX_ON
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows exclude_path:deferred vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _NormalStrength;
		uniform sampler2D _NormalTex;
		uniform float4 _NormalTex_ST;
		uniform float _InvertNormal;
		uniform float4 _AlbedoColor;
		uniform float4 _AlbedoColorOverlay;
		uniform float _OverlayStrength;
		uniform sampler2D _AlbedoTex;
		uniform float4 _AlbedoTex_ST;
		uniform float4 AmbientColorGlobal;
		uniform float4 _AmbientColor;
		uniform float _AmbientIntensity;
		uniform sampler2D _EmissiveTex;
		uniform float4 _EmissiveTex_ST;
		uniform float _EmissiveTexStrength;
		uniform float4 _EmissiveColor;
		uniform float _Metallic;
		uniform sampler2D _MetallicTex;
		uniform float4 _MetallicTex_ST;
		uniform float _Roughness;
		uniform sampler2D _RoughnessTex;
		uniform float4 _RoughnessTex_ST;
		uniform float _RoughnessTexIntensity;
		uniform sampler2D _MaskTex;
		uniform float4 _MaskTex_ST;
		uniform float _Cutoff = 0.5;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			v.vertex.xyz += 0;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_NormalTex = i.uv_texcoord * _NormalTex_ST.xy + _NormalTex_ST.zw;
			float3 tex2DNode55 = UnpackNormal( tex2D( _NormalTex, uv_NormalTex ) );
			float4 appendResult115 = (float4(tex2DNode55.r , lerp(tex2DNode55.g,( 1.0 - tex2DNode55.g ),_InvertNormal) , tex2DNode55.b , 0.0));
			float4 appendResult117 = (float4(( _NormalStrength * appendResult115 ).xy , tex2DNode55.b , 0.0));
			o.Normal = appendResult117.xyz;
			float2 uv_AlbedoTex = i.uv_texcoord * _AlbedoTex_ST.xy + _AlbedoTex_ST.zw;
			float4 tex2DNode54 = tex2D( _AlbedoTex, uv_AlbedoTex );
			#ifdef _USEALBEDOMAP_ON
				float4 staticSwitch119 = ( ( _AlbedoColorOverlay * _OverlayStrength ) * tex2DNode54 );
			#else
				float4 staticSwitch119 = _AlbedoColor;
			#endif
			o.Albedo = staticSwitch119.rgb;
			#ifdef _USECUSTOMAMBIENTCOLOR_ON
				float4 staticSwitch162 = _AmbientColor;
			#else
				float4 staticSwitch162 = ( unity_AmbientSky * AmbientColorGlobal );
			#endif
			float2 uv_EmissiveTex = i.uv_texcoord * _EmissiveTex_ST.xy + _EmissiveTex_ST.zw;
			float4 tex2DNode129 = tex2D( _EmissiveTex, uv_EmissiveTex );
			float4 temp_cast_3 = (_EmissiveTexStrength).xxxx;
			float4 lerpResult135 = lerp( tex2DNode129 , temp_cast_3 , tex2DNode129.a);
			o.Emission = ( ( staticSwitch162 * _AmbientIntensity ) + ( lerpResult135 * _EmissiveColor ) ).rgb;
			float4 temp_cast_5 = (_Metallic).xxxx;
			float2 uv_MetallicTex = i.uv_texcoord * _MetallicTex_ST.xy + _MetallicTex_ST.zw;
			#ifdef _USEMETALLICTEX_ON
				float4 staticSwitch58 = tex2D( _MetallicTex, uv_MetallicTex );
			#else
				float4 staticSwitch58 = temp_cast_5;
			#endif
			o.Metallic = staticSwitch58.r;
			float4 temp_cast_7 = (( 1.0 - _Roughness )).xxxx;
			float2 uv_RoughnessTex = i.uv_texcoord * _RoughnessTex_ST.xy + _RoughnessTex_ST.zw;
			#ifdef _USEROUGHNESSTEX_ON
				float4 staticSwitch60 = ( ( 1.0 - tex2D( _RoughnessTex, uv_RoughnessTex ) ) * _RoughnessTexIntensity );
			#else
				float4 staticSwitch60 = temp_cast_7;
			#endif
			o.Smoothness = staticSwitch60.r;
			o.Alpha = 1;
			float4 temp_cast_9 = (tex2DNode54.a).xxxx;
			float2 uv_MaskTex = i.uv_texcoord * _MaskTex_ST.xy + _MaskTex_ST.zw;
			#ifdef _USEMASKTEX_ON
				float4 staticSwitch140 = tex2D( _MaskTex, uv_MaskTex );
			#else
				float4 staticSwitch140 = temp_cast_9;
			#endif
			clip( staticSwitch140.r - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16400
179;35;1635;845;2652.439;1150.341;2.824338;True;True
Node;AmplifyShaderEditor.CommentaryNode;165;-1538.091,-606.8537;Float;False;1071.299;273.9151;Normals;7;117;116;118;115;114;113;55;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;166;-1526.397,-271.83;Float;False;1077.797;671.8712;Emissive;13;159;136;137;135;131;129;52;53;162;122;161;25;121;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;55;-1522.571,-553.3301;Float;True;Property;_NormalTex;Normal Tex;17;0;Create;True;0;0;False;0;None;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FogAndAmbientColorsNode;25;-1487.679,-210.9854;Float;False;unity_AmbientSky;0;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;168;-880.3759,441.8378;Float;False;912.4687;423.639;Roughness;7;60;142;124;123;61;143;56;;1,1,1,1;0;0
Node;AmplifyShaderEditor.ColorNode;121;-1489.932,-134.3799;Float;False;Global;AmbientColorGlobal;Ambient Color Global;1;1;[HDR];Create;True;0;0;False;0;0,0.1188681,0.509434,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;113;-1243.12,-433.857;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;164;-1548.838,-1121.384;Float;False;1299.35;445.8799;Albedo;7;108;120;109;1;119;62;54;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;56;-864.1498,575.7697;Float;True;Property;_RoughnessTex;Roughness Tex;12;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;129;-1252.132,-10.36825;Float;True;Property;_EmissiveTex;Emissive Tex;19;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ToggleSwitchNode;114;-1093.356,-462.9146;Float;False;Property;_InvertNormal;Invert Normal;16;0;Create;True;0;0;False;0;0;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;122;-1223.3,-204.994;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;108;-1525.128,-1056.479;Float;False;Property;_OverlayStrength;Overlay Strength;8;0;Create;True;0;0;False;0;1;0.5;0.5;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;131;-1252.652,181.5091;Float;False;Property;_EmissiveTexStrength;Emissive Tex Strength;20;0;Create;True;0;0;False;0;1;0.5;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;161;-1491.676,36.3373;Float;False;Property;_AmbientColor;Ambient Color;3;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;1;-1231.773,-1066.981;Float;False;Property;_AlbedoColorOverlay;Albedo Color Overlay;7;1;[HDR];Create;True;0;0;False;0;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;136;-1489.702,207.6824;Float;False;Property;_EmissiveColor;Emissive Color;21;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;135;-959.9653,-5.109568;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;115;-878.9419,-505.2575;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;167;-1525.989,439.53;Float;False;597.0671;314.6807;Metallic;3;58;59;57;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;169;-427.3672,-594.148;Float;False;596.1321;254.1381;Opacity Mask;2;140;141;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;118;-1242.716,-548.9995;Float;False;Property;_NormalStrength;Normal Strength;18;0;Create;True;0;0;False;0;1;1;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;124;-566.5151,580.8588;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;162;-1082.58,-206.8835;Float;False;Property;_UseCustomAmbientColor;Use Custom Ambient Color;2;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;53;-1263.032,-93.31845;Float;False;Property;_AmbientIntensity;Ambient Intensity;4;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;171;-98.57813,-6.833507;Float;False;453.8268;328.223;Outline;2;170;172;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;109;-974.9489,-1064.786;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;61;-862.0762,491.111;Float;False;Property;_Roughness;Roughness;9;0;Create;True;0;0;False;0;1;0.05;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;54;-1233.198,-887.2599;Float;True;Property;_AlbedoTex;Albedo Tex;6;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;143;-860.4949,774.2911;Float;False;Property;_RoughnessTexIntensity;Roughness Tex Intensity;10;0;Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;137;-811.7037,-5.179026;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;57;-1503.357,562.6653;Float;True;Property;_MetallicTex;MetallicTex;15;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;142;-413.1871,580.9769;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;59;-1502.72,488.2339;Float;False;Property;_Metallic;Metallic;13;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;120;-788.2114,-1066.266;Float;False;Property;_AlbedoColor;Albedo Color;1;1;[HDR];Create;True;0;0;False;0;1,1,1,0;0.636,0.4784587,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;62;-718.8075,-813.3267;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;173;-9.170726,348.4413;Float;False;Property;_OutlineSize;Outline Size;25;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;141;-412.0577,-548.9423;Float;True;Property;_MaskTex;Mask Tex;23;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;172;-71.65707,46.46642;Float;False;Property;_OutlineColor;Outline Color;24;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;-749.9314,-202.7944;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;116;-748.3445,-504.2195;Float;False;2;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.OneMinusNode;123;-570.5565,495.4673;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;140;-91.15945,-542.9388;Float;False;Property;_UseMaskTex;Use Mask Tex;22;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;117;-616.5261,-500.3747;Float;False;FLOAT4;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.OutlineNode;170;132.4561,53.63092;Float;False;1;True;None;2;3;Front;3;0;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StaticSwitch;58;-1208.547,489.5292;Float;False;Property;_UseMetallicTex;Use Metallic Tex;14;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;119;-547.1434,-813.778;Float;False;Property;_UseAlbedoMap;Use Albedo Map;5;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;60;-272.753,500.1423;Float;False;Property;_UseRoughnessTex;Use Roughness Tex;11;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;159;-576.0899,-201.3394;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;10;519.377,-584.6972;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;OWL/MasterShader_OL;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Transparent;;Geometry;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0.06;0,0,0,0;VertexOffset;False;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;28;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;113;0;55;2
WireConnection;114;0;55;2
WireConnection;114;1;113;0
WireConnection;122;0;25;0
WireConnection;122;1;121;0
WireConnection;135;0;129;0
WireConnection;135;1;131;0
WireConnection;135;2;129;4
WireConnection;115;0;55;1
WireConnection;115;1;114;0
WireConnection;115;2;55;3
WireConnection;124;0;56;0
WireConnection;162;1;122;0
WireConnection;162;0;161;0
WireConnection;109;0;1;0
WireConnection;109;1;108;0
WireConnection;137;0;135;0
WireConnection;137;1;136;0
WireConnection;142;0;124;0
WireConnection;142;1;143;0
WireConnection;62;0;109;0
WireConnection;62;1;54;0
WireConnection;52;0;162;0
WireConnection;52;1;53;0
WireConnection;116;0;118;0
WireConnection;116;1;115;0
WireConnection;123;0;61;0
WireConnection;140;1;54;4
WireConnection;140;0;141;0
WireConnection;117;0;116;0
WireConnection;117;2;55;3
WireConnection;170;0;172;0
WireConnection;170;1;173;0
WireConnection;58;1;59;0
WireConnection;58;0;57;0
WireConnection;119;1;120;0
WireConnection;119;0;62;0
WireConnection;60;1;123;0
WireConnection;60;0;142;0
WireConnection;159;0;52;0
WireConnection;159;1;137;0
WireConnection;10;0;119;0
WireConnection;10;1;117;0
WireConnection;10;2;159;0
WireConnection;10;3;58;0
WireConnection;10;4;60;0
WireConnection;10;10;140;0
WireConnection;10;11;170;0
ASEEND*/
//CHKSM=B6786D308832B329C7B526E1FD58997B91D46F7E