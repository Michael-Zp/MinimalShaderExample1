namespace Example
{
    using System;
    using System.Numerics;

    /// <summary>
    /// Adapted from http://natureofcode.com/book/chapter-2-forces/
    /// </summary>
    public class Body : IBody
    {
        public const float G = .1f; //physically correct would be 6.67408e-11f;

        public float DragPassthroughFactor { get; set; }

        public Vector3 Acceleration { get; set; }
        public Vector3 Location { get; set; }
        public float Mass { get; set; }
        public Vector3 Velocity { get; set; }
        public Vector3 Scale { get; set; }
        public float MinX { get { return Location.X - Scale.X; } }
        public float MaxX { get { return Location.X + Scale.X; } }
        public float MinY { get { return Location.Y - Scale.Y; } }
        public float MaxY { get { return Location.Y + Scale.Y; } }
        public float MinZ { get { return Location.Z - Scale.Z; } }
        public float MaxZ { get { return Location.Z + Scale.Z; } }


        public Body()
        {
            Acceleration = Vector3.Zero;
            Location = Vector3.Zero;
            Mass = 1;
            Velocity = Vector3.Zero;
        }

        public Body(Vector3 location, Vector3 scale, float mass, float dragPassthroughFactor = 0.0f)
        {
            Acceleration = Vector3.Zero;
            Location = location;
            Scale = scale;
            Mass = mass;
            DragPassthroughFactor = dragPassthroughFactor;
            Velocity = Vector3.Zero;
        }

        public void ApplyForce(Vector3 force)
        {
            //Newtons 2nd Law: Force = Mass * Acceleration; 
            //but also consider force accumulation:
            //acceleration equals the sum of all forces / Mass
            Acceleration += force / Mass;
        }

        public void ApplyDrag(Body water)
        {
            if (!Intercepts(water, this))
                return;

            float dragMagnitude = water.DragPassthroughFactor * Velocity.LengthSquared();

            ApplyFriction(dragMagnitude);
        }

        public void ApplyFriction(float rate)
        {
            Velocity *= (1 - rate);
        }

        public void ApplyGravity(Body otherBody)
        {
            Vector3 direction = Vector3.Normalize(otherBody.Location - Location);

            float distance = Vector3.Distance(otherBody.Location, Location);
            float force = 0;
            if(distance > 10)
            {
                force = (G * (otherBody.Mass * Mass)) / (float)Math.Pow(distance, 2);
            }

            ApplyForce(direction * force);
        }

        public void Update(Body otherCollider)
        {
            Velocity += Acceleration;

            if(Intercepts(this, otherCollider))
            {
                Velocity -= Acceleration;
                Velocity *= -1;
            }

            //Newtons 1st law
            Location += Velocity;
            //force was spend reset Acceleration
            Acceleration = Vector3.Zero;
        }

        public bool Intercepts(Body one, Body two)
        {
            return ((one.MaxX >= two.MinX && one.MinX <= two.MaxX) &&
                    (one.MaxY >= two.MinY && one.MinY <= two.MaxY) &&
                    (one.MaxZ >= two.MinZ && one.MinZ <= two.MaxZ));
        }
    }
}
