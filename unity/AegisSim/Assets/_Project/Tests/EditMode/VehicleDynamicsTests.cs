using NUnit.Framework;
using AegisSim.Simulation;

namespace AegisSim.Tests.EditMode
{
    public class VehicleDynamicsTests
    {
        // Trace B from lesson 002: start at rest, requested 5 m/s, a_max 10 m/s², dt 0.1 s.
        [Test]
        public void Step_FromRest_MatchesHandTrace()
        {
            var state = new VehicleState(0, 0);

            state = VehicleDynamics.Step(state, 5, 10, 0.1);   // step 1
            Assert.AreEqual(1, state.Velocity);
            Assert.AreEqual(0.1, state.Position);

            //step 2 (v = 2, p = 0.3)
            state = VehicleDynamics.Step(state, 5, 10, 0.1);
            Assert.AreEqual(2, state.Velocity);
            Assert.AreEqual(0.3, state.Position, 1e-9);

            //step 3 (v = 3, p = 0.6)
            state = VehicleDynamics.Step(state, 5, 10, 0.1);
            Assert.AreEqual(3, state.Velocity);
            Assert.AreEqual(0.6, state.Position, 1e-9);
        }

        // Braking with a limit: 8 m/s asked for 5 m/s, a_max 10 m/s², dt 0.1 s.
        [Test]
        public void Step_Braking_IsLimitedByMaxAcceleration()
        {
            var state = new VehicleState(0, 8);

            state = VehicleDynamics.Step(state, 5, 10, 0.1);   // step 1
            Assert.AreEqual(7, state.Velocity, 1e-9);

            state = VehicleDynamics.Step(state, 5, 10, 0.1);   // step 2
            Assert.AreEqual(6, state.Velocity, 1e-9);
        }

        // Holding speed: already at the requested 5 m/s (Trace B, steps 6 to 8).
        [Test]
        public void Step_AtRequestedSpeed_HoldsSpeed()
        {
            
            var state = new VehicleState(1.5, 5);

            state = VehicleDynamics.Step(state, 5, 10, 0.1);   // step 1
            Assert.AreEqual(5, state.Velocity, 1e-9);
            Assert.AreEqual(2, state.Position, 1e-9);

            state = VehicleDynamics.Step(state, 5, 10, 0.1);   // step 2
            Assert.AreEqual(5, state.Velocity, 1e-9);
            Assert.AreEqual(2.5, state.Position, 1e-9);

            state = VehicleDynamics.Step(state, 5, 10, 0.1);   // step 3
            Assert.AreEqual(5, state.Velocity, 1e-9);
            Assert.AreEqual(3, state.Position, 1e-9);
        }
    }
}