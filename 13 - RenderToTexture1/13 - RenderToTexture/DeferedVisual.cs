namespace Example
{
    using OpenTK.Graphics.OpenGL4;
    using Zenseless.Geometry;
    using Zenseless.HLGL;
    using Zenseless.OpenGL;

    public class DeferedVisual
    {
        public DeferedVisual(IRenderState renderState, IContentLoader contentLoader)
        {
            this.renderState = renderState;
            deferedDataProgram = contentLoader.Load<IShaderProgram>(new string[] { "lambert.vert", "deferedData.frag" });
            deferedProgram = contentLoader.LoadPixelShader("defered.frag");
            //deferedProgram = contentLoader.Load<IShaderProgram>("lambert.*");

            var mesh = Meshes.CreateCornellBox(); //TODO: ATI seams to do VAO vertex attribute ordering different for each shader would need to create own VAO
            geometry = VAOLoader.FromMesh(mesh, deferedDataProgram);

        }

        private readonly IRenderState renderState;

        public void Draw(ITransformation camera)
        {
            fbo.Activate();

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            renderState.Set(new DepthTest(true));
            renderState.Set(new BackFaceCulling(true));

            deferedDataProgram.Activate();
            deferedDataProgram.Uniform("camera", camera);

            GL.DrawBuffers(3, new DrawBuffersEnum[] { DrawBuffersEnum.ColorAttachment0, DrawBuffersEnum.ColorAttachment1, DrawBuffersEnum.ColorAttachment2 });

            geometry.Draw();

            deferedDataProgram.Deactivate();

            renderState.Set(new BackFaceCulling(false));
            renderState.Set(new DepthTest(false));

            fbo.Deactivate();


            string[] names = new string[] {
                "normalTex",
                "depthTex",
                "materialColorTex"
            };

            deferedProgram.Activate();

            for (int i = 0; i < fbo.Textures.Count; i++)
            {
                GL.ActiveTexture(TextureUnit.Texture0 + i);
                fbo.Textures[i].Activate();
                GL.Uniform1(deferedProgram.GetResourceLocation(ShaderResourceType.Uniform, names[i]), i);
            }


            deferedProgram.Uniform("camera", camera);

            GL.DrawArrays(PrimitiveType.Quads, 0, 4); //draw quad


            for (int i = 0; i < fbo.Textures.Count; i++)
            {
                GL.ActiveTexture(TextureUnit.Texture0 + i);
                fbo.Textures[i].Deactivate();
            }

            deferedProgram.Deactivate();
        }


        public void Resize(int width, int height)
        {
            fbo = new FBOwithDepth(Texture2dGL.Create(width, height));
            fbo.Attach(Texture2dGL.Create(width, height));
            fbo.Attach(Texture2dGL.Create(width, height));
            fbo.Texture.WrapFunction = TextureWrapFunction.MirroredRepeat;
        }

        private IRenderSurface fbo;
        private IShaderProgram deferedDataProgram;
        private IShaderProgram deferedProgram;
        private VAO geometry;
    }
}
