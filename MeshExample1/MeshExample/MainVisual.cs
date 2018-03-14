﻿using OpenTK.Graphics.OpenGL4;
using Zenseless.Geometry;
using Zenseless.HLGL;
using Zenseless.OpenGL;

namespace Example
{
    public class MainVisual
    {
        public MainVisual(IRenderState renderState, IContentLoader contentLoader)
        {
            //renderState.Set(BoolState<IDepthState>.Enabled);
            renderState.Set(BoolState<IBackfaceCullingState>.Enabled);
            //texDiffuse = TextureLoader.FromBitmap(Resourcen.capsule0);
            shaderProgram = contentLoader.Load<IShaderProgram>("shader.*");
            //load geometry
            var mesh = contentLoader.Load<DefaultMesh>("cat.obj");
            geometry = VAOLoader.FromMesh(mesh, shaderProgram);
        }

        public void Render()
        {
            if (shaderProgram is null) return;
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            shaderProgram.Activate();
            //texDiffuse.Activate();
            geometry.Draw();
            //texDiffuse.Deactivate();
            shaderProgram.Deactivate();
        }

        private IShaderProgram shaderProgram;
        private VAO geometry;
        //private Texture texDiffuse;
    }
}
