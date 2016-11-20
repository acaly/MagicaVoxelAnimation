using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicaVoxelAnimation.Graphics
{
    public class RenderContext
    {
        private DeviceContext context;
        private SwapChain swapChain;
        private Output screen;
        private Stopwatch clock;
        private long lastFrameMilliseconds;
        private long frameMilliseconds;

        public RenderContext(DeviceContext context, SwapChain swapChain, Output screen)
        {
            this.context = context;
            this.swapChain = swapChain;
            this.screen = screen;
            this.clock = new Stopwatch();
            this.clock.Start();
        }

        public long TotalTimeMilliseconds
        {
            get
            {
                return frameMilliseconds;
            }
        }

        public float FrameTimeSecond
        {
            get
            {
                return (frameMilliseconds - lastFrameMilliseconds) / 1000.0f;
            }
        }

        protected void NewFrame()
        {
            lastFrameMilliseconds = frameMilliseconds;
            frameMilliseconds = clock.ElapsedMilliseconds;
        }

        public void ApplyShader<T>(Shader<T> shader) where T : struct
        {
            shader.SetupDeviceContext(context);
        }

        public void UpdateShaderConstant<T>(Shader<T> shader) where T : struct
        {
            shader.UpdateConstantTable(context);
        }

        public void SetRenderData<T>(RenderData<T> data) where T : struct
        {
            data.SetupDeviceContext(context);
        }

        public void Draw<T>(RenderData<T> data) where T : struct
        {
            data.Render(context);
        }

        public void Present(bool vsync)
        {
            frameMilliseconds = clock.ElapsedMilliseconds;

            swapChain.Present(0, PresentFlags.None);

            ++frameCount;
            if (frameCount > 1000)
            {
                frameCount = 0;
                frameCountStartTime = clock.ElapsedMilliseconds;
            }
            if (vsync)
            {
                screen.WaitForVerticalBlank();
            }
        }

        private long frameCount = 0;
        private long frameCountStartTime = 0;

        public float Fps
        {
            get
            {
                return frameCount * 1000.0f / (clock.ElapsedMilliseconds - frameCountStartTime);
            }
        }
    }
}
