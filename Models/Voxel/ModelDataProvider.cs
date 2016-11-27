using MagicaVoxelAnimation.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicaVoxelAnimation.Models.Voxel
{
    class ModelDataProvider : BlockDataProvider
    {
        private class ModelCoordDictionary : CoordDictionary<int>
        {
            protected override int AutoCreate(Coord pos)
            {
                return 0;
            }
        }
        private readonly int _Width, _Height;
        private readonly ModelCoordDictionary _Dictionary = new ModelCoordDictionary();
        private readonly ModelCoordDictionary _AdjDictionary = new ModelCoordDictionary();

        //TODO avoid adding adj to new dict. try to ask other provider
        public ModelDataProvider(Model model, Model[] adj = null)
        {
            _Width = Math.Max(model.SizeX, model.SizeY);
            _Height = model.SizeZ;

            for (int i = 0; i < model.Voxel.Length; ++i)
            {
                var v = model.Voxel[i];
                var c = model.Palette[v.ColorIndex].ToArgb();
                _Dictionary.Set(new Coord(v.X, v.Y, v.Z), c);
            }
            if (adj != null)
            {
                for (int j = 0; j < adj.Length; ++j)
                {
                    var m = adj[j];
                    for (int i = 0; i < m.Voxel.Length; ++i)
                    {
                        var v = m.Voxel[i];
                        var c = m.Palette[v.ColorIndex].ToArgb();
                        _AdjDictionary.Set(new Coord(v.X, v.Y, v.Z), c);
                    }
                }
            }
        }

        protected override int Width
        {
            get { return _Width; }
        }

        protected override int Height
        {
            get { return _Height; }
        }

        protected override bool AnyBlockAt(Coord coord)
        {
            return _Dictionary.Contains(coord) || _AdjDictionary.Contains(coord);
        }

        protected override int GetBlockColor(Coord coord)
        {
            int ret;
            if (_Dictionary.TryGet(coord, out ret))
            {
                return ret;
            }
            return 0;
        }
    }
}
