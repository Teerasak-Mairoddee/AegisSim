namespace AegisSim.Simulation
{
    /// <summary>
    /// The true state of one vehicle at one moment (1D for now).
    /// Position in metres, velocity in metres per second.
    /// </summary>
    public readonly struct VehicleState
    {
        public double Position { get; }
        public double Velocity { get; }

        public VehicleState(double position, double velocity)
        {
            Position = position;
            Velocity = velocity;
        }
    }
}