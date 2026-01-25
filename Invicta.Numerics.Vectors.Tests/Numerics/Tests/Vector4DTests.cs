// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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
            Assert.Equal(16, Marshal.SizeOf<Vector4D>());
            Assert.Equal(16, Marshal.SizeOf<Vector4D>(new Vector4D()));
        }

        [Theory]
        [InlineData(0.0f, 1.0f, 0.0f, 1.0f)]
        [InlineData(1.0f, 0.0f, 1.0f, 0.0f)]
        [InlineData(3.1434343f, 1.1234123f, 0.1234123f, -0.1234123f)]
        [InlineData(1.0000001f, 0.0000001f, 2.0000001f, 0.0000002f)]
        public void Vector4DIndexerGetTest(float x, float y, float z, float w)
        {
            var vector = new Vector4D(x, y, z, w);

            Assert.Equal(x, vector[0]);
            Assert.Equal(y, vector[1]);
            Assert.Equal(z, vector[2]);
            Assert.Equal(w, vector[3]);
        }

        [Theory]
        [InlineData(0.0f, 1.0f, 0.0f, 1.0f)]
        [InlineData(1.0f, 0.0f, 1.0f, 0.0f)]
        [InlineData(3.1434343f, 1.1234123f, 0.1234123f, -0.1234123f)]
        [InlineData(1.0000001f, 0.0000001f, 2.0000001f, 0.0000002f)]
        public void Vector4DIndexerSetTest(float x, float y, float z, float w)
        {
            var vector = new Vector4D(0.0f, 0.0f, 0.0f, 0.0f);

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
            Vector4D v1 = new Vector4D(2.5f, 2.0f, 3.0f, 3.3f);

            float[] a = new float[5];
            float[] b = new float[4];

            Assert.Throws<NullReferenceException>(() => v1.CopyTo(null, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => v1.CopyTo(a, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => v1.CopyTo(a, a.Length));
            Assert.Throws<ArgumentException>(() => v1.CopyTo(a, a.Length - 2));

            v1.CopyTo(a, 1);
            v1.CopyTo(b);
            Assert.Equal(0.0f, a[0]);
            Assert.Equal(2.5f, a[1]);
            Assert.Equal(2.0f, a[2]);
            Assert.Equal(3.0f, a[3]);
            Assert.Equal(3.3f, a[4]);
            Assert.Equal(2.5f, b[0]);
            Assert.Equal(2.0f, b[1]);
            Assert.Equal(3.0f, b[2]);
            Assert.Equal(3.3f, b[3]);
        }

        [Fact]
        public void Vector4DCopyToSpanTest()
        {
            Vector4D vector = new Vector4D(1.0f, 2.0f, 3.0f, 4.0f);
            Span<float> destination = new float[4];

            Assert.Throws<ArgumentException>(() => vector.CopyTo(new Span<float>(new float[3])));
            vector.CopyTo(destination);

            Assert.Equal(1.0f, vector.X);
            Assert.Equal(2.0f, vector.Y);
            Assert.Equal(3.0f, vector.Z);
            Assert.Equal(4.0f, vector.W);
            Assert.Equal(vector.X, destination[0]);
            Assert.Equal(vector.Y, destination[1]);
            Assert.Equal(vector.Z, destination[2]);
            Assert.Equal(vector.W, destination[3]);
        }

        [Fact]
        public void Vector4DTryCopyToTest()
        {
            Vector4D vector = new Vector4D(1.0f, 2.0f, 3.0f, 4.0f);
            Span<float> destination = new float[4];

            Assert.False(vector.TryCopyTo(new Span<float>(new float[3])));
            Assert.True(vector.TryCopyTo(destination));

            Assert.Equal(1.0f, vector.X);
            Assert.Equal(2.0f, vector.Y);
            Assert.Equal(3.0f, vector.Z);
            Assert.Equal(4.0f, vector.W);
            Assert.Equal(vector.X, destination[0]);
            Assert.Equal(vector.Y, destination[1]);
            Assert.Equal(vector.Z, destination[2]);
            Assert.Equal(vector.W, destination[3]);
        }

        [Fact]
        public void Vector4DGetHashCodeTest()
        {
            Vector4D v1 = new Vector4D(2.5f, 2.0f, 3.0f, 3.3f);
            Vector4D v2 = new Vector4D(2.5f, 2.0f, 3.0f, 3.3f);
            Vector4D v3 = new Vector4D(2.5f, 2.0f, 3.0f, 3.3f);
            Vector4D v5 = new Vector4D(3.3f, 3.0f, 2.0f, 2.5f);
            Assert.Equal(v1.GetHashCode(), v1.GetHashCode());
            Assert.Equal(v1.GetHashCode(), v2.GetHashCode());
            Assert.NotEqual(v1.GetHashCode(), v5.GetHashCode());
            Assert.Equal(v1.GetHashCode(), v3.GetHashCode());
            Vector4D v4 = new Vector4D(0.0f, 0.0f, 0.0f, 0.0f);
            Vector4D v6 = new Vector4D(1.0f, 0.0f, 0.0f, 0.0f);
            Vector4D v7 = new Vector4D(0.0f, 1.0f, 0.0f, 0.0f);
            Vector4D v8 = new Vector4D(1.0f, 1.0f, 1.0f, 1.0f);
            Vector4D v9 = new Vector4D(1.0f, 1.0f, 0.0f, 0.0f);
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

            Vector4D v1 = new Vector4D(2.5f, 2.0f, 3.0f, 3.3f);

            string v1str = v1.ToString();
            string expectedv1 = string.Format(CultureInfo.CurrentCulture
                , "<{1:G}{0} {2:G}{0} {3:G}{0} {4:G}>"
                , separator, 2.5, 2, 3, 3.3);
            Assert.Equal(expectedv1, v1str);

            string v1strformatted = v1.ToString("c", CultureInfo.CurrentCulture);
            string expectedv1formatted = string.Format(CultureInfo.CurrentCulture
                , "<{1:c}{0} {2:c}{0} {3:c}{0} {4:c}>"
                , separator, 2.5, 2, 3, 3.3);
            Assert.Equal(expectedv1formatted, v1strformatted);

            string v2strformatted = v1.ToString("c", enUsCultureInfo);
            string expectedv2formatted = string.Format(enUsCultureInfo
                , "<{1:c}{0} {2:c}{0} {3:c}{0} {4:c}>"
                , enUsCultureInfo.NumberFormat.NumberGroupSeparator, 2.5, 2, 3, 3.3);
            Assert.Equal(expectedv2formatted, v2strformatted);

            string v3strformatted = v1.ToString("c");
            string expectedv3formatted = string.Format(CultureInfo.CurrentCulture
                , "<{1:c}{0} {2:c}{0} {3:c}{0} {4:c}>"
                , separator, 2.5, 2, 3, 3.3);
            Assert.Equal(expectedv3formatted, v3strformatted);
        }

        // A test for DistanceSquared (Vector4Df, Vector4Df)
        [Fact]
        public void Vector4DDistanceSquaredTest()
        {
            Vector4D a = new Vector4D(1.0f, 2.0f, 3.0f, 4.0f);
            Vector4D b = new Vector4D(5.0f, 6.0f, 7.0f, 8.0f);

            float expected = 64.0f;
            float actual;

            actual = Vector4D.DistanceSquared(a, b);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.DistanceSquared did not return the expected value.");
        }

        // A test for Distance (Vector4Df, Vector4Df)
        [Fact]
        public void Vector4DDistanceTest()
        {
            Vector4D a = new Vector4D(1.0f, 2.0f, 3.0f, 4.0f);
            Vector4D b = new Vector4D(5.0f, 6.0f, 7.0f, 8.0f);

            float expected = 8.0f;
            float actual;

            actual = Vector4D.Distance(a, b);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.Distance did not return the expected value.");
        }

        // A test for Distance (Vector4Df, Vector4Df)
        // Distance from the same point
        [Fact]
        public void Vector4DDistanceTest1()
        {
            Vector4D a = new Vector4D(new Vector2D(1.051f, 2.05f), 3.478f, 1.0f);
            Vector4D b = new Vector4D(new Vector3D(1.051f, 2.05f, 3.478f), 0.0f);
            b.W = 1.0f;

            float actual = Vector4D.Distance(a, b);
            Assert.Equal(0.0f, actual);
        }

        // A test for Dot (Vector4Df, Vector4Df)
        [Fact]
        public void Vector4DDotTest()
        {
            Vector4D a = new Vector4D(1.0f, 2.0f, 3.0f, 4.0f);
            Vector4D b = new Vector4D(5.0f, 6.0f, 7.0f, 8.0f);

            float expected = 70.0f;
            float actual;

            actual = Vector4D.Dot(a, b);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.Dot did not return the expected value.");
        }

        // A test for Dot (Vector4Df, Vector4Df)
        // Dot test for perpendicular vector
        [Fact]
        public void Vector4DDotTest1()
        {
            Vector3D a = new Vector3D(1.55f, 1.55f, 1);
            Vector3D b = new Vector3D(2.5f, 3, 1.5f);
            Vector3D c = Vector3D.Cross(a, b);

            Vector4D d = new Vector4D(a, 0);
            Vector4D e = new Vector4D(c, 0);

            float actual = Vector4D.Dot(d, e);
            Assert.True(MathHelper.Equal(0.0f, actual), "Vector4Df.Dot did not return the expected value.");
        }

        [Fact]
        public void Vector4DCrossTest()
        {
            Vector3D a3 = new Vector3D(1.0f, 0.0f, 0.0f);
            Vector3D b3 = new Vector3D(0.0f, 1.0f, 0.0f);
            Vector3D e3 = Vector3D.Cross(a3, b3);

            Vector4D a4 = new Vector4D(a3, 2.0f);
            Vector4D b4 = new Vector4D(b3, 3.0f);
            Vector4D e4 = new Vector4D(e3, a4.W * b4.W);

            Vector4D actual = Vector4D.Cross(a4, b4);
            Assert.True(MathHelper.Equal(e4, actual), "Vector4Df.Cross did not return the expected value.");
        }

        [Fact]
        public void Vector4DCrossTest1()
        {
            // Cross test of the same vector
            Vector3D a3 = new Vector3D(0.0f, 1.0f, 0.0f);
            Vector3D b3 = new Vector3D(0.0f, 1.0f, 0.0f);
            Vector3D e3 = Vector3D.Cross(a3, b3);

            Vector4D a4 = new Vector4D(a3, 3.0f);
            Vector4D b4 = new Vector4D(b3, 3.0f);
            Vector4D e4 = new Vector4D(e3, a4.W * b4.W);

            Vector4D actual = Vector4D.Cross(a4, b4);
            Assert.True(MathHelper.Equal(e4, actual), "Vector4Df.Cross did not return the expected value.");
        }

        // A test for Length ()
        [Fact]
        public void Vector4DLengthTest()
        {
            Vector3D a = new Vector3D(1.0f, 2.0f, 3.0f);
            float w = 4.0f;

            Vector4D target = new Vector4D(a, w);

            float expected = (float)System.Math.Sqrt(30.0f);
            float actual;

            actual = target.Length();

            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.Length did not return the expected value.");
        }

        // A test for Length ()
        // Length test where length is zero
        [Fact]
        public void Vector4DLengthTest1()
        {
            Vector4D target = new Vector4D();

            float expected = 0.0f;
            float actual = target.Length();

            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.Length did not return the expected value.");
        }

        // A test for LengthSquared ()
        [Fact]
        public void Vector4DLengthSquaredTest()
        {
            Vector3D a = new Vector3D(1.0f, 2.0f, 3.0f);
            float w = 4.0f;

            Vector4D target = new Vector4D(a, w);

            float expected = 30;
            float actual;

            actual = target.LengthSquared();

            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.LengthSquared did not return the expected value.");
        }

        // A test for Min (Vector4Df, Vector4Df)
        [Fact]
        public void Vector4DMinTest()
        {
            Vector4D a = new Vector4D(-1.0f, 4.0f, -3.0f, 1000.0f);
            Vector4D b = new Vector4D(2.0f, 1.0f, -1.0f, 0.0f);

            Vector4D expected = new Vector4D(-1.0f, 1.0f, -3.0f, 0.0f);
            Vector4D actual;
            actual = Vector4D.Min(a, b);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.Min did not return the expected value.");
        }

        // A test for Max (Vector4Df, Vector4Df)
        [Fact]
        public void Vector4DMaxTest()
        {
            Vector4D a = new Vector4D(-1.0f, 4.0f, -3.0f, 1000.0f);
            Vector4D b = new Vector4D(2.0f, 1.0f, -1.0f, 0.0f);

            Vector4D expected = new Vector4D(2.0f, 4.0f, -1.0f, 1000.0f);
            Vector4D actual;
            actual = Vector4D.Max(a, b);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.Max did not return the expected value.");
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

        // A test for Clamp (Vector4Df, Vector4Df, Vector4Df)
        [Fact]
        public void Vector4DClampTest()
        {
            Vector4D a = new Vector4D(0.5f, 0.3f, 0.33f, 0.44f);
            Vector4D min = new Vector4D(0.0f, 0.1f, 0.13f, 0.14f);
            Vector4D max = new Vector4D(1.0f, 1.1f, 1.13f, 1.14f);

            // Normal case.
            // Case N1: specified value is in the range.
            Vector4D expected = new Vector4D(0.5f, 0.3f, 0.33f, 0.44f);
            Vector4D actual = Vector4D.Clamp(a, min, max);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.Clamp did not return the expected value.");

            // Normal case.
            // Case N2: specified value is bigger than max value.
            a = new Vector4D(2.0f, 3.0f, 4.0f, 5.0f);
            expected = max;
            actual = Vector4D.Clamp(a, min, max);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.Clamp did not return the expected value.");

            // Case N3: specified value is smaller than max value.
            a = new Vector4D(-2.0f, -3.0f, -4.0f, -5.0f);
            expected = min;
            actual = Vector4D.Clamp(a, min, max);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.Clamp did not return the expected value.");

            // Case N4: combination case.
            a = new Vector4D(-2.0f, 0.5f, 4.0f, -5.0f);
            expected = new Vector4D(min.X, a.Y, max.Z, min.W);
            actual = Vector4D.Clamp(a, min, max);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.Clamp did not return the expected value.");

            // User specified min value is bigger than max value.
            max = new Vector4D(0.0f, 0.1f, 0.13f, 0.14f);
            min = new Vector4D(1.0f, 1.1f, 1.13f, 1.14f);

            // Case W1: specified value is in the range.
            a = new Vector4D(0.5f, 0.3f, 0.33f, 0.44f);
            expected = max;
            actual = Vector4D.Clamp(a, min, max);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.Clamp did not return the expected value.");

            // Normal case.
            // Case W2: specified value is bigger than max and min value.
            a = new Vector4D(2.0f, 3.0f, 4.0f, 5.0f);
            expected = max;
            actual = Vector4D.Clamp(a, min, max);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.Clamp did not return the expected value.");

            // Case W3: specified value is smaller than min and max value.
            a = new Vector4D(-2.0f, -3.0f, -4.0f, -5.0f);
            expected = max;
            actual = Vector4D.Clamp(a, min, max);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.Clamp did not return the expected value.");
        }

        // A test for Lerp (Vector4Df, Vector4Df, float)
        [Fact]
        public void Vector4DLerpTest()
        {
            Vector4D a = new Vector4D(1.0f, 2.0f, 3.0f, 4.0f);
            Vector4D b = new Vector4D(5.0f, 6.0f, 7.0f, 8.0f);

            float t = 0.5f;

            Vector4D expected = new Vector4D(3.0f, 4.0f, 5.0f, 6.0f);
            Vector4D actual;

            actual = Vector4D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector4Df, Vector4Df, float)
        // Lerp test with factor zero
        [Fact]
        public void Vector4DLerpTest1()
        {
            Vector4D a = new Vector4D(new Vector3D(1.0f, 2.0f, 3.0f), 4.0f);
            Vector4D b = new Vector4D(4.0f, 5.0f, 6.0f, 7.0f);

            float t = 0.0f;
            Vector4D expected = new Vector4D(1.0f, 2.0f, 3.0f, 4.0f);
            Vector4D actual = Vector4D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector4Df, Vector4Df, float)
        // Lerp test with factor one
        [Fact]
        public void Vector4DLerpTest2()
        {
            Vector4D a = new Vector4D(new Vector3D(1.0f, 2.0f, 3.0f), 4.0f);
            Vector4D b = new Vector4D(4.0f, 5.0f, 6.0f, 7.0f);

            float t = 1.0f;
            Vector4D expected = new Vector4D(4.0f, 5.0f, 6.0f, 7.0f);
            Vector4D actual = Vector4D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector4Df, Vector4Df, float)
        // Lerp test with factor > 1
        [Fact]
        public void Vector4DLerpTest3()
        {
            Vector4D a = new Vector4D(new Vector3D(0.0f, 0.0f, 0.0f), 0.0f);
            Vector4D b = new Vector4D(4.0f, 5.0f, 6.0f, 7.0f);

            float t = 2.0f;
            Vector4D expected = new Vector4D(8.0f, 10.0f, 12.0f, 14.0f);
            Vector4D actual = Vector4D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector4Df, Vector4Df, float)
        // Lerp test with factor < 0
        [Fact]
        public void Vector4DLerpTest4()
        {
            Vector4D a = new Vector4D(new Vector3D(0.0f, 0.0f, 0.0f), 0.0f);
            Vector4D b = new Vector4D(4.0f, 5.0f, 6.0f, 7.0f);

            float t = -2.0f;
            Vector4D expected = -(b * 2);
            Vector4D actual = Vector4D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector4Df, Vector4Df, float)
        // Lerp test with special float value
        [Fact]
        public void Vector4DLerpTest5()
        {
            Vector4D a = new Vector4D(45.67f, 90.0f, 0, 0);
            Vector4D b = new Vector4D(float.PositiveInfinity, float.NegativeInfinity, 0, 0);

            float t = 0.408f;
            Vector4D actual = Vector4D.Lerp(a, b, t);
            Assert.True(float.IsPositiveInfinity(actual.X), "Vector4Df.Lerp did not return the expected value.");
            Assert.True(float.IsNegativeInfinity(actual.Y), "Vector4Df.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector4Df, Vector4Df, float)
        // Lerp test from the same point
        [Fact]
        public void Vector4DLerpTest6()
        {
            Vector4D a = new Vector4D(4.0f, 5.0f, 6.0f, 7.0f);
            Vector4D b = new Vector4D(4.0f, 5.0f, 6.0f, 7.0f);

            float t = 0.85f;
            Vector4D expected = a;
            Vector4D actual = Vector4D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector4Df, Vector4Df, float)
        // Lerp test with values known to be inaccurate with the old lerp impl
        [Fact]
        public void Vector4DLerpTest7()
        {
            Vector4D a = new Vector4D(0.44728136f);
            Vector4D b = new Vector4D(0.46345946f);

            float t = 0.26402435f;

            Vector4D expected = new Vector4D(0.45155275f);
            Vector4D actual = Vector4D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector4Df, Vector4Df, float)
        // Lerp test with values known to be inaccurate with the old lerp impl
        // (Old code incorrectly gets 0.33333588)
        [Fact]
        public void Vector4DLerpTest8()
        {
            Vector4D a = new Vector4D(-100);
            Vector4D b = new Vector4D(0.33333334f);

            float t = 1f;

            Vector4D expected = new Vector4D(0.33333334f);
            Vector4D actual = Vector4D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.Lerp did not return the expected value.");
        }

        // A test for Transform (Vector2Df, Matrix4x4D)
        [Fact]
        public void Vector4DTransformTest1()
        {
            Vector2D v = new Vector2D(1.0f, 2.0f);

            Matrix4x4D m =
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0f)) *
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0f)) *
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0f));
            m.M41 = 10.0f;
            m.M42 = 20.0f;
            m.M43 = 30.0f;

            Vector4D expected = new Vector4D(10.316987f, 22.183012f, 30.3660259f, 1.0f);
            Vector4D actual;

            actual = Vector4D.Transform(v, m);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.Transform did not return the expected value.");
        }

        // A test for Transform (Vector3Df, Matrix4x4D)
        [Fact]
        public void Vector4DTransformTest2()
        {
            Vector3D v = new Vector3D(1.0f, 2.0f, 3.0f);

            Matrix4x4D m =
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0f)) *
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0f)) *
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0f));
            m.M41 = 10.0f;
            m.M42 = 20.0f;
            m.M43 = 30.0f;

            Vector4D expected = new Vector4D(12.19198728f, 21.53349376f, 32.61602545f, 1.0f);
            Vector4D actual;

            actual = Vector4D.Transform(v, m);
            Assert.True(MathHelper.Equal(expected, actual), "vector4.Transform did not return the expected value.");
        }

        // A test for Transform (Vector4Df, Matrix4x4D)
        [Fact]
        public void Vector4DTransformVector4DTest()
        {
            Vector4D v = new Vector4D(1.0f, 2.0f, 3.0f, 0.0f);

            Matrix4x4D m =
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0f)) *
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0f)) *
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0f));
            m.M41 = 10.0f;
            m.M42 = 20.0f;
            m.M43 = 30.0f;

            Vector4D expected = new Vector4D(2.19198728f, 1.53349376f, 2.61602545f, 0.0f);
            Vector4D actual;

            actual = Vector4D.Transform(v, m);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.Transform did not return the expected value.");

            //
            v.W = 1.0f;

            expected = new Vector4D(12.19198728f, 21.53349376f, 32.61602545f, 1.0f);
            actual = Vector4D.Transform(v, m);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.Transform did not return the expected value.");
        }

        // A test for Transform (Vector4Df, Matrix4x4D)
        // Transform vector4 with zero matrix
        [Fact]
        public void Vector4DTransformVector4DTest1()
        {
            Vector4D v = new Vector4D(1.0f, 2.0f, 3.0f, 0.0f);
            Matrix4x4D m = new Matrix4x4D();
            Vector4D expected = new Vector4D(0, 0, 0, 0);

            Vector4D actual = Vector4D.Transform(v, m);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.Transform did not return the expected value.");
        }

        // A test for Transform (Vector4Df, Matrix4x4D)
        // Transform vector4 with identity matrix
        [Fact]
        public void Vector4DTransformVector4DTest2()
        {
            Vector4D v = new Vector4D(1.0f, 2.0f, 3.0f, 0.0f);
            Matrix4x4D m = Matrix4x4D.Identity;
            Vector4D expected = new Vector4D(1.0f, 2.0f, 3.0f, 0.0f);

            Vector4D actual = Vector4D.Transform(v, m);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.Transform did not return the expected value.");
        }

        // A test for Transform (Vector3Df, Matrix4x4D)
        // Transform Vector3Df test
        [Fact]
        public void Vector4DTransformVector3DTest()
        {
            Vector3D v = new Vector3D(1.0f, 2.0f, 3.0f);

            Matrix4x4D m =
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0f)) *
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0f)) *
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0f));
            m.M41 = 10.0f;
            m.M42 = 20.0f;
            m.M43 = 30.0f;

            Vector4D expected = Vector4D.Transform(new Vector4D(v, 1.0f), m);
            Vector4D actual = Vector4D.Transform(v, m);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.Transform did not return the expected value.");
        }

        // A test for Transform (Vector3Df, Matrix4x4D)
        // Transform vector3 with zero matrix
        [Fact]
        public void Vector4DTransformVector3DTest1()
        {
            Vector3D v = new Vector3D(1.0f, 2.0f, 3.0f);
            Matrix4x4D m = new Matrix4x4D();
            Vector4D expected = new Vector4D(0, 0, 0, 0);

            Vector4D actual = Vector4D.Transform(v, m);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.Transform did not return the expected value.");
        }

        // A test for Transform (Vector3Df, Matrix4x4D)
        // Transform vector3 with identity matrix
        [Fact]
        public void Vector4DTransformVector3DTest2()
        {
            Vector3D v = new Vector3D(1.0f, 2.0f, 3.0f);
            Matrix4x4D m = Matrix4x4D.Identity;
            Vector4D expected = new Vector4D(1.0f, 2.0f, 3.0f, 1.0f);

            Vector4D actual = Vector4D.Transform(v, m);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.Transform did not return the expected value.");
        }

        // A test for Transform (Vector2Df, Matrix4x4D)
        // Transform Vector2Df test
        [Fact]
        public void Vector4DTransformVector2DTest()
        {
            Vector2D v = new Vector2D(1.0f, 2.0f);

            Matrix4x4D m =
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0f)) *
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0f)) *
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0f));
            m.M41 = 10.0f;
            m.M42 = 20.0f;
            m.M43 = 30.0f;

            Vector4D expected = Vector4D.Transform(new Vector4D(v, 0.0f, 1.0f), m);
            Vector4D actual = Vector4D.Transform(v, m);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.Transform did not return the expected value.");
        }

        // A test for Transform (Vector2Df, Matrix4x4D)
        // Transform Vector2Df with zero matrix
        [Fact]
        public void Vector4DTransformVector2DTest1()
        {
            Vector2D v = new Vector2D(1.0f, 2.0f);
            Matrix4x4D m = new Matrix4x4D();
            Vector4D expected = new Vector4D(0, 0, 0, 0);

            Vector4D actual = Vector4D.Transform(v, m);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.Transform did not return the expected value.");
        }

        // A test for Transform (Vector2Df, Matrix4x4D)
        // Transform vector2 with identity matrix
        [Fact]
        public void Vector4DTransformVector2DTest2()
        {
            Vector2D v = new Vector2D(1.0f, 2.0f);
            Matrix4x4D m = Matrix4x4D.Identity;
            Vector4D expected = new Vector4D(1.0f, 2.0f, 0, 1.0f);

            Vector4D actual = Vector4D.Transform(v, m);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.Transform did not return the expected value.");
        }

        // A test for Transform (Vector2Df, QuaternionD)
        [Fact]
        public void Vector4DTransformVector2DQuatanionTest()
        {
            Vector2D v = new Vector2D(1.0f, 2.0f);

            Matrix4x4D m =
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0f)) *
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0f)) *
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0f));

            QuaternionD q = QuaternionD.CreateFromRotationMatrix(m);

            Vector4D expected = Vector4D.Transform(v, m);
            Vector4D actual;

            actual = Vector4D.Transform(v, q);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.Transform did not return the expected value.");
        }

        // A test for Transform (Vector3Df, QuaternionD)
        [Fact]
        public void Vector4DTransformVector3DQuaternionD()
        {
            Vector3D v = new Vector3D(1.0f, 2.0f, 3.0f);

            Matrix4x4D m =
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0f)) *
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0f)) *
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0f));
            QuaternionD q = QuaternionD.CreateFromRotationMatrix(m);

            Vector4D expected = Vector4D.Transform(v, m);
            Vector4D actual;

            actual = Vector4D.Transform(v, q);
            Assert.True(MathHelper.Equal(expected, actual), "vector4.Transform did not return the expected value.");
        }

        // A test for Transform (Vector4Df, QuaternionD)
        [Fact]
        public void Vector4DTransformVector4DQuaternionDTest()
        {
            Vector4D v = new Vector4D(1.0f, 2.0f, 3.0f, 0.0f);

            Matrix4x4D m =
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0f)) *
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0f)) *
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0f));
            QuaternionD q = QuaternionD.CreateFromRotationMatrix(m);

            Vector4D expected = Vector4D.Transform(v, m);
            Vector4D actual;

            actual = Vector4D.Transform(v, q);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.Transform did not return the expected value.");

            //
            v.W = 1.0f;
            expected.W = 1.0f;
            actual = Vector4D.Transform(v, q);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.Transform did not return the expected value.");
        }

        // A test for Transform (Vector4Df, QuaternionD)
        // Transform vector4 with zero quaternion
        [Fact]
        public void Vector4DTransformVector4DQuaternionDTest1()
        {
            Vector4D v = new Vector4D(1.0f, 2.0f, 3.0f, 0.0f);
            QuaternionD q = new QuaternionD();
            Vector4D expected = Vector4D.Zero;

            Vector4D actual = Vector4D.Transform(v, q);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.Transform did not return the expected value.");
        }

        // A test for Transform (Vector4Df, QuaternionD)
        // Transform vector4 with identity matrix
        [Fact]
        public void Vector4DTransformVector4DQuaternionDTest2()
        {
            Vector4D v = new Vector4D(1.0f, 2.0f, 3.0f, 0.0f);
            QuaternionD q = QuaternionD.Identity;
            Vector4D expected = new Vector4D(1.0f, 2.0f, 3.0f, 0.0f);

            Vector4D actual = Vector4D.Transform(v, q);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.Transform did not return the expected value.");
        }

        // A test for Transform (Vector3Df, QuaternionD)
        // Transform Vector3Df test
        [Fact]
        public void Vector4DTransformVector3DQuaternionDTest()
        {
            Vector3D v = new Vector3D(1.0f, 2.0f, 3.0f);

            Matrix4x4D m =
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0f)) *
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0f)) *
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0f));
            QuaternionD q = QuaternionD.CreateFromRotationMatrix(m);

            Vector4D expected = Vector4D.Transform(v, m);
            Vector4D actual = Vector4D.Transform(v, q);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.Transform did not return the expected value.");
        }

        // A test for Transform (Vector3Df, QuaternionD)
        // Transform vector3 with zero quaternion
        [Fact]
        public void Vector4DTransformVector3DQuaternionDTest1()
        {
            Vector3D v = new Vector3D(1.0f, 2.0f, 3.0f);
            QuaternionD q = new QuaternionD();
            Vector4D expected = Vector4D.Zero;

            Vector4D actual = Vector4D.Transform(v, q);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.Transform did not return the expected value.");
        }

        // A test for Transform (Vector3Df, QuaternionD)
        // Transform vector3 with identity quaternion
        [Fact]
        public void Vector4DTransformVector3DQuaternionDTest2()
        {
            Vector3D v = new Vector3D(1.0f, 2.0f, 3.0f);
            QuaternionD q = QuaternionD.Identity;
            Vector4D expected = new Vector4D(1.0f, 2.0f, 3.0f, 1.0f);

            Vector4D actual = Vector4D.Transform(v, q);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.Transform did not return the expected value.");
        }

        // A test for Transform (Vector2Df, QuaternionD)
        // Transform Vector2Df by quaternion test
        [Fact]
        public void Vector4DTransformVector2DQuaternionDTest()
        {
            Vector2D v = new Vector2D(1.0f, 2.0f);

            Matrix4x4D m =
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0f)) *
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0f)) *
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0f));
            QuaternionD q = QuaternionD.CreateFromRotationMatrix(m);

            Vector4D expected = Vector4D.Transform(v, m);
            Vector4D actual = Vector4D.Transform(v, q);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.Transform did not return the expected value.");
        }

        // A test for Transform (Vector2Df, QuaternionD)
        // Transform Vector2Df with zero quaternion
        [Fact]
        public void Vector4DTransformVector2DQuaternionDTest1()
        {
            Vector2D v = new Vector2D(1.0f, 2.0f);
            QuaternionD q = new QuaternionD();
            Vector4D expected = Vector4D.Zero;

            Vector4D actual = Vector4D.Transform(v, q);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.Transform did not return the expected value.");
        }

        // A test for Transform (Vector2Df, Matrix4x4D)
        // Transform vector2 with identity QuaternionD
        [Fact]
        public void Vector4DTransformVector2DQuaternionDTest2()
        {
            Vector2D v = new Vector2D(1.0f, 2.0f);
            QuaternionD q = QuaternionD.Identity;
            Vector4D expected = new Vector4D(1.0f, 2.0f, 0, 1.0f);

            Vector4D actual = Vector4D.Transform(v, q);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.Transform did not return the expected value.");
        }

        // A test for Normalize (Vector4Df)
        [Fact]
        public void Vector4DNormalizeTest()
        {
            Vector4D a = new Vector4D(1.0f, 2.0f, 3.0f, 4.0f);

            Vector4D expected = new Vector4D(
                0.1825741858350553711523232609336f,
                0.3651483716701107423046465218672f,
                0.5477225575051661134569697828008f,
                0.7302967433402214846092930437344f);
            Vector4D actual;

            actual = Vector4D.Normalize(a);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.Normalize did not return the expected value.");
        }

        // A test for Normalize (Vector4Df)
        // Normalize vector of length one
        [Fact]
        public void Vector4DNormalizeTest1()
        {
            Vector4D a = new Vector4D(1.0f, 0.0f, 0.0f, 0.0f);

            Vector4D expected = new Vector4D(1.0f, 0.0f, 0.0f, 0.0f);
            Vector4D actual = Vector4D.Normalize(a);
            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.Normalize did not return the expected value.");
        }

        // A test for Normalize (Vector4Df)
        // Normalize vector of length zero
        [Fact]
        public void Vector4DNormalizeTest2()
        {
            Vector4D a = new Vector4D(0.0f, 0.0f, 0.0f, 0.0f);

            Vector4D expected = new Vector4D(0.0f, 0.0f, 0.0f, 0.0f);
            Vector4D actual = Vector4D.Normalize(a);
            Assert.True(float.IsNaN(actual.X) && float.IsNaN(actual.Y) && float.IsNaN(actual.Z) && float.IsNaN(actual.W), "Vector4Df.Normalize did not return the expected value.");
        }

        // A test for operator - (Vector4Df)
        [Fact]
        public void Vector4DUnaryNegationTest()
        {
            Vector4D a = new Vector4D(1.0f, 2.0f, 3.0f, 4.0f);

            Vector4D expected = new Vector4D(-1.0f, -2.0f, -3.0f, -4.0f);
            Vector4D actual;

            actual = -a;

            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.operator - did not return the expected value.");
        }

        // A test for operator - (Vector4Df, Vector4Df)
        [Fact]
        public void Vector4DSubtractionTest()
        {
            Vector4D a = new Vector4D(1.0f, 6.0f, 3.0f, 4.0f);
            Vector4D b = new Vector4D(5.0f, 2.0f, 3.0f, 9.0f);

            Vector4D expected = new Vector4D(-4.0f, 4.0f, 0.0f, -5.0f);
            Vector4D actual;

            actual = a - b;

            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.operator - did not return the expected value.");
        }

        // A test for operator * (Vector4Df, float)
        [Fact]
        public void Vector4DMultiplyOperatorTest()
        {
            Vector4D a = new Vector4D(1.0f, 2.0f, 3.0f, 4.0f);

            const float factor = 2.0f;

            Vector4D expected = new Vector4D(2.0f, 4.0f, 6.0f, 8.0f);
            Vector4D actual;

            actual = a * factor;
            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.operator * did not return the expected value.");
        }

        // A test for operator * (float, Vector4Df)
        [Fact]
        public void Vector4DMultiplyOperatorTest2()
        {
            Vector4D a = new Vector4D(1.0f, 2.0f, 3.0f, 4.0f);

            const float factor = 2.0f;
            Vector4D expected = new Vector4D(2.0f, 4.0f, 6.0f, 8.0f);
            Vector4D actual;

            actual = factor * a;
            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.operator * did not return the expected value.");
        }

        // A test for operator * (Vector4Df, Vector4Df)
        [Fact]
        public void Vector4DMultiplyOperatorTest3()
        {
            Vector4D a = new Vector4D(1.0f, 2.0f, 3.0f, 4.0f);
            Vector4D b = new Vector4D(5.0f, 6.0f, 7.0f, 8.0f);

            Vector4D expected = new Vector4D(5.0f, 12.0f, 21.0f, 32.0f);
            Vector4D actual;

            actual = a * b;

            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.operator * did not return the expected value.");
        }

        // A test for operator / (Vector4Df, float)
        [Fact]
        public void Vector4DDivisionTest()
        {
            Vector4D a = new Vector4D(1.0f, 2.0f, 3.0f, 4.0f);

            float div = 2.0f;

            Vector4D expected = new Vector4D(0.5f, 1.0f, 1.5f, 2.0f);
            Vector4D actual;

            actual = a / div;

            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.operator / did not return the expected value.");
        }

        // A test for operator / (Vector4Df, Vector4Df)
        [Fact]
        public void Vector4DDivisionTest1()
        {
            Vector4D a = new Vector4D(1.0f, 6.0f, 7.0f, 4.0f);
            Vector4D b = new Vector4D(5.0f, 2.0f, 3.0f, 8.0f);

            Vector4D expected = new Vector4D(1.0f / 5.0f, 6.0f / 2.0f, 7.0f / 3.0f, 4.0f / 8.0f);
            Vector4D actual;

            actual = a / b;

            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.operator / did not return the expected value.");
        }

        // A test for operator / (Vector4Df, Vector4Df)
        // Divide by zero
        [Fact]
        public void Vector4DDivisionTest2()
        {
            Vector4D a = new Vector4D(-2.0f, 3.0f, float.MaxValue, float.NaN);

            float div = 0.0f;

            Vector4D actual = a / div;

            Assert.True(float.IsNegativeInfinity(actual.X), "Vector4Df.operator / did not return the expected value.");
            Assert.True(float.IsPositiveInfinity(actual.Y), "Vector4Df.operator / did not return the expected value.");
            Assert.True(float.IsPositiveInfinity(actual.Z), "Vector4Df.operator / did not return the expected value.");
            Assert.True(float.IsNaN(actual.W), "Vector4Df.operator / did not return the expected value.");
        }

        // A test for operator / (Vector4Df, Vector4Df)
        // Divide by zero
        [Fact]
        public void Vector4DDivisionTest3()
        {
            Vector4D a = new Vector4D(0.047f, -3.0f, float.NegativeInfinity, float.MinValue);
            Vector4D b = new Vector4D();

            Vector4D actual = a / b;

            Assert.True(float.IsPositiveInfinity(actual.X), "Vector4Df.operator / did not return the expected value.");
            Assert.True(float.IsNegativeInfinity(actual.Y), "Vector4Df.operator / did not return the expected value.");
            Assert.True(float.IsNegativeInfinity(actual.Z), "Vector4Df.operator / did not return the expected value.");
            Assert.True(float.IsNegativeInfinity(actual.W), "Vector4Df.operator / did not return the expected value.");
        }

        // A test for operator + (Vector4Df, Vector4Df)
        [Fact]
        public void Vector4DAdditionTest()
        {
            Vector4D a = new Vector4D(1.0f, 2.0f, 3.0f, 4.0f);
            Vector4D b = new Vector4D(5.0f, 6.0f, 7.0f, 8.0f);

            Vector4D expected = new Vector4D(6.0f, 8.0f, 10.0f, 12.0f);
            Vector4D actual;

            actual = a + b;

            Assert.True(MathHelper.Equal(expected, actual), "Vector4Df.operator + did not return the expected value.");
        }

        [Fact]
        public void OperatorAddTest()
        {
            Vector4D v1 = new Vector4D(2.5f, 2.0f, 3.0f, 3.3f);
            Vector4D v2 = new Vector4D(5.5f, 4.5f, 6.5f, 7.5f);

            Vector4D v3 = v1 + v2;
            Vector4D v5 = new Vector4D(-1.0f, 0.0f, 0.0f, float.NaN);
            Vector4D v4 = v1 + v5;
            Assert.Equal(8.0f, v3.X);
            Assert.Equal(6.5f, v3.Y);
            Assert.Equal(9.5f, v3.Z);
            Assert.Equal(10.8f, v3.W);
            Assert.Equal(1.5f, v4.X);
            Assert.Equal(2.0f, v4.Y);
            Assert.Equal(3.0f, v4.Z);
            Assert.Equal(float.NaN, v4.W);
        }

        // A test for Vector4Df (float, float, float, float)
        [Fact]
        public void Vector4DConstructorTest()
        {
            float x = 1.0f;
            float y = 2.0f;
            float z = 3.0f;
            float w = 4.0f;

            Vector4D target = new Vector4D(x, y, z, w);

            Assert.True(MathHelper.Equal(target.X, x) && MathHelper.Equal(target.Y, y) && MathHelper.Equal(target.Z, z) && MathHelper.Equal(target.W, w),
                "Vector4Df constructor(x,y,z,w) did not return the expected value.");
        }

        // A test for Vector4Df (Vector2Df, float, float)
        [Fact]
        public void Vector4DConstructorTest1()
        {
            Vector2D a = new Vector2D(1.0f, 2.0f);
            float z = 3.0f;
            float w = 4.0f;

            Vector4D target = new Vector4D(a, z, w);
            Assert.True(MathHelper.Equal(target.X, a.X) && MathHelper.Equal(target.Y, a.Y) && MathHelper.Equal(target.Z, z) && MathHelper.Equal(target.W, w),
                "Vector4Df constructor(Vector2Df,z,w) did not return the expected value.");
        }

        // A test for Vector4Df (Vector3Df, float)
        [Fact]
        public void Vector4DConstructorTest2()
        {
            Vector3D a = new Vector3D(1.0f, 2.0f, 3.0f);
            float w = 4.0f;

            Vector4D target = new Vector4D(a, w);

            Assert.True(MathHelper.Equal(target.X, a.X) && MathHelper.Equal(target.Y, a.Y) && MathHelper.Equal(target.Z, a.Z) && MathHelper.Equal(target.W, w),
                "Vector4Df constructor(Vector3Df,w) did not return the expected value.");
        }

        // A test for Vector4Df ()
        // Constructor with no parameter
        [Fact]
        public void Vector4DConstructorTest4()
        {
            Vector4D a = new Vector4D();

            Assert.Equal(0.0f, a.X);
            Assert.Equal(0.0f, a.Y);
            Assert.Equal(0.0f, a.Z);
            Assert.Equal(0.0f, a.W);
        }

        // A test for Vector4Df ()
        // Constructor with special floating values
        [Fact]
        public void Vector4DConstructorTest5()
        {
            Vector4D target = new Vector4D(float.NaN, float.MaxValue, float.PositiveInfinity, float.Epsilon);

            Assert.True(float.IsNaN(target.X), "Vector4Df.constructor (float, float, float, float) did not return the expected value.");
            Assert.True(float.Equals(float.MaxValue, target.Y), "Vector4Df.constructor (float, float, float, float) did not return the expected value.");
            Assert.True(float.IsPositiveInfinity(target.Z), "Vector4Df.constructor (float, float, float, float) did not return the expected value.");
            Assert.True(float.Equals(float.Epsilon, target.W), "Vector4Df.constructor (float, float, float, float) did not return the expected value.");
        }

        // A test for Vector4Df (ReadOnlySpan<float>)
        [Fact]
        public void Vector4DConstructorTest7()
        {
            float value = 1.0f;
            Vector4D target = new Vector4D(new[] { value, value, value, value });
            Vector4D expected = new Vector4D(value);

            Assert.Equal(expected, target);
            Assert.Throws<ArgumentOutOfRangeException>(() => new Vector4D(new float[3]));
        }

        // A test for Add (Vector4Df, Vector4Df)
        [Fact]
        public void Vector4DAddTest()
        {
            Vector4D a = new Vector4D(1.0f, 2.0f, 3.0f, 4.0f);
            Vector4D b = new Vector4D(5.0f, 6.0f, 7.0f, 8.0f);

            Vector4D expected = new Vector4D(6.0f, 8.0f, 10.0f, 12.0f);
            Vector4D actual;

            actual = Vector4D.Add(a, b);
            Assert.Equal(expected, actual);
        }

        // A test for Divide (Vector4Df, float)
        [Fact]
        public void Vector4DDivideTest()
        {
            Vector4D a = new Vector4D(1.0f, 2.0f, 3.0f, 4.0f);
            float div = 2.0f;
            Vector4D expected = new Vector4D(0.5f, 1.0f, 1.5f, 2.0f);
            Vector4D actual;
            actual = Vector4D.Divide(a, div);
            Assert.Equal(expected, actual);
        }

        // A test for Divide (Vector4Df, Vector4Df)
        [Fact]
        public void Vector4DDivideTest1()
        {
            Vector4D a = new Vector4D(1.0f, 6.0f, 7.0f, 4.0f);
            Vector4D b = new Vector4D(5.0f, 2.0f, 3.0f, 8.0f);

            Vector4D expected = new Vector4D(1.0f / 5.0f, 6.0f / 2.0f, 7.0f / 3.0f, 4.0f / 8.0f);
            Vector4D actual;

            actual = Vector4D.Divide(a, b);
            Assert.Equal(expected, actual);
        }

        // A test for Equals (object)
        [Fact]
        public void Vector4DEqualsTest()
        {
            Vector4D a = new Vector4D(1.0f, 2.0f, 3.0f, 4.0f);
            Vector4D b = new Vector4D(1.0f, 2.0f, 3.0f, 4.0f);

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

        // A test for Multiply (float, Vector4Df)
        [Fact]
        public void Vector4DMultiplyTest()
        {
            Vector4D a = new Vector4D(1.0f, 2.0f, 3.0f, 4.0f);
            const float factor = 2.0f;
            Vector4D expected = new Vector4D(2.0f, 4.0f, 6.0f, 8.0f);
            Vector4D actual = Vector4D.Multiply(factor, a);
            Assert.Equal(expected, actual);
        }

        // A test for Multiply (Vector4Df, float)
        [Fact]
        public void Vector4DMultiplyTest2()
        {
            Vector4D a = new Vector4D(1.0f, 2.0f, 3.0f, 4.0f);
            const float factor = 2.0f;
            Vector4D expected = new Vector4D(2.0f, 4.0f, 6.0f, 8.0f);
            Vector4D actual = Vector4D.Multiply(a, factor);
            Assert.Equal(expected, actual);
        }

        // A test for Multiply (Vector4Df, Vector4Df)
        [Fact]
        public void Vector4DMultiplyTest3()
        {
            Vector4D a = new Vector4D(1.0f, 2.0f, 3.0f, 4.0f);
            Vector4D b = new Vector4D(5.0f, 6.0f, 7.0f, 8.0f);

            Vector4D expected = new Vector4D(5.0f, 12.0f, 21.0f, 32.0f);
            Vector4D actual;

            actual = Vector4D.Multiply(a, b);
            Assert.Equal(expected, actual);
        }

        // A test for Negate (Vector4Df)
        [Fact]
        public void Vector4DNegateTest()
        {
            Vector4D a = new Vector4D(1.0f, 2.0f, 3.0f, 4.0f);

            Vector4D expected = new Vector4D(-1.0f, -2.0f, -3.0f, -4.0f);
            Vector4D actual;

            actual = Vector4D.Negate(a);
            Assert.Equal(expected, actual);
        }

        // A test for operator != (Vector4Df, Vector4Df)
        [Fact]
        public void Vector4DInequalityTest()
        {
            Vector4D a = new Vector4D(1.0f, 2.0f, 3.0f, 4.0f);
            Vector4D b = new Vector4D(1.0f, 2.0f, 3.0f, 4.0f);

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

        // A test for operator == (Vector4Df, Vector4Df)
        [Fact]
        public void Vector4DEqualityTest()
        {
            Vector4D a = new Vector4D(1.0f, 2.0f, 3.0f, 4.0f);
            Vector4D b = new Vector4D(1.0f, 2.0f, 3.0f, 4.0f);

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

        // A test for Subtract (Vector4Df, Vector4Df)
        [Fact]
        public void Vector4DSubtractTest()
        {
            Vector4D a = new Vector4D(1.0f, 6.0f, 3.0f, 4.0f);
            Vector4D b = new Vector4D(5.0f, 2.0f, 3.0f, 9.0f);

            Vector4D expected = new Vector4D(-4.0f, 4.0f, 0.0f, -5.0f);
            Vector4D actual;

            actual = Vector4D.Subtract(a, b);

            Assert.Equal(expected, actual);
        }

        // A test for UnitW
        [Fact]
        public void Vector4DUnitWTest()
        {
            Vector4D val = new Vector4D(0.0f, 0.0f, 0.0f, 1.0f);
            Assert.Equal(val, Vector4D.UnitW);
        }

        // A test for UnitX
        [Fact]
        public void Vector4DUnitXTest()
        {
            Vector4D val = new Vector4D(1.0f, 0.0f, 0.0f, 0.0f);
            Assert.Equal(val, Vector4D.UnitX);
        }

        // A test for UnitY
        [Fact]
        public void Vector4DUnitYTest()
        {
            Vector4D val = new Vector4D(0.0f, 1.0f, 0.0f, 0.0f);
            Assert.Equal(val, Vector4D.UnitY);
        }

        // A test for UnitZ
        [Fact]
        public void Vector4DUnitZTest()
        {
            Vector4D val = new Vector4D(0.0f, 0.0f, 1.0f, 0.0f);
            Assert.Equal(val, Vector4D.UnitZ);
        }

        // A test for One
        [Fact]
        public void Vector4DOneTest()
        {
            Vector4D val = new Vector4D(1.0f, 1.0f, 1.0f, 1.0f);
            Assert.Equal(val, Vector4D.One);
        }

        // A test for Zero
        [Fact]
        public void Vector4DZeroTest()
        {
            Vector4D val = new Vector4D(0.0f, 0.0f, 0.0f, 0.0f);
            Assert.Equal(val, Vector4D.Zero);
        }

        // A test for Equals (Vector4Df)
        [Fact]
        public void Vector4DEqualsTest1()
        {
            Vector4D a = new Vector4D(1.0f, 2.0f, 3.0f, 4.0f);
            Vector4D b = new Vector4D(1.0f, 2.0f, 3.0f, 4.0f);

            // case 1: compare between same values
            Assert.True(a.Equals(b));

            // case 2: compare between different values
            b.X = 10.0f;
            Assert.False(a.Equals(b));
        }

        // A test for Vector4Df (float)
        [Fact]
        public void Vector4DConstructorTest6()
        {
            float value = 1.0f;
            Vector4D target = new Vector4D(value);

            Vector4D expected = new Vector4D(value, value, value, value);
            Assert.Equal(expected, target);

            value = 2.0f;
            target = new Vector4D(value);
            expected = new Vector4D(value, value, value, value);
            Assert.Equal(expected, target);
        }

        // A test for Vector4Df comparison involving NaN values
        [Fact]
        public void Vector4DEqualsNaNTest()
        {
            Vector4D a = new Vector4D(float.NaN, 0, 0, 0);
            Vector4D b = new Vector4D(0, float.NaN, 0, 0);
            Vector4D c = new Vector4D(0, 0, float.NaN, 0);
            Vector4D d = new Vector4D(0, 0, 0, float.NaN);

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
            Vector4D v1 = new Vector4D(-2.5f, 2.0f, 3.0f, 3.3f);
            Vector4D v3 = Vector4D.Abs(new Vector4D(float.PositiveInfinity, 0.0f, float.NegativeInfinity, float.NaN));
            Vector4D v = Vector4D.Abs(v1);
            Assert.Equal(2.5f, v.X);
            Assert.Equal(2.0f, v.Y);
            Assert.Equal(3.0f, v.Z);
            Assert.Equal(3.3f, v.W);
            Assert.Equal(float.PositiveInfinity, v3.X);
            Assert.Equal(0.0f, v3.Y);
            Assert.Equal(float.PositiveInfinity, v3.Z);
            Assert.Equal(float.NaN, v3.W);
        }

        [Fact]
        public void Vector4DSqrtTest()
        {
            Vector4D v1 = new Vector4D(-2.5f, 2.0f, 3.0f, 3.3f);
            Vector4D v2 = new Vector4D(5.5f, 4.5f, 6.5f, 7.5f);
            Assert.Equal(2, (int)Vector4D.SquareRoot(v2).X);
            Assert.Equal(2, (int)Vector4D.SquareRoot(v2).Y);
            Assert.Equal(2, (int)Vector4D.SquareRoot(v2).Z);
            Assert.Equal(2, (int)Vector4D.SquareRoot(v2).W);
            Assert.Equal(float.NaN, Vector4D.SquareRoot(v1).X);
        }

        // A test to make sure these types are blittable directly into GPU buffer memory layouts
        [Fact]
        public unsafe void Vector4DSizeofTest()
        {
            Assert.Equal(16, sizeof(Vector4D));
            Assert.Equal(32, sizeof(Vector4D_2x));
            Assert.Equal(20, sizeof(Vector4DPlusFloat));
            Assert.Equal(40, sizeof(Vector4DPlusFloat_2x));
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Vector4D_2x
        {
            private Vector4D _a;
            private Vector4D _b;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Vector4DPlusFloat
        {
            private Vector4D _v;
            private float _f;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Vector4DPlusFloat_2x
        {
            private Vector4DPlusFloat _a;
            private Vector4DPlusFloat _b;
        }

        [Fact]
        public void SetFieldsTest()
        {
            Vector4D v3 = new Vector4D(4f, 5f, 6f, 7f);
            v3.X = 1.0f;
            v3.Y = 2.0f;
            v3.Z = 3.0f;
            v3.W = 4.0f;
            Assert.Equal(1.0f, v3.X);
            Assert.Equal(2.0f, v3.Y);
            Assert.Equal(3.0f, v3.Z);
            Assert.Equal(4.0f, v3.W);
            Vector4D v4 = v3;
            v4.Y = 0.5f;
            v4.Z = 2.2f;
            v4.W = 3.5f;
            Assert.Equal(1.0f, v4.X);
            Assert.Equal(0.5f, v4.Y);
            Assert.Equal(2.2f, v4.Z);
            Assert.Equal(3.5f, v4.W);
            Assert.Equal(2.0f, v3.Y);
        }

        [Fact]
        public void EmbeddedVectorSetFields()
        {
            EmbeddedVectorObject evo = new EmbeddedVectorObject();
            evo.FieldVector.X = 5.0f;
            evo.FieldVector.Y = 5.0f;
            evo.FieldVector.Z = 5.0f;
            evo.FieldVector.W = 5.0f;
            Assert.Equal(5.0f, evo.FieldVector.X);
            Assert.Equal(5.0f, evo.FieldVector.Y);
            Assert.Equal(5.0f, evo.FieldVector.Z);
            Assert.Equal(5.0f, evo.FieldVector.W);
        }

        [Fact]
        public void DeeplyEmbeddedObjectTest()
        {
            DeeplyEmbeddedClass obj = new DeeplyEmbeddedClass();
            obj.L0.L1.L2.L3.L4.L5.L6.L7.EmbeddedVector.X = 5f;
            Assert.Equal(5f, obj.RootEmbeddedObject.X);
            Assert.Equal(5f, obj.RootEmbeddedObject.Y);
            Assert.Equal(1f, obj.RootEmbeddedObject.Z);
            Assert.Equal(-5f, obj.RootEmbeddedObject.W);
            obj.L0.L1.L2.L3.L4.L5.L6.L7.EmbeddedVector = new Vector4D(1, 2, 3, 4);
            Assert.Equal(1f, obj.RootEmbeddedObject.X);
            Assert.Equal(2f, obj.RootEmbeddedObject.Y);
            Assert.Equal(3f, obj.RootEmbeddedObject.Z);
            Assert.Equal(4f, obj.RootEmbeddedObject.W);
        }

        [Fact]
        public void DeeplyEmbeddedStructTest()
        {
            DeeplyEmbeddedStruct obj = DeeplyEmbeddedStruct.Create();
            obj.L0.L1.L2.L3.L4.L5.L6.L7.EmbeddedVector.X = 5f;
            Assert.Equal(5f, obj.RootEmbeddedObject.X);
            Assert.Equal(5f, obj.RootEmbeddedObject.Y);
            Assert.Equal(1f, obj.RootEmbeddedObject.Z);
            Assert.Equal(-5f, obj.RootEmbeddedObject.W);
            obj.L0.L1.L2.L3.L4.L5.L6.L7.EmbeddedVector = new Vector4D(1, 2, 3, 4);
            Assert.Equal(1f, obj.RootEmbeddedObject.X);
            Assert.Equal(2f, obj.RootEmbeddedObject.Y);
            Assert.Equal(3f, obj.RootEmbeddedObject.Z);
            Assert.Equal(4f, obj.RootEmbeddedObject.W);
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
                private float _buffer0, _buffer1;
                public Level1 L1;
                private float _buffer2;
                public struct Level1
                {
                    private float _buffer0, _buffer1;
                    public Level2 L2;
                    private byte _buffer2;
                    public struct Level2
                    {
                        public Level3 L3;
                        private float _buffer0;
                        private byte _buffer1;
                        public struct Level3
                        {
                            public Level4 L4;
                            public struct Level4
                            {
                                private float _buffer0;
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
        [MemberData(nameof(GenericMathTestMemberData.CosSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void CosSingleTest(float value, float expectedResult, float variance)
        {
            Vector4D actualResult = Vector4D.Cos(Vector4D.Create(value));
            AssertEqual(Vector4D.Create(expectedResult), actualResult, Vector4D.Create(variance));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.ExpSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void ExpSingleTest(float value, float expectedResult, float variance)
        {
            Vector4D actualResult = Vector4D.Exp(Vector4D.Create(value));
            AssertEqual(Vector4D.Create(expectedResult), actualResult, Vector4D.Create(variance));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.LogSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void LogSingleTest(float value, float expectedResult, float variance)
        {
            Vector4D actualResult = Vector4D.Log(Vector4D.Create(value));
            AssertEqual(Vector4D.Create(expectedResult), actualResult, Vector4D.Create(variance));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.Log2Single), MemberType = typeof(GenericMathTestMemberData))]
        public void Log2SingleTest(float value, float expectedResult, float variance)
        {
            Vector4D actualResult = Vector4D.Log2(Vector4D.Create(value));
            AssertEqual(Vector4D.Create(expectedResult), actualResult, Vector4D.Create(variance));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.FusedMultiplyAddSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void FusedMultiplyAddSingleTest(float left, float right, float addend, float expectedResult)
        {
            AssertEqual(Vector4D.Create(expectedResult), Vector4D.FusedMultiplyAdd(Vector4D.Create(left), Vector4D.Create(right), Vector4D.Create(addend)), Vector4D.Zero);
            AssertEqual(Vector4D.Create(float.MultiplyAddEstimate(left, right, addend)), Vector4D.MultiplyAddEstimate(Vector4D.Create(left), Vector4D.Create(right), Vector4D.Create(addend)), Vector4D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.ClampSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void ClampSingleTest(float x, float min, float max, float expectedResult)
        {
            Vector4D actualResult = Vector4D.Clamp(Vector4D.Create(x), Vector4D.Create(min), Vector4D.Create(max));
            AssertEqual(Vector4D.Create(expectedResult), actualResult, Vector4D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.CopySignSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void CopySignSingleTest(float x, float y, float expectedResult)
        {
            Vector4D actualResult = Vector4D.CopySign(Vector4D.Create(x), Vector4D.Create(y));
            AssertEqual(Vector4D.Create(expectedResult), actualResult, Vector4D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.DegreesToRadiansSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void DegreesToRadiansSingleTest(float value, float expectedResult, float variance)
        {
            AssertEqual(Vector4D.Create(-expectedResult), Vector4D.DegreesToRadians(Vector4D.Create(-value)), Vector4D.Create(variance));
            AssertEqual(Vector4D.Create(+expectedResult), Vector4D.DegreesToRadians(Vector4D.Create(+value)), Vector4D.Create(variance));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.HypotSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void HypotSingleTest(float x, float y, float expectedResult, float variance)
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
        [MemberData(nameof(GenericMathTestMemberData.LerpSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void LerpSingleTest(float x, float y, float amount, float expectedResult)
        {
            AssertEqual(Vector4D.Create(+expectedResult), Vector4D.Lerp(Vector4D.Create(+x), Vector4D.Create(+y), Vector4D.Create(amount)), Vector4D.Zero);
            AssertEqual(Vector4D.Create((expectedResult == 0.0f) ? expectedResult : -expectedResult), Vector4D.Lerp(Vector4D.Create(-x), Vector4D.Create(-y), Vector4D.Create(amount)), Vector4D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MaxSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void MaxSingleTest(float x, float y, float expectedResult)
        {
            Vector4D actualResult = Vector4D.Max(Vector4D.Create(x), Vector4D.Create(y));
            AssertEqual(Vector4D.Create(expectedResult), actualResult, Vector4D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MaxMagnitudeSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void MaxMagnitudeSingleTest(float x, float y, float expectedResult)
        {
            Vector4D actualResult = Vector4D.MaxMagnitude(Vector4D.Create(x), Vector4D.Create(y));
            AssertEqual(Vector4D.Create(expectedResult), actualResult, Vector4D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MaxMagnitudeNumberSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void MaxMagnitudeNumberSingleTest(float x, float y, float expectedResult)
        {
            Vector4D actualResult = Vector4D.MaxMagnitudeNumber(Vector4D.Create(x), Vector4D.Create(y));
            AssertEqual(Vector4D.Create(expectedResult), actualResult, Vector4D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MaxNumberSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void MaxNumberSingleTest(float x, float y, float expectedResult)
        {
            Vector4D actualResult = Vector4D.MaxNumber(Vector4D.Create(x), Vector4D.Create(y));
            AssertEqual(Vector4D.Create(expectedResult), actualResult, Vector4D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MinSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void MinSingleTest(float x, float y, float expectedResult)
        {
            Vector4D actualResult = Vector4D.Min(Vector4D.Create(x), Vector4D.Create(y));
            AssertEqual(Vector4D.Create(expectedResult), actualResult, Vector4D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MinMagnitudeSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void MinMagnitudeSingleTest(float x, float y, float expectedResult)
        {
            Vector4D actualResult = Vector4D.MinMagnitude(Vector4D.Create(x), Vector4D.Create(y));
            AssertEqual(Vector4D.Create(expectedResult), actualResult, Vector4D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MinMagnitudeNumberSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void MinMagnitudeNumberSingleTest(float x, float y, float expectedResult)
        {
            Vector4D actualResult = Vector4D.MinMagnitudeNumber(Vector4D.Create(x), Vector4D.Create(y));
            AssertEqual(Vector4D.Create(expectedResult), actualResult, Vector4D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MinNumberSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void MinNumberSingleTest(float x, float y, float expectedResult)
        {
            Vector4D actualResult = Vector4D.MinNumber(Vector4D.Create(x), Vector4D.Create(y));
            AssertEqual(Vector4D.Create(expectedResult), actualResult, Vector4D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.RadiansToDegreesSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void RadiansToDegreesSingleTest(float value, float expectedResult, float variance)
        {
            AssertEqual(Vector4D.Create(-expectedResult), Vector4D.RadiansToDegrees(Vector4D.Create(-value)), Vector4D.Create(variance));
            AssertEqual(Vector4D.Create(+expectedResult), Vector4D.RadiansToDegrees(Vector4D.Create(+value)), Vector4D.Create(variance));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.RoundSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void RoundSingleTest(float value, float expectedResult)
        {
            Vector4D actualResult = Vector4D.Round(Vector4D.Create(value));
            AssertEqual(Vector4D.Create(expectedResult), actualResult, Vector4D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.RoundAwayFromZeroSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void RoundAwayFromZeroSingleTest(float value, float expectedResult)
        {
            Vector4D actualResult = Vector4D.Round(Vector4D.Create(value), MidpointRounding.AwayFromZero);
            AssertEqual(Vector4D.Create(expectedResult), actualResult, Vector4D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.RoundToEvenSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void RoundToEvenSingleTest(float value, float expectedResult)
        {
            Vector4D actualResult = Vector4D.Round(Vector4D.Create(value), MidpointRounding.ToEven);
            AssertEqual(Vector4D.Create(expectedResult), actualResult, Vector4D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.SinSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void SinSingleTest(float value, float expectedResult, float variance)
        {
            Vector4D actualResult = Vector4D.Sin(Vector4D.Create(value));
            AssertEqual(Vector4D.Create(expectedResult), actualResult, Vector4D.Create(variance));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.SinCosSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void SinCosSingleTest(float value, float expectedResultSin, float expectedResultCos, float allowedVarianceSin, float allowedVarianceCos)
        {
            (Vector4D resultSin, Vector4D resultCos) = Vector4D.SinCos(Vector4D.Create(value));
            AssertEqual(Vector4D.Create(expectedResultSin), resultSin, Vector4D.Create(allowedVarianceSin));
            AssertEqual(Vector4D.Create(expectedResultCos), resultCos, Vector4D.Create(allowedVarianceCos));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.TruncateSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void TruncateSingleTest(float value, float expectedResult)
        {
            Vector4D actualResult = Vector4D.Truncate(Vector4D.Create(value));
            AssertEqual(Vector4D.Create(expectedResult), actualResult, Vector4D.Zero);
        }

        [Fact]
        public void AllAnyNoneTest()
        {
            Test(3, 2);

            [MethodImpl(MethodImplOptions.NoInlining)]
            void Test(float value1, float value2)
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
            Test(BitConverter.Int32BitsToSingle(-1));

            [MethodImpl(MethodImplOptions.NoInlining)]
            void Test(float value)
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
            Test(BitConverter.Int32BitsToSingle(-1), 2);

            [MethodImpl(MethodImplOptions.NoInlining)]
            void Test(float allBitsSet, float value2)
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
            void Test(float value1, float value2)
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
            Test(BitConverter.Int32BitsToSingle(-1));

            [MethodImpl(MethodImplOptions.NoInlining)]
            void Test(float value)
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
            Test(BitConverter.Int32BitsToSingle(-1), 2);

            [MethodImpl(MethodImplOptions.NoInlining)]
            void Test(float allBitsSet, float value2)
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
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsEvenIntegerTest(float value) => Assert.Equal(float.IsEvenInteger(value) ? Vector4D.AllBitsSet : Vector4D.Zero, Vector4D.IsEvenInteger(Vector4D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsFiniteTest(float value) => Assert.Equal(float.IsFinite(value) ? Vector4D.AllBitsSet : Vector4D.Zero, Vector4D.IsFinite(Vector4D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsInfinityTest(float value) => Assert.Equal(float.IsInfinity(value) ? Vector4D.AllBitsSet : Vector4D.Zero, Vector4D.IsInfinity(Vector4D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsIntegerTest(float value) => Assert.Equal(float.IsInteger(value) ? Vector4D.AllBitsSet : Vector4D.Zero, Vector4D.IsInteger(Vector4D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsNaNTest(float value) => Assert.Equal(float.IsNaN(value) ? Vector4D.AllBitsSet : Vector4D.Zero, Vector4D.IsNaN(Vector4D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsNegativeTest(float value) => Assert.Equal(float.IsNegative(value) ? Vector4D.AllBitsSet : Vector4D.Zero, Vector4D.IsNegative(Vector4D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsNegativeInfinityTest(float value) => Assert.Equal(float.IsNegativeInfinity(value) ? Vector4D.AllBitsSet : Vector4D.Zero, Vector4D.IsNegativeInfinity(Vector4D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsNormalTest(float value) => Assert.Equal(float.IsNormal(value) ? Vector4D.AllBitsSet : Vector4D.Zero, Vector4D.IsNormal(Vector4D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsOddIntegerTest(float value) => Assert.Equal(float.IsOddInteger(value) ? Vector4D.AllBitsSet : Vector4D.Zero, Vector4D.IsOddInteger(Vector4D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsPositiveTest(float value) => Assert.Equal(float.IsPositive(value) ? Vector4D.AllBitsSet : Vector4D.Zero, Vector4D.IsPositive(Vector4D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsPositiveInfinityTest(float value) => Assert.Equal(float.IsPositiveInfinity(value) ? Vector4D.AllBitsSet : Vector4D.Zero, Vector4D.IsPositiveInfinity(Vector4D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsSubnormalTest(float value) => Assert.Equal(float.IsSubnormal(value) ? Vector4D.AllBitsSet : Vector4D.Zero, Vector4D.IsSubnormal(Vector4D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsZeroSingleTest(float value) => Assert.Equal((value == 0) ? Vector4D.AllBitsSet : Vector4D.Zero, Vector4D.IsZero(Vector4D.Create(value)));

        [Fact]
        public void AllBitsSetTest()
        {
            Assert.Equal(-1, BitConverter.SingleToInt32Bits(Vector4D.AllBitsSet.X));
            Assert.Equal(-1, BitConverter.SingleToInt32Bits(Vector4D.AllBitsSet.Y));
            Assert.Equal(-1, BitConverter.SingleToInt32Bits(Vector4D.AllBitsSet.Z));
            Assert.Equal(-1, BitConverter.SingleToInt32Bits(Vector4D.AllBitsSet.W));
        }

        [Fact]
        public void ConditionalSelectTest()
        {
            Test(Vector4D.Create(1, 2, 3, 4), Vector4D.AllBitsSet, Vector4D.Create(1, 2, 3, 4), Vector4D.Create(5, 6, 7, 8));
            Test(Vector4D.Create(5, 6, 7, 8), Vector4D.Zero, Vector4D.Create(1, 2, 3, 4), Vector4D.Create(5, 6, 7, 8));
            Test(Vector4D.Create(1, 6, 3, 8), Vector128.Create(-1, 0, -1, 0).AsSingle().AsVector4D(), Vector4D.Create(1, 2, 3, 4), Vector4D.Create(5, 6, 7, 8));

            [MethodImpl(MethodImplOptions.NoInlining)]
            void Test(Vector4D expectedResult, Vector4D condition, Vector4D left, Vector4D right)
            {
                Assert.Equal(expectedResult, Vector4D.ConditionalSelect(condition, left, right));
            }
        }

        [Theory]
        [InlineData(+0.0f, +0.0f, +0.0f, +0.0f, 0b0000)]
        [InlineData(-0.0f, +1.0f, -0.0f, +0.0f, 0b0101)]
        [InlineData(-0.0f, -0.0f, -0.0f, -0.0f, 0b1111)]
        public void ExtractMostSignificantBitsTest(float x, float y, float z, float w, uint expectedResult)
        {
            Assert.Equal(expectedResult, Vector4D.Create(x, y, z, w).ExtractMostSignificantBits());
        }

        [Theory]
        [InlineData(1.0f, 2.0f, 3.0f, 4.0f)]
        [InlineData(5.0f, 6.0f, 7.0f, 8.0f)]
        public void GetElementTest(float x, float y, float z, float w)
        {
            Assert.Equal(x, Vector4D.Create(x, y, z, w).GetElement(0));
            Assert.Equal(y, Vector4D.Create(x, y, z, w).GetElement(1));
            Assert.Equal(z, Vector4D.Create(x, y, z, w).GetElement(2));
            Assert.Equal(w, Vector4D.Create(x, y, z, w).GetElement(3));
        }

        [Theory]
        [InlineData(1.0f, 2.0f, 3.0f, 4.0f)]
        [InlineData(5.0f, 6.0f, 7.0f, 8.0f)]
        public void ShuffleTest(float x, float y, float z, float w)
        {
            Assert.Equal(Vector4D.Create(w, z, y, x), Vector4D.Shuffle(Vector4D.Create(x, y, z, w), 3, 2, 1, 0));
            Assert.Equal(Vector4D.Create(y, x, w, z), Vector4D.Shuffle(Vector4D.Create(x, y, z, w), 1, 0, 3, 2));
        }

        [Theory]
        [InlineData(1.0f, 2.0f, 3.0f, 4.0f, 10.0f)]
        [InlineData(5.0f, 6.0f, 7.0f, 8.0f, 26.0f)]
        public void SumTest(float x, float y, float z, float w, float expectedResult)
        {
            Assert.Equal(expectedResult, Vector4D.Sum(Vector4D.Create(x, y, z, w)));
        }

        [Theory]
        [InlineData(1.0f, 2.0f, 3.0f, 4.0f)]
        [InlineData(5.0f, 6.0f, 7.0f, 8.0f)]
        public void ToScalarTest(float x, float y, float z, float w)
        {
            Assert.Equal(x, Vector4D.Create(x, y, z, w).ToScalar());
        }

        [Theory]
        [InlineData(1.0f, 2.0f, 3.0f, 4.0f)]
        [InlineData(5.0f, 6.0f, 7.0f, 8.0f)]
        public void WithElementTest(float x, float y, float z, float w)
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
            var vector = Vector4D.CreateScalar(float.Pi);

            Assert.Equal(float.Pi, vector.X);
            Assert.Equal(0, vector.Y);
            Assert.Equal(0, vector.Z);
            Assert.Equal(0, vector.W);

            vector = Vector4D.CreateScalar(float.E);

            Assert.Equal(float.E, vector.X);
            Assert.Equal(0, vector.Y);
            Assert.Equal(0, vector.Z);
            Assert.Equal(0, vector.W);
        }

        [Fact]
        public void CreateScalarUnsafeTest()
        {
            var vector = Vector4D.CreateScalarUnsafe(float.Pi);
            Assert.Equal(float.Pi, vector.X);

            vector = Vector4D.CreateScalarUnsafe(float.E);
            Assert.Equal(float.E, vector.X);
        }
    }
}
