using MagicaVoxelAnimation.Models.Animation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MagicaVoxelAnimation
{
    partial class FormEditRotation : Form
    {
        private Frame _Frame;
        private const int ScrollMax = 100;

        public FormEditRotation(Frame frame)
        {
            InitializeComponent();

            _Frame = frame;
            hScrollBar1.Minimum = -ScrollMax;
            hScrollBar2.Minimum = -ScrollMax;
            hScrollBar3.Minimum = -ScrollMax;
            hScrollBar4.Minimum = -ScrollMax;
            hScrollBar1.Maximum = ScrollMax;
            hScrollBar2.Maximum = ScrollMax;
            hScrollBar3.Maximum = ScrollMax;
            hScrollBar4.Maximum = ScrollMax;

            for (int i = 0; i < frame.Transforms.Length; ++i)
            {
                listBox1.Items.Add(i);
            }
        }

        private void Recalc()
        {
            var m = (float)ScrollMax;
            SetTransform(hScrollBar1.Value / m,
                hScrollBar2.Value / m,
                hScrollBar3.Value / m,
                hScrollBar4.Value / m);
        }

        private void SetTransform(float a, float b, float c, float d)
        {
            if (listBox1.SelectedIndex == -1)
            {
                return;
            }
            lock (_Frame)
            {
                var q = new SharpDX.Quaternion(a, b, c, d);
                q.Normalize();
                _Frame.Transforms[listBox1.SelectedIndex].Rotation = q;
            }
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            Recalc();
        }

        private void hScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {
            Recalc();
        }

        private void hScrollBar3_Scroll(object sender, ScrollEventArgs e)
        {
            Recalc();
        }

        private void hScrollBar4_Scroll(object sender, ScrollEventArgs e)
        {
            Recalc();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                var q = _Frame.Transforms[listBox1.SelectedIndex].Rotation;
                hScrollBar1.Value = (int)(ScrollMax * q.X);
                hScrollBar2.Value = (int)(ScrollMax * q.Y);
                hScrollBar3.Value = (int)(ScrollMax * q.Z);
                hScrollBar4.Value = (int)(ScrollMax * q.W);
            }
        }
    }
}
