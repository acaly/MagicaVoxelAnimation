using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicaVoxelAnimation.Models.Animation
{
    abstract class InterpolationMethod
    {
        public abstract float Interpolate(float t);

        private class _Linear : InterpolationMethod
        {
            public override float Interpolate(float t)
            {
                return t;
            }
            public override string ToString()
            {
                return "Linear";
            }
        }

        public static readonly InterpolationMethod Linear = new _Linear();

        public static InterpolationMethod FromString(string name)
        {
            var f = typeof(InterpolationMethod).GetField(name);
            if (f == null)
            {
                return null;
            }
            return (InterpolationMethod)f.GetValue(null);
        }
    }
}
