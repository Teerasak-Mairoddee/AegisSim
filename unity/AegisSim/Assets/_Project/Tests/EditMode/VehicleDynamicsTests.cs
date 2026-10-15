using AegisSim.Maths;
using AegisSim.Simulation;
using NUnit.Framework;
using System;

namespace AegisSim.Tests.EditMode
{
    public class VehicleDynamicsTests
    {
        // Top speed 20 m/s in the tests below: higher than any speed they use, so it never interferes.

        // Trace B from lesson 002: start at rest, requested 5 m/s, a_max 10 m/s², dt 0.1 s.
        // Flown along the X axis only, so the numbers match the 1D hand trace.
        [Test]
        public void Step_FromRest_MatchesHandTrace()
        {
            var state = new VehicleState(Vec3.Zero, Vec3.Zero);
            var requested = new Vec3(5, 0, 0);

            state = VehicleDynamics.Step(state, requested, 20, 10, 0.1);   // step 1
            Assert.AreEqual(1, state.Velocity.X, 1e-9);
            Assert.AreEqual(0.1, state.Position.X, 1e-9);

            //step 2 (v = 2, p = 0.3)
            state = VehicleDynamics.Step(state, requested, 20, 10, 0.1);
            Assert.AreEqual(2, state.Velocity.X, 1e-9);
            Assert.AreEqual(0.3, state.Position.X, 1e-9);

            //step 3 (v = 3, p = 0.6)
            state = VehicleDynamics.Step(state, requested, 20, 10, 0.1);
            Assert.AreEqual(3, state.Velocity.X, 1e-9);
            Assert.AreEqual(0.6, state.Position.X, 1e-9);
        }

        // Braking with a limit: 8 m/s asked for 5 m/s, a_max 10 m/s², dt 0.1 s.
        [Test]
        public void Step_Braking_IsLimitedByMaxAcceleration()
        {
            var state = new VehicleState(Vec3.Zero, new Vec3(8, 0, 0));

            state = VehicleDynamics.Step(state, new Vec3(5, 0, 0), 20, 10, 0.1);   // step 1
            Assert.AreEqual(7, state.Velocity.X, 1e-9);

            state = VehicleDynamics.Step(state, new Vec3(5, 0, 0), 20, 10, 0.1);   // step 2
            Assert.AreEqual(6, state.Velocity.X, 1e-9);
        }

        // Holding speed: already at the requested 5 m/s (Trace B, steps 6 to 8).
        [Test]
        public void Step_AtRequestedSpeed_HoldsSpeed()
        {

            var state = new VehicleState(new Vec3(1.5, 0, 0), new Vec3(5, 0, 0));

            state = VehicleDynamics.Step(state, new Vec3(5, 0, 0), 20, 10, 0.1);   // step 1
            Assert.AreEqual(5, state.Velocity.X, 1e-9);
            Assert.AreEqual(2, state.Position.X, 1e-9);

            state = VehicleDynamics.Step(state, new Vec3(5, 0, 0), 20, 10, 0.1);   // step 2
            Assert.AreEqual(5, state.Velocity.X, 1e-9);
            Assert.AreEqual(2.5, state.Position.X, 1e-9);

            state = VehicleDynamics.Step(state, new Vec3(5, 0, 0), 20, 10, 0.1);   // step 3
            Assert.AreEqual(5, state.Velocity.X, 1e-9);
            Assert.AreEqual(3, state.Position.X, 1e-9);
        }

        // Bad input: Step must refuse a zero or negative dt.
        [Test]
        public void Step_ZeroOrNegativeDt_Throws()
        {
            var state = new VehicleState(Vec3.Zero, Vec3.Zero);

            Assert.Throws<ArgumentOutOfRangeException>(() => VehicleDynamics.Step(state, new Vec3(5, 0, 0), 20, 10, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => VehicleDynamics.Step(state, new Vec3(5, 0, 0), 20, 10, -0.1));
        }


        [Test]
        public void Step_RequestAboveTopSpeed_IsCappedAtTopSpeed()
        {
            var state = new VehicleState(new Vec3(0, 0, 0), new Vec3(0, 0, 0));

            state = VehicleDynamics.Step(state, new Vec3(30, 0, 40), 10, 100, 0.1);
            Assert.AreEqual(6, state.Velocity.X, 1e-9);
            Assert.AreEqual(0, state.Velocity.Y, 1e-9);
            Assert.AreEqual(8, state.Velocity.Z, 1e-9);
            Assert.AreEqual(10, state.Velocity.Magnitude, 1e-9);

        }
    }
}