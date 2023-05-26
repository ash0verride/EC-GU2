sampler sceneSampler;

texture maskTexture;
sampler maskSampler = sampler_state{Texture = maskTexture;};

float4 LightMask(float2 coords: TEXCOORD) : COLOR
{
	float4 sourceColour = tex2D(sceneSampler, coords);
	float4 maskColour = tex2D(maskSampler, coords);

	return sourceColour * maskColour;
}

technique
{
	pass
	{
		PixelShader = compile ps_2_0 LightMask();
	}
}