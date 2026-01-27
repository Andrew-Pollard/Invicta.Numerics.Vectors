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
    public sealed class Vector2DDTests
    {
        private const int ElementCount = 2;

        /// <summary>Verifies that two <see cref="Vector2D" /> values are equal, within the <paramref name="variance" />.</summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The value to be compared against</param>
        /// <param name="variance">The total variance allowed between the expected and actual results.</param>
        /// <exception cref="EqualException">Thrown when the values are not equal</exception>
        internal static void AssertEqual(Vector2D expected, Vector2D actual, Vector2D variance)
        {
            AssertExtensions.Equal(expected.X, actual.X, variance.X);
            AssertExtensions.Equal(expected.Y, actual.Y, variance.Y);
        }

        [Fact]
        public void Vector2DMarshalSizeTest()
        {
            Assert.Equal(8, Marshal.SizeOf<Vector2D>());
            Assert.Equal(8, Marshal.SizeOf<Vector2D>(new Vector2D()));
        }

        [Theory]
        [InlineData(0.0d, 1.0d)]
        [InlineData(1.0d, 0.0d)]
        [InlineData(3.1434343d, 1.1234123d)]
        [InlineData(1.0000001d, 0.0000001d)]
        public void Vector2DIndexerGetTest(double x, double y)
        {
            var vector = new Vector2D(x, y);

            Assert.Equal(x, vector[0]);
            Assert.Equal(y, vector[1]);
        }

        [Theory]
        [InlineData(0.0d, 1.0d)]
        [InlineData(1.0d, 0.0d)]
        [InlineData(3.1434343d, 1.1234123d)]
        [InlineData(1.0000001d, 0.0000001d)]
        public void Vector2DIndexerSetTest(double x, double y)
        {
            var vector = new Vector2D(0.0d, 0.0d);

            vector[0] = x;
            vector[1] = y;

            Assert.Equal(x, vector[0]);
            Assert.Equal(y, vector[1]);
        }

        [Fact]
        public void Vector2DCopyToTest()
        {
            Vector2D v1 = new Vector2D(2.0d, 3.0d);

            double[] a = new double[3];
            double[] b = new double[2];

            Assert.Throws<NullReferenceException>(() => v1.CopyTo(null, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => v1.CopyTo(a, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => v1.CopyTo(a, a.Length));
            Assert.Throws<ArgumentException>(() => v1.CopyTo(a, 2));

            v1.CopyTo(a, 1);
            v1.CopyTo(b);
            Assert.Equal(0.0, a[0]);
            Assert.Equal(2.0, a[1]);
            Assert.Equal(3.0, a[2]);
            Assert.Equal(2.0, b[0]);
            Assert.Equal(3.0, b[1]);
        }

        [Fact]
        public void Vector2DCopyToSpanTest()
        {
            Vector2D vector = new Vector2D(1.0d, 2.0d);
            Span<double> destination = new double[2];

            Assert.Throws<ArgumentException>(() => vector.CopyTo(new Span<double>(new double[1])));
            vector.CopyTo(destination);

            Assert.Equal(1.0d, vector.X);
            Assert.Equal(2.0d, vector.Y);
            Assert.Equal(vector.X, destination[0]);
            Assert.Equal(vector.Y, destination[1]);
        }

        [Fact]
        public void Vector2DTryCopyToTest()
        {
            Vector2D vector = new Vector2D(1.0d, 2.0d);
            Span<double> destination = new double[2];

            Assert.False(vector.TryCopyTo(new Span<double>(new double[1])));
            Assert.True(vector.TryCopyTo(destination));

            Assert.Equal(1.0d, vector.X);
            Assert.Equal(2.0d, vector.Y);
            Assert.Equal(vector.X, destination[0]);
            Assert.Equal(vector.Y, destination[1]);
        }

        [Fact]
        public void Vector2DGetHashCodeTest()
        {
            Vector2D v1 = new Vector2D(2.0d, 3.0d);
            Vector2D v2 = new Vector2D(2.0d, 3.0d);
            Vector2D v3 = new Vector2D(3.0d, 2.0d);
            Assert.Equal(v1.GetHashCode(), v1.GetHashCode());
            Assert.Equal(v1.GetHashCode(), v2.GetHashCode());
            Assert.NotEqual(v1.GetHashCode(), v3.GetHashCode());
            Vector2D v4 = new Vector2D(0.0d, 0.0d);
            Vector2D v6 = new Vector2D(1.0d, 0.0d);
            Vector2D v7 = new Vector2D(0.0d, 1.0d);
            Vector2D v8 = new Vector2D(1.0d, 1.0d);
            Assert.NotEqual(v4.GetHashCode(), v6.GetHashCode());
            Assert.NotEqual(v4.GetHashCode(), v7.GetHashCode());
            Assert.NotEqual(v4.GetHashCode(), v8.GetHashCode());
            Assert.NotEqual(v7.GetHashCode(), v6.GetHashCode());
            Assert.NotEqual(v8.GetHashCode(), v6.GetHashCode());
            Assert.NotEqual(v8.GetHashCode(), v7.GetHashCode());
        }

        [Fact]
        public void Vector2DToStringTest()
        {
            string separator = CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator;
            CultureInfo enUsCultureInfo = new CultureInfo("en-US");

            Vector2D v1 = new Vector2D(2.0d, 3.0d);

            string v1str = v1.ToString();
            string expectedv1 = string.Format(CultureInfo.CurrentCulture
                , "<{1:G}{0} {2:G}>"
                , new object[] { separator, 2, 3 });
            Assert.Equal(expectedv1, v1str);

            string v1strformatted = v1.ToString("c", CultureInfo.CurrentCulture);
            string expectedv1dormatted = string.Format(CultureInfo.CurrentCulture
                , "<{1:c}{0} {2:c}>"
                , new object[] { separator, 2, 3 });
            Assert.Equal(expectedv1dormatted, v1strformatted);

            string v2strformatted = v1.ToString("c", enUsCultureInfo);
            string expectedv2dormatted = string.Format(enUsCultureInfo
                , "<{1:c}{0} {2:c}>"
                , new object[] { enUsCultureInfo.NumberFormat.NumberGroupSeparator, 2, 3 });
            Assert.Equal(expectedv2dormatted, v2strformatted);

            string v3strformatted = v1.ToString("c");
            string expectedv3dormatted = string.Format(CultureInfo.CurrentCulture
                , "<{1:c}{0} {2:c}>"
                , new object[] { separator, 2, 3 });
            Assert.Equal(expectedv3dormatted, v3strformatted);
        }

        // A test for Distance (Vector2Df, Vector2Df)
        [Fact]
        public void Vector2DDistanceTest()
        {
            Vector2D a = new Vector2D(1.0d, 2.0d);
            Vector2D b = new Vector2D(3.0d, 4.0d);

            double expected = (double)System.Math.Sqrt(8);
            double actual;

            actual = Vector2D.Distance(a, b);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Distance did not return the expected value.");
        }

        // A test for Distance (Vector2Df, Vector2Df)
        // Distance from the same point
        [Fact]
        public void Vector2DDistanceTest2()
        {
            Vector2D a = new Vector2D(1.051d, 2.05d);
            Vector2D b = new Vector2D(1.051d, 2.05d);

            double actual = Vector2D.Distance(a, b);
            Assert.Equal(0.0d, actual);
        }

        // A test for DistanceSquared (Vector2Df, Vector2Df)
        [Fact]
        public void Vector2DDistanceSquaredTest()
        {
            Vector2D a = new Vector2D(1.0d, 2.0d);
            Vector2D b = new Vector2D(3.0d, 4.0d);

            double expected = 8.0d;
            double actual;

            actual = Vector2D.DistanceSquared(a, b);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.DistanceSquared did not return the expected value.");
        }

        // A test for Dot (Vector2Df, Vector2Df)
        [Fact]
        public void Vector2DDotTest()
        {
            Vector2D a = new Vector2D(1.0d, 2.0d);
            Vector2D b = new Vector2D(3.0d, 4.0d);

            double expected = 11.0d;
            double actual;

            actual = Vector2D.Dot(a, b);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Dot did not return the expected value.");
        }

        // A test for Dot (Vector2Df, Vector2Df)
        // Dot test for perpendicular vector
        [Fact]
        public void Vector2DDotTest1()
        {
            Vector2D a = new Vector2D(1.55d, 1.55d);
            Vector2D b = new Vector2D(-1.55d, 1.55d);

            double expected = 0.0d;
            double actual = Vector2D.Dot(a, b);
            Assert.Equal(expected, actual);
        }

        // A test for Dot (Vector2Df, Vector2Df)
        // Dot test with specail double values
        [Fact]
        public void Vector2DDotTest2()
        {
            Vector2D a = new Vector2D(double.MinValue, double.MinValue);
            Vector2D b = new Vector2D(double.MaxValue, double.MaxValue);

            double actual = Vector2D.Dot(a, b);
            Assert.True(double.IsNegativeInfinity(actual), "Vector2Df.Dot did not return the expected value.");
        }

        [Fact]
        public void Vector2DCrossTest()
        {
            Vector2D a = new Vector2D(1.0d, 2.0d);
            Vector2D b = new Vector2D(-4.0d, 3.0d);

            double expected = 11.0d;
            double actual = Vector2D.Cross(a, b);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Cross did not return the expected value.");
        }

        [Fact]
        public void Vector2DCrossTest1()
        {
            // Cross test for parallel vector
            Vector2D a = new Vector2D(1.55d, 1.55d);
            Vector2D b = new Vector2D(-1.55d, -1.55d);

            double expected = 0.0d;
            double actual = Vector2D.Cross(a, b);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Vector2DCrossTest2()
        {
            // Cross test with specail double values
            Vector2D a = new Vector2D(double.MinValue, double.MinValue);
            Vector2D b = new Vector2D(double.MinValue, double.MaxValue);

            double actual = Vector2D.Cross(a, b);
            Assert.True(double.IsNegativeInfinity(actual), "Vector2Df.Cross did not return the expected value.");
        }

        // A test for Length ()
        [Fact]
        public void Vector2DLengthTest()
        {
            Vector2D a = new Vector2D(2.0d, 4.0d);

            Vector2D target = a;

            double expected = (double)System.Math.Sqrt(20);
            double actual;

            actual = target.Length();

            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Length did not return the expected value.");
        }

        // A test for Length ()
        // Length test where length is zero
        [Fact]
        public void Vector2DLengthTest1()
        {
            Vector2D target = new Vector2D();
            target.X = 0.0d;
            target.Y = 0.0d;

            double expected = 0.0d;
            double actual;

            actual = target.Length();

            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Length did not return the expected value.");
        }

        // A test for LengthSquared ()
        [Fact]
        public void Vector2DLengthSquaredTest()
        {
            Vector2D a = new Vector2D(2.0d, 4.0d);

            Vector2D target = a;

            double expected = 20.0d;
            double actual;

            actual = target.LengthSquared();

            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.LengthSquared did not return the expected value.");
        }

        // A test for LengthSquared ()
        // LengthSquared test where the result is zero
        [Fact]
        public void Vector2DLengthSquaredTest1()
        {
            Vector2D a = new Vector2D(0.0d, 0.0d);

            double expected = 0.0d;
            double actual = a.LengthSquared();

            Assert.Equal(expected, actual);
        }

        // A test for Min (Vector2Df, Vector2Df)
        [Fact]
        public void Vector2DMinTest()
        {
            Vector2D a = new Vector2D(-1.0d, 4.0d);
            Vector2D b = new Vector2D(2.0d, 1.0d);

            Vector2D expected = new Vector2D(-1.0d, 1.0d);
            Vector2D actual;
            actual = Vector2D.Min(a, b);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Min did not return the expected value.");
        }

        [Fact]
        public void Vector2DMinMaxCodeCoverageTest()
        {
            Vector2D min = new Vector2D(0, 0);
            Vector2D max = new Vector2D(1, 1);
            Vector2D actual;

            // Min.
            actual = Vector2D.Min(min, max);
            Assert.Equal(actual, min);

            actual = Vector2D.Min(max, min);
            Assert.Equal(actual, min);

            // Max.
            actual = Vector2D.Max(min, max);
            Assert.Equal(actual, max);

            actual = Vector2D.Max(max, min);
            Assert.Equal(actual, max);
        }

        // A test for Max (Vector2Df, Vector2Df)
        [Fact]
        public void Vector2DMaxTest()
        {
            Vector2D a = new Vector2D(-1.0d, 4.0d);
            Vector2D b = new Vector2D(2.0d, 1.0d);

            Vector2D expected = new Vector2D(2.0d, 4.0d);
            Vector2D actual;
            actual = Vector2D.Max(a, b);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Max did not return the expected value.");
        }

        // A test for Clamp (Vector2Df, Vector2Df, Vector2Df)
        [Fact]
        public void Vector2DClampTest()
        {
            Vector2D a = new Vector2D(0.5d, 0.3d);
            Vector2D min = new Vector2D(0.0d, 0.1d);
            Vector2D max = new Vector2D(1.0d, 1.1d);

            // Normal case.
            // Case N1: specified value is in the range.
            Vector2D expected = new Vector2D(0.5d, 0.3d);
            Vector2D actual = Vector2D.Clamp(a, min, max);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Clamp did not return the expected value.");
            // Normal case.
            // Case N2: specified value is bigger than max value.
            a = new Vector2D(2.0d, 3.0d);
            expected = max;
            actual = Vector2D.Clamp(a, min, max);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Clamp did not return the expected value.");
            // Case N3: specified value is smaller than max value.
            a = new Vector2D(-1.0d, -2.0d);
            expected = min;
            actual = Vector2D.Clamp(a, min, max);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Clamp did not return the expected value.");
            // Case N4: combination case.
            a = new Vector2D(-2.0d, 4.0d);
            expected = new Vector2D(min.X, max.Y);
            actual = Vector2D.Clamp(a, min, max);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Clamp did not return the expected value.");
            // User specified min value is bigger than max value.
            max = new Vector2D(0.0d, 0.1d);
            min = new Vector2D(1.0d, 1.1d);

            // Case W1: specified value is in the range.
            a = new Vector2D(0.5d, 0.3d);
            expected = max;
            actual = Vector2D.Clamp(a, min, max);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Clamp did not return the expected value.");

            // Normal case.
            // Case W2: specified value is bigger than max and min value.
            a = new Vector2D(2.0d, 3.0d);
            expected = max;
            actual = Vector2D.Clamp(a, min, max);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Clamp did not return the expected value.");

            // Case W3: specified value is smaller than min and max value.
            a = new Vector2D(-1.0d, -2.0d);
            expected = max;
            actual = Vector2D.Clamp(a, min, max);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Clamp did not return the expected value.");
        }

        // A test for Lerp (Vector2Df, Vector2Df, double)
        [Fact]
        public void Vector2DLerpTest()
        {
            Vector2D a = new Vector2D(1.0d, 2.0d);
            Vector2D b = new Vector2D(3.0d, 4.0d);

            double t = 0.5d;

            Vector2D expected = new Vector2D(2.0d, 3.0d);
            Vector2D actual;
            actual = Vector2D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector2Df, Vector2Df, double)
        // Lerp test with factor zero
        [Fact]
        public void Vector2DLerpTest1()
        {
            Vector2D a = new Vector2D(0.0d, 0.0d);
            Vector2D b = new Vector2D(3.18d, 4.25d);

            double t = 0.0d;
            Vector2D expected = Vector2D.Zero;
            Vector2D actual = Vector2D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector2Df, Vector2Df, double)
        // Lerp test with factor one
        [Fact]
        public void Vector2DLerpTest2()
        {
            Vector2D a = new Vector2D(0.0d, 0.0d);
            Vector2D b = new Vector2D(3.18d, 4.25d);

            double t = 1.0d;
            Vector2D expected = new Vector2D(3.18d, 4.25d);
            Vector2D actual = Vector2D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector2Df, Vector2Df, double)
        // Lerp test with factor > 1
        [Fact]
        public void Vector2DLerpTest3()
        {
            Vector2D a = new Vector2D(0.0d, 0.0d);
            Vector2D b = new Vector2D(3.18d, 4.25d);

            double t = 2.0d;
            Vector2D expected = b * 2.0d;
            Vector2D actual = Vector2D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector2Df, Vector2Df, double)
        // Lerp test with factor < 0
        [Fact]
        public void Vector2DLerpTest4()
        {
            Vector2D a = new Vector2D(0.0d, 0.0d);
            Vector2D b = new Vector2D(3.18d, 4.25d);

            double t = -2.0d;
            Vector2D expected = -(b * 2.0d);
            Vector2D actual = Vector2D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector2Df, Vector2Df, double)
        // Lerp test with special double value
        [Fact]
        public void Vector2DLerpTest5()
        {
            Vector2D a = new Vector2D(45.67d, 90.0d);
            Vector2D b = new Vector2D(double.PositiveInfinity, double.NegativeInfinity);

            double t = 0.408d;
            Vector2D actual = Vector2D.Lerp(a, b, t);
            Assert.True(double.IsPositiveInfinity(actual.X), "Vector2Df.Lerp did not return the expected value.");
            Assert.True(double.IsNegativeInfinity(actual.Y), "Vector2Df.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector2Df, Vector2Df, double)
        // Lerp test from the same point
        [Fact]
        public void Vector2DLerpTest6()
        {
            Vector2D a = new Vector2D(1.0d, 2.0d);
            Vector2D b = new Vector2D(1.0d, 2.0d);

            double t = 0.5d;

            Vector2D expected = new Vector2D(1.0d, 2.0d);
            Vector2D actual = Vector2D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector2Df, Vector2Df, double)
        // Lerp test with values known to be inaccurate with the old lerp impl
        [Fact]
        public void Vector2DLerpTest7()
        {
            Vector2D a = new Vector2D(0.44728136d);
            Vector2D b = new Vector2D(0.46345946d);

            double t = 0.26402435d;

            Vector2D expected = new Vector2D(0.45155275d);
            Vector2D actual = Vector2D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector2Df, Vector2Df, double)
        // Lerp test with values known to be inaccurate with the old lerp impl
        // (Old code incorrectly gets 0.33333588)
        [Fact]
        public void Vector2DLerpTest8()
        {
            Vector2D a = new Vector2D(-100);
            Vector2D b = new Vector2D(0.33333334d);

            double t = 1d;

            Vector2D expected = new Vector2D(0.33333334d);
            Vector2D actual = Vector2D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Lerp did not return the expected value.");
        }

        // A test for Transform(Vector2Df, Matrix4x4D)
        [Fact]
        public void Vector2DTransformTest()
        {
            Vector2D v = new Vector2D(1.0d, 2.0d);
            Matrix4x4D m =
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0d)) *
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0d)) *
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0d));
            m.M41 = 10.0d;
            m.M42 = 20.0d;
            m.M43 = 30.0d;

            Vector2D expected = new Vector2D(10.316987d, 22.183012d);
            Vector2D actual;

            actual = Vector2D.Transform(v, m);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Transform did not return the expected value.");
        }

        // A test for Transform(Vector2Df, Matrix3x2D)
        [Fact]
        public void Vector2DTransform3x2Test()
        {
            Vector2D v = new Vector2D(1.0d, 2.0d);
            Matrix3x2D m = Matrix3x2D.CreateRotation(MathHelper.ToRadians(30.0d));
            m.M31 = 10.0d;
            m.M32 = 20.0d;

            Vector2D expected = new Vector2D(9.866025d, 22.23205d);
            Vector2D actual;

            actual = Vector2D.Transform(v, m);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Transform did not return the expected value.");
        }

        // A test for TransformNormal (Vector2Df, Matrix4x4D)
        [Fact]
        public void Vector2DTransformNormalTest()
        {
            Vector2D v = new Vector2D(1.0d, 2.0d);
            Matrix4x4D m =
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0d)) *
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0d)) *
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0d));
            m.M41 = 10.0d;
            m.M42 = 20.0d;
            m.M43 = 30.0d;

            Vector2D expected = new Vector2D(0.3169873d, 2.18301272d);
            Vector2D actual;

            actual = Vector2D.TransformNormal(v, m);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Transform did not return the expected value.");
        }

        // A test for TransformNormal (Vector2Df, Matrix3x2D)
        [Fact]
        public void Vector2DTransformNormal3x2Test()
        {
            Vector2D v = new Vector2D(1.0d, 2.0d);
            Matrix3x2D m = Matrix3x2D.CreateRotation(MathHelper.ToRadians(30.0d));
            m.M31 = 10.0d;
            m.M32 = 20.0d;

            Vector2D expected = new Vector2D(-0.133974612d, 2.232051d);
            Vector2D actual;

            actual = Vector2D.TransformNormal(v, m);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Transform did not return the expected value.");
        }

        // A test for Transform (Vector2Df, QuaternionD)
        [Fact]
        public void Vector2DTransformByQuaternionDTest()
        {
            Vector2D v = new Vector2D(1.0d, 2.0d);

            Matrix4x4D m =
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0d)) *
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0d)) *
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0d));
            QuaternionD q = QuaternionD.CreateFromRotationMatrix(m);

            Vector2D expected = Vector2D.Transform(v, m);
            Vector2D actual = Vector2D.Transform(v, q);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Transform did not return the expected value.");
        }

        // A test for Transform (Vector2Df, QuaternionD)
        // Transform Vector2Df with zero quaternion
        [Fact]
        public void Vector2DTransformByQuaternionDTest1()
        {
            Vector2D v = new Vector2D(1.0d, 2.0d);
            QuaternionD q = new QuaternionD();
            Vector2D expected = Vector2D.Zero;

            Vector2D actual = Vector2D.Transform(v, q);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Transform did not return the expected value.");
        }

        // A test for Transform (Vector2Df, QuaternionD)
        // Transform Vector2Df with identity quaternion
        [Fact]
        public void Vector2DTransformByQuaternionDTest2()
        {
            Vector2D v = new Vector2D(1.0d, 2.0d);
            QuaternionD q = QuaternionD.Identity;
            Vector2D expected = v;

            Vector2D actual = Vector2D.Transform(v, q);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Transform did not return the expected value.");
        }

        // A test for Normalize (Vector2Df)
        [Fact]
        public void Vector2DNormalizeTest()
        {
            Vector2D a = new Vector2D(2.0d, 3.0d);
            Vector2D expected = new Vector2D(0.554700196225229122018341733457d, 0.8320502943378436830275126001855d);
            Vector2D actual;

            actual = Vector2D.Normalize(a);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Normalize did not return the expected value.");
        }

        // A test for Normalize (Vector2Df)
        // Normalize zero length vector
        [Fact]
        public void Vector2DNormalizeTest1()
        {
            Vector2D a = new Vector2D(); // no parameter, default to 0.0d
            Vector2D actual = Vector2D.Normalize(a);
            Assert.True(double.IsNaN(actual.X) && double.IsNaN(actual.Y), "Vector2Df.Normalize did not return the expected value.");
        }

        // A test for Normalize (Vector2Df)
        // Normalize infinite length vector
        [Fact]
        public void Vector2DNormalizeTest2()
        {
            Vector2D a = new Vector2D(double.MaxValue, double.MaxValue);
            Vector2D actual = Vector2D.Normalize(a);
            Vector2D expected = new Vector2D(0, 0);
            Assert.Equal(expected, actual);
        }

        // A test for operator - (Vector2Df)
        [Fact]
        public void Vector2DUnaryNegationTest()
        {
            Vector2D a = new Vector2D(1.0d, 2.0d);

            Vector2D expected = new Vector2D(-1.0d, -2.0d);
            Vector2D actual;

            actual = -a;

            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.operator - did not return the expected value.");
        }



        // A test for operator - (Vector2Df)
        // Negate test with special double value
        [Fact]
        public void Vector2DUnaryNegationTest1()
        {
            Vector2D a = new Vector2D(double.PositiveInfinity, double.NegativeInfinity);

            Vector2D actual = -a;

            Assert.True(double.IsNegativeInfinity(actual.X), "Vector2Df.operator - did not return the expected value.");
            Assert.True(double.IsPositiveInfinity(actual.Y), "Vector2Df.operator - did not return the expected value.");
        }

        // A test for operator - (Vector2Df)
        // Negate test with special double value
        [Fact]
        public void Vector2DUnaryNegationTest2()
        {
            Vector2D a = new Vector2D(double.NaN, 0.0d);
            Vector2D actual = -a;

            Assert.True(double.IsNaN(actual.X), "Vector2Df.operator - did not return the expected value.");
            Assert.True(double.Equals(0.0d, actual.Y), "Vector2Df.operator - did not return the expected value.");
        }

        // A test for operator - (Vector2Df, Vector2Df)
        [Fact]
        public void Vector2DSubtractionTest()
        {
            Vector2D a = new Vector2D(1.0d, 3.0d);
            Vector2D b = new Vector2D(2.0d, 1.5d);

            Vector2D expected = new Vector2D(-1.0d, 1.5d);
            Vector2D actual;

            actual = a - b;

            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.operator - did not return the expected value.");
        }

        // A test for operator * (Vector2Df, double)
        [Fact]
        public void Vector2DMultiplyOperatorTest()
        {
            Vector2D a = new Vector2D(2.0d, 3.0d);
            const double factor = 2.0d;

            Vector2D expected = new Vector2D(4.0d, 6.0d);
            Vector2D actual;

            actual = a * factor;
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.operator * did not return the expected value.");
        }

        // A test for operator * (double, Vector2Df)
        [Fact]
        public void Vector2DMultiplyOperatorTest2()
        {
            Vector2D a = new Vector2D(2.0d, 3.0d);
            const double factor = 2.0d;

            Vector2D expected = new Vector2D(4.0d, 6.0d);
            Vector2D actual;

            actual = factor * a;
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.operator * did not return the expected value.");
        }

        // A test for operator * (Vector2Df, Vector2Df)
        [Fact]
        public void Vector2DMultiplyOperatorTest3()
        {
            Vector2D a = new Vector2D(2.0d, 3.0d);
            Vector2D b = new Vector2D(4.0d, 5.0d);

            Vector2D expected = new Vector2D(8.0d, 15.0d);
            Vector2D actual;

            actual = a * b;

            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.operator * did not return the expected value.");
        }

        // A test for operator / (Vector2Df, double)
        [Fact]
        public void Vector2DDivisionTest()
        {
            Vector2D a = new Vector2D(2.0d, 3.0d);

            double div = 2.0d;

            Vector2D expected = new Vector2D(1.0d, 1.5d);
            Vector2D actual;

            actual = a / div;

            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.operator / did not return the expected value.");
        }

        // A test for operator / (Vector2Df, Vector2Df)
        [Fact]
        public void Vector2DDivisionTest1()
        {
            Vector2D a = new Vector2D(2.0d, 3.0d);
            Vector2D b = new Vector2D(4.0d, 5.0d);

            Vector2D expected = new Vector2D(2.0d / 4.0d, 3.0d / 5.0d);
            Vector2D actual;

            actual = a / b;

            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.operator / did not return the expected value.");
        }

        // A test for operator / (Vector2Df, double)
        // Divide by zero
        [Fact]
        public void Vector2DDivisionTest2()
        {
            Vector2D a = new Vector2D(-2.0d, 3.0d);

            double div = 0.0d;

            Vector2D actual = a / div;

            Assert.True(double.IsNegativeInfinity(actual.X), "Vector2Df.operator / did not return the expected value.");
            Assert.True(double.IsPositiveInfinity(actual.Y), "Vector2Df.operator / did not return the expected value.");
        }

        // A test for operator / (Vector2Df, Vector2Df)
        // Divide by zero
        [Fact]
        public void Vector2DDivisionTest3()
        {
            Vector2D a = new Vector2D(0.047d, -3.0d);
            Vector2D b = new Vector2D();

            Vector2D actual = a / b;

            Assert.True(double.IsInfinity(actual.X), "Vector2Df.operator / did not return the expected value.");
            Assert.True(double.IsInfinity(actual.Y), "Vector2Df.operator / did not return the expected value.");
        }

        // A test for operator + (Vector2Df, Vector2Df)
        [Fact]
        public void Vector2DAdditionTest()
        {
            Vector2D a = new Vector2D(1.0d, 2.0d);
            Vector2D b = new Vector2D(3.0d, 4.0d);

            Vector2D expected = new Vector2D(4.0d, 6.0d);
            Vector2D actual;

            actual = a + b;

            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.operator + did not return the expected value.");
        }

        // A test for Vector2Df (double, double)
        [Fact]
        public void Vector2DConstructorTest()
        {
            double x = 1.0d;
            double y = 2.0d;

            Vector2D target = new Vector2D(x, y);
            Assert.True(MathHelper.Equal(target.X, x) && MathHelper.Equal(target.Y, y), "Vector2Df(x,y) constructor did not return the expected value.");
        }

        // A test for Vector2Df ()
        // Constructor with no parameter
        [Fact]
        public void Vector2DConstructorTest2()
        {
            Vector2D target = new Vector2D();
            Assert.Equal(0.0d, target.X);
            Assert.Equal(0.0d, target.Y);
        }

        // A test for Vector2Df (double, double)
        // Constructor with special floating values
        [Fact]
        public void Vector2DConstructorTest3()
        {
            Vector2D target = new Vector2D(double.NaN, double.MaxValue);
            Assert.Equal(double.NaN, target.X);
            Assert.Equal(double.MaxValue, target.Y);
        }

        // A test for Vector2Df (double)
        [Fact]
        public void Vector2DConstructorTest4()
        {
            double value = 1.0d;
            Vector2D target = new Vector2D(value);

            Vector2D expected = new Vector2D(value, value);
            Assert.Equal(expected, target);

            value = 2.0d;
            target = new Vector2D(value);
            expected = new Vector2D(value, value);
            Assert.Equal(expected, target);
        }

        // A test for Vector2Df (ReadOnlySpan<double>)
        [Fact]
        public void Vector2DConstructorTest5()
        {
            double value = 1.0d;
            Vector2D target = new Vector2D(new[] { value, value });
            Vector2D expected = new Vector2D(value);

            Assert.Equal(expected, target);
            Assert.Throws<ArgumentOutOfRangeException>(() => new Vector2D(new double[1]));
        }

        // A test for Add (Vector2Df, Vector2Df)
        [Fact]
        public void Vector2DAddTest()
        {
            Vector2D a = new Vector2D(1.0d, 2.0d);
            Vector2D b = new Vector2D(5.0d, 6.0d);

            Vector2D expected = new Vector2D(6.0d, 8.0d);
            Vector2D actual;

            actual = Vector2D.Add(a, b);
            Assert.Equal(expected, actual);
        }

        // A test for Divide (Vector2Df, double)
        [Fact]
        public void Vector2DDivideTest()
        {
            Vector2D a = new Vector2D(1.0d, 2.0d);
            double div = 2.0d;
            Vector2D expected = new Vector2D(0.5d, 1.0d);
            Vector2D actual;
            actual = Vector2D.Divide(a, div);
            Assert.Equal(expected, actual);
        }

        // A test for Divide (Vector2Df, Vector2Df)
        [Fact]
        public void Vector2DDivideTest1()
        {
            Vector2D a = new Vector2D(1.0d, 6.0d);
            Vector2D b = new Vector2D(5.0d, 2.0d);

            Vector2D expected = new Vector2D(1.0d / 5.0d, 6.0d / 2.0d);
            Vector2D actual;

            actual = Vector2D.Divide(a, b);
            Assert.Equal(expected, actual);
        }

        // A test for Equals (object)
        [Fact]
        public void Vector2DEqualsTest()
        {
            Vector2D a = new Vector2D(1.0d, 2.0d);
            Vector2D b = new Vector2D(1.0d, 2.0d);

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

        // A test for Multiply (Vector2Df, double)
        [Fact]
        public void Vector2DMultiplyTest()
        {
            Vector2D a = new Vector2D(1.0d, 2.0d);
            const double factor = 2.0d;
            Vector2D expected = new Vector2D(2.0d, 4.0d);
            Vector2D actual = Vector2D.Multiply(a, factor);
            Assert.Equal(expected, actual);
        }

        // A test for Multiply (double, Vector2Df)
        [Fact]
        public void Vector2DMultiplyTest2()
        {
            Vector2D a = new Vector2D(1.0d, 2.0d);
            const double factor = 2.0d;
            Vector2D expected = new Vector2D(2.0d, 4.0d);
            Vector2D actual = Vector2D.Multiply(factor, a);
            Assert.Equal(expected, actual);
        }

        // A test for Multiply (Vector2Df, Vector2Df)
        [Fact]
        public void Vector2DMultiplyTest3()
        {
            Vector2D a = new Vector2D(1.0d, 2.0d);
            Vector2D b = new Vector2D(5.0d, 6.0d);

            Vector2D expected = new Vector2D(5.0d, 12.0d);
            Vector2D actual;

            actual = Vector2D.Multiply(a, b);
            Assert.Equal(expected, actual);
        }

        // A test for Negate (Vector2Df)
        [Fact]
        public void Vector2DNegateTest()
        {
            Vector2D a = new Vector2D(1.0d, 2.0d);

            Vector2D expected = new Vector2D(-1.0d, -2.0d);
            Vector2D actual;

            actual = Vector2D.Negate(a);
            Assert.Equal(expected, actual);
        }

        // A test for operator != (Vector2Df, Vector2Df)
        [Fact]
        public void Vector2DInequalityTest()
        {
            Vector2D a = new Vector2D(1.0d, 2.0d);
            Vector2D b = new Vector2D(1.0d, 2.0d);

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

        // A test for operator == (Vector2Df, Vector2Df)
        [Fact]
        public void Vector2DEqualityTest()
        {
            Vector2D a = new Vector2D(1.0d, 2.0d);
            Vector2D b = new Vector2D(1.0d, 2.0d);

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

        // A test for Subtract (Vector2Df, Vector2Df)
        [Fact]
        public void Vector2DSubtractTest()
        {
            Vector2D a = new Vector2D(1.0d, 6.0d);
            Vector2D b = new Vector2D(5.0d, 2.0d);

            Vector2D expected = new Vector2D(-4.0d, 4.0d);
            Vector2D actual;

            actual = Vector2D.Subtract(a, b);
            Assert.Equal(expected, actual);
        }

        // A test for UnitX
        [Fact]
        public void Vector2DUnitXTest()
        {
            Vector2D val = new Vector2D(1.0d, 0.0d);
            Assert.Equal(val, Vector2D.UnitX);
        }

        // A test for UnitY
        [Fact]
        public void Vector2DUnitYTest()
        {
            Vector2D val = new Vector2D(0.0d, 1.0d);
            Assert.Equal(val, Vector2D.UnitY);
        }

        // A test for One
        [Fact]
        public void Vector2DOneTest()
        {
            Vector2D val = new Vector2D(1.0d, 1.0d);
            Assert.Equal(val, Vector2D.One);
        }

        // A test for Zero
        [Fact]
        public void Vector2DZeroTest()
        {
            Vector2D val = new Vector2D(0.0d, 0.0d);
            Assert.Equal(val, Vector2D.Zero);
        }

        // A test for Equals (Vector2Df)
        [Fact]
        public void Vector2DEqualsTest1()
        {
            Vector2D a = new Vector2D(1.0d, 2.0d);
            Vector2D b = new Vector2D(1.0d, 2.0d);

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

        // A test for Vector2Df comparison involving NaN values
        [Fact]
        public void Vector2DEqualsNaNTest()
        {
            Vector2D a = new Vector2D(double.NaN, 0);
            Vector2D b = new Vector2D(0, double.NaN);

            Assert.False(a == Vector2D.Zero);
            Assert.False(b == Vector2D.Zero);

            Assert.True(a != Vector2D.Zero);
            Assert.True(b != Vector2D.Zero);

            Assert.False(a.Equals(Vector2D.Zero));
            Assert.False(b.Equals(Vector2D.Zero));

            Assert.True(a.Equals(a));
            Assert.True(b.Equals(b));
        }

        // A test for Reflect (Vector2Df, Vector2Df)
        [Fact]
        public void Vector2DReflectTest()
        {
            Vector2D a = Vector2D.Normalize(new Vector2D(1.0d, 1.0d));

            // Reflect on XZ plane.
            Vector2D n = new Vector2D(0.0d, 1.0d);
            Vector2D expected = new Vector2D(a.X, -a.Y);
            Vector2D actual = Vector2D.Reflect(a, n);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Reflect did not return the expected value.");

            // Reflect on XY plane.
            n = new Vector2D(0.0d, 0.0d);
            expected = new Vector2D(a.X, a.Y);
            actual = Vector2D.Reflect(a, n);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Reflect did not return the expected value.");

            // Reflect on YZ plane.
            n = new Vector2D(1.0d, 0.0d);
            expected = new Vector2D(-a.X, a.Y);
            actual = Vector2D.Reflect(a, n);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Reflect did not return the expected value.");
        }

        // A test for Reflect (Vector2Df, Vector2Df)
        // Reflection when normal and source are the same
        [Fact]
        public void Vector2DReflectTest1()
        {
            Vector2D n = new Vector2D(0.45d, 1.28d);
            n = Vector2D.Normalize(n);
            Vector2D a = n;

            Vector2D expected = -n;
            Vector2D actual = Vector2D.Reflect(a, n);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Reflect did not return the expected value.");
        }

        // A test for Reflect (Vector2Df, Vector2Df)
        // Reflection when normal and source are negation
        [Fact]
        public void Vector2DReflectTest2()
        {
            Vector2D n = new Vector2D(0.45d, 1.28d);
            n = Vector2D.Normalize(n);
            Vector2D a = -n;

            Vector2D expected = n;
            Vector2D actual = Vector2D.Reflect(a, n);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Reflect did not return the expected value.");
        }

        [Fact]
        public void Vector2DAbsTest()
        {
            Vector2D v1 = new Vector2D(-2.5d, 2.0d);
            Vector2D v3 = Vector2D.Abs(new Vector2D(0.0d, double.NegativeInfinity));
            Vector2D v = Vector2D.Abs(v1);
            Assert.Equal(2.5d, v.X);
            Assert.Equal(2.0d, v.Y);
            Assert.Equal(0.0d, v3.X);
            Assert.Equal(double.PositiveInfinity, v3.Y);
        }

        [Fact]
        public void Vector2DSqrtTest()
        {
            Vector2D v1 = new Vector2D(-2.5d, 2.0d);
            Vector2D v2 = new Vector2D(5.5d, 4.5d);
            Assert.Equal(2, (int)Vector2D.SquareRoot(v2).X);
            Assert.Equal(2, (int)Vector2D.SquareRoot(v2).Y);
            Assert.Equal(double.NaN, Vector2D.SquareRoot(v1).X);
        }

        // A test to make sure these types are blittable directly into GPU buffer memory layouts
        [Fact]
        public unsafe void Vector2DSizeofTest()
        {
            Assert.Equal(8, sizeof(Vector2D));
            Assert.Equal(16, sizeof(Vector2D_2x));
            Assert.Equal(12, sizeof(Vector2DPlusFloat));
            Assert.Equal(24, sizeof(Vector2DPlusFloat_2x));
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Vector2D_2x
        {
            private Vector2D _a;
            private Vector2D _b;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Vector2DPlusFloat
        {
            private Vector2D _v;
            private double _f;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Vector2DPlusFloat_2x
        {
            private Vector2DPlusFloat _a;
            private Vector2DPlusFloat _b;
        }

        [Fact]
        public void SetFieldsTest()
        {
            Vector2D v3 = new Vector2D(4d, 5d);
            v3.X = 1.0d;
            v3.Y = 2.0d;
            Assert.Equal(1.0d, v3.X);
            Assert.Equal(2.0d, v3.Y);
            Vector2D v4 = v3;
            v4.Y = 0.5d;
            Assert.Equal(1.0d, v4.X);
            Assert.Equal(0.5d, v4.Y);
            Assert.Equal(2.0d, v3.Y);
        }

        [Fact]
        public void EmbeddedVectorSetFields()
        {
            EmbeddedVectorObject evo = new EmbeddedVectorObject();
            evo.FieldVector.X = 5.0d;
            evo.FieldVector.Y = 5.0d;
            Assert.Equal(5.0d, evo.FieldVector.X);
            Assert.Equal(5.0d, evo.FieldVector.Y);
        }

        private class EmbeddedVectorObject
        {
            public Vector2D FieldVector;
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.CosDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void CosDoubleTest(double value, double expectedResult, double variance)
        {
            Vector2D actualResult = Vector2D.Cos(Vector2D.Create(value));
            AssertEqual(Vector2D.Create(expectedResult), actualResult, Vector2D.Create(variance));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.ExpDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void ExpDoubleTest(double value, double expectedResult, double variance)
        {
            Vector2D actualResult = Vector2D.Exp(Vector2D.Create(value));
            AssertEqual(Vector2D.Create(expectedResult), actualResult, Vector2D.Create(variance));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.LogDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void LogDoubleTest(double value, double expectedResult, double variance)
        {
            Vector2D actualResult = Vector2D.Log(Vector2D.Create(value));
            AssertEqual(Vector2D.Create(expectedResult), actualResult, Vector2D.Create(variance));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.Log2Double), MemberType = typeof(GenericMathTestMemberData))]
        public void Log2DoubleTest(double value, double expectedResult, double variance)
        {
            Vector2D actualResult = Vector2D.Log2(Vector2D.Create(value));
            AssertEqual(Vector2D.Create(expectedResult), actualResult, Vector2D.Create(variance));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.FusedMultiplyAddDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void FusedMultiplyAddDoubleTest(double left, double right, double addend, double expectedResult)
        {
            AssertEqual(Vector2D.Create(expectedResult), Vector2D.FusedMultiplyAdd(Vector2D.Create(left), Vector2D.Create(right), Vector2D.Create(addend)), Vector2D.Zero);
            AssertEqual(Vector2D.Create(double.MultiplyAddEstimate(left, right, addend)), Vector2D.MultiplyAddEstimate(Vector2D.Create(left), Vector2D.Create(right), Vector2D.Create(addend)), Vector2D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.ClampDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void ClampDoubleTest(double x, double min, double max, double expectedResult)
        {
            Vector2D actualResult = Vector2D.Clamp(Vector2D.Create(x), Vector2D.Create(min), Vector2D.Create(max));
            AssertEqual(Vector2D.Create(expectedResult), actualResult, Vector2D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.CopySignDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void CopySignDoubleTest(double x, double y, double expectedResult)
        {
            Vector2D actualResult = Vector2D.CopySign(Vector2D.Create(x), Vector2D.Create(y));
            AssertEqual(Vector2D.Create(expectedResult), actualResult, Vector2D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.DegreesToRadiansDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void DegreesToRadiansDoubleTest(double value, double expectedResult, double variance)
        {
            AssertEqual(Vector2D.Create(-expectedResult), Vector2D.DegreesToRadians(Vector2D.Create(-value)), Vector2D.Create(variance));
            AssertEqual(Vector2D.Create(+expectedResult), Vector2D.DegreesToRadians(Vector2D.Create(+value)), Vector2D.Create(variance));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.HypotDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void HypotDoubleTest(double x, double y, double expectedResult, double variance)
        {
            AssertEqual(Vector2D.Create(expectedResult), Vector2D.Hypot(Vector2D.Create(-x), Vector2D.Create(-y)), Vector2D.Create(variance));
            AssertEqual(Vector2D.Create(expectedResult), Vector2D.Hypot(Vector2D.Create(-x), Vector2D.Create(+y)), Vector2D.Create(variance));
            AssertEqual(Vector2D.Create(expectedResult), Vector2D.Hypot(Vector2D.Create(+x), Vector2D.Create(-y)), Vector2D.Create(variance));
            AssertEqual(Vector2D.Create(expectedResult), Vector2D.Hypot(Vector2D.Create(+x), Vector2D.Create(+y)), Vector2D.Create(variance));

            AssertEqual(Vector2D.Create(expectedResult), Vector2D.Hypot(Vector2D.Create(-y), Vector2D.Create(-x)), Vector2D.Create(variance));
            AssertEqual(Vector2D.Create(expectedResult), Vector2D.Hypot(Vector2D.Create(-y), Vector2D.Create(+x)), Vector2D.Create(variance));
            AssertEqual(Vector2D.Create(expectedResult), Vector2D.Hypot(Vector2D.Create(+y), Vector2D.Create(-x)), Vector2D.Create(variance));
            AssertEqual(Vector2D.Create(expectedResult), Vector2D.Hypot(Vector2D.Create(+y), Vector2D.Create(+x)), Vector2D.Create(variance));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.LerpDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void LerpDoubleTest(double x, double y, double amount, double expectedResult)
        {
            AssertEqual(Vector2D.Create(+expectedResult), Vector2D.Lerp(Vector2D.Create(+x), Vector2D.Create(+y), Vector2D.Create(amount)), Vector2D.Zero);
            AssertEqual(Vector2D.Create((expectedResult == 0.0d) ? expectedResult : -expectedResult), Vector2D.Lerp(Vector2D.Create(-x), Vector2D.Create(-y), Vector2D.Create(amount)), Vector2D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MaxDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void MaxDoubleTest(double x, double y, double expectedResult)
        {
            Vector2D actualResult = Vector2D.Max(Vector2D.Create(x), Vector2D.Create(y));
            AssertEqual(Vector2D.Create(expectedResult), actualResult, Vector2D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MaxMagnitudeDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void MaxMagnitudeDoubleTest(double x, double y, double expectedResult)
        {
            Vector2D actualResult = Vector2D.MaxMagnitude(Vector2D.Create(x), Vector2D.Create(y));
            AssertEqual(Vector2D.Create(expectedResult), actualResult, Vector2D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MaxMagnitudeNumberDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void MaxMagnitudeNumberDoubleTest(double x, double y, double expectedResult)
        {
            Vector2D actualResult = Vector2D.MaxMagnitudeNumber(Vector2D.Create(x), Vector2D.Create(y));
            AssertEqual(Vector2D.Create(expectedResult), actualResult, Vector2D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MaxNumberDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void MaxNumberDoubleTest(double x, double y, double expectedResult)
        {
            Vector2D actualResult = Vector2D.MaxNumber(Vector2D.Create(x), Vector2D.Create(y));
            AssertEqual(Vector2D.Create(expectedResult), actualResult, Vector2D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MinDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void MinDoubleTest(double x, double y, double expectedResult)
        {
            Vector2D actualResult = Vector2D.Min(Vector2D.Create(x), Vector2D.Create(y));
            AssertEqual(Vector2D.Create(expectedResult), actualResult, Vector2D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MinMagnitudeDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void MinMagnitudeDoubleTest(double x, double y, double expectedResult)
        {
            Vector2D actualResult = Vector2D.MinMagnitude(Vector2D.Create(x), Vector2D.Create(y));
            AssertEqual(Vector2D.Create(expectedResult), actualResult, Vector2D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MinMagnitudeNumberDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void MinMagnitudeNumberDoubleTest(double x, double y, double expectedResult)
        {
            Vector2D actualResult = Vector2D.MinMagnitudeNumber(Vector2D.Create(x), Vector2D.Create(y));
            AssertEqual(Vector2D.Create(expectedResult), actualResult, Vector2D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MinNumberDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void MinNumberDoubleTest(double x, double y, double expectedResult)
        {
            Vector2D actualResult = Vector2D.MinNumber(Vector2D.Create(x), Vector2D.Create(y));
            AssertEqual(Vector2D.Create(expectedResult), actualResult, Vector2D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.RadiansToDegreesDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void RadiansToDegreesDoubleTest(double value, double expectedResult, double variance)
        {
            AssertEqual(Vector2D.Create(-expectedResult), Vector2D.RadiansToDegrees(Vector2D.Create(-value)), Vector2D.Create(variance));
            AssertEqual(Vector2D.Create(+expectedResult), Vector2D.RadiansToDegrees(Vector2D.Create(+value)), Vector2D.Create(variance));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.RoundDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void RoundDoubleTest(double value, double expectedResult)
        {
            Vector2D actualResult = Vector2D.Round(Vector2D.Create(value));
            AssertEqual(Vector2D.Create(expectedResult), actualResult, Vector2D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.RoundAwayFromZeroDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void RoundAwayFromZeroDoubleTest(double value, double expectedResult)
        {
            Vector2D actualResult = Vector2D.Round(Vector2D.Create(value), MidpointRounding.AwayFromZero);
            AssertEqual(Vector2D.Create(expectedResult), actualResult, Vector2D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.RoundToEvenDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void RoundToEvenDoubleTest(double value, double expectedResult)
        {
            Vector2D actualResult = Vector2D.Round(Vector2D.Create(value), MidpointRounding.ToEven);
            AssertEqual(Vector2D.Create(expectedResult), actualResult, Vector2D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.SinDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void SinDoubleTest(double value, double expectedResult, double variance)
        {
            Vector2D actualResult = Vector2D.Sin(Vector2D.Create(value));
            AssertEqual(Vector2D.Create(expectedResult), actualResult, Vector2D.Create(variance));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.SinCosDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void SinCosDoubleTest(double value, double expectedResultSin, double expectedResultCos, double allowedVarianceSin, double allowedVarianceCos)
        {
            (Vector2D resultSin, Vector2D resultCos) = Vector2D.SinCos(Vector2D.Create(value));
            AssertEqual(Vector2D.Create(expectedResultSin), resultSin, Vector2D.Create(allowedVarianceSin));
            AssertEqual(Vector2D.Create(expectedResultCos), resultCos, Vector2D.Create(allowedVarianceCos));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.TruncateDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void TruncateDoubleTest(double value, double expectedResult)
        {
            Vector2D actualResult = Vector2D.Truncate(Vector2D.Create(value));
            AssertEqual(Vector2D.Create(expectedResult), actualResult, Vector2D.Zero);
        }

        [Fact]
        public void AllAnyNoneTest()
        {
            Test(3, 2);

            [MethodImpl(MethodImplOptions.NoInlining)]
            void Test(double value1, double value2)
            {
                var input1 = Vector2D.Create(value1);
                var input2 = Vector2D.Create(value2);

                Assert.True(Vector2D.All(input1, value1));
                Assert.True(Vector2D.All(input2, value2));
                Assert.False(Vector2D.All(input1.WithElement(0, value2), value1));
                Assert.False(Vector2D.All(input2.WithElement(0, value1), value2));
                Assert.False(Vector2D.All(input1, value2));
                Assert.False(Vector2D.All(input2, value1));
                Assert.False(Vector2D.All(input1.WithElement(0, value2), value2));
                Assert.False(Vector2D.All(input2.WithElement(0, value1), value1));

                Assert.True(Vector2D.Any(input1, value1));
                Assert.True(Vector2D.Any(input2, value2));
                Assert.True(Vector2D.Any(input1.WithElement(0, value2), value1));
                Assert.True(Vector2D.Any(input2.WithElement(0, value1), value2));
                Assert.False(Vector2D.Any(input1, value2));
                Assert.False(Vector2D.Any(input2, value1));
                Assert.True(Vector2D.Any(input1.WithElement(0, value2), value2));
                Assert.True(Vector2D.Any(input2.WithElement(0, value1), value1));

                Assert.False(Vector2D.None(input1, value1));
                Assert.False(Vector2D.None(input2, value2));
                Assert.False(Vector2D.None(input1.WithElement(0, value2), value1));
                Assert.False(Vector2D.None(input2.WithElement(0, value1), value2));
                Assert.True(Vector2D.None(input1, value2));
                Assert.True(Vector2D.None(input2, value1));
                Assert.False(Vector2D.None(input1.WithElement(0, value2), value2));
                Assert.False(Vector2D.None(input2.WithElement(0, value1), value1));
            }
        }

        [Fact]
        public void AllAnyNoneTest_AllBitsSet()
        {
            Test(BitConverter.Int32BitsToSingle(-1));

            [MethodImpl(MethodImplOptions.NoInlining)]
            void Test(double value)
            {
                var input = Vector2D.Create(value);

                Assert.False(Vector2D.All(input, value));
                Assert.False(Vector2D.Any(input, value));
                Assert.True(Vector2D.None(input, value));
            }
        }

        [Fact]
        public void AllAnyNoneWhereAllBitsSetTest()
        {
            Test(BitConverter.Int32BitsToSingle(-1), 2);

            [MethodImpl(MethodImplOptions.NoInlining)]
            void Test(double allBitsSet, double value2)
            {
                var input1 = Vector2D.Create(allBitsSet);
                var input2 = Vector2D.Create(value2);

                Assert.True(Vector2D.AllWhereAllBitsSet(input1));
                Assert.False(Vector2D.AllWhereAllBitsSet(input2));
                Assert.False(Vector2D.AllWhereAllBitsSet(input1.WithElement(0, value2)));
                Assert.False(Vector2D.AllWhereAllBitsSet(input2.WithElement(0, allBitsSet)));

                Assert.True(Vector2D.AnyWhereAllBitsSet(input1));
                Assert.False(Vector2D.AnyWhereAllBitsSet(input2));
                Assert.True(Vector2D.AnyWhereAllBitsSet(input1.WithElement(0, value2)));
                Assert.True(Vector2D.AnyWhereAllBitsSet(input2.WithElement(0, allBitsSet)));

                Assert.False(Vector2D.NoneWhereAllBitsSet(input1));
                Assert.True(Vector2D.NoneWhereAllBitsSet(input2));
                Assert.False(Vector2D.NoneWhereAllBitsSet(input1.WithElement(0, value2)));
                Assert.False(Vector2D.NoneWhereAllBitsSet(input2.WithElement(0, allBitsSet)));
            }
        }

        [Fact]
        public void CountIndexOfLastIndexOfSingleTest()
        {
            Test(3, 2);

            [MethodImpl(MethodImplOptions.NoInlining)]
            void Test(double value1, double value2)
            {
                var input1 = Vector2D.Create(value1);
                var input2 = Vector2D.Create(value2);

                Assert.Equal(ElementCount, Vector2D.Count(input1, value1));
                Assert.Equal(ElementCount, Vector2D.Count(input2, value2));
                Assert.Equal(ElementCount - 1, Vector2D.Count(input1.WithElement(0, value2), value1));
                Assert.Equal(ElementCount - 1, Vector2D.Count(input2.WithElement(0, value1), value2));
                Assert.Equal(0, Vector2D.Count(input1, value2));
                Assert.Equal(0, Vector2D.Count(input2, value1));
                Assert.Equal(1, Vector2D.Count(input1.WithElement(0, value2), value2));
                Assert.Equal(1, Vector2D.Count(input2.WithElement(0, value1), value1));

                Assert.Equal(0, Vector2D.IndexOf(input1, value1));
                Assert.Equal(0, Vector2D.IndexOf(input2, value2));
                Assert.Equal(1, Vector2D.IndexOf(input1.WithElement(0, value2), value1));
                Assert.Equal(1, Vector2D.IndexOf(input2.WithElement(0, value1), value2));
                Assert.Equal(-1, Vector2D.IndexOf(input1, value2));
                Assert.Equal(-1, Vector2D.IndexOf(input2, value1));
                Assert.Equal(0, Vector2D.IndexOf(input1.WithElement(0, value2), value2));
                Assert.Equal(0, Vector2D.IndexOf(input2.WithElement(0, value1), value1));

                Assert.Equal(ElementCount - 1, Vector2D.LastIndexOf(input1, value1));
                Assert.Equal(ElementCount - 1, Vector2D.LastIndexOf(input2, value2));
                Assert.Equal(ElementCount - 1, Vector2D.LastIndexOf(input1.WithElement(0, value2), value1));
                Assert.Equal(ElementCount - 1, Vector2D.LastIndexOf(input2.WithElement(0, value1), value2));
                Assert.Equal(-1, Vector2D.LastIndexOf(input1, value2));
                Assert.Equal(-1, Vector2D.LastIndexOf(input2, value1));
                Assert.Equal(0, Vector2D.LastIndexOf(input1.WithElement(0, value2), value2));
                Assert.Equal(0, Vector2D.LastIndexOf(input2.WithElement(0, value1), value1));
            }
        }

        [Fact]
        public void CountIndexOfLastIndexOfSingleTest_AllBitsSet()
        {
            Test(BitConverter.Int32BitsToSingle(-1));

            [MethodImpl(MethodImplOptions.NoInlining)]
            void Test(double value)
            {
                var input = Vector2D.Create(value);

                Assert.Equal(0, Vector2D.Count(input, value));
                Assert.Equal(-1, Vector2D.IndexOf(input, value));
                Assert.Equal(-1, Vector2D.LastIndexOf(input, value));
            }
        }

        [Fact]
        public void CountIndexOfLastIndexOfWhereAllBitsSetSingleTest()
        {
            Test(BitConverter.Int32BitsToSingle(-1), 2);

            [MethodImpl(MethodImplOptions.NoInlining)]
            void Test(double allBitsSet, double value2)
            {
                var input1 = Vector2D.Create(allBitsSet);
                var input2 = Vector2D.Create(value2);

                Assert.Equal(ElementCount, Vector2D.CountWhereAllBitsSet(input1));
                Assert.Equal(0, Vector2D.CountWhereAllBitsSet(input2));
                Assert.Equal(ElementCount - 1, Vector2D.CountWhereAllBitsSet(input1.WithElement(0, value2)));
                Assert.Equal(1, Vector2D.CountWhereAllBitsSet(input2.WithElement(0, allBitsSet)));

                Assert.Equal(0, Vector2D.IndexOfWhereAllBitsSet(input1));
                Assert.Equal(-1, Vector2D.IndexOfWhereAllBitsSet(input2));
                Assert.Equal(1, Vector2D.IndexOfWhereAllBitsSet(input1.WithElement(0, value2)));
                Assert.Equal(0, Vector2D.IndexOfWhereAllBitsSet(input2.WithElement(0, allBitsSet)));

                Assert.Equal(ElementCount - 1, Vector2D.LastIndexOfWhereAllBitsSet(input1));
                Assert.Equal(-1, Vector2D.LastIndexOfWhereAllBitsSet(input2));
                Assert.Equal(ElementCount - 1, Vector2D.LastIndexOfWhereAllBitsSet(input1.WithElement(0, value2)));
                Assert.Equal(0, Vector2D.LastIndexOfWhereAllBitsSet(input2.WithElement(0, allBitsSet)));
            }
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void IsEvenIntegerTest(double value) => Assert.Equal(double.IsEvenInteger(value) ? Vector2D.AllBitsSet : Vector2D.Zero, Vector2D.IsEvenInteger(Vector2D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void IsFiniteTest(double value) => Assert.Equal(double.IsFinite(value) ? Vector2D.AllBitsSet : Vector2D.Zero, Vector2D.IsFinite(Vector2D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void IsInfinityTest(double value) => Assert.Equal(double.IsInfinity(value) ? Vector2D.AllBitsSet : Vector2D.Zero, Vector2D.IsInfinity(Vector2D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void IsIntegerTest(double value) => Assert.Equal(double.IsInteger(value) ? Vector2D.AllBitsSet : Vector2D.Zero, Vector2D.IsInteger(Vector2D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void IsNaNTest(double value) => Assert.Equal(double.IsNaN(value) ? Vector2D.AllBitsSet : Vector2D.Zero, Vector2D.IsNaN(Vector2D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void IsNegativeTest(double value) => Assert.Equal(double.IsNegative(value) ? Vector2D.AllBitsSet : Vector2D.Zero, Vector2D.IsNegative(Vector2D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void IsNegativeInfinityTest(double value) => Assert.Equal(double.IsNegativeInfinity(value) ? Vector2D.AllBitsSet : Vector2D.Zero, Vector2D.IsNegativeInfinity(Vector2D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void IsNormalTest(double value) => Assert.Equal(double.IsNormal(value) ? Vector2D.AllBitsSet : Vector2D.Zero, Vector2D.IsNormal(Vector2D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void IsOddIntegerTest(double value) => Assert.Equal(double.IsOddInteger(value) ? Vector2D.AllBitsSet : Vector2D.Zero, Vector2D.IsOddInteger(Vector2D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void IsPositiveTest(double value) => Assert.Equal(double.IsPositive(value) ? Vector2D.AllBitsSet : Vector2D.Zero, Vector2D.IsPositive(Vector2D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void IsPositiveInfinityTest(double value) => Assert.Equal(double.IsPositiveInfinity(value) ? Vector2D.AllBitsSet : Vector2D.Zero, Vector2D.IsPositiveInfinity(Vector2D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void IsSubnormalTest(double value) => Assert.Equal(double.IsSubnormal(value) ? Vector2D.AllBitsSet : Vector2D.Zero, Vector2D.IsSubnormal(Vector2D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void IsZeroSingleTest(double value) => Assert.Equal((value == 0) ? Vector2D.AllBitsSet : Vector2D.Zero, Vector2D.IsZero(Vector2D.Create(value)));

        [Fact]
        public void AllBitsSetTest()
        {
            Assert.Equal(-1, BitConverter.DoubleToInt64Bits(Vector2D.AllBitsSet.X));
            Assert.Equal(-1, BitConverter.DoubleToInt64Bits(Vector2D.AllBitsSet.Y));
        }

        [Fact]
        public void ConditionalSelectTest()
        {
            Test(Vector2D.Create(1, 2), Vector2D.AllBitsSet, Vector2D.Create(1, 2), Vector2D.Create(5, 6));
            Test(Vector2D.Create(5, 6), Vector2D.Zero, Vector2D.Create(1, 2), Vector2D.Create(5, 6));
            Test(Vector2D.Create(1, 6), Vector256.Create(-1, 0, -1, 0).AsDouble().AsVector2D(), Vector2D.Create(1, 2), Vector2D.Create(5, 6));

            [MethodImpl(MethodImplOptions.NoInlining)]
            void Test(Vector2D expectedResult, Vector2D condition, Vector2D left, Vector2D right)
            {
                Assert.Equal(expectedResult, Vector2D.ConditionalSelect(condition, left, right));
            }
        }

        [Theory]
        [InlineData(+0.0d, +0.0d, 0b00)]
        [InlineData(-0.0d, +1.0d, 0b01)]
        [InlineData(-0.0d, -0.0d, 0b11)]
        public void ExtractMostSignificantBitsTest(double x, double y, uint expectedResult)
        {
            Assert.Equal(expectedResult, Vector2D.Create(x, y).ExtractMostSignificantBits());
        }

        [Theory]
        [InlineData(1.0d, 2.0d)]
        [InlineData(5.0d, 6.0d)]
        public void GetElementTest(double x, double y)
        {
            Assert.Equal(x, Vector2D.Create(x, y).GetElement(0));
            Assert.Equal(y, Vector2D.Create(x, y).GetElement(1));
        }

        [Theory]
        [InlineData(1.0d, 2.0d)]
        [InlineData(5.0d, 6.0d)]
        public void ShuffleTest(double x, double y)
        {
            Assert.Equal(Vector2D.Create(y, x), Vector2D.Shuffle(Vector2D.Create(x, y), 1, 0));
            Assert.Equal(Vector2D.Create(x, x), Vector2D.Shuffle(Vector2D.Create(x, y), 0, 0));
        }

        [Theory]
        [InlineData(1.0d, 2.0d, 3.0d)]
        [InlineData(5.0d, 6.0d, 11.0d)]
        public void SumTest(double x, double y, double expectedResult)
        {
            Assert.Equal(expectedResult, Vector2D.Sum(Vector2D.Create(x, y)));
        }

        [Theory]
        [InlineData(1.0d, 2.0d)]
        [InlineData(5.0d, 6.0d)]
        public void ToScalarTest(double x, double y)
        {
            Assert.Equal(x, Vector2D.Create(x, y).ToScalar());
        }

        [Theory]
        [InlineData(1.0d, 2.0d)]
        [InlineData(5.0d, 6.0d)]
        public void WithElementTest(double x, double y)
        {
            var vector = Vector2D.Create(10);

            Assert.Equal(10, vector.X);
            Assert.Equal(10, vector.Y);

            vector = vector.WithElement(0, x);

            Assert.Equal(x, vector.X);
            Assert.Equal(10, vector.Y);

            vector = vector.WithElement(1, y);

            Assert.Equal(x, vector.X);
            Assert.Equal(y, vector.Y);
        }

        [Theory]
        [InlineData(1.0d, 2.0d)]
        [InlineData(5.0d, 6.0d)]
        public void AsVector3DTest(double x, double y)
        {
            var vector = Vector2D.Create(x, y).AsVector3D();

            Assert.Equal(x, vector.X);
            Assert.Equal(y, vector.Y);
            Assert.Equal(0, vector.Z);
        }

        [Theory]
        [InlineData(1.0d, 2.0d)]
        [InlineData(5.0d, 6.0d)]
        public void AsVector3DUnsafeTest(double x, double y)
        {
            var vector = Vector2D.Create(x, y).AsVector3DUnsafe();

            Assert.Equal(x, vector.X);
            Assert.Equal(y, vector.Y);
        }

        [Fact]
        public void CreateScalarTest()
        {
            var vector = Vector2D.CreateScalar(double.Pi);

            Assert.Equal(double.Pi, vector.X);
            Assert.Equal(0, vector.Y);

            vector = Vector2D.CreateScalar(double.E);

            Assert.Equal(double.E, vector.X);
            Assert.Equal(0, vector.Y);
        }

        [Fact]
        public void CreateScalarUnsafeTest()
        {
            var vector = Vector2D.CreateScalarUnsafe(double.Pi);
            Assert.Equal(double.Pi, vector.X);

            vector = Vector2D.CreateScalarUnsafe(double.E);
            Assert.Equal(double.E, vector.X);
        }
    }
}
