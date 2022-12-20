#include "../HexMetrics.hlsl"
#include "../HexCellData.hlsl"

void GetVertexCellData_float (
	float3 Indices,
	float3 Weights,
	bool EditMode,
	out float3 Terrain,
	out float4 Visibility
) {
	float4 cell0 = GetCellData(Indices, 0, EditMode);
	float4 cell1 = GetCellData(Indices, 1, EditMode);
	float4 cell2 = GetCellData(Indices, 2, EditMode);

	Terrain.x = cell0.w;
	Terrain.y = cell1.w;
	Terrain.z = cell2.w;

	Visibility.x = cell0.x;
	Visibility.y = cell1.x;
	Visibility.z = cell2.x;
	Visibility.xyz = lerp(0.25, 1, Visibility.xyz);
	Visibility.w = cell0.y * Weights.x + cell1.y * Weights.y + cell2.y * Weights.z;
}

float4 GetTerrainColor (
	UnityTexture2DArray TerrainTextures,
	float3 WorldPosition,
	float3 Terrain,
	float3 Color,
	float4 Visibility,
	int index
) {
	float3 uvw = float3(
		WorldPosition.xz * (2 * TILING_SCALE),
		Terrain[index]
	);
	float4 c = TerrainTextures.Sample(TerrainTextures.samplerstate, uvw);
	return c * (Color[index] * Visibility[index]);
}

void GetFragmentData_float (
	UnityTexture2DArray TerrainTextures,
	float3 WorldPosition,
	float3 Terrain,
	float4 Visibility,
	float3 Weights,
	UnityTexture2D GridTexture,
	bool ShowGrid,
	out float3 BaseColor,
	out float Exploration
) {
	float4 c =
		GetTerrainColor(TerrainTextures, WorldPosition, Terrain, Weights, Visibility, 0) +
		GetTerrainColor(TerrainTextures, WorldPosition, Terrain, Weights, Visibility, 1) +
		GetTerrainColor(TerrainTextures, WorldPosition, Terrain, Weights, Visibility, 2);

	float4 grid = 1;
	if (ShowGrid) {
		float2 gridUV = WorldPosition.xz;
		gridUV.x *= 1 / (4 * 8.66025404);
		gridUV.y *= 1 / (2 * 15.0);
		grid = GridTexture.Sample(GridTexture.samplerstate, gridUV);
	}

	BaseColor = c.rgb * grid.rgb;
	Exploration = Visibility.w;
}