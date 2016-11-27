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
            var ret = ModelLoader.CreateFromVoxelModels(
                new[] {
                    Voxel.ModelLoader.Load(p + "body2.vox"),
                    Voxel.ModelLoader.Load(p + "head.vox"),
                    Voxel.ModelLoader.Load(p + "arm_l_u.vox"),
                    Voxel.ModelLoader.Load(p + "arm_r_u.vox"),
                    Voxel.ModelLoader.Load(p + "leg_l_u.vox"),
                    Voxel.ModelLoader.Load(p + "leg_r_u.vox"),
                    Voxel.ModelLoader.Load(p + "leg_l_d.vox"),
                    Voxel.ModelLoader.Load(p + "leg_r_d.vox"),
                    Voxel.ModelLoader.Load(p + "arm_l_d.vox"),
                    Voxel.ModelLoader.Load(p + "arm_r_d.vox"),
                },
                new[] {
                    new SharpDX.Vector3(20, 25, 0),
                    new SharpDX.Vector3(20, 24.5f, 28),
                    new SharpDX.Vector3(20.5f, 20, 25),
                    new SharpDX.Vector3(20.5f, 29, 25),
                    new SharpDX.Vector3(21, 23, 18),
                    new SharpDX.Vector3(21, 26, 18),
                    new SharpDX.Vector3(19.5f, 23, 15.5f),
                    new SharpDX.Vector3(19.5f, 26, 15.5f),
                    new SharpDX.Vector3(21.5f, 19.5f, 21.5f),
                    new SharpDX.Vector3(21.5f, 29.5f, 21.5f),
                },
                new[] {
                    new SharpDX.Vector3(0, 0, 0),
                    new SharpDX.Vector3(-0.004f, 0, 0),
                    new SharpDX.Vector3(0, 0.004f, 0),
                    new SharpDX.Vector3(0, -0.004f, 0),
                    new SharpDX.Vector3(-0.004f, 0.006f, 0),
                    new SharpDX.Vector3(-0.004f, -0.006f, 0),
                    new SharpDX.Vector3(-0.004f, 0.003f, 0.004f),
                    new SharpDX.Vector3(-0.004f, -0.003f, 0.004f),
                    new SharpDX.Vector3(0, 0.004f, 0),
                    new SharpDX.Vector3(0, -0.004f, 0),
                },
                new[] { -1, 0, 0, 0, 0, 0, 4, 5, 2, 3 }
            );
            ret.Parts[4].AdjacentParts = new[] { 6 };
            ret.Parts[6].AdjacentParts = new[] { 4 };
            ret.Parts[5].AdjacentParts = new[] { 7 };
            ret.Parts[7].AdjacentParts = new[] { 5 };
            ret.Parts[2].AdjacentParts = new[] { 8 };
            ret.Parts[8].AdjacentParts = new[] { 2 };
            ret.Parts[3].AdjacentParts = new[] { 9 };
            ret.Parts[9].AdjacentParts = new[] { 3 };
            return ret;
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
                    new PartTransform {
                        Rotation = Quaternion.RotationAxis(Vector3.UnitY, -1.0f),
                        Scaling = new Vector3(1, 1, 1),
                    },
                    new PartTransform {
                        Rotation = Quaternion.RotationAxis(Vector3.UnitY, -1.0f),
                        Scaling = new Vector3(1, 1, 1),
                    },
                    new PartTransform {
                        Rotation = Quaternion.RotationAxis(Vector3.UnitY, 1.0f),
                        Scaling = new Vector3(1, 1, 1),
                    },
                    new PartTransform {
                        Rotation = Quaternion.RotationAxis(Vector3.UnitY, 1.0f),
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
                    new PartTransform {
                        Rotation = Quaternion.Identity,
                        Scaling = new Vector3(1, 1, 1),
                    },
                    new PartTransform {
                        Rotation = Quaternion.Identity,
                        Scaling = new Vector3(1, 1, 1),
                    },
                    new PartTransform {
                        Rotation = Quaternion.Identity,
                        Scaling = new Vector3(1, 1, 1),
                    },
                    new PartTransform {
                        Rotation = Quaternion.Identity,
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
                    InterpolationMethod.Linear,
                    InterpolationMethod.Linear,
                    InterpolationMethod.Linear,
                    InterpolationMethod.Linear,
                },
                InterpolateRotateEndPoint = new[]
                {
                    1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f
                },
            };
        }
    }
}
