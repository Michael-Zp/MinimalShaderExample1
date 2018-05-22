namespace Example
{
    using OpenTK.Graphics.OpenGL4;
    using System.Collections.Generic;
    using Zenseless.Geometry;
    using Zenseless.HLGL;
    using Zenseless.OpenGL;

    public class MainVisual
    {
        public MainVisual(IRenderState renderState, IContentLoader contentLoader)
        {
            this.renderState = renderState;
            shaderProgram = contentLoader.Load<IShaderProgram>("lambert.*");
            //shaderPostProcess.Add(contentLoader.LoadPixelShader("swirl"));
            //shaderPostProcess.Add(contentLoader.LoadPixelShader("sepia"));
            //shaderPostProcess.Add(contentLoader.LoadPixelShader("vignet"));
            //shaderPostProcess.Add(contentLoader.LoadPixelShader("Ripple"));
            //shaderPostProcess.Add(contentLoader.LoadPixelShader("BadTV"));
            //shaderPostProcess.Add(contentLoader.LoadPixelShader("Blur"));
            shaderPostProcess.Add(contentLoader.LoadPixelShader("BloomGausPass1"));
            shaderPostProcess.Add(contentLoader.LoadPixelShader("BloomGausPass2"));

            var mesh = Meshes.CreateCornellBox(); //TODO: ATI seams to do VAO vertex attribute ordering different for each shader would need to create own VAO
            geometry = VAOLoader.FromMesh(mesh, shaderProgram);
        }

        private readonly IRenderState renderState;

        public void Draw(ITransformation camera)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            renderState.Set(new DepthTest(true));
            renderState.Set(new BackFaceCulling(true));
            shaderProgram.Activate();
            shaderProgram.Uniform("camera", camera);
            geometry.Draw();
            shaderProgram.Deactivate();
            renderState.Set(new BackFaceCulling(false));
            renderState.Set(new DepthTest(false));
        }

        public void DrawWithPostProcessing(float time, ITransformation camera)
        {
            renderToTexture.Activate(); //start drawing into texture
            Draw(camera);
            renderToTexture.Deactivate(); //stop drawing into texture


            renderToTexture.Activate();

            renderToTexture.Texture.Activate(); //use this new texture

            for(int i = 0; i < shaderPostProcess.Count - 1; i++)
            {
                shaderPostProcess[i].Activate(); //activate post processing shader
                shaderPostProcess[i].Uniform("iGlobalTime", time);
                GL.DrawArrays(PrimitiveType.Quads, 0, 4); //draw quad
                shaderPostProcess[i].Deactivate();
            }

            renderToTexture.Deactivate();

            shaderPostProcess[shaderPostProcess.Count - 1].Activate(); //activate post processing shader
            shaderPostProcess[shaderPostProcess.Count - 1].Uniform("iGlobalTime", time);
            GL.DrawArrays(PrimitiveType.Quads, 0, 4); //draw quad
            shaderPostProcess[shaderPostProcess.Count - 1].Deactivate();

            renderToTexture.Texture.Deactivate();
        }

        public void Resize(int width, int height)
        {
            renderToTexture = new FBOwithDepth(Texture2dGL.Create(width, height));
            renderToTexture.Texture.WrapFunction = TextureWrapFunction.Repeat;
        }

        private IRenderSurface renderToTexture;
        private List<IShaderProgram> shaderPostProcess = new List<IShaderProgram>();
        private IShaderProgram shaderProgram;
        private VAO geometry;
    }
}
