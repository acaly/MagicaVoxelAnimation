using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MagicaVoxelAnimation.Render
{
    class Camera
    {
        //from entity
        public Vector3 Position, Velocity, Acceloration;

        public float yaw = -2.34f, pitch = 0.85f;
        private Vector3 eye_offset;
        private Dictionary<Keys, bool> keys;

        public Camera(Vector3 pos)
        {
            this.Position = pos;
            this.Acceloration = new Vector3(0, 0, 0);
            this.eye_offset = new Vector3(0.0f, 0.0f, 0.75f);
        }

        private Vector3 CalcOffset()
        {
            Vector4 offset = new Vector4(-1, 0, 0, 0);
            Vector4 x = new Vector4(0, 1, 0, 0);
            offset = Vector4.Transform(offset, Matrix.RotationZ(yaw));
            x = Vector4.Transform(x, Matrix.RotationZ(yaw));
            pitch = Math.Min((float)Math.PI / 2.001f, pitch);
            pitch = Math.Max((float)Math.PI / -2.001f, pitch);
            offset = Vector4.Transform(offset, Matrix.RotationAxis(new Vector3(x.X, x.Y, x.Z), -pitch));
            return new Vector3(offset.X, offset.Y, offset.Z);
        }

        public Matrix GetViewMatrix()
        {
            return Matrix.LookAtLH(Position + eye_offset, Position + eye_offset + CalcOffset(), Vector3.UnitZ);
        }

        public Vector3 MoveHorizontal(Vector4 b)
        {
            Vector4 move = b;
            move = Vector4.Transform(move, Matrix.RotationZ(yaw));
            return new Vector3(move.X, move.Y, move.Z);
        }

        public Vector3 MoveHorizontal(float x, float y)
        {
            return MoveHorizontal(new Vector4(x, y, 0, 0));
        }

        public void SetForm(Form form)
        {
            keys = new Dictionary<Keys, bool>() {
                        { Keys.W, false }, { Keys.S, false }, { Keys.A, false }, { Keys.D, false },
                        { Keys.Up, false }, { Keys.Down, false }, { Keys.Left, false }, { Keys.Right, false },
                        { Keys.Q, false }, { Keys.E, false },
                        { Keys.Space, false }, { Keys.Z, false },
                    };
            //TODO sync!
            form.KeyDown += delegate(object obj, KeyEventArgs e)
            {
                if (keys.ContainsKey(e.KeyCode)) keys[e.KeyCode] = true;
            };
            form.KeyUp += delegate(object obj, KeyEventArgs e)
            {
                if (keys.ContainsKey(e.KeyCode)) keys[e.KeyCode] = false;
            };
        }

        public void Step()
        {
            Vector3 acc;
            Vector4 movedir = new Vector4();
            if (keys[Keys.W]) movedir.X -= 1;
            if (keys[Keys.S]) movedir.X += 1;
            if (keys[Keys.A]) movedir.Y += 1;
            if (keys[Keys.D]) movedir.Y -= 1;
            movedir.Normalize();
            acc = MoveHorizontal(movedir) * 30.0f;
            acc.Z = Acceloration.Z;

            if (keys[Keys.Up]) pitch -= 0.03f;
            if (keys[Keys.Down]) pitch += 0.03f;
            if (keys[Keys.Left]) yaw -= 0.03f;
            if (keys[Keys.Right]) yaw += 0.03f;

            if (keys[Keys.Space]) Velocity.Z += 1;
            if (keys[Keys.Z]) Velocity.Z -= 1;
            Acceloration = acc;

            if (keys[Keys.Space])
            {
                Velocity.Z += 1;
            }

            //Acceloration.Z -= 0.00001f;
            Velocity = Velocity * 0.9f + Acceloration / 60;
            Position += Velocity / 60;
        }
    }
}
