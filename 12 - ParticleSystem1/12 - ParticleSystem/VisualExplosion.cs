using OpenTK.Graphics.OpenGL4;
using System;
using System.Numerics;
using Zenseless.HLGL;
using Zenseless.OpenGL;

namespace Example
{
    public class VisualExplosion
    {
        public VisualExplosion(Vector3 emitterPos, IRenderState renderState, IContentLoader contentLoader)
        {
            this.renderState = renderState;
            this.emitterPos = emitterPos;
            texStar = contentLoader.Load<ITexture2D>("water_splash");
            shaderExplosion = contentLoader.Load<IShaderProgram>("explosion.*");
            particleSystem.ReleaseInterval = 0.001f;
            particleSystem.OnParticleCreate += Create;
            particleSystem.OnAfterParticleUpdate += OnAfterParticleUpdate;
        }

        public Particle Create(float creationTime)
        {
            var p = new Particle(creationTime, 2f);
            float Rnd01() => (float)random.NextDouble();
            float RndCoord() => (Rnd01() - 0.5f) * 2.0f;
            //around emitter position
            Vector3 startVector = Vector3.Normalize(new Vector3(RndCoord(), RndCoord(), RndCoord()));
            p.Position = emitterPos + startVector * .1f;
            //start speed
            p.Velocity = startVector;
            //downward gravity
            p.Acceleration = new Vector3(0, -.5f, 0);
            return p;
        }

        private void OnAfterParticleUpdate(Particle particle)
        {
            //if collision with ground plane
            if (particle.Position.Y < 0)
            {
                particle.Position = new Vector3(particle.Position.X, 0, particle.Position.Z);

                //Move particle on floor and simulate drag while rolling on the floor
                particle.Velocity = new Vector3(particle.Position.X, 0, particle.Position.Z) * particle.Velocity.Length();
                particle.Acceleration = -particle.Velocity / 5.0f;
            }
        }

        public void Update(float time)
        {
            if (shaderExplosion is null) return;
            particleSystem.Update(time);
            //gather all active particle positions into array
            var positions = new Vector3[particleSystem.ParticleCount];
            var fadeToRed = new float[particleSystem.ParticleCount];
            int i = 0;
            foreach (var particle in particleSystem.Particles)
            {
                //fading with age effect
                var age = time - particle.CreationTime;
                fadeToRed[i] = 1f - age / particle.LifeTime;
                positions[i] = particle.Position;
                ++i;
            }

            particles.SetAttribute(shaderExplosion.GetResourceLocation(ShaderResourceType.Attribute, "position"), positions, VertexAttribPointerType.Float, 3);
            particles.SetAttribute(shaderExplosion.GetResourceLocation(ShaderResourceType.Attribute, "fadeToRed"), fadeToRed, VertexAttribPointerType.Float, 1);
        }

        public void Render(in Matrix4x4 camera)
        {
            if (shaderExplosion is null) return;
            renderState.Set(BlendStates.Additive);
            GL.DepthMask(false);
            renderState.Set(BoolState<IPointSpriteState>.Enabled);
            renderState.Set(BoolState<IShaderPointSizeState>.Enabled);

            shaderExplosion.Activate();
            shaderExplosion.Uniform("camera", camera);
            shaderExplosion.Uniform("pointSize", 0.5f);
            //shader.Uniform("texParticle", 0);
            texStar.Activate();
            particles.Draw();
            texStar.Deactivate();
            shaderExplosion.Deactivate();

            renderState.Set(BoolState<IShaderPointSizeState>.Disabled);
            renderState.Set(BoolState<IPointSpriteState>.Disabled);
            renderState.Set(BlendStates.Opaque);
            GL.DepthMask(true);
        }

        private IShaderProgram shaderExplosion;
        private ITexture texStar;
        private VAO particles = new VAO(PrimitiveType.Points);
        private ParticleSystem<Particle> particleSystem = new ParticleSystem<Particle>(400);
        private Random random = new Random();
        private readonly IRenderState renderState;
        private readonly Vector3 emitterPos;
    }
}
