using OpenTK.Graphics.OpenGL;
using Zenseless.Geometry;
using Zenseless.HLGL;
using Zenseless.OpenGL;

namespace Example
{
    public class MainVisual
    {
        public MainVisual(IRenderState renderState, IContentLoader contentLoader)
        {
            renderState.Set(BoolState<IDepthState>.Enabled);
            renderState.Set(BoolState<IBackfaceCullingState>.Enabled);
            texDiffuse = contentLoader.Load<ITexture2D>("capsule0.jpg");
            shaderProgram = contentLoader.Load<IShaderProgram>("shader.*");
            //load geometry
            var mesh = contentLoader.Load<DefaultMesh>("capsule.obj");
            geometry = VAOLoader.FromMesh(mesh, shaderProgram);
        }

        public void Render()
        {
            if (shaderProgram is null) return;
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            shaderProgram.Activate();
            texDiffuse.Activate();
            GL.Rotate(.5f, .5f, .5f, .5f);
            geometry.Draw();
            texDiffuse.Deactivate();
            shaderProgram.Deactivate();
        }

        private IShaderProgram shaderProgram;
        private VAO geometry;
        private ITexture2D texDiffuse;
    }
}
