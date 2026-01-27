// Copyright (c) Andrew Pollard, .NET Foundation and Contributors.
// Licensed under the MIT license - see README.md for details.

using System.Globalization;
using System.Runtime.InteropServices;
using Xunit;

namespace Invicta.Numerics.Tests
{
    public sealed class QuaternionDDTests
    {
        // A test for Dot (QuaternionD, QuaternionD)
        [Fact]
        public void QuaternionDDotTest()
        {
            QuaternionD a = new QuaternionD(1.0d, 2.0d, 3.0d, 4.0d);
            QuaternionD b = new QuaternionD(5.0d, 6.0d, 7.0d, 8.0d);

            double expected = 70.0d;
            double actual;

            actual = QuaternionD.Dot(a, b);
            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.Dot did not return the expected value: expected {expected} actual {actual}");
        }

        [Theory]
        [InlineData(0.0d, 1.0d, 0.0d, 1.0d)]
        [InlineData(1.0d, 0.0d, 1.0d, 0.0d)]
        [InlineData(3.1434343d, 1.1234123d, 0.1234123d, -0.1234123d)]
        [InlineData(1.0000001d, 0.0000001d, 2.0000001d, 0.0000002d)]
        public void QuaternionDIndexerGetTest(double x, double y, double z, double w)
        {
            var quaternion = new QuaternionD(x, y, z, w);

            Assert.Equal(x, quaternion[0]);
            Assert.Equal(y, quaternion[1]);
            Assert.Equal(z, quaternion[2]);
            Assert.Equal(w, quaternion[3]);
        }

        [Theory]
        [InlineData(0.0d, 1.0d, 0.0d, 1.0d)]
        [InlineData(1.0d, 0.0d, 1.0d, 0.0d)]
        [InlineData(3.1434343d, 1.1234123d, 0.1234123d, -0.1234123d)]
        [InlineData(1.0000001d, 0.0000001d, 2.0000001d, 0.0000002d)]
        public void QuaternionDIndexerSetTest(double x, double y, double z, double w)
        {
            var quaternion = new QuaternionD(0.0d, 0.0d, 0.0d, 0.0d);

            quaternion[0] = x;
            quaternion[1] = y;
            quaternion[2] = z;
            quaternion[3] = w;

            Assert.Equal(x, quaternion[0]);
            Assert.Equal(y, quaternion[1]);
            Assert.Equal(z, quaternion[2]);
            Assert.Equal(w, quaternion[3]);
        }

        // A test for Length ()
        [Fact]
        public void QuaternionDLengthTest()
        {
            Vector3D v = new Vector3D(1.0d, 2.0d, 3.0d);

            double w = 4.0d;

            QuaternionD target = new QuaternionD(v, w);

            double expected = 5.477226d;
            double actual;

            actual = target.Length();

            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.Length did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for LengthSquared ()
        [Fact]
        public void QuaternionDLengthSquaredTest()
        {
            Vector3D v = new Vector3D(1.0d, 2.0d, 3.0d);
            double w = 4.0d;

            QuaternionD target = new QuaternionD(v, w);

            double expected = 30.0d;
            double actual;

            actual = target.LengthSquared();

            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.LengthSquared did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for Lerp (QuaternionD, QuaternionD, double)
        [Fact]
        public void QuaternionDLerpTest()
        {
            Vector3D axis = Vector3D.Normalize(new Vector3D(1.0d, 2.0d, 3.0d));
            QuaternionD a = QuaternionD.CreateFromAxisAngle(axis, MathHelper.ToRadians(10.0d));
            QuaternionD b = QuaternionD.CreateFromAxisAngle(axis, MathHelper.ToRadians(30.0d));

            double t = 0.5d;

            QuaternionD expected = QuaternionD.CreateFromAxisAngle(axis, MathHelper.ToRadians(20.0d));
            QuaternionD actual;

            actual = QuaternionD.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.Lerp did not return the expected value: expected {expected} actual {actual}");

            // Case a and b are same.
            expected = a;
            actual = QuaternionD.Lerp(a, a, t);
            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.Lerp did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for Lerp (QuaternionD, QuaternionD, double)
        // Lerp test when t = 0
        [Fact]
        public void QuaternionDLerpTest1()
        {
            Vector3D axis = Vector3D.Normalize(new Vector3D(1.0d, 2.0d, 3.0d));
            QuaternionD a = QuaternionD.CreateFromAxisAngle(axis, MathHelper.ToRadians(10.0d));
            QuaternionD b = QuaternionD.CreateFromAxisAngle(axis, MathHelper.ToRadians(30.0d));

            double t = 0.0d;

            QuaternionD expected = new QuaternionD(a.X, a.Y, a.Z, a.W);
            QuaternionD actual = QuaternionD.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.Lerp did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for Lerp (QuaternionD, QuaternionD, double)
        // Lerp test when t = 1
        [Fact]
        public void QuaternionDLerpTest2()
        {
            Vector3D axis = Vector3D.Normalize(new Vector3D(1.0d, 2.0d, 3.0d));
            QuaternionD a = QuaternionD.CreateFromAxisAngle(axis, MathHelper.ToRadians(10.0d));
            QuaternionD b = QuaternionD.CreateFromAxisAngle(axis, MathHelper.ToRadians(30.0d));

            double t = 1.0d;

            QuaternionD expected = new QuaternionD(b.X, b.Y, b.Z, b.W);
            QuaternionD actual = QuaternionD.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.Lerp did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for Lerp (QuaternionD, QuaternionD, double)
        // Lerp test when the two quaternions are more than 90 degree (dot product <0)
        [Fact]
        public void QuaternionDLerpTest3()
        {
            Vector3D axis = Vector3D.Normalize(new Vector3D(1.0d, 2.0d, 3.0d));
            QuaternionD a = QuaternionD.CreateFromAxisAngle(axis, MathHelper.ToRadians(10.0d));
            QuaternionD b = QuaternionD.Negate(a);

            double t = 1.0d;

            QuaternionD actual = QuaternionD.Lerp(a, b, t);
            // Note that in quaternion world, Q == -Q. In the case of quaternions dot product is zero,
            // one of the quaternion will be flipped to compute the shortest distance. When t = 1, we
            // expect the result to be the same as quaternion b but flipped.
            Assert.True(actual == a, $"QuaternionD.Lerp did not return the expected value: expected {a} actual {actual}");
        }

        // A test for Conjugate(QuaternionD)
        [Fact]
        public void QuaternionDConjugateTest1()
        {
            QuaternionD a = new QuaternionD(1, 2, 3, 4);

            QuaternionD expected = new QuaternionD(-1, -2, -3, 4);
            QuaternionD actual;

            actual = QuaternionD.Conjugate(a);
            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.Conjugate did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for Normalize (QuaternionD)
        [Fact]
        public void QuaternionDNormalizeTest()
        {
            QuaternionD a = new QuaternionD(1.0d, 2.0d, 3.0d, 4.0d);

            QuaternionD expected = new QuaternionD(0.182574168d, 0.365148336d, 0.5477225d, 0.7302967d);
            QuaternionD actual;

            actual = QuaternionD.Normalize(a);
            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.Normalize did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for Normalize (QuaternionD)
        // Normalize zero length quaternion
        [Fact]
        public void QuaternionDNormalizeTest1()
        {
            QuaternionD a = new QuaternionD(0.0d, 0.0d, -0.0d, 0.0d);

            QuaternionD actual = QuaternionD.Normalize(a);
            Assert.True(double.IsNaN(actual.X) && double.IsNaN(actual.Y) && double.IsNaN(actual.Z) && double.IsNaN(actual.W)
                , $"QuaternionD.Normalize did not return the expected value: expected {new QuaternionD(double.NaN, double.NaN, double.NaN, double.NaN)} actual {actual}");
        }

        // A test for Concatenate(QuaternionD, QuaternionD)
        [Fact]
        public void QuaternionDConcatenateTest1()
        {
            QuaternionD b = new QuaternionD(1.0d, 2.0d, 3.0d, 4.0d);
            QuaternionD a = new QuaternionD(5.0d, 6.0d, 7.0d, 8.0d);

            QuaternionD expected = new QuaternionD(24.0d, 48.0d, 48.0d, -6.0d);
            QuaternionD actual;

            actual = QuaternionD.Concatenate(a, b);
            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.Concatenate did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for operator - (QuaternionD, QuaternionD)
        [Fact]
        public void QuaternionDSubtractionTest()
        {
            QuaternionD a = new QuaternionD(1.0d, 6.0d, 7.0d, 4.0d);
            QuaternionD b = new QuaternionD(5.0d, 2.0d, 3.0d, 8.0d);

            QuaternionD expected = new QuaternionD(-4.0d, 4.0d, 4.0d, -4.0d);
            QuaternionD actual;

            actual = a - b;

            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.operator - did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for operator * (QuaternionD, double)
        [Fact]
        public void QuaternionDMultiplyTest()
        {
            QuaternionD a = new QuaternionD(1.0d, 2.0d, 3.0d, 4.0d);
            double factor = 0.5d;

            QuaternionD expected = new QuaternionD(0.5d, 1.0d, 1.5d, 2.0d);
            QuaternionD actual;

            actual = a * factor;

            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.operator * did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for operator * (QuaternionD, QuaternionD)
        [Fact]
        public void QuaternionDMultiplyTest1()
        {
            QuaternionD a = new QuaternionD(1.0d, 2.0d, 3.0d, 4.0d);
            QuaternionD b = new QuaternionD(5.0d, 6.0d, 7.0d, 8.0d);

            QuaternionD expected = new QuaternionD(24.0d, 48.0d, 48.0d, -6.0d);
            QuaternionD actual;

            actual = a * b;

            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.operator * did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for operator / (QuaternionD, QuaternionD)
        [Fact]
        public void QuaternionDDivisionTest1()
        {
            QuaternionD a = new QuaternionD(1.0d, 2.0d, 3.0d, 4.0d);
            QuaternionD b = new QuaternionD(5.0d, 6.0d, 7.0d, 8.0d);

            QuaternionD expected = new QuaternionD(-0.045977015d, -0.09195402d, -7.450581E-9d, 0.402298868d);
            QuaternionD actual;

            actual = a / b;

            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.operator / did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for operator + (QuaternionD, QuaternionD)
        [Fact]
        public void QuaternionDAdditionTest()
        {
            QuaternionD a = new QuaternionD(1.0d, 2.0d, 3.0d, 4.0d);
            QuaternionD b = new QuaternionD(5.0d, 6.0d, 7.0d, 8.0d);

            QuaternionD expected = new QuaternionD(6.0d, 8.0d, 10.0d, 12.0d);
            QuaternionD actual;

            actual = a + b;

            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.operator + did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for QuaternionD (double, double, double, double)
        [Fact]
        public void QuaternionDConstructorTest()
        {
            double x = 1.0d;
            double y = 2.0d;
            double z = 3.0d;
            double w = 4.0d;

            QuaternionD target = new QuaternionD(x, y, z, w);

            Assert.True(MathHelper.Equal(target.X, x) && MathHelper.Equal(target.Y, y) && MathHelper.Equal(target.Z, z) && MathHelper.Equal(target.W, w),
                "QuaternionD.constructor (x,y,z,w) did not return the expected value.");
        }

        // A test for QuaternionD (Vector3Df, double)
        [Fact]
        public void QuaternionDConstructorTest1()
        {
            Vector3D v = new Vector3D(1.0d, 2.0d, 3.0d);
            double w = 4.0d;

            QuaternionD target = new QuaternionD(v, w);
            Assert.True(MathHelper.Equal(target.X, v.X) && MathHelper.Equal(target.Y, v.Y) && MathHelper.Equal(target.Z, v.Z) && MathHelper.Equal(target.W, w),
                "QuaternionD.constructor (Vector3Df,w) did not return the expected value.");
        }

        // A test for CreateFromAxisAngle (Vector3Df, double)
        [Fact]
        public void QuaternionDCreateFromAxisAngleTest()
        {
            Vector3D axis = Vector3D.Normalize(new Vector3D(1.0d, 2.0d, 3.0d));
            double angle = MathHelper.ToRadians(30.0d);

            QuaternionD expected = new QuaternionD(0.06917231d, 0.13834462d, 0.2075169d, 0.9659258d);
            QuaternionD actual;

            actual = QuaternionD.CreateFromAxisAngle(axis, angle);
            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.CreateFromAxisAngle did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for CreateFromAxisAngle (Vector3Df, double)
        // CreateFromAxisAngle of zero vector
        [Fact]
        public void QuaternionDCreateFromAxisAngleTest1()
        {
            Vector3D axis = new Vector3D();
            double angle = MathHelper.ToRadians(-30.0d);

            double cos = (double)System.Math.Cos(angle / 2.0d);
            QuaternionD actual = QuaternionD.CreateFromAxisAngle(axis, angle);

            Assert.True(actual.X == 0.0d && actual.Y == 0.0d && actual.Z == 0.0d && MathHelper.Equal(cos, actual.W)
                , "QuaternionD.CreateFromAxisAngle did not return the expected value.");
        }

        // A test for CreateFromAxisAngle (Vector3Df, double)
        // CreateFromAxisAngle of angle = 30 && 750
        [Fact]
        public void QuaternionDCreateFromAxisAngleTest2()
        {
            Vector3D axis = new Vector3D(1, 0, 0);
            double angle1 = MathHelper.ToRadians(30.0d);
            double angle2 = MathHelper.ToRadians(750.0d);

            QuaternionD actual1 = QuaternionD.CreateFromAxisAngle(axis, angle1);
            QuaternionD actual2 = QuaternionD.CreateFromAxisAngle(axis, angle2);
            Assert.True(MathHelper.Equal(actual1, actual2), $"QuaternionD.CreateFromAxisAngle did not return the expected value: actual1 {actual1} actual2 {actual2}");
        }

        // A test for CreateFromAxisAngle (Vector3Df, double)
        // CreateFromAxisAngle of angle = 30 && 390
        [Fact]
        public void QuaternionDCreateFromAxisAngleTest3()
        {
            Vector3D axis = new Vector3D(1, 0, 0);
            double angle1 = MathHelper.ToRadians(30.0d);
            double angle2 = MathHelper.ToRadians(390.0d);

            QuaternionD actual1 = QuaternionD.CreateFromAxisAngle(axis, angle1);
            QuaternionD actual2 = QuaternionD.CreateFromAxisAngle(axis, angle2);
            actual1.X = -actual1.X;
            actual1.W = -actual1.W;

            Assert.True(MathHelper.Equal(actual1, actual2), $"QuaternionD.CreateFromAxisAngle did not return the expected value: actual1 {actual1} actual2 {actual2}");
        }

        [Fact]
        public void QuaternionDCreateFromYawPitchRollTest1()
        {
            double yawAngle = MathHelper.ToRadians(30.0d);
            double pitchAngle = MathHelper.ToRadians(40.0d);
            double rollAngle = MathHelper.ToRadians(50.0d);

            QuaternionD yaw = QuaternionD.CreateFromAxisAngle(Vector3D.UnitY, yawAngle);
            QuaternionD pitch = QuaternionD.CreateFromAxisAngle(Vector3D.UnitX, pitchAngle);
            QuaternionD roll = QuaternionD.CreateFromAxisAngle(Vector3D.UnitZ, rollAngle);

            QuaternionD expected = yaw * pitch * roll;
            QuaternionD actual = QuaternionD.CreateFromYawPitchRoll(yawAngle, pitchAngle, rollAngle);
            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.QuaternionDCreateFromYawPitchRollTest1 did not return the expected value: expected {expected} actual {actual}");
        }

        // Covers more numeric rigions
        [Fact]
        public void QuaternionDCreateFromYawPitchRollTest2()
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

                        QuaternionD yaw = QuaternionD.CreateFromAxisAngle(Vector3D.UnitY, yawRad);
                        QuaternionD pitch = QuaternionD.CreateFromAxisAngle(Vector3D.UnitX, pitchRad);
                        QuaternionD roll = QuaternionD.CreateFromAxisAngle(Vector3D.UnitZ, rollRad);

                        QuaternionD expected = yaw * pitch * roll;
                        QuaternionD actual = QuaternionD.CreateFromYawPitchRoll(yawRad, pitchRad, rollRad);
                        Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.QuaternionDCreateFromYawPitchRollTest2 Yaw:{yawAngle} Pitch:{pitchAngle} Roll:{rollAngle} did not return the expected value: expected {expected} actual {actual}");
                    }
                }
            }
        }

        // A test for Slerp (QuaternionD, QuaternionD, double)
        [Fact]
        public void QuaternionDSlerpTest()
        {
            Vector3D axis = Vector3D.Normalize(new Vector3D(1.0d, 2.0d, 3.0d));
            QuaternionD a = QuaternionD.CreateFromAxisAngle(axis, MathHelper.ToRadians(10.0d));
            QuaternionD b = QuaternionD.CreateFromAxisAngle(axis, MathHelper.ToRadians(30.0d));

            double t = 0.5d;

            QuaternionD expected = QuaternionD.CreateFromAxisAngle(axis, MathHelper.ToRadians(20.0d));
            QuaternionD actual;

            actual = QuaternionD.Slerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.Slerp did not return the expected value: expected {expected} actual {actual}");

            // Case a and b are same.
            expected = a;
            actual = QuaternionD.Slerp(a, a, t);
            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.Slerp did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for Slerp (QuaternionD, QuaternionD, double)
        // Slerp test where t = 0
        [Fact]
        public void QuaternionDSlerpTest1()
        {
            Vector3D axis = Vector3D.Normalize(new Vector3D(1.0d, 2.0d, 3.0d));
            QuaternionD a = QuaternionD.CreateFromAxisAngle(axis, MathHelper.ToRadians(10.0d));
            QuaternionD b = QuaternionD.CreateFromAxisAngle(axis, MathHelper.ToRadians(30.0d));

            double t = 0.0d;

            QuaternionD expected = new QuaternionD(a.X, a.Y, a.Z, a.W);
            QuaternionD actual = QuaternionD.Slerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.Slerp did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for Slerp (QuaternionD, QuaternionD, double)
        // Slerp test where t = 1
        [Fact]
        public void QuaternionDSlerpTest2()
        {
            Vector3D axis = Vector3D.Normalize(new Vector3D(1.0d, 2.0d, 3.0d));
            QuaternionD a = QuaternionD.CreateFromAxisAngle(axis, MathHelper.ToRadians(10.0d));
            QuaternionD b = QuaternionD.CreateFromAxisAngle(axis, MathHelper.ToRadians(30.0d));

            double t = 1.0d;

            QuaternionD expected = new QuaternionD(b.X, b.Y, b.Z, b.W);
            QuaternionD actual = QuaternionD.Slerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.Slerp did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for Slerp (QuaternionD, QuaternionD, double)
        // Slerp test where dot product is < 0
        [Fact]
        public void QuaternionDSlerpTest3()
        {
            Vector3D axis = Vector3D.Normalize(new Vector3D(1.0d, 2.0d, 3.0d));
            QuaternionD a = QuaternionD.CreateFromAxisAngle(axis, MathHelper.ToRadians(10.0d));
            QuaternionD b = -a;

            double t = 1.0d;

            QuaternionD expected = a;
            QuaternionD actual = QuaternionD.Slerp(a, b, t);
            // Note that in quaternion world, Q == -Q. In the case of quaternions dot product is zero,
            // one of the quaternion will be flipped to compute the shortest distance. When t = 1, we
            // expect the result to be the same as quaternion b but flipped.
            Assert.True(actual == expected, $"QuaternionD.Slerp did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for Slerp (QuaternionD, QuaternionD, double)
        // Slerp test where the quaternion is flipped
        [Fact]
        public void QuaternionDSlerpTest4()
        {
            Vector3D axis = Vector3D.Normalize(new Vector3D(1.0d, 2.0d, 3.0d));
            QuaternionD a = QuaternionD.CreateFromAxisAngle(axis, MathHelper.ToRadians(10.0d));
            QuaternionD b = -QuaternionD.CreateFromAxisAngle(axis, MathHelper.ToRadians(30.0d));

            double t = 0.0d;

            QuaternionD expected = new QuaternionD(a.X, a.Y, a.Z, a.W);
            QuaternionD actual = QuaternionD.Slerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.Slerp did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for operator - (QuaternionD)
        [Fact]
        public void QuaternionDUnaryNegationTest()
        {
            QuaternionD a = new QuaternionD(1.0d, 2.0d, 3.0d, 4.0d);

            QuaternionD expected = new QuaternionD(-1.0d, -2.0d, -3.0d, -4.0d);
            QuaternionD actual;

            actual = -a;

            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.operator - did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for Inverse (QuaternionD)
        [Fact]
        public void QuaternionDInverseTest()
        {
            QuaternionD a = new QuaternionD(5.0d, 6.0d, 7.0d, 8.0d);

            QuaternionD expected = new QuaternionD(-0.0287356321d, -0.03448276d, -0.0402298868d, 0.04597701d);
            QuaternionD actual;

            actual = QuaternionD.Inverse(a);
            Assert.Equal(expected, actual);
        }

        // A test for Inverse (QuaternionD)
        // Invert zero length quaternion
        [Fact]
        public void QuaternionDInverseTest1()
        {
            QuaternionD a = new QuaternionD();

            QuaternionD expected = QuaternionD.Zero;
            QuaternionD actual = QuaternionD.Inverse(a);

            Assert.Equal(expected, actual);
        }

        // A test for ToString ()
        [Fact]
        public void QuaternionDToStringTest()
        {
            QuaternionD target = new QuaternionD(-1.0d, 2.2d, 3.3d, -4.4d);

            string expected = string.Format(CultureInfo.CurrentCulture
                , "{{X:{0} Y:{1} Z:{2} W:{3}}}"
                , -1.0d, 2.2d, 3.3d, -4.4d);

            string actual = target.ToString();
            Assert.Equal(expected, actual);
        }

        // A test for Add (QuaternionD, QuaternionD)
        [Fact]
        public void QuaternionDAddTest()
        {
            QuaternionD a = new QuaternionD(1.0d, 2.0d, 3.0d, 4.0d);
            QuaternionD b = new QuaternionD(5.0d, 6.0d, 7.0d, 8.0d);

            QuaternionD expected = new QuaternionD(6.0d, 8.0d, 10.0d, 12.0d);
            QuaternionD actual;

            actual = QuaternionD.Add(a, b);
            Assert.Equal(expected, actual);
        }

        // A test for Divide (QuaternionD, QuaternionD)
        [Fact]
        public void QuaternionDDivideTest()
        {
            QuaternionD a = new QuaternionD(1.0d, 2.0d, 3.0d, 4.0d);
            QuaternionD b = new QuaternionD(5.0d, 6.0d, 7.0d, 8.0d);

            QuaternionD expected = new QuaternionD(-0.045977015d, -0.09195402d, -7.450581E-9d, 0.402298868d);
            QuaternionD actual;

            actual = QuaternionD.Divide(a, b);
            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.Divide did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for Equals (object)
        [Fact]
        public void QuaternionDEqualsTest()
        {
            QuaternionD a = new QuaternionD(1.0d, 2.0d, 3.0d, 4.0d);
            QuaternionD b = new QuaternionD(1.0d, 2.0d, 3.0d, 4.0d);

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
        public void QuaternionDGetHashCodeTest()
        {
            QuaternionD a = new QuaternionD(1.0d, 2.0d, 3.0d, 4.0d);

            int expected = HashCode.Combine(a.X, a.Y, a.Z, a.W);
            int actual = a.GetHashCode();
            Assert.Equal(expected, actual);
        }

        // A test for Multiply (QuaternionD, double)
        [Fact]
        public void QuaternionDMultiplyTest2()
        {
            QuaternionD a = new QuaternionD(1.0d, 2.0d, 3.0d, 4.0d);
            double factor = 0.5d;

            QuaternionD expected = new QuaternionD(0.5d, 1.0d, 1.5d, 2.0d);
            QuaternionD actual;

            actual = QuaternionD.Multiply(a, factor);
            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.Multiply did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for Multiply (QuaternionD, QuaternionD)
        [Fact]
        public void QuaternionDMultiplyTest3()
        {
            QuaternionD a = new QuaternionD(1.0d, 2.0d, 3.0d, 4.0d);
            QuaternionD b = new QuaternionD(5.0d, 6.0d, 7.0d, 8.0d);

            QuaternionD expected = new QuaternionD(24.0d, 48.0d, 48.0d, -6.0d);
            QuaternionD actual;

            actual = QuaternionD.Multiply(a, b);
            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.Multiply did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for Negate (QuaternionD)
        [Fact]
        public void QuaternionDNegateTest()
        {
            QuaternionD a = new QuaternionD(1.0d, 2.0d, 3.0d, 4.0d);

            QuaternionD expected = new QuaternionD(-1.0d, -2.0d, -3.0d, -4.0d);
            QuaternionD actual;

            actual = QuaternionD.Negate(a);
            Assert.Equal(expected, actual);
        }

        // A test for Subtract (QuaternionD, QuaternionD)
        [Fact]
        public void QuaternionDSubtractTest()
        {
            QuaternionD a = new QuaternionD(1.0d, 6.0d, 7.0d, 4.0d);
            QuaternionD b = new QuaternionD(5.0d, 2.0d, 3.0d, 8.0d);

            QuaternionD expected = new QuaternionD(-4.0d, 4.0d, 4.0d, -4.0d);
            QuaternionD actual;

            actual = QuaternionD.Subtract(a, b);
            Assert.Equal(expected, actual);
        }

        // A test for operator != (QuaternionD, QuaternionD)
        [Fact]
        public void QuaternionDInequalityTest()
        {
            QuaternionD a = new QuaternionD(1.0d, 2.0d, 3.0d, 4.0d);
            QuaternionD b = new QuaternionD(1.0d, 2.0d, 3.0d, 4.0d);

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

        // A test for operator == (QuaternionD, QuaternionD)
        [Fact]
        public void QuaternionDEqualityTest()
        {
            QuaternionD a = new QuaternionD(1.0d, 2.0d, 3.0d, 4.0d);
            QuaternionD b = new QuaternionD(1.0d, 2.0d, 3.0d, 4.0d);

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

        // A test for CreateFromRotationMatrix (Matrix4x4D)
        // Convert Identity matrix test
        [Fact]
        public void QuaternionDFromRotationMatrixTest1()
        {
            Matrix4x4D matrix = Matrix4x4D.Identity;

            QuaternionD expected = new QuaternionD(0.0d, 0.0d, 0.0d, 1.0d);
            QuaternionD actual = QuaternionD.CreateFromRotationMatrix(matrix);
            Assert.True(MathHelper.Equal(expected, actual),
                $"QuaternionD.CreateFromRotationMatrix did not return the expected value: expected {expected} actual {actual}");

            // make sure convert back to matrix is same as we passed matrix.
            Matrix4x4D m2 = Matrix4x4D.CreateFromQuaternion(actual);
            Assert.True(MathHelper.Equal(matrix, m2),
                $"QuaternionD.CreateFromQuaternion did not return the expected value: matrix {matrix} m2 {m2}");
        }

        // A test for CreateFromRotationMatrix (Matrix4x4D)
        // Convert X axis rotation matrix
        [Fact]
        public void QuaternionDFromRotationMatrixTest2()
        {
            for (double angle = 0.0d; angle < 720.0d; angle += 10.0d)
            {
                Matrix4x4D matrix = Matrix4x4D.CreateRotationX(angle);

                QuaternionD expected = QuaternionD.CreateFromAxisAngle(Vector3D.UnitX, angle);
                QuaternionD actual = QuaternionD.CreateFromRotationMatrix(matrix);
                Assert.True(MathHelper.EqualRotation(expected, actual),
                    $"QuaternionD.CreateFromRotationMatrix angle:{angle} did not return the expected value: expected {expected} actual {actual}");

                // make sure convert back to matrix is same as we passed matrix.
                Matrix4x4D m2 = Matrix4x4D.CreateFromQuaternion(actual);
                Assert.True(MathHelper.Equal(matrix, m2),
                    $"QuaternionD.CreateFromQuaternion angle:{angle} did not return the expected value: matrix {matrix} m2 {m2}");
            }
        }

        // A test for CreateFromRotationMatrix (Matrix4x4D)
        // Convert Y axis rotation matrix
        [Fact]
        public void QuaternionDFromRotationMatrixTest3()
        {
            for (double angle = 0.0d; angle < 720.0d; angle += 10.0d)
            {
                Matrix4x4D matrix = Matrix4x4D.CreateRotationY(angle);

                QuaternionD expected = QuaternionD.CreateFromAxisAngle(Vector3D.UnitY, angle);
                QuaternionD actual = QuaternionD.CreateFromRotationMatrix(matrix);
                Assert.True(MathHelper.EqualRotation(expected, actual),
                    $"QuaternionD.CreateFromRotationMatrix angle:{angle} did not return the expected value: expected {expected} actual {actual}");

                // make sure convert back to matrix is same as we passed matrix.
                Matrix4x4D m2 = Matrix4x4D.CreateFromQuaternion(actual);
                Assert.True(MathHelper.Equal(matrix, m2),
                    $"QuaternionD.CreateFromQuaternion angle:{angle} did not return the expected value: matrix {matrix} m2 {m2}");
            }
        }

        // A test for CreateFromRotationMatrix (Matrix4x4D)
        // Convert Z axis rotation matrix
        [Fact]
        public void QuaternionDFromRotationMatrixTest4()
        {
            for (double angle = 0.0d; angle < 720.0d; angle += 10.0d)
            {
                Matrix4x4D matrix = Matrix4x4D.CreateRotationZ(angle);

                QuaternionD expected = QuaternionD.CreateFromAxisAngle(Vector3D.UnitZ, angle);
                QuaternionD actual = QuaternionD.CreateFromRotationMatrix(matrix);
                Assert.True(MathHelper.EqualRotation(expected, actual),
                    $"QuaternionD.CreateFromRotationMatrix angle:{angle} did not return the expected value: expected {expected} actual {actual}");

                // make sure convert back to matrix is same as we passed matrix.
                Matrix4x4D m2 = Matrix4x4D.CreateFromQuaternion(actual);
                Assert.True(MathHelper.Equal(matrix, m2),
                    $"QuaternionD.CreateFromQuaternion angle:{angle} did not return the expected value: matrix {matrix} m2 {m2}");
            }
        }

        // A test for CreateFromRotationMatrix (Matrix4x4D)
        // Convert XYZ axis rotation matrix
        [Fact]
        public void QuaternionDFromRotationMatrixTest5()
        {
            for (double angle = 0.0d; angle < 720.0d; angle += 10.0d)
            {
                Matrix4x4D matrix = Matrix4x4D.CreateRotationX(angle) * Matrix4x4D.CreateRotationY(angle) * Matrix4x4D.CreateRotationZ(angle);

                QuaternionD expected =
                    QuaternionD.CreateFromAxisAngle(Vector3D.UnitZ, angle) *
                    QuaternionD.CreateFromAxisAngle(Vector3D.UnitY, angle) *
                    QuaternionD.CreateFromAxisAngle(Vector3D.UnitX, angle);

                QuaternionD actual = QuaternionD.CreateFromRotationMatrix(matrix);
                Assert.True(MathHelper.EqualRotation(expected, actual),
                    $"QuaternionD.CreateFromRotationMatrix angle:{angle} did not return the expected value: expected {expected} actual {actual}");

                // make sure convert back to matrix is same as we passed matrix.
                Matrix4x4D m2 = Matrix4x4D.CreateFromQuaternion(actual);
                Assert.True(MathHelper.Equal(matrix, m2),
                    $"QuaternionD.CreateFromQuaternion angle:{angle} did not return the expected value: matrix {matrix} m2 {m2}");
            }
        }

        // A test for CreateFromRotationMatrix (Matrix4x4D)
        // X axis is most large axis case
        [Fact]
        public void QuaternionDFromRotationMatrixWithScaledMatrixTest1()
        {
            double angle = MathHelper.ToRadians(180.0d);
            Matrix4x4D matrix = Matrix4x4D.CreateRotationY(angle) * Matrix4x4D.CreateRotationZ(angle);

            QuaternionD expected = QuaternionD.CreateFromAxisAngle(Vector3D.UnitZ, angle) * QuaternionD.CreateFromAxisAngle(Vector3D.UnitY, angle);
            QuaternionD actual = QuaternionD.CreateFromRotationMatrix(matrix);
            Assert.True(MathHelper.EqualRotation(expected, actual),
                $"QuaternionD.CreateFromRotationMatrix did not return the expected value: expected {expected} actual {actual}");

            // make sure convert back to matrix is same as we passed matrix.
            Matrix4x4D m2 = Matrix4x4D.CreateFromQuaternion(actual);
            Assert.True(MathHelper.Equal(matrix, m2),
                $"QuaternionD.CreateFromQuaternion did not return the expected value: matrix {matrix} m2 {m2}");
        }

        // A test for CreateFromRotationMatrix (Matrix4x4D)
        // Y axis is most large axis case
        [Fact]
        public void QuaternionDFromRotationMatrixWithScaledMatrixTest2()
        {
            double angle = MathHelper.ToRadians(180.0d);
            Matrix4x4D matrix = Matrix4x4D.CreateRotationX(angle) * Matrix4x4D.CreateRotationZ(angle);

            QuaternionD expected = QuaternionD.CreateFromAxisAngle(Vector3D.UnitZ, angle) * QuaternionD.CreateFromAxisAngle(Vector3D.UnitX, angle);
            QuaternionD actual = QuaternionD.CreateFromRotationMatrix(matrix);
            Assert.True(MathHelper.EqualRotation(expected, actual),
                $"QuaternionD.CreateFromRotationMatrix did not return the expected value: expected {expected} actual {actual}");

            // make sure convert back to matrix is same as we passed matrix.
            Matrix4x4D m2 = Matrix4x4D.CreateFromQuaternion(actual);
            Assert.True(MathHelper.Equal(matrix, m2),
                $"QuaternionD.CreateFromQuaternion did not return the expected value: matrix {matrix} m2 {m2}");
        }

        // A test for CreateFromRotationMatrix (Matrix4x4D)
        // Z axis is most large axis case
        [Fact]
        public void QuaternionDFromRotationMatrixWithScaledMatrixTest3()
        {
            double angle = MathHelper.ToRadians(180.0d);
            Matrix4x4D matrix = Matrix4x4D.CreateRotationX(angle) * Matrix4x4D.CreateRotationY(angle);

            QuaternionD expected = QuaternionD.CreateFromAxisAngle(Vector3D.UnitY, angle) * QuaternionD.CreateFromAxisAngle(Vector3D.UnitX, angle);
            QuaternionD actual = QuaternionD.CreateFromRotationMatrix(matrix);
            Assert.True(MathHelper.EqualRotation(expected, actual),
                $"QuaternionD.CreateFromRotationMatrix did not return the expected value: expected {expected} actual {actual}");

            // make sure convert back to matrix is same as we passed matrix.
            Matrix4x4D m2 = Matrix4x4D.CreateFromQuaternion(actual);
            Assert.True(MathHelper.Equal(matrix, m2),
                $"QuaternionD.CreateFromQuaternion did not return the expected value: matrix {matrix} m2 {m2}");
        }

        // A test for Equals (QuaternionD)
        [Fact]
        public void QuaternionDEqualsTest1()
        {
            QuaternionD a = new QuaternionD(1.0d, 2.0d, 3.0d, 4.0d);
            QuaternionD b = new QuaternionD(1.0d, 2.0d, 3.0d, 4.0d);

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

        // A test for Zero
        [Fact]
        public void QuaternionDZeroTest()
        {
            // A default value should be equal to a zero value.
            Assert.Equal(default(QuaternionD), QuaternionD.Zero);

            // A newly constructed value should be equal to a zero value.
            Assert.Equal(new QuaternionD(), QuaternionD.Zero);

            // A newly constructed value with (0, 0, 0, 0) should be equal to a zero value.
            Assert.Equal(new QuaternionD(0, 0, 0, 0), QuaternionD.Zero);
        }

        // A test for Identity
        [Fact]
        public void QuaternionDIdentityTest()
        {
            QuaternionD val = new QuaternionD(0, 0, 0, 1);
            Assert.Equal(val, QuaternionD.Identity);
        }

        // A test for IsIdentity
        [Fact]
        public void QuaternionDIsIdentityTest()
        {
            Assert.True(QuaternionD.Identity.IsIdentity);
            Assert.True(new QuaternionD(0, 0, 0, 1).IsIdentity);
            Assert.False(new QuaternionD(1, 0, 0, 1).IsIdentity);
            Assert.False(new QuaternionD(0, 1, 0, 1).IsIdentity);
            Assert.False(new QuaternionD(0, 0, 1, 1).IsIdentity);
            Assert.False(new QuaternionD(0, 0, 0, 0).IsIdentity);
        }

        // A test for QuaternionD comparison involving NaN values
        [Fact]
        public void QuaternionDEqualsNaNTest()
        {
            QuaternionD a = new QuaternionD(double.NaN, 0, 0, 0);
            QuaternionD b = new QuaternionD(0, double.NaN, 0, 0);
            QuaternionD c = new QuaternionD(0, 0, double.NaN, 0);
            QuaternionD d = new QuaternionD(0, 0, 0, double.NaN);

            Assert.False(a == new QuaternionD(0, 0, 0, 0));
            Assert.False(b == new QuaternionD(0, 0, 0, 0));
            Assert.False(c == new QuaternionD(0, 0, 0, 0));
            Assert.False(d == new QuaternionD(0, 0, 0, 0));

            Assert.True(a != new QuaternionD(0, 0, 0, 0));
            Assert.True(b != new QuaternionD(0, 0, 0, 0));
            Assert.True(c != new QuaternionD(0, 0, 0, 0));
            Assert.True(d != new QuaternionD(0, 0, 0, 0));

            Assert.False(a.Equals(new QuaternionD(0, 0, 0, 0)));
            Assert.False(b.Equals(new QuaternionD(0, 0, 0, 0)));
            Assert.False(c.Equals(new QuaternionD(0, 0, 0, 0)));
            Assert.False(d.Equals(new QuaternionD(0, 0, 0, 0)));

            Assert.False(a.IsIdentity);
            Assert.False(b.IsIdentity);
            Assert.False(c.IsIdentity);
            Assert.False(d.IsIdentity);

            Assert.True(a.Equals(a));
            Assert.True(b.Equals(b));
            Assert.True(c.Equals(c));
            Assert.True(d.Equals(d));
        }

        // A test to make sure these types are blittable directly into GPU buffer memory layouts
        [Fact]
        public unsafe void QuaternionDSizeofTest()
        {
            Assert.Equal(32, sizeof(QuaternionD));
            Assert.Equal(64, sizeof(QuaternionD_2x));
            Assert.Equal(40, sizeof(QuaternionDPlusDouble));
            Assert.Equal(80, sizeof(QuaternionDPlusDouble_2x));
        }

        [StructLayout(LayoutKind.Sequential)]
        struct QuaternionD_2x
        {
            private QuaternionD _a;
            private QuaternionD _b;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct QuaternionDPlusDouble
        {
            private QuaternionD _v;
            private double _f;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct QuaternionDPlusDouble_2x
        {
            private QuaternionDPlusDouble _a;
            private QuaternionDPlusDouble _b;
        }

        // A test to make sure the fields are laid out how we expect
        [Fact]
        public unsafe void QuaternionDFieldOffsetTest()
        {
            QuaternionD quat = new QuaternionD();

            double* basePtr = &quat.X; // Take address of first element
            QuaternionD* quatPtr = &quat; // Take address of whole QuaternionD

            Assert.Equal(new IntPtr(basePtr), new IntPtr(quatPtr));

            Assert.Equal(new IntPtr(basePtr + 0), new IntPtr(&quat.X));
            Assert.Equal(new IntPtr(basePtr + 1), new IntPtr(&quat.Y));
            Assert.Equal(new IntPtr(basePtr + 2), new IntPtr(&quat.Z));
            Assert.Equal(new IntPtr(basePtr + 3), new IntPtr(&quat.W));
        }
    }
}
