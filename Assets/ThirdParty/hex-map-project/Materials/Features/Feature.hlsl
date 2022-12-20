#include "../HexCellData.hlsl"

void GetVertexCellData_float (
	UnityTexture2D GridCoordinatesTexture,
	float3 WorldPosition,
	bool EditMode,
	out float2 Visibility
) {
	float2 gridUV = WorldPosition.xz;
	gridUV.x *= 1 / (4 * 8.66025404);
	gridUV.y *= 1 / (2 * 15.0);
	float2 cellDataCoordinates = floor(gridUV.xy) + GridCoordinatesTexture.SampleLevel(
		GridCoordinatesTexture.samplerstate, gridUV, 0
	).rg;
	cellDataCoordinates *= 2;

	float4 cellData = GetCellData(cellDataCoordinates, EditMode);
	Visibility.x = cellData.x;
	Visibility.x = lerp(0.25, 1, Visibility.x);
	Visibility.y = cellData.y;
}

void GetFragmentData_float (
	UnityTexture2D BaseTexture,
	float2 UV,
	float3 WorldPosition,
	float3 Color,
	float2 Visibility,
	out float3 BaseColor,
	out float Exploration
) {
	float3 c = BaseTexture.Sample(BaseTexture.samplerstate, UV).rgb * Color;
	BaseColor = c.rgb * Visibility.x;
	Exploration = Visibility.y;
}
