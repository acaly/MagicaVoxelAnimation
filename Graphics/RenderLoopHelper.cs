using SharpDX.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MagicaVoxelAnimation.Graphics
{
    class RenderLoopHelper
    {
        private class ShutdownSignal
        {
            public volatile bool shutdown = false;
            public volatile bool fpsUpdate = false;
            public volatile float fps;
        }

        public static void Run(RenderManager rm, bool multithread, Action<RenderContext> action)
        {
            if (multithread)
            {
                var signal = new ShutdownSignal();
                var renderThread = new Thread(new ThreadStart(delegate()
                {
                    int counter = 0;
                    while (!signal.shutdown)
                    {
                        var frame = rm.NextFrame(true);
                        if (frame == null) break;

                        if (++counter >= 60)
                        {
                            signal.fps = frame.Fps;
                            signal.fpsUpdate = true;
                            counter = 0;
                        }

                        action(frame);
                    }
                }));

                rm.ShowWindow();
                renderThread.Start();
                RenderLoop.Run(rm.Form, delegate
                {
                    Thread.Sleep(5);
                    if (signal.fpsUpdate)
                    {
                        rm.Form.Text = "FPS: " + signal.fps;
                        signal.fpsUpdate = false;
                    }
                });
                signal.shutdown = true;
                renderThread.Join();
            }
            else
            {
                rm.ShowWindow();
                int counter = 0;
                while (true)
                {
                    var frame = rm.NextFrame(false);
                    if (frame == null) break;

                    if (++counter >= 60)
                    {
                        rm.Form.Text = "FPS: " + frame.Fps;
                        counter = 0;
                    }

                    action(frame);
                }
            }
        }
    }
}
