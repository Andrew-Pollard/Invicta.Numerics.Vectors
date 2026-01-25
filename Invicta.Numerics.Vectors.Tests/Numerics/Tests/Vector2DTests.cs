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
        [InlineData(0.0f, 1.0f)]
        [InlineData(1.0f, 0.0f)]
        [InlineData(3.1434343f, 1.1234123f)]
        [InlineData(1.0000001f, 0.0000001f)]
        public void Vector2DIndexerGetTest(float x, float y)
        {
            var vector = new Vector2D(x, y);

            Assert.Equal(x, vector[0]);
            Assert.Equal(y, vector[1]);
        }

        [Theory]
        [InlineData(0.0f, 1.0f)]
        [InlineData(1.0f, 0.0f)]
        [InlineData(3.1434343f, 1.1234123f)]
        [InlineData(1.0000001f, 0.0000001f)]
        public void Vector2DIndexerSetTest(float x, float y)
        {
            var vector = new Vector2D(0.0f, 0.0f);

            vector[0] = x;
            vector[1] = y;

            Assert.Equal(x, vector[0]);
            Assert.Equal(y, vector[1]);
        }

        [Fact]
        public void Vector2DCopyToTest()
        {
            Vector2D v1 = new Vector2D(2.0f, 3.0f);

            float[] a = new float[3];
            float[] b = new float[2];

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
            Vector2D vector = new Vector2D(1.0f, 2.0f);
            Span<float> destination = new float[2];

            Assert.Throws<ArgumentException>(() => vector.CopyTo(new Span<float>(new float[1])));
            vector.CopyTo(destination);

            Assert.Equal(1.0f, vector.X);
            Assert.Equal(2.0f, vector.Y);
            Assert.Equal(vector.X, destination[0]);
            Assert.Equal(vector.Y, destination[1]);
        }

        [Fact]
        public void Vector2DTryCopyToTest()
        {
            Vector2D vector = new Vector2D(1.0f, 2.0f);
            Span<float> destination = new float[2];

            Assert.False(vector.TryCopyTo(new Span<float>(new float[1])));
            Assert.True(vector.TryCopyTo(destination));

            Assert.Equal(1.0f, vector.X);
            Assert.Equal(2.0f, vector.Y);
            Assert.Equal(vector.X, destination[0]);
            Assert.Equal(vector.Y, destination[1]);
        }

        [Fact]
        public void Vector2DGetHashCodeTest()
        {
            Vector2D v1 = new Vector2D(2.0f, 3.0f);
            Vector2D v2 = new Vector2D(2.0f, 3.0f);
            Vector2D v3 = new Vector2D(3.0f, 2.0f);
            Assert.Equal(v1.GetHashCode(), v1.GetHashCode());
            Assert.Equal(v1.GetHashCode(), v2.GetHashCode());
            Assert.NotEqual(v1.GetHashCode(), v3.GetHashCode());
            Vector2D v4 = new Vector2D(0.0f, 0.0f);
            Vector2D v6 = new Vector2D(1.0f, 0.0f);
            Vector2D v7 = new Vector2D(0.0f, 1.0f);
            Vector2D v8 = new Vector2D(1.0f, 1.0f);
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

            Vector2D v1 = new Vector2D(2.0f, 3.0f);

            string v1str = v1.ToString();
            string expectedv1 = string.Format(CultureInfo.CurrentCulture
                , "<{1:G}{0} {2:G}>"
                , new object[] { separator, 2, 3 });
            Assert.Equal(expectedv1, v1str);

            string v1strformatted = v1.ToString("c", CultureInfo.CurrentCulture);
            string expectedv1formatted = string.Format(CultureInfo.CurrentCulture
                , "<{1:c}{0} {2:c}>"
                , new object[] { separator, 2, 3 });
            Assert.Equal(expectedv1formatted, v1strformatted);

            string v2strformatted = v1.ToString("c", enUsCultureInfo);
            string expectedv2formatted = string.Format(enUsCultureInfo
                , "<{1:c}{0} {2:c}>"
                , new object[] { enUsCultureInfo.NumberFormat.NumberGroupSeparator, 2, 3 });
            Assert.Equal(expectedv2formatted, v2strformatted);

            string v3strformatted = v1.ToString("c");
            string expectedv3formatted = string.Format(CultureInfo.CurrentCulture
                , "<{1:c}{0} {2:c}>"
                , new object[] { separator, 2, 3 });
            Assert.Equal(expectedv3formatted, v3strformatted);
        }

        // A test for Distance (Vector2Df, Vector2Df)
        [Fact]
        public void Vector2DDistanceTest()
        {
            Vector2D a = new Vector2D(1.0f, 2.0f);
            Vector2D b = new Vector2D(3.0f, 4.0f);

            float expected = (float)System.Math.Sqrt(8);
            float actual;

            actual = Vector2D.Distance(a, b);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Distance did not return the expected value.");
        }

        // A test for Distance (Vector2Df, Vector2Df)
        // Distance from the same point
        [Fact]
        public void Vector2DDistanceTest2()
        {
            Vector2D a = new Vector2D(1.051f, 2.05f);
            Vector2D b = new Vector2D(1.051f, 2.05f);

            float actual = Vector2D.Distance(a, b);
            Assert.Equal(0.0f, actual);
        }

        // A test for DistanceSquared (Vector2Df, Vector2Df)
        [Fact]
        public void Vector2DDistanceSquaredTest()
        {
            Vector2D a = new Vector2D(1.0f, 2.0f);
            Vector2D b = new Vector2D(3.0f, 4.0f);

            float expected = 8.0f;
            float actual;

            actual = Vector2D.DistanceSquared(a, b);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.DistanceSquared did not return the expected value.");
        }

        // A test for Dot (Vector2Df, Vector2Df)
        [Fact]
        public void Vector2DDotTest()
        {
            Vector2D a = new Vector2D(1.0f, 2.0f);
            Vector2D b = new Vector2D(3.0f, 4.0f);

            float expected = 11.0f;
            float actual;

            actual = Vector2D.Dot(a, b);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Dot did not return the expected value.");
        }

        // A test for Dot (Vector2Df, Vector2Df)
        // Dot test for perpendicular vector
        [Fact]
        public void Vector2DDotTest1()
        {
            Vector2D a = new Vector2D(1.55f, 1.55f);
            Vector2D b = new Vector2D(-1.55f, 1.55f);

            float expected = 0.0f;
            float actual = Vector2D.Dot(a, b);
            Assert.Equal(expected, actual);
        }

        // A test for Dot (Vector2Df, Vector2Df)
        // Dot test with specail float values
        [Fact]
        public void Vector2DDotTest2()
        {
            Vector2D a = new Vector2D(float.MinValue, float.MinValue);
            Vector2D b = new Vector2D(float.MaxValue, float.MaxValue);

            float actual = Vector2D.Dot(a, b);
            Assert.True(float.IsNegativeInfinity(actual), "Vector2Df.Dot did not return the expected value.");
        }

        [Fact]
        public void Vector2DCrossTest()
        {
            Vector2D a = new Vector2D(1.0f, 2.0f);
            Vector2D b = new Vector2D(-4.0f, 3.0f);

            float expected = 11.0f;
            float actual = Vector2D.Cross(a, b);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Cross did not return the expected value.");
        }

        [Fact]
        public void Vector2DCrossTest1()
        {
            // Cross test for parallel vector
            Vector2D a = new Vector2D(1.55f, 1.55f);
            Vector2D b = new Vector2D(-1.55f, -1.55f);

            float expected = 0.0f;
            float actual = Vector2D.Cross(a, b);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Vector2DCrossTest2()
        {
            // Cross test with specail float values
            Vector2D a = new Vector2D(float.MinValue, float.MinValue);
            Vector2D b = new Vector2D(float.MinValue, float.MaxValue);

            float actual = Vector2D.Cross(a, b);
            Assert.True(float.IsNegativeInfinity(actual), "Vector2Df.Cross did not return the expected value.");
        }

        // A test for Length ()
        [Fact]
        public void Vector2DLengthTest()
        {
            Vector2D a = new Vector2D(2.0f, 4.0f);

            Vector2D target = a;

            float expected = (float)System.Math.Sqrt(20);
            float actual;

            actual = target.Length();

            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Length did not return the expected value.");
        }

        // A test for Length ()
        // Length test where length is zero
        [Fact]
        public void Vector2DLengthTest1()
        {
            Vector2D target = new Vector2D();
            target.X = 0.0f;
            target.Y = 0.0f;

            float expected = 0.0f;
            float actual;

            actual = target.Length();

            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Length did not return the expected value.");
        }

        // A test for LengthSquared ()
        [Fact]
        public void Vector2DLengthSquaredTest()
        {
            Vector2D a = new Vector2D(2.0f, 4.0f);

            Vector2D target = a;

            float expected = 20.0f;
            float actual;

            actual = target.LengthSquared();

            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.LengthSquared did not return the expected value.");
        }

        // A test for LengthSquared ()
        // LengthSquared test where the result is zero
        [Fact]
        public void Vector2DLengthSquaredTest1()
        {
            Vector2D a = new Vector2D(0.0f, 0.0f);

            float expected = 0.0f;
            float actual = a.LengthSquared();

            Assert.Equal(expected, actual);
        }

        // A test for Min (Vector2Df, Vector2Df)
        [Fact]
        public void Vector2DMinTest()
        {
            Vector2D a = new Vector2D(-1.0f, 4.0f);
            Vector2D b = new Vector2D(2.0f, 1.0f);

            Vector2D expected = new Vector2D(-1.0f, 1.0f);
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
            Vector2D a = new Vector2D(-1.0f, 4.0f);
            Vector2D b = new Vector2D(2.0f, 1.0f);

            Vector2D expected = new Vector2D(2.0f, 4.0f);
            Vector2D actual;
            actual = Vector2D.Max(a, b);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Max did not return the expected value.");
        }

        // A test for Clamp (Vector2Df, Vector2Df, Vector2Df)
        [Fact]
        public void Vector2DClampTest()
        {
            Vector2D a = new Vector2D(0.5f, 0.3f);
            Vector2D min = new Vector2D(0.0f, 0.1f);
            Vector2D max = new Vector2D(1.0f, 1.1f);

            // Normal case.
            // Case N1: specified value is in the range.
            Vector2D expected = new Vector2D(0.5f, 0.3f);
            Vector2D actual = Vector2D.Clamp(a, min, max);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Clamp did not return the expected value.");
            // Normal case.
            // Case N2: specified value is bigger than max value.
            a = new Vector2D(2.0f, 3.0f);
            expected = max;
            actual = Vector2D.Clamp(a, min, max);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Clamp did not return the expected value.");
            // Case N3: specified value is smaller than max value.
            a = new Vector2D(-1.0f, -2.0f);
            expected = min;
            actual = Vector2D.Clamp(a, min, max);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Clamp did not return the expected value.");
            // Case N4: combination case.
            a = new Vector2D(-2.0f, 4.0f);
            expected = new Vector2D(min.X, max.Y);
            actual = Vector2D.Clamp(a, min, max);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Clamp did not return the expected value.");
            // User specified min value is bigger than max value.
            max = new Vector2D(0.0f, 0.1f);
            min = new Vector2D(1.0f, 1.1f);

            // Case W1: specified value is in the range.
            a = new Vector2D(0.5f, 0.3f);
            expected = max;
            actual = Vector2D.Clamp(a, min, max);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Clamp did not return the expected value.");

            // Normal case.
            // Case W2: specified value is bigger than max and min value.
            a = new Vector2D(2.0f, 3.0f);
            expected = max;
            actual = Vector2D.Clamp(a, min, max);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Clamp did not return the expected value.");

            // Case W3: specified value is smaller than min and max value.
            a = new Vector2D(-1.0f, -2.0f);
            expected = max;
            actual = Vector2D.Clamp(a, min, max);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Clamp did not return the expected value.");
        }

        // A test for Lerp (Vector2Df, Vector2Df, float)
        [Fact]
        public void Vector2DLerpTest()
        {
            Vector2D a = new Vector2D(1.0f, 2.0f);
            Vector2D b = new Vector2D(3.0f, 4.0f);

            float t = 0.5f;

            Vector2D expected = new Vector2D(2.0f, 3.0f);
            Vector2D actual;
            actual = Vector2D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector2Df, Vector2Df, float)
        // Lerp test with factor zero
        [Fact]
        public void Vector2DLerpTest1()
        {
            Vector2D a = new Vector2D(0.0f, 0.0f);
            Vector2D b = new Vector2D(3.18f, 4.25f);

            float t = 0.0f;
            Vector2D expected = Vector2D.Zero;
            Vector2D actual = Vector2D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector2Df, Vector2Df, float)
        // Lerp test with factor one
        [Fact]
        public void Vector2DLerpTest2()
        {
            Vector2D a = new Vector2D(0.0f, 0.0f);
            Vector2D b = new Vector2D(3.18f, 4.25f);

            float t = 1.0f;
            Vector2D expected = new Vector2D(3.18f, 4.25f);
            Vector2D actual = Vector2D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector2Df, Vector2Df, float)
        // Lerp test with factor > 1
        [Fact]
        public void Vector2DLerpTest3()
        {
            Vector2D a = new Vector2D(0.0f, 0.0f);
            Vector2D b = new Vector2D(3.18f, 4.25f);

            float t = 2.0f;
            Vector2D expected = b * 2.0f;
            Vector2D actual = Vector2D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector2Df, Vector2Df, float)
        // Lerp test with factor < 0
        [Fact]
        public void Vector2DLerpTest4()
        {
            Vector2D a = new Vector2D(0.0f, 0.0f);
            Vector2D b = new Vector2D(3.18f, 4.25f);

            float t = -2.0f;
            Vector2D expected = -(b * 2.0f);
            Vector2D actual = Vector2D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector2Df, Vector2Df, float)
        // Lerp test with special float value
        [Fact]
        public void Vector2DLerpTest5()
        {
            Vector2D a = new Vector2D(45.67f, 90.0f);
            Vector2D b = new Vector2D(float.PositiveInfinity, float.NegativeInfinity);

            float t = 0.408f;
            Vector2D actual = Vector2D.Lerp(a, b, t);
            Assert.True(float.IsPositiveInfinity(actual.X), "Vector2Df.Lerp did not return the expected value.");
            Assert.True(float.IsNegativeInfinity(actual.Y), "Vector2Df.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector2Df, Vector2Df, float)
        // Lerp test from the same point
        [Fact]
        public void Vector2DLerpTest6()
        {
            Vector2D a = new Vector2D(1.0f, 2.0f);
            Vector2D b = new Vector2D(1.0f, 2.0f);

            float t = 0.5f;

            Vector2D expected = new Vector2D(1.0f, 2.0f);
            Vector2D actual = Vector2D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector2Df, Vector2Df, float)
        // Lerp test with values known to be inaccurate with the old lerp impl
        [Fact]
        public void Vector2DLerpTest7()
        {
            Vector2D a = new Vector2D(0.44728136f);
            Vector2D b = new Vector2D(0.46345946f);

            float t = 0.26402435f;

            Vector2D expected = new Vector2D(0.45155275f);
            Vector2D actual = Vector2D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector2Df, Vector2Df, float)
        // Lerp test with values known to be inaccurate with the old lerp impl
        // (Old code incorrectly gets 0.33333588)
        [Fact]
        public void Vector2DLerpTest8()
        {
            Vector2D a = new Vector2D(-100);
            Vector2D b = new Vector2D(0.33333334f);

            float t = 1f;

            Vector2D expected = new Vector2D(0.33333334f);
            Vector2D actual = Vector2D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Lerp did not return the expected value.");
        }

        // A test for Transform(Vector2Df, Matrix4x4D)
        [Fact]
        public void Vector2DTransformTest()
        {
            Vector2D v = new Vector2D(1.0f, 2.0f);
            Matrix4x4D m =
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0f)) *
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0f)) *
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0f));
            m.M41 = 10.0f;
            m.M42 = 20.0f;
            m.M43 = 30.0f;

            Vector2D expected = new Vector2D(10.316987f, 22.183012f);
            Vector2D actual;

            actual = Vector2D.Transform(v, m);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Transform did not return the expected value.");
        }

        // A test for Transform(Vector2Df, Matrix3x2D)
        [Fact]
        public void Vector2DTransform3x2Test()
        {
            Vector2D v = new Vector2D(1.0f, 2.0f);
            Matrix3x2D m = Matrix3x2D.CreateRotation(MathHelper.ToRadians(30.0f));
            m.M31 = 10.0f;
            m.M32 = 20.0f;

            Vector2D expected = new Vector2D(9.866025f, 22.23205f);
            Vector2D actual;

            actual = Vector2D.Transform(v, m);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Transform did not return the expected value.");
        }

        // A test for TransformNormal (Vector2Df, Matrix4x4D)
        [Fact]
        public void Vector2DTransformNormalTest()
        {
            Vector2D v = new Vector2D(1.0f, 2.0f);
            Matrix4x4D m =
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0f)) *
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0f)) *
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0f));
            m.M41 = 10.0f;
            m.M42 = 20.0f;
            m.M43 = 30.0f;

            Vector2D expected = new Vector2D(0.3169873f, 2.18301272f);
            Vector2D actual;

            actual = Vector2D.TransformNormal(v, m);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Transform did not return the expected value.");
        }

        // A test for TransformNormal (Vector2Df, Matrix3x2D)
        [Fact]
        public void Vector2DTransformNormal3x2Test()
        {
            Vector2D v = new Vector2D(1.0f, 2.0f);
            Matrix3x2D m = Matrix3x2D.CreateRotation(MathHelper.ToRadians(30.0f));
            m.M31 = 10.0f;
            m.M32 = 20.0f;

            Vector2D expected = new Vector2D(-0.133974612f, 2.232051f);
            Vector2D actual;

            actual = Vector2D.TransformNormal(v, m);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Transform did not return the expected value.");
        }

        // A test for Transform (Vector2Df, QuaternionD)
        [Fact]
        public void Vector2DTransformByQuaternionDTest()
        {
            Vector2D v = new Vector2D(1.0f, 2.0f);

            Matrix4x4D m =
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0f)) *
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0f)) *
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0f));
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
            Vector2D v = new Vector2D(1.0f, 2.0f);
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
            Vector2D v = new Vector2D(1.0f, 2.0f);
            QuaternionD q = QuaternionD.Identity;
            Vector2D expected = v;

            Vector2D actual = Vector2D.Transform(v, q);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Transform did not return the expected value.");
        }

        // A test for Normalize (Vector2Df)
        [Fact]
        public void Vector2DNormalizeTest()
        {
            Vector2D a = new Vector2D(2.0f, 3.0f);
            Vector2D expected = new Vector2D(0.554700196225229122018341733457f, 0.8320502943378436830275126001855f);
            Vector2D actual;

            actual = Vector2D.Normalize(a);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Normalize did not return the expected value.");
        }

        // A test for Normalize (Vector2Df)
        // Normalize zero length vector
        [Fact]
        public void Vector2DNormalizeTest1()
        {
            Vector2D a = new Vector2D(); // no parameter, default to 0.0f
            Vector2D actual = Vector2D.Normalize(a);
            Assert.True(float.IsNaN(actual.X) && float.IsNaN(actual.Y), "Vector2Df.Normalize did not return the expected value.");
        }

        // A test for Normalize (Vector2Df)
        // Normalize infinite length vector
        [Fact]
        public void Vector2DNormalizeTest2()
        {
            Vector2D a = new Vector2D(float.MaxValue, float.MaxValue);
            Vector2D actual = Vector2D.Normalize(a);
            Vector2D expected = new Vector2D(0, 0);
            Assert.Equal(expected, actual);
        }

        // A test for operator - (Vector2Df)
        [Fact]
        public void Vector2DUnaryNegationTest()
        {
            Vector2D a = new Vector2D(1.0f, 2.0f);

            Vector2D expected = new Vector2D(-1.0f, -2.0f);
            Vector2D actual;

            actual = -a;

            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.operator - did not return the expected value.");
        }



        // A test for operator - (Vector2Df)
        // Negate test with special float value
        [Fact]
        public void Vector2DUnaryNegationTest1()
        {
            Vector2D a = new Vector2D(float.PositiveInfinity, float.NegativeInfinity);

            Vector2D actual = -a;

            Assert.True(float.IsNegativeInfinity(actual.X), "Vector2Df.operator - did not return the expected value.");
            Assert.True(float.IsPositiveInfinity(actual.Y), "Vector2Df.operator - did not return the expected value.");
        }

        // A test for operator - (Vector2Df)
        // Negate test with special float value
        [Fact]
        public void Vector2DUnaryNegationTest2()
        {
            Vector2D a = new Vector2D(float.NaN, 0.0f);
            Vector2D actual = -a;

            Assert.True(float.IsNaN(actual.X), "Vector2Df.operator - did not return the expected value.");
            Assert.True(float.Equals(0.0f, actual.Y), "Vector2Df.operator - did not return the expected value.");
        }

        // A test for operator - (Vector2Df, Vector2Df)
        [Fact]
        public void Vector2DSubtractionTest()
        {
            Vector2D a = new Vector2D(1.0f, 3.0f);
            Vector2D b = new Vector2D(2.0f, 1.5f);

            Vector2D expected = new Vector2D(-1.0f, 1.5f);
            Vector2D actual;

            actual = a - b;

            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.operator - did not return the expected value.");
        }

        // A test for operator * (Vector2Df, float)
        [Fact]
        public void Vector2DMultiplyOperatorTest()
        {
            Vector2D a = new Vector2D(2.0f, 3.0f);
            const float factor = 2.0f;

            Vector2D expected = new Vector2D(4.0f, 6.0f);
            Vector2D actual;

            actual = a * factor;
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.operator * did not return the expected value.");
        }

        // A test for operator * (float, Vector2Df)
        [Fact]
        public void Vector2DMultiplyOperatorTest2()
        {
            Vector2D a = new Vector2D(2.0f, 3.0f);
            const float factor = 2.0f;

            Vector2D expected = new Vector2D(4.0f, 6.0f);
            Vector2D actual;

            actual = factor * a;
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.operator * did not return the expected value.");
        }

        // A test for operator * (Vector2Df, Vector2Df)
        [Fact]
        public void Vector2DMultiplyOperatorTest3()
        {
            Vector2D a = new Vector2D(2.0f, 3.0f);
            Vector2D b = new Vector2D(4.0f, 5.0f);

            Vector2D expected = new Vector2D(8.0f, 15.0f);
            Vector2D actual;

            actual = a * b;

            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.operator * did not return the expected value.");
        }

        // A test for operator / (Vector2Df, float)
        [Fact]
        public void Vector2DDivisionTest()
        {
            Vector2D a = new Vector2D(2.0f, 3.0f);

            float div = 2.0f;

            Vector2D expected = new Vector2D(1.0f, 1.5f);
            Vector2D actual;

            actual = a / div;

            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.operator / did not return the expected value.");
        }

        // A test for operator / (Vector2Df, Vector2Df)
        [Fact]
        public void Vector2DDivisionTest1()
        {
            Vector2D a = new Vector2D(2.0f, 3.0f);
            Vector2D b = new Vector2D(4.0f, 5.0f);

            Vector2D expected = new Vector2D(2.0f / 4.0f, 3.0f / 5.0f);
            Vector2D actual;

            actual = a / b;

            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.operator / did not return the expected value.");
        }

        // A test for operator / (Vector2Df, float)
        // Divide by zero
        [Fact]
        public void Vector2DDivisionTest2()
        {
            Vector2D a = new Vector2D(-2.0f, 3.0f);

            float div = 0.0f;

            Vector2D actual = a / div;

            Assert.True(float.IsNegativeInfinity(actual.X), "Vector2Df.operator / did not return the expected value.");
            Assert.True(float.IsPositiveInfinity(actual.Y), "Vector2Df.operator / did not return the expected value.");
        }

        // A test for operator / (Vector2Df, Vector2Df)
        // Divide by zero
        [Fact]
        public void Vector2DDivisionTest3()
        {
            Vector2D a = new Vector2D(0.047f, -3.0f);
            Vector2D b = new Vector2D();

            Vector2D actual = a / b;

            Assert.True(float.IsInfinity(actual.X), "Vector2Df.operator / did not return the expected value.");
            Assert.True(float.IsInfinity(actual.Y), "Vector2Df.operator / did not return the expected value.");
        }

        // A test for operator + (Vector2Df, Vector2Df)
        [Fact]
        public void Vector2DAdditionTest()
        {
            Vector2D a = new Vector2D(1.0f, 2.0f);
            Vector2D b = new Vector2D(3.0f, 4.0f);

            Vector2D expected = new Vector2D(4.0f, 6.0f);
            Vector2D actual;

            actual = a + b;

            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.operator + did not return the expected value.");
        }

        // A test for Vector2Df (float, float)
        [Fact]
        public void Vector2DConstructorTest()
        {
            float x = 1.0f;
            float y = 2.0f;

            Vector2D target = new Vector2D(x, y);
            Assert.True(MathHelper.Equal(target.X, x) && MathHelper.Equal(target.Y, y), "Vector2Df(x,y) constructor did not return the expected value.");
        }

        // A test for Vector2Df ()
        // Constructor with no parameter
        [Fact]
        public void Vector2DConstructorTest2()
        {
            Vector2D target = new Vector2D();
            Assert.Equal(0.0f, target.X);
            Assert.Equal(0.0f, target.Y);
        }

        // A test for Vector2Df (float, float)
        // Constructor with special floating values
        [Fact]
        public void Vector2DConstructorTest3()
        {
            Vector2D target = new Vector2D(float.NaN, float.MaxValue);
            Assert.Equal(float.NaN, target.X);
            Assert.Equal(float.MaxValue, target.Y);
        }

        // A test for Vector2Df (float)
        [Fact]
        public void Vector2DConstructorTest4()
        {
            float value = 1.0f;
            Vector2D target = new Vector2D(value);

            Vector2D expected = new Vector2D(value, value);
            Assert.Equal(expected, target);

            value = 2.0f;
            target = new Vector2D(value);
            expected = new Vector2D(value, value);
            Assert.Equal(expected, target);
        }

        // A test for Vector2Df (ReadOnlySpan<float>)
        [Fact]
        public void Vector2DConstructorTest5()
        {
            float value = 1.0f;
            Vector2D target = new Vector2D(new[] { value, value });
            Vector2D expected = new Vector2D(value);

            Assert.Equal(expected, target);
            Assert.Throws<ArgumentOutOfRangeException>(() => new Vector2D(new float[1]));
        }

        // A test for Add (Vector2Df, Vector2Df)
        [Fact]
        public void Vector2DAddTest()
        {
            Vector2D a = new Vector2D(1.0f, 2.0f);
            Vector2D b = new Vector2D(5.0f, 6.0f);

            Vector2D expected = new Vector2D(6.0f, 8.0f);
            Vector2D actual;

            actual = Vector2D.Add(a, b);
            Assert.Equal(expected, actual);
        }

        // A test for Divide (Vector2Df, float)
        [Fact]
        public void Vector2DDivideTest()
        {
            Vector2D a = new Vector2D(1.0f, 2.0f);
            float div = 2.0f;
            Vector2D expected = new Vector2D(0.5f, 1.0f);
            Vector2D actual;
            actual = Vector2D.Divide(a, div);
            Assert.Equal(expected, actual);
        }

        // A test for Divide (Vector2Df, Vector2Df)
        [Fact]
        public void Vector2DDivideTest1()
        {
            Vector2D a = new Vector2D(1.0f, 6.0f);
            Vector2D b = new Vector2D(5.0f, 2.0f);

            Vector2D expected = new Vector2D(1.0f / 5.0f, 6.0f / 2.0f);
            Vector2D actual;

            actual = Vector2D.Divide(a, b);
            Assert.Equal(expected, actual);
        }

        // A test for Equals (object)
        [Fact]
        public void Vector2DEqualsTest()
        {
            Vector2D a = new Vector2D(1.0f, 2.0f);
            Vector2D b = new Vector2D(1.0f, 2.0f);

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

        // A test for Multiply (Vector2Df, float)
        [Fact]
        public void Vector2DMultiplyTest()
        {
            Vector2D a = new Vector2D(1.0f, 2.0f);
            const float factor = 2.0f;
            Vector2D expected = new Vector2D(2.0f, 4.0f);
            Vector2D actual = Vector2D.Multiply(a, factor);
            Assert.Equal(expected, actual);
        }

        // A test for Multiply (float, Vector2Df)
        [Fact]
        public void Vector2DMultiplyTest2()
        {
            Vector2D a = new Vector2D(1.0f, 2.0f);
            const float factor = 2.0f;
            Vector2D expected = new Vector2D(2.0f, 4.0f);
            Vector2D actual = Vector2D.Multiply(factor, a);
            Assert.Equal(expected, actual);
        }

        // A test for Multiply (Vector2Df, Vector2Df)
        [Fact]
        public void Vector2DMultiplyTest3()
        {
            Vector2D a = new Vector2D(1.0f, 2.0f);
            Vector2D b = new Vector2D(5.0f, 6.0f);

            Vector2D expected = new Vector2D(5.0f, 12.0f);
            Vector2D actual;

            actual = Vector2D.Multiply(a, b);
            Assert.Equal(expected, actual);
        }

        // A test for Negate (Vector2Df)
        [Fact]
        public void Vector2DNegateTest()
        {
            Vector2D a = new Vector2D(1.0f, 2.0f);

            Vector2D expected = new Vector2D(-1.0f, -2.0f);
            Vector2D actual;

            actual = Vector2D.Negate(a);
            Assert.Equal(expected, actual);
        }

        // A test for operator != (Vector2Df, Vector2Df)
        [Fact]
        public void Vector2DInequalityTest()
        {
            Vector2D a = new Vector2D(1.0f, 2.0f);
            Vector2D b = new Vector2D(1.0f, 2.0f);

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

        // A test for operator == (Vector2Df, Vector2Df)
        [Fact]
        public void Vector2DEqualityTest()
        {
            Vector2D a = new Vector2D(1.0f, 2.0f);
            Vector2D b = new Vector2D(1.0f, 2.0f);

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

        // A test for Subtract (Vector2Df, Vector2Df)
        [Fact]
        public void Vector2DSubtractTest()
        {
            Vector2D a = new Vector2D(1.0f, 6.0f);
            Vector2D b = new Vector2D(5.0f, 2.0f);

            Vector2D expected = new Vector2D(-4.0f, 4.0f);
            Vector2D actual;

            actual = Vector2D.Subtract(a, b);
            Assert.Equal(expected, actual);
        }

        // A test for UnitX
        [Fact]
        public void Vector2DUnitXTest()
        {
            Vector2D val = new Vector2D(1.0f, 0.0f);
            Assert.Equal(val, Vector2D.UnitX);
        }

        // A test for UnitY
        [Fact]
        public void Vector2DUnitYTest()
        {
            Vector2D val = new Vector2D(0.0f, 1.0f);
            Assert.Equal(val, Vector2D.UnitY);
        }

        // A test for One
        [Fact]
        public void Vector2DOneTest()
        {
            Vector2D val = new Vector2D(1.0f, 1.0f);
            Assert.Equal(val, Vector2D.One);
        }

        // A test for Zero
        [Fact]
        public void Vector2DZeroTest()
        {
            Vector2D val = new Vector2D(0.0f, 0.0f);
            Assert.Equal(val, Vector2D.Zero);
        }

        // A test for Equals (Vector2Df)
        [Fact]
        public void Vector2DEqualsTest1()
        {
            Vector2D a = new Vector2D(1.0f, 2.0f);
            Vector2D b = new Vector2D(1.0f, 2.0f);

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

        // A test for Vector2Df comparison involving NaN values
        [Fact]
        public void Vector2DEqualsNaNTest()
        {
            Vector2D a = new Vector2D(float.NaN, 0);
            Vector2D b = new Vector2D(0, float.NaN);

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
            Vector2D a = Vector2D.Normalize(new Vector2D(1.0f, 1.0f));

            // Reflect on XZ plane.
            Vector2D n = new Vector2D(0.0f, 1.0f);
            Vector2D expected = new Vector2D(a.X, -a.Y);
            Vector2D actual = Vector2D.Reflect(a, n);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Reflect did not return the expected value.");

            // Reflect on XY plane.
            n = new Vector2D(0.0f, 0.0f);
            expected = new Vector2D(a.X, a.Y);
            actual = Vector2D.Reflect(a, n);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Reflect did not return the expected value.");

            // Reflect on YZ plane.
            n = new Vector2D(1.0f, 0.0f);
            expected = new Vector2D(-a.X, a.Y);
            actual = Vector2D.Reflect(a, n);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Reflect did not return the expected value.");
        }

        // A test for Reflect (Vector2Df, Vector2Df)
        // Reflection when normal and source are the same
        [Fact]
        public void Vector2DReflectTest1()
        {
            Vector2D n = new Vector2D(0.45f, 1.28f);
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
            Vector2D n = new Vector2D(0.45f, 1.28f);
            n = Vector2D.Normalize(n);
            Vector2D a = -n;

            Vector2D expected = n;
            Vector2D actual = Vector2D.Reflect(a, n);
            Assert.True(MathHelper.Equal(expected, actual), "Vector2Df.Reflect did not return the expected value.");
        }

        [Fact]
        public void Vector2DAbsTest()
        {
            Vector2D v1 = new Vector2D(-2.5f, 2.0f);
            Vector2D v3 = Vector2D.Abs(new Vector2D(0.0f, float.NegativeInfinity));
            Vector2D v = Vector2D.Abs(v1);
            Assert.Equal(2.5f, v.X);
            Assert.Equal(2.0f, v.Y);
            Assert.Equal(0.0f, v3.X);
            Assert.Equal(float.PositiveInfinity, v3.Y);
        }

        [Fact]
        public void Vector2DSqrtTest()
        {
            Vector2D v1 = new Vector2D(-2.5f, 2.0f);
            Vector2D v2 = new Vector2D(5.5f, 4.5f);
            Assert.Equal(2, (int)Vector2D.SquareRoot(v2).X);
            Assert.Equal(2, (int)Vector2D.SquareRoot(v2).Y);
            Assert.Equal(float.NaN, Vector2D.SquareRoot(v1).X);
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
            private float _f;
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
            Vector2D v3 = new Vector2D(4f, 5f);
            v3.X = 1.0f;
            v3.Y = 2.0f;
            Assert.Equal(1.0f, v3.X);
            Assert.Equal(2.0f, v3.Y);
            Vector2D v4 = v3;
            v4.Y = 0.5f;
            Assert.Equal(1.0f, v4.X);
            Assert.Equal(0.5f, v4.Y);
            Assert.Equal(2.0f, v3.Y);
        }

        [Fact]
        public void EmbeddedVectorSetFields()
        {
            EmbeddedVectorObject evo = new EmbeddedVectorObject();
            evo.FieldVector.X = 5.0f;
            evo.FieldVector.Y = 5.0f;
            Assert.Equal(5.0f, evo.FieldVector.X);
            Assert.Equal(5.0f, evo.FieldVector.Y);
        }

        private class EmbeddedVectorObject
        {
            public Vector2D FieldVector;
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.CosSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void CosSingleTest(float value, float expectedResult, float variance)
        {
            Vector2D actualResult = Vector2D.Cos(Vector2D.Create(value));
            AssertEqual(Vector2D.Create(expectedResult), actualResult, Vector2D.Create(variance));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.ExpSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void ExpSingleTest(float value, float expectedResult, float variance)
        {
            Vector2D actualResult = Vector2D.Exp(Vector2D.Create(value));
            AssertEqual(Vector2D.Create(expectedResult), actualResult, Vector2D.Create(variance));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.LogSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void LogSingleTest(float value, float expectedResult, float variance)
        {
            Vector2D actualResult = Vector2D.Log(Vector2D.Create(value));
            AssertEqual(Vector2D.Create(expectedResult), actualResult, Vector2D.Create(variance));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.Log2Single), MemberType = typeof(GenericMathTestMemberData))]
        public void Log2SingleTest(float value, float expectedResult, float variance)
        {
            Vector2D actualResult = Vector2D.Log2(Vector2D.Create(value));
            AssertEqual(Vector2D.Create(expectedResult), actualResult, Vector2D.Create(variance));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.FusedMultiplyAddSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void FusedMultiplyAddSingleTest(float left, float right, float addend, float expectedResult)
        {
            AssertEqual(Vector2D.Create(expectedResult), Vector2D.FusedMultiplyAdd(Vector2D.Create(left), Vector2D.Create(right), Vector2D.Create(addend)), Vector2D.Zero);
            AssertEqual(Vector2D.Create(float.MultiplyAddEstimate(left, right, addend)), Vector2D.MultiplyAddEstimate(Vector2D.Create(left), Vector2D.Create(right), Vector2D.Create(addend)), Vector2D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.ClampSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void ClampSingleTest(float x, float min, float max, float expectedResult)
        {
            Vector2D actualResult = Vector2D.Clamp(Vector2D.Create(x), Vector2D.Create(min), Vector2D.Create(max));
            AssertEqual(Vector2D.Create(expectedResult), actualResult, Vector2D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.CopySignSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void CopySignSingleTest(float x, float y, float expectedResult)
        {
            Vector2D actualResult = Vector2D.CopySign(Vector2D.Create(x), Vector2D.Create(y));
            AssertEqual(Vector2D.Create(expectedResult), actualResult, Vector2D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.DegreesToRadiansSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void DegreesToRadiansSingleTest(float value, float expectedResult, float variance)
        {
            AssertEqual(Vector2D.Create(-expectedResult), Vector2D.DegreesToRadians(Vector2D.Create(-value)), Vector2D.Create(variance));
            AssertEqual(Vector2D.Create(+expectedResult), Vector2D.DegreesToRadians(Vector2D.Create(+value)), Vector2D.Create(variance));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.HypotSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void HypotSingleTest(float x, float y, float expectedResult, float variance)
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
        [MemberData(nameof(GenericMathTestMemberData.LerpSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void LerpSingleTest(float x, float y, float amount, float expectedResult)
        {
            AssertEqual(Vector2D.Create(+expectedResult), Vector2D.Lerp(Vector2D.Create(+x), Vector2D.Create(+y), Vector2D.Create(amount)), Vector2D.Zero);
            AssertEqual(Vector2D.Create((expectedResult == 0.0f) ? expectedResult : -expectedResult), Vector2D.Lerp(Vector2D.Create(-x), Vector2D.Create(-y), Vector2D.Create(amount)), Vector2D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MaxSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void MaxSingleTest(float x, float y, float expectedResult)
        {
            Vector2D actualResult = Vector2D.Max(Vector2D.Create(x), Vector2D.Create(y));
            AssertEqual(Vector2D.Create(expectedResult), actualResult, Vector2D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MaxMagnitudeSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void MaxMagnitudeSingleTest(float x, float y, float expectedResult)
        {
            Vector2D actualResult = Vector2D.MaxMagnitude(Vector2D.Create(x), Vector2D.Create(y));
            AssertEqual(Vector2D.Create(expectedResult), actualResult, Vector2D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MaxMagnitudeNumberSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void MaxMagnitudeNumberSingleTest(float x, float y, float expectedResult)
        {
            Vector2D actualResult = Vector2D.MaxMagnitudeNumber(Vector2D.Create(x), Vector2D.Create(y));
            AssertEqual(Vector2D.Create(expectedResult), actualResult, Vector2D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MaxNumberSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void MaxNumberSingleTest(float x, float y, float expectedResult)
        {
            Vector2D actualResult = Vector2D.MaxNumber(Vector2D.Create(x), Vector2D.Create(y));
            AssertEqual(Vector2D.Create(expectedResult), actualResult, Vector2D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MinSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void MinSingleTest(float x, float y, float expectedResult)
        {
            Vector2D actualResult = Vector2D.Min(Vector2D.Create(x), Vector2D.Create(y));
            AssertEqual(Vector2D.Create(expectedResult), actualResult, Vector2D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MinMagnitudeSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void MinMagnitudeSingleTest(float x, float y, float expectedResult)
        {
            Vector2D actualResult = Vector2D.MinMagnitude(Vector2D.Create(x), Vector2D.Create(y));
            AssertEqual(Vector2D.Create(expectedResult), actualResult, Vector2D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MinMagnitudeNumberSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void MinMagnitudeNumberSingleTest(float x, float y, float expectedResult)
        {
            Vector2D actualResult = Vector2D.MinMagnitudeNumber(Vector2D.Create(x), Vector2D.Create(y));
            AssertEqual(Vector2D.Create(expectedResult), actualResult, Vector2D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.MinNumberSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void MinNumberSingleTest(float x, float y, float expectedResult)
        {
            Vector2D actualResult = Vector2D.MinNumber(Vector2D.Create(x), Vector2D.Create(y));
            AssertEqual(Vector2D.Create(expectedResult), actualResult, Vector2D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.RadiansToDegreesSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void RadiansToDegreesSingleTest(float value, float expectedResult, float variance)
        {
            AssertEqual(Vector2D.Create(-expectedResult), Vector2D.RadiansToDegrees(Vector2D.Create(-value)), Vector2D.Create(variance));
            AssertEqual(Vector2D.Create(+expectedResult), Vector2D.RadiansToDegrees(Vector2D.Create(+value)), Vector2D.Create(variance));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.RoundSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void RoundSingleTest(float value, float expectedResult)
        {
            Vector2D actualResult = Vector2D.Round(Vector2D.Create(value));
            AssertEqual(Vector2D.Create(expectedResult), actualResult, Vector2D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.RoundAwayFromZeroSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void RoundAwayFromZeroSingleTest(float value, float expectedResult)
        {
            Vector2D actualResult = Vector2D.Round(Vector2D.Create(value), MidpointRounding.AwayFromZero);
            AssertEqual(Vector2D.Create(expectedResult), actualResult, Vector2D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.RoundToEvenSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void RoundToEvenSingleTest(float value, float expectedResult)
        {
            Vector2D actualResult = Vector2D.Round(Vector2D.Create(value), MidpointRounding.ToEven);
            AssertEqual(Vector2D.Create(expectedResult), actualResult, Vector2D.Zero);
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.SinSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void SinSingleTest(float value, float expectedResult, float variance)
        {
            Vector2D actualResult = Vector2D.Sin(Vector2D.Create(value));
            AssertEqual(Vector2D.Create(expectedResult), actualResult, Vector2D.Create(variance));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.SinCosSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void SinCosSingleTest(float value, float expectedResultSin, float expectedResultCos, float allowedVarianceSin, float allowedVarianceCos)
        {
            (Vector2D resultSin, Vector2D resultCos) = Vector2D.SinCos(Vector2D.Create(value));
            AssertEqual(Vector2D.Create(expectedResultSin), resultSin, Vector2D.Create(allowedVarianceSin));
            AssertEqual(Vector2D.Create(expectedResultCos), resultCos, Vector2D.Create(allowedVarianceCos));
        }

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.TruncateSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void TruncateSingleTest(float value, float expectedResult)
        {
            Vector2D actualResult = Vector2D.Truncate(Vector2D.Create(value));
            AssertEqual(Vector2D.Create(expectedResult), actualResult, Vector2D.Zero);
        }

        [Fact]
        public void AllAnyNoneTest()
        {
            Test(3, 2);

            [MethodImpl(MethodImplOptions.NoInlining)]
            void Test(float value1, float value2)
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
            void Test(float value)
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
            void Test(float allBitsSet, float value2)
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
            void Test(float value1, float value2)
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
            void Test(float value)
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
            void Test(float allBitsSet, float value2)
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
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsEvenIntegerTest(float value) => Assert.Equal(float.IsEvenInteger(value) ? Vector2D.AllBitsSet : Vector2D.Zero, Vector2D.IsEvenInteger(Vector2D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsFiniteTest(float value) => Assert.Equal(float.IsFinite(value) ? Vector2D.AllBitsSet : Vector2D.Zero, Vector2D.IsFinite(Vector2D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsInfinityTest(float value) => Assert.Equal(float.IsInfinity(value) ? Vector2D.AllBitsSet : Vector2D.Zero, Vector2D.IsInfinity(Vector2D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsIntegerTest(float value) => Assert.Equal(float.IsInteger(value) ? Vector2D.AllBitsSet : Vector2D.Zero, Vector2D.IsInteger(Vector2D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsNaNTest(float value) => Assert.Equal(float.IsNaN(value) ? Vector2D.AllBitsSet : Vector2D.Zero, Vector2D.IsNaN(Vector2D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsNegativeTest(float value) => Assert.Equal(float.IsNegative(value) ? Vector2D.AllBitsSet : Vector2D.Zero, Vector2D.IsNegative(Vector2D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsNegativeInfinityTest(float value) => Assert.Equal(float.IsNegativeInfinity(value) ? Vector2D.AllBitsSet : Vector2D.Zero, Vector2D.IsNegativeInfinity(Vector2D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsNormalTest(float value) => Assert.Equal(float.IsNormal(value) ? Vector2D.AllBitsSet : Vector2D.Zero, Vector2D.IsNormal(Vector2D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsOddIntegerTest(float value) => Assert.Equal(float.IsOddInteger(value) ? Vector2D.AllBitsSet : Vector2D.Zero, Vector2D.IsOddInteger(Vector2D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsPositiveTest(float value) => Assert.Equal(float.IsPositive(value) ? Vector2D.AllBitsSet : Vector2D.Zero, Vector2D.IsPositive(Vector2D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsPositiveInfinityTest(float value) => Assert.Equal(float.IsPositiveInfinity(value) ? Vector2D.AllBitsSet : Vector2D.Zero, Vector2D.IsPositiveInfinity(Vector2D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsSubnormalTest(float value) => Assert.Equal(float.IsSubnormal(value) ? Vector2D.AllBitsSet : Vector2D.Zero, Vector2D.IsSubnormal(Vector2D.Create(value)));

        [Theory]
        [MemberData(nameof(GenericMathTestMemberData.IsTestSingle), MemberType = typeof(GenericMathTestMemberData))]
        public void IsZeroSingleTest(float value) => Assert.Equal((value == 0) ? Vector2D.AllBitsSet : Vector2D.Zero, Vector2D.IsZero(Vector2D.Create(value)));

        [Fact]
        public void AllBitsSetTest()
        {
            Assert.Equal(-1, BitConverter.SingleToInt32Bits(Vector2D.AllBitsSet.X));
            Assert.Equal(-1, BitConverter.SingleToInt32Bits(Vector2D.AllBitsSet.Y));
        }

        [Fact]
        public void ConditionalSelectTest()
        {
            Test(Vector2D.Create(1, 2), Vector2D.AllBitsSet, Vector2D.Create(1, 2), Vector2D.Create(5, 6));
            Test(Vector2D.Create(5, 6), Vector2D.Zero, Vector2D.Create(1, 2), Vector2D.Create(5, 6));
            Test(Vector2D.Create(1, 6), Vector128.Create(-1, 0, -1, 0).AsSingle().AsVector2D(), Vector2D.Create(1, 2), Vector2D.Create(5, 6));

            [MethodImpl(MethodImplOptions.NoInlining)]
            void Test(Vector2D expectedResult, Vector2D condition, Vector2D left, Vector2D right)
            {
                Assert.Equal(expectedResult, Vector2D.ConditionalSelect(condition, left, right));
            }
        }

        [Theory]
        [InlineData(+0.0f, +0.0f, 0b00)]
        [InlineData(-0.0f, +1.0f, 0b01)]
        [InlineData(-0.0f, -0.0f, 0b11)]
        public void ExtractMostSignificantBitsTest(float x, float y, uint expectedResult)
        {
            Assert.Equal(expectedResult, Vector2D.Create(x, y).ExtractMostSignificantBits());
        }

        [Theory]
        [InlineData(1.0f, 2.0f)]
        [InlineData(5.0f, 6.0f)]
        public void GetElementTest(float x, float y)
        {
            Assert.Equal(x, Vector2D.Create(x, y).GetElement(0));
            Assert.Equal(y, Vector2D.Create(x, y).GetElement(1));
        }

        [Theory]
        [InlineData(1.0f, 2.0f)]
        [InlineData(5.0f, 6.0f)]
        public void ShuffleTest(float x, float y)
        {
            Assert.Equal(Vector2D.Create(y, x), Vector2D.Shuffle(Vector2D.Create(x, y), 1, 0));
            Assert.Equal(Vector2D.Create(x, x), Vector2D.Shuffle(Vector2D.Create(x, y), 0, 0));
        }

        [Theory]
        [InlineData(1.0f, 2.0f, 3.0f)]
        [InlineData(5.0f, 6.0f, 11.0f)]
        public void SumTest(float x, float y, float expectedResult)
        {
            Assert.Equal(expectedResult, Vector2D.Sum(Vector2D.Create(x, y)));
        }

        [Theory]
        [InlineData(1.0f, 2.0f)]
        [InlineData(5.0f, 6.0f)]
        public void ToScalarTest(float x, float y)
        {
            Assert.Equal(x, Vector2D.Create(x, y).ToScalar());
        }

        [Theory]
        [InlineData(1.0f, 2.0f)]
        [InlineData(5.0f, 6.0f)]
        public void WithElementTest(float x, float y)
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
        [InlineData(1.0f, 2.0f)]
        [InlineData(5.0f, 6.0f)]
        public void AsVector3DTest(float x, float y)
        {
            var vector = Vector2D.Create(x, y).AsVector3D();

            Assert.Equal(x, vector.X);
            Assert.Equal(y, vector.Y);
            Assert.Equal(0, vector.Z);
        }

        [Theory]
        [InlineData(1.0f, 2.0f)]
        [InlineData(5.0f, 6.0f)]
        public void AsVector3DUnsafeTest(float x, float y)
        {
            var vector = Vector2D.Create(x, y).AsVector3DUnsafe();

            Assert.Equal(x, vector.X);
            Assert.Equal(y, vector.Y);
        }

        [Fact]
        public void CreateScalarTest()
        {
            var vector = Vector2D.CreateScalar(float.Pi);

            Assert.Equal(float.Pi, vector.X);
            Assert.Equal(0, vector.Y);

            vector = Vector2D.CreateScalar(float.E);

            Assert.Equal(float.E, vector.X);
            Assert.Equal(0, vector.Y);
        }

        [Fact]
        public void CreateScalarUnsafeTest()
        {
            var vector = Vector2D.CreateScalarUnsafe(float.Pi);
            Assert.Equal(float.Pi, vector.X);

            vector = Vector2D.CreateScalarUnsafe(float.E);
            Assert.Equal(float.E, vector.X);
        }
    }
}
