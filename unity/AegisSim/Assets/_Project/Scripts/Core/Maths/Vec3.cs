using System;

namespace AegisSim.Maths
{
    /// <summary>
    /// A 3D vector of doubles: a position (m), a velocity (m/s) or an acceleration (m/s²).
    /// Core can't use Unity's Vector3 (No Engine References), and Vector3 uses float.
    /// Axes follow Unity: Y is up, X and Z are the ground plane.
    /// </summary>
    public readonly struct Vec3
    {
        public double X { get; }
        public double Y { get; }
        public double Z { get; }

        public Vec3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vec3 Zero => new Vec3(0, 0, 0);

        /// <summary>Length of the arrow: √(x² + y² + z²). For a velocity, this is the speed.</summary>
        public double Magnitude => Math.Sqrt(X * X + Y * Y + Z * Z);

        /// <summary>
        /// Returns this vector shrunk to at most maxLength, keeping its direction.
        /// Used for the acceleration cap and the top-speed cap.
        /// </summary>
        public Vec3 ClampMagnitude(double maxLength)
        {
            // 1. Work out this vector's magnitude.
            double length = Magnitude;
            // 2. If it is not over maxLength, return this vector unchanged ("return this;").
            if (length <= maxLength)
            {
                return this;
            }
            // 3. Otherwise return this vector multiplied by (maxLength / magnitude).
            return this * (maxLength / length);
        }

        // Operators work axis by axis, so the 1D maths carries over to each axis.
        public static Vec3 operator +(Vec3 a, Vec3 b) => new Vec3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Vec3 operator -(Vec3 a, Vec3 b) => new Vec3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        public static Vec3 operator *(Vec3 v, double s) => new Vec3(v.X * s, v.Y * s, v.Z * s);
        public static Vec3 operator /(Vec3 v, double s) => new Vec3(v.X / s, v.Y / s, v.Z / s);

        public override string ToString() => $"({X}, {Y}, {Z})";
    }
}
