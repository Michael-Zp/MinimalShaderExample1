namespace Example
{
    using OpenTK.Graphics.OpenGL4;
    using System.Numerics;
    using Zenseless.Geometry;
    using Zenseless.HLGL;
    using Zenseless.OpenGL;

    public class MainVisual
    {
        private readonly Camera<Orbit, Perspective> light = new Camera<Orbit, Perspective>(new Orbit(8, -100, 44), new Perspective(farClip: 50));
        private IShaderProgram shaderProgram;
        private IShaderProgram shaderProgramDepth;
        private IRenderSurface fboShadowMap = new FBOwithDepth(Texture2dGL.Create(2048, 2048, 1, true));
        private IDrawable geometry;

        private float _width = 0;
        private float _height = 0;

        public MainVisual(IRenderState renderState, IContentLoader contentLoader)
        {
            renderState.Set(new DepthTest(true));
            fboShadowMap.Texture.Filter = TextureFilterMode.Nearest;

            shaderProgram = contentLoader.Load<IShaderProgram>("shadowMap.*");
            var mesh = Meshes.CreatePlane(10, 10, 10, 10);
            var sphere = Meshes.CreateSphere(0.5f, 2);
            sphere.SetConstantUV(new Vector2(0.5f, 0.5f));
            mesh.Add(sphere.Transform(Transformation.Translation(0, 2, -2)));
            mesh.Add(sphere.Transform(Transformation.Translation(0, 2, 0)));
            mesh.Add(sphere.Transform(Transformation.Translation(2, 2, -1)));
            geometry = VAOLoader.FromMesh(mesh, shaderProgram);

            shaderProgramDepth = contentLoader.Load<IShaderProgram>("depth.*");
            //todo: radeon cards created errors with geometry bound to one shader and used in other shaders because of binding id changes
        }

        public void Resize(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public Camera<Orbit, Perspective> Camera { get; } = new Camera<Orbit, Perspective>(new Orbit(8, 0, 30), new Perspective(90, 0.1f, 50f));

        public void Render()
        {
            if (shaderProgram is null) return;
            if (shaderProgramDepth is null) return;
            //first pass: create shadow map
            fboShadowMap.Activate();

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            shaderProgramDepth.Activate();
            shaderProgramDepth.Uniform("ambient", new Vector3(0.1f, 0.1f, 0f));
            shaderProgramDepth.Uniform("camera", light);
            geometry.Draw();
            shaderProgramDepth.Deactivate();

            fboShadowMap.Deactivate();


            //GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            //TextureDebugger.Draw(fboShadowMap.Texture);

            //second pass: render with shadow map
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            shaderProgram.Activate();
            fboShadowMap.Texture.Activate();
            shaderProgram.Uniform("ambient", new Vector3(0.1f, 0.1f, 0f));
            shaderProgram.Uniform("camera", Camera);
            shaderProgram.Uniform("lightCamera", light);
            geometry.Draw();
            fboShadowMap.Texture.Deactivate();
            shaderProgram.Deactivate();
        }

    }
}
