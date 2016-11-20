using MagicaVoxelAnimation.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicaVoxelAnimation.Render
{
    class BlockDataManager
    {
        private RenderManager _RenderManager;

        private const int BufferLength = 16384;

        private BlockRenderData[] _CurrentBuffer;
        private int _CurrentBufferPos;
        private List<RenderData<BlockRenderData>> _RenderData = new List<RenderData<BlockRenderData>>();

        private Action<RenderData<BlockRenderData>> _SetupRenderData;

        public BlockDataManager(RenderManager rm)
        {
            _RenderManager = rm;
        }

        public void SetLayoutFromShader<T>(Shader<T> shader) where T : struct
        {
            _SetupRenderData = delegate(RenderData<BlockRenderData> rd)
            {
                rd.SetLayoutFromShader(shader);
            };
        }

        public void Clear()
        {
            _CurrentBuffer = null;
            _CurrentBufferPos = 0;
            _RenderData.Clear();
        }

        public void BeginData()
        {
            if (_SetupRenderData == null)
            {
                throw new InvalidOperationException();
            }

            _CurrentBuffer = new BlockRenderData[BufferLength];
            _CurrentBufferPos = 0;
            _RenderData.Clear();
        }

        public void AppendRenderData(BlockRenderData data)
        {
            if (_CurrentBuffer == null)
            {
                throw new InvalidOperationException();
            }

            if (_CurrentBufferPos >= _CurrentBuffer.Length)
            {
                _CurrentBufferPos = 0;

                var rd = RenderData<BlockRenderData>.Create(_RenderManager,
                    SharpDX.Direct3D.PrimitiveTopology.PointList, _CurrentBuffer);
                _RenderData.Add(rd);

                _SetupRenderData(rd);
            }
            _CurrentBuffer[_CurrentBufferPos++] = data;
        }

        public RenderData<BlockRenderData>[] EndData()
        {
            var rd = RenderData<BlockRenderData>.Create(_RenderManager,
                SharpDX.Direct3D.PrimitiveTopology.PointList, _CurrentBuffer, _CurrentBufferPos);
            _RenderData.Add(rd);

            _SetupRenderData(rd);

            _CurrentBuffer = null;
            _CurrentBufferPos = 0;

            return _RenderData.ToArray();
        }
    }
}
