using AegisSim.Maths;
using NUnit.Framework;

namespace AegisSim.Tests.EditMode
{
    public class Vec3Tests
    {
        // From the practice sheet: 2² + 3² + 6² = 49, √49 = 7.
        [Test]
        public void Magnitude_TwoThreeSix_IsSeven()
        {
            var v = new Vec3(2, 3, 6);

            Assert.AreEqual(7, v.Magnitude, 1e-9);
        }

        [Test]
        public void Magnitude_ThreeFourZero_MatchesHandCalculation()
        {
            var v = new Vec3(3, 4, 0);

            Assert.AreEqual(5, v.Magnitude, 1e-9);   // Tee: your answer here
        }

        [Test]
        public void Add_AddsEachAxis()
        {
            var sum = new Vec3(1, 2, 3) + new Vec3(10, 20, 30);

            Assert.AreEqual(11, sum.X, 1e-9);
            Assert.AreEqual(22, sum.Y, 1e-9);
            Assert.AreEqual(33, sum.Z, 1e-9);
        }

        [Test]
        public void MultiplyByNumber_ScalesEachAxis()
        {
            var scaled = new Vec3(5, 0, -2) * 0.1;

            Assert.AreEqual(0.5, scaled.X, 1e-9);
            Assert.AreEqual(0, scaled.Y, 1e-9);
            Assert.AreEqual(-0.2, scaled.Z, 1e-9);
        }

        [Test]
        public void DivideByNumber_DividesEachAxis()
        {
            var divided = new Vec3(3, 0, -1) / 0.1;

            Assert.AreEqual(30, divided.X, 1e-9);
            Assert.AreEqual(0, divided.Y, 1e-9);
            Assert.AreEqual(-10, divided.Z, 1e-9);
        }

        // Too long: (6, 8, 0) has length 10, capped at 5, so every part is halved.
        [Test]
        public void ClampMagnitude_TooLong_ShrinksKeepingDirection()
        {
            var clamped = new Vec3(6, 8, 0).ClampMagnitude(5);

            Assert.AreEqual(3, clamped.X, 1e-9);
            Assert.AreEqual(4, clamped.Y, 1e-9);
            Assert.AreEqual(0, clamped.Z, 1e-9);
        }

        // Short enough: (1, 2, 2) has length 3, under the cap of 5, so it is unchanged.
        [Test]
        public void ClampMagnitude_UnderLimit_LeavesVectorUnchanged()
        {
            var clamped = new Vec3(1, 2, 2).ClampMagnitude(5);

            Assert.AreEqual(1, clamped.X, 1e-9);
            Assert.AreEqual(2, clamped.Y, 1e-9);
            Assert.AreEqual(2, clamped.Z, 1e-9);
        }
    }
}
