// Copyright (c) Andrew Pollard, .NET Foundation and Contributors.
// Licensed under the MIT license - see README.md for details.

using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
//using System.Tests;
using Xunit;

namespace Invicta.Numerics.Tests
{
    public sealed class Vector3DDTests
    {
        private const int ElementCount = 3;

        /// <summary>Verifies that two <see cref="Vector3D" /> values are equal, within the <paramref name="variance" />.</summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The value to be compared against</param>
        /// <param name="variance">The total variance allowed between the expected and actual results.</param>
        /// <exception cref="EqualException">Thrown when the values are not equal</exception>
        internal static void AssertEqual(Vector3D expected, Vector3D actual, Vector3D variance)
        {
            AssertExtensions.Equal(expected.X, actual.X, variance.X);
            AssertExtensions.Equal(expected.Y, actual.Y, variance.Y);
            AssertExtensions.Equal(expected.Z, actual.Z, variance.Z);
        }

        [Fact]
        public void Vector3DMarshalSizeTest()
        {
            Assert.Equal(12, Marshal.SizeOf<Vector3D>());
            Assert.Equal(12, Marshal.SizeOf<Vector3D>(new Vector3D()));
        }

        [Theory]
        [InlineData(0.0d, 1.0d, 0.0d)]
        [InlineData(1.0d, 0.0d, 1.0d)]
        [InlineData(3.1434343d, 1.1234123d, 0.1234123d)]
        [InlineData(1.0000001d, 0.0000001d, 2.0000001d)]
        public void Vector3DIndexerGetTest(double x, double y, double z)
        {
            var vector = new Vector3D(x, y, z);

            Assert.Equal(x, vector[0]);
            Assert.Equal(y, vector[1]);
            Assert.Equal(z, vector[2]);
        }

        [Theory]
        [InlineData(0.0d, 1.0d, 0.0d)]
        [InlineData(1.0d, 0.0d, 1.0d)]
        [InlineData(3.1434343d, 1.1234123d, 0.1234123d)]
        [InlineData(1.0000001d, 0.0000001d, 2.0000001d)]
        public void Vector3DIndexerSetTest(double x, double y, double z)
        {
            var vector = new Vector3D(0.0d, 0.0d, 0.0d);

            vector[0] = x;
            vector[1] = y;
            vector[2] = z;

            Assert.Equal(x, vector[0]);
            Assert.Equal(y, vector[1]);
            Assert.Equal(z, vector[2]);
        }

        [Fact]
        public void Vector3DCopyToTest()
        {
            Vector3D v1 = new Vector3D(2.0d, 3.0d, 3.3d);

            double[] a = new double[4];
            double[] b = new double[3];

            Assert.Throws<NullReferenceException>(() => v1.CopyTo(null, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => v1.CopyTo(a, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => v1.CopyTo(a, a.Length));
            Assert.Throws<ArgumentException>(() => v1.CopyTo(a, a.Length - 2));

            v1.CopyTo(a, 1);
            v1.CopyTo(b);
            Assert.Equal(0.0d, a[0]);
            Assert.Equal(2.0d, a[1]);
            Assert.Equal(3.0d, a[2]);
            Assert.Equal(3.3d, a[3]);
            Assert.Equal(2.0d, b[0]);
            Assert.Equal(3.0d, b[1]);
            Assert.Equal(3.3d, b[2]);
        }

        [Fact]
        public void Vector3DCopyToSpanTest()
        {
            Vector3D vector = new Vector3D(1.0d, 2.0d, 3.0d);
            Span<double> destination = new double[3];

            Assert.Throws<ArgumentException>(() => vector.CopyTo(new Span<double>(new double[2])));
            vector.CopyTo(destination);

            Assert.Equal(1.0d, vector.X);
            Assert.Equal(2.0d, vector.Y);
            Assert.Equal(3.0d, vector.Z);
            Assert.Equal(vector.X, destination[0]);
            Assert.Equal(vector.Y, destination[1]);
            Assert.Equal(vector.Z, destination[2]);
        }

        [Fact]
        public void Vector3DTryCopyToTest()
        {
            Vector3D vector = new Vector3D(1.0d, 2.0d, 3.0d);
            Span<double> destination = new double[3];

            Assert.False(vector.TryCopyTo(new Span<double>(new double[2])));
            Assert.True(vector.TryCopyTo(destination));

            Assert.Equal(1.0d, vector.X);
            Assert.Equal(2.0d, vector.Y);
            Assert.Equal(3.0d, vector.Z);
            Assert.Equal(vector.X, destination[0]);
            Assert.Equal(vector.Y, destination[1]);
            Assert.Equal(vector.Z, destination[2]);
        }

        [Fact]
        public void Vector3DGetHashCodeTest()
        {
            Vector3D v1 = new Vector3D(2.0d, 3.0d, 3.3d);
            Vector3D v2 = new Vector3D(2.0d, 3.0d, 3.3d);
            Vector3D v3 = new Vector3D(2.0d, 3.0d, 3.3d);
            Vector3D v5 = new Vector3D(3.0d, 2.0d, 3.3d);
            Assert.Equal(v1.GetHashCode(), v1.GetHashCode());
            Assert.Equal(v1.GetHashCode(), v2.GetHashCode());
            Assert.NotEqual(v1.GetHashCode(), v5.GetHashCode());
            Assert.Equal(v1.GetHashCode(), v3.GetHashCode());
            Vector3D v4 = new Vector3D(0.0d, 0.0d, 0.0d);
            Vector3D v6 = new Vector3D(1.0d, 0.0d, 0.0d);
            Vector3D v7 = new Vector3D(0.0d, 1.0d, 0.0d);
            Vector3D v8 = new Vector3D(1.0d, 1.0d, 1.0d);
            Vector3D v9 = new Vector3D(1.0d, 1.0d, 0.0d);
            Assert.NotEqual(v4.GetHashCode(), v6.GetHashCode());
            Assert.NotEqual(v4.GetHashCode(), v7.GetHashCode());
            Assert.NotEqual(v4.GetHashCode(), v8.GetHashCode());
            Assert.NotEqual(v7.GetHashCode(), v6.GetHashCode());
            Assert.NotEqual(v8.GetHashCode(), v6.GetHashCode());
            Assert.NotEqual(v8.GetHashCode(), v9.GetHashCode());
            Assert.NotEqual(v7.GetHashCode(), v9.GetHashCode());
        }

        [Fact]
        public void Vector3DToStringTest()
        {
            string separator = CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator;
            CultureInfo enUsCultureInfo = new CultureInfo("en-US");

            Vector3D v1 = new Vector3D(2.0d, 3.0d, 3.3d);
            string v1str = v1.ToString();
            string expectedv1 = string.Format(CultureInfo.CurrentCulture
                , "<{1:G}{0} {2:G}{0} {3:G}>"
                , separator, 2, 3, 3.3);
            Assert.Equal(expectedv1, v1str);

            string v1strformatted = v1.ToString("c", CultureInfo.CurrentCulture);
            string expectedv1dormatted = string.Format(CultureInfo.CurrentCulture
                , "<{1:c}{0} {2:c}{0} {3:c}>"
                , separator, 2, 3, 3.3);
            Assert.Equal(expectedv1dormatted, v1strformatted);

            string v2strformatted = v1.ToString("c", enUsCultureInfo);
            string expectedv2dormatted = string.Format(enUsCultureInfo
                , "<{1:c}{0} {2:c}{0} {3:c}>"
                , enUsCultureInfo.NumberFormat.NumberGroupSeparator, 2, 3, 3.3);
            Assert.Equal(expectedv2dormatted, v2strformatted);

            string v3strformatted = v1.ToString("c");
            string expectedv3dormatted = string.Format(CultureInfo.CurrentCulture
                , "<{1:c}{0} {2:c}{0} {3:c}>"
                , separator, 2, 3, 3.3);
            Assert.Equal(expectedv3dormatted, v3strformatted);
        }

        // A test for Cross (Vector3Df, Vector3Df)
        [Fact]
        public void Vector3DCrossTest()
        {
            Vector3D a = new Vector3D(1.0d, 0.0d, 0.0d);
            Vector3D b = new Vector3D(0.0d, 1.0d, 0.0d);

            Vector3D expected = new Vector3D(0.0d, 0.0d, 1.0d);
            Vector3D actual;

            actual = Vector3D.Cross(a, b);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Cross did not return the expected value.");
        }

        // A test for Cross (Vector3Df, Vector3Df)
        // Cross test of the same vector
        [Fact]
        public void Vector3DCrossTest1()
        {
            Vector3D a = new Vector3D(0.0d, 1.0d, 0.0d);
            Vector3D b = new Vector3D(0.0d, 1.0d, 0.0d);

            Vector3D expected = new Vector3D(0.0d, 0.0d, 0.0d);
            Vector3D actual = Vector3D.Cross(a, b);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Cross did not return the expected value.");
        }

        // A test for Cross (Vector3Df, Vector3Df)
        // Cross test of the same parallel vector
        [Fact]
        public void Vector3DCrossSameParallelVectors()
        {
            var v = new Vector3D(-1, 1, 0);
            var n = Vector3D.Normalize(v);
            var actual = Vector3D.Cross(n, n);
            Assert.Equal(Vector3D.Zero, actual);
        }

        // A test for Distance (Vector3Df, Vector3Df)
        [Fact]
        public void Vector3DDistanceTest()
        {
            Vector3D a = new Vector3D(1.0d, 2.0d, 3.0d);
            Vector3D b = new Vector3D(4.0d, 5.0d, 6.0d);

            double expected = (double)System.Math.Sqrt(27);
            double actual;

            actual = Vector3D.Distance(a, b);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Distance did not return the expected value.");
        }

        // A test for Distance (Vector3Df, Vector3Df)
        // Distance from the same point
        [Fact]
        public void Vector3DDistanceTest1()
        {
            Vector3D a = new Vector3D(1.051d, 2.05d, 3.478d);
            Vector3D b = new Vector3D(new Vector2D(1.051d, 0.0d), 1);
            b.Y = 2.05d;
            b.Z = 3.478d;

            double actual = Vector3D.Distance(a, b);
            Assert.Equal(0.0d, actual);
        }

        // A test for DistanceSquared (Vector3Df, Vector3Df)
        [Fact]
        public void Vector3DDistanceSquaredTest()
        {
            Vector3D a = new Vector3D(1.0d, 2.0d, 3.0d);
            Vector3D b = new Vector3D(4.0d, 5.0d, 6.0d);

            double expected = 27.0d;
            double actual;

            actual = Vector3D.DistanceSquared(a, b);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.DistanceSquared did not return the expected value.");
        }

        // A test for Dot (Vector3Df, Vector3Df)
        [Fact]
        public void Vector3DDotTest()
        {
            Vector3D a = new Vector3D(1.0d, 2.0d, 3.0d);
            Vector3D b = new Vector3D(4.0d, 5.0d, 6.0d);

            double expected = 32.0d;
            double actual;

            actual = Vector3D.Dot(a, b);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Dot did not return the expected value.");
        }

        // A test for Dot (Vector3Df, Vector3Df)
        // Dot test for perpendicular vector
        [Fact]
        public void Vector3DDotTest1()
        {
            Vector3D a = new Vector3D(1.55d, 1.55d, 1);
            Vector3D b = new Vector3D(2.5d, 3, 1.5d);
            Vector3D c = Vector3D.Cross(a, b);

            double expected = 0.0d;
            double actual1 = Vector3D.Dot(a, c);
            double actual2 = Vector3D.Dot(b, c);
            Assert.True(MathHelper.Equal(expected, actual1), "Vector3Df.Dot did not return the expected value.");
            Assert.True(MathHelper.Equal(expected, actual2), "Vector3Df.Dot did not return the expected value.");
        }

        // A test for Length ()
        [Fact]
        public void Vector3DLengthTest()
        {
            Vector2D a = new Vector2D(1.0d, 2.0d);

            double z = 3.0d;

            Vector3D target = new Vector3D(a, z);

            double expected = (double)System.Math.Sqrt(14.0d);
            double actual;

            actual = target.Length();
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Length did not return the expected value.");
        }

        // A test for Length ()
        // Length test where length is zero
        [Fact]
        public void Vector3DLengthTest1()
        {
            Vector3D target = new Vector3D();

            double expected = 0.0d;
            double actual = target.Length();
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Length did not return the expected value.");
        }

        // A test for LengthSquared ()
        [Fact]
        public void Vector3DLengthSquaredTest()
        {
            Vector2D a = new Vector2D(1.0d, 2.0d);

            double z = 3.0d;

            Vector3D target = new Vector3D(a, z);

            double expected = 14.0d;
            double actual;

            actual = target.LengthSquared();
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.LengthSquared did not return the expected value.");
        }

        // A test for Min (Vector3Df, Vector3Df)
        [Fact]
        public void Vector3DMinTest()
        {
            Vector3D a = new Vector3D(-1.0d, 4.0d, -3.0d);
            Vector3D b = new Vector3D(2.0d, 1.0d, -1.0d);

            Vector3D expected = new Vector3D(-1.0d, 1.0d, -3.0d);
            Vector3D actual;
            actual = Vector3D.Min(a, b);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Min did not return the expected value.");
        }

        // A test for Max (Vector3Df, Vector3Df)
        [Fact]
        public void Vector3DMaxTest()
        {
            Vector3D a = new Vector3D(-1.0d, 4.0d, -3.0d);
            Vector3D b = new Vector3D(2.0d, 1.0d, -1.0d);

            Vector3D expected = new Vector3D(2.0d, 4.0d, -1.0d);
            Vector3D actual;
            actual = Vector3D.Max(a, b);
            Assert.True(MathHelper.Equal(expected, actual), "vector3.Max did not return the expected value.");
        }

        [Fact]
        public void Vector3DMinMaxCodeCoverageTest()
        {
            Vector3D min = Vector3D.Zero;
            Vector3D max = Vector3D.One;
            Vector3D actual;

            // Min.
            actual = Vector3D.Min(min, max);
            Assert.Equal(actual, min);

            actual = Vector3D.Min(max, min);
            Assert.Equal(actual, min);

            // Max.
            actual = Vector3D.Max(min, max);
            Assert.Equal(actual, max);

            actual = Vector3D.Max(max, min);
            Assert.Equal(actual, max);
        }

        // A test for Lerp (Vector3Df, Vector3Df, double)
        [Fact]
        public void Vector3DLerpTest()
        {
            Vector3D a = new Vector3D(1.0d, 2.0d, 3.0d);
            Vector3D b = new Vector3D(4.0d, 5.0d, 6.0d);

            double t = 0.5d;

            Vector3D expected = new Vector3D(2.5d, 3.5d, 4.5d);
            Vector3D actual;

            actual = Vector3D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector3Df, Vector3Df, double)
        // Lerp test with factor zero
        [Fact]
        public void Vector3DLerpTest1()
        {
            Vector3D a = new Vector3D(1.0d, 2.0d, 3.0d);
            Vector3D b = new Vector3D(4.0d, 5.0d, 6.0d);

            double t = 0.0d;
            Vector3D expected = new Vector3D(1.0d, 2.0d, 3.0d);
            Vector3D actual = Vector3D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector3Df, Vector3Df, double)
        // Lerp test with factor one
        [Fact]
        public void Vector3DLerpTest2()
        {
            Vector3D a = new Vector3D(1.0d, 2.0d, 3.0d);
            Vector3D b = new Vector3D(4.0d, 5.0d, 6.0d);

            double t = 1.0d;
            Vector3D expected = new Vector3D(4.0d, 5.0d, 6.0d);
            Vector3D actual = Vector3D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector3Df, Vector3Df, double)
        // Lerp test with factor > 1
        [Fact]
        public void Vector3DLerpTest3()
        {
            Vector3D a = new Vector3D(0.0d, 0.0d, 0.0d);
            Vector3D b = new Vector3D(4.0d, 5.0d, 6.0d);

            double t = 2.0d;
            Vector3D expected = new Vector3D(8.0d, 10.0d, 12.0d);
            Vector3D actual = Vector3D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector3Df, Vector3Df, double)
        // Lerp test with factor < 0
        [Fact]
        public void Vector3DLerpTest4()
        {
            Vector3D a = new Vector3D(0.0d, 0.0d, 0.0d);
            Vector3D b = new Vector3D(4.0d, 5.0d, 6.0d);

            double t = -2.0d;
            Vector3D expected = new Vector3D(-8.0d, -10.0d, -12.0d);
            Vector3D actual = Vector3D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector3Df, Vector3Df, double)
        // Lerp test with special double value
        [Fact]
        public void Vector3DLerpTest5()
        {
            Vector3D a = new Vector3D(45.67d, 90.0d, 0d);
            Vector3D b = new Vector3D(double.PositiveInfinity, double.NegativeInfinity, 0);

            double t = 0.408d;
            Vector3D actual = Vector3D.Lerp(a, b, t);
            Assert.True(double.IsPositiveInfinity(actual.X), "Vector3Df.Lerp did not return the expected value.");
            Assert.True(double.IsNegativeInfinity(actual.Y), "Vector3Df.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector3Df, Vector3Df, double)
        // Lerp test from the same point
        [Fact]
        public void Vector3DLerpTest6()
        {
            Vector3D a = new Vector3D(1.68d, 2.34d, 5.43d);
            Vector3D b = a;

            double t = 0.18d;
            Vector3D expected = new Vector3D(1.68d, 2.34d, 5.43d);
            Vector3D actual = Vector3D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector3Df, Vector3Df, double)
        // Lerp test with values known to be inaccurate with the old lerp impl
        [Fact]
        public void Vector3DLerpTest7()
        {
            Vector3D a = new Vector3D(0.44728136d);
            Vector3D b = new Vector3D(0.46345946d);

            double t = 0.26402435d;

            Vector3D expected = new Vector3D(0.45155275d);
            Vector3D actual = Vector3D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector3Df, Vector3Df, double)
        // Lerp test with values known to be inaccurate with the old lerp impl
        // (Old code incorrectly gets 0.33333588)
        [Fact]
        public void Vector3DLerpTest8()
        {
            Vector3D a = new Vector3D(-100);
            Vector3D b = new Vector3D(0.33333334d);

            double t = 1d;

            Vector3D expected = new Vector3D(0.33333334d);
            Vector3D actual = Vector3D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Lerp did not return the expected value.");
        }

        // A test for Reflect (Vector3Df, Vector3Df)
        [Fact]
        public void Vector3DReflectTest()
        {
            Vector3D a = Vector3D.Normalize(new Vector3D(1.0d, 1.0d, 1.0d));

            // Reflect on XZ plane.
            Vector3D n = new Vector3D(0.0d, 1.0d, 0.0d);
            Vector3D expected = new Vector3D(a.X, -a.Y, a.Z);
            Vector3D actual = Vector3D.Reflect(a, n);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Reflect did not return the expected value.");

            // Reflect on XY plane.
            n = new Vector3D(0.0d, 0.0d, 1.0d);
            expected = new Vector3D(a.X, a.Y, -a.Z);
            actual = Vector3D.Reflect(a, n);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Reflect did not return the expected value.");

            // Reflect on YZ plane.
            n = new Vector3D(1.0d, 0.0d, 0.0d);
            expected = new Vector3D(-a.X, a.Y, a.Z);
            actual = Vector3D.Reflect(a, n);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Reflect did not return the expected value.");
        }

        // A test for Reflect (Vector3Df, Vector3Df)
        // Reflection when normal and source are the same
        [Fact]
        public void Vector3DReflectTest1()
        {
            Vector3D n = new Vector3D(0.45d, 1.28d, 0.86d);
            n = Vector3D.Normalize(n);
            Vector3D a = n;

            Vector3D expected = -n;
            Vector3D actual = Vector3D.Reflect(a, n);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Reflect did not return the expected value.");
        }

        // A test for Reflect (Vector3Df, Vector3Df)
        // Reflection when normal and source are negation
        [Fact]
        public void Vector3DReflectTest2()
        {
            Vector3D n = new Vector3D(0.45d, 1.28d, 0.86d);
            n = Vector3D.Normalize(n);
            Vector3D a = -n;

            Vector3D expected = n;
            Vector3D actual = Vector3D.Reflect(a, n);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Reflect did not return the expected value.");
        }

        // A test for Reflect (Vector3Df, Vector3Df)
        // Reflection when normal and source are perpendicular (a dot n = 0)
        [Fact]
        public void Vector3DReflectTest3()
        {
            Vector3D n = new Vector3D(0.45d, 1.28d, 0.86d);
            Vector3D temp = new Vector3D(1.28d, 0.45d, 0.01d);
            // find a perpendicular vector of n
            Vector3D a = Vector3D.Cross(temp, n);

            Vector3D expected = a;
            Vector3D actual = Vector3D.Reflect(a, n);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Reflect did not return the expected value.");
        }

        // A test for Transform(Vector3Df, Matrix4x4D)
        [Fact]
        public void Vector3DTransformTest()
        {
            Vector3D v = new Vector3D(1.0d, 2.0d, 3.0d);
            Matrix4x4D m =
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0d)) *
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0d)) *
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0d));
            m.M41 = 10.0d;
            m.M42 = 20.0d;
            m.M43 = 30.0d;

            Vector3D expected = new Vector3D(12.191987d, 21.533493d, 32.616024d);
            Vector3D actual;

            actual = Vector3D.Transform(v, m);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Transform did not return the expected value.");
        }

        // A test for Clamp (Vector3Df, Vector3Df, Vector3Df)
        [Fact]
        public void Vector3DClampTest()
        {
            Vector3D a = new Vector3D(0.5d, 0.3d, 0.33d);
            Vector3D min = new Vector3D(0.0d, 0.1d, 0.13d);
            Vector3D max = new Vector3D(1.0d, 1.1d, 1.13d);

            // Normal case.
            // Case N1: specified value is in the range.
            Vector3D expected = new Vector3D(0.5d, 0.3d, 0.33d);
            Vector3D actual = Vector3D.Clamp(a, min, max);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Clamp did not return the expected value.");

            // Normal case.
            // Case N2: specified value is bigger than max value.
            a = new Vector3D(2.0d, 3.0d, 4.0d);
            expected = max;
            actual = Vector3D.Clamp(a, min, max);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Clamp did not return the expected value.");

            // Case N3: specified value is smaller than max value.
            a = new Vector3D(-2.0d, -3.0d, -4.0d);
            expected = min;
            actual = Vector3D.Clamp(a, min, max);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Clamp did not return the expected value.");

            // Case N4: combination case.
            a = new Vector3D(-2.0d, 0.5d, 4.0d);
            expected = new Vector3D(min.X, a.Y, max.Z);
            actual = Vector3D.Clamp(a, min, max);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Clamp did not return the expected value.");

            // User specified min value is bigger than max value.
            max = new Vector3D(0.0d, 0.1d, 0.13d);
            min = new Vector3D(1.0d, 1.1d, 1.13d);

            // Case W1: specified value is in the range.
            a = new Vector3D(0.5d, 0.3d, 0.33d);
            expected = max;
            actual = Vector3D.Clamp(a, min, max);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Clamp did not return the expected value.");

            // Normal case.
            // Case W2: specified value is bigger than max and min value.
            a = new Vector3D(2.0d, 3.0d, 4.0d);
            expected = max;
            actual = Vector3D.Clamp(a, min, max);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Clamp did not return the expected value.");

            // Case W3: specified value is smaller than min and max value.
            a = new Vector3D(-2.0d, -3.0d, -4.0d);
            expected = max;
            actual = Vector3D.Clamp(a, min, max);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Clamp did not return the expected value.");
        }

        // A test for TransformNormal (Vector3Df, Matrix4x4D)
        [Fact]
        public void Vector3DTransformNormalTest()
        {
            Vector3D v = new Vector3D(1.0d, 2.0d, 3.0d);
            Matrix4x4D m =
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0d)) *
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0d)) *
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0d));
            m.M41 = 10.0d;
            m.M42 = 20.0d;
            m.M43 = 30.0d;

            Vector3D expected = new Vector3D(2.19198728d, 1.53349364d, 2.61602545d);
            Vector3D actual;

            actual = Vector3D.TransformNormal(v, m);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.TransformNormal did not return the expected value.");
        }

        // A test for Transform (Vector3Df, QuaternionD)
        [Fact]
        public void Vector3DTransformByQuaternionDTest()
        {
            Vector3D v = new Vector3D(1.0d, 2.0d, 3.0d);

            Matrix4x4D m =
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0d)) *
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0d)) *
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0d));
            QuaternionD q = QuaternionD.CreateFromRotationMatrix(m);

            Vector3D expected = Vector3D.Transform(v, m);
            Vector3D actual = Vector3D.Transform(v, q);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Transform did not return the expected value.");
        }

        // A test for Transform (Vector3Df, QuaternionD)
        // Transform vector3 with zero quaternion
        [Fact]
        public void Vector3DTransformByQuaternionDTest1()
        {
            Vector3D v = new Vector3D(1.0d, 2.0d, 3.0d);
            QuaternionD q = new QuaternionD();
            Vector3D expected = Vector3D.Zero;

            Vector3D actual = Vector3D.Transform(v, q);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Transform did not return the expected value.");
        }

        // A test for Transform (Vector3Df, QuaternionD)
        // Transform vector3 with identity quaternion
        [Fact]
        public void Vector3DTransformByQuaternionDTest2()
        {
            Vector3D v = new Vector3D(1.0d, 2.0d, 3.0d);
            QuaternionD q = QuaternionD.Identity;
            Vector3D expected = v;

            Vector3D actual = Vector3D.Transform(v, q);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Transform did not return the expected value.");
        }

        // A test for Normalize (Vector3Df)
        [Fact]
        public void Vector3DNormalizeTest()
        {
            Vector3D a = new Vector3D(1.0d, 2.0d, 3.0d);

            Vector3D expected = new Vector3D(
                0.26726124191242438468455348087975d,
                0.53452248382484876936910696175951d,
                0.80178372573727315405366044263926d);
            Vector3D actual;

            actual = Vector3D.Normalize(a);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Normalize did not return the expected value.");
        }

        // A test for Normalize (Vector3Df)
        // Normalize vector of length one
        [Fact]
        public void Vector3DNormalizeTest1()
        {
            Vector3D a = new Vector3D(1.0d, 0.0d, 0.0d);

            Vector3D expected = new Vector3D(1.0d, 0.0d, 0.0d);
            Vector3D actual = Vector3D.Normalize(a);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Normalize did not return the expected value.");
        }

        // A test for Normalize (Vector3Df)
        // Normalize vector of length zero
        [Fact]
        public void Vector3DNormalizeTest2()
        {
            Vector3D a = new Vector3D(0.0d, 0.0d, 0.0d);

            Vector3D expected = new Vector3D(0.0d, 0.0d, 0.0d);
            Vector3D actual = Vector3D.Normalize(a);
            Assert.True(double.IsNaN(actual.X) && double.IsNaN(actual.Y) && double.IsNaN(actual.Z), "Vector3Df.Normalize did not return the expected value.");
        }

        // A test for operator - (Vector3Df)
        [Fact]
        public void Vector3DUnaryNegationTest()
        {
            Vector3D a = new Vector3D(1.0d, 2.0d, 3.0d);

            Vector3D expected = new Vector3D(-1.0d, -2.0d, -3.0d);
            Vector3D actual;

            actual = -a;

            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.operator - did not return the expected value.");
        }

        [Fact]
        public void Vector3DUnaryNegationTest1()
        {
            Vector3D a = -new Vector3D(double.NaN, double.PositiveInfinity, double.NegativeInfinity);
            Vector3D b = -new Vector3D(0.0d, 0.0d, 0.0d);
            Assert.Equal(double.NaN, a.X);
            Assert.Equal(double.NegativeInfinity, a.Y);
            Assert.Equal(double.PositiveInfinity, a.Z);
            Assert.Equal(0.0d, b.X);
            Assert.Equal(0.0d, b.Y);
            Assert.Equal(0.0d, b.Z);
        }

        // A test for operator - (Vector3Df, Vector3Df)
        [Fact]
        public void Vector3DSubtractionTest()
        {
            Vector3D a = new Vector3D(4.0d, 2.0d, 3.0d);

            Vector3D b = new Vector3D(1.0d, 5.0d, 7.0d);

            Vector3D expected = new Vector3D(3.0d, -3.0d, -4.0d);
            Vector3D actual;

            actual = a - b;

            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.operator - did not return the expected value.");
        }

        // A test for operator * (Vector3Df, double)
        [Fact]
        public void Vector3DMultiplyOperatorTest()
        {
            Vector3D a = new Vector3D(1.0d, 2.0d, 3.0d);

            double factor = 2.0d;

            Vector3D expected = new Vector3D(2.0d, 4.0d, 6.0d);
            Vector3D actual;

            actual = a * factor;

            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.operator * did not return the expected value.");
        }

        // A test for operator * (double, Vector3Df)
        [Fact]
        public void Vector3DMultiplyOperatorTest2()
        {
            Vector3D a = new Vector3D(1.0d, 2.0d, 3.0d);

            const double factor = 2.0d;

            Vector3D expected = new Vector3D(2.0d, 4.0d, 6.0d);
            Vector3D actual;

            actual = factor * a;

            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.operator * did not return the expected value.");
        }

        // A test for operator * (Vector3Df, Vector3Df)
        [Fact]
        public void Vector3DMultiplyOperatorTest3()
        {
            Vector3D a = new Vector3D(1.0d, 2.0d, 3.0d);

            Vector3D b = new Vector3D(4.0d, 5.0d, 6.0d);

            Vector3D expected = new Vector3D(4.0d, 10.0d, 18.0d);
            Vector3D actual;

            actual = a * b;

            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.operator * did not return the expected value.");
        }

        // A test for operator / (Vector3Df, double)
        [Fact]
        public void Vector3DDivisionTest()
        {
            Vector3D a = new Vector3D(1.0d, 2.0d, 3.0d);

            double div = 2.0d;

            Vector3D expected = new Vector3D(0.5d, 1.0d, 1.5d);
            Vector3D actual;

            actual = a / div;

            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.operator / did not return the expected value.");
        }

        // A test for operator / (Vector3Df, Vector3Df)
        [Fact]
        public void Vector3DDivisionTest1()
        {
            Vector3D a = new Vector3D(4.0d, 2.0d, 3.0d);

            Vector3D b = new Vector3D(1.0d, 5.0d, 6.0d);

            Vector3D expected = new Vector3D(4.0d, 0.4d, 0.5d);
            Vector3D actual;

            actual = a / b;

            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.operator / did not return the expected value.");
        }

        // A test for operator / (Vector3Df, Vector3Df)
        // Divide by zero
        [Fact]
        public void Vector3DDivisionTest2()
        {
            Vector3D a = new Vector3D(-2.0d, 3.0d, double.MaxValue);

            double div = 0.0d;

            Vector3D actual = a / div;

            Assert.True(double.IsNegativeInfinity(actual.X), "Vector3Df.operator / did not return the expected value.");
            Assert.True(double.IsPositiveInfinity(actual.Y), "Vector3Df.operator / did not return the expected value.");
            Assert.True(double.IsPositiveInfinity(actual.Z), "Vector3Df.operator / did not return the expected value.");
        }

        // A test for operator / (Vector3Df, Vector3Df)
        // Divide by zero
        [Fact]
        public void Vector3DDivisionTest3()
        {
            Vector3D a = new Vector3D(0.047d, -3.0d, double.NegativeInfinity);
            Vector3D b = new Vector3D();

            Vector3D actual = a / b;

            Assert.True(double.IsPositiveInfinity(actual.X), "Vector3Df.operator / did not return the expected value.");
            Assert.True(double.IsNegativeInfinity(actual.Y), "Vector3Df.operator / did not return the expected value.");
            Assert.True(double.IsNegativeInfinity(actual.Z), "Vector3Df.operator / did not return the expected value.");
        }

        // A test for operator + (Vector3Df, Vector3Df)
        [Fact]
        public void Vector3DAdditionTest()
        {
            Vector3D a = new Vector3D(1.0d, 2.0d, 3.0d);
            Vector3D b = new Vector3D(4.0d, 5.0d, 6.0d);

            Vector3D expected = new Vector3D(5.0d, 7.0d, 9.0d);
            Vector3D actual;

            actual = a + b;

            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.operator + did not return the expected value.");
        }

        // A test for Vector3Df (double, double, double)
        [Fact]
        public void Vector3DConstructorTest()
        {
            double x = 1.0d;
            double y = 2.0d;
            double z = 3.0d;

            Vector3D target = new Vector3D(x, y, z);
            Assert.True(MathHelper.Equal(target.X, x) && MathHelper.Equal(target.Y, y) && MathHelper.Equal(target.Z, z), "Vector3Df.constructor (x,y,z) did not return the expected value.");
        }

        // A test for Vector3Df (Vector2Df, double)
        [Fact]
        public void Vector3DConstructorTest1()
        {
            Vector2D a = new Vector2D(1.0d, 2.0d);

            double z = 3.0d;

            Vector3D target = new Vector3D(a, z);
            Assert.True(MathHelper.Equal(target.X, a.X) && MathHelper.Equal(target.Y, a.Y) && MathHelper.Equal(target.Z, z), "Vector3Df.constructor (Vector2Df,z) did not return the expected value.");
        }

        // A test for Vector3Df ()
        // Constructor with no parameter
        [Fact]
        public void Vector3DConstructorTest3()
        {
            Vector3D a = new Vector3D();

            Assert.Equal(0.0d, a.X);
            Assert.Equal(0.0d, a.Y);
            Assert.Equal(0.0d, a.Z);
        }

        // A test for Vector2Df (double, double)
        // Constructor with special doubleing values
        [Fact]
        public void Vector3DConstructorTest4()
        {
            Vector3D target = new Vector3D(double.NaN, double.MaxValue, double.PositiveInfinity);

            Assert.True(double.IsNaN(target.X), "Vector3Df.constructor (Vector3Df) did not return the expected value.");
            Assert.True(double.Equals(double.MaxValue, target.Y), "Vector3Df.constructor (Vector3Df) did not return the expected value.");
            Assert.True(double.IsPositiveInfinity(target.Z), "Vector3Df.constructor (Vector3Df) did not return the expected value.");
        }

        // A test for Vector3Df (ReadOnlySpan<double>)
        [Fact]
        public void Vector3DConstructorTest6()
        {
            double value = 1.0d;
            Vector3D target = new Vector3D(new[] { value, value, value });
            Vector3D expected = new Vector3D(value);

            Assert.Equal(expected, target);
            Assert.Throws<ArgumentOutOfRangeException>(() => new Vector3D(new double[2]));
        }

        // A test for Add (Vector3Df, Vector3Df)
        [Fact]
        public void Vector3DAddTest()
        {
            Vector3D a = new Vector3D(1.0d, 2.0d, 3.0d);
            Vector3D b = new Vector3D(5.0d, 6.0d, 7.0d);

            Vector3D expected = new Vector3D(6.0d, 8.0d, 10.0d);
            Vector3D actual;

            actual = Vector3D.Add(a, b);
            Assert.Equal(expected, actual);
        }

        // A test for Divide (Vector3Df, double)
        [Fact]
        public void Vector3DDivideTest()
        {
            Vector3D a = new Vector3D(1.0d, 2.0d, 3.0d);
            double div = 2.0d;
            Vector3D expected = new Vector3D(0.5d, 1.0d, 1.5d);
            Vector3D actual;
            actual = Vector3D.Divide(a, div);
            Assert.Equal(expected, actual);
        }

        // A test for Divide (Vector3Df, Vector3Df)
        [Fact]
        public void Vector3DDivideTest1()
        {
            Vector3D a = new Vector3D(1.0d, 6.0d, 7.0d);
            Vector3D b = new Vector3D(5.0d, 2.0d, 3.0d);

            Vector3D expected = new Vector3D(1.0d / 5.0d, 6.0d / 2.0d, 7.0d / 3.0d);
            Vector3D actual;

            actual = Vector3D.Divide(a, b);
            Assert.Equal(expected, actual);
        }

        // A test for Equals (object)
        [Fact]
        public void Vector3DEqualsTest()
        {
            Vector3D a = new Vector3D(1.0d, 2.0d, 3.0d);
            Vector3D b = new Vector3D(1.0d, 2.0d, 3.0d);

            // case 1: compare between same values
            object obj = b;

            bool expected = true;
            bool actual = a.Equals(obj);
            Assert.Equal(expected, actual);

            // case 2: compare between different values
            b.X = 10.0d;
            obj = b;
            expected = false;
            actual = a.Equals(obj);
            Assert.Equal(expected, actual);

            // case 3: compare between different types.
            obj = new QuaternionD();
            expected = false;
            actual = a.Equals(obj);
            Assert.Equal(expected, actual);

            // case 3: compare against null.
            obj = null;
            expected = false;
            actual = a.Equals(obj);
            Assert.Equal(expected, actual);
        }

        // A test for Multiply (Vector3Df, double)
        [Fact]
        public void Vector3DMultiplyTest()
        {
            Vector3D a = new Vector3D(1.0d, 2.0d, 3.0d);
            const double factor = 2.0d;
            Vector3D expected = new Vector3D(2.0d, 4.0d, 6.0d);
            Vector3D actual = Vector3D.Multiply(a, factor);
            Assert.Equal(expected, actual);
        }

        // A test for Multiply (double, Vector3Df)
        [Fact]
        public static void Vector3DMultiplyTest2()
        {
            Vector3D a = new Vector3D(1.0d, 2.0d, 3.0d);
            const double factor = 2.0d;
            Vector3D expected = new Vector3D(2.0d, 4.0d, 6.0d);
            Vector3D actual = Vector3D.Multiply(factor, a);
            Assert.Equal(expected, actual);
        }

        // A test for Multiply (Vector3Df, Vector3Df)
        [Fact]
        public void Vector3DMultiplyTest3()
        {
            Vector3D a = new Vector3D(1.0d, 2.0d, 3.0d);
            Vector3D b = new Vector3D(5.0d, 6.0d, 7.0d);

            Vector3D expected = new Vector3D(5.0d, 12.0d, 21.0d);
            Vector3D actual;

            actual = Vector3D.Multiply(a, b);
            Assert.Equal(expected, actual);
        }

        // A test for Negate (Vector3Df)
        [Fact]
        public void Vector3DNegateTest()
        {
            Vector3D a = new Vector3D(1.0d, 2.0d, 3.0d);

            Vector3D expected = new Vector3D(-1.0d, -2.0d, -3.0d);
            Vector3D actual;

            actual = Vector3D.Negate(a);
            Assert.Equal(expected, actual);
        }

        // A test for operator != (Vector3Df, Vector3Df)
        [Fact]
        public void Vector3DInequalityTest()
        {
            Vector3D a = new Vector3D(1.0d, 2.0d, 3.0d);
            Vector3D b = new Vector3D(1.0d, 2.0d, 3.0d);

            // case 1: compare between same values
            bool expected = false;
            bool actual = a != b;
            Assert.Equal(expected, actual);

            // case 2: compare between different values
            b.X = 10.0d;
            expected = true;
            actual = a != b;
            Assert.Equal(expected, actual);
        }

        // A test for operator == (Vector3Df, Vector3Df)
        [Fact]
        public void Vector3DEqualityTest()
        {
            Vector3D a = new Vector3D(1.0d, 2.0d, 3.0d);
            Vector3D b = new Vector3D(1.0d, 2.0d, 3.0d);

            // case 1: compare between same values
            bool expected = true;
            bool actual = a == b;
            Assert.Equal(expected, actual);

            // case 2: compare between different values
            b.X = 10.0d;
            expected = false;
            actual = a == b;
            Assert.Equal(expected, actual);
        }

        // A test for Subtract (Vector3Df, Vector3Df)
        [Fact]
        public void Vector3DSubtractTest()
        {
            Vector3D a = new Vector3D(1.0d, 6.0d, 3.0d);
            Vector3D b = new Vector3D(5.0d, 2.0d, 3.0d);

            Vector3D expected = new Vector3D(-4.0d, 4.0d, 0.0d);
            Vector3D actual;

            actual = Vector3D.Subtract(a, b);
            Assert.Equal(expected, actual);
        }

        // A test for One
        [Fact]
        public void Vector3DOneTest()
        {
            Vector3D val = new Vector3D(1.0d, 1.0d, 1.0d);
            Assert.Equal(val, Vector3D.One);
        }

        // A test for UnitX
        [Fact]
        public void Vector3DUnitXTest()
        {
            Vector3D val = new Vector3D(1.0d, 0.0d, 0.0d);
            Assert.Equal(val, Vector3D.UnitX);
        }

        // A test for UnitY
        [Fact]
        public void Vector3DUnitYTest()
        {
            Vector3D val = new Vector3D(0.0d, 1.0d, 0.0d);
            Assert.Equal(val, Vector3D.UnitY);
        }

        // A test for UnitZ
        [Fact]
        public void Vector3DUnitZTest()
        {
            Vector3D val = new Vector3D(0.0d, 0.0d, 1.0d);
            Assert.Equal(val, Vector3D.UnitZ);
        }

        // A test for Zero
        [Fact]
        public void Vector3DZeroTest()
        {
            Vector3D val = new Vector3D(0.0d, 0.0d, 0.0d);
            Assert.Equal(val, Vector3D.Zero);
        }

        // A test for Equals (Vector3Df)
        [Fact]
        public void Vector3DEqualsTest1()
        {
            Vector3D a = new Vector3D(1.0d, 2.0d, 3.0d);
            Vector3D b = new Vector3D(1.0d, 2.0d, 3.0d);

            // case 1: compare between same values
            bool expected = true;
            bool actual = a.Equals(b);
            Assert.Equal(expected, actual);

            // case 2: compare between different values
            b.X = 10.0d;
            expected = false;
            actual = a.Equals(b);
            Assert.Equal(expected, actual);
        }

        // A test for Vector3Df (double)
        [Fact]
        public void Vector3DConstructorTest5()
        {
            double value = 1.0d;
            Vector3D target = new Vector3D(value);

            Vector3D expected = new Vector3D(value, value, value);
            Assert.Equal(expected, target);

            value = 2.0d;
            target = new Vector3D(value);
            expected = new Vector3D(value, value, value);
            Assert.Equal(expected, target);
        }

        // A test for Vector3Df comparison involving NaN values
        [Fact]
        public void Vector3DEqualsNaNTest()
        {
            Vector3D a = new Vector3D(double.NaN, 0, 0);
            Vector3D b = new Vector3D(0, double.NaN, 0);
            Vector3D c = new Vector3D(0, 0, double.NaN);

            Assert.False(a == Vector3D.Zero);
            Assert.False(b == Vector3D.Zero);
            Assert.False(c == Vector3D.Zero);

            Assert.True(a != Vector3D.Zero);
            Assert.True(b != Vector3D.Zero);
            Assert.True(c != Vector3D.Zero);

            Assert.False(a.Equals(Vector3D.Zero));
            Assert.False(b.Equals(Vector3D.Zero));
            Assert.False(c.Equals(Vector3D.Zero));

            Assert.True(a.Equals(a));
            Assert.True(b.Equals(b));
            Assert.True(c.Equals(c));
        }

        [Fact]
        public void Vector3DAbsTest()
        {
            Vector3D v1 = new Vector3D(-2.5d, 2.0d, 0.5d);
            Vector3D v3 = Vector3D.Abs(new Vector3D(0.0d, double.NegativeInfinity, double.NaN));
            Vector3D v = Vector3D.Abs(v1);
            Assert.Equal(2.5d, v.X);
            Assert.Equal(2.0d, v.Y);
            Assert.Equal(0.5d, v.Z);
            Assert.Equal(0.0d, v3.X);
            Assert.Equal(double.PositiveInfinity, v3.Y);
            Assert.Equal(double.NaN, v3.Z);
        }

        [Fact]
        public void Vector3DSqrtTest()
        {
            Vector3D a = new Vector3D(-2.5d, 2.0d, 0.5d);
            Vector3D b = new Vector3D(5.5d, 4.5d, 16.5d);
            Assert.Equal(2, (int)Vector3D.SquareRoot(b).X);
            Assert.Equal(2, (int)Vector3D.SquareRoot(b).Y);
            Assert.Equal(4, (int)Vector3D.SquareRoot(b).Z);
            Assert.Equal(double.NaN, Vector3D.SquareRoot(a).X);
        }

        // A test to make sure these types are blittable directly into GPU buffer memory layouts
        [Fact]
        public unsafe void Vector3DSizeofTest()
        {
            Assert.Equal(12, sizeof(Vector3D));
            Assert.Equal(24, sizeof(Vector3D_2x));
            Assert.Equal(16, sizeof(Vector3DPlusFloat));
            Assert.Equal(32, sizeof(Vector3DPlusFloat_2x));
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Vector3D_2x
        {
            private Vector3D _a;
            private Vector3D _b;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Vector3DPlusFloat
        {
            private Vector3D _v;
            private double _f;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Vector3DPlusFloat_2x
        {
            private Vector3DPlusFloat _a;
            private Vector3DPlusFloat _b;
        }

        [Fact]
        public void SetFieldsTest()
        {
            Vector3D v3 = new Vector3D(4d, 5d, 6d);
            v3.X = 1.0d;
            v3.Y = 2.0d;
            v3.Z = 3.0d;
            Assert.Equal(1.0d, v3.X);
            Assert.Equal(2.0d, v3.Y);
            Assert.Equal(3.0d, v3.Z);
            Vector3D v4 = v3;
            v4.Y = 0.5d;
            v4.Z = 2.2d;
            Assert.Equal(1.0d, v4.X);
            Assert.Equal(0.5d, v4.Y);
            Assert.Equal(2.2d, v4.Z);
            Assert.Equal(2.0d, v3.Y);

            Vector3D before = new Vector3D(1d, 2d, 3d);
            Vector3D after = before;
            after.X = 500.0d;
            Assert.NotEqual(before, after);
        }

        [Fact]
        public void EmbeddedVectorSetFields()
        {
            EmbeddedVectorObject evo = new EmbeddedVectorObject();
            evo.FieldVector.X = 5.0d;
            evo.FieldVector.Y = 5.0d;
            evo.FieldVector.Z = 5.0d;
            Assert.Equal(5.0d, evo.FieldVector.X);
            Assert.Equal(5.0d, evo.FieldVector.Y);
            Assert.Equal(5.0d, evo.FieldVector.Z);
        }

        private class EmbeddedVectorObject
        {
            public Vector3D FieldVector;
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.CosSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void CosSingleTest(double value, double expectedResult, double variance)
        {
            Vector3D actualResult = Vector3D.Cos(Vector3D.Create(value));
            AssertEqual(Vector3D.Create(expectedResult), actualResult, Vector3D.Create(variance));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.ExpSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void ExpSingleTest(double value, double expectedResult, double variance)
        {
            Vector3D actualResult = Vector3D.Exp(Vector3D.Create(value));
            AssertEqual(Vector3D.Create(expectedResult), actualResult, Vector3D.Create(variance));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.LogSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void LogSingleTest(double value, double expectedResult, double variance)
        {
            Vector3D actualResult = Vector3D.Log(Vector3D.Create(value));
            AssertEqual(Vector3D.Create(expectedResult), actualResult, Vector3D.Create(variance));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.Log2Single), MemberType = typeof(GenericMathTestMemberData))]
        public void Log2SingleTest(double value, double expectedResult, double variance)
        {
            Vector3D actualResult = Vector3D.Log2(Vector3D.Create(value));
            AssertEqual(Vector3D.Create(expectedResult), actualResult, Vector3D.Create(variance));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.FusedMultiplyAddSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void FusedMultiplyAddSingleTest(double left, double right, double addend, double expectedResult)
        {
            AssertEqual(Vector3D.Create(expectedResult), Vector3D.FusedMultiplyAdd(Vector3D.Create(left), Vector3D.Create(right), Vector3D.Create(addend)), Vector3D.Zero);
            AssertEqual(Vector3D.Create(double.MultiplyAddEstimate(left, right, addend)), Vector3D.MultiplyAddEstimate(Vector3D.Create(left), Vector3D.Create(right), Vector3D.Create(addend)), Vector3D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.ClampSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void ClampSingleTest(double x, double min, double max, double expectedResult)
        {
            Vector3D actualResult = Vector3D.Clamp(Vector3D.Create(x), Vector3D.Create(min), Vector3D.Create(max));
            AssertEqual(Vector3D.Create(expectedResult), actualResult, Vector3D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.CopySignSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void CopySignSingleTest(double x, double y, double expectedResult)
        {
            Vector3D actualResult = Vector3D.CopySign(Vector3D.Create(x), Vector3D.Create(y));
            AssertEqual(Vector3D.Create(expectedResult), actualResult, Vector3D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.DegreesToRadiansSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void DegreesToRadiansSingleTest(double value, double expectedResult, double variance)
        {
            AssertEqual(Vector3D.Create(-expectedResult), Vector3D.DegreesToRadians(Vector3D.Create(-value)), Vector3D.Create(variance));
            AssertEqual(Vector3D.Create(+expectedResult), Vector3D.DegreesToRadians(Vector3D.Create(+value)), Vector3D.Create(variance));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.HypotSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void HypotSingleTest(double x, double y, double expectedResult, double variance)
        {
            AssertEqual(Vector3D.Create(expectedResult), Vector3D.Hypot(Vector3D.Create(-x), Vector3D.Create(-y)), Vector3D.Create(variance));
            AssertEqual(Vector3D.Create(expectedResult), Vector3D.Hypot(Vector3D.Create(-x), Vector3D.Create(+y)), Vector3D.Create(variance));
            AssertEqual(Vector3D.Create(expectedResult), Vector3D.Hypot(Vector3D.Create(+x), Vector3D.Create(-y)), Vector3D.Create(variance));
            AssertEqual(Vector3D.Create(expectedResult), Vector3D.Hypot(Vector3D.Create(+x), Vector3D.Create(+y)), Vector3D.Create(variance));

            AssertEqual(Vector3D.Create(expectedResult), Vector3D.Hypot(Vector3D.Create(-y), Vector3D.Create(-x)), Vector3D.Create(variance));
            AssertEqual(Vector3D.Create(expectedResult), Vector3D.Hypot(Vector3D.Create(-y), Vector3D.Create(+x)), Vector3D.Create(variance));
            AssertEqual(Vector3D.Create(expectedResult), Vector3D.Hypot(Vector3D.Create(+y), Vector3D.Create(-x)), Vector3D.Create(variance));
            AssertEqual(Vector3D.Create(expectedResult), Vector3D.Hypot(Vector3D.Create(+y), Vector3D.Create(+x)), Vector3D.Create(variance));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.LerpSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void LerpSingleTest(double x, double y, double amount, double expectedResult)
        {
            AssertEqual(Vector3D.Create(+expectedResult), Vector3D.Lerp(Vector3D.Create(+x), Vector3D.Create(+y), Vector3D.Create(amount)), Vector3D.Zero);
            AssertEqual(Vector3D.Create((expectedResult == 0.0d) ? expectedResult : -expectedResult), Vector3D.Lerp(Vector3D.Create(-x), Vector3D.Create(-y), Vector3D.Create(amount)), Vector3D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MaxSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void MaxSingleTest(double x, double y, double expectedResult)
        {
            Vector3D actualResult = Vector3D.Max(Vector3D.Create(x), Vector3D.Create(y));
            AssertEqual(Vector3D.Create(expectedResult), actualResult, Vector3D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MaxMagnitudeSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void MaxMagnitudeSingleTest(double x, double y, double expectedResult)
        {
            Vector3D actualResult = Vector3D.MaxMagnitude(Vector3D.Create(x), Vector3D.Create(y));
            AssertEqual(Vector3D.Create(expectedResult), actualResult, Vector3D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MaxMagnitudeNumberSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void MaxMagnitudeNumberSingleTest(double x, double y, double expectedResult)
        {
            Vector3D actualResult = Vector3D.MaxMagnitudeNumber(Vector3D.Create(x), Vector3D.Create(y));
            AssertEqual(Vector3D.Create(expectedResult), actualResult, Vector3D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MaxNumberSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void MaxNumberSingleTest(double x, double y, double expectedResult)
        {
            Vector3D actualResult = Vector3D.MaxNumber(Vector3D.Create(x), Vector3D.Create(y));
            AssertEqual(Vector3D.Create(expectedResult), actualResult, Vector3D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MinSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void MinSingleTest(double x, double y, double expectedResult)
        {
            Vector3D actualResult = Vector3D.Min(Vector3D.Create(x), Vector3D.Create(y));
            AssertEqual(Vector3D.Create(expectedResult), actualResult, Vector3D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MinMagnitudeSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void MinMagnitudeSingleTest(double x, double y, double expectedResult)
        {
            Vector3D actualResult = Vector3D.MinMagnitude(Vector3D.Create(x), Vector3D.Create(y));
            AssertEqual(Vector3D.Create(expectedResult), actualResult, Vector3D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MinMagnitudeNumberSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void MinMagnitudeNumberSingleTest(double x, double y, double expectedResult)
        {
            Vector3D actualResult = Vector3D.MinMagnitudeNumber(Vector3D.Create(x), Vector3D.Create(y));
            AssertEqual(Vector3D.Create(expectedResult), actualResult, Vector3D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MinNumberSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void MinNumberSingleTest(double x, double y, double expectedResult)
        {
            Vector3D actualResult = Vector3D.MinNumber(Vector3D.Create(x), Vector3D.Create(y));
            AssertEqual(Vector3D.Create(expectedResult), actualResult, Vector3D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.RadiansToDegreesSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void RadiansToDegreesSingleTest(double value, double expectedResult, double variance)
        {
            AssertEqual(Vector3D.Create(-expectedResult), Vector3D.RadiansToDegrees(Vector3D.Create(-value)), Vector3D.Create(variance));
            AssertEqual(Vector3D.Create(+expectedResult), Vector3D.RadiansToDegrees(Vector3D.Create(+value)), Vector3D.Create(variance));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.RoundSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void RoundSingleTest(double value, double expectedResult)
        {
            Vector3D actualResult = Vector3D.Round(Vector3D.Create(value));
            AssertEqual(Vector3D.Create(expectedResult), actualResult, Vector3D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.RoundAwayFromZeroSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void RoundAwayFromZeroSingleTest(double value, double expectedResult)
        {
            Vector3D actualResult = Vector3D.Round(Vector3D.Create(value), MidpointRounding.AwayFromZero);
            AssertEqual(Vector3D.Create(expectedResult), actualResult, Vector3D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.RoundToEvenSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void RoundToEvenSingleTest(double value, double expectedResult)
        {
            Vector3D actualResult = Vector3D.Round(Vector3D.Create(value), MidpointRounding.ToEven);
            AssertEqual(Vector3D.Create(expectedResult), actualResult, Vector3D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.SinSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void SinSingleTest(double value, double expectedResult, double variance)
        {
            Vector3D actualResult = Vector3D.Sin(Vector3D.Create(value));
            AssertEqual(Vector3D.Create(expectedResult), actualResult, Vector3D.Create(variance));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.SinCosSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void SinCosSingleTest(double value, double expectedResultSin, double expectedResultCos, double allowedVarianceSin, double allowedVarianceCos)
        {
            (Vector3D resultSin, Vector3D resultCos) = Vector3D.SinCos(Vector3D.Create(value));
            AssertEqual(Vector3D.Create(expectedResultSin), resultSin, Vector3D.Create(allowedVarianceSin));
            AssertEqual(Vector3D.Create(expectedResultCos), resultCos, Vector3D.Create(allowedVarianceCos));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.TruncateSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void TruncateSingleTest(double value, double expectedResult)
        {
            Vector3D actualResult = Vector3D.Truncate(Vector3D.Create(value));
            AssertEqual(Vector3D.Create(expectedResult), actualResult, Vector3D.Zero);
        }

        [Fact]
        public void AllAnyNoneTest()
        {
            Test(3, 2);

            [MethodImpl(MethodImplOptions.NoInlining)]
            void Test(double value1, double value2)
            {
                var input1 = Vector3D.Create(value1);
                var input2 = Vector3D.Create(value2);

                Assert.True(Vector3D.All(input1, value1));
                Assert.True(Vector3D.All(input2, value2));
                Assert.False(Vector3D.All(input1.WithElement(0, value2), value1));
                Assert.False(Vector3D.All(input2.WithElement(0, value1), value2));
                Assert.False(Vector3D.All(input1, value2));
                Assert.False(Vector3D.All(input2, value1));
                Assert.False(Vector3D.All(input1.WithElement(0, value2), value2));
                Assert.False(Vector3D.All(input2.WithElement(0, value1), value1));

                Assert.True(Vector3D.Any(input1, value1));
                Assert.True(Vector3D.Any(input2, value2));
                Assert.True(Vector3D.Any(input1.WithElement(0, value2), value1));
                Assert.True(Vector3D.Any(input2.WithElement(0, value1), value2));
                Assert.False(Vector3D.Any(input1, value2));
                Assert.False(Vector3D.Any(input2, value1));
                Assert.True(Vector3D.Any(input1.WithElement(0, value2), value2));
                Assert.True(Vector3D.Any(input2.WithElement(0, value1), value1));

                Assert.False(Vector3D.None(input1, value1));
                Assert.False(Vector3D.None(input2, value2));
                Assert.False(Vector3D.None(input1.WithElement(0, value2), value1));
                Assert.False(Vector3D.None(input2.WithElement(0, value1), value2));
                Assert.True(Vector3D.None(input1, value2));
                Assert.True(Vector3D.None(input2, value1));
                Assert.False(Vector3D.None(input1.WithElement(0, value2), value2));
                Assert.False(Vector3D.None(input2.WithElement(0, value1), value1));
            }
        }

        [Fact]
        public void AllAnyNoneTest_AllBitsSet()
        {
            Test(BitConverter.Int32BitsToSingle(-1));

            [MethodImpl(MethodImplOptions.NoInlining)]
            void Test(double value)
            {
                var input = Vector3D.Create(value);

                Assert.False(Vector3D.All(input, value));
                Assert.False(Vector3D.Any(input, value));
                Assert.True(Vector3D.None(input, value));
            }
        }

        [Fact]
        public void AllAnyNoneWhereAllBitsSetTest()
        {
            Test(BitConverter.Int32BitsToSingle(-1), 2);

            [MethodImpl(MethodImplOptions.NoInlining)]
            void Test(double allBitsSet, double value2)
            {
                var input1 = Vector3D.Create(allBitsSet);
                var input2 = Vector3D.Create(value2);

                Assert.True(Vector3D.AllWhereAllBitsSet(input1));
                Assert.False(Vector3D.AllWhereAllBitsSet(input2));
                Assert.False(Vector3D.AllWhereAllBitsSet(input1.WithElement(0, value2)));
                Assert.False(Vector3D.AllWhereAllBitsSet(input2.WithElement(0, allBitsSet)));

                Assert.True(Vector3D.AnyWhereAllBitsSet(input1));
                Assert.False(Vector3D.AnyWhereAllBitsSet(input2));
                Assert.True(Vector3D.AnyWhereAllBitsSet(input1.WithElement(0, value2)));
                Assert.True(Vector3D.AnyWhereAllBitsSet(input2.WithElement(0, allBitsSet)));

                Assert.False(Vector3D.NoneWhereAllBitsSet(input1));
                Assert.True(Vector3D.NoneWhereAllBitsSet(input2));
                Assert.False(Vector3D.NoneWhereAllBitsSet(input1.WithElement(0, value2)));
                Assert.False(Vector3D.NoneWhereAllBitsSet(input2.WithElement(0, allBitsSet)));
            }
        }

        [Fact]
        public void CountIndexOfLastIndexOfSingleTest()
        {
            Test(3, 2);

            [MethodImpl(MethodImplOptions.NoInlining)]
            void Test(double value1, double value2)
            {
                var input1 = Vector3D.Create(value1);
                var input2 = Vector3D.Create(value2);

                Assert.Equal(ElementCount, Vector3D.Count(input1, value1));
                Assert.Equal(ElementCount, Vector3D.Count(input2, value2));
                Assert.Equal(ElementCount - 1, Vector3D.Count(input1.WithElement(0, value2), value1));
                Assert.Equal(ElementCount - 1, Vector3D.Count(input2.WithElement(0, value1), value2));
                Assert.Equal(0, Vector3D.Count(input1, value2));
                Assert.Equal(0, Vector3D.Count(input2, value1));
                Assert.Equal(1, Vector3D.Count(input1.WithElement(0, value2), value2));
                Assert.Equal(1, Vector3D.Count(input2.WithElement(0, value1), value1));

                Assert.Equal(0, Vector3D.IndexOf(input1, value1));
                Assert.Equal(0, Vector3D.IndexOf(input2, value2));
                Assert.Equal(1, Vector3D.IndexOf(input1.WithElement(0, value2), value1));
                Assert.Equal(1, Vector3D.IndexOf(input2.WithElement(0, value1), value2));
                Assert.Equal(-1, Vector3D.IndexOf(input1, value2));
                Assert.Equal(-1, Vector3D.IndexOf(input2, value1));
                Assert.Equal(0, Vector3D.IndexOf(input1.WithElement(0, value2), value2));
                Assert.Equal(0, Vector3D.IndexOf(input2.WithElement(0, value1), value1));

                Assert.Equal(ElementCount - 1, Vector3D.LastIndexOf(input1, value1));
                Assert.Equal(ElementCount - 1, Vector3D.LastIndexOf(input2, value2));
                Assert.Equal(ElementCount - 1, Vector3D.LastIndexOf(input1.WithElement(0, value2), value1));
                Assert.Equal(ElementCount - 1, Vector3D.LastIndexOf(input2.WithElement(0, value1), value2));
                Assert.Equal(-1, Vector3D.LastIndexOf(input1, value2));
                Assert.Equal(-1, Vector3D.LastIndexOf(input2, value1));
                Assert.Equal(0, Vector3D.LastIndexOf(input1.WithElement(0, value2), value2));
                Assert.Equal(0, Vector3D.LastIndexOf(input2.WithElement(0, value1), value1));
            }
        }

        [Fact]
        public void CountIndexOfLastIndexOfSingleTest_AllBitsSet()
        {
            Test(BitConverter.Int32BitsToSingle(-1));

            [MethodImpl(MethodImplOptions.NoInlining)]
            void Test(double value)
            {
                var input = Vector3D.Create(value);

                Assert.Equal(0, Vector3D.Count(input, value));
                Assert.Equal(-1, Vector3D.IndexOf(input, value));
                Assert.Equal(-1, Vector3D.LastIndexOf(input, value));
            }
        }

        [Fact]
        public void CountIndexOfLastIndexOfWhereAllBitsSetSingleTest()
        {
            Test(BitConverter.Int32BitsToSingle(-1), 2);

            [MethodImpl(MethodImplOptions.NoInlining)]
            void Test(double allBitsSet, double value2)
            {
                var input1 = Vector3D.Create(allBitsSet);
                var input2 = Vector3D.Create(value2);

                Assert.Equal(ElementCount, Vector3D.CountWhereAllBitsSet(input1));
                Assert.Equal(0, Vector3D.CountWhereAllBitsSet(input2));
                Assert.Equal(ElementCount - 1, Vector3D.CountWhereAllBitsSet(input1.WithElement(0, value2)));
                Assert.Equal(1, Vector3D.CountWhereAllBitsSet(input2.WithElement(0, allBitsSet)));

                Assert.Equal(0, Vector3D.IndexOfWhereAllBitsSet(input1));
                Assert.Equal(-1, Vector3D.IndexOfWhereAllBitsSet(input2));
                Assert.Equal(1, Vector3D.IndexOfWhereAllBitsSet(input1.WithElement(0, value2)));
                Assert.Equal(0, Vector3D.IndexOfWhereAllBitsSet(input2.WithElement(0, allBitsSet)));

                Assert.Equal(ElementCount - 1, Vector3D.LastIndexOfWhereAllBitsSet(input1));
                Assert.Equal(-1, Vector3D.LastIndexOfWhereAllBitsSet(input2));
                Assert.Equal(ElementCount - 1, Vector3D.LastIndexOfWhereAllBitsSet(input1.WithElement(0, value2)));
                Assert.Equal(0, Vector3D.LastIndexOfWhereAllBitsSet(input2.WithElement(0, allBitsSet)));
            }
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsEvenIntegerTest(double value) => Assert.Equal(double.IsEvenInteger(value) ? Vector3D.AllBitsSet : Vector3D.Zero, Vector3D.IsEvenInteger(Vector3D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsFiniteTest(double value) => Assert.Equal(double.IsFinite(value) ? Vector3D.AllBitsSet : Vector3D.Zero, Vector3D.IsFinite(Vector3D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsInfinityTest(double value) => Assert.Equal(double.IsInfinity(value) ? Vector3D.AllBitsSet : Vector3D.Zero, Vector3D.IsInfinity(Vector3D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsIntegerTest(double value) => Assert.Equal(double.IsInteger(value) ? Vector3D.AllBitsSet : Vector3D.Zero, Vector3D.IsInteger(Vector3D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsNaNTest(double value) => Assert.Equal(double.IsNaN(value) ? Vector3D.AllBitsSet : Vector3D.Zero, Vector3D.IsNaN(Vector3D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsNegativeTest(double value) => Assert.Equal(double.IsNegative(value) ? Vector3D.AllBitsSet : Vector3D.Zero, Vector3D.IsNegative(Vector3D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsNegativeInfinityTest(double value) => Assert.Equal(double.IsNegativeInfinity(value) ? Vector3D.AllBitsSet : Vector3D.Zero, Vector3D.IsNegativeInfinity(Vector3D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsNormalTest(double value) => Assert.Equal(double.IsNormal(value) ? Vector3D.AllBitsSet : Vector3D.Zero, Vector3D.IsNormal(Vector3D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsOddIntegerTest(double value) => Assert.Equal(double.IsOddInteger(value) ? Vector3D.AllBitsSet : Vector3D.Zero, Vector3D.IsOddInteger(Vector3D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsPositiveTest(double value) => Assert.Equal(double.IsPositive(value) ? Vector3D.AllBitsSet : Vector3D.Zero, Vector3D.IsPositive(Vector3D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsPositiveInfinityTest(double value) => Assert.Equal(double.IsPositiveInfinity(value) ? Vector3D.AllBitsSet : Vector3D.Zero, Vector3D.IsPositiveInfinity(Vector3D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsSubnormalTest(double value) => Assert.Equal(double.IsSubnormal(value) ? Vector3D.AllBitsSet : Vector3D.Zero, Vector3D.IsSubnormal(Vector3D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsZeroSingleTest(double value) => Assert.Equal((value == 0) ? Vector3D.AllBitsSet : Vector3D.Zero, Vector3D.IsZero(Vector3D.Create(value)));

        [Fact]
        public void AllBitsSetTest()
        {
            Assert.Equal(-1, BitConverter.DoubleToInt64Bits(Vector3D.AllBitsSet.X));
            Assert.Equal(-1, BitConverter.DoubleToInt64Bits(Vector3D.AllBitsSet.Y));
            Assert.Equal(-1, BitConverter.DoubleToInt64Bits(Vector3D.AllBitsSet.Z));
        }

        [Fact]
        public void ConditionalSelectTest()
        {
            Test(Vector3D.Create(1, 2, 3), Vector3D.AllBitsSet, Vector3D.Create(1, 2, 3), Vector3D.Create(5, 6, 7));
            Test(Vector3D.Create(5, 6, 7), Vector3D.Zero, Vector3D.Create(1, 2, 3), Vector3D.Create(5, 6, 7));
            Test(Vector3D.Create(1, 6, 3), Vector256.Create(-1, 0, -1, 0).AsSingle().AsVector3D(), Vector3D.Create(1, 2, 3), Vector3D.Create(5, 6, 7));

            [MethodImpl(MethodImplOptions.NoInlining)]
            void Test(Vector3D expectedResult, Vector3D condition, Vector3D left, Vector3D right)
            {
                Assert.Equal(expectedResult, Vector3D.ConditionalSelect(condition, left, right));
            }
        }

        [Theory]
        [InlineData(+0.0d, +0.0d, +0.0d, 0b000)]
        [InlineData(-0.0d, +1.0d, -0.0d, 0b101)]
        [InlineData(-0.0d, -0.0d, -0.0d, 0b111)]
        public void ExtractMostSignificantBitsTest(double x, double y, double z, uint expectedResult)
        {
            Assert.Equal(expectedResult, Vector3D.Create(x, y, z).ExtractMostSignificantBits());
        }

        [Theory]
        [InlineData(1.0d, 2.0d, 3.0d)]
        [InlineData(5.0d, 6.0d, 7.0d)]
        public void GetElementTest(double x, double y, double z)
        {
            Assert.Equal(x, Vector3D.Create(x, y, z).GetElement(0));
            Assert.Equal(y, Vector3D.Create(x, y, z).GetElement(1));
            Assert.Equal(z, Vector3D.Create(x, y, z).GetElement(2));
        }

        [Theory]
        [InlineData(1.0d, 2.0d, 3.0d)]
        [InlineData(5.0d, 6.0d, 7.0d)]
        public void ShuffleTest(double x, double y, double z)
        {
            Assert.Equal(Vector3D.Create(z, y, x), Vector3D.Shuffle(Vector3D.Create(x, y, z), 2, 1, 0));
            Assert.Equal(Vector3D.Create(y, x, z), Vector3D.Shuffle(Vector3D.Create(x, y, z), 1, 0, 2));
        }

        [Theory]
        [InlineData(1.0d, 2.0d, 3.0d, 6.0d)]
        [InlineData(5.0d, 6.0d, 7.0d, 18.0d)]
        public void SumTest(double x, double y, double z, double expectedResult)
        {
            Assert.Equal(expectedResult, Vector3D.Sum(Vector3D.Create(x, y, z)));
        }

        [Theory]
        [InlineData(1.0d, 2.0d, 3.0d)]
        [InlineData(5.0d, 6.0d, 7.0d)]
        public void ToScalarTest(double x, double y, double z)
        {
            Assert.Equal(x, Vector3D.Create(x, y, z).ToScalar());
        }

        [Theory]
        [InlineData(1.0d, 2.0d, 3.0d)]
        [InlineData(5.0d, 6.0d, 7.0d)]
        public void WithElementTest(double x, double y, double z)
        {
            var vector = Vector3D.Create(10);

            Assert.Equal(10, vector.X);
            Assert.Equal(10, vector.Y);
            Assert.Equal(10, vector.Z);

            vector = vector.WithElement(0, x);

            Assert.Equal(x, vector.X);
            Assert.Equal(10, vector.Y);
            Assert.Equal(10, vector.Z);

            vector = vector.WithElement(1, y);

            Assert.Equal(x, vector.X);
            Assert.Equal(y, vector.Y);
            Assert.Equal(10, vector.Z);

            vector = vector.WithElement(2, z);

            Assert.Equal(x, vector.X);
            Assert.Equal(y, vector.Y);
            Assert.Equal(z, vector.Z);
        }

        [Theory]
        [InlineData(1.0d, 2.0d, 3.0d)]
        [InlineData(5.0d, 6.0d, 7.0d)]
        public void AsVector2DTest(double x, double y, double z)
        {
            var vector = Vector3D.Create(x, y, z).AsVector2D();

            Assert.Equal(x, vector.X);
            Assert.Equal(y, vector.Y);
        }

        [Fact]
        public void CreateScalarTest()
        {
            var vector = Vector3D.CreateScalar(double.Pi);

            Assert.Equal(double.Pi, vector.X);
            Assert.Equal(0, vector.Y);
            Assert.Equal(0, vector.Z);

            vector = Vector3D.CreateScalar(double.E);

            Assert.Equal(double.E, vector.X);
            Assert.Equal(0, vector.Y);
            Assert.Equal(0, vector.Z);
        }

        [Fact]
        public void CreateScalarUnsafeTest()
        {
            var vector = Vector3D.CreateScalarUnsafe(double.Pi);
            Assert.Equal(double.Pi, vector.X);

            vector = Vector3D.CreateScalarUnsafe(double.E);
            Assert.Equal(double.E, vector.X);
        }
    }
}
