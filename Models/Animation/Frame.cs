using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicaVoxelAnimation.Models.Animation
{
    class Frame
    {
        public PartTransform[] Transforms;

        public Frame(int n)
        {
            Transforms = new PartTransform[n];
            for (int i = 0; i < n; ++i)
            {
                Transforms[i].Scaling = new Vector3(1, 1, 1);
            }
        }

        public void TransformModel(Model model)
        {
            for (int i = 0; i < model.Parts.Length; ++i)
            {
                var p = model.Parts[i];
                var tr1 = Matrix.Translation(p.BasePoint * -1.0f);
                var s = Matrix.Scaling(Transforms[i].Scaling);
                var r = Matrix.RotationQuaternion(Transforms[i].Rotation);
                var tr2 = Matrix.Translation(p.BasePoint);
                if (i == 0)
                {
                    p.Transform = tr1 * s * r * tr2;
                }
                else
                {
                    p.Transform = tr1 * s * r * tr2 * model.Parts[p.ParentPart].Transform;
                }
            }
        }
    }
}
