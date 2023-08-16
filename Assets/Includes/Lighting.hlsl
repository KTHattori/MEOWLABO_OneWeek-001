#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED

#ifndef SHADERGRAPH_PREVIEW
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#endif

void CalculateMainLight_float(float3 WorldPos, out float3 Direction, out float3 Color, 
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
		ShadowAttenuation = SampleScreenSpaceShadowmap(shadowCoord);
	#else
		half4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
	#endif
	Light mainLight = GetMainLight(shadowCoord);
	Direction = mainLight.direction;
	Color = mainLight.color;
	DistanceAttenuation = mainLight.distanceAttenuation;

	#if !defined(_MAIN_LIGHT_SHADOWS) || defined(_RECEIVE_SHADOWS_OFF)
		ShadowAttenuation = 1;
	#else
		ShadowSamplingData shadowSamplingData = GetMainLightShadowSamplingData();
		float shadowStrength = GetMainLightShadowStrength();
		ShadowAttenuation = SampleShadowmap(shadowCoord,
		TEXTURE2D_ARGS(_MainLightShadowmapTexture,sampler_MainLightShadowmapTexture),
		shadowSamplingData, shadowStrength,false);
	#endif
#endif
}

void AddAdditionalLights_float(float Smoothness, float3 WorldPosition, float3 WorldNormal, float3 WorldView,
		float MainDiffuse, float MainSpecular, float3 MainColor, 
		out float Diffuse, out float Specular, out float3 Color)
{
		Diffuse = MainDiffuse;
		Specular = MainSpecular;
		Color = MainColor * (MainDiffuse + MainSpecular);

#if defined(SHADERGRAPH_PREVIEW)
#else 
	// get count of pixel lights
	int count = GetAdditionalLightsCount();
	// loop over pixel lights
	for(int i = 0;i < count;++i)
	{
		Light light = GetAdditionalLight(i,WorldPosition);
		float NdotL = saturate(dot(WorldNormal,light.direction));
		float atten = light.distanceAttenuation * light.shadowAttenuation;
		float thisDiffuse = atten * NdotL;
		float thisSpecular = LightingSpecular(thisDiffuse,light.direction,WorldNormal,WorldView,1,Smoothness);
		Diffuse += thisDiffuse;
		Specular += thisSpecular;
		Color += light.color * (thisDiffuse + thisSpecular);
	}
#endif
		half total = Diffuse + Specular;
		// If no lights, use the main light's color
		Color = total <= 0 ? MainColor : Color / total;
}
#endif