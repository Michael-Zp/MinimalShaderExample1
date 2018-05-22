namespace Example
{
    using OpenTK.Input;
    using System;
    using Zenseless.Base;
    using Zenseless.ExampleFramework;
    using Zenseless.Geometry;
    using Zenseless.OpenGL;

    class Controller
    {
        [STAThread]
        private static void Main()
        {
            var window = new ExampleWindow();

            var camera = window.GameWindow.CreateOrbitingCameraController(1.8f, 70, 0.1f, 50f);
            camera.View.TargetY = -0.3f;

            bool defered = true;

            if (defered)
            {
                DrawDeferedVisual(window, camera);
            }
            else
            {
                DrawMainVisual(window, camera);
            }
        }

        private static void DrawMainVisual(ExampleWindow window, ITransformation camera)
        {
            var visual = new MainVisual(window.RenderContext.RenderState, window.ContentLoader);

            var globalTime = new GameTime();
            bool doPostProcessing = false;

            window.Render += () =>
            {
                if (doPostProcessing)
                {
                    visual.DrawWithPostProcessing(globalTime.AbsoluteTime, camera);
                }
                else
                {
                    visual.Draw(camera);
                }
            };

            window.Update += (t) => doPostProcessing = !Keyboard.GetState()[Key.Space];
            window.Resize += visual.Resize;
            window.Run();
        }


        private static void DrawDeferedVisual(ExampleWindow window, ITransformation camera)
        {
            var visual = new DeferedVisual(window.RenderContext.RenderState, window.ContentLoader);

            var globalTime = new GameTime();

            window.Render += () =>
            {
                visual.Draw(camera);
            };
            
            window.Resize += visual.Resize;
            window.Run();
        }
    }
}