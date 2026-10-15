using AegisSim.Maths;

namespace AegisSim.Simulation
{
    /// <summary>
    /// The true state of one vehicle at one moment.
    /// Position in metres, velocity in metres per second (Unity axes: Y is up).
    /// </summary>
    public readonly struct VehicleState
    {
        public Vec3 Position { get; }
        public Vec3 Velocity { get; }

        public VehicleState(Vec3 position, Vec3 velocity)
        {
            Position = position;
            Velocity = velocity;
        }
    }
}
