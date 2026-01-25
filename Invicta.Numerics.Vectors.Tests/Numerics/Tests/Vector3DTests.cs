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
        [InlineData(0.0f, 1.0f, 0.0f)]
        [InlineData(1.0f, 0.0f, 1.0f)]
        [InlineData(3.1434343f, 1.1234123f, 0.1234123f)]
        [InlineData(1.0000001f, 0.0000001f, 2.0000001f)]
        public void Vector3DIndexerGetTest(float x, float y, float z)
        {
            var vector = new Vector3D(x, y, z);

            Assert.Equal(x, vector[0]);
            Assert.Equal(y, vector[1]);
            Assert.Equal(z, vector[2]);
        }

        [Theory]
        [InlineData(0.0f, 1.0f, 0.0f)]
        [InlineData(1.0f, 0.0f, 1.0f)]
        [InlineData(3.1434343f, 1.1234123f, 0.1234123f)]
        [InlineData(1.0000001f, 0.0000001f, 2.0000001f)]
        public void Vector3DIndexerSetTest(float x, float y, float z)
        {
            var vector = new Vector3D(0.0f, 0.0f, 0.0f);

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
            Vector3D v1 = new Vector3D(2.0f, 3.0f, 3.3f);

            float[] a = new float[4];
            float[] b = new float[3];

            Assert.Throws<NullReferenceException>(() => v1.CopyTo(null, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => v1.CopyTo(a, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => v1.CopyTo(a, a.Length));
            Assert.Throws<ArgumentException>(() => v1.CopyTo(a, a.Length - 2));

            v1.CopyTo(a, 1);
            v1.CopyTo(b);
            Assert.Equal(0.0f, a[0]);
            Assert.Equal(2.0f, a[1]);
            Assert.Equal(3.0f, a[2]);
            Assert.Equal(3.3f, a[3]);
            Assert.Equal(2.0f, b[0]);
            Assert.Equal(3.0f, b[1]);
            Assert.Equal(3.3f, b[2]);
        }

        [Fact]
        public void Vector3DCopyToSpanTest()
        {
            Vector3D vector = new Vector3D(1.0f, 2.0f, 3.0f);
            Span<float> destination = new float[3];

            Assert.Throws<ArgumentException>(() => vector.CopyTo(new Span<float>(new float[2])));
            vector.CopyTo(destination);

            Assert.Equal(1.0f, vector.X);
            Assert.Equal(2.0f, vector.Y);
            Assert.Equal(3.0f, vector.Z);
            Assert.Equal(vector.X, destination[0]);
            Assert.Equal(vector.Y, destination[1]);
            Assert.Equal(vector.Z, destination[2]);
        }

        [Fact]
        public void Vector3DTryCopyToTest()
        {
            Vector3D vector = new Vector3D(1.0f, 2.0f, 3.0f);
            Span<float> destination = new float[3];

            Assert.False(vector.TryCopyTo(new Span<float>(new float[2])));
            Assert.True(vector.TryCopyTo(destination));

            Assert.Equal(1.0f, vector.X);
            Assert.Equal(2.0f, vector.Y);
            Assert.Equal(3.0f, vector.Z);
            Assert.Equal(vector.X, destination[0]);
            Assert.Equal(vector.Y, destination[1]);
            Assert.Equal(vector.Z, destination[2]);
        }

        [Fact]
        public void Vector3DGetHashCodeTest()
        {
            Vector3D v1 = new Vector3D(2.0f, 3.0f, 3.3f);
            Vector3D v2 = new Vector3D(2.0f, 3.0f, 3.3f);
            Vector3D v3 = new Vector3D(2.0f, 3.0f, 3.3f);
            Vector3D v5 = new Vector3D(3.0f, 2.0f, 3.3f);
            Assert.Equal(v1.GetHashCode(), v1.GetHashCode());
            Assert.Equal(v1.GetHashCode(), v2.GetHashCode());
            Assert.NotEqual(v1.GetHashCode(), v5.GetHashCode());
            Assert.Equal(v1.GetHashCode(), v3.GetHashCode());
            Vector3D v4 = new Vector3D(0.0f, 0.0f, 0.0f);
            Vector3D v6 = new Vector3D(1.0f, 0.0f, 0.0f);
            Vector3D v7 = new Vector3D(0.0f, 1.0f, 0.0f);
            Vector3D v8 = new Vector3D(1.0f, 1.0f, 1.0f);
            Vector3D v9 = new Vector3D(1.0f, 1.0f, 0.0f);
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

            Vector3D v1 = new Vector3D(2.0f, 3.0f, 3.3f);
            string v1str = v1.ToString();
            string expectedv1 = string.Format(CultureInfo.CurrentCulture
                , "<{1:G}{0} {2:G}{0} {3:G}>"
                , separator, 2, 3, 3.3);
            Assert.Equal(expectedv1, v1str);

            string v1strformatted = v1.ToString("c", CultureInfo.CurrentCulture);
            string expectedv1formatted = string.Format(CultureInfo.CurrentCulture
                , "<{1:c}{0} {2:c}{0} {3:c}>"
                , separator, 2, 3, 3.3);
            Assert.Equal(expectedv1formatted, v1strformatted);

            string v2strformatted = v1.ToString("c", enUsCultureInfo);
            string expectedv2formatted = string.Format(enUsCultureInfo
                , "<{1:c}{0} {2:c}{0} {3:c}>"
                , enUsCultureInfo.NumberFormat.NumberGroupSeparator, 2, 3, 3.3);
            Assert.Equal(expectedv2formatted, v2strformatted);

            string v3strformatted = v1.ToString("c");
            string expectedv3formatted = string.Format(CultureInfo.CurrentCulture
                , "<{1:c}{0} {2:c}{0} {3:c}>"
                , separator, 2, 3, 3.3);
            Assert.Equal(expectedv3formatted, v3strformatted);
        }

        // A test for Cross (Vector3Df, Vector3Df)
        [Fact]
        public void Vector3DCrossTest()
        {
            Vector3D a = new Vector3D(1.0f, 0.0f, 0.0f);
            Vector3D b = new Vector3D(0.0f, 1.0f, 0.0f);

            Vector3D expected = new Vector3D(0.0f, 0.0f, 1.0f);
            Vector3D actual;

            actual = Vector3D.Cross(a, b);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Cross did not return the expected value.");
        }

        // A test for Cross (Vector3Df, Vector3Df)
        // Cross test of the same vector
        [Fact]
        public void Vector3DCrossTest1()
        {
            Vector3D a = new Vector3D(0.0f, 1.0f, 0.0f);
            Vector3D b = new Vector3D(0.0f, 1.0f, 0.0f);

            Vector3D expected = new Vector3D(0.0f, 0.0f, 0.0f);
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
            Vector3D a = new Vector3D(1.0f, 2.0f, 3.0f);
            Vector3D b = new Vector3D(4.0f, 5.0f, 6.0f);

            float expected = (float)System.Math.Sqrt(27);
            float actual;

            actual = Vector3D.Distance(a, b);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Distance did not return the expected value.");
        }

        // A test for Distance (Vector3Df, Vector3Df)
        // Distance from the same point
        [Fact]
        public void Vector3DDistanceTest1()
        {
            Vector3D a = new Vector3D(1.051f, 2.05f, 3.478f);
            Vector3D b = new Vector3D(new Vector2D(1.051f, 0.0f), 1);
            b.Y = 2.05f;
            b.Z = 3.478f;

            float actual = Vector3D.Distance(a, b);
            Assert.Equal(0.0f, actual);
        }

        // A test for DistanceSquared (Vector3Df, Vector3Df)
        [Fact]
        public void Vector3DDistanceSquaredTest()
        {
            Vector3D a = new Vector3D(1.0f, 2.0f, 3.0f);
            Vector3D b = new Vector3D(4.0f, 5.0f, 6.0f);

            float expected = 27.0f;
            float actual;

            actual = Vector3D.DistanceSquared(a, b);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.DistanceSquared did not return the expected value.");
        }

        // A test for Dot (Vector3Df, Vector3Df)
        [Fact]
        public void Vector3DDotTest()
        {
            Vector3D a = new Vector3D(1.0f, 2.0f, 3.0f);
            Vector3D b = new Vector3D(4.0f, 5.0f, 6.0f);

            float expected = 32.0f;
            float actual;

            actual = Vector3D.Dot(a, b);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Dot did not return the expected value.");
        }

        // A test for Dot (Vector3Df, Vector3Df)
        // Dot test for perpendicular vector
        [Fact]
        public void Vector3DDotTest1()
        {
            Vector3D a = new Vector3D(1.55f, 1.55f, 1);
            Vector3D b = new Vector3D(2.5f, 3, 1.5f);
            Vector3D c = Vector3D.Cross(a, b);

            float expected = 0.0f;
            float actual1 = Vector3D.Dot(a, c);
            float actual2 = Vector3D.Dot(b, c);
            Assert.True(MathHelper.Equal(expected, actual1), "Vector3Df.Dot did not return the expected value.");
            Assert.True(MathHelper.Equal(expected, actual2), "Vector3Df.Dot did not return the expected value.");
        }

        // A test for Length ()
        [Fact]
        public void Vector3DLengthTest()
        {
            Vector2D a = new Vector2D(1.0f, 2.0f);

            float z = 3.0f;

            Vector3D target = new Vector3D(a, z);

            float expected = (float)System.Math.Sqrt(14.0f);
            float actual;

            actual = target.Length();
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Length did not return the expected value.");
        }

        // A test for Length ()
        // Length test where length is zero
        [Fact]
        public void Vector3DLengthTest1()
        {
            Vector3D target = new Vector3D();

            float expected = 0.0f;
            float actual = target.Length();
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Length did not return the expected value.");
        }

        // A test for LengthSquared ()
        [Fact]
        public void Vector3DLengthSquaredTest()
        {
            Vector2D a = new Vector2D(1.0f, 2.0f);

            float z = 3.0f;

            Vector3D target = new Vector3D(a, z);

            float expected = 14.0f;
            float actual;

            actual = target.LengthSquared();
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.LengthSquared did not return the expected value.");
        }

        // A test for Min (Vector3Df, Vector3Df)
        [Fact]
        public void Vector3DMinTest()
        {
            Vector3D a = new Vector3D(-1.0f, 4.0f, -3.0f);
            Vector3D b = new Vector3D(2.0f, 1.0f, -1.0f);

            Vector3D expected = new Vector3D(-1.0f, 1.0f, -3.0f);
            Vector3D actual;
            actual = Vector3D.Min(a, b);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Min did not return the expected value.");
        }

        // A test for Max (Vector3Df, Vector3Df)
        [Fact]
        public void Vector3DMaxTest()
        {
            Vector3D a = new Vector3D(-1.0f, 4.0f, -3.0f);
            Vector3D b = new Vector3D(2.0f, 1.0f, -1.0f);

            Vector3D expected = new Vector3D(2.0f, 4.0f, -1.0f);
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

        // A test for Lerp (Vector3Df, Vector3Df, float)
        [Fact]
        public void Vector3DLerpTest()
        {
            Vector3D a = new Vector3D(1.0f, 2.0f, 3.0f);
            Vector3D b = new Vector3D(4.0f, 5.0f, 6.0f);

            float t = 0.5f;

            Vector3D expected = new Vector3D(2.5f, 3.5f, 4.5f);
            Vector3D actual;

            actual = Vector3D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector3Df, Vector3Df, float)
        // Lerp test with factor zero
        [Fact]
        public void Vector3DLerpTest1()
        {
            Vector3D a = new Vector3D(1.0f, 2.0f, 3.0f);
            Vector3D b = new Vector3D(4.0f, 5.0f, 6.0f);

            float t = 0.0f;
            Vector3D expected = new Vector3D(1.0f, 2.0f, 3.0f);
            Vector3D actual = Vector3D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector3Df, Vector3Df, float)
        // Lerp test with factor one
        [Fact]
        public void Vector3DLerpTest2()
        {
            Vector3D a = new Vector3D(1.0f, 2.0f, 3.0f);
            Vector3D b = new Vector3D(4.0f, 5.0f, 6.0f);

            float t = 1.0f;
            Vector3D expected = new Vector3D(4.0f, 5.0f, 6.0f);
            Vector3D actual = Vector3D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector3Df, Vector3Df, float)
        // Lerp test with factor > 1
        [Fact]
        public void Vector3DLerpTest3()
        {
            Vector3D a = new Vector3D(0.0f, 0.0f, 0.0f);
            Vector3D b = new Vector3D(4.0f, 5.0f, 6.0f);

            float t = 2.0f;
            Vector3D expected = new Vector3D(8.0f, 10.0f, 12.0f);
            Vector3D actual = Vector3D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector3Df, Vector3Df, float)
        // Lerp test with factor < 0
        [Fact]
        public void Vector3DLerpTest4()
        {
            Vector3D a = new Vector3D(0.0f, 0.0f, 0.0f);
            Vector3D b = new Vector3D(4.0f, 5.0f, 6.0f);

            float t = -2.0f;
            Vector3D expected = new Vector3D(-8.0f, -10.0f, -12.0f);
            Vector3D actual = Vector3D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector3Df, Vector3Df, float)
        // Lerp test with special float value
        [Fact]
        public void Vector3DLerpTest5()
        {
            Vector3D a = new Vector3D(45.67f, 90.0f, 0f);
            Vector3D b = new Vector3D(float.PositiveInfinity, float.NegativeInfinity, 0);

            float t = 0.408f;
            Vector3D actual = Vector3D.Lerp(a, b, t);
            Assert.True(float.IsPositiveInfinity(actual.X), "Vector3Df.Lerp did not return the expected value.");
            Assert.True(float.IsNegativeInfinity(actual.Y), "Vector3Df.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector3Df, Vector3Df, float)
        // Lerp test from the same point
        [Fact]
        public void Vector3DLerpTest6()
        {
            Vector3D a = new Vector3D(1.68f, 2.34f, 5.43f);
            Vector3D b = a;

            float t = 0.18f;
            Vector3D expected = new Vector3D(1.68f, 2.34f, 5.43f);
            Vector3D actual = Vector3D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector3Df, Vector3Df, float)
        // Lerp test with values known to be inaccurate with the old lerp impl
        [Fact]
        public void Vector3DLerpTest7()
        {
            Vector3D a = new Vector3D(0.44728136f);
            Vector3D b = new Vector3D(0.46345946f);

            float t = 0.26402435f;

            Vector3D expected = new Vector3D(0.45155275f);
            Vector3D actual = Vector3D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector3Df, Vector3Df, float)
        // Lerp test with values known to be inaccurate with the old lerp impl
        // (Old code incorrectly gets 0.33333588)
        [Fact]
        public void Vector3DLerpTest8()
        {
            Vector3D a = new Vector3D(-100);
            Vector3D b = new Vector3D(0.33333334f);

            float t = 1f;

            Vector3D expected = new Vector3D(0.33333334f);
            Vector3D actual = Vector3D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Lerp did not return the expected value.");
        }

        // A test for Reflect (Vector3Df, Vector3Df)
        [Fact]
        public void Vector3DReflectTest()
        {
            Vector3D a = Vector3D.Normalize(new Vector3D(1.0f, 1.0f, 1.0f));

            // Reflect on XZ plane.
            Vector3D n = new Vector3D(0.0f, 1.0f, 0.0f);
            Vector3D expected = new Vector3D(a.X, -a.Y, a.Z);
            Vector3D actual = Vector3D.Reflect(a, n);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Reflect did not return the expected value.");

            // Reflect on XY plane.
            n = new Vector3D(0.0f, 0.0f, 1.0f);
            expected = new Vector3D(a.X, a.Y, -a.Z);
            actual = Vector3D.Reflect(a, n);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Reflect did not return the expected value.");

            // Reflect on YZ plane.
            n = new Vector3D(1.0f, 0.0f, 0.0f);
            expected = new Vector3D(-a.X, a.Y, a.Z);
            actual = Vector3D.Reflect(a, n);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Reflect did not return the expected value.");
        }

        // A test for Reflect (Vector3Df, Vector3Df)
        // Reflection when normal and source are the same
        [Fact]
        public void Vector3DReflectTest1()
        {
            Vector3D n = new Vector3D(0.45f, 1.28f, 0.86f);
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
            Vector3D n = new Vector3D(0.45f, 1.28f, 0.86f);
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
            Vector3D n = new Vector3D(0.45f, 1.28f, 0.86f);
            Vector3D temp = new Vector3D(1.28f, 0.45f, 0.01f);
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
            Vector3D v = new Vector3D(1.0f, 2.0f, 3.0f);
            Matrix4x4D m =
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0f)) *
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0f)) *
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0f));
            m.M41 = 10.0f;
            m.M42 = 20.0f;
            m.M43 = 30.0f;

            Vector3D expected = new Vector3D(12.191987f, 21.533493f, 32.616024f);
            Vector3D actual;

            actual = Vector3D.Transform(v, m);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Transform did not return the expected value.");
        }

        // A test for Clamp (Vector3Df, Vector3Df, Vector3Df)
        [Fact]
        public void Vector3DClampTest()
        {
            Vector3D a = new Vector3D(0.5f, 0.3f, 0.33f);
            Vector3D min = new Vector3D(0.0f, 0.1f, 0.13f);
            Vector3D max = new Vector3D(1.0f, 1.1f, 1.13f);

            // Normal case.
            // Case N1: specified value is in the range.
            Vector3D expected = new Vector3D(0.5f, 0.3f, 0.33f);
            Vector3D actual = Vector3D.Clamp(a, min, max);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Clamp did not return the expected value.");

            // Normal case.
            // Case N2: specified value is bigger than max value.
            a = new Vector3D(2.0f, 3.0f, 4.0f);
            expected = max;
            actual = Vector3D.Clamp(a, min, max);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Clamp did not return the expected value.");

            // Case N3: specified value is smaller than max value.
            a = new Vector3D(-2.0f, -3.0f, -4.0f);
            expected = min;
            actual = Vector3D.Clamp(a, min, max);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Clamp did not return the expected value.");

            // Case N4: combination case.
            a = new Vector3D(-2.0f, 0.5f, 4.0f);
            expected = new Vector3D(min.X, a.Y, max.Z);
            actual = Vector3D.Clamp(a, min, max);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Clamp did not return the expected value.");

            // User specified min value is bigger than max value.
            max = new Vector3D(0.0f, 0.1f, 0.13f);
            min = new Vector3D(1.0f, 1.1f, 1.13f);

            // Case W1: specified value is in the range.
            a = new Vector3D(0.5f, 0.3f, 0.33f);
            expected = max;
            actual = Vector3D.Clamp(a, min, max);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Clamp did not return the expected value.");

            // Normal case.
            // Case W2: specified value is bigger than max and min value.
            a = new Vector3D(2.0f, 3.0f, 4.0f);
            expected = max;
            actual = Vector3D.Clamp(a, min, max);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Clamp did not return the expected value.");

            // Case W3: specified value is smaller than min and max value.
            a = new Vector3D(-2.0f, -3.0f, -4.0f);
            expected = max;
            actual = Vector3D.Clamp(a, min, max);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Clamp did not return the expected value.");
        }

        // A test for TransformNormal (Vector3Df, Matrix4x4D)
        [Fact]
        public void Vector3DTransformNormalTest()
        {
            Vector3D v = new Vector3D(1.0f, 2.0f, 3.0f);
            Matrix4x4D m =
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0f)) *
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0f)) *
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0f));
            m.M41 = 10.0f;
            m.M42 = 20.0f;
            m.M43 = 30.0f;

            Vector3D expected = new Vector3D(2.19198728f, 1.53349364f, 2.61602545f);
            Vector3D actual;

            actual = Vector3D.TransformNormal(v, m);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.TransformNormal did not return the expected value.");
        }

        // A test for Transform (Vector3Df, QuaternionD)
        [Fact]
        public void Vector3DTransformByQuaternionDTest()
        {
            Vector3D v = new Vector3D(1.0f, 2.0f, 3.0f);

            Matrix4x4D m =
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0f)) *
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0f)) *
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0f));
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
            Vector3D v = new Vector3D(1.0f, 2.0f, 3.0f);
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
            Vector3D v = new Vector3D(1.0f, 2.0f, 3.0f);
            QuaternionD q = QuaternionD.Identity;
            Vector3D expected = v;

            Vector3D actual = Vector3D.Transform(v, q);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Transform did not return the expected value.");
        }

        // A test for Normalize (Vector3Df)
        [Fact]
        public void Vector3DNormalizeTest()
        {
            Vector3D a = new Vector3D(1.0f, 2.0f, 3.0f);

            Vector3D expected = new Vector3D(
                0.26726124191242438468455348087975f,
                0.53452248382484876936910696175951f,
                0.80178372573727315405366044263926f);
            Vector3D actual;

            actual = Vector3D.Normalize(a);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Normalize did not return the expected value.");
        }

        // A test for Normalize (Vector3Df)
        // Normalize vector of length one
        [Fact]
        public void Vector3DNormalizeTest1()
        {
            Vector3D a = new Vector3D(1.0f, 0.0f, 0.0f);

            Vector3D expected = new Vector3D(1.0f, 0.0f, 0.0f);
            Vector3D actual = Vector3D.Normalize(a);
            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.Normalize did not return the expected value.");
        }

        // A test for Normalize (Vector3Df)
        // Normalize vector of length zero
        [Fact]
        public void Vector3DNormalizeTest2()
        {
            Vector3D a = new Vector3D(0.0f, 0.0f, 0.0f);

            Vector3D expected = new Vector3D(0.0f, 0.0f, 0.0f);
            Vector3D actual = Vector3D.Normalize(a);
            Assert.True(float.IsNaN(actual.X) && float.IsNaN(actual.Y) && float.IsNaN(actual.Z), "Vector3Df.Normalize did not return the expected value.");
        }

        // A test for operator - (Vector3Df)
        [Fact]
        public void Vector3DUnaryNegationTest()
        {
            Vector3D a = new Vector3D(1.0f, 2.0f, 3.0f);

            Vector3D expected = new Vector3D(-1.0f, -2.0f, -3.0f);
            Vector3D actual;

            actual = -a;

            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.operator - did not return the expected value.");
        }

        [Fact]
        public void Vector3DUnaryNegationTest1()
        {
            Vector3D a = -new Vector3D(float.NaN, float.PositiveInfinity, float.NegativeInfinity);
            Vector3D b = -new Vector3D(0.0f, 0.0f, 0.0f);
            Assert.Equal(float.NaN, a.X);
            Assert.Equal(float.NegativeInfinity, a.Y);
            Assert.Equal(float.PositiveInfinity, a.Z);
            Assert.Equal(0.0f, b.X);
            Assert.Equal(0.0f, b.Y);
            Assert.Equal(0.0f, b.Z);
        }

        // A test for operator - (Vector3Df, Vector3Df)
        [Fact]
        public void Vector3DSubtractionTest()
        {
            Vector3D a = new Vector3D(4.0f, 2.0f, 3.0f);

            Vector3D b = new Vector3D(1.0f, 5.0f, 7.0f);

            Vector3D expected = new Vector3D(3.0f, -3.0f, -4.0f);
            Vector3D actual;

            actual = a - b;

            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.operator - did not return the expected value.");
        }

        // A test for operator * (Vector3Df, float)
        [Fact]
        public void Vector3DMultiplyOperatorTest()
        {
            Vector3D a = new Vector3D(1.0f, 2.0f, 3.0f);

            float factor = 2.0f;

            Vector3D expected = new Vector3D(2.0f, 4.0f, 6.0f);
            Vector3D actual;

            actual = a * factor;

            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.operator * did not return the expected value.");
        }

        // A test for operator * (float, Vector3Df)
        [Fact]
        public void Vector3DMultiplyOperatorTest2()
        {
            Vector3D a = new Vector3D(1.0f, 2.0f, 3.0f);

            const float factor = 2.0f;

            Vector3D expected = new Vector3D(2.0f, 4.0f, 6.0f);
            Vector3D actual;

            actual = factor * a;

            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.operator * did not return the expected value.");
        }

        // A test for operator * (Vector3Df, Vector3Df)
        [Fact]
        public void Vector3DMultiplyOperatorTest3()
        {
            Vector3D a = new Vector3D(1.0f, 2.0f, 3.0f);

            Vector3D b = new Vector3D(4.0f, 5.0f, 6.0f);

            Vector3D expected = new Vector3D(4.0f, 10.0f, 18.0f);
            Vector3D actual;

            actual = a * b;

            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.operator * did not return the expected value.");
        }

        // A test for operator / (Vector3Df, float)
        [Fact]
        public void Vector3DDivisionTest()
        {
            Vector3D a = new Vector3D(1.0f, 2.0f, 3.0f);

            float div = 2.0f;

            Vector3D expected = new Vector3D(0.5f, 1.0f, 1.5f);
            Vector3D actual;

            actual = a / div;

            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.operator / did not return the expected value.");
        }

        // A test for operator / (Vector3Df, Vector3Df)
        [Fact]
        public void Vector3DDivisionTest1()
        {
            Vector3D a = new Vector3D(4.0f, 2.0f, 3.0f);

            Vector3D b = new Vector3D(1.0f, 5.0f, 6.0f);

            Vector3D expected = new Vector3D(4.0f, 0.4f, 0.5f);
            Vector3D actual;

            actual = a / b;

            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.operator / did not return the expected value.");
        }

        // A test for operator / (Vector3Df, Vector3Df)
        // Divide by zero
        [Fact]
        public void Vector3DDivisionTest2()
        {
            Vector3D a = new Vector3D(-2.0f, 3.0f, float.MaxValue);

            float div = 0.0f;

            Vector3D actual = a / div;

            Assert.True(float.IsNegativeInfinity(actual.X), "Vector3Df.operator / did not return the expected value.");
            Assert.True(float.IsPositiveInfinity(actual.Y), "Vector3Df.operator / did not return the expected value.");
            Assert.True(float.IsPositiveInfinity(actual.Z), "Vector3Df.operator / did not return the expected value.");
        }

        // A test for operator / (Vector3Df, Vector3Df)
        // Divide by zero
        [Fact]
        public void Vector3DDivisionTest3()
        {
            Vector3D a = new Vector3D(0.047f, -3.0f, float.NegativeInfinity);
            Vector3D b = new Vector3D();

            Vector3D actual = a / b;

            Assert.True(float.IsPositiveInfinity(actual.X), "Vector3Df.operator / did not return the expected value.");
            Assert.True(float.IsNegativeInfinity(actual.Y), "Vector3Df.operator / did not return the expected value.");
            Assert.True(float.IsNegativeInfinity(actual.Z), "Vector3Df.operator / did not return the expected value.");
        }

        // A test for operator + (Vector3Df, Vector3Df)
        [Fact]
        public void Vector3DAdditionTest()
        {
            Vector3D a = new Vector3D(1.0f, 2.0f, 3.0f);
            Vector3D b = new Vector3D(4.0f, 5.0f, 6.0f);

            Vector3D expected = new Vector3D(5.0f, 7.0f, 9.0f);
            Vector3D actual;

            actual = a + b;

            Assert.True(MathHelper.Equal(expected, actual), "Vector3Df.operator + did not return the expected value.");
        }

        // A test for Vector3Df (float, float, float)
        [Fact]
        public void Vector3DConstructorTest()
        {
            float x = 1.0f;
            float y = 2.0f;
            float z = 3.0f;

            Vector3D target = new Vector3D(x, y, z);
            Assert.True(MathHelper.Equal(target.X, x) && MathHelper.Equal(target.Y, y) && MathHelper.Equal(target.Z, z), "Vector3Df.constructor (x,y,z) did not return the expected value.");
        }

        // A test for Vector3Df (Vector2Df, float)
        [Fact]
        public void Vector3DConstructorTest1()
        {
            Vector2D a = new Vector2D(1.0f, 2.0f);

            float z = 3.0f;

            Vector3D target = new Vector3D(a, z);
            Assert.True(MathHelper.Equal(target.X, a.X) && MathHelper.Equal(target.Y, a.Y) && MathHelper.Equal(target.Z, z), "Vector3Df.constructor (Vector2Df,z) did not return the expected value.");
        }

        // A test for Vector3Df ()
        // Constructor with no parameter
        [Fact]
        public void Vector3DConstructorTest3()
        {
            Vector3D a = new Vector3D();

            Assert.Equal(0.0f, a.X);
            Assert.Equal(0.0f, a.Y);
            Assert.Equal(0.0f, a.Z);
        }

        // A test for Vector2Df (float, float)
        // Constructor with special floating values
        [Fact]
        public void Vector3DConstructorTest4()
        {
            Vector3D target = new Vector3D(float.NaN, float.MaxValue, float.PositiveInfinity);

            Assert.True(float.IsNaN(target.X), "Vector3Df.constructor (Vector3Df) did not return the expected value.");
            Assert.True(float.Equals(float.MaxValue, target.Y), "Vector3Df.constructor (Vector3Df) did not return the expected value.");
            Assert.True(float.IsPositiveInfinity(target.Z), "Vector3Df.constructor (Vector3Df) did not return the expected value.");
        }

        // A test for Vector3Df (ReadOnlySpan<float>)
        [Fact]
        public void Vector3DConstructorTest6()
        {
            float value = 1.0f;
            Vector3D target = new Vector3D(new[] { value, value, value });
            Vector3D expected = new Vector3D(value);

            Assert.Equal(expected, target);
            Assert.Throws<ArgumentOutOfRangeException>(() => new Vector3D(new float[2]));
        }

        // A test for Add (Vector3Df, Vector3Df)
        [Fact]
        public void Vector3DAddTest()
        {
            Vector3D a = new Vector3D(1.0f, 2.0f, 3.0f);
            Vector3D b = new Vector3D(5.0f, 6.0f, 7.0f);

            Vector3D expected = new Vector3D(6.0f, 8.0f, 10.0f);
            Vector3D actual;

            actual = Vector3D.Add(a, b);
            Assert.Equal(expected, actual);
        }

        // A test for Divide (Vector3Df, float)
        [Fact]
        public void Vector3DDivideTest()
        {
            Vector3D a = new Vector3D(1.0f, 2.0f, 3.0f);
            float div = 2.0f;
            Vector3D expected = new Vector3D(0.5f, 1.0f, 1.5f);
            Vector3D actual;
            actual = Vector3D.Divide(a, div);
            Assert.Equal(expected, actual);
        }

        // A test for Divide (Vector3Df, Vector3Df)
        [Fact]
        public void Vector3DDivideTest1()
        {
            Vector3D a = new Vector3D(1.0f, 6.0f, 7.0f);
            Vector3D b = new Vector3D(5.0f, 2.0f, 3.0f);

            Vector3D expected = new Vector3D(1.0f / 5.0f, 6.0f / 2.0f, 7.0f / 3.0f);
            Vector3D actual;

            actual = Vector3D.Divide(a, b);
            Assert.Equal(expected, actual);
        }

        // A test for Equals (object)
        [Fact]
        public void Vector3DEqualsTest()
        {
            Vector3D a = new Vector3D(1.0f, 2.0f, 3.0f);
            Vector3D b = new Vector3D(1.0f, 2.0f, 3.0f);

            // case 1: compare between same values
            object obj = b;

            bool expected = true;
            bool actual = a.Equals(obj);
            Assert.Equal(expected, actual);

            // case 2: compare between different values
            b.X = 10.0f;
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

        // A test for Multiply (Vector3Df, float)
        [Fact]
        public void Vector3DMultiplyTest()
        {
            Vector3D a = new Vector3D(1.0f, 2.0f, 3.0f);
            const float factor = 2.0f;
            Vector3D expected = new Vector3D(2.0f, 4.0f, 6.0f);
            Vector3D actual = Vector3D.Multiply(a, factor);
            Assert.Equal(expected, actual);
        }

        // A test for Multiply (float, Vector3Df)
        [Fact]
        public static void Vector3DMultiplyTest2()
        {
            Vector3D a = new Vector3D(1.0f, 2.0f, 3.0f);
            const float factor = 2.0f;
            Vector3D expected = new Vector3D(2.0f, 4.0f, 6.0f);
            Vector3D actual = Vector3D.Multiply(factor, a);
            Assert.Equal(expected, actual);
        }

        // A test for Multiply (Vector3Df, Vector3Df)
        [Fact]
        public void Vector3DMultiplyTest3()
        {
            Vector3D a = new Vector3D(1.0f, 2.0f, 3.0f);
            Vector3D b = new Vector3D(5.0f, 6.0f, 7.0f);

            Vector3D expected = new Vector3D(5.0f, 12.0f, 21.0f);
            Vector3D actual;

            actual = Vector3D.Multiply(a, b);
            Assert.Equal(expected, actual);
        }

        // A test for Negate (Vector3Df)
        [Fact]
        public void Vector3DNegateTest()
        {
            Vector3D a = new Vector3D(1.0f, 2.0f, 3.0f);

            Vector3D expected = new Vector3D(-1.0f, -2.0f, -3.0f);
            Vector3D actual;

            actual = Vector3D.Negate(a);
            Assert.Equal(expected, actual);
        }

        // A test for operator != (Vector3Df, Vector3Df)
        [Fact]
        public void Vector3DInequalityTest()
        {
            Vector3D a = new Vector3D(1.0f, 2.0f, 3.0f);
            Vector3D b = new Vector3D(1.0f, 2.0f, 3.0f);

            // case 1: compare between same values
            bool expected = false;
            bool actual = a != b;
            Assert.Equal(expected, actual);

            // case 2: compare between different values
            b.X = 10.0f;
            expected = true;
            actual = a != b;
            Assert.Equal(expected, actual);
        }

        // A test for operator == (Vector3Df, Vector3Df)
        [Fact]
        public void Vector3DEqualityTest()
        {
            Vector3D a = new Vector3D(1.0f, 2.0f, 3.0f);
            Vector3D b = new Vector3D(1.0f, 2.0f, 3.0f);

            // case 1: compare between same values
            bool expected = true;
            bool actual = a == b;
            Assert.Equal(expected, actual);

            // case 2: compare between different values
            b.X = 10.0f;
            expected = false;
            actual = a == b;
            Assert.Equal(expected, actual);
        }

        // A test for Subtract (Vector3Df, Vector3Df)
        [Fact]
        public void Vector3DSubtractTest()
        {
            Vector3D a = new Vector3D(1.0f, 6.0f, 3.0f);
            Vector3D b = new Vector3D(5.0f, 2.0f, 3.0f);

            Vector3D expected = new Vector3D(-4.0f, 4.0f, 0.0f);
            Vector3D actual;

            actual = Vector3D.Subtract(a, b);
            Assert.Equal(expected, actual);
        }

        // A test for One
        [Fact]
        public void Vector3DOneTest()
        {
            Vector3D val = new Vector3D(1.0f, 1.0f, 1.0f);
            Assert.Equal(val, Vector3D.One);
        }

        // A test for UnitX
        [Fact]
        public void Vector3DUnitXTest()
        {
            Vector3D val = new Vector3D(1.0f, 0.0f, 0.0f);
            Assert.Equal(val, Vector3D.UnitX);
        }

        // A test for UnitY
        [Fact]
        public void Vector3DUnitYTest()
        {
            Vector3D val = new Vector3D(0.0f, 1.0f, 0.0f);
            Assert.Equal(val, Vector3D.UnitY);
        }

        // A test for UnitZ
        [Fact]
        public void Vector3DUnitZTest()
        {
            Vector3D val = new Vector3D(0.0f, 0.0f, 1.0f);
            Assert.Equal(val, Vector3D.UnitZ);
        }

        // A test for Zero
        [Fact]
        public void Vector3DZeroTest()
        {
            Vector3D val = new Vector3D(0.0f, 0.0f, 0.0f);
            Assert.Equal(val, Vector3D.Zero);
        }

        // A test for Equals (Vector3Df)
        [Fact]
        public void Vector3DEqualsTest1()
        {
            Vector3D a = new Vector3D(1.0f, 2.0f, 3.0f);
            Vector3D b = new Vector3D(1.0f, 2.0f, 3.0f);

            // case 1: compare between same values
            bool expected = true;
            bool actual = a.Equals(b);
            Assert.Equal(expected, actual);

            // case 2: compare between different values
            b.X = 10.0f;
            expected = false;
            actual = a.Equals(b);
            Assert.Equal(expected, actual);
        }

        // A test for Vector3Df (float)
        [Fact]
        public void Vector3DConstructorTest5()
        {
            float value = 1.0f;
            Vector3D target = new Vector3D(value);

            Vector3D expected = new Vector3D(value, value, value);
            Assert.Equal(expected, target);

            value = 2.0f;
            target = new Vector3D(value);
            expected = new Vector3D(value, value, value);
            Assert.Equal(expected, target);
        }

        // A test for Vector3Df comparison involving NaN values
        [Fact]
        public void Vector3DEqualsNaNTest()
        {
            Vector3D a = new Vector3D(float.NaN, 0, 0);
            Vector3D b = new Vector3D(0, float.NaN, 0);
            Vector3D c = new Vector3D(0, 0, float.NaN);

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
            Vector3D v1 = new Vector3D(-2.5f, 2.0f, 0.5f);
            Vector3D v3 = Vector3D.Abs(new Vector3D(0.0f, float.NegativeInfinity, float.NaN));
            Vector3D v = Vector3D.Abs(v1);
            Assert.Equal(2.5f, v.X);
            Assert.Equal(2.0f, v.Y);
            Assert.Equal(0.5f, v.Z);
            Assert.Equal(0.0f, v3.X);
            Assert.Equal(float.PositiveInfinity, v3.Y);
            Assert.Equal(float.NaN, v3.Z);
        }

        [Fact]
        public void Vector3DSqrtTest()
        {
            Vector3D a = new Vector3D(-2.5f, 2.0f, 0.5f);
            Vector3D b = new Vector3D(5.5f, 4.5f, 16.5f);
            Assert.Equal(2, (int)Vector3D.SquareRoot(b).X);
            Assert.Equal(2, (int)Vector3D.SquareRoot(b).Y);
            Assert.Equal(4, (int)Vector3D.SquareRoot(b).Z);
            Assert.Equal(float.NaN, Vector3D.SquareRoot(a).X);
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
            private float _f;
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
            Vector3D v3 = new Vector3D(4f, 5f, 6f);
            v3.X = 1.0f;
            v3.Y = 2.0f;
            v3.Z = 3.0f;
            Assert.Equal(1.0f, v3.X);
            Assert.Equal(2.0f, v3.Y);
            Assert.Equal(3.0f, v3.Z);
            Vector3D v4 = v3;
            v4.Y = 0.5f;
            v4.Z = 2.2f;
            Assert.Equal(1.0f, v4.X);
            Assert.Equal(0.5f, v4.Y);
            Assert.Equal(2.2f, v4.Z);
            Assert.Equal(2.0f, v3.Y);

            Vector3D before = new Vector3D(1f, 2f, 3f);
            Vector3D after = before;
            after.X = 500.0f;
            Assert.NotEqual(before, after);
        }

        [Fact]
        public void EmbeddedVectorSetFields()
        {
            EmbeddedVectorObject evo = new EmbeddedVectorObject();
            evo.FieldVector.X = 5.0f;
            evo.FieldVector.Y = 5.0f;
            evo.FieldVector.Z = 5.0f;
            Assert.Equal(5.0f, evo.FieldVector.X);
            Assert.Equal(5.0f, evo.FieldVector.Y);
            Assert.Equal(5.0f, evo.FieldVector.Z);
        }

        private class EmbeddedVectorObject
        {
            public Vector3D FieldVector;
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.CosSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void CosSingleTest(float value, float expectedResult, float variance)
        {
            Vector3D actualResult = Vector3D.Cos(Vector3D.Create(value));
            AssertEqual(Vector3D.Create(expectedResult), actualResult, Vector3D.Create(variance));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.ExpSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void ExpSingleTest(float value, float expectedResult, float variance)
        {
            Vector3D actualResult = Vector3D.Exp(Vector3D.Create(value));
            AssertEqual(Vector3D.Create(expectedResult), actualResult, Vector3D.Create(variance));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.LogSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void LogSingleTest(float value, float expectedResult, float variance)
        {
            Vector3D actualResult = Vector3D.Log(Vector3D.Create(value));
            AssertEqual(Vector3D.Create(expectedResult), actualResult, Vector3D.Create(variance));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.Log2Single), MemberType = typeof(GenericMathTestMemberData))]
        public void Log2SingleTest(float value, float expectedResult, float variance)
        {
            Vector3D actualResult = Vector3D.Log2(Vector3D.Create(value));
            AssertEqual(Vector3D.Create(expectedResult), actualResult, Vector3D.Create(variance));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.FusedMultiplyAddSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void FusedMultiplyAddSingleTest(float left, float right, float addend, float expectedResult)
        {
            AssertEqual(Vector3D.Create(expectedResult), Vector3D.FusedMultiplyAdd(Vector3D.Create(left), Vector3D.Create(right), Vector3D.Create(addend)), Vector3D.Zero);
            AssertEqual(Vector3D.Create(float.MultiplyAddEstimate(left, right, addend)), Vector3D.MultiplyAddEstimate(Vector3D.Create(left), Vector3D.Create(right), Vector3D.Create(addend)), Vector3D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.ClampSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void ClampSingleTest(float x, float min, float max, float expectedResult)
        {
            Vector3D actualResult = Vector3D.Clamp(Vector3D.Create(x), Vector3D.Create(min), Vector3D.Create(max));
            AssertEqual(Vector3D.Create(expectedResult), actualResult, Vector3D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.CopySignSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void CopySignSingleTest(float x, float y, float expectedResult)
        {
            Vector3D actualResult = Vector3D.CopySign(Vector3D.Create(x), Vector3D.Create(y));
            AssertEqual(Vector3D.Create(expectedResult), actualResult, Vector3D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.DegreesToRadiansSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void DegreesToRadiansSingleTest(float value, float expectedResult, float variance)
        {
            AssertEqual(Vector3D.Create(-expectedResult), Vector3D.DegreesToRadians(Vector3D.Create(-value)), Vector3D.Create(variance));
            AssertEqual(Vector3D.Create(+expectedResult), Vector3D.DegreesToRadians(Vector3D.Create(+value)), Vector3D.Create(variance));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.HypotSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void HypotSingleTest(float x, float y, float expectedResult, float variance)
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
        public void LerpSingleTest(float x, float y, float amount, float expectedResult)
        {
            AssertEqual(Vector3D.Create(+expectedResult), Vector3D.Lerp(Vector3D.Create(+x), Vector3D.Create(+y), Vector3D.Create(amount)), Vector3D.Zero);
            AssertEqual(Vector3D.Create((expectedResult == 0.0f) ? expectedResult : -expectedResult), Vector3D.Lerp(Vector3D.Create(-x), Vector3D.Create(-y), Vector3D.Create(amount)), Vector3D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MaxSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void MaxSingleTest(float x, float y, float expectedResult)
        {
            Vector3D actualResult = Vector3D.Max(Vector3D.Create(x), Vector3D.Create(y));
            AssertEqual(Vector3D.Create(expectedResult), actualResult, Vector3D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MaxMagnitudeSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void MaxMagnitudeSingleTest(float x, float y, float expectedResult)
        {
            Vector3D actualResult = Vector3D.MaxMagnitude(Vector3D.Create(x), Vector3D.Create(y));
            AssertEqual(Vector3D.Create(expectedResult), actualResult, Vector3D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MaxMagnitudeNumberSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void MaxMagnitudeNumberSingleTest(float x, float y, float expectedResult)
        {
            Vector3D actualResult = Vector3D.MaxMagnitudeNumber(Vector3D.Create(x), Vector3D.Create(y));
            AssertEqual(Vector3D.Create(expectedResult), actualResult, Vector3D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MaxNumberSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void MaxNumberSingleTest(float x, float y, float expectedResult)
        {
            Vector3D actualResult = Vector3D.MaxNumber(Vector3D.Create(x), Vector3D.Create(y));
            AssertEqual(Vector3D.Create(expectedResult), actualResult, Vector3D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MinSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void MinSingleTest(float x, float y, float expectedResult)
        {
            Vector3D actualResult = Vector3D.Min(Vector3D.Create(x), Vector3D.Create(y));
            AssertEqual(Vector3D.Create(expectedResult), actualResult, Vector3D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MinMagnitudeSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void MinMagnitudeSingleTest(float x, float y, float expectedResult)
        {
            Vector3D actualResult = Vector3D.MinMagnitude(Vector3D.Create(x), Vector3D.Create(y));
            AssertEqual(Vector3D.Create(expectedResult), actualResult, Vector3D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MinMagnitudeNumberSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void MinMagnitudeNumberSingleTest(float x, float y, float expectedResult)
        {
            Vector3D actualResult = Vector3D.MinMagnitudeNumber(Vector3D.Create(x), Vector3D.Create(y));
            AssertEqual(Vector3D.Create(expectedResult), actualResult, Vector3D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MinNumberSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void MinNumberSingleTest(float x, float y, float expectedResult)
        {
            Vector3D actualResult = Vector3D.MinNumber(Vector3D.Create(x), Vector3D.Create(y));
            AssertEqual(Vector3D.Create(expectedResult), actualResult, Vector3D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.RadiansToDegreesSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void RadiansToDegreesSingleTest(float value, float expectedResult, float variance)
        {
            AssertEqual(Vector3D.Create(-expectedResult), Vector3D.RadiansToDegrees(Vector3D.Create(-value)), Vector3D.Create(variance));
            AssertEqual(Vector3D.Create(+expectedResult), Vector3D.RadiansToDegrees(Vector3D.Create(+value)), Vector3D.Create(variance));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.RoundSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void RoundSingleTest(float value, float expectedResult)
        {
            Vector3D actualResult = Vector3D.Round(Vector3D.Create(value));
            AssertEqual(Vector3D.Create(expectedResult), actualResult, Vector3D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.RoundAwayFromZeroSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void RoundAwayFromZeroSingleTest(float value, float expectedResult)
        {
            Vector3D actualResult = Vector3D.Round(Vector3D.Create(value), MidpointRounding.AwayFromZero);
            AssertEqual(Vector3D.Create(expectedResult), actualResult, Vector3D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.RoundToEvenSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void RoundToEvenSingleTest(float value, float expectedResult)
        {
            Vector3D actualResult = Vector3D.Round(Vector3D.Create(value), MidpointRounding.ToEven);
            AssertEqual(Vector3D.Create(expectedResult), actualResult, Vector3D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.SinSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void SinSingleTest(float value, float expectedResult, float variance)
        {
            Vector3D actualResult = Vector3D.Sin(Vector3D.Create(value));
            AssertEqual(Vector3D.Create(expectedResult), actualResult, Vector3D.Create(variance));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.SinCosSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void SinCosSingleTest(float value, float expectedResultSin, float expectedResultCos, float allowedVarianceSin, float allowedVarianceCos)
        {
            (Vector3D resultSin, Vector3D resultCos) = Vector3D.SinCos(Vector3D.Create(value));
            AssertEqual(Vector3D.Create(expectedResultSin), resultSin, Vector3D.Create(allowedVarianceSin));
            AssertEqual(Vector3D.Create(expectedResultCos), resultCos, Vector3D.Create(allowedVarianceCos));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.TruncateSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void TruncateSingleTest(float value, float expectedResult)
        {
            Vector3D actualResult = Vector3D.Truncate(Vector3D.Create(value));
            AssertEqual(Vector3D.Create(expectedResult), actualResult, Vector3D.Zero);
        }

        [Fact]
        public void AllAnyNoneTest()
        {
            Test(3, 2);

            [MethodImpl(MethodImplOptions.NoInlining)]
            void Test(float value1, float value2)
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
            void Test(float value)
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
            void Test(float allBitsSet, float value2)
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
            void Test(float value1, float value2)
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
            void Test(float value)
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
            void Test(float allBitsSet, float value2)
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
        public void IsEvenIntegerTest(float value) => Assert.Equal(float.IsEvenInteger(value) ? Vector3D.AllBitsSet : Vector3D.Zero, Vector3D.IsEvenInteger(Vector3D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsFiniteTest(float value) => Assert.Equal(float.IsFinite(value) ? Vector3D.AllBitsSet : Vector3D.Zero, Vector3D.IsFinite(Vector3D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsInfinityTest(float value) => Assert.Equal(float.IsInfinity(value) ? Vector3D.AllBitsSet : Vector3D.Zero, Vector3D.IsInfinity(Vector3D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsIntegerTest(float value) => Assert.Equal(float.IsInteger(value) ? Vector3D.AllBitsSet : Vector3D.Zero, Vector3D.IsInteger(Vector3D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsNaNTest(float value) => Assert.Equal(float.IsNaN(value) ? Vector3D.AllBitsSet : Vector3D.Zero, Vector3D.IsNaN(Vector3D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsNegativeTest(float value) => Assert.Equal(float.IsNegative(value) ? Vector3D.AllBitsSet : Vector3D.Zero, Vector3D.IsNegative(Vector3D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsNegativeInfinityTest(float value) => Assert.Equal(float.IsNegativeInfinity(value) ? Vector3D.AllBitsSet : Vector3D.Zero, Vector3D.IsNegativeInfinity(Vector3D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsNormalTest(float value) => Assert.Equal(float.IsNormal(value) ? Vector3D.AllBitsSet : Vector3D.Zero, Vector3D.IsNormal(Vector3D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsOddIntegerTest(float value) => Assert.Equal(float.IsOddInteger(value) ? Vector3D.AllBitsSet : Vector3D.Zero, Vector3D.IsOddInteger(Vector3D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsPositiveTest(float value) => Assert.Equal(float.IsPositive(value) ? Vector3D.AllBitsSet : Vector3D.Zero, Vector3D.IsPositive(Vector3D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsPositiveInfinityTest(float value) => Assert.Equal(float.IsPositiveInfinity(value) ? Vector3D.AllBitsSet : Vector3D.Zero, Vector3D.IsPositiveInfinity(Vector3D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsSubnormalTest(float value) => Assert.Equal(float.IsSubnormal(value) ? Vector3D.AllBitsSet : Vector3D.Zero, Vector3D.IsSubnormal(Vector3D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsZeroSingleTest(float value) => Assert.Equal((value == 0) ? Vector3D.AllBitsSet : Vector3D.Zero, Vector3D.IsZero(Vector3D.Create(value)));

        [Fact]
        public void AllBitsSetTest()
        {
            Assert.Equal(-1, BitConverter.SingleToInt32Bits(Vector3D.AllBitsSet.X));
            Assert.Equal(-1, BitConverter.SingleToInt32Bits(Vector3D.AllBitsSet.Y));
            Assert.Equal(-1, BitConverter.SingleToInt32Bits(Vector3D.AllBitsSet.Z));
        }

        [Fact]
        public void ConditionalSelectTest()
        {
            Test(Vector3D.Create(1, 2, 3), Vector3D.AllBitsSet, Vector3D.Create(1, 2, 3), Vector3D.Create(5, 6, 7));
            Test(Vector3D.Create(5, 6, 7), Vector3D.Zero, Vector3D.Create(1, 2, 3), Vector3D.Create(5, 6, 7));
            Test(Vector3D.Create(1, 6, 3), Vector128.Create(-1, 0, -1, 0).AsSingle().AsVector3D(), Vector3D.Create(1, 2, 3), Vector3D.Create(5, 6, 7));

            [MethodImpl(MethodImplOptions.NoInlining)]
            void Test(Vector3D expectedResult, Vector3D condition, Vector3D left, Vector3D right)
            {
                Assert.Equal(expectedResult, Vector3D.ConditionalSelect(condition, left, right));
            }
        }

        [Theory]
        [InlineData(+0.0f, +0.0f, +0.0f, 0b000)]
        [InlineData(-0.0f, +1.0f, -0.0f, 0b101)]
        [InlineData(-0.0f, -0.0f, -0.0f, 0b111)]
        public void ExtractMostSignificantBitsTest(float x, float y, float z, uint expectedResult)
        {
            Assert.Equal(expectedResult, Vector3D.Create(x, y, z).ExtractMostSignificantBits());
        }

        [Theory]
        [InlineData(1.0f, 2.0f, 3.0f)]
        [InlineData(5.0f, 6.0f, 7.0f)]
        public void GetElementTest(float x, float y, float z)
        {
            Assert.Equal(x, Vector3D.Create(x, y, z).GetElement(0));
            Assert.Equal(y, Vector3D.Create(x, y, z).GetElement(1));
            Assert.Equal(z, Vector3D.Create(x, y, z).GetElement(2));
        }

        [Theory]
        [InlineData(1.0f, 2.0f, 3.0f)]
        [InlineData(5.0f, 6.0f, 7.0f)]
        public void ShuffleTest(float x, float y, float z)
        {
            Assert.Equal(Vector3D.Create(z, y, x), Vector3D.Shuffle(Vector3D.Create(x, y, z), 2, 1, 0));
            Assert.Equal(Vector3D.Create(y, x, z), Vector3D.Shuffle(Vector3D.Create(x, y, z), 1, 0, 2));
        }

        [Theory]
        [InlineData(1.0f, 2.0f, 3.0f, 6.0f)]
        [InlineData(5.0f, 6.0f, 7.0f, 18.0f)]
        public void SumTest(float x, float y, float z, float expectedResult)
        {
            Assert.Equal(expectedResult, Vector3D.Sum(Vector3D.Create(x, y, z)));
        }

        [Theory]
        [InlineData(1.0f, 2.0f, 3.0f)]
        [InlineData(5.0f, 6.0f, 7.0f)]
        public void ToScalarTest(float x, float y, float z)
        {
            Assert.Equal(x, Vector3D.Create(x, y, z).ToScalar());
        }

        [Theory]
        [InlineData(1.0f, 2.0f, 3.0f)]
        [InlineData(5.0f, 6.0f, 7.0f)]
        public void WithElementTest(float x, float y, float z)
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
        [InlineData(1.0f, 2.0f, 3.0f)]
        [InlineData(5.0f, 6.0f, 7.0f)]
        public void AsVector2DTest(float x, float y, float z)
        {
            var vector = Vector3D.Create(x, y, z).AsVector2D();

            Assert.Equal(x, vector.X);
            Assert.Equal(y, vector.Y);
        }

        [Fact]
        public void CreateScalarTest()
        {
            var vector = Vector3D.CreateScalar(float.Pi);

            Assert.Equal(float.Pi, vector.X);
            Assert.Equal(0, vector.Y);
            Assert.Equal(0, vector.Z);

            vector = Vector3D.CreateScalar(float.E);

            Assert.Equal(float.E, vector.X);
            Assert.Equal(0, vector.Y);
            Assert.Equal(0, vector.Z);
        }

        [Fact]
        public void CreateScalarUnsafeTest()
        {
            var vector = Vector3D.CreateScalarUnsafe(float.Pi);
            Assert.Equal(float.Pi, vector.X);

            vector = Vector3D.CreateScalarUnsafe(float.E);
            Assert.Equal(float.E, vector.X);
        }
    }
}
