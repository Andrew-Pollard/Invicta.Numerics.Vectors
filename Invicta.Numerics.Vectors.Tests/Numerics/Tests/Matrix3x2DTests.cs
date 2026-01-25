// Copyright (c) Andrew Pollard, .NET Foundation and Contributors.
// Licensed under the MIT license - see README.md for details.

using System.Runtime.InteropServices;
using Xunit;

namespace Invicta.Numerics.Tests
{
    public sealed class Matrix3x2DDTests
    {
        static Matrix3x2D GenerateIncrementalMatrixNumber(float value = 0.0f)
        {
            Matrix3x2D a = new Matrix3x2D();
            a.M11 = value + 1.0f;
            a.M12 = value + 2.0f;
            a.M21 = value + 3.0f;
            a.M22 = value + 4.0f;
            a.M31 = value + 5.0f;
            a.M32 = value + 6.0f;
            return a;
        }

        static Matrix3x2D GenerateTestMatrix()
        {
            Matrix3x2D m = Matrix3x2D.CreateRotation(MathHelper.ToRadians(30.0f));
            m.Translation = new Vector2D(111.0f, 222.0f);
            return m;
        }

        [Theory]
        [InlineData(0.0f, 1.0f, 0.0f, 1.0f, 0.0f, 1.0f)]
        [InlineData(1.0f, 0.0f, 1.0f, 0.0f, 1.0f, 0.0f)]
        [InlineData(3.1434343f, 1.1234123f, 0.1234123f, -0.1234123f, 3.1434343f, 1.1234123f)]
        [InlineData(1.0000001f, 0.0000001f, 2.0000001f, 0.0000002f, 1.0000001f, 0.0000001f)]
        public void Matrix3x2DIndexerGetTest(float m11, float m12, float m21, float m22, float m31, float m32)
        {
            var matrix = new Matrix3x2D(m11, m12, m21, m22, m31, m32);

            Assert.Equal(m11, matrix[0, 0]);
            Assert.Equal(m12, matrix[0, 1]);
            Assert.Equal(m21, matrix[1, 0]);
            Assert.Equal(m22, matrix[1, 1]);
            Assert.Equal(m31, matrix[2, 0]);
            Assert.Equal(m32, matrix[2, 1]);
        }

        [Theory]
        [InlineData(0.0f, 1.0f, 0.0f, 1.0f, 0.0f, 1.0f)]
        [InlineData(1.0f, 0.0f, 1.0f, 0.0f, 1.0f, 0.0f)]
        [InlineData(3.1434343f, 1.1234123f, 0.1234123f, -0.1234123f, 3.1434343f, 1.1234123f)]
        [InlineData(1.0000001f, 0.0000001f, 2.0000001f, 0.0000002f, 1.0000001f, 0.0000001f)]
        public void Matrix3x2DIndexerSetTest(float m11, float m12, float m21, float m22, float m31, float m32)
        {
            var matrix = new Matrix3x2D(0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f);

            matrix[0, 0] = m11;
            matrix[0, 1] = m12;
            matrix[1, 0] = m21;
            matrix[1, 1] = m22;
            matrix[2, 0] = m31;
            matrix[2, 1] = m32;

            Assert.Equal(m11, matrix[0, 0]);
            Assert.Equal(m12, matrix[0, 1]);
            Assert.Equal(m21, matrix[1, 0]);
            Assert.Equal(m22, matrix[1, 1]);
            Assert.Equal(m31, matrix[2, 0]);
            Assert.Equal(m32, matrix[2, 1]);
        }

        // A test for Identity
        [Fact]
        public void Matrix3x2DIdentityTest()
        {
            Matrix3x2D val = new Matrix3x2D();
            val.M11 = val.M22 = 1.0f;

            Assert.True(MathHelper.Equal(val, Matrix3x2D.Identity), "Matrix3x2D.Indentity was not set correctly.");
        }

        // A test for Determinant
        [Fact]
        public void Matrix3x2DDeterminantTest()
        {
            Matrix3x2D target = Matrix3x2D.CreateRotation(MathHelper.ToRadians(30.0f));

            float val = 1.0f;
            float det = target.GetDeterminant();

            Assert.True(MathHelper.Equal(val, det), "Matrix3x2D.Determinant was not set correctly.");
        }

        // A test for Determinant
        // Determinant test |A| = 1 / |A'|
        [Fact]
        public void Matrix3x2DDeterminantTest1()
        {
            Matrix3x2D a = new Matrix3x2D();
            a.M11 = 5.0f;
            a.M12 = 2.0f;
            a.M21 = 12.0f;
            a.M22 = 6.8f;
            a.M31 = 6.5f;
            a.M32 = 1.0f;
            Matrix3x2D i;
            Assert.True(Matrix3x2D.Invert(a, out i));

            float detA = a.GetDeterminant();
            float detI = i.GetDeterminant();
            float t = 1.0f / detI;

            // only accurate to 3 precision
            Assert.True(System.Math.Abs(detA - t) < 1e-3, "Matrix3x2D.Determinant was not set correctly.");

            // sanity check against 4x4 version
            Assert.Equal(new Matrix4x4D(a).GetDeterminant(), detA);
            Assert.Equal(new Matrix4x4D(i).GetDeterminant(), detI);
        }

        // A test for Invert (Matrix3x2D)
        [Fact]
        public void Matrix3x2DInvertTest()
        {
            Matrix3x2D mtx = Matrix3x2D.CreateRotation(MathHelper.ToRadians(30.0f));

            Matrix3x2D expected = new Matrix3x2D();
            expected.M11 = 0.8660254f;
            expected.M12 = -0.5f;

            expected.M21 = 0.5f;
            expected.M22 = 0.8660254f;

            expected.M31 = 0;
            expected.M32 = 0;

            Matrix3x2D actual;

            Assert.True(Matrix3x2D.Invert(mtx, out actual));
            Assert.True(MathHelper.Equal(expected, actual), "Matrix3x2D.Invert did not return the expected value.");

            Matrix3x2D i = mtx * actual;
            Assert.True(MathHelper.Equal(i, Matrix3x2D.Identity), "Matrix3x2D.Invert did not return the expected value.");
        }

        // A test for Invert (Matrix3x2D)
        [Fact]
        public void Matrix3x2DInvertIdentityTest()
        {
            Matrix3x2D mtx = Matrix3x2D.Identity;

            Matrix3x2D actual;
            Assert.True(Matrix3x2D.Invert(mtx, out actual));

            Assert.True(MathHelper.Equal(actual, Matrix3x2D.Identity));
        }

        // A test for Invert (Matrix3x2D)
        [Fact]
        public void Matrix3x2DInvertTranslationTest()
        {
            Matrix3x2D mtx = Matrix3x2D.CreateTranslation(23, 42);

            Matrix3x2D actual;
            Assert.True(Matrix3x2D.Invert(mtx, out actual));

            Matrix3x2D i = mtx * actual;
            Assert.True(MathHelper.Equal(i, Matrix3x2D.Identity));
        }

        // A test for Invert (Matrix3x2D)
        [Fact]
        public void Matrix3x2DInvertRotationTest()
        {
            Matrix3x2D mtx = Matrix3x2D.CreateRotation(2);

            Matrix3x2D actual;
            Assert.True(Matrix3x2D.Invert(mtx, out actual));

            Matrix3x2D i = mtx * actual;
            Assert.True(MathHelper.Equal(i, Matrix3x2D.Identity));
        }

        // A test for Invert (Matrix3x2D)
        [Fact]
        public void Matrix3x2DInvertScaleTest()
        {
            Matrix3x2D mtx = Matrix3x2D.CreateScale(23, -42);

            Matrix3x2D actual;
            Assert.True(Matrix3x2D.Invert(mtx, out actual));

            Matrix3x2D i = mtx * actual;
            Assert.True(MathHelper.Equal(i, Matrix3x2D.Identity));
        }

        // A test for Invert (Matrix3x2D)
        [Fact]
        public void Matrix3x2DInvertAffineTest()
        {
            Matrix3x2D mtx = Matrix3x2D.CreateRotation(2) *
                            Matrix3x2D.CreateScale(23, -42) *
                            Matrix3x2D.CreateTranslation(17, 53);

            Matrix3x2D actual;
            Assert.True(Matrix3x2D.Invert(mtx, out actual));

            Matrix3x2D i = mtx * actual;
            Assert.True(MathHelper.Equal(i, Matrix3x2D.Identity));
        }



        // A test for CreateRotation (float)
        [Fact]
        public void Matrix3x2DCreateRotationTest()
        {
            float radians = MathHelper.ToRadians(50.0f);

            Matrix3x2D expected = new Matrix3x2D();
            expected.M11 = 0.642787635f;
            expected.M12 = 0.766044438f;
            expected.M21 = -0.766044438f;
            expected.M22 = 0.642787635f;

            Matrix3x2D actual;
            actual = Matrix3x2D.CreateRotation(radians);
            Assert.True(MathHelper.Equal(expected, actual), "Matrix3x2D.CreateRotation did not return the expected value.");
        }

        // A test for CreateRotation (float, Vector2D)
        [Fact]
        public void Matrix3x2DCreateRotationCenterTest()
        {
            float radians = MathHelper.ToRadians(30.0f);
            Vector2D center = new Vector2D(23, 42);

            Matrix3x2D rotateAroundZero = Matrix3x2D.CreateRotation(radians, Vector2D.Zero);
            Matrix3x2D rotateAroundZeroExpected = Matrix3x2D.CreateRotation(radians);
            Assert.True(MathHelper.Equal(rotateAroundZero, rotateAroundZeroExpected));

            Matrix3x2D rotateAroundCenter = Matrix3x2D.CreateRotation(radians, center);
            Matrix3x2D rotateAroundCenterExpected = Matrix3x2D.CreateTranslation(-center) * Matrix3x2D.CreateRotation(radians) * Matrix3x2D.CreateTranslation(center);
            Assert.True(MathHelper.Equal(rotateAroundCenter, rotateAroundCenterExpected));
        }

        // A test for CreateRotation (float)
        [Fact]
        public void Matrix3x2DCreateRotationRightAngleTest()
        {
            // 90 degree rotations must be exact!
            Matrix3x2D actual = Matrix3x2D.CreateRotation(0);
            Assert.Equal(new Matrix3x2D(1, 0, 0, 1, 0, 0), actual);

            actual = Matrix3x2D.CreateRotation(MathHelper.Pi / 2);
            Assert.Equal(new Matrix3x2D(0, 1, -1, 0, 0, 0), actual);

            actual = Matrix3x2D.CreateRotation(MathHelper.Pi);
            Assert.Equal(new Matrix3x2D(-1, 0, 0, -1, 0, 0), actual);

            actual = Matrix3x2D.CreateRotation(MathHelper.Pi * 3 / 2);
            Assert.Equal(new Matrix3x2D(0, -1, 1, 0, 0, 0), actual);

            actual = Matrix3x2D.CreateRotation(MathHelper.Pi * 2);
            Assert.Equal(new Matrix3x2D(1, 0, 0, 1, 0, 0), actual);

            actual = Matrix3x2D.CreateRotation(MathHelper.Pi * 5 / 2);
            Assert.Equal(new Matrix3x2D(0, 1, -1, 0, 0, 0), actual);

            actual = Matrix3x2D.CreateRotation(-MathHelper.Pi / 2);
            Assert.Equal(new Matrix3x2D(0, -1, 1, 0, 0, 0), actual);

            // But merely close-to-90 rotations should not be excessively clamped.
            float delta = MathHelper.ToRadians(0.01f);

            actual = Matrix3x2D.CreateRotation(MathHelper.Pi + delta);
            Assert.False(MathHelper.Equal(new Matrix3x2D(-1, 0, 0, -1, 0, 0), actual));

            actual = Matrix3x2D.CreateRotation(MathHelper.Pi - delta);
            Assert.False(MathHelper.Equal(new Matrix3x2D(-1, 0, 0, -1, 0, 0), actual));
        }

        // A test for CreateRotation (float, Vector2D)
        [Fact]
        public void Matrix3x2DCreateRotationRightAngleCenterTest()
        {
            Vector2D center = new Vector2D(3, 7);

            // 90 degree rotations must be exact!
            Matrix3x2D actual = Matrix3x2D.CreateRotation(0, center);
            Assert.Equal(new Matrix3x2D(1, 0, 0, 1, 0, 0), actual);

            actual = Matrix3x2D.CreateRotation(MathHelper.Pi / 2, center);
            Assert.Equal(new Matrix3x2D(0, 1, -1, 0, 10, 4), actual);

            actual = Matrix3x2D.CreateRotation(MathHelper.Pi, center);
            Assert.Equal(new Matrix3x2D(-1, 0, 0, -1, 6, 14), actual);

            actual = Matrix3x2D.CreateRotation(MathHelper.Pi * 3 / 2, center);
            Assert.Equal(new Matrix3x2D(0, -1, 1, 0, -4, 10), actual);

            actual = Matrix3x2D.CreateRotation(MathHelper.Pi * 2, center);
            Assert.Equal(new Matrix3x2D(1, 0, 0, 1, 0, 0), actual);

            actual = Matrix3x2D.CreateRotation(MathHelper.Pi * 5 / 2, center);
            Assert.Equal(new Matrix3x2D(0, 1, -1, 0, 10, 4), actual);

            actual = Matrix3x2D.CreateRotation(-MathHelper.Pi / 2, center);
            Assert.Equal(new Matrix3x2D(0, -1, 1, 0, -4, 10), actual);

            // But merely close-to-90 rotations should not be excessively clamped.
            float delta = MathHelper.ToRadians(0.01f);

            actual = Matrix3x2D.CreateRotation(MathHelper.Pi + delta, center);
            Assert.False(MathHelper.Equal(new Matrix3x2D(-1, 0, 0, -1, 6, 14), actual));

            actual = Matrix3x2D.CreateRotation(MathHelper.Pi - delta, center);
            Assert.False(MathHelper.Equal(new Matrix3x2D(-1, 0, 0, -1, 6, 14), actual));
        }

        // A test for Invert (Matrix3x2D)
        // Non invertible matrix - determinant is zero - singular matrix
        [Fact]
        public void Matrix3x2DInvertTest1()
        {
            Matrix3x2D a = new Matrix3x2D();
            a.M11 = 0.0f;
            a.M12 = 2.0f;
            a.M21 = 0.0f;
            a.M22 = 4.0f;
            a.M31 = 5.0f;
            a.M32 = 6.0f;

            float detA = a.GetDeterminant();
            Assert.True(MathHelper.Equal(detA, 0.0f), "Matrix3x2D.Invert did not return the expected value.");

            Matrix3x2D actual;
            Assert.False(Matrix3x2D.Invert(a, out actual));

            // all the elements in Actual is NaN
            Assert.True(
                float.IsNaN(actual.M11) && float.IsNaN(actual.M12) &&
                float.IsNaN(actual.M21) && float.IsNaN(actual.M22) &&
                float.IsNaN(actual.M31) && float.IsNaN(actual.M32)
                , "Matrix3x2D.Invert did not return the expected value.");
        }

        // A test for Lerp (Matrix3x2D, Matrix3x2D, float)
        [Fact]
        public void Matrix3x2DLerpTest()
        {
            Matrix3x2D a = new Matrix3x2D();
            a.M11 = 11.0f;
            a.M12 = 12.0f;
            a.M21 = 21.0f;
            a.M22 = 22.0f;
            a.M31 = 31.0f;
            a.M32 = 32.0f;

            Matrix3x2D b = GenerateIncrementalMatrixNumber();

            float t = 0.5f;

            Matrix3x2D expected = new Matrix3x2D();
            expected.M11 = a.M11 + (b.M11 - a.M11) * t;
            expected.M12 = a.M12 + (b.M12 - a.M12) * t;

            expected.M21 = a.M21 + (b.M21 - a.M21) * t;
            expected.M22 = a.M22 + (b.M22 - a.M22) * t;

            expected.M31 = a.M31 + (b.M31 - a.M31) * t;
            expected.M32 = a.M32 + (b.M32 - a.M32) * t;

            Matrix3x2D actual;
            actual = Matrix3x2D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Matrix3x2D.Lerp did not return the expected value.");
        }

        // A test for operator - (Matrix3x2D)
        [Fact]
        public void Matrix3x2DUnaryNegationTest()
        {
            Matrix3x2D a = GenerateIncrementalMatrixNumber();

            Matrix3x2D expected = new Matrix3x2D();
            expected.M11 = -1.0f;
            expected.M12 = -2.0f;
            expected.M21 = -3.0f;
            expected.M22 = -4.0f;
            expected.M31 = -5.0f;
            expected.M32 = -6.0f;

            Matrix3x2D actual = -a;
            Assert.True(MathHelper.Equal(expected, actual), "Matrix3x2D.operator - did not return the expected value.");
        }

        // A test for operator - (Matrix3x2D, Matrix3x2D)
        [Fact]
        public void Matrix3x2DSubtractionTest()
        {
            Matrix3x2D a = GenerateIncrementalMatrixNumber();
            Matrix3x2D b = GenerateIncrementalMatrixNumber(-3.0f);
            Matrix3x2D expected = new Matrix3x2D();
            expected.M11 = a.M11 - b.M11;
            expected.M12 = a.M12 - b.M12;
            expected.M21 = a.M21 - b.M21;
            expected.M22 = a.M22 - b.M22;
            expected.M31 = a.M31 - b.M31;
            expected.M32 = a.M32 - b.M32;

            Matrix3x2D actual = a - b;
            Assert.True(MathHelper.Equal(expected, actual), "Matrix3x2D.operator - did not return the expected value.");
        }

        // A test for operator * (Matrix3x2D, Matrix3x2D)
        [Fact]
        public void Matrix3x2DMultiplyTest1()
        {
            Matrix3x2D a = GenerateIncrementalMatrixNumber();
            Matrix3x2D b = GenerateIncrementalMatrixNumber(-3.0f);

            Matrix3x2D expected = new Matrix3x2D();
            expected.M11 = a.M11 * b.M11 + a.M12 * b.M21;
            expected.M12 = a.M11 * b.M12 + a.M12 * b.M22;

            expected.M21 = a.M21 * b.M11 + a.M22 * b.M21;
            expected.M22 = a.M21 * b.M12 + a.M22 * b.M22;

            expected.M31 = a.M31 * b.M11 + a.M32 * b.M21 + b.M31;
            expected.M32 = a.M31 * b.M12 + a.M32 * b.M22 + b.M32;

            Matrix3x2D actual = a * b;
            Assert.True(MathHelper.Equal(expected, actual), "Matrix3x2D.operator * did not return the expected value.");

            // Sanity check by comparison with 4x4 multiply.
            a = Matrix3x2D.CreateRotation(MathHelper.ToRadians(30)) * Matrix3x2D.CreateTranslation(23, 42);
            b = Matrix3x2D.CreateScale(3, 7) * Matrix3x2D.CreateTranslation(666, -1);

            actual = a * b;

            Matrix4x4D a44 = new Matrix4x4D(a);
            Matrix4x4D b44 = new Matrix4x4D(b);
            Matrix4x4D expected44 = a44 * b44;
            Matrix4x4D actual44 = new Matrix4x4D(actual);

            Assert.True(MathHelper.Equal(expected44, actual44), "Matrix3x2D.operator * did not return the expected value.");
        }

        // A test for operator * (Matrix3x2D, Matrix3x2D)
        // Multiply with identity matrix
        [Fact]
        public void Matrix3x2DMultiplyTest4()
        {
            Matrix3x2D a = new Matrix3x2D();
            a.M11 = 1.0f;
            a.M12 = 2.0f;
            a.M21 = 5.0f;
            a.M22 = -6.0f;
            a.M31 = 9.0f;
            a.M32 = 10.0f;

            Matrix3x2D b = new Matrix3x2D();
            b = Matrix3x2D.Identity;

            Matrix3x2D expected = a;
            Matrix3x2D actual = a * b;

            Assert.True(MathHelper.Equal(expected, actual), "Matrix3x2D.operator * did not return the expected value.");
        }

        // A test for operator + (Matrix3x2D, Matrix3x2D)
        [Fact]
        public void Matrix3x2DAdditionTest()
        {
            Matrix3x2D a = GenerateIncrementalMatrixNumber();
            Matrix3x2D b = GenerateIncrementalMatrixNumber(-3.0f);

            Matrix3x2D expected = new Matrix3x2D();
            expected.M11 = a.M11 + b.M11;
            expected.M12 = a.M12 + b.M12;
            expected.M21 = a.M21 + b.M21;
            expected.M22 = a.M22 + b.M22;
            expected.M31 = a.M31 + b.M31;
            expected.M32 = a.M32 + b.M32;

            Matrix3x2D actual;

            actual = a + b;

            Assert.True(MathHelper.Equal(expected, actual), "Matrix3x2D.operator + did not return the expected value.");
        }

        // A test for ToString ()
        [Fact]
        public void Matrix3x2DToStringTest()
        {
            Matrix3x2D a = new Matrix3x2D();
            a.M11 = 11.0f;
            a.M12 = -12.0f;
            a.M21 = 21.0f;
            a.M22 = 22.0f;
            a.M31 = 31.0f;
            a.M32 = 32.0f;

            string expected = "{ {M11:11 M12:-12} " +
                                "{M21:21 M22:22} " +
                                "{M31:31 M32:32} }";
            string actual;

            actual = a.ToString();
            Assert.Equal(expected, actual);
        }

        // A test for Add (Matrix3x2D, Matrix3x2D)
        [Fact]
        public void Matrix3x2DAddTest()
        {
            Matrix3x2D a = GenerateIncrementalMatrixNumber();
            Matrix3x2D b = GenerateIncrementalMatrixNumber(-3.0f);

            Matrix3x2D expected = new Matrix3x2D();
            expected.M11 = a.M11 + b.M11;
            expected.M12 = a.M12 + b.M12;
            expected.M21 = a.M21 + b.M21;
            expected.M22 = a.M22 + b.M22;
            expected.M31 = a.M31 + b.M31;
            expected.M32 = a.M32 + b.M32;

            Matrix3x2D actual;

            actual = Matrix3x2D.Add(a, b);
            Assert.Equal(expected, actual);
        }

        // A test for Equals (object)
        [Fact]
        public void Matrix3x2DEqualsTest()
        {
            Matrix3x2D a = GenerateIncrementalMatrixNumber();
            Matrix3x2D b = GenerateIncrementalMatrixNumber();

            // case 1: compare between same values
            object obj = b;

            bool expected = true;
            bool actual = a.Equals(obj);
            Assert.Equal(expected, actual);

            // case 2: compare between different values
            b.M11 = 11.0f;
            obj = b;
            expected = false;
            actual = a.Equals(obj);
            Assert.Equal(expected, actual);

            // case 3: compare between different types.
            obj = new Vector4D();
            expected = false;
            actual = a.Equals(obj);
            Assert.Equal(expected, actual);

            // case 3: compare against null.
            obj = null;
            expected = false;
            actual = a.Equals(obj);
            Assert.Equal(expected, actual);
        }

        // A test for GetHashCode ()
        [Fact]
        public void Matrix3x2DGetHashCodeTest()
        {
            Matrix3x2D target = GenerateIncrementalMatrixNumber();

            int expected = HashCode.Combine(
                new Vector2D(target.M11, target.M12),
                new Vector2D(target.M21, target.M22),
                new Vector2D(target.M31, target.M32)
            );

            int actual = target.GetHashCode();

            Assert.Equal(expected, actual);
        }

        // A test for Multiply (Matrix3x2D, Matrix3x2D)
        [Fact]
        public void Matrix3x2DMultiplyTest3()
        {
            Matrix3x2D a = GenerateIncrementalMatrixNumber();
            Matrix3x2D b = GenerateIncrementalMatrixNumber(-3.0f);

            Matrix3x2D expected = new Matrix3x2D();
            expected.M11 = a.M11 * b.M11 + a.M12 * b.M21;
            expected.M12 = a.M11 * b.M12 + a.M12 * b.M22;

            expected.M21 = a.M21 * b.M11 + a.M22 * b.M21;
            expected.M22 = a.M21 * b.M12 + a.M22 * b.M22;

            expected.M31 = a.M31 * b.M11 + a.M32 * b.M21 + b.M31;
            expected.M32 = a.M31 * b.M12 + a.M32 * b.M22 + b.M32;
            Matrix3x2D actual;
            actual = Matrix3x2D.Multiply(a, b);

            Assert.Equal(expected, actual);

            // Sanity check by comparison with 4x4 multiply.
            a = Matrix3x2D.CreateRotation(MathHelper.ToRadians(30)) * Matrix3x2D.CreateTranslation(23, 42);
            b = Matrix3x2D.CreateScale(3, 7) * Matrix3x2D.CreateTranslation(666, -1);

            actual = Matrix3x2D.Multiply(a, b);

            Matrix4x4D a44 = new Matrix4x4D(a);
            Matrix4x4D b44 = new Matrix4x4D(b);
            Matrix4x4D expected44 = Matrix4x4D.Multiply(a44, b44);
            Matrix4x4D actual44 = new Matrix4x4D(actual);

            Assert.True(MathHelper.Equal(expected44, actual44), "Matrix3x2D.Multiply did not return the expected value.");
        }

        // A test for Multiply (Matrix3x2D, float)
        [Fact]
        public void Matrix3x2DMultiplyTest5()
        {
            Matrix3x2D a = GenerateIncrementalMatrixNumber();
            Matrix3x2D expected = new Matrix3x2D(3, 6, 9, 12, 15, 18);
            Matrix3x2D actual = Matrix3x2D.Multiply(a, 3);

            Assert.Equal(expected, actual);
        }

        // A test for Multiply (Matrix3x2D, float)
        [Fact]
        public void Matrix3x2DMultiplyTest6()
        {
            Matrix3x2D a = GenerateIncrementalMatrixNumber();
            Matrix3x2D expected = new Matrix3x2D(3, 6, 9, 12, 15, 18);
            Matrix3x2D actual = a * 3;

            Assert.Equal(expected, actual);
        }

        // A test for Negate (Matrix3x2D)
        [Fact]
        public void Matrix3x2DNegateTest()
        {
            Matrix3x2D m = GenerateIncrementalMatrixNumber();

            Matrix3x2D expected = new Matrix3x2D();
            expected.M11 = -1.0f;
            expected.M12 = -2.0f;
            expected.M21 = -3.0f;
            expected.M22 = -4.0f;
            expected.M31 = -5.0f;
            expected.M32 = -6.0f;
            Matrix3x2D actual;

            actual = Matrix3x2D.Negate(m);
            Assert.Equal(expected, actual);
        }

        // A test for operator != (Matrix3x2D, Matrix3x2D)
        [Fact]
        public void Matrix3x2DInequalityTest()
        {
            Matrix3x2D a = GenerateIncrementalMatrixNumber();
            Matrix3x2D b = GenerateIncrementalMatrixNumber();

            // case 1: compare between same values
            bool expected = false;
            bool actual = a != b;
            Assert.Equal(expected, actual);

            // case 2: compare between different values
            b.M11 = 11.0f;
            expected = true;
            actual = a != b;
            Assert.Equal(expected, actual);
        }

        // A test for operator == (Matrix3x2D, Matrix3x2D)
        [Fact]
        public void Matrix3x2DEqualityTest()
        {
            Matrix3x2D a = GenerateIncrementalMatrixNumber();
            Matrix3x2D b = GenerateIncrementalMatrixNumber();

            // case 1: compare between same values
            bool expected = true;
            bool actual = a == b;
            Assert.Equal(expected, actual);

            // case 2: compare between different values
            b.M11 = 11.0f;
            expected = false;
            actual = a == b;
            Assert.Equal(expected, actual);
        }

        // A test for Subtract (Matrix3x2D, Matrix3x2D)
        [Fact]
        public void Matrix3x2DSubtractTest()
        {
            Matrix3x2D a = GenerateIncrementalMatrixNumber();
            Matrix3x2D b = GenerateIncrementalMatrixNumber(-3.0f);
            Matrix3x2D expected = new Matrix3x2D();
            expected.M11 = a.M11 - b.M11;
            expected.M12 = a.M12 - b.M12;
            expected.M21 = a.M21 - b.M21;
            expected.M22 = a.M22 - b.M22;
            expected.M31 = a.M31 - b.M31;
            expected.M32 = a.M32 - b.M32;

            Matrix3x2D actual;
            actual = Matrix3x2D.Subtract(a, b);
            Assert.Equal(expected, actual);
        }

        // A test for CreateScale (Vector2D)
        [Fact]
        public void Matrix3x2DCreateScaleTest1()
        {
            Vector2D scales = new Vector2D(2.0f, 3.0f);
            Matrix3x2D expected = new Matrix3x2D(
                2.0f, 0.0f,
                0.0f, 3.0f,
                0.0f, 0.0f);
            Matrix3x2D actual = Matrix3x2D.CreateScale(scales);
            Assert.Equal(expected, actual);
        }

        // A test for CreateScale (Vector2D, Vector2D)
        [Fact]
        public void Matrix3x2DCreateScaleCenterTest1()
        {
            Vector2D scale = new Vector2D(3, 4);
            Vector2D center = new Vector2D(23, 42);

            Matrix3x2D scaleAroundZero = Matrix3x2D.CreateScale(scale, Vector2D.Zero);
            Matrix3x2D scaleAroundZeroExpected = Matrix3x2D.CreateScale(scale);
            Assert.True(MathHelper.Equal(scaleAroundZero, scaleAroundZeroExpected));

            Matrix3x2D scaleAroundCenter = Matrix3x2D.CreateScale(scale, center);
            Matrix3x2D scaleAroundCenterExpected = Matrix3x2D.CreateTranslation(-center) * Matrix3x2D.CreateScale(scale) * Matrix3x2D.CreateTranslation(center);
            Assert.True(MathHelper.Equal(scaleAroundCenter, scaleAroundCenterExpected));
        }

        // A test for CreateScale (float)
        [Fact]
        public void Matrix3x2DCreateScaleTest2()
        {
            float scale = 2.0f;
            Matrix3x2D expected = new Matrix3x2D(
                2.0f, 0.0f,
                0.0f, 2.0f,
                0.0f, 0.0f);
            Matrix3x2D actual = Matrix3x2D.CreateScale(scale);
            Assert.Equal(expected, actual);
        }

        // A test for CreateScale (float, Vector2D)
        [Fact]
        public void Matrix3x2DCreateScaleCenterTest2()
        {
            float scale = 5;
            Vector2D center = new Vector2D(23, 42);

            Matrix3x2D scaleAroundZero = Matrix3x2D.CreateScale(scale, Vector2D.Zero);
            Matrix3x2D scaleAroundZeroExpected = Matrix3x2D.CreateScale(scale);
            Assert.True(MathHelper.Equal(scaleAroundZero, scaleAroundZeroExpected));

            Matrix3x2D scaleAroundCenter = Matrix3x2D.CreateScale(scale, center);
            Matrix3x2D scaleAroundCenterExpected = Matrix3x2D.CreateTranslation(-center) * Matrix3x2D.CreateScale(scale) * Matrix3x2D.CreateTranslation(center);
            Assert.True(MathHelper.Equal(scaleAroundCenter, scaleAroundCenterExpected));
        }

        // A test for CreateScale (float, float)
        [Fact]
        public void Matrix3x2DCreateScaleTest3()
        {
            float xScale = 2.0f;
            float yScale = 3.0f;
            Matrix3x2D expected = new Matrix3x2D(
                2.0f, 0.0f,
                0.0f, 3.0f,
                0.0f, 0.0f);
            Matrix3x2D actual = Matrix3x2D.CreateScale(xScale, yScale);
            Assert.Equal(expected, actual);
        }

        // A test for CreateScale (float, float, Vector2D)
        [Fact]
        public void Matrix3x2DCreateScaleCenterTest3()
        {
            Vector2D scale = new Vector2D(3, 4);
            Vector2D center = new Vector2D(23, 42);

            Matrix3x2D scaleAroundZero = Matrix3x2D.CreateScale(scale.X, scale.Y, Vector2D.Zero);
            Matrix3x2D scaleAroundZeroExpected = Matrix3x2D.CreateScale(scale.X, scale.Y);
            Assert.True(MathHelper.Equal(scaleAroundZero, scaleAroundZeroExpected));

            Matrix3x2D scaleAroundCenter = Matrix3x2D.CreateScale(scale.X, scale.Y, center);
            Matrix3x2D scaleAroundCenterExpected = Matrix3x2D.CreateTranslation(-center) * Matrix3x2D.CreateScale(scale.X, scale.Y) * Matrix3x2D.CreateTranslation(center);
            Assert.True(MathHelper.Equal(scaleAroundCenter, scaleAroundCenterExpected));
        }

        // A test for CreateTranslation (Vector2D)
        [Fact]
        public void Matrix3x2DCreateTranslationTest1()
        {
            Vector2D position = new Vector2D(2.0f, 3.0f);
            Matrix3x2D expected = new Matrix3x2D(
                1.0f, 0.0f,
                0.0f, 1.0f,
                2.0f, 3.0f);

            Matrix3x2D actual = Matrix3x2D.CreateTranslation(position);
            Assert.Equal(expected, actual);
        }

        // A test for CreateTranslation (float, float)
        [Fact]
        public void Matrix3x2DCreateTranslationTest2()
        {
            float xPosition = 2.0f;
            float yPosition = 3.0f;

            Matrix3x2D expected = new Matrix3x2D(
                1.0f, 0.0f,
                0.0f, 1.0f,
                2.0f, 3.0f);

            Matrix3x2D actual = Matrix3x2D.CreateTranslation(xPosition, yPosition);
            Assert.Equal(expected, actual);
        }

        // A test for Translation
        [Fact]
        public void Matrix3x2DTranslationTest()
        {
            Matrix3x2D a = GenerateTestMatrix();
            Matrix3x2D b = a;

            // Transformed vector that has same semantics of property must be same.
            Vector2D val = new Vector2D(a.M31, a.M32);
            Assert.Equal(val, a.Translation);

            // Set value and get value must be same.
            val = new Vector2D(1.0f, 2.0f);
            a.Translation = val;
            Assert.Equal(val, a.Translation);

            // Make sure it only modifies expected value of matrix.
            Assert.True(
                a.M11 == b.M11 && a.M12 == b.M12 &&
                a.M21 == b.M21 && a.M22 == b.M22 &&
                a.M31 != b.M31 && a.M32 != b.M32,
                "Matrix3x2D.Translation modified unexpected value of matrix.");
        }

        // A test for Equals (Matrix3x2D)
        [Fact]
        public void Matrix3x2DEqualsTest1()
        {
            Matrix3x2D a = GenerateIncrementalMatrixNumber();
            Matrix3x2D b = GenerateIncrementalMatrixNumber();

            // case 1: compare between same values
            bool expected = true;
            bool actual = a.Equals(b);
            Assert.Equal(expected, actual);

            // case 2: compare between different values
            b.M11 = 11.0f;
            expected = false;
            actual = a.Equals(b);
            Assert.Equal(expected, actual);
        }

        // A test for CreateSkew (float, float)
        [Fact]
        public void Matrix3x2DCreateSkewIdentityTest()
        {
            Matrix3x2D expected = Matrix3x2D.Identity;
            Matrix3x2D actual = Matrix3x2D.CreateSkew(0, 0);
            Assert.Equal(expected, actual);
        }

        // A test for CreateSkew (float, float)
        [Fact]
        public void Matrix3x2DCreateSkewXTest()
        {
            Matrix3x2D expected = new Matrix3x2D(1, 0, -0.414213562373095f, 1, 0, 0);
            Matrix3x2D actual = Matrix3x2D.CreateSkew(-MathHelper.Pi / 8, 0);
            Assert.True(MathHelper.Equal(expected, actual));

            expected = new Matrix3x2D(1, 0, 0.414213562373095f, 1, 0, 0);
            actual = Matrix3x2D.CreateSkew(MathHelper.Pi / 8, 0);
            Assert.True(MathHelper.Equal(expected, actual));

            Vector2D result = Vector2D.Transform(new Vector2D(0, 0), actual);
            Assert.True(MathHelper.Equal(new Vector2D(0, 0), result));

            result = Vector2D.Transform(new Vector2D(0, 1), actual);
            Assert.True(MathHelper.Equal(new Vector2D(0.414213568f, 1), result));

            result = Vector2D.Transform(new Vector2D(0, -1), actual);
            Assert.True(MathHelper.Equal(new Vector2D(-0.414213568f, -1), result));

            result = Vector2D.Transform(new Vector2D(3, 10), actual);
            Assert.True(MathHelper.Equal(new Vector2D(7.14213568f, 10), result));
        }

        // A test for CreateSkew (float, float)
        [Fact]
        public void Matrix3x2DCreateSkewYTest()
        {
            Matrix3x2D expected = new Matrix3x2D(1, -0.414213562373095f, 0, 1, 0, 0);
            Matrix3x2D actual = Matrix3x2D.CreateSkew(0, -MathHelper.Pi / 8);
            Assert.True(MathHelper.Equal(expected, actual));

            expected = new Matrix3x2D(1, 0.414213562373095f, 0, 1, 0, 0);
            actual = Matrix3x2D.CreateSkew(0, MathHelper.Pi / 8);
            Assert.True(MathHelper.Equal(expected, actual));

            Vector2D result = Vector2D.Transform(new Vector2D(0, 0), actual);
            Assert.True(MathHelper.Equal(new Vector2D(0, 0), result));

            result = Vector2D.Transform(new Vector2D(1, 0), actual);
            Assert.True(MathHelper.Equal(new Vector2D(1, 0.414213568f), result));

            result = Vector2D.Transform(new Vector2D(-1, 0), actual);
            Assert.True(MathHelper.Equal(new Vector2D(-1, -0.414213568f), result));

            result = Vector2D.Transform(new Vector2D(10, 3), actual);
            Assert.True(MathHelper.Equal(new Vector2D(10, 7.14213568f), result));
        }

        // A test for CreateSkew (float, float)
        [Fact]
        public void Matrix3x2DCreateSkewXYTest()
        {
            Matrix3x2D expected = new Matrix3x2D(1, -0.414213562373095f, 1, 1, 0, 0);
            Matrix3x2D actual = Matrix3x2D.CreateSkew(MathHelper.Pi / 4, -MathHelper.Pi / 8);
            Assert.True(MathHelper.Equal(expected, actual));

            Vector2D result = Vector2D.Transform(new Vector2D(0, 0), actual);
            Assert.True(MathHelper.Equal(new Vector2D(0, 0), result));

            result = Vector2D.Transform(new Vector2D(1, 0), actual);
            Assert.True(MathHelper.Equal(new Vector2D(1, -0.414213562373095f), result));

            result = Vector2D.Transform(new Vector2D(0, 1), actual);
            Assert.True(MathHelper.Equal(new Vector2D(1, 1), result));

            result = Vector2D.Transform(new Vector2D(1, 1), actual);
            Assert.True(MathHelper.Equal(new Vector2D(2, 0.585786437626905f), result));
        }

        // A test for CreateSkew (float, float, Vector2D)
        [Fact]
        public void Matrix3x2DCreateSkewCenterTest()
        {
            float skewX = 1, skewY = 2;
            Vector2D center = new Vector2D(23, 42);

            Matrix3x2D skewAroundZero = Matrix3x2D.CreateSkew(skewX, skewY, Vector2D.Zero);
            Matrix3x2D skewAroundZeroExpected = Matrix3x2D.CreateSkew(skewX, skewY);
            Assert.True(MathHelper.Equal(skewAroundZero, skewAroundZeroExpected));

            Matrix3x2D skewAroundCenter = Matrix3x2D.CreateSkew(skewX, skewY, center);
            Matrix3x2D skewAroundCenterExpected = Matrix3x2D.CreateTranslation(-center) * Matrix3x2D.CreateSkew(skewX, skewY) * Matrix3x2D.CreateTranslation(center);
            Assert.True(MathHelper.Equal(skewAroundCenter, skewAroundCenterExpected));
        }

        // A test for IsIdentity
        [Fact]
        public void Matrix3x2DIsIdentityTest()
        {
            Assert.True(Matrix3x2D.Identity.IsIdentity);
            Assert.True(new Matrix3x2D(1, 0, 0, 1, 0, 0).IsIdentity);
            Assert.False(new Matrix3x2D(0, 0, 0, 1, 0, 0).IsIdentity);
            Assert.False(new Matrix3x2D(1, 1, 0, 1, 0, 0).IsIdentity);
            Assert.False(new Matrix3x2D(1, 0, 1, 1, 0, 0).IsIdentity);
            Assert.False(new Matrix3x2D(1, 0, 0, 0, 0, 0).IsIdentity);
            Assert.False(new Matrix3x2D(1, 0, 0, 1, 1, 0).IsIdentity);
            Assert.False(new Matrix3x2D(1, 0, 0, 1, 0, 1).IsIdentity);
        }

        // A test for Matrix3x2D comparison involving NaN values
        [Fact]
        public void Matrix3x2DEqualsNaNTest()
        {
            Matrix3x2D a = new Matrix3x2D(float.NaN, 0, 0, 0, 0, 0);
            Matrix3x2D b = new Matrix3x2D(0, float.NaN, 0, 0, 0, 0);
            Matrix3x2D c = new Matrix3x2D(0, 0, float.NaN, 0, 0, 0);
            Matrix3x2D d = new Matrix3x2D(0, 0, 0, float.NaN, 0, 0);
            Matrix3x2D e = new Matrix3x2D(0, 0, 0, 0, float.NaN, 0);
            Matrix3x2D f = new Matrix3x2D(0, 0, 0, 0, 0, float.NaN);

            Assert.False(a == new Matrix3x2D());
            Assert.False(b == new Matrix3x2D());
            Assert.False(c == new Matrix3x2D());
            Assert.False(d == new Matrix3x2D());
            Assert.False(e == new Matrix3x2D());
            Assert.False(f == new Matrix3x2D());

            Assert.True(a != new Matrix3x2D());
            Assert.True(b != new Matrix3x2D());
            Assert.True(c != new Matrix3x2D());
            Assert.True(d != new Matrix3x2D());
            Assert.True(e != new Matrix3x2D());
            Assert.True(f != new Matrix3x2D());

            Assert.False(a.Equals(new Matrix3x2D()));
            Assert.False(b.Equals(new Matrix3x2D()));
            Assert.False(c.Equals(new Matrix3x2D()));
            Assert.False(d.Equals(new Matrix3x2D()));
            Assert.False(e.Equals(new Matrix3x2D()));
            Assert.False(f.Equals(new Matrix3x2D()));

            Assert.False(a.IsIdentity);
            Assert.False(b.IsIdentity);
            Assert.False(c.IsIdentity);
            Assert.False(d.IsIdentity);
            Assert.False(e.IsIdentity);
            Assert.False(f.IsIdentity);

            Assert.True(a.Equals(a));
            Assert.True(b.Equals(b));
            Assert.True(c.Equals(c));
            Assert.True(d.Equals(d));
            Assert.True(e.Equals(e));
            Assert.True(f.Equals(f));
        }

        // A test to make sure these types are blittable directly into GPU buffer memory layouts
        [Fact]
        public unsafe void Matrix3x2DSizeofTest()
        {
            Assert.Equal(24, sizeof(Matrix3x2D));
            Assert.Equal(48, sizeof(Matrix3x2D_2x));
            Assert.Equal(28, sizeof(Matrix3x2DPlusFloat));
            Assert.Equal(56, sizeof(Matrix3x2DPlusFloat_2x));
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Matrix3x2D_2x
        {
            private Matrix3x2D _a;
            private Matrix3x2D _b;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Matrix3x2DPlusFloat
        {
            private Matrix3x2D _v;
            private float _f;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Matrix3x2DPlusFloat_2x
        {
            private Matrix3x2DPlusFloat _a;
            private Matrix3x2DPlusFloat _b;
        }

        // A test to make sure the fields are laid out how we expect
        [Fact]
        public unsafe void Matrix3x2DFieldOffsetTest()
        {
            Matrix3x2D mat = new Matrix3x2D();
            float* basePtr = &mat.M11; // Take address of first element
            Matrix3x2D* matPtr = &mat; // Take address of whole matrix

            Assert.Equal(new IntPtr(basePtr), new IntPtr(matPtr));

            Assert.Equal(new IntPtr(basePtr + 0), new IntPtr(&mat.M11));
            Assert.Equal(new IntPtr(basePtr + 1), new IntPtr(&mat.M12));

            Assert.Equal(new IntPtr(basePtr + 2), new IntPtr(&mat.M21));
            Assert.Equal(new IntPtr(basePtr + 3), new IntPtr(&mat.M22));

            Assert.Equal(new IntPtr(basePtr + 4), new IntPtr(&mat.M31));
            Assert.Equal(new IntPtr(basePtr + 5), new IntPtr(&mat.M32));
        }

        [Fact]
        public void Matrix3x2DCreateBroadcastScalarTest()
        {
            Matrix3x2D a = Matrix3x2D.Create(float.Pi);

            Assert.Equal(Vector2D.Create(float.Pi), a.X);
            Assert.Equal(Vector2D.Create(float.Pi), a.Y);
            Assert.Equal(Vector2D.Create(float.Pi), a.Z);
        }

        [Fact]
        public void Matrix3x2DCreateBroadcastVectorTest()
        {
            Matrix3x2D a = Matrix3x2D.Create(Vector2D.Create(float.Pi, float.E));

            Assert.Equal(Vector2D.Create(float.Pi, float.E), a.X);
            Assert.Equal(Vector2D.Create(float.Pi, float.E), a.Y);
            Assert.Equal(Vector2D.Create(float.Pi, float.E), a.Z);
        }

        [Fact]
        public void Matrix3x2DCreateVectorsTest()
        {
            Matrix3x2D a = Matrix3x2D.Create(
                Vector2D.Create(11.0f, 12.0f),
                Vector2D.Create(21.0f, 22.0f),
                Vector2D.Create(31.0f, 32.0f)
            );

            Assert.Equal(Vector2D.Create(11.0f, 12.0f), a.X);
            Assert.Equal(Vector2D.Create(21.0f, 22.0f), a.Y);
            Assert.Equal(Vector2D.Create(31.0f, 32.0f), a.Z);
        }

        [Fact]
        public void Matrix3x2DGetElementTest()
        {
            Matrix3x2D a = GenerateTestMatrix();

            Assert.Equal(a.M11, a.X.X);
            Assert.Equal(a.M11, a[0, 0]);
            Assert.Equal(a.M11, a.GetElement(0, 0));

            Assert.Equal(a.M12, a.X.Y);
            Assert.Equal(a.M12, a[0, 1]);
            Assert.Equal(a.M12, a.GetElement(0, 1));

            Assert.Equal(a.M21, a.Y.X);
            Assert.Equal(a.M21, a[1, 0]);
            Assert.Equal(a.M21, a.GetElement(1, 0));

            Assert.Equal(a.M22, a.Y.Y);
            Assert.Equal(a.M22, a[1, 1]);
            Assert.Equal(a.M22, a.GetElement(1, 1));

            Assert.Equal(a.M31, a.Z.X);
            Assert.Equal(a.M31, a[2, 0]);
            Assert.Equal(a.M31, a.GetElement(2, 0));

            Assert.Equal(a.M32, a.Z.Y);
            Assert.Equal(a.M32, a[2, 1]);
            Assert.Equal(a.M32, a.GetElement(2, 1));
        }

        [Fact]
        public void Matrix3x2DGetRowTest()
        {
            Matrix3x2D a = GenerateTestMatrix();

            Vector2D vx = new Vector2D(a.M11, a.M12);
            Assert.Equal(vx, a.X);
            Assert.Equal(vx, a[0]);
            Assert.Equal(vx, a.GetRow(0));

            Vector2D vy = new Vector2D(a.M21, a.M22);
            Assert.Equal(vy, a.Y);
            Assert.Equal(vy, a[1]);
            Assert.Equal(vy, a.GetRow(1));

            Vector2D vz = new Vector2D(a.M31, a.M32);
            Assert.Equal(vz, a.Z);
            Assert.Equal(vz, a[2]);
            Assert.Equal(vz, a.GetRow(2));
        }

        [Fact]
        public void Matrix3x2DWithElementTest()
        {
            Matrix3x2D a = Matrix3x2D.Identity;

            a[0, 0] = 11.0f;
            Assert.Equal(11.5f, a.WithElement(0, 0, 11.5f).M11);
            Assert.Equal(11.0f, a.M11);

            a[0, 1] = 12.0f;
            Assert.Equal(12.5f, a.WithElement(0, 1, 12.5f).M12);
            Assert.Equal(12.0f, a.M12);

            a[1, 0] = 21.0f;
            Assert.Equal(21.5f, a.WithElement(1, 0, 21.5f).M21);
            Assert.Equal(21.0f, a.M21);

            a[1, 1] = 22.0f;
            Assert.Equal(22.5f, a.WithElement(1, 1, 22.5f).M22);
            Assert.Equal(22.0f, a.M22);

            a[2, 0] = 31.0f;
            Assert.Equal(31.5f, a.WithElement(2, 0, 31.5f).M31);
            Assert.Equal(31.0f, a.M31);

            a[2, 1] = 32.0f;
            Assert.Equal(32.5f, a.WithElement(2, 1, 32.5f).M32);
            Assert.Equal(32.0f, a.M32);
        }

        [Fact]
        public void Matrix3x2DWithRowTest()
        {
            Matrix3x2D a = Matrix3x2D.Identity;

            a[0] = Vector2D.Create(11.0f, 12.0f);
            Assert.Equal(Vector2D.Create(11.5f, 12.5f), a.WithRow(0, Vector2D.Create(11.5f, 12.5f)).X);
            Assert.Equal(Vector2D.Create(11.0f, 12.0f), a.X);

            a[1] = Vector2D.Create(21.0f, 22.0f);
            Assert.Equal(Vector2D.Create(21.5f, 22.5f), a.WithRow(1, Vector2D.Create(21.5f, 22.5f)).Y);
            Assert.Equal(Vector2D.Create(21.0f, 22.0f), a.Y);

            a[2] = Vector2D.Create(31.0f, 32.0f);
            Assert.Equal(Vector2D.Create(31.5f, 32.5f), a.WithRow(2, Vector2D.Create(31.5f, 32.5f)).Z);
            Assert.Equal(Vector2D.Create(31.0f, 32.0f), a.Z);
        }
    }
}
