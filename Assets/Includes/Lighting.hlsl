#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED

#ifndef SHADERGRAPH_PREVIEW
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#endif

void CalculateMainLight_float(float3 WorldPos, out float3 Direction, out float3 Color, 
		out float DistanceAttenuation,out float ShadowAttenuation) {
#if defined(SHADERGRAPH_PREVIEW)
	Direction = float3(0.5, 0.5, 0);
	Color = 1;
	DistanceAttenuation = 1;
	ShadowAttenuation = 1;
#else
	#if defined(SHADOWS_SCREEN)
		float4 clipPos = TransformWorldToHClip(WorldPos);
		float4 shadowCoord = ComputeScreenPos(clipPos);
	#else
		float4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
	#endif
	Light mainLight = GetMainLight(shadowCoord);
	Direction = mainLight.direction;
	Color = mainLight.color;
	DistanceAttenuation = mainLight.distanceAttenuation;
	ShadowAttenuation = mainLight.shadowAttenuation;
#endif
}

void CalculateMainLight_half(float3 WorldPos, out float3 Direction, out float3 Color, 
		out half DistanceAttenuation,out half ShadowAttenuation) {
#if defined(SHADERGRAPH_PREVIEW)
	Direction = half3(0.5, 0.5, 0);
	Color = 1;
	DistanceAttenuation = 1;
	ShadowAttenuation = 1;
#else
	#if defined(SHADOWS_SCREEN)
		half4 clipPos = TransformWorldToHClip(WorldPos);
		half4 shadowCoord = ComputeScreenPos(clipPos);
	#else
		half4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
	#endif
	Light mainLight = GetMainLight(shadowCoord);
	Direction = mainLight.direction;
	Color = mainLight.color;
	DistanceAttenuation = mainLight.distanceAttenuation;
	ShadowAttenuation = mainLight.shadowAttenuation;
#endif
}

void DirectSpecular_float(float3 Specular, float Smoothness, float3 Direction, float3 Color, float3 WorldNormal, float3 WorldView, out float3 Out)
{
#if SHADERGRAPH_PREVIEW
   Out = 0;
#else
   Smoothness = exp2(10 * Smoothness + 1);
   WorldNormal = normalize(WorldNormal);
   WorldView = SafeNormalize(WorldView);
   Out = LightingSpecular(Color, Direction, WorldNormal, WorldView, float4(Specular, 0), Smoothness);
#endif
}

void DirectSpecular_half(half3 Specular, half Smoothness, half3 Direction, half3 Color, half3 WorldNormal, half3 WorldView, out half3 Out)
{
#if SHADERGRAPH_PREVIEW
   Out = 0;
#else
   Smoothness = exp2(10 * Smoothness + 1);
   WorldNormal = normalize(WorldNormal);
   WorldView = SafeNormalize(WorldView);
   Out = LightingSpecular(Color, Direction, WorldNormal, WorldView, half4(Specular, 0), Smoothness);
#endif
}

void CalculateAdditionalLights_float(float3 SpecularColor, float Smoothness, float3 WorldPosition, float3 WorldNormal, float3 WorldView, out float3 Diffuse, out float3 Specular, out float3 Color)
{
   float3 diffuseColor = 0;
   float3 specularColor = 0;
   float3 lightColor = 0;

#if defined(SHADERGRAPH_PREVIEW)
#else
   Smoothness = exp2(10 * Smoothness + 1);
   WorldNormal = normalize(WorldNormal);
   WorldView = SafeNormalize(WorldView);
   int pixelLightCount = GetAdditionalLightsCount();
   for (int i = 0; i < pixelLightCount; ++i)
   {
       Light light = GetAdditionalLight(i, WorldPosition);
       float3 attenuatedLightColor = light.color * (light.distanceAttenuation * light.shadowAttenuation);
       diffuseColor += LightingLambert(attenuatedLightColor, light.direction, WorldNormal);
       specularColor += LightingSpecular(attenuatedLightColor, light.direction, WorldNormal, WorldView, float4(SpecularColor, 0), Smoothness);
	   lightColor += attenuatedLightColor;
   }
#endif

   Diffuse = diffuseColor;
   Specular = specularColor;
   Color = lightColor;
}

void CalculateAdditionalLights_half(half3 SpecularColor, half Smoothness, half3 WorldPosition, half3 WorldNormal, half3 WorldView, out half3 Diffuse, out half3 Specular, out half3 Color)
{
   half3 diffuseColor = 0;
   half3 specularColor = 0;
   half3 lightColor = 0;

#if defined(SHADERGRAPH_PREVIEW)
#else
   Smoothness = exp2(10 * Smoothness + 1);
   WorldNormal = normalize(WorldNormal);
   WorldView = SafeNormalize(WorldView);
   int pixelLightCount = GetAdditionalLightsCount();
   for (int i = 0; i < pixelLightCount; ++i)
   {
       Light light = GetAdditionalLight(i, WorldPosition);
       half3 attenuatedLightColor = light.color * (light.distanceAttenuation * light.shadowAttenuation);
       diffuseColor += LightingLambert(attenuatedLightColor, light.direction, WorldNormal);
       specularColor += LightingSpecular(attenuatedLightColor, light.direction, WorldNormal, WorldView, half4(SpecularColor, 0), Smoothness);
	   lightColor += attenuatedLightColor;
   }
#endif

   Diffuse = diffuseColor;
   Specular = specularColor;
   Color = lightColor;
}
#endif