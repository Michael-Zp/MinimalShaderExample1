﻿using OpenTK.Graphics.OpenGL4;
using System;
using System.Numerics;
using Zenseless.Geometry;
using Zenseless.HLGL;
using Zenseless.OpenGL;

namespace Example
{
    public class MainVisual
    {
        public MainVisual(IRenderState renderState, IContentLoader contentLoader)
        {
            CameraDistance = 10.0f;
            renderState.Set(BoolState<IDepthState>.Enabled);
            renderState.Set(BoolState<IBackfaceCullingState>.Enabled);
            shaderProgram = contentLoader.Load<IShaderProgram>("shader.*");
            var mesh = contentLoader.Load<DefaultMesh>("suzanne");

            geometry = VAOLoader.FromMesh(mesh, shaderProgram);

            //per instance attributes
            var rnd = new Random(12);
            float Rnd01() => (float)rnd.NextDouble();
            float RndCoord() => (Rnd01() - 0.5f) * 35.0f;
            var instancePositions = new Vector3[particelCount];
            for (int i = 0; i < particelCount; ++i)
            {
                instancePositions[i] = new Vector3(RndCoord(), RndCoord(), RndCoord());
            }
            geometry.SetAttribute(shaderProgram.GetResourceLocation(ShaderResourceType.Attribute, "instancePosition"), instancePositions, VertexAttribPointerType.Float, 3, true);
        }

        public float CameraDistance { get; set; }
        public float CameraAzimuth { get; set; }
        public float CameraElevation { get; set; }

        public void Render()
        {
            if (shaderProgram is null) return;
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            shaderProgram.Activate();
            shaderProgram.Uniform("camera", camera, true);
            geometry.Draw(particelCount);
            shaderProgram.Deactivate();
        }

        public void Update(float updatePeriod)
        {
            //TODO student: use CameraDistance, CameraAzimuth, CameraElevation
            var distance = Matrix4x4.CreateTranslation(0.0f, 0.0f, -CameraDistance);
            var azimuth = Matrix4x4.CreateRotationY(CameraAzimuth);
            var elevation = Matrix4x4.CreateRotationX(CameraElevation);
            var p = Matrix4x4.CreatePerspectiveFieldOfView(0.5f, 1.0f, 0.1f, 1000.0f);
            camera = azimuth * elevation * distance * p;
        }

        private const int particelCount = 500;

        private IShaderProgram shaderProgram;
        private Matrix4x4 camera = Matrix4x4.Identity;
        private VAO geometry;
    }
}
