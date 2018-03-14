using Zenseless.ExampleFramework;
using Zenseless.HLGL;
using Zenseless.OpenGL;
using OpenTK.Graphics.OpenGL4;
using System;
using System.IO;

namespace Example
{
    class MyVisual
    {
        private MyVisual()
        {
            string sVertexShader = LoadShader("vertex.glsl");
            string sFragmentShd = LoadShader("fragment.glsl");
            //read shader from file

            shaderProgram = ShaderLoader.FromStrings(sVertexShader, sFragmentShd);
        }

        private string LoadShader(string fileName)
        {
            string shader = "";
            string path = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory())) + @"\ShaderFiles";
            try
            {
                using (StreamReader sr = new StreamReader(Path.Combine(path, fileName)))
                {
                    shader = sr.ReadToEnd();
                    sr.Dispose();
                }
            }
            catch { };
            return shader;
        }

    private IShaderProgram shaderProgram;

    private void Render()
    {
        if (shaderProgram is null) return;
        GL.Clear(ClearBufferMask.ColorBufferBit);
        shaderProgram.Activate();
        GL.DrawArrays(PrimitiveType.Quads, 0, 4);
        shaderProgram.Deactivate();
    }

    [STAThread]
    private static void Main()
    {
        var window = new ExampleWindow();
        var visual = new MyVisual();
        window.Render += visual.Render;
        window.Run();
    }
}
}