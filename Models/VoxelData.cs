using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicaVoxelAnimation.Models
{
    [Serializable]
    public struct VoxelData
    {
        public int X, Y, Z;
        public int ColorIndex;
    }
}
