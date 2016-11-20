using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicaVoxelAnimation.Render
{
    abstract class BlockDataProvider
    {
        protected abstract int Width { get; }
        protected abstract int Height { get; }

        protected abstract bool AnyBlockAt(Coord coord);
        protected abstract int GetBlockColor(Coord coord);

        public Vector4 Translation;

        public void FlushRenderData(BlockDataManager buffer)
        {
            for (int gridZ = 0; gridZ < Height; ++gridZ)
            {
                for (int gridY = 0; gridY < Width; ++gridY)
                {
                    for (int gridX = 0; gridX < Width; ++gridX)
                    {
                        AppendBlockRenderData(buffer, gridX, gridY, gridZ);
                    }
                }
            }
        }

        private void AppendBlockRenderData(BlockDataManager buffer, int x, int y, int z)
        {
            Coord coord = new Coord(x, y, z);
            var color = GetBlockColor(coord);
            if (color == 0) return;

            for (int face = 0; face < 6; ++face)
            {
                var faceDir = new Coord.Direction1(face);
                if (!IsNormalCubeBeside(coord, faceDir))
                {
                    buffer.AppendRenderData(new BlockRenderData
                    {
                        pos = Translation + coord.ToVector4(1.0f) + faceDir.coord.ToVector4(0) * 0.5f,
                        col = GetColorFromInt(color),
                        dir_u = 0.5f * faceDir.U().coord.ToVector4(0),
                        dir_v = 0.5f * faceDir.V().coord.ToVector4(0),
                        aooffset = GetAOOffset(coord, face),
                        lightness = GetLightnessVec(coord, face),
                    });
                }
            }
        }

        private bool IsNormalCubeBeside(Coord coord, Coord.Direction1 offset)
        {
            return AnyBlockAt(coord.WithOffset(offset.coord));
        }

        private Vector4 GetColorFromInt(int color)
        {
            int x = color & 255;
            int y = color >> 8 & 255;
            int z = color >> 16 & 255;
            return new Vector4(z / 255.0f, y / 255.0f, x / 255.0f, 1.0f);
        }

        private bool MakeAOInner(Coord coord, Coord.Direction2 dir)
        {
            return AnyBlockAt(coord.WithOffset(dir.coord)) ||
                AnyBlockAt(coord.WithOffset(dir.Devide(0).coord)) ||
                AnyBlockAt(coord.WithOffset(dir.Devide(1).coord));
        }

        private Vector4 GetAOOffset(Coord coord, int face)
        {
            Coord.Direction1 dir = new Coord.Direction1(face);
            var offsetCoord = coord.WithOffset(dir.coord);
            return AmbientOcculsionTexture.MakeAOOffset(
                MakeAOInner(offsetCoord, dir.UVPN(0)),
                MakeAOInner(offsetCoord, dir.UVPN(1)),
                MakeAOInner(offsetCoord, dir.UVPN(2)),
                MakeAOInner(offsetCoord, dir.UVPN(3)));
        }

        private float[] sunlightOnFace = new float[] { 0.60f, 0.52f, 0.42f, 0.40f, 1.0f, 0.3f, 0.39f };

        private float LightnessByteToFloat(int face, byte b)
        {
            //return sunlightOnFace[face] * 0.8f;
            if (b == 0) return 0.0f;
            float ret = b / 16.0f;
            if (b == 14)
            {
                ret *= sunlightOnFace[face];
            }
            else
            {
                ret *= sunlightOnFace[6];
            }
            return ret;
        }

        private float MakeLightnessForVertex(int face, byte ba, byte bb, byte bc, byte bd)
        {
            float a = LightnessByteToFloat(face, ba), b = LightnessByteToFloat(face, bb),
                c = LightnessByteToFloat(face, bc), d = LightnessByteToFloat(face, bd);
            float ret;
            if (b == 0.0f && c == 0.0f)
            {
                ret = a;
            }
            else
            {
                int count = 1;
                float sum = a;
                if (b != 0) { ++count; sum += b; }
                if (c != 0) { ++count; sum += c; }
                if (d != 0) { ++count; sum += d; }
                ret = sum / count;
            }
            return ret * 1.2f;
        }

        private byte GetLightnessOnFace(Coord coord, int face)
        {
            return 14;
        }

        private float GetLightnessForVertex(Coord coord, int face, int index)
        {
            Coord.Direction2 vertex = new Coord.Direction1(face).UVPN(index);
            return MakeLightnessForVertex(face,
                GetLightnessOnFace(coord, face),
                GetLightnessOnFace(coord.WithOffset(vertex.Devide(0).coord), face),
                GetLightnessOnFace(coord.WithOffset(vertex.Devide(1).coord), face),
                GetLightnessOnFace(coord.WithOffset(vertex.coord), face)
                );
        }

        private Vector4 GetLightnessVec(Coord coord, int face)
        {
            return new Vector4(
                GetLightnessForVertex(coord, face, 0),
                GetLightnessForVertex(coord, face, 1),
                GetLightnessForVertex(coord, face, 2),
                GetLightnessForVertex(coord, face, 3)
                );
        }
    }
}
