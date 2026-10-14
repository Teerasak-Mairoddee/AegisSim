using System;

namespace AegisSim.Simulation
{
    /// <summary>
    /// Point-mass kinematic model with bounded acceleration (lesson 002).
    /// Velocity is updated first, then position uses the new velocity.
    /// </summary>
    public static class VehicleDynamics
    {
        public static VehicleState Step(VehicleState state, double requestedVelocity, double maxAcceleration, double dt)
        {
            if (dt <= 0)
                throw new ArgumentOutOfRangeException(nameof(dt), dt, "dt must be greater than 0.");
            if (maxAcceleration < 0)
                throw new ArgumentOutOfRangeException(nameof(maxAcceleration), maxAcceleration, "maxAcceleration cannot be negative.");

            double neededAcceleration = (requestedVelocity - state.Velocity) / dt;   // close the gap in one step
            double acceleration = Math.Clamp(neededAcceleration, -maxAcceleration, maxAcceleration);   // cap at ±a_max

            double newVelocity = state.Velocity + acceleration * dt;
            double newPosition = state.Position + newVelocity * dt;
            return new VehicleState(newPosition, newVelocity);
        }
    }
}