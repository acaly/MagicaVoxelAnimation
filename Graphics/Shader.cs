using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Buffer = SharpDX.Direct3D11.Buffer;

namespace MagicaVoxelAnimation.Graphics
{
    public class Shader<T> : IDisposable
        where T : struct
    {
        public T buffer;

        private Device device;

        private VertexShader vertexShader;
        private GeometryShader geometryShader;
        private PixelShader pixelShader;

        private ShaderSignature signature;
        private Buffer constantBuffer;

        private List<SamplerState> samplers = new List<SamplerState>();
        private List<ShaderResourceView> resources = new List<ShaderResourceView>();

        private Shader() { }

        public static Shader<T> CreateFromFile(RenderManager manager, string filename)
        {
            return null;
        }

        public static Shader<T> CreateFromString(RenderManager manager, string shaderSource)
        {
            var device = manager.Device;

            Shader<T> ret = new Shader<T>();
            ret.buffer = default(T);

            ret.device = device;

            using (var vertexShaderByteCode = ShaderBytecode.Compile(shaderSource, "VS", "vs_4_0"))
            {
                ret.vertexShader = new VertexShader(device, vertexShaderByteCode);
                ret.signature = ShaderSignature.GetInputSignature(vertexShaderByteCode);
            }
            using (var geometryShaderByteCode = ShaderBytecode.Compile(shaderSource, "GS", "gs_4_0"))
            {
                ret.geometryShader = new GeometryShader(device, geometryShaderByteCode);
            }
            using (var pixelShaderByteCode = ShaderBytecode.Compile(shaderSource, "PS", "ps_4_0"))
            {
                ret.pixelShader = new PixelShader(device, pixelShaderByteCode);
            }

            ret.constantBuffer = new Buffer(device, Utilities.SizeOf<T>(), ResourceUsage.Default, BindFlags.ConstantBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0);

            return ret;
        }

        public ShaderSignature VertexSignature
        {
            get
            {
                return signature;
            }
        }

        public void SetupDeviceContext(DeviceContext context)
        {
            context.VertexShader.Set(vertexShader);
            context.VertexShader.SetConstantBuffer(0, constantBuffer);
            context.GeometryShader.Set(geometryShader);
            context.PixelShader.Set(pixelShader);

            for (int i = 0; i < samplers.Count; ++i)
            {
                if (samplers[i] != null) context.PixelShader.SetSampler(i, samplers[i]);
            }
            for (int i = 0; i < resources.Count; ++i)
            {
                if (resources[i] != null) context.PixelShader.SetShaderResource(i, resources[i]);
            }
        }

        public void UpdateConstantTable(DeviceContext context)
        {
            context.UpdateSubresource(ref buffer, constantBuffer);
        }

        public void CreateSamplerForPixelShader(int id, SamplerStateDescription desc)
        {
            while (samplers.Count <= id)
            {
                samplers.Add(null);
            }
            samplers[id] = new SamplerState(device, desc);
        }

        public void SetResourceForPixelShader(int slot, ShaderResourceView res)
        {
            while (resources.Count <= slot)
            {
                resources.Add(null);
            }
            resources[slot] = res;
        }

        public void Dispose()
        {
            Utilities.Dispose(ref vertexShader);
            Utilities.Dispose(ref geometryShader);
            Utilities.Dispose(ref pixelShader);
            Utilities.Dispose(ref signature);
            Utilities.Dispose(ref constantBuffer);
        }
    }
}
