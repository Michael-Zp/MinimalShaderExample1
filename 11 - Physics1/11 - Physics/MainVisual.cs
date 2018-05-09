namespace Example
{
    using OpenTK.Graphics.OpenGL4;
    using System;
    using System.Collections.Generic;
    using System.Numerics;
    using Zenseless.Geometry;
    using Zenseless.HLGL;
    using Zenseless.OpenGL;

    public class MainVisual
    {
        private IShaderProgram shaderProgram;
        private VAO geometryBody;
        private IShaderProgram floorShaderProgram;
        private VAO floor;
        private VAO waterCube;

        public MainVisual(IRenderState renderState, IContentLoader contentLoader)
        {
            renderState.Set(BoolState<IDepthState>.Enabled);
            renderState.Set(BoolState<IBackfaceCullingState>.Enabled);
            shaderProgram = contentLoader.Load<IShaderProgram>("shader.*");
            var mesh = contentLoader.Load<DefaultMesh>("suzanne");
            geometryBody = VAOLoader.FromMesh(mesh, shaderProgram);

            floorShaderProgram = contentLoader.Load<IShaderProgram>("floor.*");
            
            floor = VAOLoader.FromMesh(Meshes.CreatePlane(100, 100, 1, 1).Transform(new Translation3D(new Vector3(0, -30, 0))), floorShaderProgram);
            waterCube = VAOLoader.FromMesh(Meshes.CreateCubeWithNormals(100).Transform(new Translation3D(new Vector3(-50, -50, 0))), floorShaderProgram);
        }

        public void Render(IEnumerable<IBody> bodies, float time, Transformation3D camera)
        {
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            if (shaderProgram is null) return;
            var instancePositions = new List<Vector3>();
            var instanceScale = new List<float>();
            foreach (var body in bodies)
            {
                instancePositions.Add(body.Location);
                instanceScale.Add((float)Math.Pow(body.Mass, 0.33f));
            }
            geometryBody.SetAttribute(shaderProgram.GetResourceLocation(ShaderResourceType.Attribute, "instancePosition"), instancePositions.ToArray(), VertexAttribPointerType.Float, 3, true);
            geometryBody.SetAttribute(shaderProgram.GetResourceLocation(ShaderResourceType.Attribute, "instanceScale"), instanceScale.ToArray(), VertexAttribPointerType.Float, 1, true);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            shaderProgram.Activate();
            shaderProgram.Uniform("time", time);
            shaderProgram.Uniform("camera", camera.CalcLocalToWorldColumnMajorMatrix());
            geometryBody.Draw(instancePositions.Count);
            shaderProgram.Deactivate();

            floorShaderProgram.Activate();
            floorShaderProgram.Uniform("time", time);
            floorShaderProgram.Uniform("floorColor", new Vector4(0, .5f, .5f, 1f));
            floorShaderProgram.Uniform("camera", camera.CalcLocalToWorldColumnMajorMatrix());
            floor.Draw();

            floorShaderProgram.Uniform("floorColor", new Vector4(0, 0, 1, 0.5f));
            waterCube.Draw();
            floorShaderProgram.Deactivate();
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.DepthTest);
        }
    }
}
