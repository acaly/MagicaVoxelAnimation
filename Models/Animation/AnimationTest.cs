using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicaVoxelAnimation.Models.Animation
{
    class AnimationTest
    {
        public static Model CreateModel()
        {
            var p = @"E:\Programmes\MagicaVoxel\myvox\hiyori_animation\";
            return ModelLoader.CreateFromVoxelModels(
                new[] {
                    Voxel.ModelLoader.Load(p + "body2.vox"),
                    Voxel.ModelLoader.Load(p + "head.vox"),
                    Voxel.ModelLoader.Load(p + "arm_l.vox"),
                    Voxel.ModelLoader.Load(p + "arm_r.vox"),
                    Voxel.ModelLoader.Load(p + "leg_l.vox"),
                    Voxel.ModelLoader.Load(p + "leg_r.vox"),
                },
                new[] {
                    new SharpDX.Vector3(20, 25, 0),
                    new SharpDX.Vector3(20, 24.5f, 28),
                    new SharpDX.Vector3(20.5f, 20, 25),
                    new SharpDX.Vector3(20.5f, 29, 25),
                    new SharpDX.Vector3(21, 23, 18),
                    new SharpDX.Vector3(21, 26, 18),
                },
                new[] {
                    new SharpDX.Vector3(0, 0, 0),
                    new SharpDX.Vector3(-0.01f, 0, 0),
                    new SharpDX.Vector3(0, 0.01f, 0),
                    new SharpDX.Vector3(0, -0.01f, 0),
                    new SharpDX.Vector3(0, 0.01f, 0),
                    new SharpDX.Vector3(0, -0.01f, 0),
                },
                new[] { -1, 0, 0, 0, 0, 0 }
                );
        }

        public static Segment CreateSegment()
        {
            return new Segment
            {
                Length = 20.0f,
                Loop = SegmentLoopMethod.ReverseLoop,
                TransformsStart = new[] { 
                    new PartTransform {
                        Rotation = Quaternion.Identity,
                        Scaling = new Vector3(1, 1, 1),
                    },
                    new PartTransform {
                        Rotation = Quaternion.RotationAxis(Vector3.UnitX, -0.02f),
                        Scaling = new Vector3(1, 1, 1),
                    },
                    new PartTransform {
                        Rotation = Quaternion.RotationAxis(Vector3.UnitY, -0.3f),
                        Scaling = new Vector3(1, 1, 1),
                    },
                    new PartTransform {
                        Rotation = Quaternion.RotationAxis(Vector3.UnitY, 0.5f),
                        Scaling = new Vector3(1, 1, 1),
                    },
                    new PartTransform {
                        Rotation = Quaternion.RotationAxis(Vector3.UnitY, 0.5f),
                        Scaling = new Vector3(1, 1, 1),
                    },
                    new PartTransform {
                        Rotation = Quaternion.RotationAxis(Vector3.UnitY, -0.5f),
                        Scaling = new Vector3(1, 1, 1),
                    },
                },
                TransformsEnd = new[] { 
                    new PartTransform {
                        Rotation = Quaternion.Identity,
                        Scaling = new Vector3(1, 1, 1),
                    },
                    new PartTransform {
                        Rotation = Quaternion.RotationAxis(Vector3.UnitX, 0.02f),
                        Scaling = new Vector3(1, 1, 1),
                    },
                    new PartTransform {
                        Rotation = Quaternion.RotationAxis(Vector3.UnitY, 0.5f),
                        Scaling = new Vector3(1, 1, 1),
                    },
                    new PartTransform {
                        Rotation = Quaternion.RotationAxis(Vector3.UnitY, -0.3f),
                        Scaling = new Vector3(1, 1, 1),
                    },
                    new PartTransform {
                        Rotation = Quaternion.RotationAxis(Vector3.UnitY, -0.5f),
                        Scaling = new Vector3(1, 1, 1),
                    },
                    new PartTransform {
                        Rotation = Quaternion.RotationAxis(Vector3.UnitY, 0.5f),
                        Scaling = new Vector3(1, 1, 1),
                    },
                },
                InterpRotation = new[] {
                    InterpolationMethod.Linear,
                    InterpolationMethod.Linear,
                    InterpolationMethod.Linear,
                    InterpolationMethod.Linear,
                    InterpolationMethod.Linear,
                    InterpolationMethod.Linear,
                },
                InterpScaling = new[] {
                    InterpolationMethod.Linear,
                    InterpolationMethod.Linear,
                    InterpolationMethod.Linear,
                    InterpolationMethod.Linear,
                    InterpolationMethod.Linear,
                    InterpolationMethod.Linear,
                },
                InterpolateRotateEndPoint = new[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f },
            };
        }

        public static Segment CreateSegment_()
        {
            return new Segment
            {
                Length = 120.0f,
                Loop = SegmentLoopMethod.Loop,
                TransformsStart = new[] { 
                    new PartTransform {
                        Rotation = SharpDX.Quaternion.Identity,
                        Scaling = new SharpDX.Vector3(1, 1, 1),
                    },
                },
                TransformsEnd = new[] { 
                    new PartTransform {
                        Rotation = SharpDX.Quaternion.RotationAxis(SharpDX.Vector3.UnitZ, 3.14f / 2),
                        Scaling = new SharpDX.Vector3(1, 1, 1),
                    },
                },
                InterpRotation = new[] {
                    InterpolationMethod.Linear
                },
                InterpScaling = new[] {
                    InterpolationMethod.Linear
                },
                InterpolateRotateEndPoint = new[] { 0.25f },
            };
        }
    }
}
