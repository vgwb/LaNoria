#include "Water.hlsl"
#include "HexCellData.hlsl"

// Used by Water and Water Shore shader graphs.
void GetVertexCellData_float (
	float3 Indices,
	float3 Weights,
	bool EditMode,
	out float2 Visibility
) {
	float4 cell0 = GetCellData(Indices, 0, EditMode);
	float4 cell1 = GetCellData(Indices, 1, EditMode);
	float4 cell2 = GetCellData(Indices, 2, EditMode);
	
	Visibility = 0;
	Visibility.x = cell0.x * Weights.x + cell1.x * Weights.y + cell2.x * Weights.z;
	Visibility.x = lerp(0.25, 1, Visibility.x);
	Visibility.y = cell0.y * Weights.x + cell1.y * Weights.y + cell2.y * Weights.z;
}

// Used by shader graphs that cross a cell edge: Estuary, River, and Road.
void GetVertexCellDataEdge_float (
	float3 Indices,
	float2 Weights,
	bool EditMode,
	out float2 Visibility
) {
	float4 cell0 = GetCellData(Indices, 0, EditMode);
	float4 cell1 = GetCellData(Indices, 1, EditMode);
	
	Visibility = 0;
	Visibility.x = cell0.x * Weights.x + cell1.x * Weights.y;
	Visibility.x = lerp(0.25, 1, Visibility.x);
	Visibility.y = cell0.y * Weights.x + cell1.y * Weights.y;
}

void GetFragmentDataEstuary_float (
	UnityTexture2D NoiseTexture,
	float2 RiverUV,
	float2 ShoreUV,
	float3 WorldPosition,
	float4 Color,
	float2 Visibility,
	float Time,
	out float3 BaseColor,
	out float Alpha,
	out float Exploration
) {
	float shore = ShoreUV.y;
	float foam = Foam(shore, WorldPosition.xz, Time, NoiseTexture);
	float waves = Waves(WorldPosition.xz, Time, NoiseTexture);
	waves *= 1 - shore;
	float shoreWater = max(foam, waves);

	float river = River(RiverUV, Time, NoiseTexture);

	float water = lerp(shoreWater, river, ShoreUV.x);

	float4 c = saturate(Color + water);
	BaseColor = c.rgb * Visibility.x;
	Alpha = c.a;
	Exploration = Visibility.y;
}

void GetFragmentDataRoad_float (
	UnityTexture2D NoiseTexture,
	float2 BlendUV,
	float3 WorldPosition,
	float4 Color,
	float2 Visibility,
	out float3 BaseColor,
	out float Alpha,
	out float Exploration
) {
	float4 noise = NoiseTexture.Sample(
		NoiseTexture.samplerstate, WorldPosition.xz * (3 * TILING_SCALE)
	);
	BaseColor = Color.rgb * ((noise.y * 0.75 + 0.25) * Visibility.x);
	Alpha = BlendUV.x;
	Alpha *= noise.x + 0.5;
	Alpha = smoothstep(0.4, 0.7, Alpha);
	Exploration = Visibility.y;
}

void GetFragmentDataRiver_float (
	UnityTexture2D NoiseTexture,
	float2 RiverUV,
	float4 Color,
	float2 Visibility,
	float Time,
	out float3 BaseColor,
	out float Alpha,
	out float Exploration
) {
	float river = River(RiverUV, Time, NoiseTexture);
	float4 c = saturate(Color + river);
	BaseColor = c.rgb * Visibility.x;
	Alpha = c.a;
	Exploration = Visibility.y;
}

void GetFragmentDataWater_float (
	UnityTexture2D NoiseTexture,
	float3 WorldPosition,
	float4 Color,
	float2 Visibility,
	float Time,
	out float3 BaseColor,
	out float Alpha,
	out float Exploration
) {
	float waves = Waves(WorldPosition.xz, Time, NoiseTexture);
	float4 c = saturate(Color + waves);

	BaseColor = c.rgb * Visibility.x;
	Alpha = c.a;
	Exploration = Visibility.y;
}

void GetFragmentDataShore_float (
	UnityTexture2D NoiseTexture,
	float2 ShoreUV,
	float3 WorldPosition,
	float4 Color,
	float2 Visibility,
	float Time,
	out float3 BaseColor,
	out float Alpha,
	out float Exploration
) {
	float shore = ShoreUV.y;
	float foam = Foam(shore, WorldPosition.xz, Time, NoiseTexture);
	float waves = Waves(WorldPosition.xz, Time, NoiseTexture);
	waves *= 1 - shore;
	float4 c = saturate(Color + max(foam, waves));
	
	BaseColor = c.rgb * Visibility.x;
	Alpha = c.a;
	Exploration = Visibility.y;
}