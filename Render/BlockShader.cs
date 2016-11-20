using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicaVoxelAnimation.Render
{
    class BlockShader
    {
        public static readonly string Code = @"

struct VS_IN
{
    float4 pos : POSITION;
    float4 dir_u : TEXCOORD1;
    float4 dir_v : TEXCOORD2;
    float4 col : COLOR0;
    float4 aooffset : TEXCOORD3;
    float4 lightness : COLOR1;
};

struct GS_IN
{
    float4 pos : SV_POSITION;
    float4 dir_u : TEXCOORD1;
    float4 dir_v : TEXCOORD2;
    float4 col : COLOR0;
    float4 aooffset : TEXCOORD3;
    float4 lightness : COLOR1;
};

struct PS_IN
{
	float4 pos : SV_POSITION;
    float4 col : COLOR0;
    float4 aooffset : TEXCOORD3;
    float4 lightness : COLOR1;
    float4 pointCoord : TEXCOORD4;
};

cbuffer VS_CONSTANT_BUFFER
{
	float4x4 worldViewProj;
}

Texture2D faceTexture : register(t0);
SamplerState MeshTextureSampler : register(s0);

GS_IN VS(VS_IN input)
{
	GS_IN output = (GS_IN)0;

	output.pos = mul(input.pos, worldViewProj);
    output.dir_u = mul(input.dir_u, worldViewProj);
    output.dir_v = mul(input.dir_v, worldViewProj);
    output.aooffset = input.aooffset;

    float3 lightdir = normalize(float3(0, 0.6, 4));
    float nDotL = saturate(0.3 + 0.9 * dot(cross(input.dir_u.xyz, input.dir_v.xyz), -lightdir));
    //output.col = saturate(
    //    (
    //        nDotL * (input.lightness.x) * 1.2 +
    //        input.lightness.y * 0.6 + 0.07
    //    ) * 1.2
    //    * input.col * 1.3 );
    //output.col = saturate(nDotL * float4(0.4, 1.2, 0.0, 1.0));
    //output.col = saturate(nDotL * input.col * 1.2);
    output.col = input.col;
    output.lightness = input.lightness;

	return output;
}

[maxvertexcount(4)]
void GS(point GS_IN input[1], inout TriangleStream<PS_IN> triStream)
{
    PS_IN point_pp = (PS_IN)0;
    PS_IN point_pn = (PS_IN)0;
    PS_IN point_np = (PS_IN)0;
    PS_IN point_nn = (PS_IN)0;

    point_pp.col = input[0].col;
    point_pn.col = input[0].col;
    point_np.col = input[0].col;
    point_nn.col = input[0].col;

    point_pp.lightness = input[0].lightness;
    point_pn.lightness = input[0].lightness;
    point_np.lightness = input[0].lightness;
    point_nn.lightness = input[0].lightness;

    point_pp.pointCoord = float4(1, 1, 0, 0);
    point_pn.pointCoord = float4(1, 0, 0, 0);
    point_np.pointCoord = float4(0, 1, 0, 0);
    point_nn.pointCoord = float4(0, 0, 0, 0);

    point_pp.pos = input[0].pos + input[0].dir_u + input[0].dir_v;
    point_pn.pos = input[0].pos + input[0].dir_u - input[0].dir_v;
    point_np.pos = input[0].pos - input[0].dir_u + input[0].dir_v;
    point_nn.pos = input[0].pos - input[0].dir_u - input[0].dir_v;

    //input[0].aooffset = float4(0.25, 0.25, 0, 0);
    float ao1 = 0.25 / 4, ao2 = 0.25 - ao1;
    point_pp.aooffset = input[0].aooffset + float4(ao2,ao2,0,   0);
    point_pn.aooffset = input[0].aooffset + float4(ao2,ao1,0,   0);
    point_np.aooffset = input[0].aooffset + float4(ao1,ao2,0,   0);
    point_nn.aooffset = input[0].aooffset + float4(ao1,ao1,0,   0);
    //point_np.col = float4(1, 1, 1, 0);
    
	triStream.Append(point_pp);
	triStream.Append(point_pn);
	triStream.Append(point_np);
	triStream.Append(point_nn);
	triStream.RestartStrip();
}

float4 PS(PS_IN input) : SV_Target
{
    //float2 coord = input.tex.xy * 16;
    //coord.x = floor(coord.x) / 16;
    //coord.y = floor(coord.y) / 16;
    
    //float4 col = faceTexture.Sample(MeshTextureSampler, coord);

    float4 ret = input.col;

    float4 aocolor = faceTexture.Sample(MeshTextureSampler, input.aooffset.xy);
    ret = ret * (1 - aocolor.r * 0.2);

    float lightness = 
        input.lightness.x * input.pointCoord.x * input.pointCoord.y + 
        input.lightness.y * input.pointCoord.x * (1 - input.pointCoord.y) +
        input.lightness.z * (1 - input.pointCoord.x) * input.pointCoord.y + 
        input.lightness.w * (1 - input.pointCoord.x) * (1 - input.pointCoord.y);
    ret = ret * lightness * 1.2;

    return ret;
}
";
    }
}
