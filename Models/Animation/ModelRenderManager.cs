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
            Voxel.Model[] vmodel = new Voxel.Model[model.Parts.Length];
            for (int i = 0; i < model.Parts.Length; ++i)
            {
                vmodel[i] = new Voxel.Model();
                vmodel[i].Palette = model.Palette;
                vmodel[i].SizeX = model.SizeX;
                vmodel[i].SizeY = model.SizeY;
                vmodel[i].SizeZ = model.SizeZ;
                vmodel[i].Voxel = model.Parts[i].Voxel;
            }

            for (int i = 0; i < model.Parts.Length; ++i)
            {
                var p = model.Parts[i];
                bdm.Clear();
                bdm.BeginData();
                Voxel.Model[] adj = null;
                if (p.AdjacentParts != null)
                {
                    adj = p.AdjacentParts.Select(ii => vmodel[ii]).ToArray();
                }
                var provider = new Voxel.ModelDataProvider(vmodel[i], adj);
                provider.Translation = new Vector4(p.Translation, 0);
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
