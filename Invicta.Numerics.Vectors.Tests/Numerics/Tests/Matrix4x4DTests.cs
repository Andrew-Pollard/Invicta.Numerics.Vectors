// Copyright (c) Andrew Pollard, .NET Foundation and Contributors.
// Licensed under the MIT license - see README.md for details.

using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Xunit;

namespace Invicta.Numerics.Tests
{
    public sealed class Matrix4x4DDTests
    {
        private static Matrix4x4D GenerateIncrementalMatrixNumber(double value = 0.0d)
        {
            Matrix4x4D a = new Matrix4x4D();
            a.M11 = value + 1.0d;
            a.M12 = value + 2.0d;
            a.M13 = value + 3.0d;
            a.M14 = value + 4.0d;
            a.M21 = value + 5.0d;
            a.M22 = value + 6.0d;
            a.M23 = value + 7.0d;
            a.M24 = value + 8.0d;
            a.M31 = value + 9.0d;
            a.M32 = value + 10.0d;
            a.M33 = value + 11.0d;
            a.M34 = value + 12.0d;
            a.M41 = value + 13.0d;
            a.M42 = value + 14.0d;
            a.M43 = value + 15.0d;
            a.M44 = value + 16.0d;
            return a;
        }

        private static Matrix4x4D GenerateTestMatrix()
        {
            Matrix4x4D m =
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0d)) *
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0d)) *
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0d));
            m.Translation = new Vector3D(111.0d, 222.0d, 333.0d);
            return m;
        }

        private static Matrix4x4D DefaultVarianceMatrix = GenerateFilledMatrix(1e-5d);

        private static Matrix4x4D GenerateFilledMatrix(double value) => new Matrix4x4D
        {
            M11 = value,
            M12 = value,
            M13 = value,
            M14 = value,
            M21 = value,
            M22 = value,
            M23 = value,
            M24 = value,
            M31 = value,
            M32 = value,
            M33 = value,
            M34 = value,
            M41 = value,
            M42 = value,
            M43 = value,
            M44 = value
        };

        private static Vector3D InverseHandedness(Vector3D vector) => new Vector3D(vector.X, vector.Y, -vector.Z);

        // The handedness-swapped matrix of matrix M is B^-1 * M * B where B is the change of handedness matrix.
        // Since only the Z coordinate is flipped when changing handedness,
        // 
        // B = [ 1  0  0  0
        //       0  1  0  0
        //       0  0 -1  0
        //       0  0  0  1 ]
        //
        // and B is its own inverse. So the handedness swap can be simplified to
        // 
        // B^-1 * M * B = [  m11  m12  -m13  m14
        //                   m21  m22  -m23  m24
        //                  -m31 -m32   m33 -m34
        //                   m41  m42  -m43  m44 ]
        private static Matrix4x4D InverseHandedness(Matrix4x4D matrix) => new Matrix4x4D(
             matrix.M11, matrix.M12, -matrix.M13, matrix.M14,
             matrix.M21, matrix.M22, -matrix.M23, matrix.M24,
            -matrix.M31, -matrix.M32, matrix.M33, -matrix.M34,
             matrix.M41, matrix.M42, -matrix.M43, matrix.M44);

        private static void AssertEqual(Matrix4x4D expected, Matrix4x4D actual, Matrix4x4D variance)
        {
            for (var r = 0; r < 4; r++)
                for (var c = 0; c < 4; c++)
                    AssertExtensions.Equal(expected[r, c], actual[r, c], variance[r, c], $"Values differ at Matrix4x4D.M{r + 1}{c + 1}");
        }

        [Theory]
        [InlineData(0.0d, 1.0d, 0.0d, 1.0d, 0.0d, 1.0d, 0.0d, 1.0d, 0.0d, 1.0d, 0.0d, 1.0d, 0.0d, 1.0d, 0.0d, 1.0d)]
        [InlineData(1.0d, 0.0d, 1.0d, 0.0d, 1.0d, 0.0d, 1.0d, 0.0d, 1.0d, 0.0d, 1.0d, 0.0d, 1.0d, 0.0d, 1.0d, 0.0d)]
        [InlineData(3.1434343d, 1.1234123d, 0.1234123d, -0.1234123d, 3.1434343d, 1.1234123d, 3.1434343d, 1.1234123d, 0.1234123d, -0.1234123d, 3.1434343d, 1.1234123d, 3.1434343d, 1.1234123d, 0.1234123d, -0.1234123d)]
        [InlineData(1.0000001d, 0.0000001d, 2.0000001d, 0.0000002d, 1.0000001d, 0.0000001d, 1.0000001d, 0.0000001d, 2.0000001d, 0.0000002d, 1.0000001d, 0.0000001d, 1.0000001d, 0.0000001d, 2.0000001d, 0.0000002d)]
        public void Matrix4x4DIndexerGetTest(double m11, double m12, double m13, double m14, double m21, double m22, double m23, double m24, double m31, double m32, double m33, double m34, double m41, double m42, double m43, double m44)
        {
            var matrix = new Matrix4x4D(m11, m12, m13, m14, m21, m22, m23, m24, m31, m32, m33, m34, m41, m42, m43, m44);

            Assert.Equal(m11, matrix[0, 0]);
            Assert.Equal(m12, matrix[0, 1]);
            Assert.Equal(m13, matrix[0, 2]);
            Assert.Equal(m14, matrix[0, 3]);

            Assert.Equal(m21, matrix[1, 0]);
            Assert.Equal(m22, matrix[1, 1]);
            Assert.Equal(m23, matrix[1, 2]);
            Assert.Equal(m24, matrix[1, 3]);

            Assert.Equal(m31, matrix[2, 0]);
            Assert.Equal(m32, matrix[2, 1]);
            Assert.Equal(m33, matrix[2, 2]);
            Assert.Equal(m34, matrix[2, 3]);

            Assert.Equal(m41, matrix[3, 0]);
            Assert.Equal(m42, matrix[3, 1]);
            Assert.Equal(m43, matrix[3, 2]);
            Assert.Equal(m44, matrix[3, 3]);
        }

        [Theory]
        [InlineData(0.0d, 1.0d, 0.0d, 1.0d, 0.0d, 1.0d, 0.0d, 1.0d, 0.0d, 1.0d, 0.0d, 1.0d, 0.0d, 1.0d, 0.0d, 1.0d)]
        [InlineData(1.0d, 0.0d, 1.0d, 0.0d, 1.0d, 0.0d, 1.0d, 0.0d, 1.0d, 0.0d, 1.0d, 0.0d, 1.0d, 0.0d, 1.0d, 0.0d)]
        [InlineData(3.1434343d, 1.1234123d, 0.1234123d, -0.1234123d, 3.1434343d, 1.1234123d, 3.1434343d, 1.1234123d, 0.1234123d, -0.1234123d, 3.1434343d, 1.1234123d, 3.1434343d, 1.1234123d, 0.1234123d, -0.1234123d)]
        [InlineData(1.0000001d, 0.0000001d, 2.0000001d, 0.0000002d, 1.0000001d, 0.0000001d, 1.0000001d, 0.0000001d, 2.0000001d, 0.0000002d, 1.0000001d, 0.0000001d, 1.0000001d, 0.0000001d, 2.0000001d, 0.0000002d)]
        //[ActiveIssue("https://github.com/dotnet/runtime/issues/80876", TestPlatforms.iOS | TestPlatforms.tvOS)]
        public void Matrix4x4DIndexerSetTest(double m11, double m12, double m13, double m14, double m21, double m22, double m23, double m24, double m31, double m32, double m33, double m34, double m41, double m42, double m43, double m44)
        {
            var matrix = new Matrix4x4D(0.0d, 0.0d, 0.0d, 0.0d, 0.0d, 0.0d, 0.0d, 0.0d, 0.0d, 0.0d, 0.0d, 0.0d, 0.0d, 0.0d, 0.0d, 0.0d);

            matrix[0, 0] = m11;
            matrix[0, 1] = m12;
            matrix[0, 2] = m13;
            matrix[0, 3] = m14;

            matrix[1, 0] = m21;
            matrix[1, 1] = m22;
            matrix[1, 2] = m23;
            matrix[1, 3] = m24;

            matrix[2, 0] = m31;
            matrix[2, 1] = m32;
            matrix[2, 2] = m33;
            matrix[2, 3] = m34;

            matrix[3, 0] = m41;
            matrix[3, 1] = m42;
            matrix[3, 2] = m43;
            matrix[3, 3] = m44;

            Assert.Equal(m11, matrix[0, 0]);
            Assert.Equal(m12, matrix[0, 1]);
            Assert.Equal(m13, matrix[0, 2]);
            Assert.Equal(m14, matrix[0, 3]);

            Assert.Equal(m21, matrix[1, 0]);
            Assert.Equal(m22, matrix[1, 1]);
            Assert.Equal(m23, matrix[1, 2]);
            Assert.Equal(m24, matrix[1, 3]);

            Assert.Equal(m31, matrix[2, 0]);
            Assert.Equal(m32, matrix[2, 1]);
            Assert.Equal(m33, matrix[2, 2]);
            Assert.Equal(m34, matrix[2, 3]);

            Assert.Equal(m41, matrix[3, 0]);
            Assert.Equal(m42, matrix[3, 1]);
            Assert.Equal(m43, matrix[3, 2]);
            Assert.Equal(m44, matrix[3, 3]);
        }

        // A test for Identity
        [Fact]
        public void Matrix4x4DIdentityTest()
        {
            Matrix4x4D val = new Matrix4x4D();
            val.M11 = val.M22 = val.M33 = val.M44 = 1.0d;

            Assert.True(MathHelper.Equal(val, Matrix4x4D.Identity), "Matrix4x4D.Indentity was not set correctly.");
        }

        // A test for Determinant
        [Fact]
        public void Matrix4x4DDeterminantTest()
        {
            Matrix4x4D target =
                    Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0d)) *
                    Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0d)) *
                    Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0d));

            double val = 1.0d;
            double det = target.GetDeterminant();

            Assert.True(MathHelper.Equal(val, det), "Matrix4x4D.Determinant was not set correctly.");
        }

        // A test for Determinant
        // Determinant test |A| = 1 / |A'|
        [Fact]
        public void Matrix4x4DDeterminantTest1()
        {
            Matrix4x4D a = new Matrix4x4D();
            a.M11 = 5.0d;
            a.M12 = 2.0d;
            a.M13 = 8.25d;
            a.M14 = 1.0d;
            a.M21 = 12.0d;
            a.M22 = 6.8d;
            a.M23 = 2.14d;
            a.M24 = 9.6d;
            a.M31 = 6.5d;
            a.M32 = 1.0d;
            a.M33 = 3.14d;
            a.M34 = 2.22d;
            a.M41 = 0d;
            a.M42 = 0.86d;
            a.M43 = 4.0d;
            a.M44 = 1.0d;
            Matrix4x4D i;
            Assert.True(Matrix4x4D.Invert(a, out i));

            double detA = a.GetDeterminant();
            double detI = i.GetDeterminant();
            double t = 1.0d / detI;

            // only accurate to 3 precision
            Assert.True(System.Math.Abs(detA - t) < 1e-3, "Matrix4x4D.Determinant was not set correctly.");
        }

        // A test for Invert (Matrix4x4D)
        [Fact]
        public void Matrix4x4DInvertTest()
        {
            Matrix4x4D mtx =
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0d)) *
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0d)) *
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0d));

            Matrix4x4D expected = new Matrix4x4D();
            expected.M11 = 0.74999994d;
            expected.M12 = -0.216506317d;
            expected.M13 = 0.62499994d;
            expected.M14 = 0.0d;

            expected.M21 = 0.433012635d;
            expected.M22 = 0.87499994d;
            expected.M23 = -0.216506317d;
            expected.M24 = 0.0d;

            expected.M31 = -0.49999997d;
            expected.M32 = 0.433012635d;
            expected.M33 = 0.74999994d;
            expected.M34 = 0.0d;

            expected.M41 = 0.0d;
            expected.M42 = 0.0d;
            expected.M43 = 0.0d;
            expected.M44 = 0.99999994d;

            Matrix4x4D actual;

            Assert.True(Matrix4x4D.Invert(mtx, out actual));
            Assert.True(MathHelper.Equal(expected, actual), "Matrix4x4D.Invert did not return the expected value.");

            // Make sure M*M is identity matrix
            Matrix4x4D i = mtx * actual;
            Assert.True(MathHelper.Equal(i, Matrix4x4D.Identity), "Matrix4x4D.Invert did not return the expected value.");
        }

        // A test for Invert (Matrix4x4D)
        [Fact]
        public void Matrix4x4DInvertIdentityTest()
        {
            Matrix4x4D mtx = Matrix4x4D.Identity;

            Matrix4x4D actual;
            Assert.True(Matrix4x4D.Invert(mtx, out actual));

            Assert.True(MathHelper.Equal(actual, Matrix4x4D.Identity));
        }

        // A test for Invert (Matrix4x4D)
        [Fact]
        public void Matrix4x4DInvertTranslationTest()
        {
            Matrix4x4D mtx = Matrix4x4D.CreateTranslation(23, 42, 666);

            Matrix4x4D actual;
            Assert.True(Matrix4x4D.Invert(mtx, out actual));

            Matrix4x4D i = mtx * actual;
            Assert.True(MathHelper.Equal(i, Matrix4x4D.Identity));
        }

        // A test for Invert (Matrix4x4D)
        [Fact]
        public void Matrix4x4DInvertRotationTest()
        {
            Matrix4x4D mtx = Matrix4x4D.CreateFromYawPitchRoll(3, 4, 5);

            Matrix4x4D actual;
            Assert.True(Matrix4x4D.Invert(mtx, out actual));

            Matrix4x4D i = mtx * actual;
            Assert.True(MathHelper.Equal(i, Matrix4x4D.Identity));
        }

        // A test for Invert (Matrix4x4D)
        [Fact]
        public void Matrix4x4DInvertScaleTest()
        {
            Matrix4x4D mtx = Matrix4x4D.CreateScale(23, 42, -666);

            Matrix4x4D actual;
            Assert.True(Matrix4x4D.Invert(mtx, out actual));

            Matrix4x4D i = mtx * actual;
            Assert.True(MathHelper.Equal(i, Matrix4x4D.Identity));
        }

        // A test for Invert (Matrix4x4D)
        [Fact]
        public void Matrix4x4DInvertProjectionTest()
        {
            Matrix4x4D mtx = Matrix4x4D.CreatePerspectiveFieldOfView(1, 1.333d, 0.1d, 666);

            Matrix4x4D actual;
            Assert.True(Matrix4x4D.Invert(mtx, out actual));

            Matrix4x4D i = mtx * actual;
            Assert.True(MathHelper.Equal(i, Matrix4x4D.Identity));
        }

        // A test for Invert (Matrix4x4D)
        [Fact]
        public void Matrix4x4DInvertAffineTest()
        {
            Matrix4x4D mtx = Matrix4x4D.CreateFromYawPitchRoll(3, 4, 5) *
                            Matrix4x4D.CreateScale(23, 42, -666) *
                            Matrix4x4D.CreateTranslation(17, 53, 89);

            Matrix4x4D actual;
            Assert.True(Matrix4x4D.Invert(mtx, out actual));

            Matrix4x4D i = mtx * actual;
            Assert.True(MathHelper.Equal(i, Matrix4x4D.Identity));
        }

        // A test for Invert (Matrix4x4D)
        [Fact]
        public void Matrix4x4DInvertRank3()
        {
            // A 4x4 Matrix having a rank of 3
            Matrix4x4D mtx = new Matrix4x4D(1.0d, 2.0d, 3.0d, 0.0d,
                                          5.0d, 1.0d, 6.0d, 0.0d,
                                          8.0d, 9.0d, 1.0d, 0.0d,
                                          4.0d, 7.0d, 3.0d, 0.0d);

            Matrix4x4D actual;
            Assert.False(Matrix4x4D.Invert(mtx, out actual));

            Matrix4x4D i = mtx * actual;
            Assert.False(MathHelper.Equal(i, Matrix4x4D.Identity));
        }

        void DecomposeTest(double yaw, double pitch, double roll, Vector3D expectedTranslation, Vector3D expectedScales)
        {
            QuaternionD expectedRotation = QuaternionD.CreateFromYawPitchRoll(MathHelper.ToRadians(yaw),
                                                                            MathHelper.ToRadians(pitch),
                                                                            MathHelper.ToRadians(roll));

            Matrix4x4D m = Matrix4x4D.CreateScale(expectedScales) *
                          Matrix4x4D.CreateFromQuaternion(expectedRotation) *
                          Matrix4x4D.CreateTranslation(expectedTranslation);

            Vector3D scales;
            QuaternionD rotation;
            Vector3D translation;

            bool actualResult = Matrix4x4D.Decompose(m, out scales, out rotation, out translation);
            Assert.True(actualResult, "Matrix4x4D.Decompose did not return expected value.");

            bool scaleIsZeroOrNegative = expectedScales.X <= 0 ||
                                         expectedScales.Y <= 0 ||
                                         expectedScales.Z <= 0;

            if (scaleIsZeroOrNegative)
            {
                Assert.True(MathHelper.Equal(Math.Abs(expectedScales.X), Math.Abs(scales.X)), "Matrix4x4D.Decompose did not return expected value.");
                Assert.True(MathHelper.Equal(Math.Abs(expectedScales.Y), Math.Abs(scales.Y)), "Matrix4x4D.Decompose did not return expected value.");
                Assert.True(MathHelper.Equal(Math.Abs(expectedScales.Z), Math.Abs(scales.Z)), "Matrix4x4D.Decompose did not return expected value.");
            }
            else
            {
                Assert.True(MathHelper.Equal(expectedScales, scales), string.Format("Matrix4x4D.Decompose did not return expected value Expected:{0} actual:{1}.", expectedScales, scales));
                Assert.True(MathHelper.EqualRotation(expectedRotation, rotation), string.Format("Matrix4x4D.Decompose did not return expected value. Expected:{0} actual:{1}.", expectedRotation, rotation));
            }

            Assert.True(MathHelper.Equal(expectedTranslation, translation), string.Format("Matrix4x4D.Decompose did not return expected value. Expected:{0} actual:{1}.", expectedTranslation, translation));
        }

        // Various rotation decompose test.
        [Fact]
        public void Matrix4x4DDecomposeTest01()
        {
            DecomposeTest(10.0d, 20.0d, 30.0d, new Vector3D(10, 20, 30), new Vector3D(2, 3, 4));

            const double step = 35.0d;

            for (double yawAngle = -720.0d; yawAngle <= 720.0d; yawAngle += step)
            {
                for (double pitchAngle = -720.0d; pitchAngle <= 720.0d; pitchAngle += step)
                {
                    for (double rollAngle = -720.0d; rollAngle <= 720.0d; rollAngle += step)
                    {
                        DecomposeTest(yawAngle, pitchAngle, rollAngle, new Vector3D(10, 20, 30), new Vector3D(2, 3, 4));
                    }
                }
            }
        }

        // Various scaled matrix decompose test.
        [Fact]
        public void Matrix4x4DDecomposeTest02()
        {
            DecomposeTest(10.0d, 20.0d, 30.0d, new Vector3D(10, 20, 30), new Vector3D(2, 3, 4));

            // Various scales.
            DecomposeTest(0, 0, 0, Vector3D.Zero, new Vector3D(1, 2, 3));
            DecomposeTest(0, 0, 0, Vector3D.Zero, new Vector3D(1, 3, 2));
            DecomposeTest(0, 0, 0, Vector3D.Zero, new Vector3D(2, 1, 3));
            DecomposeTest(0, 0, 0, Vector3D.Zero, new Vector3D(2, 3, 1));
            DecomposeTest(0, 0, 0, Vector3D.Zero, new Vector3D(3, 1, 2));
            DecomposeTest(0, 0, 0, Vector3D.Zero, new Vector3D(3, 2, 1));

            DecomposeTest(0, 0, 0, Vector3D.Zero, new Vector3D(-2, 1, 1));

            // Small scales.
            DecomposeTest(0, 0, 0, Vector3D.Zero, new Vector3D(1e-4d, 2e-4d, 3e-4d));
            DecomposeTest(0, 0, 0, Vector3D.Zero, new Vector3D(1e-4d, 3e-4d, 2e-4d));
            DecomposeTest(0, 0, 0, Vector3D.Zero, new Vector3D(2e-4d, 1e-4d, 3e-4d));
            DecomposeTest(0, 0, 0, Vector3D.Zero, new Vector3D(2e-4d, 3e-4d, 1e-4d));
            DecomposeTest(0, 0, 0, Vector3D.Zero, new Vector3D(3e-4d, 1e-4d, 2e-4d));
            DecomposeTest(0, 0, 0, Vector3D.Zero, new Vector3D(3e-4d, 2e-4d, 1e-4d));

            // Zero scales.
            DecomposeTest(0, 0, 0, new Vector3D(10, 20, 30), new Vector3D(0, 0, 0));
            DecomposeTest(0, 0, 0, new Vector3D(10, 20, 30), new Vector3D(1, 0, 0));
            DecomposeTest(0, 0, 0, new Vector3D(10, 20, 30), new Vector3D(0, 1, 0));
            DecomposeTest(0, 0, 0, new Vector3D(10, 20, 30), new Vector3D(0, 0, 1));
            DecomposeTest(0, 0, 0, new Vector3D(10, 20, 30), new Vector3D(0, 1, 1));
            DecomposeTest(0, 0, 0, new Vector3D(10, 20, 30), new Vector3D(1, 0, 1));
            DecomposeTest(0, 0, 0, new Vector3D(10, 20, 30), new Vector3D(1, 1, 0));

            // Negative scales.
            DecomposeTest(0, 0, 0, new Vector3D(10, 20, 30), new Vector3D(-1, -1, -1));
            DecomposeTest(0, 0, 0, new Vector3D(10, 20, 30), new Vector3D(1, -1, -1));
            DecomposeTest(0, 0, 0, new Vector3D(10, 20, 30), new Vector3D(-1, 1, -1));
            DecomposeTest(0, 0, 0, new Vector3D(10, 20, 30), new Vector3D(-1, -1, 1));
            DecomposeTest(0, 0, 0, new Vector3D(10, 20, 30), new Vector3D(-1, 1, 1));
            DecomposeTest(0, 0, 0, new Vector3D(10, 20, 30), new Vector3D(1, -1, 1));
            DecomposeTest(0, 0, 0, new Vector3D(10, 20, 30), new Vector3D(1, 1, -1));
        }

        void DecomposeScaleTest(double sx, double sy, double sz)
        {
            Matrix4x4D m = Matrix4x4D.CreateScale(sx, sy, sz);

            Vector3D expectedScales = new Vector3D(sx, sy, sz);
            Vector3D scales;
            QuaternionD rotation;
            Vector3D translation;

            bool actualResult = Matrix4x4D.Decompose(m, out scales, out rotation, out translation);
            Assert.True(actualResult, "Matrix4x4D.Decompose did not return expected value.");
            Assert.True(MathHelper.Equal(expectedScales, scales), "Matrix4x4D.Decompose did not return expected value.");
            Assert.True(MathHelper.EqualRotation(QuaternionD.Identity, rotation), "Matrix4x4D.Decompose did not return expected value.");
            Assert.True(MathHelper.Equal(Vector3D.Zero, translation), "Matrix4x4D.Decompose did not return expected value.");
        }

        // Tiny scale decompose test.
        [Fact]
        public void Matrix4x4DDecomposeTest03()
        {
            DecomposeScaleTest(1, 2e-4d, 3e-4d);
            DecomposeScaleTest(1, 3e-4d, 2e-4d);
            DecomposeScaleTest(2e-4d, 1, 3e-4d);
            DecomposeScaleTest(2e-4d, 3e-4d, 1);
            DecomposeScaleTest(3e-4d, 1, 2e-4d);
            DecomposeScaleTest(3e-4d, 2e-4d, 1);
        }

        [Fact]
        public void Matrix4x4DDecomposeTest04()
        {
            Vector3D scales;
            QuaternionD rotation;
            Vector3D translation;

            Assert.False(Matrix4x4D.Decompose(GenerateIncrementalMatrixNumber(), out scales, out rotation, out translation), "decompose should have failed.");
            Assert.False(Matrix4x4D.Decompose(new Matrix4x4D(Matrix3x2D.CreateSkew(1, 2)), out scales, out rotation, out translation), "decompose should have failed.");
        }

        // Transform by quaternion test
        [Fact]
        public void Matrix4x4DTransformTest()
        {
            Matrix4x4D target = GenerateIncrementalMatrixNumber();

            Matrix4x4D m =
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0d)) *
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0d)) *
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0d));

            QuaternionD q = QuaternionD.CreateFromRotationMatrix(m);

            Matrix4x4D expected = target * m;
            Matrix4x4D actual;
            actual = Matrix4x4D.Transform(target, q);
            Assert.True(MathHelper.Equal(expected, actual), "Matrix4x4D.Transform did not return the expected value.");
        }

        // A test for CreateRotationX (double)
        [Fact]
        public void Matrix4x4DCreateRotationXTest()
        {
            double radians = MathHelper.ToRadians(30.0d);

            Matrix4x4D expected = new Matrix4x4D();

            expected.M11 = 1.0d;
            expected.M22 = 0.8660254d;
            expected.M23 = 0.5d;
            expected.M32 = -0.5d;
            expected.M33 = 0.8660254d;
            expected.M44 = 1.0d;

            Matrix4x4D actual;

            actual = Matrix4x4D.CreateRotationX(radians);
            Assert.True(MathHelper.Equal(expected, actual), "Matrix4x4D.CreateRotationX did not return the expected value.");
        }

        // A test for CreateRotationX (double)
        // CreateRotationX of zero degree
        [Fact]
        public void Matrix4x4DCreateRotationXTest1()
        {
            double radians = 0;

            Matrix4x4D expected = Matrix4x4D.Identity;
            Matrix4x4D actual = Matrix4x4D.CreateRotationX(radians);
            Assert.True(MathHelper.Equal(expected, actual), "Matrix4x4D.CreateRotationX did not return the expected value.");
        }

        // A test for CreateRotationX (double, Vector3D)
        [Fact]
        public void Matrix4x4DCreateRotationXCenterTest()
        {
            double radians = MathHelper.ToRadians(30.0d);
            Vector3D center = new Vector3D(23, 42, 66);

            Matrix4x4D rotateAroundZero = Matrix4x4D.CreateRotationX(radians, Vector3D.Zero);
            Matrix4x4D rotateAroundZeroExpected = Matrix4x4D.CreateRotationX(radians);
            Assert.True(MathHelper.Equal(rotateAroundZero, rotateAroundZeroExpected));

            Matrix4x4D rotateAroundCenter = Matrix4x4D.CreateRotationX(radians, center);
            Matrix4x4D rotateAroundCenterExpected = Matrix4x4D.CreateTranslation(-center) * Matrix4x4D.CreateRotationX(radians) * Matrix4x4D.CreateTranslation(center);
            Assert.True(MathHelper.Equal(rotateAroundCenter, rotateAroundCenterExpected));
        }

        // A test for CreateRotationY (double)
        [Fact]
        public void Matrix4x4DCreateRotationYTest()
        {
            double radians = MathHelper.ToRadians(60.0d);

            Matrix4x4D expected = new Matrix4x4D();

            expected.M11 = 0.49999997d;
            expected.M13 = -0.866025448d;
            expected.M22 = 1.0d;
            expected.M31 = 0.866025448d;
            expected.M33 = 0.49999997d;
            expected.M44 = 1.0d;

            Matrix4x4D actual;
            actual = Matrix4x4D.CreateRotationY(radians);
            Assert.True(MathHelper.Equal(expected, actual), "Matrix4x4D.CreateRotationY did not return the expected value.");
        }

        // A test for RotationY (double)
        // CreateRotationY test for negative angle
        [Fact]
        public void Matrix4x4DCreateRotationYTest1()
        {
            double radians = MathHelper.ToRadians(-300.0d);

            Matrix4x4D expected = new Matrix4x4D();

            expected.M11 = 0.49999997d;
            expected.M13 = -0.866025448d;
            expected.M22 = 1.0d;
            expected.M31 = 0.866025448d;
            expected.M33 = 0.49999997d;
            expected.M44 = 1.0d;

            Matrix4x4D actual = Matrix4x4D.CreateRotationY(radians);
            Assert.True(MathHelper.Equal(expected, actual), "Matrix4x4D.CreateRotationY did not return the expected value.");
        }

        // A test for CreateRotationY (double, Vector3D)
        [Fact]
        public void Matrix4x4DCreateRotationYCenterTest()
        {
            double radians = MathHelper.ToRadians(30.0d);
            Vector3D center = new Vector3D(23, 42, 66);

            Matrix4x4D rotateAroundZero = Matrix4x4D.CreateRotationY(radians, Vector3D.Zero);
            Matrix4x4D rotateAroundZeroExpected = Matrix4x4D.CreateRotationY(radians);
            Assert.True(MathHelper.Equal(rotateAroundZero, rotateAroundZeroExpected));

            Matrix4x4D rotateAroundCenter = Matrix4x4D.CreateRotationY(radians, center);
            Matrix4x4D rotateAroundCenterExpected = Matrix4x4D.CreateTranslation(-center) * Matrix4x4D.CreateRotationY(radians) * Matrix4x4D.CreateTranslation(center);
            Assert.True(MathHelper.Equal(rotateAroundCenter, rotateAroundCenterExpected));
        }

        // A test for CreateFromAxisAngle(Vector3D,double)
        [Fact]
        public void Matrix4x4DCreateFromAxisAngleTest()
        {
            double radians = MathHelper.ToRadians(-30.0d);

            Matrix4x4D expected = Matrix4x4D.CreateRotationX(radians);
            Matrix4x4D actual = Matrix4x4D.CreateFromAxisAngle(Vector3D.UnitX, radians);
            Assert.True(MathHelper.Equal(expected, actual));

            expected = Matrix4x4D.CreateRotationY(radians);
            actual = Matrix4x4D.CreateFromAxisAngle(Vector3D.UnitY, radians);
            Assert.True(MathHelper.Equal(expected, actual));

            expected = Matrix4x4D.CreateRotationZ(radians);
            actual = Matrix4x4D.CreateFromAxisAngle(Vector3D.UnitZ, radians);
            Assert.True(MathHelper.Equal(expected, actual));

            expected = Matrix4x4D.CreateFromQuaternion(QuaternionD.CreateFromAxisAngle(Vector3D.Normalize(Vector3D.One), radians));
            actual = Matrix4x4D.CreateFromAxisAngle(Vector3D.Normalize(Vector3D.One), radians);
            Assert.True(MathHelper.Equal(expected, actual));

            const int rotCount = 16;
            for (int i = 0; i < rotCount; ++i)
            {
                double latitude = (2.0d * MathHelper.Pi) * ((double)i / (double)rotCount);
                for (int j = 0; j < rotCount; ++j)
                {
                    double longitude = -MathHelper.PiOver2 + MathHelper.Pi * ((double)j / (double)rotCount);

                    Matrix4x4D m = Matrix4x4D.CreateRotationZ(longitude) * Matrix4x4D.CreateRotationY(latitude);
                    Vector3D axis = new Vector3D(m.M11, m.M12, m.M13);
                    for (int k = 0; k < rotCount; ++k)
                    {
                        double rot = (2.0d * MathHelper.Pi) * ((double)k / (double)rotCount);
                        expected = Matrix4x4D.CreateFromQuaternion(QuaternionD.CreateFromAxisAngle(axis, rot));
                        actual = Matrix4x4D.CreateFromAxisAngle(axis, rot);
                        Assert.True(MathHelper.Equal(expected, actual));
                    }
                }
            }
        }

        [Fact]
        public void Matrix4x4DCreateFromYawPitchRollTest1()
        {
            double yawAngle = MathHelper.ToRadians(30.0d);
            double pitchAngle = MathHelper.ToRadians(40.0d);
            double rollAngle = MathHelper.ToRadians(50.0d);

            Matrix4x4D yaw = Matrix4x4D.CreateFromAxisAngle(Vector3D.UnitY, yawAngle);
            Matrix4x4D pitch = Matrix4x4D.CreateFromAxisAngle(Vector3D.UnitX, pitchAngle);
            Matrix4x4D roll = Matrix4x4D.CreateFromAxisAngle(Vector3D.UnitZ, rollAngle);

            Matrix4x4D expected = roll * pitch * yaw;
            Matrix4x4D actual = Matrix4x4D.CreateFromYawPitchRoll(yawAngle, pitchAngle, rollAngle);
            Assert.True(MathHelper.Equal(expected, actual));
        }

        // Covers more numeric rigions
        [Fact]
        public void Matrix4x4DCreateFromYawPitchRollTest2()
        {
            const double step = 35.0d;

            for (double yawAngle = -720.0d; yawAngle <= 720.0d; yawAngle += step)
            {
                for (double pitchAngle = -720.0d; pitchAngle <= 720.0d; pitchAngle += step)
                {
                    for (double rollAngle = -720.0d; rollAngle <= 720.0d; rollAngle += step)
                    {
                        double yawRad = MathHelper.ToRadians(yawAngle);
                        double pitchRad = MathHelper.ToRadians(pitchAngle);
                        double rollRad = MathHelper.ToRadians(rollAngle);
                        Matrix4x4D yaw = Matrix4x4D.CreateFromAxisAngle(Vector3D.UnitY, yawRad);
                        Matrix4x4D pitch = Matrix4x4D.CreateFromAxisAngle(Vector3D.UnitX, pitchRad);
                        Matrix4x4D roll = Matrix4x4D.CreateFromAxisAngle(Vector3D.UnitZ, rollRad);

                        Matrix4x4D expected = roll * pitch * yaw;
                        Matrix4x4D actual = Matrix4x4D.CreateFromYawPitchRoll(yawRad, pitchRad, rollRad);
                        Assert.True(MathHelper.Equal(expected, actual), string.Format("Yaw:{0} Pitch:{1} Roll:{2}", yawAngle, pitchAngle, rollAngle));
                    }
                }
            }
        }

        // Simple shadow test.
        [Fact]
        public void Matrix4x4DCreateShadowTest01()
        {
            Vector3D lightDir = Vector3D.UnitY;
            PlaneD plane = new PlaneD(Vector3D.UnitY, 0);

            Matrix4x4D expected = Matrix4x4D.CreateScale(1, 0, 1);

            Matrix4x4D actual = Matrix4x4D.CreateShadow(lightDir, plane);
            Assert.True(MathHelper.Equal(expected, actual), "Matrix4x4D.CreateShadow did not returned expected value.");
        }

        // Various plane projections.
        [Fact]
        public void Matrix4x4DCreateShadowTest02()
        {
            // Complex cases.
            PlaneD[] planes = {
                new PlaneD( 0, 1, 0, 0 ),
                new PlaneD( 1, 2, 3, 4 ),
                new PlaneD( 5, 6, 7, 8 ),
                new PlaneD(-1,-2,-3,-4 ),
                new PlaneD(-5,-6,-7,-8 ),
            };

            Vector3D[] points = {
                new Vector3D( 1, 2, 3),
                new Vector3D( 5, 6, 7),
                new Vector3D( 8, 9, 10),
                new Vector3D(-1,-2,-3),
                new Vector3D(-5,-6,-7),
                new Vector3D(-8,-9,-10),
            };

            foreach (PlaneD p in planes)
            {
                PlaneD plane = PlaneD.Normalize(p);

                // Try various direction of light directions.
                var testDirections = new Vector3D[]
                {
                    new Vector3D( -1.0d, 1.0d, 1.0d ),
                    new Vector3D(  0.0d, 1.0d, 1.0d ),
                    new Vector3D(  1.0d, 1.0d, 1.0d ),
                    new Vector3D( -1.0d, 0.0d, 1.0d ),
                    new Vector3D(  0.0d, 0.0d, 1.0d ),
                    new Vector3D(  1.0d, 0.0d, 1.0d ),
                    new Vector3D( -1.0d,-1.0d, 1.0d ),
                    new Vector3D(  0.0d,-1.0d, 1.0d ),
                    new Vector3D(  1.0d,-1.0d, 1.0d ),

                    new Vector3D( -1.0d, 1.0d, 0.0d ),
                    new Vector3D(  0.0d, 1.0d, 0.0d ),
                    new Vector3D(  1.0d, 1.0d, 0.0d ),
                    new Vector3D( -1.0d, 0.0d, 0.0d ),
                    new Vector3D(  0.0d, 0.0d, 0.0d ),
                    new Vector3D(  1.0d, 0.0d, 0.0d ),
                    new Vector3D( -1.0d,-1.0d, 0.0d ),
                    new Vector3D(  0.0d,-1.0d, 0.0d ),
                    new Vector3D(  1.0d,-1.0d, 0.0d ),

                    new Vector3D( -1.0d, 1.0d,-1.0d ),
                    new Vector3D(  0.0d, 1.0d,-1.0d ),
                    new Vector3D(  1.0d, 1.0d,-1.0d ),
                    new Vector3D( -1.0d, 0.0d,-1.0d ),
                    new Vector3D(  0.0d, 0.0d,-1.0d ),
                    new Vector3D(  1.0d, 0.0d,-1.0d ),
                    new Vector3D( -1.0d,-1.0d,-1.0d ),
                    new Vector3D(  0.0d,-1.0d,-1.0d ),
                    new Vector3D(  1.0d,-1.0d,-1.0d ),
                };

                foreach (Vector3D lightDirInfo in testDirections)
                {
                    if (lightDirInfo.Length() < 0.1d)
                        continue;
                    Vector3D lightDir = Vector3D.Normalize(lightDirInfo);

                    if (PlaneD.DotNormal(plane, lightDir) < 0.1d)
                        continue;

                    Matrix4x4D m = Matrix4x4D.CreateShadow(lightDir, plane);
                    Vector3D pp = -plane.D * plane.Normal; // origin of the plane.

                    //
                    foreach (Vector3D point in points)
                    {
                        Vector4D v4 = Vector4D.Transform(point, m);

                        Vector3D sp = new Vector3D(v4.X, v4.Y, v4.Z) / v4.W;

                        // Make sure transformed position is on the plane.
                        Vector3D v = sp - pp;
                        double d = Vector3D.Dot(v, plane.Normal);
                        Assert.True(MathHelper.Equal(d, 0), "Matrix4x4D.CreateShadow did not provide expected value.");

                        // make sure direction between transformed position and original position are same as light direction.
                        if (Vector3D.Dot(point - pp, plane.Normal) > 0.0001d)
                        {
                            Vector3D dir = Vector3D.Normalize(point - sp);
                            Assert.True(MathHelper.Equal(dir, lightDir), "Matrix4x4D.CreateShadow did not provide expected value.");
                        }
                    }
                }
            }
        }

        void CreateReflectionTest(PlaneD plane, Matrix4x4D expected)
        {
            Matrix4x4D actual = Matrix4x4D.CreateReflection(plane);
            Assert.True(MathHelper.Equal(actual, expected), "Matrix4x4D.CreateReflection did not return expected value.");
        }

        [Fact]
        public void Matrix4x4DCreateReflectionTest01()
        {
            // XY plane.
            CreateReflectionTest(new PlaneD(Vector3D.UnitZ, 0), Matrix4x4D.CreateScale(1, 1, -1));
            // XZ plane.
            CreateReflectionTest(new PlaneD(Vector3D.UnitY, 0), Matrix4x4D.CreateScale(1, -1, 1));
            // YZ plane.
            CreateReflectionTest(new PlaneD(Vector3D.UnitX, 0), Matrix4x4D.CreateScale(-1, 1, 1));

            // Complex cases.
            PlaneD[] planes = {
                new PlaneD( 0, 1, 0, 0 ),
                new PlaneD( 1, 2, 3, 4 ),
                new PlaneD( 5, 6, 7, 8 ),
                new PlaneD(-1,-2,-3,-4 ),
                new PlaneD(-5,-6,-7,-8 ),
            };

            Vector3D[] points = {
                new Vector3D( 1, 2, 3),
                new Vector3D( 5, 6, 7),
                new Vector3D(-1,-2,-3),
                new Vector3D(-5,-6,-7),
            };

            foreach (PlaneD p in planes)
            {
                PlaneD plane = PlaneD.Normalize(p);
                Matrix4x4D m = Matrix4x4D.CreateReflection(plane);
                Vector3D pp = -plane.D * plane.Normal; // Position on the plane.

                //
                foreach (Vector3D point in points)
                {
                    Vector3D rp = Vector3D.Transform(point, m);

                    // Manually compute reflection point and compare results.
                    Vector3D v = point - pp;
                    double d = Vector3D.Dot(v, plane.Normal);
                    Vector3D vp = point - 2.0d * d * plane.Normal;
                    Assert.True(MathHelper.Equal(rp, vp), "Matrix4x4D.CreateReflection did not provide expected value.");
                }
            }
        }

        [Fact]
        public void Matrix4x4DCreateReflectionTest02()
        {
            PlaneD plane = new PlaneD(0, 1, 0, 60);
            Matrix4x4D actual = Matrix4x4D.CreateReflection(plane);

            AssertExtensions.Equal(1.0d, actual.M11, 0.0d);
            AssertExtensions.Equal(0.0d, actual.M12, 0.0d);
            AssertExtensions.Equal(0.0d, actual.M13, 0.0d);
            AssertExtensions.Equal(0.0d, actual.M14, 0.0d);

            AssertExtensions.Equal(0.0d, actual.M21, 0.0d);
            AssertExtensions.Equal(-1.0d, actual.M22, 0.0d);
            AssertExtensions.Equal(0.0d, actual.M23, 0.0d);
            AssertExtensions.Equal(0.0d, actual.M24, 0.0d);

            AssertExtensions.Equal(0.0d, actual.M31, 0.0d);
            AssertExtensions.Equal(0.0d, actual.M32, 0.0d);
            AssertExtensions.Equal(1.0d, actual.M33, 0.0d);
            AssertExtensions.Equal(0.0d, actual.M34, 0.0d);

            AssertExtensions.Equal(0.0d, actual.M41, 0.0d);
            AssertExtensions.Equal(-120.0d, actual.M42, 0.0d);
            AssertExtensions.Equal(0.0d, actual.M43, 0.0d);
            AssertExtensions.Equal(1.0d, actual.M44, 0.0d);
        }

        // A test for CreateRotationZ (double)
        [Fact]
        public void Matrix4x4DCreateRotationZTest()
        {
            double radians = MathHelper.ToRadians(50.0d);

            Matrix4x4D expected = new Matrix4x4D();
            expected.M11 = 0.642787635d;
            expected.M12 = 0.766044438d;
            expected.M21 = -0.766044438d;
            expected.M22 = 0.642787635d;
            expected.M33 = 1.0d;
            expected.M44 = 1.0d;

            Matrix4x4D actual;
            actual = Matrix4x4D.CreateRotationZ(radians);
            Assert.True(MathHelper.Equal(expected, actual), "Matrix4x4D.CreateRotationZ did not return the expected value.");
        }

        // A test for CreateRotationZ (double, Vector3D)
        [Fact]
        public void Matrix4x4DCreateRotationZCenterTest()
        {
            double radians = MathHelper.ToRadians(30.0d);
            Vector3D center = new Vector3D(23, 42, 66);

            Matrix4x4D rotateAroundZero = Matrix4x4D.CreateRotationZ(radians, Vector3D.Zero);
            Matrix4x4D rotateAroundZeroExpected = Matrix4x4D.CreateRotationZ(radians);
            Assert.True(MathHelper.Equal(rotateAroundZero, rotateAroundZeroExpected));

            Matrix4x4D rotateAroundCenter = Matrix4x4D.CreateRotationZ(radians, center);
            Matrix4x4D rotateAroundCenterExpected = Matrix4x4D.CreateTranslation(-center) * Matrix4x4D.CreateRotationZ(radians) * Matrix4x4D.CreateTranslation(center);
            Assert.True(MathHelper.Equal(rotateAroundCenter, rotateAroundCenterExpected));
        }

        [Fact]
        public void Matrix4x4DCreateLookAtTest()
        {
            Vector3D cameraPosition = new Vector3D(10.0d, 20.0d, 30.0d);
            Vector3D cameraTarget = new Vector3D(3.0d, 2.0d, -4.0d);
            Vector3D cameraUpVector = new Vector3D(0.0d, 1.0d, 0.0d);

            Matrix4x4D expected = new Matrix4x4D();
            expected.M11 = +0.979457d;
            expected.M12 = -0.0928268d;
            expected.M13 = +0.179017d;

            expected.M21 = +0.0d;
            expected.M22 = +0.887748d;
            expected.M23 = +0.460329d;

            expected.M31 = -0.201653d;
            expected.M32 = -0.450873d;
            expected.M33 = +0.869511d;

            expected.M41 = -3.74498d;
            expected.M42 = -3.30051d;
            expected.M43 = -37.0821d;
            expected.M44 = +1.0d;

            Matrix4x4D actual = Matrix4x4D.CreateLookAt(cameraPosition, cameraTarget, cameraUpVector);
            Assert.True(MathHelper.Equal(expected, actual), $"{nameof(Matrix4x4D)}.{nameof(Matrix4x4D.CreateLookAt)} did not return the expected value.");
        }

        [Fact]
        public void Matrix4x4DCreateLookAtLeftHandedTest()
        {
            Vector3D cameraPosition = new Vector3D(10.0d, 20.0d, 30.0d);
            Vector3D cameraTarget = new Vector3D(3.0d, 2.0d, -4.0d);
            Vector3D cameraUpVector = new Vector3D(0.0d, 1.0d, 0.0d);

            Matrix4x4D expected = new Matrix4x4D();
            expected.M11 = -0.979457d;
            expected.M12 = -0.0928268d;
            expected.M13 = -0.179017d;

            expected.M21 = +0.0d;
            expected.M22 = +0.887748d;
            expected.M23 = -0.460329d;

            expected.M31 = +0.201653d;
            expected.M32 = -0.450873d;
            expected.M33 = -0.869511d;

            expected.M41 = +3.74498d;
            expected.M42 = -3.30051d;
            expected.M43 = +37.0821d;
            expected.M44 = +1.0d;

            Matrix4x4D actual = Matrix4x4D.CreateLookAtLeftHanded(cameraPosition, cameraTarget, cameraUpVector);
            Assert.True(MathHelper.Equal(expected, actual), $"{nameof(Matrix4x4D)}.{nameof(Matrix4x4D.CreateLookAtLeftHanded)} did not return the expected value.");
        }

        [Fact]
        public void Matrix4x4DCreateLookToTest()
        {
            Vector3D cameraPosition = new Vector3D(10.0d, 20.0d, 30.0d);
            Vector3D cameraDirection = new Vector3D(-7.0d, -18.0d, -34.0d);
            Vector3D cameraUpVector = new Vector3D(0.0d, 1.0d, 0.0d);

            Matrix4x4D expected = new Matrix4x4D();
            expected.M11 = +0.979457d;
            expected.M12 = -0.0928268d;
            expected.M13 = +0.179017d;

            expected.M21 = +0.0d;
            expected.M22 = +0.887748d;
            expected.M23 = +0.460329d;

            expected.M31 = -0.201653d;
            expected.M32 = -0.450873d;
            expected.M33 = +0.869511d;

            expected.M41 = -3.74498d;
            expected.M42 = -3.30051d;
            expected.M43 = -37.0821d;
            expected.M44 = +1.0d;

            Matrix4x4D actual = Matrix4x4D.CreateLookTo(cameraPosition, cameraDirection, cameraUpVector);
            Assert.True(MathHelper.Equal(expected, actual), $"{nameof(Matrix4x4D)}.{nameof(Matrix4x4D.CreateLookTo)} did not return the expected value.");
        }

        [Fact]
        public void Matrix4x4DCreateLookToLeftHandedTest()
        {
            Vector3D cameraPosition = new Vector3D(10.0d, 20.0d, 30.0d);
            Vector3D cameraDirection = new Vector3D(-7.0d, -18.0d, -34.0d);
            Vector3D cameraUpVector = new Vector3D(0.0d, 1.0d, 0.0d);

            Matrix4x4D expected = new Matrix4x4D();
            expected.M11 = -0.979457d;
            expected.M12 = -0.0928268d;
            expected.M13 = -0.179017d;

            expected.M21 = +0.0d;
            expected.M22 = +0.887748d;
            expected.M23 = -0.460329d;

            expected.M31 = +0.201653d;
            expected.M32 = -0.450873d;
            expected.M33 = -0.869511d;

            expected.M41 = +3.74498d;
            expected.M42 = -3.30051d;
            expected.M43 = +37.0821d;
            expected.M44 = +1.0d;

            Matrix4x4D actual = Matrix4x4D.CreateLookToLeftHanded(cameraPosition, cameraDirection, cameraUpVector);
            Assert.True(MathHelper.Equal(expected, actual), $"{nameof(Matrix4x4D)}.{nameof(Matrix4x4D.CreateLookToLeftHanded)} did not return the expected value.");
        }

        [Fact]
        public void Matrix4x4DCreateViewportTest()
        {
            double x = 10.0d;
            double y = 20.0d;
            double width = 80.0d;
            double height = 160.0d;
            double minDepth = 1.5d;
            double maxDepth = 1000.0d;

            Matrix4x4D expected = new Matrix4x4D();
            expected.M11 = +40.0d;

            expected.M22 = -80.0d;

            expected.M33 = -998.5d;

            expected.M41 = +50.0d;
            expected.M42 = +100.0d;
            expected.M43 = +1.5d;
            expected.M44 = +1.0d;

            Matrix4x4D actual = Matrix4x4D.CreateViewport(x, y, width, height, minDepth, maxDepth);
            Assert.True(MathHelper.Equal(expected, actual), $"{nameof(Matrix4x4D)}.{nameof(Matrix4x4D.CreateViewport)} did not return the expected value.");
        }

        [Fact]
        public void Matrix4x4DCreateViewportLeftHandedTest()
        {
            double x = 10.0d, y = 20.0d;
            double width = 3.0d, height = 4.0d;
            double minDepth = 100.0d, maxDepth = 200.0d;

            Matrix4x4D expected = Matrix4x4D.Identity;
            expected.M11 = width * 0.5d;
            expected.M22 = -height * 0.5d;
            expected.M33 = maxDepth - minDepth;
            expected.M41 = x + expected.M11;
            expected.M42 = y - expected.M22;
            expected.M43 = minDepth;

            Matrix4x4D actual = Matrix4x4D.CreateViewportLeftHanded(x, y, width, height, minDepth, maxDepth);
            Assert.True(MathHelper.Equal(expected, actual), $"{nameof(Matrix4x4D)}.{nameof(Matrix4x4D.CreateViewportLeftHanded)} did not return the expected value.");
        }

        // A test for CreateWorld (Vector3D, Vector3D, Vector3D)
        [Fact]
        public void Matrix4x4DCreateWorldTest()
        {
            Vector3D objectPosition = new Vector3D(10.0d, 20.0d, 30.0d);
            Vector3D objectForwardDirection = new Vector3D(3.0d, 2.0d, -4.0d);
            Vector3D objectUpVector = new Vector3D(0.0d, 1.0d, 0.0d);

            Matrix4x4D expected = new Matrix4x4D();
            expected.M11 = 0.799999952d;
            expected.M12 = 0;
            expected.M13 = 0.599999964d;
            expected.M14 = 0;

            expected.M21 = -0.2228344d;
            expected.M22 = 0.928476632d;
            expected.M23 = 0.297112525d;
            expected.M24 = 0;

            expected.M31 = -0.557086d;
            expected.M32 = -0.371390671d;
            expected.M33 = 0.742781341d;
            expected.M34 = 0;

            expected.M41 = 10;
            expected.M42 = 20;
            expected.M43 = 30;
            expected.M44 = 1.0d;

            Matrix4x4D actual = Matrix4x4D.CreateWorld(objectPosition, objectForwardDirection, objectUpVector);
            Assert.True(MathHelper.Equal(expected, actual), "Matrix4x4D.CreateWorld did not return the expected value.");

            Assert.Equal(objectPosition, actual.Translation);
            Assert.True(Vector3D.Dot(Vector3D.Normalize(objectUpVector), new Vector3D(actual.M21, actual.M22, actual.M23)) > 0);
            Assert.True(Vector3D.Dot(Vector3D.Normalize(objectForwardDirection), new Vector3D(-actual.M31, -actual.M32, -actual.M33)) > 0.999d);
        }

        [Fact]
        public void Matrix4x4DCreateOrthoTest()
        {
            double width = 100.0d;
            double height = 200.0d;
            double zNearPlaneD = 1.5d;
            double zFarPlaneD = 1000.0d;

            Matrix4x4D expected = new Matrix4x4D();
            expected.M11 = +0.02d;

            expected.M22 = +0.01d;

            expected.M33 = -0.0010015d;

            expected.M43 = -0.00150225d;
            expected.M44 = +1.0d;

            Matrix4x4D actual;
            actual = Matrix4x4D.CreateOrthographic(width, height, zNearPlaneD, zFarPlaneD);
            Assert.True(MathHelper.Equal(expected, actual), $"{nameof(Matrix4x4D)}.{nameof(Matrix4x4D.CreateOrthographic)} did not return the expected value.");
        }

        [Fact]
        public void Matrix4x4DCreateOrthoLeftHandedTest()
        {
            double width = 100.0d;
            double height = 200.0d;
            double zNearPlaneD = 1.5d;
            double zFarPlaneD = 1000.0d;

            Matrix4x4D expected = new Matrix4x4D();
            expected.M11 = +0.02d;

            expected.M22 = +0.01d;

            expected.M33 = +0.0010015d;

            expected.M43 = -0.00150225d;
            expected.M44 = +1.0d;

            Matrix4x4D actual = Matrix4x4D.CreateOrthographicLeftHanded(width, height, zNearPlaneD, zFarPlaneD);
            Assert.True(MathHelper.Equal(expected, actual), $"{nameof(Matrix4x4D)}.{nameof(Matrix4x4D.CreateOrthographicLeftHanded)} did not return the expected value.");
        }

        [Fact]
        public void Matrix4x4DCreateOrthoOffCenterTest()
        {
            double left = 10.0d;
            double right = 90.0d;
            double bottom = 20.0d;
            double top = 180.0d;
            double zNearPlaneD = 1.5d;
            double zFarPlaneD = 1000.0d;

            Matrix4x4D expected = new Matrix4x4D();
            expected.M11 = +0.025d;

            expected.M22 = +0.0125d;

            expected.M33 = -0.0010015d;

            expected.M41 = -1.25d;
            expected.M42 = -1.25d;
            expected.M43 = -0.00150225d;
            expected.M44 = +1.0d;

            Matrix4x4D actual = Matrix4x4D.CreateOrthographicOffCenter(left, right, bottom, top, zNearPlaneD, zFarPlaneD);
            Assert.True(MathHelper.Equal(expected, actual), $"{nameof(Matrix4x4D)}.{nameof(Matrix4x4D.CreateOrthographicOffCenter)} did not return the expected value.");
        }

        [Fact]
        public void Matrix4x4DCreateOrthoOffCenterLeftHandedTest()
        {
            double left = 10.0d;
            double right = 90.0d;
            double bottom = 20.0d;
            double top = 180.0d;
            double zNearPlaneD = 1.5d;
            double zFarPlaneD = 1000.0d;

            Matrix4x4D expected = new Matrix4x4D();
            expected.M11 = +0.025d;

            expected.M22 = +0.0125d;

            expected.M33 = +0.0010015d;

            expected.M41 = -1.25d;
            expected.M42 = -1.25d;
            expected.M43 = -0.00150225d;
            expected.M44 = +1.0d;

            Matrix4x4D actual = Matrix4x4D.CreateOrthographicOffCenterLeftHanded(left, right, bottom, top, zNearPlaneD, zFarPlaneD);
            Assert.True(MathHelper.Equal(expected, actual), $"{nameof(Matrix4x4D)}.{nameof(Matrix4x4D.CreateOrthographicOffCenterLeftHanded)} did not return the expected value.");
        }

        [Fact]
        public void Matrix4x4DCreatePerspectiveTest()
        {
            double width = 100.0d;
            double height = 200.0d;
            double zNearPlaneD = 1.5d;
            double zFarPlaneD = 1000.0d;

            Matrix4x4D expected = new Matrix4x4D();
            expected.M11 = +0.03d;

            expected.M22 = +0.015d;

            expected.M33 = -1.0015d;
            expected.M34 = -1.0d;

            expected.M43 = -1.50225d;

            Matrix4x4D actual = Matrix4x4D.CreatePerspective(width, height, zNearPlaneD, zFarPlaneD);
            Assert.True(MathHelper.Equal(expected, actual), $"{nameof(Matrix4x4D)}.{nameof(Matrix4x4D.CreatePerspective)} did not return the expected value.");
        }

        [Fact]
        public void Matrix4x4DCreatePerspectiveLeftHandedTest()
        {
            double width = 100.0d;
            double height = 200.0d;
            double zNearPlaneD = 1.5d;
            double zFarPlaneD = 1000.0d;

            Matrix4x4D expected = new Matrix4x4D();
            expected.M11 = +0.03d;

            expected.M22 = +0.015d;

            expected.M33 = +1.0015d;
            expected.M34 = +1.0d;

            expected.M43 = -1.50225d;

            Matrix4x4D actual = Matrix4x4D.CreatePerspectiveLeftHanded(width, height, zNearPlaneD, zFarPlaneD);
            Assert.True(MathHelper.Equal(expected, actual), $"{nameof(Matrix4x4D)}.{nameof(Matrix4x4D.CreatePerspectiveLeftHanded)} did not return the expected value.");
        }

        // A test for CreatePerspective (double, double, double, double)
        // CreatePerspective test where znear = zfar
        [Fact]
        public void Matrix4x4DCreatePerspectiveTest1()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                double width = 100.0d;
                double height = 200.0d;
                double zNearPlaneD = 0.0d;
                double zFarPlaneD = 0.0d;

                Matrix4x4D actual = Matrix4x4D.CreatePerspective(width, height, zNearPlaneD, zFarPlaneD);
            });
        }

        // A test for CreatePerspective (double, double, double, double)
        // CreatePerspective test where near plane is negative value
        [Fact]
        public void Matrix4x4DCreatePerspectiveTest2()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Matrix4x4D actual = Matrix4x4D.CreatePerspective(10, 10, -10, 10);
            });
        }

        // A test for CreatePerspective (double, double, double, double)
        // CreatePerspective test where far plane is negative value
        [Fact]
        public void Matrix4x4DCreatePerspectiveTest3()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Matrix4x4D actual = Matrix4x4D.CreatePerspective(10, 10, 10, -10);
            });
        }

        // A test for CreatePerspective (double, double, double, double)
        // CreatePerspective test where near plane is beyond far plane
        [Fact]
        public void Matrix4x4DCreatePerspectiveTest4()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Matrix4x4D actual = Matrix4x4D.CreatePerspective(10, 10, 10, 1);
            });
        }

        [Fact]
        public void Matrix4x4DCreatePerspectiveFieldOfViewTest()
        {
            double fieldOfView = MathHelper.ToRadians(30.0d);
            double aspectRatio = 1280.0d / 720.0d;
            double zNearPlaneD = 1.5d;
            double zFarPlaneD = 1000.0d;

            Matrix4x4D expected = new Matrix4x4D();
            expected.M11 = +2.09928d;

            expected.M22 = +3.73205d;

            expected.M33 = -1.0015d;
            expected.M34 = -1.0d;

            expected.M43 = -1.50225d;

            Matrix4x4D actual = Matrix4x4D.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, zNearPlaneD, zFarPlaneD);
            Assert.True(MathHelper.Equal(expected, actual), $"{nameof(Matrix4x4D)}.{nameof(Matrix4x4D.CreatePerspectiveFieldOfView)} did not return the expected value.");
        }

        [Fact]
        public void Matrix4x4DCreatePerspectiveFieldOfViewLeftHandedTest()
        {
            double fieldOfView = MathHelper.ToRadians(30.0d);
            double aspectRatio = 1280.0d / 720.0d;
            double zNearPlaneD = 1.5d;
            double zFarPlaneD = 1000.0d;

            Matrix4x4D expected = new Matrix4x4D();
            expected.M11 = +2.09928d;

            expected.M22 = +3.73205d;

            expected.M33 = +1.0015d;
            expected.M34 = +1.0d;

            expected.M43 = -1.50225d;

            Matrix4x4D actual = Matrix4x4D.CreatePerspectiveFieldOfViewLeftHanded(fieldOfView, aspectRatio, zNearPlaneD, zFarPlaneD);
            Assert.True(MathHelper.Equal(expected, actual), $"{nameof(Matrix4x4D)}.{nameof(Matrix4x4D.CreatePerspectiveFieldOfViewLeftHanded)} did not return the expected value.");
        }

        // A test for CreatePerspectiveFieldOfView (double, double, double, double)
        // CreatePerspectiveFieldOfView test where filedOfView is negative value.
        [Fact]
        public void Matrix4x4DCreatePerspectiveFieldOfViewTest1()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Matrix4x4D mtx = Matrix4x4D.CreatePerspectiveFieldOfView(-1, 1, 1, 10);
            });
        }

        // A test for CreatePerspectiveFieldOfView (double, double, double, double)
        // CreatePerspectiveFieldOfView test where filedOfView is more than pi.
        [Fact]
        public void Matrix4x4DCreatePerspectiveFieldOfViewTest2()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Matrix4x4D mtx = Matrix4x4D.CreatePerspectiveFieldOfView(MathHelper.Pi + 0.01d, 1, 1, 10);
            });
        }

        // A test for CreatePerspectiveFieldOfView (double, double, double, double)
        // CreatePerspectiveFieldOfView test where nearPlaneDDistance is negative value.
        [Fact]
        public void Matrix4x4DCreatePerspectiveFieldOfViewTest3()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Matrix4x4D mtx = Matrix4x4D.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 1, -1, 10);
            });
        }

        // A test for CreatePerspectiveFieldOfView (double, double, double, double)
        // CreatePerspectiveFieldOfView test where farPlaneDDistance is negative value.
        [Fact]
        public void Matrix4x4DCreatePerspectiveFieldOfViewTest4()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Matrix4x4D mtx = Matrix4x4D.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 1, 1, -10);
            });
        }

        // A test for CreatePerspectiveFieldOfView (double, double, double, double)
        // CreatePerspectiveFieldOfView test where nearPlaneDDistance is larger than farPlaneDDistance.
        [Fact]
        public void Matrix4x4DCreatePerspectiveFieldOfViewTest5()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Matrix4x4D mtx = Matrix4x4D.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 1, 10, 1);
            });
        }

        [Fact]
        public void Matrix4x4DCreatePerspectiveOffCenterTest()
        {
            double left = 10.0d;
            double right = 90.0d;
            double bottom = 20.0d;
            double top = 180.0d;
            double zNearPlaneD = 1.5d;
            double zFarPlaneD = 1000.0d;

            Matrix4x4D expected = new Matrix4x4D();
            expected.M11 = +0.0375d;

            expected.M22 = +0.01875d;

            expected.M31 = +1.25d;
            expected.M32 = +1.25d;
            expected.M33 = -1.0015d;
            expected.M34 = -1.0d;

            expected.M43 = -1.50225d;

            Matrix4x4D actual = Matrix4x4D.CreatePerspectiveOffCenter(left, right, bottom, top, zNearPlaneD, zFarPlaneD);
            Assert.True(MathHelper.Equal(expected, actual), $"{nameof(Matrix4x4D)}.{nameof(Matrix4x4D.CreatePerspectiveOffCenter)} did not return the expected value.");
        }

        [Fact]
        public void Matrix4x4DCreatePerspectiveOffCenterLeftHandedTest()
        {
            double left = 10.0d;
            double right = 90.0d;
            double bottom = 20.0d;
            double top = 180.0d;
            double zNearPlaneD = 1.5d;
            double zFarPlaneD = 1000.0d;

            Matrix4x4D expected = new Matrix4x4D();
            expected.M11 = +0.0375d;

            expected.M22 = +0.01875d;

            expected.M31 = -1.25d;
            expected.M32 = -1.25d;
            expected.M33 = +1.0015d;
            expected.M34 = +1.0d;

            expected.M43 = -1.50225d;


            Matrix4x4D actual = Matrix4x4D.CreatePerspectiveOffCenterLeftHanded(left, right, bottom, top, zNearPlaneD, zFarPlaneD);
            Assert.True(MathHelper.Equal(expected, actual), $"{nameof(Matrix4x4D)}.{nameof(Matrix4x4D.CreatePerspectiveOffCenterLeftHanded)} did not return the expected value.");
        }

        // A test for CreatePerspectiveOffCenter (double, double, double, double, double, double)
        // CreatePerspectiveOffCenter test where nearPlaneDDistance is negative.
        [Fact]
        public void Matrix4x4DCreatePerspectiveOffCenterTest1()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                double left = 10.0d, right = 90.0d, bottom = 20.0d, top = 180.0d;
                Matrix4x4D actual = Matrix4x4D.CreatePerspectiveOffCenter(left, right, bottom, top, -1, 10);
            });
        }

        // A test for CreatePerspectiveOffCenter (double, double, double, double, double, double)
        // CreatePerspectiveOffCenter test where farPlaneDDistance is negative.
        [Fact]
        public void Matrix4x4DCreatePerspectiveOffCenterTest2()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                double left = 10.0d, right = 90.0d, bottom = 20.0d, top = 180.0d;
                Matrix4x4D actual = Matrix4x4D.CreatePerspectiveOffCenter(left, right, bottom, top, 1, -10);
            });
        }

        // A test for CreatePerspectiveOffCenter (double, double, double, double, double, double)
        // CreatePerspectiveOffCenter test where test where nearPlaneDDistance is larger than farPlaneDDistance.
        [Fact]
        public void Matrix4x4DCreatePerspectiveOffCenterTest3()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                double left = 10.0d, right = 90.0d, bottom = 20.0d, top = 180.0d;
                Matrix4x4D actual = Matrix4x4D.CreatePerspectiveOffCenter(left, right, bottom, top, 10, 1);
            });
        }

        // A test for Invert (Matrix4x4D)
        // Non invertible matrix - determinant is zero - singular matrix
        [Fact]
        public void Matrix4x4DInvertTest1()
        {
            Matrix4x4D a = new Matrix4x4D();
            a.M11 = 1.0d;
            a.M12 = 2.0d;
            a.M13 = 3.0d;
            a.M14 = 4.0d;
            a.M21 = 5.0d;
            a.M22 = 6.0d;
            a.M23 = 7.0d;
            a.M24 = 8.0d;
            a.M31 = 9.0d;
            a.M32 = 10.0d;
            a.M33 = 11.0d;
            a.M34 = 12.0d;
            a.M41 = 13.0d;
            a.M42 = 14.0d;
            a.M43 = 15.0d;
            a.M44 = 16.0d;

            double detA = a.GetDeterminant();
            Assert.True(MathHelper.Equal(detA, 0.0d), "Matrix4x4D.Invert did not return the expected value.");

            Matrix4x4D actual;
            Assert.False(Matrix4x4D.Invert(a, out actual));

            // all the elements in Actual is NaN
            Assert.True(
                double.IsNaN(actual.M11) && double.IsNaN(actual.M12) && double.IsNaN(actual.M13) && double.IsNaN(actual.M14) &&
                double.IsNaN(actual.M21) && double.IsNaN(actual.M22) && double.IsNaN(actual.M23) && double.IsNaN(actual.M24) &&
                double.IsNaN(actual.M31) && double.IsNaN(actual.M32) && double.IsNaN(actual.M33) && double.IsNaN(actual.M34) &&
                double.IsNaN(actual.M41) && double.IsNaN(actual.M42) && double.IsNaN(actual.M43) && double.IsNaN(actual.M44)
                , "Matrix4x4D.Invert did not return the expected value.");
        }

        // A test for Lerp (Matrix4x4D, Matrix4x4D, double)
        [Fact]
        public void Matrix4x4DLerpTest()
        {
            Matrix4x4D a = new Matrix4x4D();
            a.M11 = 11.0d;
            a.M12 = 12.0d;
            a.M13 = 13.0d;
            a.M14 = 14.0d;
            a.M21 = 21.0d;
            a.M22 = 22.0d;
            a.M23 = 23.0d;
            a.M24 = 24.0d;
            a.M31 = 31.0d;
            a.M32 = 32.0d;
            a.M33 = 33.0d;
            a.M34 = 34.0d;
            a.M41 = 41.0d;
            a.M42 = 42.0d;
            a.M43 = 43.0d;
            a.M44 = 44.0d;

            Matrix4x4D b = GenerateIncrementalMatrixNumber();

            double t = 0.5d;

            Matrix4x4D expected = new Matrix4x4D();
            expected.M11 = a.M11 + (b.M11 - a.M11) * t;
            expected.M12 = a.M12 + (b.M12 - a.M12) * t;
            expected.M13 = a.M13 + (b.M13 - a.M13) * t;
            expected.M14 = a.M14 + (b.M14 - a.M14) * t;

            expected.M21 = a.M21 + (b.M21 - a.M21) * t;
            expected.M22 = a.M22 + (b.M22 - a.M22) * t;
            expected.M23 = a.M23 + (b.M23 - a.M23) * t;
            expected.M24 = a.M24 + (b.M24 - a.M24) * t;

            expected.M31 = a.M31 + (b.M31 - a.M31) * t;
            expected.M32 = a.M32 + (b.M32 - a.M32) * t;
            expected.M33 = a.M33 + (b.M33 - a.M33) * t;
            expected.M34 = a.M34 + (b.M34 - a.M34) * t;

            expected.M41 = a.M41 + (b.M41 - a.M41) * t;
            expected.M42 = a.M42 + (b.M42 - a.M42) * t;
            expected.M43 = a.M43 + (b.M43 - a.M43) * t;
            expected.M44 = a.M44 + (b.M44 - a.M44) * t;

            Matrix4x4D actual;
            actual = Matrix4x4D.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), "Matrix4x4D.Lerp did not return the expected value.");
        }

        // A test for operator - (Matrix4x4D)
        [Fact]
        public void Matrix4x4DUnaryNegationTest()
        {
            Matrix4x4D a = GenerateIncrementalMatrixNumber();

            Matrix4x4D expected = new Matrix4x4D();
            expected.M11 = -1.0d;
            expected.M12 = -2.0d;
            expected.M13 = -3.0d;
            expected.M14 = -4.0d;
            expected.M21 = -5.0d;
            expected.M22 = -6.0d;
            expected.M23 = -7.0d;
            expected.M24 = -8.0d;
            expected.M31 = -9.0d;
            expected.M32 = -10.0d;
            expected.M33 = -11.0d;
            expected.M34 = -12.0d;
            expected.M41 = -13.0d;
            expected.M42 = -14.0d;
            expected.M43 = -15.0d;
            expected.M44 = -16.0d;

            Matrix4x4D actual = -a;
            Assert.True(MathHelper.Equal(expected, actual), "Matrix4x4D.operator - did not return the expected value.");
        }

        // A test for operator - (Matrix4x4D, Matrix4x4D)
        [Fact]
        public void Matrix4x4DSubtractionTest()
        {
            Matrix4x4D a = GenerateIncrementalMatrixNumber();
            Matrix4x4D b = GenerateIncrementalMatrixNumber(-8.0d);

            Matrix4x4D expected = new Matrix4x4D();
            expected.M11 = a.M11 - b.M11;
            expected.M12 = a.M12 - b.M12;
            expected.M13 = a.M13 - b.M13;
            expected.M14 = a.M14 - b.M14;
            expected.M21 = a.M21 - b.M21;
            expected.M22 = a.M22 - b.M22;
            expected.M23 = a.M23 - b.M23;
            expected.M24 = a.M24 - b.M24;
            expected.M31 = a.M31 - b.M31;
            expected.M32 = a.M32 - b.M32;
            expected.M33 = a.M33 - b.M33;
            expected.M34 = a.M34 - b.M34;
            expected.M41 = a.M41 - b.M41;
            expected.M42 = a.M42 - b.M42;
            expected.M43 = a.M43 - b.M43;
            expected.M44 = a.M44 - b.M44;

            Matrix4x4D actual = a - b;
            Assert.True(MathHelper.Equal(expected, actual), "Matrix4x4D.operator - did not return the expected value.");
        }

        // A test for operator * (Matrix4x4D, Matrix4x4D)
        [Fact]
        public void Matrix4x4DMultiplyTest1()
        {
            Matrix4x4D a = GenerateIncrementalMatrixNumber();
            Matrix4x4D b = GenerateIncrementalMatrixNumber(-8.0d);

            Matrix4x4D expected = new Matrix4x4D();
            expected.M11 = a.M11 * b.M11 + a.M12 * b.M21 + a.M13 * b.M31 + a.M14 * b.M41;
            expected.M12 = a.M11 * b.M12 + a.M12 * b.M22 + a.M13 * b.M32 + a.M14 * b.M42;
            expected.M13 = a.M11 * b.M13 + a.M12 * b.M23 + a.M13 * b.M33 + a.M14 * b.M43;
            expected.M14 = a.M11 * b.M14 + a.M12 * b.M24 + a.M13 * b.M34 + a.M14 * b.M44;

            expected.M21 = a.M21 * b.M11 + a.M22 * b.M21 + a.M23 * b.M31 + a.M24 * b.M41;
            expected.M22 = a.M21 * b.M12 + a.M22 * b.M22 + a.M23 * b.M32 + a.M24 * b.M42;
            expected.M23 = a.M21 * b.M13 + a.M22 * b.M23 + a.M23 * b.M33 + a.M24 * b.M43;
            expected.M24 = a.M21 * b.M14 + a.M22 * b.M24 + a.M23 * b.M34 + a.M24 * b.M44;

            expected.M31 = a.M31 * b.M11 + a.M32 * b.M21 + a.M33 * b.M31 + a.M34 * b.M41;
            expected.M32 = a.M31 * b.M12 + a.M32 * b.M22 + a.M33 * b.M32 + a.M34 * b.M42;
            expected.M33 = a.M31 * b.M13 + a.M32 * b.M23 + a.M33 * b.M33 + a.M34 * b.M43;
            expected.M34 = a.M31 * b.M14 + a.M32 * b.M24 + a.M33 * b.M34 + a.M34 * b.M44;

            expected.M41 = a.M41 * b.M11 + a.M42 * b.M21 + a.M43 * b.M31 + a.M44 * b.M41;
            expected.M42 = a.M41 * b.M12 + a.M42 * b.M22 + a.M43 * b.M32 + a.M44 * b.M42;
            expected.M43 = a.M41 * b.M13 + a.M42 * b.M23 + a.M43 * b.M33 + a.M44 * b.M43;
            expected.M44 = a.M41 * b.M14 + a.M42 * b.M24 + a.M43 * b.M34 + a.M44 * b.M44;

            Matrix4x4D actual = a * b;
            Assert.True(MathHelper.Equal(expected, actual), "Matrix4x4D.operator * did not return the expected value.");
        }

        // A test for operator * (Matrix4x4D, Matrix4x4D)
        // Multiply with identity matrix
        [Fact]
        public void Matrix4x4DMultiplyTest4()
        {
            Matrix4x4D a = new Matrix4x4D();
            a.M11 = 1.0d;
            a.M12 = 2.0d;
            a.M13 = 3.0d;
            a.M14 = 4.0d;
            a.M21 = 5.0d;
            a.M22 = -6.0d;
            a.M23 = 7.0d;
            a.M24 = -8.0d;
            a.M31 = 9.0d;
            a.M32 = 10.0d;
            a.M33 = 11.0d;
            a.M34 = 12.0d;
            a.M41 = 13.0d;
            a.M42 = -14.0d;
            a.M43 = 15.0d;
            a.M44 = -16.0d;

            Matrix4x4D b = new Matrix4x4D();
            b = Matrix4x4D.Identity;

            Matrix4x4D expected = a;
            Matrix4x4D actual = a * b;

            Assert.True(MathHelper.Equal(expected, actual), "Matrix4x4D.operator * did not return the expected value.");
        }

        // A test for operator + (Matrix4x4D, Matrix4x4D)
        [Fact]
        public void Matrix4x4DAdditionTest()
        {
            Matrix4x4D a = GenerateIncrementalMatrixNumber();
            Matrix4x4D b = GenerateIncrementalMatrixNumber(-8.0d);

            Matrix4x4D expected = new Matrix4x4D();
            expected.M11 = a.M11 + b.M11;
            expected.M12 = a.M12 + b.M12;
            expected.M13 = a.M13 + b.M13;
            expected.M14 = a.M14 + b.M14;
            expected.M21 = a.M21 + b.M21;
            expected.M22 = a.M22 + b.M22;
            expected.M23 = a.M23 + b.M23;
            expected.M24 = a.M24 + b.M24;
            expected.M31 = a.M31 + b.M31;
            expected.M32 = a.M32 + b.M32;
            expected.M33 = a.M33 + b.M33;
            expected.M34 = a.M34 + b.M34;
            expected.M41 = a.M41 + b.M41;
            expected.M42 = a.M42 + b.M42;
            expected.M43 = a.M43 + b.M43;
            expected.M44 = a.M44 + b.M44;

            Matrix4x4D actual = a + b;
            Assert.True(MathHelper.Equal(expected, actual), "Matrix4x4D.operator + did not return the expected value.");
        }

        // A test for Transpose (Matrix4x4D)
        [Fact]
        public void Matrix4x4DTransposeTest()
        {
            Matrix4x4D a = GenerateIncrementalMatrixNumber();

            Matrix4x4D expected = new Matrix4x4D();
            expected.M11 = a.M11;
            expected.M12 = a.M21;
            expected.M13 = a.M31;
            expected.M14 = a.M41;
            expected.M21 = a.M12;
            expected.M22 = a.M22;
            expected.M23 = a.M32;
            expected.M24 = a.M42;
            expected.M31 = a.M13;
            expected.M32 = a.M23;
            expected.M33 = a.M33;
            expected.M34 = a.M43;
            expected.M41 = a.M14;
            expected.M42 = a.M24;
            expected.M43 = a.M34;
            expected.M44 = a.M44;

            Matrix4x4D actual = Matrix4x4D.Transpose(a);
            Assert.True(MathHelper.Equal(expected, actual), "Matrix4x4D.Transpose did not return the expected value.");
        }

        // A test for Transpose (Matrix4x4D)
        // Transpose Identity matrix
        [Fact]
        public void Matrix4x4DTransposeTest1()
        {
            Matrix4x4D a = Matrix4x4D.Identity;
            Matrix4x4D expected = Matrix4x4D.Identity;

            Matrix4x4D actual = Matrix4x4D.Transpose(a);
            Assert.True(MathHelper.Equal(expected, actual), "Matrix4x4D.Transpose did not return the expected value.");
        }

        // A test for Matrix4x4D (QuaternionD)
        [Fact]
        public void Matrix4x4DFromQuaternionDTest1()
        {
            Vector3D axis = Vector3D.Normalize(new Vector3D(1.0d, 2.0d, 3.0d));
            QuaternionD q = QuaternionD.CreateFromAxisAngle(axis, MathHelper.ToRadians(30.0d));

            Matrix4x4D expected = new Matrix4x4D();
            expected.M11 = 0.875595033d;
            expected.M12 = 0.420031041d;
            expected.M13 = -0.2385524d;
            expected.M14 = 0.0d;

            expected.M21 = -0.38175258d;
            expected.M22 = 0.904303849d;
            expected.M23 = 0.1910483d;
            expected.M24 = 0.0d;

            expected.M31 = 0.295970082d;
            expected.M32 = -0.07621294d;
            expected.M33 = 0.952151954d;
            expected.M34 = 0.0d;

            expected.M41 = 0.0d;
            expected.M42 = 0.0d;
            expected.M43 = 0.0d;
            expected.M44 = 1.0d;

            Matrix4x4D target = Matrix4x4D.CreateFromQuaternion(q);
            Assert.True(MathHelper.Equal(expected, target), "Matrix4x4D.Matrix4x4D(QuaternionD) did not return the expected value.");
        }

        // A test for FromQuaternionD (Matrix4x4D)
        // Convert X axis rotation matrix
        [Fact]
        public void Matrix4x4DFromQuaternionDTest2()
        {
            for (double angle = 0.0d; angle < 720.0d; angle += 10.0d)
            {
                QuaternionD quat = QuaternionD.CreateFromAxisAngle(Vector3D.UnitX, angle);

                Matrix4x4D expected = Matrix4x4D.CreateRotationX(angle);
                Matrix4x4D actual = Matrix4x4D.CreateFromQuaternion(quat);
                Assert.True(MathHelper.Equal(expected, actual),
                    string.Format("QuaternionD.FromQuaternionD did not return the expected value. angle:{0}",
                    angle.ToString()));

                // make sure convert back to quaternion is same as we passed quaternion.
                QuaternionD q2 = QuaternionD.CreateFromRotationMatrix(actual);
                Assert.True(MathHelper.EqualRotation(quat, q2),
                    string.Format("QuaternionD.FromQuaternionD did not return the expected value. angle:{0}",
                    angle.ToString()));
            }
        }

        // A test for FromQuaternionD (Matrix4x4D)
        // Convert Y axis rotation matrix
        [Fact]
        public void Matrix4x4DFromQuaternionDTest3()
        {
            for (double angle = 0.0d; angle < 720.0d; angle += 10.0d)
            {
                QuaternionD quat = QuaternionD.CreateFromAxisAngle(Vector3D.UnitY, angle);

                Matrix4x4D expected = Matrix4x4D.CreateRotationY(angle);
                Matrix4x4D actual = Matrix4x4D.CreateFromQuaternion(quat);
                Assert.True(MathHelper.Equal(expected, actual),
                    string.Format("QuaternionD.FromQuaternionD did not return the expected value. angle:{0}",
                    angle.ToString()));

                // make sure convert back to quaternion is same as we passed quaternion.
                QuaternionD q2 = QuaternionD.CreateFromRotationMatrix(actual);
                Assert.True(MathHelper.EqualRotation(quat, q2),
                    string.Format("QuaternionD.FromQuaternionD did not return the expected value. angle:{0}",
                    angle.ToString()));
            }
        }

        // A test for FromQuaternionD (Matrix4x4D)
        // Convert Z axis rotation matrix
        [Fact]
        public void Matrix4x4DFromQuaternionDTest4()
        {
            for (double angle = 0.0d; angle < 720.0d; angle += 10.0d)
            {
                QuaternionD quat = QuaternionD.CreateFromAxisAngle(Vector3D.UnitZ, angle);

                Matrix4x4D expected = Matrix4x4D.CreateRotationZ(angle);
                Matrix4x4D actual = Matrix4x4D.CreateFromQuaternion(quat);
                Assert.True(MathHelper.Equal(expected, actual),
                    string.Format("QuaternionD.FromQuaternionD did not return the expected value. angle:{0}",
                    angle.ToString()));

                // make sure convert back to quaternion is same as we passed quaternion.
                QuaternionD q2 = QuaternionD.CreateFromRotationMatrix(actual);
                Assert.True(MathHelper.EqualRotation(quat, q2),
                    string.Format("QuaternionD.FromQuaternionD did not return the expected value. angle:{0}",
                    angle.ToString()));
            }
        }

        // A test for FromQuaternionD (Matrix4x4D)
        // Convert XYZ axis rotation matrix
        [Fact]
        public void Matrix4x4DFromQuaternionDTest5()
        {
            for (double angle = 0.0d; angle < 720.0d; angle += 10.0d)
            {
                QuaternionD quat =
                    QuaternionD.CreateFromAxisAngle(Vector3D.UnitZ, angle) *
                    QuaternionD.CreateFromAxisAngle(Vector3D.UnitY, angle) *
                    QuaternionD.CreateFromAxisAngle(Vector3D.UnitX, angle);

                Matrix4x4D expected =
                    Matrix4x4D.CreateRotationX(angle) *
                    Matrix4x4D.CreateRotationY(angle) *
                    Matrix4x4D.CreateRotationZ(angle);
                Matrix4x4D actual = Matrix4x4D.CreateFromQuaternion(quat);
                Assert.True(MathHelper.Equal(expected, actual),
                    string.Format("QuaternionD.FromQuaternionD did not return the expected value. angle:{0}",
                    angle.ToString()));

                // make sure convert back to quaternion is same as we passed quaternion.
                QuaternionD q2 = QuaternionD.CreateFromRotationMatrix(actual);
                Assert.True(MathHelper.EqualRotation(quat, q2),
                    string.Format("QuaternionD.FromQuaternionD did not return the expected value. angle:{0}",
                    angle.ToString()));
            }
        }

        // A test for ToString ()
        [Fact]
        public void Matrix4x4DToStringTest()
        {
            Matrix4x4D a = new Matrix4x4D();
            a.M11 = 11.0d;
            a.M12 = -12.0d;
            a.M13 = -13.3d;
            a.M14 = 14.4d;
            a.M21 = 21.0d;
            a.M22 = 22.0d;
            a.M23 = 23.0d;
            a.M24 = 24.0d;
            a.M31 = 31.0d;
            a.M32 = 32.0d;
            a.M33 = 33.0d;
            a.M34 = 34.0d;
            a.M41 = 41.0d;
            a.M42 = 42.0d;
            a.M43 = 43.0d;
            a.M44 = 44.0d;

            string expected = string.Format(CultureInfo.CurrentCulture,
                "{{ {{M11:{0} M12:{1} M13:{2} M14:{3}}} {{M21:{4} M22:{5} M23:{6} M24:{7}}} {{M31:{8} M32:{9} M33:{10} M34:{11}}} {{M41:{12} M42:{13} M43:{14} M44:{15}}} }}",
                    11.0d, -12.0d, -13.3d, 14.4d,
                    21.0d, 22.0d, 23.0d, 24.0d,
                    31.0d, 32.0d, 33.0d, 34.0d,
                    41.0d, 42.0d, 43.0d, 44.0d);

            string actual = a.ToString();
            Assert.Equal(expected, actual);
        }

        // A test for Add (Matrix4x4D, Matrix4x4D)
        [Fact]
        public void Matrix4x4DAddTest()
        {
            Matrix4x4D a = GenerateIncrementalMatrixNumber();
            Matrix4x4D b = GenerateIncrementalMatrixNumber(-8.0d);

            Matrix4x4D expected = new Matrix4x4D();
            expected.M11 = a.M11 + b.M11;
            expected.M12 = a.M12 + b.M12;
            expected.M13 = a.M13 + b.M13;
            expected.M14 = a.M14 + b.M14;
            expected.M21 = a.M21 + b.M21;
            expected.M22 = a.M22 + b.M22;
            expected.M23 = a.M23 + b.M23;
            expected.M24 = a.M24 + b.M24;
            expected.M31 = a.M31 + b.M31;
            expected.M32 = a.M32 + b.M32;
            expected.M33 = a.M33 + b.M33;
            expected.M34 = a.M34 + b.M34;
            expected.M41 = a.M41 + b.M41;
            expected.M42 = a.M42 + b.M42;
            expected.M43 = a.M43 + b.M43;
            expected.M44 = a.M44 + b.M44;

            Matrix4x4D actual = Matrix4x4D.Add(a, b);
            Assert.Equal(expected, actual);
        }

        // A test for Equals (object)
        [Fact]
        public void Matrix4x4DEqualsTest()
        {
            Matrix4x4D a = GenerateIncrementalMatrixNumber();
            Matrix4x4D b = GenerateIncrementalMatrixNumber();

            // case 1: compare between same values
            object obj = b;

            bool expected = true;
            bool actual = a.Equals(obj);
            Assert.Equal(expected, actual);

            // case 2: compare between different values
            b.M11 = 11.0d;
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
        public void Matrix4x4DGetHashCodeTest()
        {
            Matrix4x4D target = GenerateIncrementalMatrixNumber();

            int expected = HashCode.Combine(
                new Vector4D(target.M11, target.M12, target.M13, target.M14),
                new Vector4D(target.M21, target.M22, target.M23, target.M24),
                new Vector4D(target.M31, target.M32, target.M33, target.M34),
                new Vector4D(target.M41, target.M42, target.M43, target.M44)
            );

            int actual = target.GetHashCode();

            Assert.Equal(expected, actual);
        }

        // A test for Multiply (Matrix4x4D, Matrix4x4D)
        [Fact]
        public void Matrix4x4DMultiplyTest3()
        {
            Matrix4x4D a = GenerateIncrementalMatrixNumber();
            Matrix4x4D b = GenerateIncrementalMatrixNumber(-8.0d);

            Matrix4x4D expected = new Matrix4x4D();
            expected.M11 = a.M11 * b.M11 + a.M12 * b.M21 + a.M13 * b.M31 + a.M14 * b.M41;
            expected.M12 = a.M11 * b.M12 + a.M12 * b.M22 + a.M13 * b.M32 + a.M14 * b.M42;
            expected.M13 = a.M11 * b.M13 + a.M12 * b.M23 + a.M13 * b.M33 + a.M14 * b.M43;
            expected.M14 = a.M11 * b.M14 + a.M12 * b.M24 + a.M13 * b.M34 + a.M14 * b.M44;

            expected.M21 = a.M21 * b.M11 + a.M22 * b.M21 + a.M23 * b.M31 + a.M24 * b.M41;
            expected.M22 = a.M21 * b.M12 + a.M22 * b.M22 + a.M23 * b.M32 + a.M24 * b.M42;
            expected.M23 = a.M21 * b.M13 + a.M22 * b.M23 + a.M23 * b.M33 + a.M24 * b.M43;
            expected.M24 = a.M21 * b.M14 + a.M22 * b.M24 + a.M23 * b.M34 + a.M24 * b.M44;

            expected.M31 = a.M31 * b.M11 + a.M32 * b.M21 + a.M33 * b.M31 + a.M34 * b.M41;
            expected.M32 = a.M31 * b.M12 + a.M32 * b.M22 + a.M33 * b.M32 + a.M34 * b.M42;
            expected.M33 = a.M31 * b.M13 + a.M32 * b.M23 + a.M33 * b.M33 + a.M34 * b.M43;
            expected.M34 = a.M31 * b.M14 + a.M32 * b.M24 + a.M33 * b.M34 + a.M34 * b.M44;

            expected.M41 = a.M41 * b.M11 + a.M42 * b.M21 + a.M43 * b.M31 + a.M44 * b.M41;
            expected.M42 = a.M41 * b.M12 + a.M42 * b.M22 + a.M43 * b.M32 + a.M44 * b.M42;
            expected.M43 = a.M41 * b.M13 + a.M42 * b.M23 + a.M43 * b.M33 + a.M44 * b.M43;
            expected.M44 = a.M41 * b.M14 + a.M42 * b.M24 + a.M43 * b.M34 + a.M44 * b.M44;
            Matrix4x4D actual;
            actual = Matrix4x4D.Multiply(a, b);

            Assert.Equal(expected, actual);
        }

        // A test for Multiply (Matrix4x4D, double)
        [Fact]
        public void Matrix4x4DMultiplyTest5()
        {
            Matrix4x4D a = GenerateIncrementalMatrixNumber();
            Matrix4x4D expected = new Matrix4x4D(3, 6, 9, 12, 15, 18, 21, 24, 27, 30, 33, 36, 39, 42, 45, 48);
            Matrix4x4D actual = Matrix4x4D.Multiply(a, 3);

            Assert.Equal(expected, actual);
        }

        // A test for Multiply (Matrix4x4D, double)
        [Fact]
        public void Matrix4x4DMultiplyTest6()
        {
            Matrix4x4D a = GenerateIncrementalMatrixNumber();
            Matrix4x4D expected = new Matrix4x4D(3, 6, 9, 12, 15, 18, 21, 24, 27, 30, 33, 36, 39, 42, 45, 48);
            Matrix4x4D actual = a * 3;

            Assert.Equal(expected, actual);
        }

        // A test for Negate (Matrix4x4D)
        [Fact]
        public void Matrix4x4DNegateTest()
        {
            Matrix4x4D m = GenerateIncrementalMatrixNumber();

            Matrix4x4D expected = new Matrix4x4D();
            expected.M11 = -1.0d;
            expected.M12 = -2.0d;
            expected.M13 = -3.0d;
            expected.M14 = -4.0d;
            expected.M21 = -5.0d;
            expected.M22 = -6.0d;
            expected.M23 = -7.0d;
            expected.M24 = -8.0d;
            expected.M31 = -9.0d;
            expected.M32 = -10.0d;
            expected.M33 = -11.0d;
            expected.M34 = -12.0d;
            expected.M41 = -13.0d;
            expected.M42 = -14.0d;
            expected.M43 = -15.0d;
            expected.M44 = -16.0d;
            Matrix4x4D actual;

            actual = Matrix4x4D.Negate(m);
            Assert.Equal(expected, actual);
        }

        // A test for operator != (Matrix4x4D, Matrix4x4D)
        [Fact]
        public void Matrix4x4DInequalityTest()
        {
            Matrix4x4D a = GenerateIncrementalMatrixNumber();
            Matrix4x4D b = GenerateIncrementalMatrixNumber();

            // case 1: compare between same values
            bool expected = false;
            bool actual = a != b;
            Assert.Equal(expected, actual);

            // case 2: compare between different values
            b.M11 = 11.0d;
            expected = true;
            actual = a != b;
            Assert.Equal(expected, actual);
        }

        // A test for operator == (Matrix4x4D, Matrix4x4D)
        [Fact]
        public void Matrix4x4DEqualityTest()
        {
            Matrix4x4D a = GenerateIncrementalMatrixNumber();
            Matrix4x4D b = GenerateIncrementalMatrixNumber();

            // case 1: compare between same values
            bool expected = true;
            bool actual = a == b;
            Assert.Equal(expected, actual);

            // case 2: compare between different values
            b.M11 = 11.0d;
            expected = false;
            actual = a == b;
            Assert.Equal(expected, actual);
        }

        // A test for Subtract (Matrix4x4D, Matrix4x4D)
        [Fact]
        public void Matrix4x4DSubtractTest()
        {
            Matrix4x4D a = GenerateIncrementalMatrixNumber();
            Matrix4x4D b = GenerateIncrementalMatrixNumber(-8.0d);

            Matrix4x4D expected = new Matrix4x4D();
            expected.M11 = a.M11 - b.M11;
            expected.M12 = a.M12 - b.M12;
            expected.M13 = a.M13 - b.M13;
            expected.M14 = a.M14 - b.M14;
            expected.M21 = a.M21 - b.M21;
            expected.M22 = a.M22 - b.M22;
            expected.M23 = a.M23 - b.M23;
            expected.M24 = a.M24 - b.M24;
            expected.M31 = a.M31 - b.M31;
            expected.M32 = a.M32 - b.M32;
            expected.M33 = a.M33 - b.M33;
            expected.M34 = a.M34 - b.M34;
            expected.M41 = a.M41 - b.M41;
            expected.M42 = a.M42 - b.M42;
            expected.M43 = a.M43 - b.M43;
            expected.M44 = a.M44 - b.M44;

            Matrix4x4D actual = Matrix4x4D.Subtract(a, b);
            Assert.Equal(expected, actual);
        }

        private void CreateBillboardFact(Vector3D placeDirection, Vector3D cameraUpVector, Matrix4x4D expectedRotationRightHanded, Matrix4x4D expectedRotationLeftHanded)
        {
            Vector3D cameraPosition = new Vector3D(3.0d, 4.0d, 5.0d);
            Vector3D objectPosition = cameraPosition + placeDirection * 10.0d;
            Matrix4x4D expected = expectedRotationRightHanded * Matrix4x4D.CreateTranslation(objectPosition);
            Matrix4x4D actualRH = Matrix4x4D.CreateBillboard(objectPosition, cameraPosition, cameraUpVector, new Vector3D(0, 0, -1));
            Assert.True(MathHelper.Equal(expected, actualRH), "Matrix4x4D.CreateBillboard did not return the expected value.");

            placeDirection = InverseHandedness(placeDirection);
            cameraUpVector = InverseHandedness(cameraUpVector);

            cameraPosition = new Vector3D(3.0d, 4.0d, -5.0d);
            objectPosition = cameraPosition + placeDirection * 10.0d;
            expected = expectedRotationLeftHanded * Matrix4x4D.CreateTranslation(objectPosition);
            Matrix4x4D actualLH = Matrix4x4D.CreateBillboardLeftHanded(objectPosition, cameraPosition, cameraUpVector, Vector3D.UnitZ);
            Assert.True(MathHelper.Equal(expected, actualLH), "Matrix4x4D.CreateBillboardLeftHanded did not return the expected value.");

            AssertEqual(actualRH, InverseHandedness(actualLH), DefaultVarianceMatrix);
            AssertEqual(InverseHandedness(actualRH), actualLH, DefaultVarianceMatrix);
        }

        // A test for CreateBillboard (Vector3D, Vector3D, Vector3D, Vector3D?)
        // Place object at Forward side of camera on XZ-plane
        [Fact]
        public void Matrix4x4DCreateBillboardTest01()
        {
            // Object placed at Forward of camera. result must be same as 180 degrees rotate along y-axis.
            CreateBillboardFact(
                new Vector3D(0, 0, -1),
                Vector3D.UnitY,
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(180.0d)),
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(180.0d)));
        }

        // A test for CreateBillboard (Vector3D, Vector3D, Vector3D, Vector3D?)
        // Place object at Backward side of camera on XZ-plane
        [Fact]
        public void Matrix4x4DCreateBillboardTest02()
        {
            // Object placed at Backward of camera. This result must be same as 0 degrees rotate along y-axis.
            CreateBillboardFact(
                Vector3D.UnitZ,
                Vector3D.UnitY,
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(0)),
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(0)));
        }

        // A test for CreateBillboard (Vector3D, Vector3D, Vector3D, Vector3D?)
        // Place object at Right side of camera on XZ-plane
        [Fact]
        public void Matrix4x4DCreateBillboardTest03()
        {
            // Place object at Right side of camera. This result must be same as 90 degrees rotate along y-axis.
            CreateBillboardFact(
                Vector3D.UnitX,
                Vector3D.UnitY,
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(90)),
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(-90)));
        }

        // A test for CreateBillboard (Vector3D, Vector3D, Vector3D, Vector3D?)
        // Place object at Left side of camera on XZ-plane
        [Fact]
        public void Matrix4x4DCreateBillboardTest04()
        {
            // Place object at Left side of camera. This result must be same as -90 degrees rotate along y-axis.
            CreateBillboardFact(
                new Vector3D(-1, 0, 0),
                Vector3D.UnitY,
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(-90)),
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(90)));
        }

        // A test for CreateBillboard (Vector3D, Vector3D, Vector3D, Vector3D?)
        // Place object at Up side of camera on XY-plane
        [Fact]
        public void Matrix4x4DCreateBillboardTest05()
        {
            // Place object at Up side of camera. result must be same as 180 degrees rotate along z-axis after 90 degrees rotate along x-axis.
            CreateBillboardFact(
                Vector3D.UnitY,
                Vector3D.UnitZ,
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(90.0d)) * Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(180)),
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(-90.0d)) * Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(180)));
        }

        // A test for CreateBillboard (Vector3D, Vector3D, Vector3D, Vector3D?)
        // Place object at Down side of camera on XY-plane
        [Fact]
        public void Matrix4x4DCreateBillboardTest06()
        {
            // Place object at Down side of camera. result must be same as 0 degrees rotate along z-axis after 90 degrees rotate along x-axis.
            CreateBillboardFact(
                new Vector3D(0, -1, 0),
                Vector3D.UnitZ,
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(90.0d)) * Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(0)),
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(-90.0d)) * Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(0)));
        }

        // A test for CreateBillboard (Vector3D, Vector3D, Vector3D, Vector3D?)
        // Place object at Right side of camera on XY-plane
        [Fact]
        public void Matrix4x4DCreateBillboardTest07()
        {
            // Place object at Right side of camera. result must be same as 90 degrees rotate along z-axis after 90 degrees rotate along x-axis.
            CreateBillboardFact(
                Vector3D.UnitX,
                Vector3D.UnitZ,
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(90.0d)) * Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(90.0d)),
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(-90.0d)) * Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(90.0d)));
        }

        // A test for CreateBillboard (Vector3D, Vector3D, Vector3D, Vector3D?)
        // Place object at Left side of camera on XY-plane
        [Fact]
        public void Matrix4x4DCreateBillboardTest08()
        {
            // Place object at Left side of camera. result must be same as -90 degrees rotate along z-axis after 90 degrees rotate along x-axis.
            CreateBillboardFact(
                new Vector3D(-1, 0, 0),
                Vector3D.UnitZ,
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(90.0d)) * Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(-90.0d)),
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(-90.0d)) * Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(-90.0d)));
        }

        // A test for CreateBillboard (Vector3D, Vector3D, Vector3D, Vector3D?)
        // Place object at Up side of camera on YZ-plane
        [Fact]
        public void Matrix4x4DCreateBillboardTest09()
        {
            // Place object at Up side of camera. result must be same as -90 degrees rotate along x-axis after 90 degrees rotate along z-axis.
            CreateBillboardFact(
                Vector3D.UnitY,
                new Vector3D(-1, 0, 0),
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(90.0d)) * Matrix4x4D.CreateRotationX(MathHelper.ToRadians(-90.0d)),
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(90.0d)) * Matrix4x4D.CreateRotationX(MathHelper.ToRadians(90.0d)));
        }

        // A test for CreateBillboard (Vector3D, Vector3D, Vector3D, Vector3D?)
        // Place object at Down side of camera on YZ-plane
        [Fact]
        public void Matrix4x4DCreateBillboardTest10()
        {
            // Place object at Down side of camera. result must be same as 90 degrees rotate along x-axis after 90 degrees rotate along z-axis.
            CreateBillboardFact(
                new Vector3D(0, -1, 0),
                new Vector3D(-1, 0, 0),
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(90.0d)) * Matrix4x4D.CreateRotationX(MathHelper.ToRadians(90.0d)),
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(90.0d)) * Matrix4x4D.CreateRotationX(MathHelper.ToRadians(-90.0d)));
        }

        // A test for CreateBillboard (Vector3D, Vector3D, Vector3D, Vector3D?)
        // Place object at Forward side of camera on YZ-plane
        [Fact]
        public void Matrix4x4DCreateBillboardTest11()
        {
            // Place object at Forward side of camera. result must be same as 180 degrees rotate along x-axis after 90 degrees rotate along z-axis.
            CreateBillboardFact(
                new Vector3D(0, 0, -1),
                new Vector3D(-1, 0, 0),
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(90.0d)) * Matrix4x4D.CreateRotationX(MathHelper.ToRadians(180.0d)),
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(90.0d)) * Matrix4x4D.CreateRotationX(MathHelper.ToRadians(180.0d)));
        }

        // A test for CreateBillboard (Vector3D, Vector3D, Vector3D, Vector3D?)
        // Place object at Backward side of camera on YZ-plane
        [Fact]
        public void Matrix4x4DCreateBillboardTest12()
        {
            // Place object at Backward side of camera. result must be same as 0 degrees rotate along x-axis after 90 degrees rotate along z-axis.
            CreateBillboardFact(
                Vector3D.UnitZ,
                new Vector3D(-1, 0, 0),
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(90.0d)) * Matrix4x4D.CreateRotationX(MathHelper.ToRadians(0.0d)),
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(90.0d)) * Matrix4x4D.CreateRotationX(MathHelper.ToRadians(0.0d)));
        }

        // A test for CreateBillboard (Vector3D, Vector3D, Vector3D, Vector3D?)
        // Object and camera positions are too close and doesn't pass cameraForwardVector.
        [Fact]
        public void Matrix4x4DCreateBillboardTooCloseTest1()
        {
            Vector3D objectPosition = new Vector3D(3.0d, 4.0d, 5.0d);
            Vector3D cameraPosition = objectPosition;
            Vector3D cameraUpVector = Vector3D.UnitY;

            // Doesn't pass camera face direction. CreateBillboard uses new Vector3D(0, 0, -1) direction. Result must be same as 180 degrees rotate along y-axis.
            Matrix4x4D expected = Matrix4x4D.CreateRotationY(MathHelper.ToRadians(180.0d)) * Matrix4x4D.CreateTranslation(objectPosition);
            Matrix4x4D actualRH = Matrix4x4D.CreateBillboard(objectPosition, cameraPosition, cameraUpVector, Vector3D.UnitZ);
            Assert.True(MathHelper.Equal(expected, actualRH), "Matrix4x4D.CreateBillboard did not return the expected value.");

            objectPosition = new Vector3D(3.0d, 4.0d, -5.0d);
            cameraPosition = objectPosition;
            cameraUpVector = Vector3D.UnitY;

            expected = Matrix4x4D.CreateRotationY(MathHelper.ToRadians(180.0d)) * Matrix4x4D.CreateTranslation(objectPosition);
            Matrix4x4D actualLH = Matrix4x4D.CreateBillboardLeftHanded(objectPosition, cameraPosition, cameraUpVector, new Vector3D(0, 0, -1));
            Assert.True(MathHelper.Equal(expected, actualLH), "Matrix4x4D.CreateBillboardLeftHanded did not return the expected value.");

            AssertEqual(actualRH, InverseHandedness(actualLH), DefaultVarianceMatrix);
            AssertEqual(InverseHandedness(actualRH), actualLH, DefaultVarianceMatrix);
        }

        // A test for CreateBillboard (Vector3D, Vector3D, Vector3D, Vector3D?)
        // Object and camera positions are too close and passed cameraForwardVector.
        [Fact]
        public void Matrix4x4DCreateBillboardTooCloseTest2()
        {
            Vector3D objectPosition = new Vector3D(3.0d, 4.0d, 5.0d);
            Vector3D cameraPosition = objectPosition;
            Vector3D cameraUpVector = Vector3D.UnitY;

            // Passes Vector3D.Right as camera face direction. Result must be same as -90 degrees rotate along y-axis.
            Matrix4x4D expected = Matrix4x4D.CreateRotationY(MathHelper.ToRadians(-90.0d)) * Matrix4x4D.CreateTranslation(objectPosition);
            Matrix4x4D actualRH = Matrix4x4D.CreateBillboard(objectPosition, cameraPosition, cameraUpVector, Vector3D.UnitX);
            Assert.True(MathHelper.Equal(expected, actualRH), "Matrix4x4D.CreateBillboard did not return the expected value.");

            objectPosition = new Vector3D(3.0d, 4.0d, -5.0d);
            cameraPosition = objectPosition;
            cameraUpVector = Vector3D.UnitY;

            expected = Matrix4x4D.CreateRotationY(MathHelper.ToRadians(90.0d)) * Matrix4x4D.CreateTranslation(objectPosition);
            Matrix4x4D actualLH = Matrix4x4D.CreateBillboardLeftHanded(objectPosition, cameraPosition, cameraUpVector, Vector3D.UnitX);
            Assert.True(MathHelper.Equal(expected, actualLH), "Matrix4x4D.CreateBillboardLeftHanded did not return the expected value.");
        }

        private void CreateConstrainedBillboardFact(Vector3D placeDirection, Vector3D rotateAxis, Matrix4x4D expectedRotationRightHanded, Matrix4x4D expectedRotationLeftHanded)
        {
            Vector3D cameraPosition = new Vector3D(3.0d, 4.0d, 5.0d);
            Vector3D objectPosition = cameraPosition + placeDirection * 10.0d;
            Matrix4x4D expected = expectedRotationRightHanded * Matrix4x4D.CreateTranslation(objectPosition);
            Matrix4x4D actualRH = Matrix4x4D.CreateConstrainedBillboard(objectPosition, cameraPosition, rotateAxis, new Vector3D(0, 0, -1), new Vector3D(0, 0, -1));
            Assert.True(MathHelper.Equal(expected, actualRH), $"{nameof(Matrix4x4D.CreateConstrainedBillboard)} did not return the expected value.");

            // When you move camera along rotateAxis, result must be same.
            cameraPosition += rotateAxis * 10.0d;
            Matrix4x4D actualTranslatedUpRH = Matrix4x4D.CreateConstrainedBillboard(objectPosition, cameraPosition, rotateAxis, new Vector3D(0, 0, -1), new Vector3D(0, 0, -1));
            Assert.True(MathHelper.Equal(expected, actualTranslatedUpRH), $"{nameof(Matrix4x4D.CreateConstrainedBillboard)} did not return the expected value.");

            cameraPosition -= rotateAxis * 30.0d;
            Matrix4x4D actualTranslatedDownRH = Matrix4x4D.CreateConstrainedBillboard(objectPosition, cameraPosition, rotateAxis, new Vector3D(0, 0, -1), new Vector3D(0, 0, -1));
            Assert.True(MathHelper.Equal(expected, actualTranslatedDownRH), $"{nameof(Matrix4x4D.CreateConstrainedBillboard)} did not return the expected value.");

            placeDirection = InverseHandedness(placeDirection);
            rotateAxis = InverseHandedness(rotateAxis);

            cameraPosition = new Vector3D(3.0d, 4.0d, -5.0d);
            objectPosition = cameraPosition + placeDirection * 10.0d;
            expected = expectedRotationLeftHanded * Matrix4x4D.CreateTranslation(objectPosition);
            Matrix4x4D actualLH = Matrix4x4D.CreateConstrainedBillboardLeftHanded(objectPosition, cameraPosition, rotateAxis, new Vector3D(0, 0, -1), Vector3D.UnitZ);
            Assert.True(MathHelper.Equal(expected, actualLH), $"{nameof(Matrix4x4D.CreateConstrainedBillboardLeftHanded)} did not return the expected value.");

            // When you move camera along rotateAxis, result must be same.
            cameraPosition += rotateAxis * 10.0d;
            Matrix4x4D actualTranslatedUpLH = Matrix4x4D.CreateConstrainedBillboardLeftHanded(objectPosition, cameraPosition, rotateAxis, new Vector3D(0, 0, -1), Vector3D.UnitZ);
            Assert.True(MathHelper.Equal(expected, actualTranslatedUpLH), $"{nameof(Matrix4x4D.CreateConstrainedBillboardLeftHanded)} did not return the expected value.");

            cameraPosition -= rotateAxis * 30.0d;
            Matrix4x4D actualTranslatedDownLH = Matrix4x4D.CreateConstrainedBillboardLeftHanded(objectPosition, cameraPosition, rotateAxis, new Vector3D(0, 0, -1), Vector3D.UnitZ);
            Assert.True(MathHelper.Equal(expected, actualTranslatedDownLH), $"{nameof(Matrix4x4D.CreateConstrainedBillboardLeftHanded)} did not return the expected value.");

            AssertEqual(actualRH, InverseHandedness(actualLH), DefaultVarianceMatrix);
            AssertEqual(InverseHandedness(actualRH), actualLH, DefaultVarianceMatrix);

            AssertEqual(actualTranslatedUpRH, InverseHandedness(actualTranslatedUpLH), DefaultVarianceMatrix);
            AssertEqual(InverseHandedness(actualTranslatedUpRH), actualTranslatedUpLH, DefaultVarianceMatrix);

            AssertEqual(actualTranslatedDownRH, InverseHandedness(actualTranslatedDownLH), DefaultVarianceMatrix);
            AssertEqual(InverseHandedness(actualTranslatedDownRH), actualTranslatedDownLH, DefaultVarianceMatrix);
        }

        // A test for CreateConstrainedBillboard (Vector3D, Vector3D, Vector3D, Vector3D?)
        // Place object at Forward side of camera on XZ-plane
        [Fact]
        public void Matrix4x4DCreateConstrainedBillboardTest01()
        {
            // Object placed at Forward of camera. result must be same as 180 degrees rotate along y-axis.
            CreateConstrainedBillboardFact(
                new Vector3D(0, 0, -1),
                Vector3D.UnitY,
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(180.0d)),
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(180.0d)));
        }

        // A test for CreateConstrainedBillboard (Vector3D, Vector3D, Vector3D, Vector3D?)
        // Place object at Backward side of camera on XZ-plane
        [Fact]
        public void Matrix4x4DCreateConstrainedBillboardTest02()
        {
            // Object placed at Backward of camera. This result must be same as 0 degrees rotate along y-axis.
            CreateConstrainedBillboardFact(
                Vector3D.UnitZ,
                Vector3D.UnitY,
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(0)),
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(0)));
        }

        // A test for CreateConstrainedBillboard (Vector3D, Vector3D, Vector3D, Vector3D?)
        // Place object at Right side of camera on XZ-plane
        [Fact]
        public void Matrix4x4DCreateConstrainedBillboardTest03()
        {
            // Place object at Right side of camera. This result must be same as 90 degrees rotate along y-axis.
            CreateConstrainedBillboardFact(
                Vector3D.UnitX,
                Vector3D.UnitY,
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(90)),
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(-90)));
        }

        // A test for CreateConstrainedBillboard (Vector3D, Vector3D, Vector3D, Vector3D?)
        // Place object at Left side of camera on XZ-plane
        [Fact]
        public void Matrix4x4DCreateConstrainedBillboardTest04()
        {
            // Place object at Left side of camera. This result must be same as -90 degrees rotate along y-axis.
            CreateConstrainedBillboardFact(
                new Vector3D(-1, 0, 0),
                Vector3D.UnitY,
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(-90)),
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(90)));
        }

        // A test for CreateConstrainedBillboard (Vector3D, Vector3D, Vector3D, Vector3D?)
        // Place object at Up side of camera on XY-plane
        [Fact]
        public void Matrix4x4DCreateConstrainedBillboardTest05()
        {
            // Place object at Up side of camera. result must be same as 180 degrees rotate along z-axis after 90 degrees rotate along x-axis.
            CreateConstrainedBillboardFact(
                Vector3D.UnitY,
                Vector3D.UnitZ,
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(90.0d)) * Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(180)),
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(-90.0d)) * Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(180)));
        }

        // A test for CreateConstrainedBillboard (Vector3D, Vector3D, Vector3D, Vector3D?)
        // Place object at Down side of camera on XY-plane
        [Fact]
        public void Matrix4x4DCreateConstrainedBillboardTest06()
        {
            // Place object at Down side of camera. result must be same as 0 degrees rotate along z-axis after 90 degrees rotate along x-axis.
            CreateConstrainedBillboardFact(
                new Vector3D(0, -1, 0),
                Vector3D.UnitZ,
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(90.0d)) * Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(0)),
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(-90.0d)) * Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(0)));
        }

        // A test for CreateConstrainedBillboard (Vector3D, Vector3D, Vector3D, Vector3D?)
        // Place object at Right side of camera on XY-plane
        [Fact]
        public void Matrix4x4DCreateConstrainedBillboardTest07()
        {
            // Place object at Right side of camera. result must be same as 90 degrees rotate along z-axis after 90 degrees rotate along x-axis.
            CreateConstrainedBillboardFact(
                Vector3D.UnitX,
                Vector3D.UnitZ,
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(90.0d)) * Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(90.0d)),
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(-90.0d)) * Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(90.0d)));
        }

        // A test for CreateConstrainedBillboard (Vector3D, Vector3D, Vector3D, Vector3D?)
        // Place object at Left side of camera on XY-plane
        [Fact]
        public void Matrix4x4DCreateConstrainedBillboardTest08()
        {
            // Place object at Left side of camera. result must be same as -90 degrees rotate along z-axis after 90 degrees rotate along x-axis.
            CreateConstrainedBillboardFact(
                new Vector3D(-1, 0, 0),
                Vector3D.UnitZ,
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(90.0d)) * Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(-90.0d)),
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(-90.0d)) * Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(-90.0d)));
        }

        // A test for CreateConstrainedBillboard (Vector3D, Vector3D, Vector3D, Vector3D?)
        // Place object at Up side of camera on YZ-plane
        [Fact]
        public void Matrix4x4DCreateConstrainedBillboardTest09()
        {
            // Place object at Up side of camera. result must be same as -90 degrees rotate along x-axis after 90 degrees rotate along z-axis.
            CreateConstrainedBillboardFact(
                Vector3D.UnitY,
                new Vector3D(-1, 0, 0),
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(90.0d)) * Matrix4x4D.CreateRotationX(MathHelper.ToRadians(-90.0d)),
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(90.0d)) * Matrix4x4D.CreateRotationX(MathHelper.ToRadians(90.0d)));
        }

        // A test for CreateConstrainedBillboard (Vector3D, Vector3D, Vector3D, Vector3D?)
        // Place object at Down side of camera on YZ-plane
        [Fact]
        public void Matrix4x4DCreateConstrainedBillboardTest10()
        {
            // Place object at Down side of camera. result must be same as 90 degrees rotate along x-axis after 90 degrees rotate along z-axis.
            CreateConstrainedBillboardFact(
                new Vector3D(0, -1, 0),
                new Vector3D(-1, 0, 0),
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(90.0d)) * Matrix4x4D.CreateRotationX(MathHelper.ToRadians(90.0d)),
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(90.0d)) * Matrix4x4D.CreateRotationX(MathHelper.ToRadians(-90.0d)));
        }

        // A test for CreateConstrainedBillboard (Vector3D, Vector3D, Vector3D, Vector3D?)
        // Place object at Forward side of camera on YZ-plane
        [Fact]
        public void Matrix4x4DCreateConstrainedBillboardTest11()
        {
            // Place object at Forward side of camera. result must be same as 180 degrees rotate along x-axis after 90 degrees rotate along z-axis.
            CreateConstrainedBillboardFact(
                new Vector3D(0, 0, -1),
                new Vector3D(-1, 0, 0),
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(90.0d)) * Matrix4x4D.CreateRotationX(MathHelper.ToRadians(180.0d)),
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(90.0d)) * Matrix4x4D.CreateRotationX(MathHelper.ToRadians(180.0d)));
        }

        // A test for CreateConstrainedBillboard (Vector3D, Vector3D, Vector3D, Vector3D?)
        // Place object at Backward side of camera on YZ-plane
        [Fact]
        public void Matrix4x4DCreateConstrainedBillboardTest12()
        {
            // Place object at Backward side of camera. result must be same as 0 degrees rotate along x-axis after 90 degrees rotate along z-axis.
            CreateConstrainedBillboardFact(
                Vector3D.UnitZ,
                new Vector3D(-1, 0, 0),
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(90.0d)) * Matrix4x4D.CreateRotationX(MathHelper.ToRadians(0.0d)),
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(90.0d)) * Matrix4x4D.CreateRotationX(MathHelper.ToRadians(0.0d)));
        }

        // A test for CreateConstrainedBillboard (Vector3D, Vector3D, Vector3D, Vector3D?)
        // Object and camera positions are too close and doesn't pass cameraForwardVector.
        [Fact]
        public void Matrix4x4DCreateConstrainedBillboardTooCloseTest1()
        {
            Vector3D objectPosition = new Vector3D(3.0d, 4.0d, 5.0d);
            Vector3D cameraPosition = objectPosition;
            Vector3D cameraUpVector = Vector3D.UnitY;

            // Doesn't pass camera face direction. CreateConstrainedBillboard uses new Vector3D(0, 0, -1) direction. Result must be same as 180 degrees rotate along y-axis.
            Matrix4x4D expected = Matrix4x4D.CreateRotationY(MathHelper.ToRadians(180.0d)) * Matrix4x4D.CreateTranslation(objectPosition);
            Matrix4x4D actualRH = Matrix4x4D.CreateConstrainedBillboard(objectPosition, cameraPosition, cameraUpVector, Vector3D.UnitZ, new Vector3D(0, 0, -1));
            Assert.True(MathHelper.Equal(expected, actualRH), $"{nameof(Matrix4x4D.CreateConstrainedBillboard)} did not return the expected value.");

            objectPosition = new Vector3D(3.0d, 4.0d, -5.0d);
            cameraPosition = objectPosition;
            cameraUpVector = Vector3D.UnitY;

            expected = Matrix4x4D.CreateRotationY(MathHelper.ToRadians(180.0d)) * Matrix4x4D.CreateTranslation(objectPosition);
            Matrix4x4D actualLH = Matrix4x4D.CreateConstrainedBillboardLeftHanded(objectPosition, cameraPosition, cameraUpVector, new Vector3D(0, 0, -1), Vector3D.UnitZ);
            Assert.True(MathHelper.Equal(expected, actualLH), $"{nameof(Matrix4x4D.CreateConstrainedBillboardLeftHanded)} did not return the expected value.");

            AssertEqual(actualRH, InverseHandedness(actualLH), DefaultVarianceMatrix);
            AssertEqual(InverseHandedness(actualRH), actualLH, DefaultVarianceMatrix);
        }

        // A test for CreateConstrainedBillboard (Vector3D, Vector3D, Vector3D, Vector3D?)
        // Object and camera positions are too close and passed cameraForwardVector.
        [Fact]
        public void Matrix4x4DCreateConstrainedBillboardTooCloseTest2()
        {
            Vector3D objectPosition = new Vector3D(3.0d, 4.0d, 5.0d);
            Vector3D cameraPosition = objectPosition;
            Vector3D cameraUpVector = Vector3D.UnitY;

            // Passes Vector3D.Right as camera face direction. Result must be same as -90 degrees rotate along y-axis.
            Matrix4x4D expected = Matrix4x4D.CreateRotationY(MathHelper.ToRadians(-90.0d)) * Matrix4x4D.CreateTranslation(objectPosition);
            Matrix4x4D actualRH = Matrix4x4D.CreateConstrainedBillboard(objectPosition, cameraPosition, cameraUpVector, Vector3D.UnitX, new Vector3D(0, 0, -1));
            Assert.True(MathHelper.Equal(expected, actualRH), $"{nameof(Matrix4x4D.CreateConstrainedBillboard)} did not return the expected value.");

            objectPosition = new Vector3D(3.0d, 4.0d, -5.0d);
            cameraPosition = objectPosition;
            cameraUpVector = Vector3D.UnitY;

            expected = Matrix4x4D.CreateRotationY(MathHelper.ToRadians(90.0d)) * Matrix4x4D.CreateTranslation(objectPosition);
            Matrix4x4D actualLH = Matrix4x4D.CreateConstrainedBillboardLeftHanded(objectPosition, cameraPosition, cameraUpVector, Vector3D.UnitX, Vector3D.UnitZ);
            Assert.True(MathHelper.Equal(expected, actualLH), $"{nameof(Matrix4x4D.CreateConstrainedBillboardLeftHanded)} did not return the expected value.");

            AssertEqual(actualRH, InverseHandedness(actualLH), DefaultVarianceMatrix);
            AssertEqual(InverseHandedness(actualRH), actualLH, DefaultVarianceMatrix);
        }

        private static void Matrix4x4DCreateConstrainedBillboardAlongAxisFact(Vector3D rotateAxis, Vector3D cameraForward, Vector3D objectForward, Matrix4x4D expectedRotationRightHanded, Matrix4x4D expectedRotationLeftHanded)
        {
            // Place camera at up side of object.
            Vector3D objectPosition = new Vector3D(3.0d, 4.0d, 5.0d);
            Vector3D cameraPosition = objectPosition + rotateAxis * 10.0d;

            Matrix4x4D expected = expectedRotationRightHanded * Matrix4x4D.CreateTranslation(objectPosition);
            Matrix4x4D actualLH = Matrix4x4D.CreateConstrainedBillboard(objectPosition, cameraPosition, rotateAxis, cameraForward, objectForward);
            Assert.True(MathHelper.Equal(expected, actualLH), $"{nameof(Matrix4x4D.CreateConstrainedBillboard)} did not return the expected value.");

            rotateAxis = InverseHandedness(rotateAxis);
            cameraForward = InverseHandedness(cameraForward);
            objectForward = InverseHandedness(objectForward);

            objectPosition = new Vector3D(3.0d, 4.0d, -5.0d);
            cameraPosition = objectPosition + rotateAxis * 10.0d;

            expected = expectedRotationLeftHanded * Matrix4x4D.CreateTranslation(objectPosition);
            Matrix4x4D actualRH = Matrix4x4D.CreateConstrainedBillboardLeftHanded(objectPosition, cameraPosition, rotateAxis, cameraForward, objectForward);
            Assert.True(MathHelper.Equal(expected, actualRH), $"{nameof(Matrix4x4D.CreateConstrainedBillboardLeftHanded)} did not return the expected value.");

            AssertEqual(actualRH, InverseHandedness(actualLH), DefaultVarianceMatrix);
            AssertEqual(InverseHandedness(actualRH), actualLH, DefaultVarianceMatrix);
        }

        // A test for CreateConstrainedBillboard (Vector3D, Vector3D, Vector3D, Vector3D?)
        // Angle between rotateAxis and camera to object vector is too small. And use doesn't passed objectForwardVector parameter.
        [Fact]
        public void Matrix4x4DCreateConstrainedBillboardAlongAxisTest1()
        {
            // In this case, CreateConstrainedBillboard picks new Vector3D(0, 0, -1) as object forward vector.
            Matrix4x4DCreateConstrainedBillboardAlongAxisFact(
                Vector3D.UnitY, new Vector3D(0, 0, -1), new Vector3D(0, 0, -1),
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(180.0d)),
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(180.0d)));
        }

        // A test for CreateConstrainedBillboard (Vector3D, Vector3D, Vector3D, Vector3D?)
        // Angle between rotateAxis and camera to object vector is too small. And user doesn't passed objectForwardVector parameter.
        [Fact]
        public void Matrix4x4DCreateConstrainedBillboardAlongAxisTest2()
        {
            // In this case, CreateConstrainedBillboard picks new Vector3D(1, 0, 0) as object forward vector.
            Matrix4x4DCreateConstrainedBillboardAlongAxisFact(
                new Vector3D(0, 0, -1), new Vector3D(0, 0, -1), new Vector3D(0, 0, -1),
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(-90.0d)) * Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(-90.0d)),
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(90.0d)) * Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(-90.0d)));
        }

        // A test for CreateConstrainedBillboard (Vector3D, Vector3D, Vector3D, Vector3D?)
        // Angle between rotateAxis and camera to object vector is too small. And user passed correct objectForwardVector parameter.
        [Fact]
        public void Matrix4x4DCreateConstrainedBillboardAlongAxisTest3()
        {
            // User passes correct objectForwardVector.
            Matrix4x4DCreateConstrainedBillboardAlongAxisFact(
                Vector3D.UnitY, new Vector3D(0, 0, -1), new Vector3D(0, 0, -1),
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(180.0d)),
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(180.0d)));
        }

        // A test for CreateConstrainedBillboard (Vector3D, Vector3D, Vector3D, Vector3D?)
        // Angle between rotateAxis and camera to object vector is too small. And user passed incorrect objectForwardVector parameter.
        [Fact]
        public void Matrix4x4DCreateConstrainedBillboardAlongAxisTest4()
        {
            // User passes correct objectForwardVector.
            Matrix4x4DCreateConstrainedBillboardAlongAxisFact(
                Vector3D.UnitY, new Vector3D(0, 0, -1), Vector3D.UnitY,
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(180.0d)),
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(180.0d)));
        }

        // A test for CreateConstrainedBillboard (Vector3D, Vector3D, Vector3D, Vector3D?)
        // Angle between rotateAxis and camera to object vector is too small. And user passed incorrect objectForwardVector parameter.
        [Fact]
        public void Matrix4x4DCreateConstrainedBillboardAlongAxisTest5()
        {
            // In this case, CreateConstrainedBillboard picks Vector3D.Right as object forward vector.
            Matrix4x4DCreateConstrainedBillboardAlongAxisFact(
                new Vector3D(0, 0, -1), new Vector3D(0, 0, -1), new Vector3D(0, 0, -1),
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(-90.0d)) * Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(-90.0d)),
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(90.0d)) * Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(-90.0d)));
        }

        // A test for CreateScale (Vector3D)
        [Fact]
        public void Matrix4x4DCreateScaleTest1()
        {
            Vector3D scales = new Vector3D(2.0d, 3.0d, 4.0d);
            Matrix4x4D expected = new Matrix4x4D(
                2.0d, 0.0d, 0.0d, 0.0d,
                0.0d, 3.0d, 0.0d, 0.0d,
                0.0d, 0.0d, 4.0d, 0.0d,
                0.0d, 0.0d, 0.0d, 1.0d);
            Matrix4x4D actual = Matrix4x4D.CreateScale(scales);
            Assert.Equal(expected, actual);
        }

        // A test for CreateScale (Vector3D, Vector3D)
        [Fact]
        public void Matrix4x4DCreateScaleCenterTest1()
        {
            Vector3D scale = new Vector3D(3, 4, 5);
            Vector3D center = new Vector3D(23, 42, 666);

            Matrix4x4D scaleAroundZero = Matrix4x4D.CreateScale(scale, Vector3D.Zero);
            Matrix4x4D scaleAroundZeroExpected = Matrix4x4D.CreateScale(scale);
            Assert.True(MathHelper.Equal(scaleAroundZero, scaleAroundZeroExpected));

            Matrix4x4D scaleAroundCenter = Matrix4x4D.CreateScale(scale, center);
            Matrix4x4D scaleAroundCenterExpected = Matrix4x4D.CreateTranslation(-center) * Matrix4x4D.CreateScale(scale) * Matrix4x4D.CreateTranslation(center);
            Assert.True(MathHelper.Equal(scaleAroundCenter, scaleAroundCenterExpected));
        }

        // A test for CreateScale (double)
        [Fact]
        public void Matrix4x4DCreateScaleTest2()
        {
            double scale = 2.0d;
            Matrix4x4D expected = new Matrix4x4D(
                2.0d, 0.0d, 0.0d, 0.0d,
                0.0d, 2.0d, 0.0d, 0.0d,
                0.0d, 0.0d, 2.0d, 0.0d,
                0.0d, 0.0d, 0.0d, 1.0d);
            Matrix4x4D actual = Matrix4x4D.CreateScale(scale);
            Assert.Equal(expected, actual);
        }

        // A test for CreateScale (double, Vector3D)
        [Fact]
        public void Matrix4x4DCreateScaleCenterTest2()
        {
            double scale = 5;
            Vector3D center = new Vector3D(23, 42, 666);

            Matrix4x4D scaleAroundZero = Matrix4x4D.CreateScale(scale, Vector3D.Zero);
            Matrix4x4D scaleAroundZeroExpected = Matrix4x4D.CreateScale(scale);
            Assert.True(MathHelper.Equal(scaleAroundZero, scaleAroundZeroExpected));

            Matrix4x4D scaleAroundCenter = Matrix4x4D.CreateScale(scale, center);
            Matrix4x4D scaleAroundCenterExpected = Matrix4x4D.CreateTranslation(-center) * Matrix4x4D.CreateScale(scale) * Matrix4x4D.CreateTranslation(center);
            Assert.True(MathHelper.Equal(scaleAroundCenter, scaleAroundCenterExpected));
        }

        // A test for CreateScale (double, double, double)
        [Fact]
        public void Matrix4x4DCreateScaleTest3()
        {
            double xScale = 2.0d;
            double yScale = 3.0d;
            double zScale = 4.0d;
            Matrix4x4D expected = new Matrix4x4D(
                2.0d, 0.0d, 0.0d, 0.0d,
                0.0d, 3.0d, 0.0d, 0.0d,
                0.0d, 0.0d, 4.0d, 0.0d,
                0.0d, 0.0d, 0.0d, 1.0d);
            Matrix4x4D actual = Matrix4x4D.CreateScale(xScale, yScale, zScale);
            Assert.Equal(expected, actual);
        }

        // A test for CreateScale (double, double, double, Vector3D)
        [Fact]
        public void Matrix4x4DCreateScaleCenterTest3()
        {
            Vector3D scale = new Vector3D(3, 4, 5);
            Vector3D center = new Vector3D(23, 42, 666);

            Matrix4x4D scaleAroundZero = Matrix4x4D.CreateScale(scale.X, scale.Y, scale.Z, Vector3D.Zero);
            Matrix4x4D scaleAroundZeroExpected = Matrix4x4D.CreateScale(scale.X, scale.Y, scale.Z);
            Assert.True(MathHelper.Equal(scaleAroundZero, scaleAroundZeroExpected));

            Matrix4x4D scaleAroundCenter = Matrix4x4D.CreateScale(scale.X, scale.Y, scale.Z, center);
            Matrix4x4D scaleAroundCenterExpected = Matrix4x4D.CreateTranslation(-center) * Matrix4x4D.CreateScale(scale.X, scale.Y, scale.Z) * Matrix4x4D.CreateTranslation(center);
            Assert.True(MathHelper.Equal(scaleAroundCenter, scaleAroundCenterExpected));
        }

        // A test for CreateTranslation (Vector3D)
        [Fact]
        public void Matrix4x4DCreateTranslationTest1()
        {
            Vector3D position = new Vector3D(2.0d, 3.0d, 4.0d);
            Matrix4x4D expected = new Matrix4x4D(
                1.0d, 0.0d, 0.0d, 0.0d,
                0.0d, 1.0d, 0.0d, 0.0d,
                0.0d, 0.0d, 1.0d, 0.0d,
                2.0d, 3.0d, 4.0d, 1.0d);

            Matrix4x4D actual = Matrix4x4D.CreateTranslation(position);
            Assert.Equal(expected, actual);
        }

        // A test for CreateTranslation (double, double, double)
        [Fact]
        public void Matrix4x4DCreateTranslationTest2()
        {
            double xPosition = 2.0d;
            double yPosition = 3.0d;
            double zPosition = 4.0d;

            Matrix4x4D expected = new Matrix4x4D(
                1.0d, 0.0d, 0.0d, 0.0d,
                0.0d, 1.0d, 0.0d, 0.0d,
                0.0d, 0.0d, 1.0d, 0.0d,
                2.0d, 3.0d, 4.0d, 1.0d);

            Matrix4x4D actual = Matrix4x4D.CreateTranslation(xPosition, yPosition, zPosition);
            Assert.Equal(expected, actual);
        }

        // A test for Translation
        [Fact]
        public void Matrix4x4DTranslationTest()
        {
            Matrix4x4D a = GenerateTestMatrix();
            Matrix4x4D b = a;

            // Transformed vector that has same semantics of property must be same.
            Vector3D val = new Vector3D(a.M41, a.M42, a.M43);
            Assert.Equal(val, a.Translation);

            // Set value and get value must be same.
            val = new Vector3D(1.0d, 2.0d, 3.0d);
            a.Translation = val;
            Assert.Equal(val, a.Translation);

            // Make sure it only modifies expected value of matrix.
            Assert.True(
                a.M11 == b.M11 && a.M12 == b.M12 && a.M13 == b.M13 && a.M14 == b.M14 &&
                a.M21 == b.M21 && a.M22 == b.M22 && a.M23 == b.M23 && a.M24 == b.M24 &&
                a.M31 == b.M31 && a.M32 == b.M32 && a.M33 == b.M33 && a.M34 == b.M34 &&
                a.M41 != b.M41 && a.M42 != b.M42 && a.M43 != b.M43 && a.M44 == b.M44);
        }

        // A test for Equals (Matrix4x4D)
        [Fact]
        public void Matrix4x4DEqualsTest1()
        {
            Matrix4x4D a = GenerateIncrementalMatrixNumber();
            Matrix4x4D b = GenerateIncrementalMatrixNumber();

            // case 1: compare between same values
            bool expected = true;
            bool actual = a.Equals(b);
            Assert.Equal(expected, actual);

            // case 2: compare between different values
            b.M11 = 11.0d;
            expected = false;
            actual = a.Equals(b);
            Assert.Equal(expected, actual);
        }

        // A test for IsIdentity
        [Fact]
        public void Matrix4x4DIsIdentityTest()
        {
            Assert.True(Matrix4x4D.Identity.IsIdentity);
            Assert.True(new Matrix4x4D(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1).IsIdentity);
            Assert.False(new Matrix4x4D(0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1).IsIdentity);
            Assert.False(new Matrix4x4D(1, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1).IsIdentity);
            Assert.False(new Matrix4x4D(1, 0, 1, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1).IsIdentity);
            Assert.False(new Matrix4x4D(1, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1).IsIdentity);
            Assert.False(new Matrix4x4D(1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1).IsIdentity);
            Assert.False(new Matrix4x4D(1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1).IsIdentity);
            Assert.False(new Matrix4x4D(1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1).IsIdentity);
            Assert.False(new Matrix4x4D(1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 1, 0, 0, 0, 0, 1).IsIdentity);
            Assert.False(new Matrix4x4D(1, 0, 0, 0, 0, 1, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1).IsIdentity);
            Assert.False(new Matrix4x4D(1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1).IsIdentity);
            Assert.False(new Matrix4x4D(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1).IsIdentity);
            Assert.False(new Matrix4x4D(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 1).IsIdentity);
            Assert.False(new Matrix4x4D(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 1).IsIdentity);
            Assert.False(new Matrix4x4D(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 1, 0, 1).IsIdentity);
            Assert.False(new Matrix4x4D(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 1).IsIdentity);
            Assert.False(new Matrix4x4D(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0).IsIdentity);
        }

        // A test for Matrix4x4D (Matrix3x2D)
        [Fact]
        public void Matrix4x4DFrom3x2Test()
        {
            Matrix3x2D source = new Matrix3x2D(1, 2, 3, 4, 5, 6);
            Matrix4x4D result = new Matrix4x4D(source);

            Assert.Equal(source.M11, result.M11);
            Assert.Equal(source.M12, result.M12);
            Assert.Equal(0d, result.M13);
            Assert.Equal(0d, result.M14);

            Assert.Equal(source.M21, result.M21);
            Assert.Equal(source.M22, result.M22);
            Assert.Equal(0d, result.M23);
            Assert.Equal(0d, result.M24);

            Assert.Equal(0d, result.M31);
            Assert.Equal(0d, result.M32);
            Assert.Equal(1d, result.M33);
            Assert.Equal(0d, result.M34);

            Assert.Equal(source.M31, result.M41);
            Assert.Equal(source.M32, result.M42);
            Assert.Equal(0d, result.M43);
            Assert.Equal(1d, result.M44);
        }

        // A test for Matrix4x4D comparison involving NaN values
        [Fact]
        public void Matrix4x4DEqualsNaNTest()
        {
            Matrix4x4D a = new Matrix4x4D(double.NaN, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            Matrix4x4D b = new Matrix4x4D(0, double.NaN, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            Matrix4x4D c = new Matrix4x4D(0, 0, double.NaN, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            Matrix4x4D d = new Matrix4x4D(0, 0, 0, double.NaN, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            Matrix4x4D e = new Matrix4x4D(0, 0, 0, 0, double.NaN, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            Matrix4x4D f = new Matrix4x4D(0, 0, 0, 0, 0, double.NaN, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            Matrix4x4D g = new Matrix4x4D(0, 0, 0, 0, 0, 0, double.NaN, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            Matrix4x4D h = new Matrix4x4D(0, 0, 0, 0, 0, 0, 0, double.NaN, 0, 0, 0, 0, 0, 0, 0, 0);
            Matrix4x4D i = new Matrix4x4D(0, 0, 0, 0, 0, 0, 0, 0, double.NaN, 0, 0, 0, 0, 0, 0, 0);
            Matrix4x4D j = new Matrix4x4D(0, 0, 0, 0, 0, 0, 0, 0, 0, double.NaN, 0, 0, 0, 0, 0, 0);
            Matrix4x4D k = new Matrix4x4D(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, double.NaN, 0, 0, 0, 0, 0);
            Matrix4x4D l = new Matrix4x4D(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, double.NaN, 0, 0, 0, 0);
            Matrix4x4D m = new Matrix4x4D(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, double.NaN, 0, 0, 0);
            Matrix4x4D n = new Matrix4x4D(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, double.NaN, 0, 0);
            Matrix4x4D o = new Matrix4x4D(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, double.NaN, 0);
            Matrix4x4D p = new Matrix4x4D(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, double.NaN);

            Assert.False(a == new Matrix4x4D());
            Assert.False(b == new Matrix4x4D());
            Assert.False(c == new Matrix4x4D());
            Assert.False(d == new Matrix4x4D());
            Assert.False(e == new Matrix4x4D());
            Assert.False(f == new Matrix4x4D());
            Assert.False(g == new Matrix4x4D());
            Assert.False(h == new Matrix4x4D());
            Assert.False(i == new Matrix4x4D());
            Assert.False(j == new Matrix4x4D());
            Assert.False(k == new Matrix4x4D());
            Assert.False(l == new Matrix4x4D());
            Assert.False(m == new Matrix4x4D());
            Assert.False(n == new Matrix4x4D());
            Assert.False(o == new Matrix4x4D());
            Assert.False(p == new Matrix4x4D());

            Assert.True(a != new Matrix4x4D());
            Assert.True(b != new Matrix4x4D());
            Assert.True(c != new Matrix4x4D());
            Assert.True(d != new Matrix4x4D());
            Assert.True(e != new Matrix4x4D());
            Assert.True(f != new Matrix4x4D());
            Assert.True(g != new Matrix4x4D());
            Assert.True(h != new Matrix4x4D());
            Assert.True(i != new Matrix4x4D());
            Assert.True(j != new Matrix4x4D());
            Assert.True(k != new Matrix4x4D());
            Assert.True(l != new Matrix4x4D());
            Assert.True(m != new Matrix4x4D());
            Assert.True(n != new Matrix4x4D());
            Assert.True(o != new Matrix4x4D());
            Assert.True(p != new Matrix4x4D());

            Assert.False(a.Equals(new Matrix4x4D()));
            Assert.False(b.Equals(new Matrix4x4D()));
            Assert.False(c.Equals(new Matrix4x4D()));
            Assert.False(d.Equals(new Matrix4x4D()));
            Assert.False(e.Equals(new Matrix4x4D()));
            Assert.False(f.Equals(new Matrix4x4D()));
            Assert.False(g.Equals(new Matrix4x4D()));
            Assert.False(h.Equals(new Matrix4x4D()));
            Assert.False(i.Equals(new Matrix4x4D()));
            Assert.False(j.Equals(new Matrix4x4D()));
            Assert.False(k.Equals(new Matrix4x4D()));
            Assert.False(l.Equals(new Matrix4x4D()));
            Assert.False(m.Equals(new Matrix4x4D()));
            Assert.False(n.Equals(new Matrix4x4D()));
            Assert.False(o.Equals(new Matrix4x4D()));
            Assert.False(p.Equals(new Matrix4x4D()));

            Assert.False(a.IsIdentity);
            Assert.False(b.IsIdentity);
            Assert.False(c.IsIdentity);
            Assert.False(d.IsIdentity);
            Assert.False(e.IsIdentity);
            Assert.False(f.IsIdentity);
            Assert.False(g.IsIdentity);
            Assert.False(h.IsIdentity);
            Assert.False(i.IsIdentity);
            Assert.False(j.IsIdentity);
            Assert.False(k.IsIdentity);
            Assert.False(l.IsIdentity);
            Assert.False(m.IsIdentity);
            Assert.False(n.IsIdentity);
            Assert.False(o.IsIdentity);
            Assert.False(p.IsIdentity);

            Assert.True(a.Equals(a));
            Assert.True(b.Equals(b));
            Assert.True(c.Equals(c));
            Assert.True(d.Equals(d));
            Assert.True(e.Equals(e));
            Assert.True(f.Equals(f));
            Assert.True(g.Equals(g));
            Assert.True(h.Equals(h));
            Assert.True(i.Equals(i));
            Assert.True(j.Equals(j));
            Assert.True(k.Equals(k));
            Assert.True(l.Equals(l));
            Assert.True(m.Equals(m));
            Assert.True(n.Equals(n));
            Assert.True(o.Equals(o));
            Assert.True(p.Equals(p));
        }

        // A test to make sure these types are blittable directly into GPU buffer memory layouts
        [Fact]
        public unsafe void Matrix4x4DSizeofTest()
        {
            Assert.Equal(128, sizeof(Matrix4x4D));
            Assert.Equal(256, sizeof(Matrix4x4D_2x));
            Assert.Equal(136, sizeof(Matrix4x4DPlusDouble));
            Assert.Equal(272, sizeof(Matrix4x4DPlusDouble_2x));
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Matrix4x4D_2x
        {
            private Matrix4x4D _a;
            private Matrix4x4D _b;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Matrix4x4DPlusDouble
        {
            private Matrix4x4D _v;
            private double _f;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Matrix4x4DPlusDouble_2x
        {
            private Matrix4x4DPlusDouble _a;
            private Matrix4x4DPlusDouble _b;
        }

        // A test to make sure the fields are laid out how we expect
        [Fact]
        public unsafe void Matrix4x4DFieldOffsetTest()
        {
            Matrix4x4D mat = new Matrix4x4D();

            double* basePtr = &mat.M11; // Take address of first element
            Matrix4x4D* matPtr = &mat; // Take address of whole matrix

            Assert.Equal(new IntPtr(basePtr), new IntPtr(matPtr));

            Assert.Equal(new IntPtr(basePtr + 0), new IntPtr(&mat.M11));
            Assert.Equal(new IntPtr(basePtr + 1), new IntPtr(&mat.M12));
            Assert.Equal(new IntPtr(basePtr + 2), new IntPtr(&mat.M13));
            Assert.Equal(new IntPtr(basePtr + 3), new IntPtr(&mat.M14));

            Assert.Equal(new IntPtr(basePtr + 4), new IntPtr(&mat.M21));
            Assert.Equal(new IntPtr(basePtr + 5), new IntPtr(&mat.M22));
            Assert.Equal(new IntPtr(basePtr + 6), new IntPtr(&mat.M23));
            Assert.Equal(new IntPtr(basePtr + 7), new IntPtr(&mat.M24));

            Assert.Equal(new IntPtr(basePtr + 8), new IntPtr(&mat.M31));
            Assert.Equal(new IntPtr(basePtr + 9), new IntPtr(&mat.M32));
            Assert.Equal(new IntPtr(basePtr + 10), new IntPtr(&mat.M33));
            Assert.Equal(new IntPtr(basePtr + 11), new IntPtr(&mat.M34));

            Assert.Equal(new IntPtr(basePtr + 12), new IntPtr(&mat.M41));
            Assert.Equal(new IntPtr(basePtr + 13), new IntPtr(&mat.M42));
            Assert.Equal(new IntPtr(basePtr + 14), new IntPtr(&mat.M43));
            Assert.Equal(new IntPtr(basePtr + 15), new IntPtr(&mat.M44));
        }

        [Fact]
        public void PerspectiveFarPlaneDAtInfinityTest()
        {
            var nearPlaneDDistance = 0.125d;
            var m = Matrix4x4D.CreatePerspective(1.0d, 1.0d, nearPlaneDDistance, double.PositiveInfinity);
            Assert.Equal(-1.0d, m.M33);
            Assert.Equal(-nearPlaneDDistance, m.M43);
        }

        [Fact]
        public void PerspectiveFieldOfViewFarPlaneDAtInfinityTest()
        {
            var nearPlaneDDistance = 0.125d;
            var m = Matrix4x4D.CreatePerspectiveFieldOfView(MathHelper.ToRadians(60.0d), 1.5d, nearPlaneDDistance, double.PositiveInfinity);
            Assert.Equal(-1.0d, m.M33);
            Assert.Equal(-nearPlaneDDistance, m.M43);
        }

        [Fact]
        public void PerspectiveOffCenterFarPlaneDAtInfinityTest()
        {
            var nearPlaneDDistance = 0.125d;
            var m = Matrix4x4D.CreatePerspectiveOffCenter(0.0d, 0.0d, 1.0d, 1.0d, nearPlaneDDistance, double.PositiveInfinity);
            Assert.Equal(-1.0d, m.M33);
            Assert.Equal(-nearPlaneDDistance, m.M43);
        }

        [Fact]
        public void Matrix4x4DCreateBroadcastScalarTest()
        {
            Matrix4x4D a = Matrix4x4D.Create(double.Pi);

            Assert.Equal(Vector4D.Create(double.Pi), a.X);
            Assert.Equal(Vector4D.Create(double.Pi), a.Y);
            Assert.Equal(Vector4D.Create(double.Pi), a.Z);
            Assert.Equal(Vector4D.Create(double.Pi), a.W);
        }

        [Fact]
        public void Matrix4x4DCreateBroadcastVectorTest()
        {
            Matrix4x4D a = Matrix4x4D.Create(Vector4D.Create(double.Pi, double.E, double.PositiveInfinity, double.NegativeInfinity));

            Assert.Equal(Vector4D.Create(double.Pi, double.E, double.PositiveInfinity, double.NegativeInfinity), a.X);
            Assert.Equal(Vector4D.Create(double.Pi, double.E, double.PositiveInfinity, double.NegativeInfinity), a.Y);
            Assert.Equal(Vector4D.Create(double.Pi, double.E, double.PositiveInfinity, double.NegativeInfinity), a.Z);
            Assert.Equal(Vector4D.Create(double.Pi, double.E, double.PositiveInfinity, double.NegativeInfinity), a.W);
        }

        [Fact]
        public void Matrix4x4DCreateVectorsTest()
        {
            Matrix4x4D a = Matrix4x4D.Create(
                Vector4D.Create(11.0d, 12.0d, 13.0d, 14.0d),
                Vector4D.Create(21.0d, 22.0d, 23.0d, 24.0d),
                Vector4D.Create(31.0d, 32.0d, 33.0d, 34.0d),
                Vector4D.Create(41.0d, 42.0d, 43.0d, 44.0d)
            );

            Assert.Equal(Vector4D.Create(11.0d, 12.0d, 13.0d, 14.0d), a.X);
            Assert.Equal(Vector4D.Create(21.0d, 22.0d, 23.0d, 24.0d), a.Y);
            Assert.Equal(Vector4D.Create(31.0d, 32.0d, 33.0d, 34.0d), a.Z);
            Assert.Equal(Vector4D.Create(41.0d, 42.0d, 43.0d, 44.0d), a.W);
        }

        [Fact]
        public void Matrix4x4DGetElementTest()
        {
            Matrix4x4D a = GenerateTestMatrix();

            Assert.Equal(a.M11, a.X.X);
            Assert.Equal(a.M11, a[0, 0]);
            Assert.Equal(a.M11, a.GetElement(0, 0));

            Assert.Equal(a.M12, a.X.Y);
            Assert.Equal(a.M12, a[0, 1]);
            Assert.Equal(a.M12, a.GetElement(0, 1));

            Assert.Equal(a.M13, a.X.Z);
            Assert.Equal(a.M13, a[0, 2]);
            Assert.Equal(a.M13, a.GetElement(0, 2));

            Assert.Equal(a.M14, a.X.W);
            Assert.Equal(a.M14, a[0, 3]);
            Assert.Equal(a.M14, a.GetElement(0, 3));

            Assert.Equal(a.M21, a.Y.X);
            Assert.Equal(a.M21, a[1, 0]);
            Assert.Equal(a.M21, a.GetElement(1, 0));

            Assert.Equal(a.M22, a.Y.Y);
            Assert.Equal(a.M22, a[1, 1]);
            Assert.Equal(a.M22, a.GetElement(1, 1));

            Assert.Equal(a.M23, a.Y.Z);
            Assert.Equal(a.M23, a[1, 2]);
            Assert.Equal(a.M23, a.GetElement(1, 2));

            Assert.Equal(a.M24, a.Y.W);
            Assert.Equal(a.M24, a[1, 3]);
            Assert.Equal(a.M24, a.GetElement(1, 3));

            Assert.Equal(a.M31, a.Z.X);
            Assert.Equal(a.M31, a[2, 0]);
            Assert.Equal(a.M31, a.GetElement(2, 0));

            Assert.Equal(a.M32, a.Z.Y);
            Assert.Equal(a.M32, a[2, 1]);
            Assert.Equal(a.M32, a.GetElement(2, 1));

            Assert.Equal(a.M33, a.Z.Z);
            Assert.Equal(a.M33, a[2, 2]);
            Assert.Equal(a.M33, a.GetElement(2, 2));

            Assert.Equal(a.M34, a.Z.W);
            Assert.Equal(a.M34, a[2, 3]);
            Assert.Equal(a.M34, a.GetElement(2, 3));

            Assert.Equal(a.M41, a.W.X);
            Assert.Equal(a.M41, a[3, 0]);
            Assert.Equal(a.M41, a.GetElement(3, 0));

            Assert.Equal(a.M42, a.W.Y);
            Assert.Equal(a.M42, a[3, 1]);
            Assert.Equal(a.M42, a.GetElement(3, 1));

            Assert.Equal(a.M43, a.W.Z);
            Assert.Equal(a.M43, a[3, 2]);
            Assert.Equal(a.M43, a.GetElement(3, 2));

            Assert.Equal(a.M44, a.W.W);
            Assert.Equal(a.M44, a[3, 3]);
            Assert.Equal(a.M44, a.GetElement(3, 3));
        }

        [Fact]
        public void Matrix4x4DGetRowTest()
        {
            Matrix4x4D a = GenerateTestMatrix();

            Vector4D vx = new Vector4D(a.M11, a.M12, a.M13, a.M14);
            Assert.Equal(vx, a.X);
            Assert.Equal(vx, a[0]);
            Assert.Equal(vx, a.GetRow(0));

            Vector4D vy = new Vector4D(a.M21, a.M22, a.M23, a.M24);
            Assert.Equal(vy, a.Y);
            Assert.Equal(vy, a[1]);
            Assert.Equal(vy, a.GetRow(1));

            Vector4D vz = new Vector4D(a.M31, a.M32, a.M33, a.M34);
            Assert.Equal(vz, a.Z);
            Assert.Equal(vz, a[2]);
            Assert.Equal(vz, a.GetRow(2));

            Vector4D vw = new Vector4D(a.M41, a.M42, a.M43, a.M44);
            Assert.Equal(vw, a.W);
            Assert.Equal(vw, a[3]);
            Assert.Equal(vw, a.GetRow(3));
        }

        [Fact]
        public void Matrix4x4DWithElementTest()
        {
            Matrix4x4D a = Matrix4x4D.Identity;

            a[0, 0] = 11.0d;
            Assert.Equal(11.5d, a.WithElement(0, 0, 11.5d).M11);
            Assert.Equal(11.0d, a.M11);

            a[0, 1] = 12.0d;
            Assert.Equal(12.5d, a.WithElement(0, 1, 12.5d).M12);
            Assert.Equal(12.0d, a.M12);

            a[0, 2] = 13.0d;
            Assert.Equal(13.5d, a.WithElement(0, 2, 13.5d).M13);
            Assert.Equal(13.0d, a.M13);

            a[0, 3] = 14.0d;
            Assert.Equal(14.5d, a.WithElement(0, 3, 14.5d).M14);
            Assert.Equal(14.0d, a.M14);

            a[1, 0] = 21.0d;
            Assert.Equal(21.5d, a.WithElement(1, 0, 21.5d).M21);
            Assert.Equal(21.0d, a.M21);

            a[1, 1] = 22.0d;
            Assert.Equal(22.5d, a.WithElement(1, 1, 22.5d).M22);
            Assert.Equal(22.0d, a.M22);

            a[1, 2] = 23.0d;
            Assert.Equal(23.5d, a.WithElement(1, 2, 23.5d).M23);
            Assert.Equal(23.0d, a.M23);

            a[1, 3] = 24.0d;
            Assert.Equal(24.5d, a.WithElement(1, 3, 24.5d).M24);
            Assert.Equal(24.0d, a.M24);

            a[2, 0] = 31.0d;
            Assert.Equal(31.5d, a.WithElement(2, 0, 31.5d).M31);
            Assert.Equal(31.0d, a.M31);

            a[2, 1] = 32.0d;
            Assert.Equal(32.5d, a.WithElement(2, 1, 32.5d).M32);
            Assert.Equal(32.0d, a.M32);

            a[2, 2] = 33.0d;
            Assert.Equal(33.5d, a.WithElement(2, 2, 33.5d).M33);
            Assert.Equal(33.0d, a.M33);

            a[2, 3] = 34.0d;
            Assert.Equal(34.5d, a.WithElement(2, 3, 34.5d).M34);
            Assert.Equal(34.0d, a.M34);

            a[3, 0] = 41.0d;
            Assert.Equal(41.5d, a.WithElement(3, 0, 41.5d).M41);
            Assert.Equal(41.0d, a.M41);

            a[3, 1] = 42.0d;
            Assert.Equal(42.5d, a.WithElement(3, 1, 42.5d).M42);
            Assert.Equal(42.0d, a.M42);

            a[3, 2] = 43.0d;
            Assert.Equal(43.5d, a.WithElement(3, 2, 43.5d).M43);
            Assert.Equal(43.0d, a.M43);

            a[3, 3] = 44.0d;
            Assert.Equal(44.5d, a.WithElement(3, 3, 44.5d).M44);
            Assert.Equal(44.0d, a.M44);
        }

        [Fact]
        public void Matrix4x4DWithRowTest()
        {
            Matrix4x4D a = Matrix4x4D.Identity;

            a[0] = Vector4D.Create(11.0d, 12.0d, 13.0d, 14.0d);
            Assert.Equal(Vector4D.Create(11.5d, 12.5d, 13.5d, 14.5d), a.WithRow(0, Vector4D.Create(11.5d, 12.5d, 13.5d, 14.5d)).X);
            Assert.Equal(Vector4D.Create(11.0d, 12.0d, 13.0d, 14.0d), a.X);

            a[1] = Vector4D.Create(21.0d, 22.0d, 23.0d, 24.0d);
            Assert.Equal(Vector4D.Create(21.5d, 22.5d, 23.5d, 24.5d), a.WithRow(1, Vector4D.Create(21.5d, 22.5d, 23.5d, 24.5d)).Y);
            Assert.Equal(Vector4D.Create(21.0d, 22.0d, 23.0d, 24.0d), a.Y);

            a[2] = Vector4D.Create(31.0d, 32.0d, 33.0d, 34.0d);
            Assert.Equal(Vector4D.Create(31.5d, 32.5d, 33.5d, 34.5d), a.WithRow(2, Vector4D.Create(31.5d, 32.5d, 33.5d, 34.5d)).Z);
            Assert.Equal(Vector4D.Create(31.0d, 32.0d, 33.0d, 34.0d), a.Z);

            a[3] = Vector4D.Create(41.0d, 42.0d, 43.0d, 44.0d);
            Assert.Equal(Vector4D.Create(41.5d, 42.5d, 43.5d, 44.5d), a.WithRow(3, Vector4D.Create(41.5d, 42.5d, 43.5d, 44.5d)).W);
            Assert.Equal(Vector4D.Create(41.0d, 42.0d, 43.0d, 44.0d), a.W);
        }
    }
}
