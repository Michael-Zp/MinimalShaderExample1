namespace Example
{
    using System.Numerics;

    public interface IBody
    {
        Vector3 Location { get; }
        Vector3 Scale { get; }
        float Mass { get; }
    }
}
