using SharpDX;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace MagicaVoxelAnimation.Models.Animation
{
    static class ModelLoader
    {
        public static Model CreateFromVoxelModels(Voxel.Model[] models, Vector3[] basePoint, Vector3[] translation, int[] parent)
        {
            var model = new Model();
            model.Palette = models[0].Palette;
            model.SizeX = models[0].SizeX;
            model.SizeY = models[0].SizeY;
            model.SizeZ = models[0].SizeZ;
            model.Parts = new Model.Part[models.Length];
            for (int i = 0; i < models.Length; ++i)
            {
                var v = models[i].Voxel;
                model.Parts[i] = new Model.Part
                {
                    ParentPart = parent[i],
                    BasePoint = basePoint[i],
                    Translation = translation[i],
                    Voxel = v,
                    Transform = Matrix.Identity,
                };
            }
            return model;
        }

        private static BinaryFormatter _Formatter = new BinaryFormatter();

        public static void Save(Model model, string filename)
        {
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            using (var file = File.Open(filename, FileMode.CreateNew))
            {
                _Formatter.Serialize(file, new SerializableModel(model));
            }
        }

        public static Model LoadModel(string filename)
        {
            using (var file = File.OpenRead(filename))
            {
                return ((SerializableModel)_Formatter.Deserialize(file)).ToModel();
            }
        }
        public static void Save(Segment model, string filename)
        {
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            using (var file = File.Open(filename, FileMode.CreateNew))
            {
                _Formatter.Serialize(file, new SerializableSegment(model));
            }
        }

        public static Segment LoadSegment(string filename)
        {
            using (var file = File.OpenRead(filename))
            {
                return ((SerializableSegment)_Formatter.Deserialize(file)).ToSegment();
            }
        }

        //fxxx SharpDX
        [Serializable]
        public class SerializableModel
        {
            public System.Drawing.Color[] Palette;
            public SerializablePart[] Parts;
            public int SizeX, SizeY, SizeZ;

            public Model ToModel()
            {
                return new Model
                {
                    Palette = Palette,
                    SizeX = SizeX,
                    SizeY = SizeY,
                    SizeZ = SizeZ,
                    Parts = Parts.Select(p => p.ToPart()).ToArray(),
                };
            }

            public SerializableModel(Model model)
            {
                Palette = model.Palette;
                Parts = model.Parts.Select(p => new SerializablePart(p)).ToArray();
                SizeX = model.SizeX;
                SizeY = model.SizeY;
                SizeZ = model.SizeZ;
            }
        }

        //fxxx SharpDX
        [Serializable]
        public class SerializablePart
        {
            public int ParentPart;
            public float[] BasePoint;
            public float[] Translation;
            public VoxelData[] Voxel;

            public Model.Part ToPart()
            {
                return new Model.Part
                {
                    ParentPart = ParentPart,
                    BasePoint = new Vector3(BasePoint),
                    Translation = new Vector3(Translation),
                    Voxel = Voxel,
                };
            }

            public SerializablePart(Model.Part p)
            {
                ParentPart = p.ParentPart;
                BasePoint = p.BasePoint.ToArray();
                Translation = p.Translation.ToArray();
                Voxel = p.Voxel;
            }
        }

        //fxxx SharpDX
        [Serializable]
        public struct SerializablePartTransform
        {
            public float[] Scaling;
            public float[] Rotation;

            public PartTransform ToPartTransform()
            {
                return new PartTransform
                {
                    Rotation = new Quaternion(Rotation),
                    Scaling = new Vector3(Scaling),
                };
            }

            public SerializablePartTransform(PartTransform pt)
            {
                Rotation = pt.Rotation.ToArray();
                Scaling = pt.Scaling.ToArray();
            }
        }

        //fxxx SharpDX
        [Serializable]
        class SerializableSegment
        {
            public SerializablePartTransform[] TransformsStart;
            public SerializablePartTransform[] TransformsEnd;
            public string[] InterpRotation;
            public string[] InterpScaling;
            public float[] InterpolateRotateEndPoint;
            public SegmentLoopMethod Loop;
            public float Length;

            public Segment ToSegment()
            {
                return new Segment
                {
                    TransformsStart = TransformsStart.Select(t => t.ToPartTransform()).ToArray(),
                    TransformsEnd = TransformsEnd.Select(t => t.ToPartTransform()).ToArray(),
                    InterpRotation = InterpRotation.Select(i => InterpolationMethod.FromString(i)).ToArray(),
                    InterpScaling = InterpScaling.Select(i => InterpolationMethod.FromString(i)).ToArray(),
                    InterpolateRotateEndPoint = InterpolateRotateEndPoint,
                    Loop = Loop,
                    Length = Length,
                };
            }

            public SerializableSegment(Segment s)
            {
                TransformsStart = s.TransformsStart.Select(t => new SerializablePartTransform(t)).ToArray();
                TransformsEnd = s.TransformsEnd.Select(t => new SerializablePartTransform(t)).ToArray();
                InterpRotation = s.InterpRotation.Select(i => i.ToString()).ToArray();
                InterpScaling = s.InterpScaling.Select(i => i.ToString()).ToArray();
                InterpolateRotateEndPoint = s.InterpolateRotateEndPoint;
                Loop = s.Loop;
                Length = s.Length;
            }
        }
    }
}
