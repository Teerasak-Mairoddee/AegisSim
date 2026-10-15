using System;
using AegisSim.Maths;

namespace AegisSim.Simulation
{
    /// <summary>
    /// Point-mass kinematic model with bounded acceleration (lesson 002), in 3D.
    /// Velocity is updated first, then position uses the new velocity.
    /// </summary>
    public static class VehicleDynamics
    {
        public static VehicleState Step(VehicleState state, Vec3 requestedVelocity, double maxAcceleration, double dt)
        {
            if (dt <= 0)
                throw new ArgumentOutOfRangeException(nameof(dt), dt, "dt must be greater than 0.");
            if (maxAcceleration < 0)
                throw new ArgumentOutOfRangeException(nameof(maxAcceleration), maxAcceleration, "maxAcceleration cannot be negative.");

            Vec3 neededAcceleration = (requestedVelocity - state.Velocity) / dt;   // close the gap in one step
            Vec3 acceleration = neededAcceleration.ClampMagnitude(maxAcceleration);   // cap the arrow's length at a_max

            Vec3 newVelocity = state.Velocity + acceleration * dt;
            Vec3 newPosition = state.Position + newVelocity * dt;
            return new VehicleState(newPosition, newVelocity);
        }
    }
}
