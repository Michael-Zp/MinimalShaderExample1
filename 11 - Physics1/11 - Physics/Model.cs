namespace Example
{
    using System;
    using System.Collections.Generic;
    using System.Numerics;

    public class Model
    {
        public IEnumerable<IBody> Bodies { get { return bodies; } }

        private List<Body> bodies = new List<Body>();
        private Body floor;
        private Body water;

        public Model()
        {
            water = new Body(new Vector3(-50, -50, 0), new Vector3(50f), 0, 0.05f);

            floor = new Body(new Vector3(0, -30, 0), new Vector3(100, 0.5f, 100), 0);

            //for each body - setup body position and mass
            bodies.Add(new Body(new Vector3(0, 18, 10), new Vector3((float)Math.Pow(50, 0.33f)), 50));
            bodies.Add(new Body(new Vector3(14, 0, -5), new Vector3((float)Math.Pow(50, 0.33f)), 50));
            bodies.Add(new Body(new Vector3(0, -20, 0), new Vector3((float)Math.Pow(5, 0.33f)), 5));
            bodies.Add(new Body(new Vector3(-20, 0, 5), new Vector3((float)Math.Pow(5, 0.33f)), 5));
            bodies.Add(new Body(new Vector3(11, -20, 0), new Vector3((float)Math.Pow(5, 0.33f)), 5));
            bodies.Add(new Body(new Vector3(-20, 17, 5), new Vector3((float)Math.Pow(5, 0.33f)), 5));
        }

        public void Update(float updatePeriod)
        {
            foreach (var body in bodies)
            {
                body.ApplyForce(Vector3.UnitY * -.00f);
                body.ApplyFriction(.00f);
                body.ApplyDrag(water);

                foreach(var otherBody in bodies)
                {
                    if (otherBody == body)
                        continue;

                    body.ApplyGravity(otherBody);
                }

                body.Update(floor);
            }
        }

    }
}
