using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Device = SharpDX.Direct3D11.Device;
using Buffer = SharpDX.Direct3D11.Buffer;

namespace MagicaVoxelAnimation.Graphics
{
    public class RenderDataElementAttribute : Attribute
    {
        public RenderDataElementAttribute(Format format, string usage, int id)
        {
            this.Format = format;
            this.Usage = usage;
            this.Id = id;
        }

        public Format Format
        {
            get;
            private set;
        }

        public string Usage
        {
            get;
            private set;
        }

        public int Id
        {
            get;
            private set;
        }
    }

    public class RenderData<T> : IDisposable
        where T : struct
    {
        private RenderData() { }

        public static RenderData<T> Create(RenderManager manager, PrimitiveTopology topology, T[] data, int count = 0)
        {
            if (count == 0)
            {
                count = data.Length;
            }

            RenderData<T> ret = new RenderData<T>();
            ret.topology = topology;
            ret.device = manager.Device;
            ret.stride = Utilities.SizeOf<T>();
            ret.ResetBuffer(data, count);

            return ret;
        }

        private struct InputElementFieldInfo
        {
            public RenderDataElementAttribute attr;
            public int offset;
        }

        public static InputElement[] CreateLayoutElementsFromType()
        {
            Type type = typeof(T);
            List<InputElementFieldInfo> fieldList = new List<InputElementFieldInfo>();
            foreach (var field in type.GetFields())
            {
                var attr = Utilities.GetCustomAttribute<RenderDataElementAttribute>(field);
                if (attr == null) continue;
                int offset = Marshal.OffsetOf(type, field.Name).ToInt32();
                fieldList.Add(new InputElementFieldInfo { attr = attr, offset = offset });
            }
            fieldList.Sort((InputElementFieldInfo a, InputElementFieldInfo b) => a.offset.CompareTo(b.offset));
            return fieldList.Select(info => new InputElement(info.attr.Usage, info.attr.Id, info.attr.Format, info.offset, 0)).ToArray();
        }

        private Device device;

        private PrimitiveTopology topology;

        private int count;
        private int stride;

        private Buffer vertexBuffer;
        private VertexBufferBinding binding;
        private InputLayout layout;

        private List<ShaderResourceView> resources = new List<ShaderResourceView>();

        public void ResetBuffer(T[] data, int size)
        {
            if (data == null || data.Length == 0)
            {
                if (vertexBuffer != null)
                {
                    vertexBuffer.Dispose();
                    vertexBuffer = null;
                }
                count = 0;
            }
            else
            {
                //TODO use devicecontext.MapSubresource
                if (vertexBuffer != null)
                {
                    vertexBuffer.Dispose();
                }
                vertexBuffer = Buffer.Create<T>(device, BindFlags.VertexBuffer, data, size * Utilities.SizeOf<T>());
                count = data.Length;
            }

            if (vertexBuffer != null)
            {
                binding = new VertexBufferBinding(vertexBuffer, stride, 0);
            }
        }

        public void SetLayoutFromShader<TT>(Shader<TT> shader) where TT : struct
        {
            if (this.layout != null)
            {
                throw new Exception();
            }
            this.layout = new InputLayout(device, shader.VertexSignature, CreateLayoutElementsFromType());
        }

        public void SetupDeviceContext(DeviceContext context)
        {
            if (vertexBuffer == null) return;

            context.InputAssembler.InputLayout = layout;
            context.InputAssembler.PrimitiveTopology = topology;
            context.InputAssembler.SetVertexBuffers(0, binding);

            for (int i = 0; i < resources.Count; ++i)
            {
                if (resources[i] != null) context.PixelShader.SetShaderResource(i, resources[i]);
            }
        }

        public void Render(DeviceContext context)
        {
            if (vertexBuffer == null) return;

            context.Draw(count, 0);
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
        }
    }
}
