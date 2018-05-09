﻿using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using Zenseless.HLGL;
using Zenseless.OpenGL;

namespace Example
{
	class MainVisual
	{
		public MainVisual(IRenderState renderState, IContentLoader contentLoader)
		{
			renderState.Set(BlendStates.Additive);
			renderState.Set(BoolState<IShaderPointSizeState>.Enabled);
			renderState.Set(BoolState<IPointSpriteState>.Enabled);

			shaderProgram = contentLoader.Load<IShaderProgram>("shader.*");
			UpdateGeometry(shaderProgram);
		}

		public void Render(float time)
		{
			if (shaderProgram is null) return;
			GL.Clear(ClearBufferMask.ColorBufferBit);
			shaderProgram.Activate();
			////ATTENTION: always give the time as a float if the uniform in the shader is a float
			GL.Uniform1(shaderProgram.GetResourceLocation(ShaderResourceType.Uniform, "time"), time);
			geometry.Draw();
			shaderProgram.Deactivate();
		}

		private const int pointCount = 500;
		private IShaderProgram shaderProgram;
		private VAO geometry;

		private void UpdateGeometry(IShaderProgram shaderProgram)
		{
			geometry = new VAO(PrimitiveType.Points);
			//generate position array on CPU
			var rnd = new Random(12);
			float Rnd01() => (float)rnd.NextDouble();
			float RndCoord() => (Rnd01() - 0.5f) * 2.0f;
			var positions = new Vector2[pointCount];
			for (int i = 0; i < pointCount; ++i)
			{
				positions[i] = new Vector2(RndCoord(), RndCoord());
			}
			//copy positions to GPU
			geometry.SetAttribute(shaderProgram.GetResourceLocation(ShaderResourceType.Attribute, "in_position"), positions, VertexAttribPointerType.Float, 2);
			//generate velocity arrray on CPU
			float RndSpeed() => (Rnd01() - 0.5f) * 0.1f;
			var velocities = new Vector2[pointCount];
			for (int i = 0; i < pointCount; ++i)
			{
				velocities[i] = new Vector2(RndSpeed(), RndSpeed()) * 10.0f;
			}
			//copy velocities to GPU
			geometry.SetAttribute(shaderProgram.GetResourceLocation(ShaderResourceType.Attribute, "in_velocity"), velocities, VertexAttribPointerType.Float, 2);
		}
	}
}
