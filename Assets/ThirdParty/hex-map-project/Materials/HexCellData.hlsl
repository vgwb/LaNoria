TEXTURE2D(_HexCellData);
SAMPLER(sampler_HexCellData);
float4 _HexCellData_TexelSize;

float4 FilterCellData (float4 data, bool editMode) {
	if (editMode) {
		data.xy = 1;
	}
	return data;
}

float4 GetCellData (float3 uv2, int index, bool editMode) {
	float2 uv;
	uv.x = (uv2[index] + 0.5) * _HexCellData_TexelSize.x;
	float row = floor(uv.x);
	uv.x -= row;
	uv.y = (row + 0.5) * _HexCellData_TexelSize.y;
	float4 data = SAMPLE_TEXTURE2D_LOD(_HexCellData, sampler_HexCellData, uv, 0);
	data.w *= 255;
	return FilterCellData(data, editMode);
}

float4 GetCellData (float2 cellDataCoordinates, bool editMode) {
	float2 uv = cellDataCoordinates + 0.5;
	uv.x *= _HexCellData_TexelSize.x;
	uv.y *= _HexCellData_TexelSize.y;
	return FilterCellData(
		SAMPLE_TEXTURE2D_LOD(_HexCellData, sampler_HexCellData, uv, 0), editMode
	);
}