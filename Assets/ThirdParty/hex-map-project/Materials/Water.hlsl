#include "HexMetrics.hlsl"

float Foam (float shore, float2 worldXZ, float time, UnityTexture2D noiseTex) {
	shore = sqrt(shore) * 0.9;

	float2 noiseUV = worldXZ + _Time.y * 0.25;
	float4 noise = noiseTex.Sample(noiseTex.samplerstate, noiseUV * (2 * TILING_SCALE));

	float distortion1 = noise.x * (1 - shore);
	float foam1 = sin((shore + distortion1) * 10 - _Time.y);
	foam1 *= foam1;

	float distortion2 = noise.y * (1 - shore);
	float foam2 = sin((shore + distortion2) * 10 + _Time.y + 2);
	foam2 *= foam2 * 0.7;

	return max(foam1, foam2) * shore;
}

float River (float2 riverUV, float time, UnityTexture2D noiseTex) {
	float2 uv = riverUV;
	uv.x = uv.x * 0.0625 + _Time.y * 0.005;
	uv.y -= _Time.y * 0.25;
	float4 noise = noiseTex.Sample(noiseTex.samplerstate, uv);

	float2 uv2 = riverUV;
	uv2.x = uv2.x * 0.0625 - _Time.y * 0.0052;
	uv2.y -= _Time.y * 0.23;
	float4 noise2 = noiseTex.Sample(noiseTex.samplerstate, uv2);

	return noise.r * noise2.w;
}

float Waves (float2 worldXZ, float time, UnityTexture2D noiseTex) {
	float2 uv1 = worldXZ;
	uv1.y += time;
	float4 noise1 = noiseTex.Sample(noiseTex.samplerstate, uv1 * (3 * TILING_SCALE));

	float2 uv2 = worldXZ;
	uv2.x += time;
	float4 noise2 = noiseTex.Sample(noiseTex.samplerstate, uv2 * (3 * TILING_SCALE));

	float blendWave = sin(
		(worldXZ.x + worldXZ.y) * 0.1 +
		(noise1.y + noise2.z) + time
	);
	blendWave *= blendWave;

	float waves =
		lerp(noise1.z, noise1.w, blendWave) +
		lerp(noise2.x, noise2.y, blendWave);
	return smoothstep(0.75, 2, waves);
}