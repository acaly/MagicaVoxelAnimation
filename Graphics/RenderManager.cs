using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Device = SharpDX.Direct3D11.Device;

namespace MagicaVoxelAnimation.Graphics
{
    public class RenderManager : IDisposable
    {
        private class MyRenderContext : RenderContext
        {
            public MyRenderContext(RenderManager m) :
                base(m.device.ImmediateContext, m.swapChain, m.screen)
            {
            }

            public new void NewFrame()
            {
                base.NewFrame();
            }
        }

        public static RenderManager Instance
        {
            get;
            private set;
        }

        public RenderManager()
        {
            if (Instance != null)
            {
                throw new Exception();
            }
            Instance = this;
        }

        public void Dispose()
        {
            if (Instance != this)
            {
                throw new Exception();
            }
            Instance = null;
        }

        public Device Device
        {
            get
            {
                return device;
            }
        }

        public RenderContext ImmediateContext
        {
            get
            {
                return renderContext;
            }
        }

        public Form Form
        {
            get
            {
                return form;
            }
        }

        public int TargetWidth
        {
            get
            {
                return form.ClientSize.Width;
            }
        }

        public int TargetHeight
        {
            get
            {
                return form.ClientSize.Height;
            }
        }

        private SwapChainDescription desc;
        private Form form;
        private Device device;
        private SwapChain swapChain;
        private Output screen;
        private Texture2D backBuffer;
        private RenderTargetView renderView;
        private Texture2D depthBuffer;
        private DepthStencilView depthView;

        public void InitDevice(Form form)
        {
            this.form = form;
            this.form.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.form.MaximizeBox = false;

            this.desc = new SwapChainDescription()
            {
                BufferCount = 1,
                ModeDescription =
                    new ModeDescription(form.ClientSize.Width, form.ClientSize.Height,
                                        new Rational(60, 1), Format.R8G8B8A8_UNorm),
                IsWindowed = true,
                OutputHandle = form.Handle,
                SampleDescription = new SampleDescription(1, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput
            };

            Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.Debug, this.desc, out this.device, out this.swapChain);

            using (var factory = this.swapChain.GetParent<Factory>())
            {
                this.screen = factory.Adapters[0].Outputs[0];
                factory.MakeWindowAssociation(this.form.Handle, WindowAssociationFlags.IgnoreAll);
            }

            var context = device.ImmediateContext;

            backBuffer = Texture2D.FromSwapChain<Texture2D>(swapChain, 0);
            renderView = new RenderTargetView(device, backBuffer);

            depthBuffer = new Texture2D(device, new Texture2DDescription()
            {
                Format = Format.D32_Float_S8X24_UInt,
                ArraySize = 1,
                MipLevels = 1,
                Width = form.ClientSize.Width,
                Height = form.ClientSize.Height,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default,
                BindFlags = BindFlags.DepthStencil,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None
            });
            depthView = new DepthStencilView(device, depthBuffer);

            context.Rasterizer.SetViewport(new Viewport(0, 0, form.ClientSize.Width, form.ClientSize.Height, 0.0f, 1.0f));
            context.OutputMerger.SetTargets(depthView, renderView);

            renderContext = new MyRenderContext(this);
        }

        private RenderLoop StartLoop()
        {
            form.Show();
            return new RenderLoop(form);
        }

        private MyRenderContext renderContext;

        private RenderContext RenderFrame()
        {
            var context = device.ImmediateContext;

            renderContext.NewFrame();
            context.ClearDepthStencilView(depthView, DepthStencilClearFlags.Depth, 1.0f, 0);
            context.ClearRenderTargetView(renderView, Color.AntiqueWhite);

            return renderContext;
        }

        private RenderLoop loop; //TODO dispose

        public RenderContext NextFrame(bool ignoreWindowsMessage = false)
        {
            if (loop == null)
            {
                loop = StartLoop();
            }
            if (ignoreWindowsMessage || loop.NextFrame())
            {
                return RenderFrame();
            }
            return null;
        }

        public void ShowWindow()
        {
            loop = StartLoop();
        }

        public bool FormIsClosed()
        {
            return !form.Visible;
        }
    }
}
