using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicaVoxelAnimation.Models.Animation
{
    enum SegmentLoopMethod
    {
        NoLoop,
        Loop,
        ReverseLoop,
    }

    class Segment
    {
        public PartTransform[] TransformsStart;
        public PartTransform[] TransformsEnd;
        public InterpolationMethod[] InterpRotation;
        public InterpolationMethod[] InterpScaling;
        public float[] InterpolateRotateEndPoint;
        public SegmentLoopMethod Loop;
        public float Length;

        public void SetupFrame(Frame frame, float time)
        {
            time /= Length;
            if (time > 1)
            {
                if (Loop == SegmentLoopMethod.Loop)
                {
                    time -= (float)Math.Floor(time);
                }
                else if (Loop == SegmentLoopMethod.ReverseLoop)
                {
                    time -= (float)Math.Floor(time / 2) * 2;
                    if (time > 1)
                    {
                        time = 2 - time;
                    }
                }
            }
            if (frame.Transforms == null || frame.Transforms.Length != TransformsStart.Length)
            {
                frame.Transforms = new PartTransform[TransformsStart.Length];
            }
            for (int i = 0; i < TransformsStart.Length; ++i)
            {
                var timer = InterpRotation[i].Interpolate(time) / InterpolateRotateEndPoint[i];
                var times = InterpScaling[i].Interpolate(time);
                var s = TransformsStart[i];
                var e = TransformsEnd[i];
                frame.Transforms[i].Rotation = Quaternion.Slerp(s.Rotation, e.Rotation, timer);
                frame.Transforms[i].Scaling = Vector3.Lerp(s.Scaling, e.Scaling, times);
            }
        }
    }
}
