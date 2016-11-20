using MagicaVoxelAnimation.Graphics;
using SharpDX;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicaVoxelAnimation.Render
{
    public struct BlockRenderData
    {
        [RenderDataElement(Format.R32G32B32A32_Float, "POSITION", 0)]
        public Vector4 pos;
        [RenderDataElement(Format.R32G32B32A32_Float, "TEXCOORD", 1)]
        public Vector4 dir_u;
        [RenderDataElement(Format.R32G32B32A32_Float, "TEXCOORD", 2)]
        public Vector4 dir_v;
        [RenderDataElement(Format.R32G32B32A32_Float, "COLOR", 0)]
        public Vector4 col;
        [RenderDataElement(Format.R32G32B32A32_Float, "TEXCOORD", 3)]
        public Vector4 aooffset;
        [RenderDataElement(Format.R32G32B32A32_Float, "COLOR", 1)]
        public Vector4 lightness;
    }
}
