using OpenTK.Graphics.OpenGL4;
using System.Numerics;
using Zenseless.Geometry;
using Zenseless.HLGL;
using Zenseless.OpenGL;

namespace Example
{
    public class MainVisual
    {

        private IShaderProgram shaderProgram;
        private ITexture envMap;
        private ITexture sphereTex;
        private VAO geometry;
        private VAO geometryRef;
        private VAO geometryRefract;


        public MainVisual(IRenderState renderState, IContentLoader contentLoader)
        {
            renderState.Set(new ClearColorState(1, 1, 1, 1));
            renderState.Set(BoolState<IDepthState>.Enabled);
            renderState.Set(BoolState<IBackfaceCullingState>.Enabled);

            envMap = contentLoader.Load<ITexture2D>("beach");
            envMap.WrapFunction = TextureWrapFunction.MirroredRepeat;
            envMap.Filter = TextureFilterMode.Linear;

            sphereTex = contentLoader.Load<ITexture2D>("hatefield1");
            sphereTex.WrapFunction = TextureWrapFunction.MirroredRepeat;
            sphereTex.Filter = TextureFilterMode.Linear;

            shaderProgram = contentLoader.Load<IShaderProgram>("envMapping.*");

            var sphere = Meshes.CreateSphere(1, 4);
            var envSphere = sphere.SwitchTriangleMeshWinding();
            var refSphere = Meshes.CreateSphere(.1f, 4).Transform(new Translation3D(new Vector3(-.1f, 0, 0)));
            var refractSphere = Meshes.CreateSphere(.1f, 4).Transform(new Translation3D(new Vector3(.1f, 0, 0)));
            geometry = VAOLoader.FromMesh(envSphere, shaderProgram);
            geometry.SetAttribute(shaderProgram.GetResourceLocation(ShaderResourceType.Attribute, "specularity"), new[] { 0.0f }, VertexAttribPointerType.Float, 1, true);
            geometry.SetAttribute(shaderProgram.GetResourceLocation(ShaderResourceType.Attribute, "translucency"), new[] { 0.0f }, VertexAttribPointerType.Float, 1, true);
            geometry.SetAttribute(shaderProgram.GetResourceLocation(ShaderResourceType.Attribute, "refractionIndex"), new[] { 1.0f }, VertexAttribPointerType.Float, 1, true);

            geometryRef = VAOLoader.FromMesh(refSphere, shaderProgram);
            geometryRef.SetAttribute(shaderProgram.GetResourceLocation(ShaderResourceType.Attribute, "specularity"), new[] { .2f }, VertexAttribPointerType.Float, 1, true);
            geometryRef.SetAttribute(shaderProgram.GetResourceLocation(ShaderResourceType.Attribute, "translucency"), new[] { 0.0f }, VertexAttribPointerType.Float, 1, true);
            geometryRef.SetAttribute(shaderProgram.GetResourceLocation(ShaderResourceType.Attribute, "refractionIndex"), new[] { 1.0f }, VertexAttribPointerType.Float, 1, true);

            geometryRefract = VAOLoader.FromMesh(refractSphere, shaderProgram);
            geometryRefract.SetAttribute(shaderProgram.GetResourceLocation(ShaderResourceType.Attribute, "specularity"), new[] { .0f }, VertexAttribPointerType.Float, 1, true);
            geometryRefract.SetAttribute(shaderProgram.GetResourceLocation(ShaderResourceType.Attribute, "translucency"), new[] { 1.0f }, VertexAttribPointerType.Float, 1, true);
            geometryRefract.SetAttribute(shaderProgram.GetResourceLocation(ShaderResourceType.Attribute, "refractionIndex"), new[] { 1.6f }, VertexAttribPointerType.Float, 1, true);
        }

        public void Render(Transformation3D camera, Vector3 cameraPosition)
        {
            if (shaderProgram is null) return;
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            DrawObject(geometry, envMap, envMap, camera, cameraPosition);
            DrawObject(geometryRef, envMap, sphereTex, camera, cameraPosition);
            DrawObject(geometryRefract, envMap, sphereTex, camera, cameraPosition);

        }

        private void DrawObject(VAO geom, ITexture envMap, ITexture materialTex, Transformation3D camera, Vector3 cameraPosition)
        {
            shaderProgram.Activate();

            GL.ActiveTexture(TextureUnit.Texture0 + 0);
            envMap.Activate();
            shaderProgram.Uniform("envMap", 0);
            GL.ActiveTexture(TextureUnit.Texture0 + 1);
            materialTex.Activate();
            shaderProgram.Uniform("materialTex", 1);

            shaderProgram.Uniform("camera", camera.CalcLocalToWorldColumnMajorMatrix());
            shaderProgram.Uniform("cameraPosition", cameraPosition);

            geom.Draw();

            GL.ActiveTexture(TextureUnit.Texture0 + 0);
            envMap.Deactivate();
            GL.ActiveTexture(TextureUnit.Texture0 + 1);
            materialTex.Deactivate();

            shaderProgram.Deactivate();
        }
    }
}
