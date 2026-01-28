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
    public sealed class Vector4DDTests
    {
        private const int ElementCount = 4;

        /// <summary>Verifies that two <see cref="Vector4D" /> values are equal, within the <paramref name="variance" />.</summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The value to be compared against</param>
        /// <param name="variance">The total variance allowed between the expected and actual results.</param>
        /// <exception cref="EqualException">Thrown when the values are not equal</exception>
        internal static void AssertEqual(Vector4D expected, Vector4D actual, Vector4D variance)
        {
            AssertExtensions.Equal(expected.X, actual.X, variance.X);
            AssertExtensions.Equal(expected.Y, actual.Y, variance.Y);
            AssertExtensions.Equal(expected.Z, actual.Z, variance.Z);
            AssertExtensions.Equal(expected.W, actual.W, variance.W);
        }

        [Fact]
        public void Vector4DMarshalSizeTest()
        {
            Assert.Equal(32, Marshal.SizeOf<Vector4D>());
            Assert.Equal(32, Marshal.SizeOf<Vector4D>(new Vector4D()));
        }

        [Theory]
        [InlineData(0.0d, 1.0d, 0.0d, 1.0d)]
        [InlineData(1.0d, 0.0d, 1.0d, 0.0d)]
        [InlineData(3.1434343d, 1.1234123d, 0.1234123d, -0.1234123d)]
        [InlineData(1.0000001d, 0.0000001d, 2.0000001d, 0.0000002d)]
        public void Vector4DIndexerGetTest(double x, double y, double z, double w)
        {
            var vector = new Vector4D(x, y, z, w);

            Assert.Equal(x, vector[0]);
            Assert.Equal(y, vector[1]);
            Assert.Equal(z, vector[2]);
            Assert.Equal(w, vector[3]);
        }

        [Theory]
        [InlineData(0.0d, 1.0d, 0.0d, 1.0d)]
        [InlineData(1.0d, 0.0d, 1.0d, 0.0d)]
        [InlineData(3.1434343d, 1.1234123d, 0.1234123d, -0.1234123d)]
        [InlineData(1.0000001d, 0.0000001d, 2.0000001d, 0.0000002d)]
        public void Vector4DIndexerSetTest(double x, double y, double z, double w)
        {
            var vector = new Vector4D(0.0d, 0.0d, 0.0d, 0.0d);

            vector[0] = x;
            vector[1] = y;
            vector[2] = z;
            vector[3] = w;

            Assert.Equal(x, vector[0]);
            Assert.Equal(y, vector[1]);
            Assert.Equal(z, vector[2]);
            Assert.Equal(w, vector[3]);
        }

        [Fact]
        public void Vector4DCopyToTest()
        {
            Vector4D v1 = new Vector4D(2.5d, 2.0d, 3.0d, 3.3d);

            double[] a = new double[5];
            double[] b = new double[4];

            Assert.Throws<NullReferenceException>(() => v1.CopyTo(null, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => v1.CopyTo(a, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => v1.CopyTo(a, a.Length));
            Assert.Throws<ArgumentException>(() => v1.CopyTo(a, a.Length - 2));

            v1.CopyTo(a, 1);
            v1.CopyTo(b);
            Assert.Equal(0.0d, a[0]);
            Assert.Equal(2.5d, a[1]);
            Assert.Equal(2.0d, a[2]);
            Assert.Equal(3.0d, a[3]);
            Assert.Equal(3.3d, a[4]);
            Assert.Equal(2.5d, b[0]);
            Assert.Equal(2.0d, b[1]);
            Assert.Equal(3.0d, b[2]);
            Assert.Equal(3.3d, b[3]);
        }

        [Fact]
        public void Vector4DCopyToSpanTest()
        {
            Vector4D vector = new Vector4D(1.0d, 2.0d, 3.0d, 4.0d);
            Span<double> destination = new double[4];

            Assert.Throws<ArgumentException>(() => vector.CopyTo(new Span<double>(new double[3])));
            vector.CopyTo(destination);

            Assert.Equal(1.0d, vector.X);
            Assert.Equal(2.0d, vector.Y);
            Assert.Equal(3.0d, vector.Z);
            Assert.Equal(4.0d, vector.W);
            Assert.Equal(vector.X, destination[0]);
            Assert.Equal(vector.Y, destination[1]);
            Assert.Equal(vector.Z, destination[2]);
            Assert.Equal(vector.W, destination[3]);
        }

        [Fact]
        public void Vector4DTryCopyToTest()
        {
            Vector4D vector = new Vector4D(1.0d, 2.0d, 3.0d, 4.0d);
            Span<double> destination = new double[4];

            Assert.False(vector.TryCopyTo(new Span<double>(new double[3])));
            Assert.True(vector.TryCopyTo(destination));

            Assert.Equal(1.0d, vector.X);
            Assert.Equal(2.0d, vector.Y);
            Assert.Equal(3.0d, vector.Z);
            Assert.Equal(4.0d, vector.W);
            Assert.Equal(vector.X, destination[0]);
            Assert.Equal(vector.Y, destination[1]);
            Assert.Equal(vector.Z, destination[2]);
            Assert.Equal(vector.W, destination[3]);
        }

        [Fact]
        public void Vector4DGetHashCodeTest()
        {
            Vector4D v1 = new Vector4D(2.5d, 2.0d, 3.0d, 3.3d);
            Vector4D v2 = new Vector4D(2.5d, 2.0d, 3.0d, 3.3d);
            Vector4D v3 = new Vector4D(2.5d, 2.0d, 3.0d, 3.3d);
            Vector4D v5 = new Vector4D(3.3d, 3.0d, 2.0d, 2.5d);
            Assert.Equal(v1.GetHashCode(), v1.GetHashCode());
            Assert.Equal(v1.GetHashCode(), v2.GetHashCode());
            Assert.NotEqual(v1.GetHashCode(), v5.GetHashCode());
            Assert.Equal(v1.GetHashCode(), v3.GetHashCode());
            Vector4D v4 = new Vector4D(0.0d, 0.0d, 0.0d, 0.0d);
            Vector4D v6 = new Vector4D(1.0d, 0.0d, 0.0d, 0.0d);
            Vector4D v7 = new Vector4D(0.0d, 1.0d, 0.0d, 0.0d);
            Vector4D v8 = new Vector4D(1.0d, 1.0d, 1.0d, 1.0d);
            Vector4D v9 = new Vector4D(1.0d, 1.0d, 0.0d, 0.0d);
            Assert.NotEqual(v4.GetHashCode(), v6.GetHashCode());
            Assert.NotEqual(v4.GetHashCode(), v7.GetHashCode());
            Assert.NotEqual(v4.GetHashCode(), v8.GetHashCode());
            Assert.NotEqual(v7.GetHashCode(), v6.GetHashCode());
            Assert.NotEqual(v8.GetHashCode(), v6.GetHashCode());
            Assert.NotEqual(v8.GetHashCode(), v7.GetHashCode());
            Assert.NotEqual(v9.GetHashCode(), v7.GetHashCode());
        }

        [Fact]
        public void Vector4DToStringTest()
        {
            string separator = CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator;
            CultureInfo enUsCultureInfo = new CultureInfo("en-US");

            Vector4D v1 = new Vector4D(2.5d, 2.0d, 3.0d, 3.3d);

            string v1str = v1.ToString();
            string expectedv1 = string.Format(CultureInfo.CurrentCulture
                , "<{1:G}{0} {2:G}{0} {3:G}{0} {4:G}>"
                , separator, 2.5, 2, 3, 3.3);
            Assert.Equal(expectedv1, v1str);

            string v1strformatted = v1.ToString("c", CultureInfo.CurrentCulture);
            string expectedv1dormatted = string.Format(CultureInfo.CurrentCulture
                , "<{1:c}{0} {2:c}{0} {3:c}{0} {4:c}>"
                , separator, 2.5, 2, 3, 3.3);
            Assert.Equal(expectedv1dormatted, v1strformatted);

            string v2strformatted = v1.ToString("c", enUsCultureInfo);
            string expectedv2dormatted = string.Format(enUsCultureInfo
                , "<{1:c}{0} {2:c}{0} {3:c}{0} {4:c}>"
                , enUsCultureInfo.NumberFormat.NumberGroupSeparator, 2.5, 2, 3, 3.3);
            Assert.Equal(expectedv2dormatted, v2strformatted);

            string v3strformatted = v1.ToString("c");
            string expectedv3dormatted = string.Format(CultureInfo.CurrentCulture
                , "<{1:c}{0} {2:c}{0} {3:c}{0} {4:c}>"
                , separator, 2.5, 2, 3, 3.3);
            Assert.Equal(expectedv3dormatted, v3strformatted);
        }

        // A test for DistanceSquared (Vector4D, Vector4D)
        [Fact]
        public void Vector4DDistanceSquaredTest()
        {
            Vector4D a = new Vector4D(1.0d, 2.0d, 3.0d, 4.0d);
            Vector4D b = new Vector4D(5.0d, 6.0d, 7.0d, 8.0d);

            double expected = 64.0d;
            double actual;

            actual = Vector4D.DistanceSquared(a, b);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.DistanceSquared did not return the expected value.");
        }

        // A test for Distance (Vector4D, Vector4D)
        [Fact]
        public void Vector4DDistanceTest()
        {
            Vector4D a = new Vector4D(1.0d, 2.0d, 3.0d, 4.0d);
            Vector4D b = new Vector4D(5.0d, 6.0d, 7.0d, 8.0d);

            double expected = 8.0d;
            double actual;

            actual = Vector4D.Distance(a, b);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.Distance did not return the expected value.");
        }

        // A test for Distance (Vector4D, Vector4D)
        // Distance from the same point
        [Fact]
        public void Vector4DDistanceTest1()
        {
            Vector4D a = new Vector4D(new Vector2D(1.051d, 2.05d), 3.478d, 1.0d);
            Vector4D b = new Vector4D(new Vector3D(1.051d, 2.05d, 3.478d), 0.0d);
            b.W = 1.0d;

            double actual = Vector4D.Distance(a, b);
            Assert.Equal(0.0d, actual);
        }

        // A test for Dot (Vector4D, Vector4D)
        [Fact]
        public void Vector4DDotTest()
        {
            Vector4D a = new Vector4D(1.0d, 2.0d, 3.0d, 4.0d);
            Vector4D b = new Vector4D(5.0d, 6.0d, 7.0d, 8.0d);

            double expected = 70.0d;
            double actual;

            actual = Vector4D.Dot(a, b);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.Dot did not return the expected value.");
        }

        // A test for Dot (Vector4D, Vector4D)
        // Dot test for perpendicular vector
        [Fact]
        public void Vector4DDotTest1()
        {
            Vector3D a = new Vector3D(1.55d, 1.55d, 1);
            Vector3D b = new Vector3D(2.5d, 3, 1.5d);
            Vector3D c = Vector3D.Cross(a, b);

            Vector4D d = new Vector4D(a, 0);
            Vector4D e = new Vector4D(c, 0);

            double actual = Vector4D.Dot(d, e);
            Assert.True(MathHelper.Equal(0.0d, actual), "Vector4D.Dot did not return the expected value.");
        }

        [Fact]
        public void Vector4DCrossTest()
        {
            Vector3D a3 = new Vector3D(1.0d, 0.0d, 0.0d);
            Vector3D b3 = new Vector3D(0.0d, 1.0d, 0.0d);
            Vector3D e3 = Vector3D.Cross(a3, b3);

            Vector4D a4 = new Vector4D(a3, 2.0d);
            Vector4D b4 = new Vector4D(b3, 3.0d);
            Vector4D e4 = new Vector4D(e3, a4.W * b4.W);

            Vector4D actual = Vector4D.Cross(a4, b4);
            Assert.True(MathHelper.Equal(e4, actual), "Vector4D.Cross did not return the expected value.");
        }

        [Fact]
        public void Vector4DCrossTest1()
        {
            // Cross test of the same vector
            Vector3D a3 = new Vector3D(0.0d, 1.0d, 0.0d);
            Vector3D b3 = new Vector3D(0.0d, 1.0d, 0.0d);
            Vector3D e3 = Vector3D.Cross(a3, b3);

            Vector4D a4 = new Vector4D(a3, 3.0d);
            Vector4D b4 = new Vector4D(b3, 3.0d);
            Vector4D e4 = new Vector4D(e3, a4.W * b4.W);

            Vector4D actual = Vector4D.Cross(a4, b4);
            Assert.True(MathHelper.Equal(e4, actual), "Vector4D.Cross did not return the expected value.");
        }

        // A test for Length ()
        [Fact]
        public void Vector4DLengthTest()
        {
            Vector3D a = new Vector3D(1.0d, 2.0d, 3.0d);
            double w = 4.0d;

            Vector4D target = new Vector4D(a, w);

            double expected = (double)System.Math.Sqrt(30.0d);
            double actual;

            actual = target.Length();

            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.Length did not return the expected value.");
        }

        // A test for Length ()
        // Length test where length is zero
        [Fact]
        public void Vector4DLengthTest1()
        {
            Vector4D target = new Vector4D();

            double expected = 0.0d;
            double actual = target.Length();

            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.Length did not return the expected value.");
        }

        // A test for LengthSquared ()
        [Fact]
        public void Vector4DLengthSquaredTest()
        {
            Vector3D a = new Vector3D(1.0d, 2.0d, 3.0d);
            double w = 4.0d;

            Vector4D target = new Vector4D(a, w);

            double expected = 30;
            double actual;

            actual = target.LengthSquared();

            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.LengthSquared did not return the expected value.");
        }

        // A test for Min (Vector4D, Vector4D)
        [Fact]
        public void Vector4DMinTest()
        {
            Vector4D a = new Vector4D(-1.0d, 4.0d, -3.0d, 1000.0d);
            Vector4D b = new Vector4D(2.0d, 1.0d, -1.0d, 0.0d);

            Vector4D expected = new Vector4D(-1.0d, 1.0d, -3.0d, 0.0d);
            Vector4D actual;
            actual = Vector4D.Min(a, b);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.Min did not return the expected value.");
        }

        // A test for Max (Vector4D, Vector4D)
        [Fact]
        public void Vector4DMaxTest()
        {
            Vector4D a = new Vector4D(-1.0d, 4.0d, -3.0d, 1000.0d);
            Vector4D b = new Vector4D(2.0d, 1.0d, -1.0d, 0.0d);

            Vector4D expected = new Vector4D(2.0d, 4.0d, -1.0d, 1000.0d);
            Vector4D actual;
            actual = Vector4D.Max(a, b);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.Max did not return the expected value.");
        }

        [Fact]
        public void Vector4DMinMaxCodeCoverageTest()
        {
            Vector4D min = Vector4D.Zero;
            Vector4D max = Vector4D.One;
            Vector4D actual;

            // Min.
            actual = Vector4D.Min(min, max);
            Assert.Equal(actual, min);

            actual = Vector4D.Min(max, min);
            Assert.Equal(actual, min);

            // Max.
            actual = Vector4D.Max(min, max);
            Assert.Equal(actual, max);

            actual = Vector4D.Max(max, min);
            Assert.Equal(actual, max);
        }

        // A test for Clamp (Vector4D, Vector4D, Vector4D)
        [Fact]
        public void Vector4DClampTest()
        {
            Vector4D a = new Vector4D(0.5d, 0.3d, 0.33d, 0.44d);
            Vector4D min = new Vector4D(0.0d, 0.1d, 0.13d, 0.14d);
            Vector4D max = new Vector4D(1.0d, 1.1d, 1.13d, 1.14d);

            // Normal case.
            // Case N1: specified value is in the range.
            Vector4D expected = new Vector4D(0.5d, 0.3d, 0.33d, 0.44d);
            Vector4D actual = Vector4D.Clamp(a, min, max);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.Clamp did not return the expected value.");

            // Normal case.
            // Case N2: specified value is bigger than max value.
            a = new Vector4D(2.0d, 3.0d, 4.0d, 5.0d);
            expected = max;
            actual = Vector4D.Clamp(a, min, max);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.Clamp did not return the expected value.");

            // Case N3: specified value is smaller than max value.
            a = new Vector4D(-2.0d, -3.0d, -4.0d, -5.0d);
            expected = min;
            actual = Vector4D.Clamp(a, min, max);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.Clamp did not return the expected value.");

            // Case N4: combination case.
            a = new Vector4D(-2.0d, 0.5d, 4.0d, -5.0d);
            expected = new Vector4D(min.X, a.Y, max.Z, min.W);
            actual = Vector4D.Clamp(a, min, max);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.Clamp did not return the expected value.");

            // User specified min value is bigger than max value.
            max = new Vector4D(0.0d, 0.1d, 0.13d, 0.14d);
            min = new Vector4D(1.0d, 1.1d, 1.13d, 1.14d);

            // Case W1: specified value is in the range.
            a = new Vector4D(0.5d, 0.3d, 0.33d, 0.44d);
            expected = max;
            actual = Vector4D.Clamp(a, min, max);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.Clamp did not return the expected value.");

            // Normal case.
            // Case W2: specified value is bigger than max and min value.
            a = new Vector4D(2.0d, 3.0d, 4.0d, 5.0d);
            expected = max;
            actual = Vector4D.Clamp(a, min, max);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.Clamp did not return the expected value.");

            // Case W3: specified value is smaller than min and max value.
            a = new Vector4D(-2.0d, -3.0d, -4.0d, -5.0d);
            expected = max;
            actual = Vector4D.Clamp(a, min, max);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.Clamp did not return the expected value.");
        }

        // A test for Lerp (Vector4D, Vector4D, double)
        [Fact]
        public void Vector4DLerpTest()
        {
            Vector4D a = new Vector4D(1.0d, 2.0d, 3.0d, 4.0d);
            Vector4D b = new Vector4D(5.0d, 6.0d, 7.0d, 8.0d);

            double t = 0.5d;

            Vector4D expected = new Vector4D(3.0d, 4.0d, 5.0d, 6.0d);
            Vector4D actual;

            actual = Vector4D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector4D, Vector4D, double)
        // Lerp test with factor zero
        [Fact]
        public void Vector4DLerpTest1()
        {
            Vector4D a = new Vector4D(new Vector3D(1.0d, 2.0d, 3.0d), 4.0d);
            Vector4D b = new Vector4D(4.0d, 5.0d, 6.0d, 7.0d);

            double t = 0.0d;
            Vector4D expected = new Vector4D(1.0d, 2.0d, 3.0d, 4.0d);
            Vector4D actual = Vector4D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector4D, Vector4D, double)
        // Lerp test with factor one
        [Fact]
        public void Vector4DLerpTest2()
        {
            Vector4D a = new Vector4D(new Vector3D(1.0d, 2.0d, 3.0d), 4.0d);
            Vector4D b = new Vector4D(4.0d, 5.0d, 6.0d, 7.0d);

            double t = 1.0d;
            Vector4D expected = new Vector4D(4.0d, 5.0d, 6.0d, 7.0d);
            Vector4D actual = Vector4D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector4D, Vector4D, double)
        // Lerp test with factor > 1
        [Fact]
        public void Vector4DLerpTest3()
        {
            Vector4D a = new Vector4D(new Vector3D(0.0d, 0.0d, 0.0d), 0.0d);
            Vector4D b = new Vector4D(4.0d, 5.0d, 6.0d, 7.0d);

            double t = 2.0d;
            Vector4D expected = new Vector4D(8.0d, 10.0d, 12.0d, 14.0d);
            Vector4D actual = Vector4D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector4D, Vector4D, double)
        // Lerp test with factor < 0
        [Fact]
        public void Vector4DLerpTest4()
        {
            Vector4D a = new Vector4D(new Vector3D(0.0d, 0.0d, 0.0d), 0.0d);
            Vector4D b = new Vector4D(4.0d, 5.0d, 6.0d, 7.0d);

            double t = -2.0d;
            Vector4D expected = -(b * 2);
            Vector4D actual = Vector4D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector4D, Vector4D, double)
        // Lerp test with special double value
        [Fact]
        public void Vector4DLerpTest5()
        {
            Vector4D a = new Vector4D(45.67d, 90.0d, 0, 0);
            Vector4D b = new Vector4D(double.PositiveInfinity, double.NegativeInfinity, 0, 0);

            double t = 0.408d;
            Vector4D actual = Vector4D.Lerp(a, b, t);
            Assert.True(double.IsPositiveInfinity(actual.X), "Vector4D.Lerp did not return the expected value.");
            Assert.True(double.IsNegativeInfinity(actual.Y), "Vector4D.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector4D, Vector4D, double)
        // Lerp test from the same point
        [Fact]
        public void Vector4DLerpTest6()
        {
            Vector4D a = new Vector4D(4.0d, 5.0d, 6.0d, 7.0d);
            Vector4D b = new Vector4D(4.0d, 5.0d, 6.0d, 7.0d);

            double t = 0.85d;
            Vector4D expected = a;
            Vector4D actual = Vector4D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector4D, Vector4D, double)
        // Lerp test with values known to be inaccurate with the old lerp impl
        [Fact]
        public void Vector4DLerpTest7()
        {
            Vector4D a = new Vector4D(0.44728136d);
            Vector4D b = new Vector4D(0.46345946d);

            double t = 0.26402435d;

            Vector4D expected = new Vector4D(0.45155275d);
            Vector4D actual = Vector4D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector4D, Vector4D, double)
        // Lerp test with values known to be inaccurate with the old lerp impl
        // (Old code incorrectly gets 0.33333588)
        [Fact]
        public void Vector4DLerpTest8()
        {
            Vector4D a = new Vector4D(-100);
            Vector4D b = new Vector4D(0.33333334d);

            double t = 1d;

            Vector4D expected = new Vector4D(0.33333334d);
            Vector4D actual = Vector4D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.Lerp did not return the expected value.");
        }

        // A test for Transform (Vector2D, Matrix4x4D)
        [Fact]
        public void Vector4DTransformTest1()
        {
            Vector2D v = new Vector2D(1.0d, 2.0d);

            Matrix4x4D m =
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0d)) *
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0d)) *
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0d));
            m.M41 = 10.0d;
            m.M42 = 20.0d;
            m.M43 = 30.0d;

            Vector4D expected = new Vector4D(10.316987d, 22.183012d, 30.3660259d, 1.0d);
            Vector4D actual;

            actual = Vector4D.Transform(v, m);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.Transform did not return the expected value.");
        }

        // A test for Transform (Vector3D, Matrix4x4D)
        [Fact]
        public void Vector4DTransformTest2()
        {
            Vector3D v = new Vector3D(1.0d, 2.0d, 3.0d);

            Matrix4x4D m =
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0d)) *
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0d)) *
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0d));
            m.M41 = 10.0d;
            m.M42 = 20.0d;
            m.M43 = 30.0d;

            Vector4D expected = new Vector4D(12.19198728d, 21.53349376d, 32.61602545d, 1.0d);
            Vector4D actual;

            actual = Vector4D.Transform(v, m);
            Assert.True(MathHelper.Equal(expected, actual), "vector4.Transform did not return the expected value.");
        }

        // A test for Transform (Vector4D, Matrix4x4D)
        [Fact]
        public void Vector4DTransformVector4DTest()
        {
            Vector4D v = new Vector4D(1.0d, 2.0d, 3.0d, 0.0d);

            Matrix4x4D m =
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0d)) *
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0d)) *
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0d));
            m.M41 = 10.0d;
            m.M42 = 20.0d;
            m.M43 = 30.0d;

            Vector4D expected = new Vector4D(2.19198728d, 1.53349376d, 2.61602545d, 0.0d);
            Vector4D actual;

            actual = Vector4D.Transform(v, m);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.Transform did not return the expected value.");

            //
            v.W = 1.0d;

            expected = new Vector4D(12.19198728d, 21.53349376d, 32.61602545d, 1.0d);
            actual = Vector4D.Transform(v, m);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.Transform did not return the expected value.");
        }

        // A test for Transform (Vector4D, Matrix4x4D)
        // Transform vector4 with zero matrix
        [Fact]
        public void Vector4DTransformVector4DTest1()
        {
            Vector4D v = new Vector4D(1.0d, 2.0d, 3.0d, 0.0d);
            Matrix4x4D m = new Matrix4x4D();
            Vector4D expected = new Vector4D(0, 0, 0, 0);

            Vector4D actual = Vector4D.Transform(v, m);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.Transform did not return the expected value.");
        }

        // A test for Transform (Vector4D, Matrix4x4D)
        // Transform vector4 with identity matrix
        [Fact]
        public void Vector4DTransformVector4DTest2()
        {
            Vector4D v = new Vector4D(1.0d, 2.0d, 3.0d, 0.0d);
            Matrix4x4D m = Matrix4x4D.Identity;
            Vector4D expected = new Vector4D(1.0d, 2.0d, 3.0d, 0.0d);

            Vector4D actual = Vector4D.Transform(v, m);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.Transform did not return the expected value.");
        }

        // A test for Transform (Vector3D, Matrix4x4D)
        // Transform Vector3D test
        [Fact]
        public void Vector4DTransformVector3DTest()
        {
            Vector3D v = new Vector3D(1.0d, 2.0d, 3.0d);

            Matrix4x4D m =
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0d)) *
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0d)) *
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0d));
            m.M41 = 10.0d;
            m.M42 = 20.0d;
            m.M43 = 30.0d;

            Vector4D expected = Vector4D.Transform(new Vector4D(v, 1.0d), m);
            Vector4D actual = Vector4D.Transform(v, m);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.Transform did not return the expected value.");
        }

        // A test for Transform (Vector3D, Matrix4x4D)
        // Transform vector3 with zero matrix
        [Fact]
        public void Vector4DTransformVector3DTest1()
        {
            Vector3D v = new Vector3D(1.0d, 2.0d, 3.0d);
            Matrix4x4D m = new Matrix4x4D();
            Vector4D expected = new Vector4D(0, 0, 0, 0);

            Vector4D actual = Vector4D.Transform(v, m);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.Transform did not return the expected value.");
        }

        // A test for Transform (Vector3D, Matrix4x4D)
        // Transform vector3 with identity matrix
        [Fact]
        public void Vector4DTransformVector3DTest2()
        {
            Vector3D v = new Vector3D(1.0d, 2.0d, 3.0d);
            Matrix4x4D m = Matrix4x4D.Identity;
            Vector4D expected = new Vector4D(1.0d, 2.0d, 3.0d, 1.0d);

            Vector4D actual = Vector4D.Transform(v, m);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.Transform did not return the expected value.");
        }

        // A test for Transform (Vector2D, Matrix4x4D)
        // Transform Vector2D test
        [Fact]
        public void Vector4DTransformVector2DTest()
        {
            Vector2D v = new Vector2D(1.0d, 2.0d);

            Matrix4x4D m =
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0d)) *
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0d)) *
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0d));
            m.M41 = 10.0d;
            m.M42 = 20.0d;
            m.M43 = 30.0d;

            Vector4D expected = Vector4D.Transform(new Vector4D(v, 0.0d, 1.0d), m);
            Vector4D actual = Vector4D.Transform(v, m);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.Transform did not return the expected value.");
        }

        // A test for Transform (Vector2D, Matrix4x4D)
        // Transform Vector2D with zero matrix
        [Fact]
        public void Vector4DTransformVector2DTest1()
        {
            Vector2D v = new Vector2D(1.0d, 2.0d);
            Matrix4x4D m = new Matrix4x4D();
            Vector4D expected = new Vector4D(0, 0, 0, 0);

            Vector4D actual = Vector4D.Transform(v, m);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.Transform did not return the expected value.");
        }

        // A test for Transform (Vector2D, Matrix4x4D)
        // Transform vector2 with identity matrix
        [Fact]
        public void Vector4DTransformVector2DTest2()
        {
            Vector2D v = new Vector2D(1.0d, 2.0d);
            Matrix4x4D m = Matrix4x4D.Identity;
            Vector4D expected = new Vector4D(1.0d, 2.0d, 0, 1.0d);

            Vector4D actual = Vector4D.Transform(v, m);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.Transform did not return the expected value.");
        }

        // A test for Transform (Vector2D, QuaternionD)
        [Fact]
        public void Vector4DTransformVector2DQuatanionTest()
        {
            Vector2D v = new Vector2D(1.0d, 2.0d);

            Matrix4x4D m =
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0d)) *
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0d)) *
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0d));

            QuaternionD q = QuaternionD.CreateFromRotationMatrix(m);

            Vector4D expected = Vector4D.Transform(v, m);
            Vector4D actual;

            actual = Vector4D.Transform(v, q);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.Transform did not return the expected value.");
        }

        // A test for Transform (Vector3D, QuaternionD)
        [Fact]
        public void Vector4DTransformVector3DQuaternionD()
        {
            Vector3D v = new Vector3D(1.0d, 2.0d, 3.0d);

            Matrix4x4D m =
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0d)) *
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0d)) *
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0d));
            QuaternionD q = QuaternionD.CreateFromRotationMatrix(m);

            Vector4D expected = Vector4D.Transform(v, m);
            Vector4D actual;

            actual = Vector4D.Transform(v, q);
            Assert.True(MathHelper.Equal(expected, actual), "vector4.Transform did not return the expected value.");
        }

        // A test for Transform (Vector4D, QuaternionD)
        [Fact]
        public void Vector4DTransformVector4DQuaternionDTest()
        {
            Vector4D v = new Vector4D(1.0d, 2.0d, 3.0d, 0.0d);

            Matrix4x4D m =
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0d)) *
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0d)) *
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0d));
            QuaternionD q = QuaternionD.CreateFromRotationMatrix(m);

            Vector4D expected = Vector4D.Transform(v, m);
            Vector4D actual;

            actual = Vector4D.Transform(v, q);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.Transform did not return the expected value.");

            //
            v.W = 1.0d;
            expected.W = 1.0d;
            actual = Vector4D.Transform(v, q);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.Transform did not return the expected value.");
        }

        // A test for Transform (Vector4D, QuaternionD)
        // Transform vector4 with zero quaternion
        [Fact]
        public void Vector4DTransformVector4DQuaternionDTest1()
        {
            Vector4D v = new Vector4D(1.0d, 2.0d, 3.0d, 0.0d);
            QuaternionD q = new QuaternionD();
            Vector4D expected = Vector4D.Zero;

            Vector4D actual = Vector4D.Transform(v, q);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.Transform did not return the expected value.");
        }

        // A test for Transform (Vector4D, QuaternionD)
        // Transform vector4 with identity matrix
        [Fact]
        public void Vector4DTransformVector4DQuaternionDTest2()
        {
            Vector4D v = new Vector4D(1.0d, 2.0d, 3.0d, 0.0d);
            QuaternionD q = QuaternionD.Identity;
            Vector4D expected = new Vector4D(1.0d, 2.0d, 3.0d, 0.0d);

            Vector4D actual = Vector4D.Transform(v, q);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.Transform did not return the expected value.");
        }

        // A test for Transform (Vector3D, QuaternionD)
        // Transform Vector3D test
        [Fact]
        public void Vector4DTransformVector3DQuaternionDTest()
        {
            Vector3D v = new Vector3D(1.0d, 2.0d, 3.0d);

            Matrix4x4D m =
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0d)) *
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0d)) *
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0d));
            QuaternionD q = QuaternionD.CreateFromRotationMatrix(m);

            Vector4D expected = Vector4D.Transform(v, m);
            Vector4D actual = Vector4D.Transform(v, q);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.Transform did not return the expected value.");
        }

        // A test for Transform (Vector3D, QuaternionD)
        // Transform vector3 with zero quaternion
        [Fact]
        public void Vector4DTransformVector3DQuaternionDTest1()
        {
            Vector3D v = new Vector3D(1.0d, 2.0d, 3.0d);
            QuaternionD q = new QuaternionD();
            Vector4D expected = Vector4D.Zero;

            Vector4D actual = Vector4D.Transform(v, q);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.Transform did not return the expected value.");
        }

        // A test for Transform (Vector3D, QuaternionD)
        // Transform vector3 with identity quaternion
        [Fact]
        public void Vector4DTransformVector3DQuaternionDTest2()
        {
            Vector3D v = new Vector3D(1.0d, 2.0d, 3.0d);
            QuaternionD q = QuaternionD.Identity;
            Vector4D expected = new Vector4D(1.0d, 2.0d, 3.0d, 1.0d);

            Vector4D actual = Vector4D.Transform(v, q);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.Transform did not return the expected value.");
        }

        // A test for Transform (Vector2D, QuaternionD)
        // Transform Vector2D by quaternion test
        [Fact]
        public void Vector4DTransformVector2DQuaternionDTest()
        {
            Vector2D v = new Vector2D(1.0d, 2.0d);

            Matrix4x4D m =
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0d)) *
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0d)) *
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0d));
            QuaternionD q = QuaternionD.CreateFromRotationMatrix(m);

            Vector4D expected = Vector4D.Transform(v, m);
            Vector4D actual = Vector4D.Transform(v, q);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.Transform did not return the expected value.");
        }

        // A test for Transform (Vector2D, QuaternionD)
        // Transform Vector2D with zero quaternion
        [Fact]
        public void Vector4DTransformVector2DQuaternionDTest1()
        {
            Vector2D v = new Vector2D(1.0d, 2.0d);
            QuaternionD q = new QuaternionD();
            Vector4D expected = Vector4D.Zero;

            Vector4D actual = Vector4D.Transform(v, q);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.Transform did not return the expected value.");
        }

        // A test for Transform (Vector2D, Matrix4x4D)
        // Transform vector2 with identity QuaternionD
        [Fact]
        public void Vector4DTransformVector2DQuaternionDTest2()
        {
            Vector2D v = new Vector2D(1.0d, 2.0d);
            QuaternionD q = QuaternionD.Identity;
            Vector4D expected = new Vector4D(1.0d, 2.0d, 0, 1.0d);

            Vector4D actual = Vector4D.Transform(v, q);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.Transform did not return the expected value.");
        }

        // A test for Normalize (Vector4D)
        [Fact]
        public void Vector4DNormalizeTest()
        {
            Vector4D a = new Vector4D(1.0d, 2.0d, 3.0d, 4.0d);

            Vector4D expected = new Vector4D(
                0.1825741858350553711523232609336d,
                0.3651483716701107423046465218672d,
                0.5477225575051661134569697828008d,
                0.7302967433402214846092930437344d);
            Vector4D actual;

            actual = Vector4D.Normalize(a);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.Normalize did not return the expected value.");
        }

        // A test for Normalize (Vector4D)
        // Normalize vector of length one
        [Fact]
        public void Vector4DNormalizeTest1()
        {
            Vector4D a = new Vector4D(1.0d, 0.0d, 0.0d, 0.0d);

            Vector4D expected = new Vector4D(1.0d, 0.0d, 0.0d, 0.0d);
            Vector4D actual = Vector4D.Normalize(a);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.Normalize did not return the expected value.");
        }

        // A test for Normalize (Vector4D)
        // Normalize vector of length zero
        [Fact]
        public void Vector4DNormalizeTest2()
        {
            Vector4D a = new Vector4D(0.0d, 0.0d, 0.0d, 0.0d);

            Vector4D expected = new Vector4D(0.0d, 0.0d, 0.0d, 0.0d);
            Vector4D actual = Vector4D.Normalize(a);
            Assert.True(double.IsNaN(actual.X) && double.IsNaN(actual.Y) && double.IsNaN(actual.Z) && double.IsNaN(actual.W), "Vector4D.Normalize did not return the expected value.");
        }

        // A test for operator - (Vector4D)
        [Fact]
        public void Vector4DUnaryNegationTest()
        {
            Vector4D a = new Vector4D(1.0d, 2.0d, 3.0d, 4.0d);

            Vector4D expected = new Vector4D(-1.0d, -2.0d, -3.0d, -4.0d);
            Vector4D actual;

            actual = -a;

            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.operator - did not return the expected value.");
        }

        // A test for operator - (Vector4D, Vector4D)
        [Fact]
        public void Vector4DSubtractionTest()
        {
            Vector4D a = new Vector4D(1.0d, 6.0d, 3.0d, 4.0d);
            Vector4D b = new Vector4D(5.0d, 2.0d, 3.0d, 9.0d);

            Vector4D expected = new Vector4D(-4.0d, 4.0d, 0.0d, -5.0d);
            Vector4D actual;

            actual = a - b;

            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.operator - did not return the expected value.");
        }

        // A test for operator * (Vector4D, double)
        [Fact]
        public void Vector4DMultiplyOperatorTest()
        {
            Vector4D a = new Vector4D(1.0d, 2.0d, 3.0d, 4.0d);

            const double factor = 2.0d;

            Vector4D expected = new Vector4D(2.0d, 4.0d, 6.0d, 8.0d);
            Vector4D actual;

            actual = a * factor;
            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.operator * did not return the expected value.");
        }

        // A test for operator * (double, Vector4D)
        [Fact]
        public void Vector4DMultiplyOperatorTest2()
        {
            Vector4D a = new Vector4D(1.0d, 2.0d, 3.0d, 4.0d);

            const double factor = 2.0d;
            Vector4D expected = new Vector4D(2.0d, 4.0d, 6.0d, 8.0d);
            Vector4D actual;

            actual = factor * a;
            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.operator * did not return the expected value.");
        }

        // A test for operator * (Vector4D, Vector4D)
        [Fact]
        public void Vector4DMultiplyOperatorTest3()
        {
            Vector4D a = new Vector4D(1.0d, 2.0d, 3.0d, 4.0d);
            Vector4D b = new Vector4D(5.0d, 6.0d, 7.0d, 8.0d);

            Vector4D expected = new Vector4D(5.0d, 12.0d, 21.0d, 32.0d);
            Vector4D actual;

            actual = a * b;

            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.operator * did not return the expected value.");
        }

        // A test for operator / (Vector4D, double)
        [Fact]
        public void Vector4DDivisionTest()
        {
            Vector4D a = new Vector4D(1.0d, 2.0d, 3.0d, 4.0d);

            double div = 2.0d;

            Vector4D expected = new Vector4D(0.5d, 1.0d, 1.5d, 2.0d);
            Vector4D actual;

            actual = a / div;

            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.operator / did not return the expected value.");
        }

        // A test for operator / (Vector4D, Vector4D)
        [Fact]
        public void Vector4DDivisionTest1()
        {
            Vector4D a = new Vector4D(1.0d, 6.0d, 7.0d, 4.0d);
            Vector4D b = new Vector4D(5.0d, 2.0d, 3.0d, 8.0d);

            Vector4D expected = new Vector4D(1.0d / 5.0d, 6.0d / 2.0d, 7.0d / 3.0d, 4.0d / 8.0d);
            Vector4D actual;

            actual = a / b;

            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.operator / did not return the expected value.");
        }

        // A test for operator / (Vector4D, Vector4D)
        // Divide by zero
        [Fact]
        public void Vector4DDivisionTest2()
        {
            Vector4D a = new Vector4D(-2.0d, 3.0d, double.MaxValue, double.NaN);

            double div = 0.0d;

            Vector4D actual = a / div;

            Assert.True(double.IsNegativeInfinity(actual.X), "Vector4D.operator / did not return the expected value.");
            Assert.True(double.IsPositiveInfinity(actual.Y), "Vector4D.operator / did not return the expected value.");
            Assert.True(double.IsPositiveInfinity(actual.Z), "Vector4D.operator / did not return the expected value.");
            Assert.True(double.IsNaN(actual.W), "Vector4D.operator / did not return the expected value.");
        }

        // A test for operator / (Vector4D, Vector4D)
        // Divide by zero
        [Fact]
        public void Vector4DDivisionTest3()
        {
            Vector4D a = new Vector4D(0.047d, -3.0d, double.NegativeInfinity, double.MinValue);
            Vector4D b = new Vector4D();

            Vector4D actual = a / b;

            Assert.True(double.IsPositiveInfinity(actual.X), "Vector4D.operator / did not return the expected value.");
            Assert.True(double.IsNegativeInfinity(actual.Y), "Vector4D.operator / did not return the expected value.");
            Assert.True(double.IsNegativeInfinity(actual.Z), "Vector4D.operator / did not return the expected value.");
            Assert.True(double.IsNegativeInfinity(actual.W), "Vector4D.operator / did not return the expected value.");
        }

        // A test for operator + (Vector4D, Vector4D)
        [Fact]
        public void Vector4DAdditionTest()
        {
            Vector4D a = new Vector4D(1.0d, 2.0d, 3.0d, 4.0d);
            Vector4D b = new Vector4D(5.0d, 6.0d, 7.0d, 8.0d);

            Vector4D expected = new Vector4D(6.0d, 8.0d, 10.0d, 12.0d);
            Vector4D actual;

            actual = a + b;

            Assert.True(MathHelper.Equal(expected, actual), "Vector4D.operator + did not return the expected value.");
        }

        [Fact]
        public void OperatorAddTest()
        {
            Vector4D v1 = new Vector4D(2.5d, 2.0d, 3.0d, 3.3d);
            Vector4D v2 = new Vector4D(5.5d, 4.5d, 6.5d, 7.5d);

            Vector4D v3 = v1 + v2;
            Vector4D v5 = new Vector4D(-1.0d, 0.0d, 0.0d, double.NaN);
            Vector4D v4 = v1 + v5;
            Assert.Equal(8.0d, v3.X);
            Assert.Equal(6.5d, v3.Y);
            Assert.Equal(9.5d, v3.Z);
            Assert.Equal(10.8d, v3.W);
            Assert.Equal(1.5d, v4.X);
            Assert.Equal(2.0d, v4.Y);
            Assert.Equal(3.0d, v4.Z);
            Assert.Equal(double.NaN, v4.W);
        }

        // A test for Vector4D (double, double, double, double)
        [Fact]
        public void Vector4DConstructorTest()
        {
            double x = 1.0d;
            double y = 2.0d;
            double z = 3.0d;
            double w = 4.0d;

            Vector4D target = new Vector4D(x, y, z, w);

            Assert.True(MathHelper.Equal(target.X, x) && MathHelper.Equal(target.Y, y) && MathHelper.Equal(target.Z, z) && MathHelper.Equal(target.W, w),
                "Vector4D constructor(x,y,z,w) did not return the expected value.");
        }

        // A test for Vector4D (Vector2D, double, double)
        [Fact]
        public void Vector4DConstructorTest1()
        {
            Vector2D a = new Vector2D(1.0d, 2.0d);
            double z = 3.0d;
            double w = 4.0d;

            Vector4D target = new Vector4D(a, z, w);
            Assert.True(MathHelper.Equal(target.X, a.X) && MathHelper.Equal(target.Y, a.Y) && MathHelper.Equal(target.Z, z) && MathHelper.Equal(target.W, w),
                "Vector4D constructor(Vector2D,z,w) did not return the expected value.");
        }

        // A test for Vector4D (Vector3D, double)
        [Fact]
        public void Vector4DConstructorTest2()
        {
            Vector3D a = new Vector3D(1.0d, 2.0d, 3.0d);
            double w = 4.0d;

            Vector4D target = new Vector4D(a, w);

            Assert.True(MathHelper.Equal(target.X, a.X) && MathHelper.Equal(target.Y, a.Y) && MathHelper.Equal(target.Z, a.Z) && MathHelper.Equal(target.W, w),
                "Vector4D constructor(Vector3D,w) did not return the expected value.");
        }

        // A test for Vector4D ()
        // Constructor with no parameter
        [Fact]
        public void Vector4DConstructorTest4()
        {
            Vector4D a = new Vector4D();

            Assert.Equal(0.0d, a.X);
            Assert.Equal(0.0d, a.Y);
            Assert.Equal(0.0d, a.Z);
            Assert.Equal(0.0d, a.W);
        }

        // A test for Vector4D ()
        // Constructor with special floating values
        [Fact]
        public void Vector4DConstructorTest5()
        {
            Vector4D target = new Vector4D(double.NaN, double.MaxValue, double.PositiveInfinity, double.Epsilon);

            Assert.True(double.IsNaN(target.X), "Vector4D.constructor (double, double, double, double) did not return the expected value.");
            Assert.True(double.Equals(double.MaxValue, target.Y), "Vector4D.constructor (double, double, double, double) did not return the expected value.");
            Assert.True(double.IsPositiveInfinity(target.Z), "Vector4D.constructor (double, double, double, double) did not return the expected value.");
            Assert.True(double.Equals(double.Epsilon, target.W), "Vector4D.constructor (double, double, double, double) did not return the expected value.");
        }

        // A test for Vector4D (ReadOnlySpan<double>)
        [Fact]
        public void Vector4DConstructorTest7()
        {
            double value = 1.0d;
            Vector4D target = new Vector4D(new[] { value, value, value, value });
            Vector4D expected = new Vector4D(value);

            Assert.Equal(expected, target);
            Assert.Throws<ArgumentOutOfRangeException>(() => new Vector4D(new double[3]));
        }

        // A test for Add (Vector4D, Vector4D)
        [Fact]
        public void Vector4DAddTest()
        {
            Vector4D a = new Vector4D(1.0d, 2.0d, 3.0d, 4.0d);
            Vector4D b = new Vector4D(5.0d, 6.0d, 7.0d, 8.0d);

            Vector4D expected = new Vector4D(6.0d, 8.0d, 10.0d, 12.0d);
            Vector4D actual;

            actual = Vector4D.Add(a, b);
            Assert.Equal(expected, actual);
        }

        // A test for Divide (Vector4D, double)
        [Fact]
        public void Vector4DDivideTest()
        {
            Vector4D a = new Vector4D(1.0d, 2.0d, 3.0d, 4.0d);
            double div = 2.0d;
            Vector4D expected = new Vector4D(0.5d, 1.0d, 1.5d, 2.0d);
            Vector4D actual;
            actual = Vector4D.Divide(a, div);
            Assert.Equal(expected, actual);
        }

        // A test for Divide (Vector4D, Vector4D)
        [Fact]
        public void Vector4DDivideTest1()
        {
            Vector4D a = new Vector4D(1.0d, 6.0d, 7.0d, 4.0d);
            Vector4D b = new Vector4D(5.0d, 2.0d, 3.0d, 8.0d);

            Vector4D expected = new Vector4D(1.0d / 5.0d, 6.0d / 2.0d, 7.0d / 3.0d, 4.0d / 8.0d);
            Vector4D actual;

            actual = Vector4D.Divide(a, b);
            Assert.Equal(expected, actual);
        }

        // A test for Equals (object)
        [Fact]
        public void Vector4DEqualsTest()
        {
            Vector4D a = new Vector4D(1.0d, 2.0d, 3.0d, 4.0d);
            Vector4D b = new Vector4D(1.0d, 2.0d, 3.0d, 4.0d);

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

        // A test for Multiply (double, Vector4D)
        [Fact]
        public void Vector4DMultiplyTest()
        {
            Vector4D a = new Vector4D(1.0d, 2.0d, 3.0d, 4.0d);
            const double factor = 2.0d;
            Vector4D expected = new Vector4D(2.0d, 4.0d, 6.0d, 8.0d);
            Vector4D actual = Vector4D.Multiply(factor, a);
            Assert.Equal(expected, actual);
        }

        // A test for Multiply (Vector4D, double)
        [Fact]
        public void Vector4DMultiplyTest2()
        {
            Vector4D a = new Vector4D(1.0d, 2.0d, 3.0d, 4.0d);
            const double factor = 2.0d;
            Vector4D expected = new Vector4D(2.0d, 4.0d, 6.0d, 8.0d);
            Vector4D actual = Vector4D.Multiply(a, factor);
            Assert.Equal(expected, actual);
        }

        // A test for Multiply (Vector4D, Vector4D)
        [Fact]
        public void Vector4DMultiplyTest3()
        {
            Vector4D a = new Vector4D(1.0d, 2.0d, 3.0d, 4.0d);
            Vector4D b = new Vector4D(5.0d, 6.0d, 7.0d, 8.0d);

            Vector4D expected = new Vector4D(5.0d, 12.0d, 21.0d, 32.0d);
            Vector4D actual;

            actual = Vector4D.Multiply(a, b);
            Assert.Equal(expected, actual);
        }

        // A test for Negate (Vector4D)
        [Fact]
        public void Vector4DNegateTest()
        {
            Vector4D a = new Vector4D(1.0d, 2.0d, 3.0d, 4.0d);

            Vector4D expected = new Vector4D(-1.0d, -2.0d, -3.0d, -4.0d);
            Vector4D actual;

            actual = Vector4D.Negate(a);
            Assert.Equal(expected, actual);
        }

        // A test for operator != (Vector4D, Vector4D)
        [Fact]
        public void Vector4DInequalityTest()
        {
            Vector4D a = new Vector4D(1.0d, 2.0d, 3.0d, 4.0d);
            Vector4D b = new Vector4D(1.0d, 2.0d, 3.0d, 4.0d);

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

        // A test for operator == (Vector4D, Vector4D)
        [Fact]
        public void Vector4DEqualityTest()
        {
            Vector4D a = new Vector4D(1.0d, 2.0d, 3.0d, 4.0d);
            Vector4D b = new Vector4D(1.0d, 2.0d, 3.0d, 4.0d);

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

        // A test for Subtract (Vector4D, Vector4D)
        [Fact]
        public void Vector4DSubtractTest()
        {
            Vector4D a = new Vector4D(1.0d, 6.0d, 3.0d, 4.0d);
            Vector4D b = new Vector4D(5.0d, 2.0d, 3.0d, 9.0d);

            Vector4D expected = new Vector4D(-4.0d, 4.0d, 0.0d, -5.0d);
            Vector4D actual;

            actual = Vector4D.Subtract(a, b);

            Assert.Equal(expected, actual);
        }

        // A test for UnitW
        [Fact]
        public void Vector4DUnitWTest()
        {
            Vector4D val = new Vector4D(0.0d, 0.0d, 0.0d, 1.0d);
            Assert.Equal(val, Vector4D.UnitW);
        }

        // A test for UnitX
        [Fact]
        public void Vector4DUnitXTest()
        {
            Vector4D val = new Vector4D(1.0d, 0.0d, 0.0d, 0.0d);
            Assert.Equal(val, Vector4D.UnitX);
        }

        // A test for UnitY
        [Fact]
        public void Vector4DUnitYTest()
        {
            Vector4D val = new Vector4D(0.0d, 1.0d, 0.0d, 0.0d);
            Assert.Equal(val, Vector4D.UnitY);
        }

        // A test for UnitZ
        [Fact]
        public void Vector4DUnitZTest()
        {
            Vector4D val = new Vector4D(0.0d, 0.0d, 1.0d, 0.0d);
            Assert.Equal(val, Vector4D.UnitZ);
        }

        // A test for One
        [Fact]
        public void Vector4DOneTest()
        {
            Vector4D val = new Vector4D(1.0d, 1.0d, 1.0d, 1.0d);
            Assert.Equal(val, Vector4D.One);
        }

        // A test for Zero
        [Fact]
        public void Vector4DZeroTest()
        {
            Vector4D val = new Vector4D(0.0d, 0.0d, 0.0d, 0.0d);
            Assert.Equal(val, Vector4D.Zero);
        }

        // A test for Equals (Vector4D)
        [Fact]
        public void Vector4DEqualsTest1()
        {
            Vector4D a = new Vector4D(1.0d, 2.0d, 3.0d, 4.0d);
            Vector4D b = new Vector4D(1.0d, 2.0d, 3.0d, 4.0d);

            // case 1: compare between same values
            Assert.True(a.Equals(b));

            // case 2: compare between different values
            b.X = 10.0d;
            Assert.False(a.Equals(b));
        }

        // A test for Vector4D (double)
        [Fact]
        public void Vector4DConstructorTest6()
        {
            double value = 1.0d;
            Vector4D target = new Vector4D(value);

            Vector4D expected = new Vector4D(value, value, value, value);
            Assert.Equal(expected, target);

            value = 2.0d;
            target = new Vector4D(value);
            expected = new Vector4D(value, value, value, value);
            Assert.Equal(expected, target);
        }

        // A test for Vector4D comparison involving NaN values
        [Fact]
        public void Vector4DEqualsNaNTest()
        {
            Vector4D a = new Vector4D(double.NaN, 0, 0, 0);
            Vector4D b = new Vector4D(0, double.NaN, 0, 0);
            Vector4D c = new Vector4D(0, 0, double.NaN, 0);
            Vector4D d = new Vector4D(0, 0, 0, double.NaN);

            Assert.False(a == Vector4D.Zero);
            Assert.False(b == Vector4D.Zero);
            Assert.False(c == Vector4D.Zero);
            Assert.False(d == Vector4D.Zero);

            Assert.True(a != Vector4D.Zero);
            Assert.True(b != Vector4D.Zero);
            Assert.True(c != Vector4D.Zero);
            Assert.True(d != Vector4D.Zero);

            Assert.False(a.Equals(Vector4D.Zero));
            Assert.False(b.Equals(Vector4D.Zero));
            Assert.False(c.Equals(Vector4D.Zero));
            Assert.False(d.Equals(Vector4D.Zero));

            Assert.True(a.Equals(a));
            Assert.True(b.Equals(b));
            Assert.True(c.Equals(c));
            Assert.True(d.Equals(d));
        }

        [Fact]
        public void Vector4DAbsTest()
        {
            Vector4D v1 = new Vector4D(-2.5d, 2.0d, 3.0d, 3.3d);
            Vector4D v3 = Vector4D.Abs(new Vector4D(double.PositiveInfinity, 0.0d, double.NegativeInfinity, double.NaN));
            Vector4D v = Vector4D.Abs(v1);
            Assert.Equal(2.5d, v.X);
            Assert.Equal(2.0d, v.Y);
            Assert.Equal(3.0d, v.Z);
            Assert.Equal(3.3d, v.W);
            Assert.Equal(double.PositiveInfinity, v3.X);
            Assert.Equal(0.0d, v3.Y);
            Assert.Equal(double.PositiveInfinity, v3.Z);
            Assert.Equal(double.NaN, v3.W);
        }

        [Fact]
        public void Vector4DSqrtTest()
        {
            Vector4D v1 = new Vector4D(-2.5d, 2.0d, 3.0d, 3.3d);
            Vector4D v2 = new Vector4D(5.5d, 4.5d, 6.5d, 7.5d);
            Assert.Equal(2, (int)Vector4D.SquareRoot(v2).X);
            Assert.Equal(2, (int)Vector4D.SquareRoot(v2).Y);
            Assert.Equal(2, (int)Vector4D.SquareRoot(v2).Z);
            Assert.Equal(2, (int)Vector4D.SquareRoot(v2).W);
            Assert.Equal(double.NaN, Vector4D.SquareRoot(v1).X);
        }

        // A test to make sure these types are blittable directly into GPU buffer memory layouts
        [Fact]
        public unsafe void Vector4DSizeofTest()
        {
            Assert.Equal(32, sizeof(Vector4D));
            Assert.Equal(64, sizeof(Vector4D_2x));
            Assert.Equal(40, sizeof(Vector4DPlusDouble));
            Assert.Equal(80, sizeof(Vector4DPlusDouble_2x));
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Vector4D_2x
        {
            private Vector4D _a;
            private Vector4D _b;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Vector4DPlusDouble
        {
            private Vector4D _v;
            private double _f;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Vector4DPlusDouble_2x
        {
            private Vector4DPlusDouble _a;
            private Vector4DPlusDouble _b;
        }

        [Fact]
        public void SetFieldsTest()
        {
            Vector4D v3 = new Vector4D(4d, 5d, 6d, 7d);
            v3.X = 1.0d;
            v3.Y = 2.0d;
            v3.Z = 3.0d;
            v3.W = 4.0d;
            Assert.Equal(1.0d, v3.X);
            Assert.Equal(2.0d, v3.Y);
            Assert.Equal(3.0d, v3.Z);
            Assert.Equal(4.0d, v3.W);
            Vector4D v4 = v3;
            v4.Y = 0.5d;
            v4.Z = 2.2d;
            v4.W = 3.5d;
            Assert.Equal(1.0d, v4.X);
            Assert.Equal(0.5d, v4.Y);
            Assert.Equal(2.2d, v4.Z);
            Assert.Equal(3.5d, v4.W);
            Assert.Equal(2.0d, v3.Y);
        }

        [Fact]
        public void EmbeddedVectorSetFields()
        {
            EmbeddedVectorObject evo = new EmbeddedVectorObject();
            evo.FieldVector.X = 5.0d;
            evo.FieldVector.Y = 5.0d;
            evo.FieldVector.Z = 5.0d;
            evo.FieldVector.W = 5.0d;
            Assert.Equal(5.0d, evo.FieldVector.X);
            Assert.Equal(5.0d, evo.FieldVector.Y);
            Assert.Equal(5.0d, evo.FieldVector.Z);
            Assert.Equal(5.0d, evo.FieldVector.W);
        }

        [Fact]
        public void DeeplyEmbeddedObjectTest()
        {
            DeeplyEmbeddedClass obj = new DeeplyEmbeddedClass();
            obj.L0.L1.L2.L3.L4.L5.L6.L7.EmbeddedVector.X = 5d;
            Assert.Equal(5d, obj.RootEmbeddedObject.X);
            Assert.Equal(5d, obj.RootEmbeddedObject.Y);
            Assert.Equal(1d, obj.RootEmbeddedObject.Z);
            Assert.Equal(-5d, obj.RootEmbeddedObject.W);
            obj.L0.L1.L2.L3.L4.L5.L6.L7.EmbeddedVector = new Vector4D(1, 2, 3, 4);
            Assert.Equal(1d, obj.RootEmbeddedObject.X);
            Assert.Equal(2d, obj.RootEmbeddedObject.Y);
            Assert.Equal(3d, obj.RootEmbeddedObject.Z);
            Assert.Equal(4d, obj.RootEmbeddedObject.W);
        }

        [Fact]
        public void DeeplyEmbeddedStructTest()
        {
            DeeplyEmbeddedStruct obj = DeeplyEmbeddedStruct.Create();
            obj.L0.L1.L2.L3.L4.L5.L6.L7.EmbeddedVector.X = 5d;
            Assert.Equal(5d, obj.RootEmbeddedObject.X);
            Assert.Equal(5d, obj.RootEmbeddedObject.Y);
            Assert.Equal(1d, obj.RootEmbeddedObject.Z);
            Assert.Equal(-5d, obj.RootEmbeddedObject.W);
            obj.L0.L1.L2.L3.L4.L5.L6.L7.EmbeddedVector = new Vector4D(1, 2, 3, 4);
            Assert.Equal(1d, obj.RootEmbeddedObject.X);
            Assert.Equal(2d, obj.RootEmbeddedObject.Y);
            Assert.Equal(3d, obj.RootEmbeddedObject.Z);
            Assert.Equal(4d, obj.RootEmbeddedObject.W);
        }

        private class EmbeddedVectorObject
        {
            public Vector4D FieldVector;
        }

        private class DeeplyEmbeddedClass
        {
            public readonly Level0 L0 = new Level0();
            public Vector4D RootEmbeddedObject { get { return L0.L1.L2.L3.L4.L5.L6.L7.EmbeddedVector; } }
            public class Level0
            {
                public readonly Level1 L1 = new Level1();
                public class Level1
                {
                    public readonly Level2 L2 = new Level2();
                    public class Level2
                    {
                        public readonly Level3 L3 = new Level3();
                        public class Level3
                        {
                            public readonly Level4 L4 = new Level4();
                            public class Level4
                            {
                                public readonly Level5 L5 = new Level5();
                                public class Level5
                                {
                                    public readonly Level6 L6 = new Level6();
                                    public class Level6
                                    {
                                        public readonly Level7 L7 = new Level7();
                                        public class Level7
                                        {
                                            public Vector4D EmbeddedVector = new Vector4D(1, 5, 1, -5);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        // Contrived test for strangely-sized and shaped embedded structures, with unused buffer fields.
#pragma warning disable 0169
        private struct DeeplyEmbeddedStruct
        {
            public static DeeplyEmbeddedStruct Create()
            {
                var obj = new DeeplyEmbeddedStruct();
                obj.L0 = new Level0();
                obj.L0.L1 = new Level0.Level1();
                obj.L0.L1.L2 = new Level0.Level1.Level2();
                obj.L0.L1.L2.L3 = new Level0.Level1.Level2.Level3();
                obj.L0.L1.L2.L3.L4 = new Level0.Level1.Level2.Level3.Level4();
                obj.L0.L1.L2.L3.L4.L5 = new Level0.Level1.Level2.Level3.Level4.Level5();
                obj.L0.L1.L2.L3.L4.L5.L6 = new Level0.Level1.Level2.Level3.Level4.Level5.Level6();
                obj.L0.L1.L2.L3.L4.L5.L6.L7 = new Level0.Level1.Level2.Level3.Level4.Level5.Level6.Level7();
                obj.L0.L1.L2.L3.L4.L5.L6.L7.EmbeddedVector = new Vector4D(1, 5, 1, -5);

                return obj;
            }

            public Level0 L0;
            public Vector4D RootEmbeddedObject { get { return L0.L1.L2.L3.L4.L5.L6.L7.EmbeddedVector; } }
            public struct Level0
            {
                private double _buffer0, _buffer1;
                public Level1 L1;
                private double _buffer2;
                public struct Level1
                {
                    private double _buffer0, _buffer1;
                    public Level2 L2;
                    private byte _buffer2;
                    public struct Level2
                    {
                        public Level3 L3;
                        private double _buffer0;
                        private byte _buffer1;
                        public struct Level3
                        {
                            public Level4 L4;
                            public struct Level4
                            {
                                private double _buffer0;
                                public Level5 L5;
                                private long _buffer1;
                                private byte _buffer2;
                                private double _buffer3;
                                public struct Level5
                                {
                                    private byte _buffer0;
                                    public Level6 L6;
                                    public struct Level6
                                    {
                                        private byte _buffer0;
                                        public Level7 L7;
                                        private byte _buffer1, _buffer2;
                                        public struct Level7
                                        {
                                            public Vector4D EmbeddedVector;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
#pragma warning restore 0169

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.CosDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void CosDoubleTest(double value, double expectedResult, double variance)
        {
            Vector4D actualResult = Vector4D.Cos(Vector4D.Create(value));
            AssertEqual(Vector4D.Create(expectedResult), actualResult, Vector4D.Create(variance));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.ExpDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void ExpDoubleTest(double value, double expectedResult, double variance)
        {
            Vector4D actualResult = Vector4D.Exp(Vector4D.Create(value));
            AssertEqual(Vector4D.Create(expectedResult), actualResult, Vector4D.Create(variance));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.LogDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void LogDoubleTest(double value, double expectedResult, double variance)
        {
            Vector4D actualResult = Vector4D.Log(Vector4D.Create(value));
            AssertEqual(Vector4D.Create(expectedResult), actualResult, Vector4D.Create(variance));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.Log2Double), MemberType = typeof(GenericMathTestMemberData))]
        public void Log2DoubleTest(double value, double expectedResult, double variance)
        {
            Vector4D actualResult = Vector4D.Log2(Vector4D.Create(value));
            AssertEqual(Vector4D.Create(expectedResult), actualResult, Vector4D.Create(variance));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.FusedMultiplyAddDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void FusedMultiplyAddDoubleTest(double left, double right, double addend, double expectedResult)
        {
            AssertEqual(Vector4D.Create(expectedResult), Vector4D.FusedMultiplyAdd(Vector4D.Create(left), Vector4D.Create(right), Vector4D.Create(addend)), Vector4D.Zero);
            AssertEqual(Vector4D.Create(double.MultiplyAddEstimate(left, right, addend)), Vector4D.MultiplyAddEstimate(Vector4D.Create(left), Vector4D.Create(right), Vector4D.Create(addend)), Vector4D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.ClampDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void ClampDoubleTest(double x, double min, double max, double expectedResult)
        {
            Vector4D actualResult = Vector4D.Clamp(Vector4D.Create(x), Vector4D.Create(min), Vector4D.Create(max));
            AssertEqual(Vector4D.Create(expectedResult), actualResult, Vector4D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.CopySignDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void CopySignDoubleTest(double x, double y, double expectedResult)
        {
            Vector4D actualResult = Vector4D.CopySign(Vector4D.Create(x), Vector4D.Create(y));
            AssertEqual(Vector4D.Create(expectedResult), actualResult, Vector4D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.DegreesToRadiansDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void DegreesToRadiansDoubleTest(double value, double expectedResult, double variance)
        {
            AssertEqual(Vector4D.Create(-expectedResult), Vector4D.DegreesToRadians(Vector4D.Create(-value)), Vector4D.Create(variance));
            AssertEqual(Vector4D.Create(+expectedResult), Vector4D.DegreesToRadians(Vector4D.Create(+value)), Vector4D.Create(variance));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.HypotDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void HypotDoubleTest(double x, double y, double expectedResult, double variance)
        {
            AssertEqual(Vector4D.Create(expectedResult), Vector4D.Hypot(Vector4D.Create(-x), Vector4D.Create(-y)), Vector4D.Create(variance));
            AssertEqual(Vector4D.Create(expectedResult), Vector4D.Hypot(Vector4D.Create(-x), Vector4D.Create(+y)), Vector4D.Create(variance));
            AssertEqual(Vector4D.Create(expectedResult), Vector4D.Hypot(Vector4D.Create(+x), Vector4D.Create(-y)), Vector4D.Create(variance));
            AssertEqual(Vector4D.Create(expectedResult), Vector4D.Hypot(Vector4D.Create(+x), Vector4D.Create(+y)), Vector4D.Create(variance));

            AssertEqual(Vector4D.Create(expectedResult), Vector4D.Hypot(Vector4D.Create(-y), Vector4D.Create(-x)), Vector4D.Create(variance));
            AssertEqual(Vector4D.Create(expectedResult), Vector4D.Hypot(Vector4D.Create(-y), Vector4D.Create(+x)), Vector4D.Create(variance));
            AssertEqual(Vector4D.Create(expectedResult), Vector4D.Hypot(Vector4D.Create(+y), Vector4D.Create(-x)), Vector4D.Create(variance));
            AssertEqual(Vector4D.Create(expectedResult), Vector4D.Hypot(Vector4D.Create(+y), Vector4D.Create(+x)), Vector4D.Create(variance));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.LerpDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void LerpDoubleTest(double x, double y, double amount, double expectedResult)
        {
            AssertEqual(Vector4D.Create(+expectedResult), Vector4D.Lerp(Vector4D.Create(+x), Vector4D.Create(+y), Vector4D.Create(amount)), Vector4D.Zero);
            AssertEqual(Vector4D.Create((expectedResult == 0.0d) ? expectedResult : -expectedResult), Vector4D.Lerp(Vector4D.Create(-x), Vector4D.Create(-y), Vector4D.Create(amount)), Vector4D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MaxDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void MaxDoubleTest(double x, double y, double expectedResult)
        {
            Vector4D actualResult = Vector4D.Max(Vector4D.Create(x), Vector4D.Create(y));
            AssertEqual(Vector4D.Create(expectedResult), actualResult, Vector4D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MaxMagnitudeDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void MaxMagnitudeDoubleTest(double x, double y, double expectedResult)
        {
            Vector4D actualResult = Vector4D.MaxMagnitude(Vector4D.Create(x), Vector4D.Create(y));
            AssertEqual(Vector4D.Create(expectedResult), actualResult, Vector4D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MaxMagnitudeNumberDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void MaxMagnitudeNumberDoubleTest(double x, double y, double expectedResult)
        {
            Vector4D actualResult = Vector4D.MaxMagnitudeNumber(Vector4D.Create(x), Vector4D.Create(y));
            AssertEqual(Vector4D.Create(expectedResult), actualResult, Vector4D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MaxNumberDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void MaxNumberDoubleTest(double x, double y, double expectedResult)
        {
            Vector4D actualResult = Vector4D.MaxNumber(Vector4D.Create(x), Vector4D.Create(y));
            AssertEqual(Vector4D.Create(expectedResult), actualResult, Vector4D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MinDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void MinDoubleTest(double x, double y, double expectedResult)
        {
            Vector4D actualResult = Vector4D.Min(Vector4D.Create(x), Vector4D.Create(y));
            AssertEqual(Vector4D.Create(expectedResult), actualResult, Vector4D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MinMagnitudeDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void MinMagnitudeDoubleTest(double x, double y, double expectedResult)
        {
            Vector4D actualResult = Vector4D.MinMagnitude(Vector4D.Create(x), Vector4D.Create(y));
            AssertEqual(Vector4D.Create(expectedResult), actualResult, Vector4D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MinMagnitudeNumberDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void MinMagnitudeNumberDoubleTest(double x, double y, double expectedResult)
        {
            Vector4D actualResult = Vector4D.MinMagnitudeNumber(Vector4D.Create(x), Vector4D.Create(y));
            AssertEqual(Vector4D.Create(expectedResult), actualResult, Vector4D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MinNumberDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void MinNumberDoubleTest(double x, double y, double expectedResult)
        {
            Vector4D actualResult = Vector4D.MinNumber(Vector4D.Create(x), Vector4D.Create(y));
            AssertEqual(Vector4D.Create(expectedResult), actualResult, Vector4D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.RadiansToDegreesDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void RadiansToDegreesDoubleTest(double value, double expectedResult, double variance)
        {
            AssertEqual(Vector4D.Create(-expectedResult), Vector4D.RadiansToDegrees(Vector4D.Create(-value)), Vector4D.Create(variance));
            AssertEqual(Vector4D.Create(+expectedResult), Vector4D.RadiansToDegrees(Vector4D.Create(+value)), Vector4D.Create(variance));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.RoundDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void RoundDoubleTest(double value, double expectedResult)
        {
            Vector4D actualResult = Vector4D.Round(Vector4D.Create(value));
            AssertEqual(Vector4D.Create(expectedResult), actualResult, Vector4D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.RoundAwayFromZeroDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void RoundAwayFromZeroDoubleTest(double value, double expectedResult)
        {
            Vector4D actualResult = Vector4D.Round(Vector4D.Create(value), MidpointRounding.AwayFromZero);
            AssertEqual(Vector4D.Create(expectedResult), actualResult, Vector4D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.RoundToEvenDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void RoundToEvenDoubleTest(double value, double expectedResult)
        {
            Vector4D actualResult = Vector4D.Round(Vector4D.Create(value), MidpointRounding.ToEven);
            AssertEqual(Vector4D.Create(expectedResult), actualResult, Vector4D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.SinDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void SinDoubleTest(double value, double expectedResult, double variance)
        {
            Vector4D actualResult = Vector4D.Sin(Vector4D.Create(value));
            AssertEqual(Vector4D.Create(expectedResult), actualResult, Vector4D.Create(variance));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.SinCosDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void SinCosDoubleTest(double value, double expectedResultSin, double expectedResultCos, double allowedVarianceSin, double allowedVarianceCos)
        {
            (Vector4D resultSin, Vector4D resultCos) = Vector4D.SinCos(Vector4D.Create(value));
            AssertEqual(Vector4D.Create(expectedResultSin), resultSin, Vector4D.Create(allowedVarianceSin));
            AssertEqual(Vector4D.Create(expectedResultCos), resultCos, Vector4D.Create(allowedVarianceCos));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.TruncateDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void TruncateDoubleTest(double value, double expectedResult)
        {
            Vector4D actualResult = Vector4D.Truncate(Vector4D.Create(value));
            AssertEqual(Vector4D.Create(expectedResult), actualResult, Vector4D.Zero);
        }

        [Fact]
        public void AllAnyNoneTest()
        {
            Test(3, 2);

            [MethodImpl(MethodImplOptions.NoInlining)]
            void Test(double value1, double value2)
            {
                var input1 = Vector4D.Create(value1);
                var input2 = Vector4D.Create(value2);

                Assert.True(Vector4D.All(input1, value1));
                Assert.True(Vector4D.All(input2, value2));
                Assert.False(Vector4D.All(input1.WithElement(0, value2), value1));
                Assert.False(Vector4D.All(input2.WithElement(0, value1), value2));
                Assert.False(Vector4D.All(input1, value2));
                Assert.False(Vector4D.All(input2, value1));
                Assert.False(Vector4D.All(input1.WithElement(0, value2), value2));
                Assert.False(Vector4D.All(input2.WithElement(0, value1), value1));

                Assert.True(Vector4D.Any(input1, value1));
                Assert.True(Vector4D.Any(input2, value2));
                Assert.True(Vector4D.Any(input1.WithElement(0, value2), value1));
                Assert.True(Vector4D.Any(input2.WithElement(0, value1), value2));
                Assert.False(Vector4D.Any(input1, value2));
                Assert.False(Vector4D.Any(input2, value1));
                Assert.True(Vector4D.Any(input1.WithElement(0, value2), value2));
                Assert.True(Vector4D.Any(input2.WithElement(0, value1), value1));

                Assert.False(Vector4D.None(input1, value1));
                Assert.False(Vector4D.None(input2, value2));
                Assert.False(Vector4D.None(input1.WithElement(0, value2), value1));
                Assert.False(Vector4D.None(input2.WithElement(0, value1), value2));
                Assert.True(Vector4D.None(input1, value2));
                Assert.True(Vector4D.None(input2, value1));
                Assert.False(Vector4D.None(input1.WithElement(0, value2), value2));
                Assert.False(Vector4D.None(input2.WithElement(0, value1), value1));
            }
        }

        [Fact]
        public void AllAnyNoneTest_AllBitsSet()
        {
            Test(BitConverter.Int64BitsToDouble(-1));

            [MethodImpl(MethodImplOptions.NoInlining)]
            void Test(double value)
            {
                var input = Vector4D.Create(value);

                Assert.False(Vector4D.All(input, value));
                Assert.False(Vector4D.Any(input, value));
                Assert.True(Vector4D.None(input, value));
            }
        }

        [Fact]
        public void AllAnyNoneWhereAllBitsSetTest()
        {
            Test(BitConverter.Int64BitsToDouble(-1), 2);

            [MethodImpl(MethodImplOptions.NoInlining)]
            void Test(double allBitsSet, double value2)
            {
                var input1 = Vector4D.Create(allBitsSet);
                var input2 = Vector4D.Create(value2);

                Assert.True(Vector4D.AllWhereAllBitsSet(input1));
                Assert.False(Vector4D.AllWhereAllBitsSet(input2));
                Assert.False(Vector4D.AllWhereAllBitsSet(input1.WithElement(0, value2)));
                Assert.False(Vector4D.AllWhereAllBitsSet(input2.WithElement(0, allBitsSet)));

                Assert.True(Vector4D.AnyWhereAllBitsSet(input1));
                Assert.False(Vector4D.AnyWhereAllBitsSet(input2));
                Assert.True(Vector4D.AnyWhereAllBitsSet(input1.WithElement(0, value2)));
                Assert.True(Vector4D.AnyWhereAllBitsSet(input2.WithElement(0, allBitsSet)));

                Assert.False(Vector4D.NoneWhereAllBitsSet(input1));
                Assert.True(Vector4D.NoneWhereAllBitsSet(input2));
                Assert.False(Vector4D.NoneWhereAllBitsSet(input1.WithElement(0, value2)));
                Assert.False(Vector4D.NoneWhereAllBitsSet(input2.WithElement(0, allBitsSet)));
            }
        }

        [Fact]
        public void CountIndexOfLastIndexOfSingleTest()
        {
            Test(3, 2);

            [MethodImpl(MethodImplOptions.NoInlining)]
            void Test(double value1, double value2)
            {
                var input1 = Vector4D.Create(value1);
                var input2 = Vector4D.Create(value2);

                Assert.Equal(ElementCount, Vector4D.Count(input1, value1));
                Assert.Equal(ElementCount, Vector4D.Count(input2, value2));
                Assert.Equal(ElementCount - 1, Vector4D.Count(input1.WithElement(0, value2), value1));
                Assert.Equal(ElementCount - 1, Vector4D.Count(input2.WithElement(0, value1), value2));
                Assert.Equal(0, Vector4D.Count(input1, value2));
                Assert.Equal(0, Vector4D.Count(input2, value1));
                Assert.Equal(1, Vector4D.Count(input1.WithElement(0, value2), value2));
                Assert.Equal(1, Vector4D.Count(input2.WithElement(0, value1), value1));

                Assert.Equal(0, Vector4D.IndexOf(input1, value1));
                Assert.Equal(0, Vector4D.IndexOf(input2, value2));
                Assert.Equal(1, Vector4D.IndexOf(input1.WithElement(0, value2), value1));
                Assert.Equal(1, Vector4D.IndexOf(input2.WithElement(0, value1), value2));
                Assert.Equal(-1, Vector4D.IndexOf(input1, value2));
                Assert.Equal(-1, Vector4D.IndexOf(input2, value1));
                Assert.Equal(0, Vector4D.IndexOf(input1.WithElement(0, value2), value2));
                Assert.Equal(0, Vector4D.IndexOf(input2.WithElement(0, value1), value1));

                Assert.Equal(ElementCount - 1, Vector4D.LastIndexOf(input1, value1));
                Assert.Equal(ElementCount - 1, Vector4D.LastIndexOf(input2, value2));
                Assert.Equal(ElementCount - 1, Vector4D.LastIndexOf(input1.WithElement(0, value2), value1));
                Assert.Equal(ElementCount - 1, Vector4D.LastIndexOf(input2.WithElement(0, value1), value2));
                Assert.Equal(-1, Vector4D.LastIndexOf(input1, value2));
                Assert.Equal(-1, Vector4D.LastIndexOf(input2, value1));
                Assert.Equal(0, Vector4D.LastIndexOf(input1.WithElement(0, value2), value2));
                Assert.Equal(0, Vector4D.LastIndexOf(input2.WithElement(0, value1), value1));
            }
        }

        [Fact]
        public void CountIndexOfLastIndexOfSingleTest_AllBitsSet()
        {
            Test(BitConverter.Int64BitsToDouble(-1));

            [MethodImpl(MethodImplOptions.NoInlining)]
            void Test(double value)
            {
                var input = Vector4D.Create(value);

                Assert.Equal(0, Vector4D.Count(input, value));
                Assert.Equal(-1, Vector4D.IndexOf(input, value));
                Assert.Equal(-1, Vector4D.LastIndexOf(input, value));
            }
        }

        [Fact]
        public void CountIndexOfLastIndexOfWhereAllBitsSetSingleTest()
        {
            Test(BitConverter.Int64BitsToDouble(-1), 2);

            [MethodImpl(MethodImplOptions.NoInlining)]
            void Test(double allBitsSet, double value2)
            {
                var input1 = Vector4D.Create(allBitsSet);
                var input2 = Vector4D.Create(value2);

                Assert.Equal(ElementCount, Vector4D.CountWhereAllBitsSet(input1));
                Assert.Equal(0, Vector4D.CountWhereAllBitsSet(input2));
                Assert.Equal(ElementCount - 1, Vector4D.CountWhereAllBitsSet(input1.WithElement(0, value2)));
                Assert.Equal(1, Vector4D.CountWhereAllBitsSet(input2.WithElement(0, allBitsSet)));

                Assert.Equal(0, Vector4D.IndexOfWhereAllBitsSet(input1));
                Assert.Equal(-1, Vector4D.IndexOfWhereAllBitsSet(input2));
                Assert.Equal(1, Vector4D.IndexOfWhereAllBitsSet(input1.WithElement(0, value2)));
                Assert.Equal(0, Vector4D.IndexOfWhereAllBitsSet(input2.WithElement(0, allBitsSet)));

                Assert.Equal(ElementCount - 1, Vector4D.LastIndexOfWhereAllBitsSet(input1));
                Assert.Equal(-1, Vector4D.LastIndexOfWhereAllBitsSet(input2));
                Assert.Equal(ElementCount - 1, Vector4D.LastIndexOfWhereAllBitsSet(input1.WithElement(0, value2)));
                Assert.Equal(0, Vector4D.LastIndexOfWhereAllBitsSet(input2.WithElement(0, allBitsSet)));
            }
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void IsEvenIntegerTest(double value) => Assert.Equal(double.IsEvenInteger(value) ? Vector4D.AllBitsSet : Vector4D.Zero, Vector4D.IsEvenInteger(Vector4D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void IsFiniteTest(double value) => Assert.Equal(double.IsFinite(value) ? Vector4D.AllBitsSet : Vector4D.Zero, Vector4D.IsFinite(Vector4D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void IsInfinityTest(double value) => Assert.Equal(double.IsInfinity(value) ? Vector4D.AllBitsSet : Vector4D.Zero, Vector4D.IsInfinity(Vector4D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void IsIntegerTest(double value) => Assert.Equal(double.IsInteger(value) ? Vector4D.AllBitsSet : Vector4D.Zero, Vector4D.IsInteger(Vector4D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void IsNaNTest(double value) => Assert.Equal(double.IsNaN(value) ? Vector4D.AllBitsSet : Vector4D.Zero, Vector4D.IsNaN(Vector4D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void IsNegativeTest(double value) => Assert.Equal(double.IsNegative(value) ? Vector4D.AllBitsSet : Vector4D.Zero, Vector4D.IsNegative(Vector4D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void IsNegativeInfinityTest(double value) => Assert.Equal(double.IsNegativeInfinity(value) ? Vector4D.AllBitsSet : Vector4D.Zero, Vector4D.IsNegativeInfinity(Vector4D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void IsNormalTest(double value) => Assert.Equal(double.IsNormal(value) ? Vector4D.AllBitsSet : Vector4D.Zero, Vector4D.IsNormal(Vector4D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void IsOddIntegerTest(double value) => Assert.Equal(double.IsOddInteger(value) ? Vector4D.AllBitsSet : Vector4D.Zero, Vector4D.IsOddInteger(Vector4D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void IsPositiveTest(double value) => Assert.Equal(double.IsPositive(value) ? Vector4D.AllBitsSet : Vector4D.Zero, Vector4D.IsPositive(Vector4D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void IsPositiveInfinityTest(double value) => Assert.Equal(double.IsPositiveInfinity(value) ? Vector4D.AllBitsSet : Vector4D.Zero, Vector4D.IsPositiveInfinity(Vector4D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void IsSubnormalTest(double value) => Assert.Equal(double.IsSubnormal(value) ? Vector4D.AllBitsSet : Vector4D.Zero, Vector4D.IsSubnormal(Vector4D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestDouble), MemberType = typeof(GenericMathTestMemberData))]
        public void IsZeroSingleTest(double value) => Assert.Equal((value == 0) ? Vector4D.AllBitsSet : Vector4D.Zero, Vector4D.IsZero(Vector4D.Create(value)));

        [Fact]
        public void AllBitsSetTest()
        {
            Assert.Equal(-1, BitConverter.DoubleToInt64Bits(Vector4D.AllBitsSet.X));
            Assert.Equal(-1, BitConverter.DoubleToInt64Bits(Vector4D.AllBitsSet.Y));
            Assert.Equal(-1, BitConverter.DoubleToInt64Bits(Vector4D.AllBitsSet.Z));
            Assert.Equal(-1, BitConverter.DoubleToInt64Bits(Vector4D.AllBitsSet.W));
        }

        [Fact]
        public void ConditionalSelectTest()
        {
            Test(Vector4D.Create(1, 2, 3, 4), Vector4D.AllBitsSet, Vector4D.Create(1, 2, 3, 4), Vector4D.Create(5, 6, 7, 8));
            Test(Vector4D.Create(5, 6, 7, 8), Vector4D.Zero, Vector4D.Create(1, 2, 3, 4), Vector4D.Create(5, 6, 7, 8));
            Test(Vector4D.Create(1, 6, 3, 8), Vector256.Create(-1, 0, -1, 0).AsDouble().AsVector4D(), Vector4D.Create(1, 2, 3, 4), Vector4D.Create(5, 6, 7, 8));

            [MethodImpl(MethodImplOptions.NoInlining)]
            void Test(Vector4D expectedResult, Vector4D condition, Vector4D left, Vector4D right)
            {
                Assert.Equal(expectedResult, Vector4D.ConditionalSelect(condition, left, right));
            }
        }

        [Theory]
        [InlineData(+0.0d, +0.0d, +0.0d, +0.0d, 0b0000)]
        [InlineData(-0.0d, +1.0d, -0.0d, +0.0d, 0b0101)]
        [InlineData(-0.0d, -0.0d, -0.0d, -0.0d, 0b1111)]
        public void ExtractMostSignificantBitsTest(double x, double y, double z, double w, uint expectedResult)
        {
            Assert.Equal(expectedResult, Vector4D.Create(x, y, z, w).ExtractMostSignificantBits());
        }

        [Theory]
        [InlineData(1.0d, 2.0d, 3.0d, 4.0d)]
        [InlineData(5.0d, 6.0d, 7.0d, 8.0d)]
        public void GetElementTest(double x, double y, double z, double w)
        {
            Assert.Equal(x, Vector4D.Create(x, y, z, w).GetElement(0));
            Assert.Equal(y, Vector4D.Create(x, y, z, w).GetElement(1));
            Assert.Equal(z, Vector4D.Create(x, y, z, w).GetElement(2));
            Assert.Equal(w, Vector4D.Create(x, y, z, w).GetElement(3));
        }

        [Theory]
        [InlineData(1.0d, 2.0d, 3.0d, 4.0d)]
        [InlineData(5.0d, 6.0d, 7.0d, 8.0d)]
        public void ShuffleTest(double x, double y, double z, double w)
        {
            Assert.Equal(Vector4D.Create(w, z, y, x), Vector4D.Shuffle(Vector4D.Create(x, y, z, w), 3, 2, 1, 0));
            Assert.Equal(Vector4D.Create(y, x, w, z), Vector4D.Shuffle(Vector4D.Create(x, y, z, w), 1, 0, 3, 2));
        }

        [Theory]
        [InlineData(1.0d, 2.0d, 3.0d, 4.0d, 10.0d)]
        [InlineData(5.0d, 6.0d, 7.0d, 8.0d, 26.0d)]
        public void SumTest(double x, double y, double z, double w, double expectedResult)
        {
            Assert.Equal(expectedResult, Vector4D.Sum(Vector4D.Create(x, y, z, w)));
        }

        [Theory]
        [InlineData(1.0d, 2.0d, 3.0d, 4.0d)]
        [InlineData(5.0d, 6.0d, 7.0d, 8.0d)]
        public void ToScalarTest(double x, double y, double z, double w)
        {
            Assert.Equal(x, Vector4D.Create(x, y, z, w).ToScalar());
        }

        [Theory]
        [InlineData(1.0d, 2.0d, 3.0d, 4.0d)]
        [InlineData(5.0d, 6.0d, 7.0d, 8.0d)]
        public void WithElementTest(double x, double y, double z, double w)
        {
            var vector = Vector4D.Create(10);

            Assert.Equal(10, vector.X);
            Assert.Equal(10, vector.Y);
            Assert.Equal(10, vector.Z);
            Assert.Equal(10, vector.W);

            vector = vector.WithElement(0, x);

            Assert.Equal(x, vector.X);
            Assert.Equal(10, vector.Y);
            Assert.Equal(10, vector.Z);
            Assert.Equal(10, vector.W);

            vector = vector.WithElement(1, y);

            Assert.Equal(x, vector.X);
            Assert.Equal(y, vector.Y);
            Assert.Equal(10, vector.Z);
            Assert.Equal(10, vector.W);

            vector = vector.WithElement(2, z);

            Assert.Equal(x, vector.X);
            Assert.Equal(y, vector.Y);
            Assert.Equal(z, vector.Z);
            Assert.Equal(10, vector.W);

            vector = vector.WithElement(3, w);

            Assert.Equal(x, vector.X);
            Assert.Equal(y, vector.Y);
            Assert.Equal(z, vector.Z);
            Assert.Equal(w, vector.W);
        }

        [Fact]
        public void CreateScalarTest()
        {
            var vector = Vector4D.CreateScalar(double.Pi);

            Assert.Equal(double.Pi, vector.X);
            Assert.Equal(0, vector.Y);
            Assert.Equal(0, vector.Z);
            Assert.Equal(0, vector.W);

            vector = Vector4D.CreateScalar(double.E);

            Assert.Equal(double.E, vector.X);
            Assert.Equal(0, vector.Y);
            Assert.Equal(0, vector.Z);
            Assert.Equal(0, vector.W);
        }

        [Fact]
        public void CreateScalarUnsafeTest()
        {
            var vector = Vector4D.CreateScalarUnsafe(double.Pi);
            Assert.Equal(double.Pi, vector.X);

            vector = Vector4D.CreateScalarUnsafe(double.E);
            Assert.Equal(double.E, vector.X);
        }
    }
}
