using MagicaVoxelAnimation.Graphics;
using MagicaVoxelAnimation.Render;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicaVoxelAnimation.Models.Animation
{
    abstract class ModelRenderManager
    {
        private Model _Model;
        private RenderData<BlockRenderData>[][] _Parts;

        public ModelRenderManager(BlockDataManager bdm, Model model)
        {
            _Model = model;

            _Parts = new RenderData<BlockRenderData>[model.Parts.Length][];
            Voxel.Model vmodel = new Voxel.Model();
            vmodel.Palette = model.Palette;
            vmodel.SizeX = model.SizeX;
            vmodel.SizeY = model.SizeY;
            vmodel.SizeZ = model.SizeZ;
            for (int i = 0; i < model.Parts.Length; ++i)
            {
                bdm.Clear();
                bdm.BeginData();
                vmodel.Voxel = model.Parts[i].Voxel;
                //don't need to save it
                var provider = new Voxel.ModelDataProvider(vmodel);
                provider.Translation = new Vector4(model.Parts[i].Translation, 0);
                provider.FlushRenderData(bdm);
                _Parts[i] = bdm.EndData();
            }
        }

        public void Render()
        {
            for (int i = 0; i < _Model.Parts.Length; ++i)
            {
                SetTransform(ref _Model.Parts[i].Transform);
                foreach (var d in _Parts[i])
                {
                    Render(d);
                }
            }
        }

        protected abstract void SetTransform(ref Matrix m);
        protected abstract void Render(RenderData<BlockRenderData> d);
    }
}
