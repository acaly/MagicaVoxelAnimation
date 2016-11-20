using MagicaVoxelAnimation.Graphics;
using MagicaVoxelAnimation.Models.Animation;
using MagicaVoxelAnimation.Render;
using SharpDX;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MagicaVoxelAnimation
{
    static class Program
    {
        public struct VertexConstData
        {
            public Matrix transform;
        }

        public class ModelRenderer : ModelRenderManager
        {
            public ModelRenderer(BlockDataManager bdm, Model m)
                : base(bdm, m)
            {
            }

            public Shader<VertexConstData> Shader;
            public RenderContext Frame;
            public Camera Camera;
            public Matrix Proj;

            protected override void SetTransform(ref Matrix m)
            {
                Shader.buffer.transform = m * Camera.GetViewMatrix() * Proj;
                Shader.buffer.transform.Transpose();
                Frame.UpdateShaderConstant(Shader);
            }

            protected override void Render(RenderData<BlockRenderData> d)
            {
                Frame.SetRenderData(d);
                Frame.Draw(d);
            }
        }


        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var rm = new RenderManager();
            rm.InitDevice(new Form1());
            
            var shaderFace = Shader<VertexConstData>.CreateFromString(rm, BlockShader.Code);
            shaderFace.CreateSamplerForPixelShader(0, new SamplerStateDescription
            {
                Filter = Filter.MinMagMipLinear,
                AddressU = TextureAddressMode.Border,
                AddressV = TextureAddressMode.Border,
                AddressW = TextureAddressMode.Border,
            });

            var aotexture = new AmbientOcculsionTexture(rm);
            shaderFace.SetResourceForPixelShader(0, aotexture.ResourceView);

            var camera = new Camera(new Vector3(0, 0, 70));
            camera.SetForm(rm.Form);
            var proj = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, 800.0f / 600.0f, 0.1f, 1000.0f);

            var model = ModelLoader.LoadModel("hiyori.model");
            var segment = ModelLoader.LoadSegment("hiyori_walk.segment");
            var animationFrame = new Frame();
            float time = 0;

            var bdm = new BlockDataManager(rm);
            bdm.SetLayoutFromShader(shaderFace);
            var renderer = new ModelRenderer(bdm, model)
            {
                Camera = camera,
                Proj = proj,
                Shader = shaderFace,
            };
            RenderLoopHelper.Run(rm, false, delegate(RenderContext frame)
            {
                //--- render world ---

                camera.Step();

                rm.ImmediateContext.ApplyShader(shaderFace);

                time += 1.0f;
                segment.SetupFrame(animationFrame, time);
                animationFrame.TransformModel(model);

                renderer.Frame = frame;
                renderer.Render();

                //--- present ---

                frame.Present(true);
            });
        }
    }
}
