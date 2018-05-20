﻿using OpenTK.Graphics.OpenGL4;
using System.Numerics;
using Zenseless.Geometry;
using Zenseless.HLGL;

namespace Example
{
    public class MainVisual
    {
        public MainVisual(IRenderState renderState, IContentLoader contentLoader)
        {
            renderState.Set(BoolState<IDepthState>.Enabled);
            plane = new VisualPlane(renderState, contentLoader);

            visualSmoke = new VisualSmoke(Vector3.Zero, new Vector3(.2f, 0, 0), renderState, contentLoader);
            visualWaterfall = new VisualWaterfall(new Vector3(-.5f, 1, -.5f), renderState, contentLoader);
            visualExplosion = new VisualExplosion(new Vector3(0f, 1, 0f), renderState, contentLoader);
        }

        public void Update(float time)
        {
            visualSmoke.Update(time);
            visualWaterfall.Update(time);
            visualExplosion.Update(time);
        }

        public void Render(Transformation3D camera)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            var cam = camera.CalcLocalToWorldColumnMajorMatrix();
            plane.Draw(cam);
            visualSmoke.Render(cam);
            visualWaterfall.Render(cam);
            visualExplosion.Render(cam);
        }

        private VisualPlane plane;
        private readonly VisualSmoke visualSmoke;
        private readonly VisualWaterfall visualWaterfall;
        private readonly VisualExplosion visualExplosion;
    }
}
