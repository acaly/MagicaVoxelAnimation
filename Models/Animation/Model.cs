using SharpDX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = System.Drawing.Color;

namespace MagicaVoxelAnimation.Models.Animation
{
    class Model
    {
        public Color[] Palette;
        public Part[] Parts;
        public int SizeX, SizeY, SizeZ;

        public class Part
        {
            public int ParentPart;
            public Vector3 BasePoint;
            public Vector3 Translation;
            public VoxelData[] Voxel;

            [NonSerialized]
            public Matrix Transform;
        }
    }
}
